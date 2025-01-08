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
			if (!Inputs.Any()) return "";
			return "\r\n"+string.Join("\r\n",Inputs);
		}

		public static string SplitCamelCase(this string Input)
		{
			string splitted= System.Text.RegularExpressions.Regex.Replace(Input, "(?<!(^|[A-Z]))(?=[A-Z])|(?<!^)(?=[A-Z][a-z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().ToLower();
			return splitted[0].ToString().ToUpper() + splitted.Substring(1);
		}

	}
}
