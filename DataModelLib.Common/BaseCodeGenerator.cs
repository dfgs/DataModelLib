using DataModelLib.Common.Schema;
using DataModelLib.Common.SourceGenerator;
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

namespace DataModelLib.Common
{

	
	//[Generator]
	public abstract class BaseCodeGenerator : IIncrementalGenerator
	{
		
		protected const string Namespace = "DataModelLib.Common";

		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			OnRegisterStaticSources(context);

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
				(ctx, t) => GenerateDynamicSources(ctx, t.Left,t.Right)
			);

			
		}

		protected abstract void OnRegisterStaticSources(IncrementalGeneratorInitializationContext context);
	
		private static (SyntaxNode Node, DataModelType DataModelType) GetDeclarationForSourceGen(GeneratorSyntaxContext context)
		{
			SyntaxNode currentNode = context.Node;

			if (currentNode.ContainsAttribute(context.SemanticModel, $"{Namespace}.TableAttribute")) return (currentNode, DataModelType.Table);
			if (currentNode.ContainsAttribute(context.SemanticModel, $"{Namespace}.DatabaseAttribute")) return (currentNode, DataModelType.Database);

			return (currentNode, DataModelType.Undefined);
		}

		private static Database? CreateDatabaseModel(SourceProductionContext context, Compilation compilation, TypeDeclarationSyntax? DatabaseDeclarationSyntax)
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

			return new Database(nameSpace, databaseClassName);
		}
		private static void CreateTableModels(SourceProductionContext context, Compilation compilation, Database DatabaseModel, IEnumerable<SyntaxNode> TableDeclarationSyntaxList)
		{
			string columnName;
			string columnType;
			string displayName;
			string tableName;
			string nameSpace;
			Table tableModel;
			INamedTypeSymbol? tableSymbol;
			IPropertySymbol? propertySymbol;
			bool isNullable;
			Column columnModel;
			AttributeData? columnAttributeData;
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

				tableModel = new Table(nameSpace, DatabaseModel.DatabaseName,  tableName);

				// on recherche les colonnes pour les ajouter à la table
				foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in tableDeclarationSyntax.ChildNodes().OfType<PropertyDeclarationSyntax>())
				{
					propertySymbol = propertyDeclarationSyntax.GetTypeSymbol<IPropertySymbol>(compilation);
					if (propertySymbol == null) continue;

					columnAttributeData = propertySymbol.GetAttribute($"{Namespace}.ColumnAttribute");
					if (columnAttributeData == null) continue;
					//if (!propertyDeclarationSyntax.ContainsAttribute(compilation, $"{Namespace}.ColumnAttribute")) continue;

					columnName = propertyDeclarationSyntax.Identifier.Text;
					columnType = propertyDeclarationSyntax.Type.ToString();
					isNullable = propertyDeclarationSyntax.Type is NullableTypeSyntax;

					if (columnAttributeData.NamedArguments.Length > 0)
					{
						object? value = columnAttributeData.NamedArguments[0].Value.Value;
						if (value == null) displayName = columnName.SplitCamelCase();
						else displayName = value.ToString();
					}
					else displayName = columnName.SplitCamelCase();

					

					columnModel = new Column(tableModel, columnName, displayName, columnType, isNullable);
					tableModel.Columns.Add(columnModel);

					if (propertyDeclarationSyntax.ContainsAttribute(compilation, $"{Namespace}.PrimaryKeyAttribute")) tableModel.PrimaryKey=columnModel;

				}
				DatabaseModel.Tables.Add(tableModel);

			}
		}
		private static void CreateRelationModels(SourceProductionContext context, Compilation compilation, Database DatabaseModel, IEnumerable<SyntaxNode> TableDeclarationSyntaxList)
		{
			string foreignColumnName;
			string foreignTableName;
			string nameSpace;
			string? primaryPropertyName,foreignPropertyName, primaryTableName, primaryColumnName,cascadeActionName;

			CascadeTriggers cascadeTrigger;

			Table? foreignTableModel,primaryTableModel;
			Column? foreignColumnModel,primaryColumnModel;

			INamedTypeSymbol? tableSymbol;
			IPropertySymbol? propertySymbol;
			//bool isNullable;
			Relation relationModel;
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

				foreignTableModel = DatabaseModel.Tables.FirstOrDefault(item => item.TableName == foreignTableName);
				if (foreignTableModel == null) continue;	

				// on recherche les relations pour les ajouter à la table
				foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in tableDeclarationSyntax.ChildNodes().OfType<PropertyDeclarationSyntax>())
				{
					propertySymbol = propertyDeclarationSyntax.GetTypeSymbol<IPropertySymbol>(compilation);
					if (propertySymbol == null) continue;
					foreignColumnName = propertyDeclarationSyntax.Identifier.Text;

					foreignColumnModel = foreignTableModel.Columns.FirstOrDefault(item => item.ColumnName == foreignColumnName);
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


					primaryTableModel = DatabaseModel.Tables.FirstOrDefault(item => item.TableName == primaryTableName);
					if (primaryTableModel == null) continue;

					primaryColumnModel = primaryTableModel.Columns.FirstOrDefault(item => item.ColumnName == primaryColumnName);
					if (primaryColumnModel == null) continue;

				
					relationModel = new Relation(primaryPropertyName,  primaryColumnModel, foreignPropertyName,  foreignColumnModel, cascadeTrigger);
					foreignTableModel.Relations.Add(relationModel);
					primaryTableModel.Relations.Add(relationModel);

					//columnType = propertyDeclarationSyntax.Type.ToString();
					//isNullable = propertyDeclarationSyntax.Type is NullableTypeSyntax;

					
				}

			}
		}

		private void GenerateDynamicSources(SourceProductionContext context, Compilation compilation, IEnumerable<(SyntaxNode, DataModelType)> Declarations)
		{
			Database? database;

			database = CreateDatabaseModel(context, compilation, Declarations.FirstOrDefault(item => item.Item2 == DataModelType.Database).Item1 as TypeDeclarationSyntax);
			if (database == null) return;
			CreateTableModels(context, compilation, database, Declarations.Where(item => item.Item2 == DataModelType.Table).Select(item => item.Item1));
			CreateRelationModels(context, compilation, database, Declarations.Where(item => item.Item2 == DataModelType.Table).Select(item => item.Item1));

			OnGenerateDynamicSources(context, database);

		}
		protected abstract void OnGenerateDynamicSources(SourceProductionContext context, Database database);
	

	}
}