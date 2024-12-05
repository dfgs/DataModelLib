using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{

	public enum CascadeTriggers { None, Delete, Update };

	public class RelationModel : DataModel
	{

		public string PrimaryPropertyName { get; private set; }
		public TableModel PrimaryTable { get; private set; }
		public ColumnModel PrimaryKey { get; private set; }
		public string ForeignPropertyName { get; private set; }
		public TableModel ForeignTable { get; private set; }
		public ColumnModel ForeignKey { get; private set; }

		public CascadeTriggers CascadeTrigger { get; private set; }	

		public RelationModel(string PrimaryPropertyName, TableModel PrimaryTable, ColumnModel PrimaryKey, string ForeignPropertyName, TableModel ForeignTable, ColumnModel ForeignKey,CascadeTriggers CascadeTrigger ) : base()
		{
			this.PrimaryPropertyName = PrimaryPropertyName;
			this.PrimaryTable = PrimaryTable;
			this.PrimaryKey = PrimaryKey;
			this.ForeignPropertyName = ForeignPropertyName;
			this.ForeignTable = ForeignTable;
			this.ForeignKey = ForeignKey;
			this.CascadeTrigger = CascadeTrigger;
		}

		public override string ToString()
		{
			return $"{ForeignTable.TableName}.{ForeignKey.ColumnName} -> {PrimaryTable.TableName}.{PrimaryKey.ColumnName}";
		}
		public string GenerateTableModelMethods(bool IsPrimaryTable)
		{
			string source;

			if (IsPrimaryTable)
			{
				source =
				$$"""
				// Get foreign items from relation {{this}}
				public IEnumerable<{{ForeignTable.TableName}}Model> Get{{PrimaryPropertyName}}()
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
					#nullable enable
					// Get primary items from relation {{this}}
					public {{PrimaryTable.TableName}}Model? Get{{ForeignPropertyName}}()
					{
						if ({{ForeignKey.ColumnName}} is null) return null;
						return databaseModel.Get{{PrimaryTable.TableName}}().First(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}});
					}
					#nullable disable
					""";
				}
				else
				{
					source =
					$$"""
					// Get primary items from relation {{this}}
					public {{PrimaryTable.TableName}}Model Get{{ForeignPropertyName}}()
					{
						return databaseModel.Get{{PrimaryTable.TableName}}().First(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}});
					}
					""";
				}

			}

			return source;
		}

		public string GenerateCascadeActionSource( )
		{
			string source="";
			switch (CascadeTrigger)
			{
				case CascadeTriggers.None:
					break;
				case CascadeTriggers.Delete:
					source =
					$$"""
					{
						// Cascade delete from relation {{this}}
						foreach({{ForeignTable.TableName}}Model foreignItem in Get{{ForeignTable.TableName}}().Where(foreignItem=>foreignItem.{{ForeignKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}}).ToArray())
						{
							foreignItem.Delete();
						}
					}
					""";
					break;
				case CascadeTriggers.Update:
					if (ForeignKey.IsNullable)
					{
						source =
						$$"""
						{
							// Cascade update from relation {{this}}
							foreach({{ForeignTable.TableName}}Model foreignItem in Get{{ForeignTable.TableName}}().Where(foreignItem=>foreignItem.{{ForeignKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}}).ToArray())
							{
								foreignItem.{{ForeignKey.ColumnName}}=null;
							}
						}
						""";
					}
					else
					{
						source =
						$$"""
						{
							// Cascade update from relation {{this}}
							{{PrimaryKey.TypeName}} fallBackValue=Get{{PrimaryTable.TableName}}().First(item=>item!=Item).{{PrimaryKey.ColumnName}};
							foreach({{ForeignTable.TableName}}Model foreignItem in Get{{ForeignTable.TableName}}().Where(foreignItem=>foreignItem.{{ForeignKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}}).ToArray())
							{
								foreignItem.{{ForeignKey.ColumnName}}=fallBackValue;
							}
						}
						""";
					}
					break;
			}
			return source;
		}

	}
}
