﻿using DataModelLib.Common;
using DataModelLib.Common.Schema;
using DataModelLib.Common.SourceGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModelLib.SourceGenerator
{
	public class DatabaseModelSourceGenerator : SourceGenerator<Database>
	{
		public override string GenerateSource(Database Database)
		{
			string source =
			$$"""
			// <auto-generated/>
			using System;
			using System.Collections.Generic;
			using System.Collections.Specialized;
			using System.Linq;
			using DataModelLib.Common;
			{{Database.Tables.Select(item => $"using {item.Namespace};").Distinct().Join()}}
			
			namespace {{Database.Namespace}}.Models
			{
				public partial class {{Database.DatabaseName}}Model
				{
			{{Database.Tables.Select(item => $"public event TableChangedEventHandler<{item.TableName}> {item.TableName}TableChanged;").Join().Indent(2)}}
			{{Database.Tables.Select(item => $"public event RowChangedEventHandler<{item.TableName}> {item.TableName}RowChanged;").Join().Indent(2)}}
			
					private {{Database.DatabaseName}} dataSource;

					public {{Database.DatabaseName}}Model({{Database.DatabaseName}} DataSource)
					{
						this.dataSource=DataSource;
					}			

			{{Database.Tables.Select(item => GenerateDatabaseModelMethods(item)).Join().Indent(2)}}
				}
			}
			""";

			return source;
		}

		private string GenerateDatabaseModelMethods(Table Table)
		{
			string removeMethod = "";
			string cascadeActions = "";
			string getByPrivateKeyMethod = "";


			if (Table.PrimaryKey != null)
			{
				cascadeActions = string.Join("\r\n", Table.Relations.Where(item => item.PrimaryTable == Table).Select(item => GenerateCascadeActions(item)));

				removeMethod =
				$$"""
				public void Remove{{Table.TableName}}({{Table.TableName}}Model Item)
				{
					{{Table.TableName}} dataSourceItem;
					int index;

					
					dataSourceItem=dataSource.{{Table.TableName}}Table.First(item=>item.{{Table.PrimaryKey.ColumnName}} == Item.{{Table.PrimaryKey.ColumnName}});
					index=dataSource.{{Table.TableName}}Table.IndexOf(dataSourceItem);
					dataSource.{{Table.TableName}}Table.Remove(dataSourceItem);
				
				{{cascadeActions.Indent(1)}}

					if ({{Table.TableName}}TableChanged != null) {{Table.TableName}}TableChanged(dataSourceItem,TableChangedActions.Remove, index);
				}
				""";

				getByPrivateKeyMethod =
				$$"""
				public {{Table.TableName}}Model Get{{Table.TableName}}({{Table.PrimaryKey.TypeName}} {{Table.PrimaryKey.ColumnName}})
				{
					return Get{{Table.TableName}}(item=>item.{{Table.PrimaryKey.ColumnName}} == {{Table.PrimaryKey.ColumnName}});
				}
				""";

			}

			string source =
			$$"""
			public {{Table.TableName}}Model Get{{Table.TableName}}(Func<{{Table.TableName}},bool> Predicate)
			{
				return new {{Table.TableName}}Model(this, dataSource.{{Table.TableName}}Table.First(Predicate));
			}
			{{getByPrivateKeyMethod}}
			public IEnumerable<{{Table.TableName}}Model> Get{{Table.TableName}}Table()
			{
				return dataSource.{{Table.TableName}}Table.Select(item=>new {{Table.TableName}}Model(this, item));
			}
			public IEnumerable<{{Table.TableName}}Model> Get{{Table.TableName}}Table(Func<{{Table.TableName}},bool> Predicate)
			{
				return dataSource.{{Table.TableName}}Table.Where(Predicate).Select(item=>new {{Table.TableName}}Model(this, item));
			}
			public void Add{{Table.TableName}}({{Table.TableName}} Item)
			{
				int index;
				index = dataSource.{{Table.TableName}}Table.Count;
				dataSource.{{Table.TableName}}Table.Add(Item);
				if ({{Table.TableName}}TableChanged != null) {{Table.TableName}}TableChanged(Item,TableChangedActions.Add, index);
			}
			{{removeMethod}}

			public void Notify{{Table.TableName}}RowChanged({{Table.TableName}} Item,string PropertyName, object OldValue, object NewValue)
			{
				if ({{Table.TableName}}RowChanged != null) {{Table.TableName}}RowChanged(Item,PropertyName,OldValue,NewValue);
			}
			""";


			return source;
		}


		private string GenerateCascadeActions(Relation Relation)
		{
			string source = "";
			switch (Relation.CascadeTrigger)
			{
				case CascadeTriggers.None:
					break;
				case CascadeTriggers.Delete:
					source =
					$$"""
					{
						// Cascade delete from relation {{this}}
						foreach({{Relation.ForeignTable.TableName}}Model foreignItem in Get{{Relation.ForeignTable.TableName}}Table(foreignItem=>foreignItem.{{Relation.ForeignKey.ColumnName}} == Item.{{Relation.PrimaryKey.ColumnName}}).ToArray())
						{
							foreignItem.Delete();
						}
					}
					""";
					break;
				case CascadeTriggers.Update:
					if (Relation.ForeignKey.IsNullable)
					{
						source =
						$$"""
						{
							// Cascade update from relation {{this}}
							foreach({{Relation.ForeignTable.TableName}}Model foreignItem in Get{{Relation.ForeignTable.TableName}}Table(foreignItem=>foreignItem.{{Relation.ForeignKey.ColumnName}} == Item.{{Relation.PrimaryKey.ColumnName}}).ToArray())
							{
								foreignItem.{{Relation.ForeignKey.ColumnName}}=null;
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
							{{Relation.PrimaryKey.TypeName}} fallBackValue=Get{{Relation.PrimaryTable.TableName}}Table().First(item=>item!=Item).{{Relation.PrimaryKey.ColumnName}};
							foreach({{Relation.ForeignTable.TableName}}Model foreignItem in Get{{Relation.ForeignTable.TableName}}Table(foreignItem=>foreignItem.{{Relation.ForeignKey.ColumnName}} == Item.{{Relation.PrimaryKey.ColumnName}}).ToArray())
							{
								foreignItem.{{Relation.ForeignKey.ColumnName}}=fallBackValue;
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
