﻿using DataModelLib.DataModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace DataModelLib
{

	
	[Generator]
	public class DataModelGenerator : IIncrementalGenerator
	{
		
		private const string Namespace = "DataModelGenerator";

		private const string TableChangedEventHandlerSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
			public enum TableChangedActions {Add,Remove};
			public delegate void TableChangedEventHandler<T>(T Item,TableChangedActions Action, int Index);
			public delegate void RowChangedEventHandler<T>(T Item,string PropertyName);
		}
		""";

		private const string DatabaseAttributeSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
		  
			[AttributeUsage(AttributeTargets.Class, Inherited = false)]
			public class DatabaseAttribute : Attribute
			{
			}
		}
		""";
		private const string TableAttributeSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
		  
			[AttributeUsage(AttributeTargets.Class, Inherited = false)]
			public class TableAttribute : Attribute
			{
			}
		}
		""";

		private const string ColumnAttributeSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
		  
			[AttributeUsage(AttributeTargets.Property, Inherited = false)]
			public class ColumnAttribute : Attribute
			{
			}
		}
		""";

		private const string PrimaryKeyAttributeSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
		  
			[AttributeUsage(AttributeTargets.Property, Inherited = false)]
			public class PrimaryKeyAttribute : Attribute
			{
				
			}
		}
		""";

		private const string ForeignKeyAttributeSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
			public enum CascadeTriggers {None,Delete,Update};
		  
			[AttributeUsage(AttributeTargets.Property, Inherited = false)]
			public class ForeignKeyAttribute : Attribute
			{
				public string ForeignPropertyName
				{
					get;
					private set;
				}
				public string PrimaryPropertyName
				{
					get;
					private set;
				}
				public string PrimaryTable
				{
					get;
					private set;
				}
				public string PrimaryKey
				{
					get;
					private set;
				}
				public CascadeTriggers CascadeTrigger
				{
					get;
					private set;
				}


				public ForeignKeyAttribute(string ForeignPropertyName, string PrimaryPropertyName,string PrimaryTable,string PrimaryKey,CascadeTriggers CascadeTrigger)
				{
					this.ForeignPropertyName=ForeignPropertyName;this.PrimaryPropertyName=PrimaryPropertyName; this.PrimaryTable=PrimaryTable; this.PrimaryKey=PrimaryKey;this.CascadeTrigger=CascadeTrigger;
				}
			}
		}
		""";


		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Delegates/TableChangedDelegate.g.cs", SourceText.From(TableChangedEventHandlerSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/DatabaseAttribute.g.cs", SourceText.From(DatabaseAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/TableAttribute.g.cs", SourceText.From(TableAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/ColumnAttribute.g.cs", SourceText.From(ColumnAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/PrimaryKeyAttribute.g.cs", SourceText.From(PrimaryKeyAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/ForeignKeyAttribute.g.cs", SourceText.From(ForeignKeyAttributeSourceCode, Encoding.UTF8)));

			IncrementalValuesProvider<(SyntaxNode,DataModelType)> syntaxNodeProvider = context.SyntaxProvider.CreateSyntaxProvider
			(
				(syntaxNode,cancellationToken) => (syntaxNode is ClassDeclarationSyntax classDeclatationSyntax) && classDeclatationSyntax.AttributeLists.Count > 0,
				transform: static (ctx, _) => GetDeclarationForSourceGen(ctx)
			)
			.Where(classDeclarationSyntax=>classDeclarationSyntax.DataModelType!=DataModelType.Undefined)
			.Select((t, _) => t);


			context.RegisterSourceOutput
			(
				context.CompilationProvider.Combine(syntaxNodeProvider.Collect()),
				(ctx, t) => GenerateCode(ctx, t.Left,t.Right)
			);

			
		}
	
		private static (SyntaxNode Node, DataModelType DataModelType) GetDeclarationForSourceGen(GeneratorSyntaxContext context)
		{
			SyntaxNode currentNode = context.Node;

			if (currentNode.ContainsAttribute(context.SemanticModel, $"{Namespace}.TableAttribute")) return (currentNode, DataModelType.Table);
			if (currentNode.ContainsAttribute(context.SemanticModel, $"{Namespace}.DatabaseAttribute")) return (currentNode, DataModelType.Database);

			return (currentNode, DataModelType.Undefined);
		}

		private static DatabaseModel? CreateDatabaseModel(SourceProductionContext context, Compilation compilation, TypeDeclarationSyntax? DatabaseDeclarationSyntax)
		{
			INamedTypeSymbol? databaseSymbol;
			string nameSpace;
			string databaseClassName;

			// no database defined, cannot proceed
			if (DatabaseDeclarationSyntax == null) return null;

			// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
			databaseSymbol = DatabaseDeclarationSyntax.GetTypeSymbol<INamedTypeSymbol>(compilation);
			if (databaseSymbol == null) return null;

			// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
			nameSpace = databaseSymbol.ContainingNamespace.ToDisplayString();
			databaseClassName = DatabaseDeclarationSyntax.Identifier.Text;

			return new DatabaseModel(nameSpace, databaseClassName);
		}
		private static void CreateTableModels(SourceProductionContext context, Compilation compilation, DatabaseModel DatabaseModel, IEnumerable<SyntaxNode> TableDeclarationSyntaxList)
		{
			string columnName;
			string columnType;
			string tableName;
			string nameSpace;
			TableModel tableModel;
			INamedTypeSymbol? tableSymbol;
			IPropertySymbol? propertySymbol;
			bool isNullable;
			ColumnModel columnModel;
			//AttributeData? tableAttributeData;

			// on enumère une première fois les tables et les colonnes pour les ajouter à la collection
			foreach (TypeDeclarationSyntax tableDeclarationSyntax in TableDeclarationSyntaxList)
			{
				// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
				tableSymbol = tableDeclarationSyntax.GetTypeSymbol<INamedTypeSymbol>(compilation);
				if (tableSymbol == null) continue;

				// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
				nameSpace = tableSymbol.ContainingNamespace.ToDisplayString();
				tableName = tableDeclarationSyntax.Identifier.Text;

				//tableAttributeData = tableSymbol.GetAttribute($"{Namespace}.TableAttribute");
				//if ((tableAttributeData == null) || (tableAttributeData.ConstructorArguments.Length==0)) tableName = $"{tableClassName}s";
				//else tableName = tableAttributeData.ConstructorArguments[0].Value?.ToString() ?? $"{tableClassName}s";

				tableModel = new TableModel(nameSpace, DatabaseModel.DatabaseName,  tableName);

				// on recherche les colonnes pour les ajouter à la table
				foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in tableDeclarationSyntax.ChildNodes().OfType<PropertyDeclarationSyntax>())
				{
					propertySymbol = propertyDeclarationSyntax.GetTypeSymbol<IPropertySymbol>(compilation);
					if (propertySymbol == null) continue;

					if (!propertyDeclarationSyntax.ContainsAttribute(compilation, $"{Namespace}.ColumnAttribute")) continue;

					columnName = propertyDeclarationSyntax.Identifier.Text;
					columnType = propertyDeclarationSyntax.Type.ToString();
					isNullable = propertyDeclarationSyntax.Type is NullableTypeSyntax;

					columnModel = new ColumnModel(tableModel, columnName, columnType, isNullable);
					tableModel.ColumnModels.Add(columnModel);

					if (propertyDeclarationSyntax.ContainsAttribute(compilation, $"{Namespace}.PrimaryKeyAttribute")) tableModel.PrimaryKey=columnModel;

				}
				DatabaseModel.TableModels.Add(tableModel);

			}
		}
		private static void CreateRelationModels(SourceProductionContext context, Compilation compilation, DatabaseModel DatabaseModel, IEnumerable<SyntaxNode> TableDeclarationSyntaxList)
		{
			string foreignColumnName;
			string foreignTableName;
			string nameSpace;
			string? primaryPropertyName,foreignPropertyName, primaryTableName, primaryColumnName,cascadeActionName;

			CascadeTriggers cascadeTrigger;

			TableModel? foreignTableModel,primaryTableModel;
			ColumnModel? foreignColumnModel,primaryColumnModel;

			INamedTypeSymbol? tableSymbol;
			IPropertySymbol? propertySymbol;
			//bool isNullable;
			RelationModel relationModel;
			//AttributeData? tableAttributeData;
			AttributeData? relationAttributeData;
			

			// on enumère une première fois les tables et les colonnes pour les ajouter à la collection
			foreach (TypeDeclarationSyntax tableDeclarationSyntax in TableDeclarationSyntaxList)
			{
				// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
				tableSymbol = tableDeclarationSyntax.GetTypeSymbol<INamedTypeSymbol>(compilation);
				if (tableSymbol == null) continue;

				// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
				nameSpace = tableSymbol.ContainingNamespace.ToDisplayString();
				foreignTableName = tableDeclarationSyntax.Identifier.Text;
				//tableAttributeData = tableSymbol.GetAttribute($"{Namespace}.TableAttribute");
				//if ((tableAttributeData == null) || (tableAttributeData.ConstructorArguments.Length == 0)) foreignTableName = $"{foreignTableClassName}s";
				//else foreignTableName = tableAttributeData.ConstructorArguments[0].Value?.ToString() ?? $"{foreignTableClassName}s";

				foreignTableModel = DatabaseModel.TableModels.FirstOrDefault(item => item.TableName == foreignTableName);
				if (foreignTableModel == null) continue;	

				// on recherche les relations pour les ajouter à la table
				foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in tableDeclarationSyntax.ChildNodes().OfType<PropertyDeclarationSyntax>())
				{
					propertySymbol = propertyDeclarationSyntax.GetTypeSymbol<IPropertySymbol>(compilation);
					if (propertySymbol == null) continue;
					foreignColumnName = propertyDeclarationSyntax.Identifier.Text;

					foreignColumnModel = foreignTableModel.ColumnModels.FirstOrDefault(item => item.ColumnName == foreignColumnName);
					if (foreignColumnModel == null) continue;

					relationAttributeData = propertySymbol.GetAttribute($"{Namespace}.ForeignKeyAttribute");
					if ((relationAttributeData == null) || (relationAttributeData.ConstructorArguments.Length<5)) continue;

					foreignPropertyName = relationAttributeData.ConstructorArguments[0].Value?.ToString();
					if (foreignPropertyName == null) continue;

					primaryPropertyName = relationAttributeData.ConstructorArguments[1].Value?.ToString();
					if (primaryPropertyName == null) continue;

					primaryTableName = relationAttributeData.ConstructorArguments[2].Value?.ToString();
					if (primaryTableName == null) continue;

					primaryColumnName = relationAttributeData.ConstructorArguments[3].Value?.ToString();
					if (primaryColumnName == null) continue;
					
					cascadeActionName = relationAttributeData.ConstructorArguments[4].Value?.ToString();
					if (cascadeActionName == null) continue;
					if (!Enum.TryParse<CascadeTriggers>(cascadeActionName, out cascadeTrigger)) continue;


					primaryTableModel = DatabaseModel.TableModels.FirstOrDefault(item => item.TableName == primaryTableName);
					if (primaryTableModel == null) continue;

					primaryColumnModel = primaryTableModel.ColumnModels.FirstOrDefault(item => item.ColumnName == primaryColumnName);
					if (primaryColumnModel == null) continue;

				
					relationModel = new RelationModel(primaryPropertyName, primaryTableModel, primaryColumnModel, foreignPropertyName, foreignTableModel, foreignColumnModel, cascadeTrigger);
					foreignTableModel.Relations.Add(relationModel);
					primaryTableModel.Relations.Add(relationModel);

					//columnType = propertyDeclarationSyntax.Type.ToString();
					//isNullable = propertyDeclarationSyntax.Type is NullableTypeSyntax;

					
				}

			}
		}

		private static void GenerateCode(SourceProductionContext context, Compilation compilation,  IEnumerable<(SyntaxNode,DataModelType)> Declarations)
		{
			DatabaseModel? databaseModel;
			string source;

			databaseModel = CreateDatabaseModel(context, compilation, Declarations.FirstOrDefault(item => item.Item2 == DataModelType.Database).Item1 as TypeDeclarationSyntax);
			if (databaseModel == null) return;

			CreateTableModels(context, compilation, databaseModel, Declarations.Where(item => item.Item2 == DataModelType.Table).Select(item => item.Item1));
			CreateRelationModels(context, compilation, databaseModel, Declarations.Where(item => item.Item2 == DataModelType.Table).Select(item => item.Item1));
			
			// On ajoute le code source de la database
			source = databaseModel.GenerateDatabaseClass();
			context.AddSource($"{databaseModel.DatabaseName}.g.cs", SourceText.From(source, Encoding.UTF8));

			source = databaseModel.GenerateDatabaseModelClass();
			context.AddSource($"Models/{databaseModel.DatabaseName}Model.g.cs", SourceText.From(source, Encoding.UTF8));

			// On ajoute le code source des tables
			foreach (TableModel tableModel in databaseModel.TableModels)
			{
				source = tableModel.GenerateTableModelClass();
				context.AddSource($"Models/{tableModel.TableName}Model.g.cs", SourceText.From(source, Encoding.UTF8));
			}

		}








	}
}