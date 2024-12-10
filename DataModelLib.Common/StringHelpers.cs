using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataModelLib.Common
{
	public static class StringHelpers
	{

		public static string Indent(this string Input, byte Indents)
		{
			return $"{new string('\t', Indents)}{Input.Replace("\r\n", "\r\n" + new string('\t', Indents))}";
		}
		public static string Join(this IEnumerable<string> Inputs)
		{
			return "\r\n"+string.Join("\r\n",Inputs);
		}



	}
}
