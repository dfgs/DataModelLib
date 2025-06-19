using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModelLib.SourceGenerator
{
	public class ModelSourceGenerator 
	{
		/*
		public override string GenerateSource(Table Table)
		{
			

			string source =
		
			""";

			return source;
		}

		private string GenerateMethods(Table Table)
		{
			string source;

			if (Table.PrimaryKey == null)
			{
				source = $$"""
				{{ Table.Relations.Select(item => GenerateRelationMethods(item,Table == item.PrimaryTable)).Join()}}
				""";
			}
			else
			{
				source = $$"""
				public void Delete()
				{
					this.databaseModel.Remove{{Table.TableName}}(this);
				}
				{{Table.Relations.Select(item => GenerateRelationMethods(item, Table == item.PrimaryTable)).Join()}}
				""";
			}

			return source;
		}
		private string GenerateProperties(Table Table, Column Column)
		{
			string source =
			$$"""
			public {Column.TypeName}} {{Column.ColumnName}} 
			{
				get => dataSource.{{Column.ColumnName}};
				set 
				{
					if (value==dataSource.{{Column.ColumnName}}) return;
					{{Column.TypeName}} oldValue=dataSource.{{Column.ColumnName}}; 
					databaseModel.Notify{{Column.Table.TableName}}RowChanging(dataSource,nameof({{Column.ColumnName}}), oldValue,value ); 
					dataSource.{{Column.ColumnName}} = value; 
					databaseModel.Notify{{Column.Table.TableName}}RowChanged(dataSource,nameof({{Column.ColumnName}}), oldValue,value ); 
					OnPropertyChanged(nameof({{Column.ColumnName}})); 
			{{Table.Relations.Where(item => (item.ForeignTable == Table) && (item.ForeignKey==Column) ).Select(item => $"On{item.ForeignPropertyName}Changed();").Join().Indent(2)}}
				}
			}
			""";

			return source;
		}
		

		private string GenerateRelationEventHandlers(Table PrimaryTable,Table ForeignTable)
		{
			string source;

			source = $$"""
			private void On{{ForeignTable.TableName}}RowChanging({{ForeignTable.TableName}} Item,string PropertyName, object OldValue, object NewValue)
			{
			{{ForeignTable.Relations.Where(item => (item.PrimaryTable == PrimaryTable) && (item.ForeignTable == ForeignTable)).Select(item => GenerateRelationChangingHandler(item)).Join().Indent(1)}}
			}
			private void On{{ForeignTable.TableName}}RowChanged({{ForeignTable.TableName}} Item,string PropertyName, object OldValue, object NewValue)
			{
			{{ForeignTable.Relations.Where(item => (item.PrimaryTable == PrimaryTable) && (item.ForeignTable == ForeignTable)).Select(item => GenerateRelationChangedHandler(item)).Join().Indent(1)}}
			}
			private void On{{ForeignTable.TableName}}TableChanging({{ForeignTable.TableName}} Item,TableChangedActions Action, int Index)
			{
			{{ForeignTable.Relations.Where(item => (item.PrimaryTable == PrimaryTable) && (item.ForeignTable == ForeignTable)).Select(item => GenerateRelationRemoveHandler(item)).Join().Indent(1)}}
			}
			private void On{{ForeignTable.TableName}}TableChanged({{ForeignTable.TableName}} Item,TableChangedActions Action, int Index)
			{
			{{ForeignTable.Relations.Where(item => (item.PrimaryTable == PrimaryTable) && (item.ForeignTable == ForeignTable)).Select(item => GenerateRelationAddHandler(item)).Join().Indent(1)}}
			}
			""";

			return source;
		}
		private string GenerateRelationChangingHandler(Relation Relation)
		{
			string source;

			source = $$"""
			// Handle event for relation {{Relation.PrimaryPropertyName}}
			{
				if ((PropertyName == "{{Relation.ForeignKey.ColumnName}}") && ValueType.Equals(OldValue, dataSource.{{Relation.PrimaryKey.ColumnName}}) && !ValueType.Equals(NewValue,OldValue) && ({{Relation.PrimaryPropertyName}}Changed!=null)  )
				{		
					int index;
					index=Get{{Relation.PrimaryPropertyName}}().FirstIndexMatch(model => model.IsModelOf(Item));
					{{Relation.PrimaryPropertyName}}Changed(Item, TableChangedActions.Remove, index);
				}
			}
			""";

			return source;
		}
		private string GenerateRelationChangedHandler(Relation Relation)
		{
			string source;

			source = $$"""
			// Handle event for relation {{Relation.PrimaryPropertyName}}
			{
				if ((PropertyName == "{{Relation.ForeignKey.ColumnName}}") && ValueType.Equals(NewValue, dataSource.{{Relation.PrimaryKey.ColumnName}}) && !ValueType.Equals(NewValue,OldValue) && ({{Relation.PrimaryPropertyName}}Changed!=null)  )
				{		
					int index;
					index=Get{{Relation.PrimaryPropertyName}}().FirstIndexMatch(model => model.IsModelOf(Item));
					{{Relation.PrimaryPropertyName}}Changed(Item, TableChangedActions.Add, index);
				}
			}
			""";

			return source;
		}
		private string GenerateRelationRemoveHandler(Relation Relation)
		{
			string source;

			source = $$"""
			// Handle event for relation {{Relation.PrimaryPropertyName}}
			{
				if ((Item.{{Relation.ForeignKey.ColumnName}} == dataSource.{{Relation.PrimaryKey.ColumnName}}) && ({{Relation.PrimaryPropertyName}}Changed!=null) && (Action==TableChangedActions.Remove) )
				{		
					int index;
					index=Get{{Relation.PrimaryPropertyName}}().FirstIndexMatch(model => model.IsModelOf(Item));
					{{Relation.PrimaryPropertyName}}Changed(Item, TableChangedActions.Remove, index);
				}
			}
			""";

			return source;
		}
		private string GenerateRelationAddHandler(Relation Relation)
		{
			string source;

			source = $$"""
			// Handle event for relation {{Relation.PrimaryPropertyName}}
			{
				if ((Item.{{Relation.ForeignKey.ColumnName}} == dataSource.{{Relation.PrimaryKey.ColumnName}}) && ({{Relation.PrimaryPropertyName}}Changed!=null) && (Action==TableChangedActions.Add) )
				{		
					int index;
					index=Get{{Relation.PrimaryPropertyName}}().FirstIndexMatch(model => model.IsModelOf(Item));
					{{Relation.PrimaryPropertyName}}Changed(Item, TableChangedActions.Add, index);
				}
			}
			""";

			return source;
		}
		private string GenerateRelationMethods(Relation Relation, bool IsPrimaryTable)
		{
			string source;

			if (IsPrimaryTable)
			{
				source =
				$$"""
				// Get foreign items from relation {{this}}
				public IEnumerable<{{Relation.ForeignTable.TableName}}Model> Get{{Relation.PrimaryPropertyName}}()
				{
					return databaseModel.Get{{Relation.ForeignTable.TableName}}Table(item=>item.{{Relation.ForeignKey.ColumnName}} == {{Relation.PrimaryKey.ColumnName}});
				}
				""";
			}
			else
			{

				if (Relation.ForeignKey.IsNullable)
				{
					source =
					$$"""
					protected virtual void On{{Relation.ForeignPropertyName}}Changed()
					{
						if ({{Relation.ForeignPropertyName}}Changed!=null) {{Relation.ForeignPropertyName}}Changed(this, EventArgs.Empty);
					}
					#nullable enable
					// Get primary items from relation {{this}}
					public {{Relation.PrimaryTable.TableName}}Model? Get{{Relation.ForeignPropertyName}}()
					{
						if ({{Relation.ForeignKey.ColumnName}} is null) return null;
						return databaseModel.Get{{Relation.PrimaryTable.TableName}}(item=>item.{{Relation.PrimaryKey.ColumnName}} == {{Relation.ForeignKey.ColumnName}});
					}
					#nullable disable
					""";
				}
				else
				{
					source =
					$$"""
					protected virtual void On{{Relation.ForeignPropertyName}}Changed()
					{
						if ({{Relation.ForeignPropertyName}}Changed!=null) {{Relation.ForeignPropertyName}}Changed(this, EventArgs.Empty);
					}
					// Get primary item from relation {{this}}
					public {{Relation.PrimaryTable.TableName}}Model Get{{Relation.ForeignPropertyName}}()
					{
						return databaseModel.Get{{Relation.PrimaryTable.TableName}}(item=>item.{{Relation.PrimaryKey.ColumnName}} == {{Relation.ForeignKey.ColumnName}});
					}
					""";
				}

			}

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
