using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class RelationModel : DataModel
	{

		public string PrimaryPropertyName { get; private set; }
		public TableModel PrimaryTable { get; private set; }
		public ColumnModel PrimaryKey { get; private set; }
		public string ForeignPropertyName { get; private set; }
		public TableModel ForeignTable { get; private set; }
		public ColumnModel ForeignKey { get; private set; }


		public RelationModel(string PrimaryPropertyName, TableModel PrimaryTable, ColumnModel PrimaryKey, string ForeignPropertyName, TableModel ForeignTable, ColumnModel ForeignKey) : base()
		{
			this.PrimaryPropertyName = PrimaryPropertyName;
			this.PrimaryTable = PrimaryTable;
			this.PrimaryKey = PrimaryKey;
			this.ForeignPropertyName = ForeignPropertyName;
			this.ForeignTable = ForeignTable;
			this.ForeignKey = ForeignKey;
		}
		public string GenerateTableModelMethods(bool IsPrimaryTable)
		{
			string source;

			if (IsPrimaryTable)
			{
				source =
				$$"""
				public IEnumerable<{{ForeignTable.TableClassName}}Model> Get{{PrimaryPropertyName}}()
				{
					return databaseModel.Get{{ForeignTable.TableName}}().Where(item=>item.{{ForeignKey.ColumnName}} == {{PrimaryKey.ColumnName}});
				}
				""";
			}
			else
			{
				if (ForeignKey.IsNullable)
				{
					source =
					$$"""
					public {{PrimaryTable.TableClassName}}Model? Get{{ForeignPropertyName}}()
					{
						if ({{ForeignKey.ColumnName}} is null) return null;
						return databaseModel.Get{{PrimaryTable.TableName}}().First(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}});
					}
					""";
				}
				else
				{
					source =
					$$"""
					public {{PrimaryTable.TableClassName}}Model Get{{ForeignPropertyName}}()
					{
						return databaseModel.Get{{PrimaryTable.TableName}}().First(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}});
					}
					""";
				}

			}

			return source;
		}



	}
}
