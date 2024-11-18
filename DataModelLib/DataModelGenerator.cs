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
		  
			[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false)]
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
		




		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/DatabaseAttribute.g.cs", SourceText.From(DatabaseAttributeSourceCode, Encoding.UTF8)));
			context.RegisterPostInitializationOutput(ctx => ctx.AddSource("Attributes/TableAttribute.g.cs", SourceText.From(TableAttributeSourceCode, Encoding.UTF8)));

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
			SemanticModel semanticModel;
			string nameSpace;
			string databaseClassName,tableClassName;
			string tableName;
			DatabaseModel databaseModel;
			TableModel tableModel;
			string source;


			TypeDeclarationSyntax? databaseDeclarationSyntax = Declarations.FirstOrDefault(item => item.Item2 == DataModelType.Database).Item1 as TypeDeclarationSyntax;
			// no database defined, cannot proceed
			if (databaseDeclarationSyntax == null) return;

			// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
			semanticModel = compilation.GetSemanticModel(databaseDeclarationSyntax.SyntaxTree);
			if (semanticModel.GetDeclaredSymbol(databaseDeclarationSyntax) is not INamedTypeSymbol databaseSymbol) return;

			// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
			nameSpace = databaseSymbol.ContainingNamespace.ToDisplayString();
			databaseClassName = databaseDeclarationSyntax.Identifier.Text;

			databaseModel = new DatabaseModel(nameSpace, databaseClassName);


			foreach (TypeDeclarationSyntax tableDeclarationSyntax in Declarations.Where(item=>item.Item2==DataModelType.Table).Select(item=>item.Item1))
			{
				// On récupère le modèle sémantique pour pouvoir manipuler les méta données et le contenu de nos objets 
				semanticModel = compilation.GetSemanticModel(tableDeclarationSyntax.SyntaxTree);
				if (semanticModel.GetDeclaredSymbol(tableDeclarationSyntax) is not INamedTypeSymbol tableSymbol) continue;

		
				// On récupère le namespace, le nom du noeud courant et on créé le nom du futur DTO
				nameSpace = tableSymbol.ContainingNamespace.ToDisplayString();
				tableClassName = tableDeclarationSyntax.Identifier.Text;
				if ((tableSymbol.GetAttributes().Length == 0) || (tableSymbol.GetAttributes()[0].ConstructorArguments.Length == 0)) tableName = $"{tableClassName}s";
				else tableName = tableSymbol.GetAttributes()[0].ConstructorArguments[0].Value?.ToString()?? $"{tableClassName}s";

				tableModel = new TableModel(nameSpace, databaseClassName, tableClassName, tableName);
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