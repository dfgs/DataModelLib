using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.Schema
{
	public class Column : SchemaObject
	{
			
		public string ColumnName { get; private set; }
		public string TypeName { get; private set; }
		public bool IsNullable { get; private set; }
		public Table Table { get; private set; }

		public Column(Table Table,string ColumnName, string TypeName, bool IsNullable) : base()
		{
			this.Table = Table;
			this.ColumnName = ColumnName;
			this.TypeName = TypeName;
			this.IsNullable = IsNullable;
		}
		public string GenerateTableModelProperties()
		{
			string source =
			$$"""
			public {{TypeName}} {{ColumnName}} 
			{
				get => dataSource.{{ColumnName}};
				set {{{TypeName}} oldValue=value; dataSource.{{ColumnName}} = value; databaseModel.Notify{{Table.TableName}}RowChanged(dataSource,nameof({{ColumnName}}), oldValue,value ); }
			}
			""";

			return source;
		}
		


	}
}
