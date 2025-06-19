
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModelLib.SourceGenerator
{
	public class DatabaseModelSourceGenerator 
	{
		/*
		public override string GenerateSource(Database Database)
		{
			string source =
			
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
					#nullable enable
					{{Table.TableName}}? dataSourceItem;
					#nullable disable
					int index;

					
					dataSourceItem=dataSource.{{Table.TableName}}Table.FirstOrDefault(item=>item.{{Table.PrimaryKey.ColumnName}} == Item.{{Table.PrimaryKey.ColumnName}});
					if (dataSourceItem == null) return;
					index=dataSource.{{Table.TableName}}Table.IndexOf(dataSourceItem);

					if ({{Table.TableName}}TableChanging != null) {{Table.TableName}}TableChanging(dataSourceItem,TableChangedActions.Remove, index);
				
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
				return Create{{Table.TableName}}Model(dataSource.{{Table.TableName}}Table.First(Predicate));
			}
			{{getByPrivateKeyMethod}}
			public IEnumerable<{{Table.TableName}}Model> Get{{Table.TableName}}Table()
			{
				return dataSource.{{Table.TableName}}Table.Select(item=> Create{{Table.TableName}}Model(item));
			}
			public IEnumerable<{{Table.TableName}}Model> Get{{Table.TableName}}Table(Func<{{Table.TableName}},bool> Predicate)
			{
				return dataSource.{{Table.TableName}}Table.Where(Predicate).Select(item=>Create{{Table.TableName}}Model(item));
			}
			public void Add{{Table.TableName}}({{Table.TableName}} Item)
			{
				int index;

				if (dataSource.{{Table.TableName}}Table.Contains(Item)) return;

				index = dataSource.{{Table.TableName}}Table.Count;
				if ({{Table.TableName}}TableChanging != null) {{Table.TableName}}TableChanging(Item,TableChangedActions.Add, index);
				dataSource.{{Table.TableName}}Table.Add(Item);
				if ({{Table.TableName}}TableChanged != null) {{Table.TableName}}TableChanged(Item,TableChangedActions.Add, index);
			}
			{{removeMethod}}

			public void Notify{{Table.TableName}}RowChanging({{Table.TableName}} Item, string PropertyName, object OldValue, object NewValue)
			{
				if ({{Table.TableName}}RowChanging != null) {{Table.TableName}}RowChanging(Item,PropertyName,OldValue,NewValue);
			}
			public void Notify{{Table.TableName}}RowChanged({{Table.TableName}} Item, string PropertyName, object OldValue, object NewValue)
			{
				if ({{Table.TableName}}RowChanged != null) {{Table.TableName}}RowChanged(Item,PropertyName,OldValue,NewValue);
			}
			
			public {{Table.TableName}}Model Create{{Table.TableName}}Model({{Table.TableName}} Item)
			{
				{{Table.TableName}}Model model;
			
				if (Item==null) throw new ArgumentNullException(nameof(Item));
			
				if (!{{Table.TableName}}Dictionary.TryGetValue(Item,out model))
				{
					model=new {{Table.TableName}}Model(this, Item);
					{{Table.TableName}}Dictionary.Add(Item,model);
				}

				return model;
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


*/

	}

}
