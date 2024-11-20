﻿using DataModelLib.DataModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
				public string Name
				{
					get;
					private set;
				}
				
				public TableAttribute(string Name)
				{
					this.Name=Name;
				}
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

		private const string ForeignKeyAttributeSourceCode =
		$$"""
		// <auto-generated/>
		using System;
		
		namespace {{Namespace}}
		{
		  
			[AttributeUsage(AttributeTargets.Property, Inherited = false)]
			public class ForeignKeyAttribute : Attribute
			{
				public string PropertyName
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
						
				public ForeignKeyAttribute(string PropertyName,string PrimaryTable,string PrimaryKey)
				{
					this.PropertyName=PropertyName; this.PrimaryTable=PrimaryTable; this.PrimaryKey=PrimaryKey;
				}
			}
		}
		""";


		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/DatabaseAttribute.g.cs", SourceText.From(DatabaseAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/TableAttribute.g.cs", SourceText.From(TableAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/ColumnAttribute.g.cs", SourceText.From(ColumnAttributeSourceCode, Encoding.UTF8)));
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


		private static void GenerateCode(SourceProductionContext context, Compilation compilation,  IEnumerable<(SyntaxNode,DataModelType)> Declarations)
		{
			string nameSpace;
			string databaseClassName,tableClassName,columnName;
			string columnType;
			string tableName;
			DatabaseModel databaseModel;
			TableModel tableModel;
			ColumnModel columnModel;
			string source;
			INamedTypeSymbol? databaseSymbol, tableSymbol;
			bool isNullable;

			TypeDeclarationSyntax? databaseDeclarationSyntax = Declarations.FirstOrDefault(item => item.Item2 == DataModelType.Database).Item1 as TypeDeclarationSyntax;
			// no database defined, cannot proceed
			if (databaseDeclarationSyntax == null) return;

			// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
			databaseSymbol = databaseDeclarationSyntax.GetTypeSymbol<INamedTypeSymbol>(compilation);
			if (databaseSymbol == null) return;

			// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
			nameSpace = databaseSymbol.ContainingNamespace.ToDisplayString();
			databaseClassName = databaseDeclarationSyntax.Identifier.Text;

			databaseModel = new DatabaseModel(nameSpace, databaseClassName);


			foreach (TypeDeclarationSyntax tableDeclarationSyntax in Declarations.Where(item=>item.Item2==DataModelType.Table).Select(item=>item.Item1))
			{
				// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
				tableSymbol = tableDeclarationSyntax.GetTypeSymbol<INamedTypeSymbol>(compilation);
				if (tableSymbol == null) continue;
		
				// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
				nameSpace = tableSymbol.ContainingNamespace.ToDisplayString();
				tableClassName = tableDeclarationSyntax.Identifier.Text;
				if ((tableSymbol.GetAttributes().Length == 0) || (tableSymbol.GetAttributes()[0].ConstructorArguments.Length == 0)) tableName = $"{tableClassName}s";
				else tableName = tableSymbol.GetAttributes()[0].ConstructorArguments[0].Value?.ToString()?? $"{tableClassName}s";

				tableModel = new TableModel(nameSpace, databaseClassName, tableClassName, tableName);

				// on recherche les colonnes
				foreach(PropertyDeclarationSyntax propertyDeclarationSyntax in tableDeclarationSyntax.ChildNodes().OfType<PropertyDeclarationSyntax>() )
				{
					if (!propertyDeclarationSyntax.ContainsAttribute(compilation, $"{Namespace}.ColumnAttribute")) continue;

					columnName = propertyDeclarationSyntax.Identifier.Text;
					columnType = propertyDeclarationSyntax.Type.ToString();
					isNullable = propertyDeclarationSyntax.Type is NullableTypeSyntax;

					columnModel = new ColumnModel(columnName,columnType,isNullable);
					tableModel.ColumnModels.Add(columnModel);
				}

				databaseModel.TableModels.Add(tableModel);

				// On ajoute le code source de la table
				source = tableModel.GenerateTableModelClass();
				context.AddSource($"{tableModel.TableClassName}Model.g.cs", SourceText.From(source, Encoding.UTF8));

			}

			// On ajoute le code source de la database
			source = databaseModel.GenerateDatabaseClass();
			context.AddSource($"{databaseModel.DatabaseClassName}.g.cs", SourceText.From(source, Encoding.UTF8));

			source = databaseModel.GenerateDatabaseModelClass();
			context.AddSource($"{databaseModel.DatabaseClassName}Model.g.cs", SourceText.From(source, Encoding.UTF8));


		}








	}
}