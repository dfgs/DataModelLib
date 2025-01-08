using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataModelLib.Common.Schema
{
	public class Column : SchemaObject
	{

		public string ColumnName { get; private set; }
		public string DisplayName { get; private set; }
		public string TypeName { get; private set; }
		public bool IsNullable { get; private set; }
		public Table Table { get; private set; }

		public Column(Table Table,string ColumnName,string DisplayName, string TypeName, bool IsNullable) : base()
		{
			this.Table = Table;
			this.ColumnName = ColumnName;
			this.DisplayName = DisplayName;
			this.TypeName = TypeName;
			this.IsNullable = IsNullable;
		}

		public override string ToString()
		{
			return $"{Table.TableName}.{ColumnName}";
		}


	}
}
