using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class ColumnModel : DataModel
	{
			
		public string ColumnName { get; private set; }
		public string TypeName { get; private set; }
		public bool IsNullable { get; private set; }
		public TableModel TableModel { get; private set; }

		public ColumnModel(TableModel TableModel,string ColumnName, string TypeName, bool IsNullable) : base()
		{
			this.TableModel = TableModel;
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
				set {{{TypeName}} oldValue=value; dataSource.{{ColumnName}} = value; databaseModel.Notify{{TableModel.TableName}}RowChanged(dataSource,nameof({{ColumnName}}), oldValue,value ); }
			}
			""";

			return source;
		}
		


	}
}
