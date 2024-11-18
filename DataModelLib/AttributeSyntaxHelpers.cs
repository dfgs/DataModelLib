using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataModelLib
{
	public static class AttributeSyntaxHelpers
	{

		

		public static bool ContainsAttribute(this AttributeSyntax AttributeSyntax, SemanticModel SemanticModel, string AttributeName)
		{
			if (SemanticModel.GetSymbolInfo(AttributeSyntax).Symbol is IMethodSymbol attributeSymbol)
			{
				string typeName = attributeSymbol.ContainingType.ToDisplayString();
				return typeName == AttributeName;
			}

			return false;
		}
		public static string? GetTableName(this AttributeSyntax AttributeSyntax)
		{
			if (AttributeSyntax.ArgumentList==null) return null;
			if (AttributeSyntax.ArgumentList.Arguments.Count == 0) return null;
			return AttributeSyntax.ArgumentList.Arguments[0].GetText().ToString();
		}

		

		public static bool ContainsAttribute(this SyntaxNode Node, SemanticModel SemanticModel, string AttributeName)
		{
			return Node.GetAttributeSyntax(SemanticModel, AttributeName) != null;
		}
		public static AttributeSyntax? GetAttributeSyntax(this SyntaxNode Node, SemanticModel SemanticModel, string AttributeName)
		{
			return Node.EnumerateAttributeSyntax().SelectMany(item=>item.Attributes).FirstOrDefault(attributeSyntax => attributeSyntax.ContainsAttribute(SemanticModel, AttributeName));
		}

		public static IEnumerable<AttributeListSyntax> EnumerateAttributeSyntax(this SyntaxNode Node)
		{
			switch(Node)
			{
				case ClassDeclarationSyntax classDeclaration:return classDeclaration.AttributeLists;
				case RecordDeclarationSyntax recordDeclaration: return recordDeclaration.AttributeLists;
				case PropertyDeclarationSyntax propertyDeclaration:return propertyDeclaration.AttributeLists;
				default:return Enumerable.Empty<AttributeListSyntax>();
			};
		}

		public static INamedTypeSymbol? GetTypeSymbol(this PropertyDeclarationSyntax Node, SemanticModel SemanticModel)
		{
			INamedTypeSymbol? symbol;

			symbol = SemanticModel.GetSymbolInfo(Node.Type).Symbol as INamedTypeSymbol;
			return symbol;
		}

	}
}
