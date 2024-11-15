using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataModelLib
{
	public static class StringHelpers
	{

		public static string Indent(this string Input, byte Indents)
		{
			return $"{new string('\t', Indents)}{Input.Replace("\r\n", "\r\n" + new string('\t', Indents))}";
		}
		


	}
}
