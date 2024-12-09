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
		public TableModel ForeignTable { get; private  set; }
		public ColumnModel ForeignKey { get; private set; }

		public CascadeTriggers CascadeTrigger { get; private set; }	

		public RelationModel(string PrimaryPropertyName,  ColumnModel PrimaryKey, string ForeignPropertyName,  ColumnModel ForeignKey,CascadeTriggers CascadeTrigger ) : base()
		{
			this.PrimaryPropertyName = PrimaryPropertyName;
			this.PrimaryTable = PrimaryKey.TableModel;
			this.PrimaryKey = PrimaryKey;
			this.ForeignPropertyName = ForeignPropertyName;
			this.ForeignTable = ForeignKey.TableModel;
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
					return databaseModel.Get{{ForeignTable.TableName}}Table(item=>item.{{ForeignKey.ColumnName}} == {{PrimaryKey.ColumnName}});
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
						return databaseModel.Get{{PrimaryTable.TableName}}(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}});
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
						return databaseModel.Get{{PrimaryTable.TableName}}(item=>item.{{PrimaryKey.ColumnName}} == {{ForeignKey.ColumnName}});
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
						foreach({{ForeignTable.TableName}}Model foreignItem in Get{{ForeignTable.TableName}}Table(foreignItem=>foreignItem.{{ForeignKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}}).ToArray())
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
							foreach({{ForeignTable.TableName}}Model foreignItem in Get{{ForeignTable.TableName}}Table(foreignItem=>foreignItem.{{ForeignKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}}).ToArray())
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
							{{PrimaryKey.TypeName}} fallBackValue=Get{{PrimaryTable.TableName}}Table().First(item=>item!=Item).{{PrimaryKey.ColumnName}};
							foreach({{ForeignTable.TableName}}Model foreignItem in Get{{ForeignTable.TableName}}Table(foreignItem=>foreignItem.{{ForeignKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}}).ToArray())
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
