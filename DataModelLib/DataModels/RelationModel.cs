using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class RelationModel : DataModel
	{

		public string PropertyName { get; private set; }
		public TableModel PrimaryTable { get; private set; }
		public ColumnModel PrimaryKey { get; private set; }
		public TableModel ForeignTable { get; private set; }
		public ColumnModel ForeignKey { get; private set; }


		public RelationModel(string PropertyName, TableModel PrimaryTable, ColumnModel PrimaryKey, TableModel ForeignTable, ColumnModel ForeignKey) : base()
		{
			this.PropertyName = PropertyName;
			this.PrimaryTable = PrimaryTable;
			this.PrimaryKey = PrimaryKey;
			this.ForeignTable = ForeignTable;
			this.ForeignKey = ForeignKey;
		}
		public string GenerateTableModelMethods()
		{
			string source =
			$$"""
			public {{PrimaryTable.TableClassName}} Get{{PropertyName}}()
			{
				return dataSource.{{PrimaryTable.TableName}}.Where(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}}).Select(item=>new {{PrimaryTable.TableClassName}}Model(this, item));
			}
			""";

			return source;
		}



	}
}
