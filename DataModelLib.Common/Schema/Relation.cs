using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.Common.Schema
{

	public enum CascadeTriggers { None, Delete, Update };

	public class Relation : SchemaObject
	{

		public string PrimaryPropertyName { get; private set; }
		public Table PrimaryTable { get; private set; }
		public Column PrimaryKey { get; private set; }
		public string ForeignPropertyName { get; private set; }
		public Table ForeignTable { get; private  set; }
		public Column ForeignKey { get; private set; }

		public CascadeTriggers CascadeTrigger { get; private set; }	

		public Relation(string PrimaryPropertyName,  Column PrimaryKey, string ForeignPropertyName,  Column ForeignKey,CascadeTriggers CascadeTrigger ) : base()
		{
			this.PrimaryPropertyName = PrimaryPropertyName;
			this.PrimaryTable = PrimaryKey.Table;
			this.PrimaryKey = PrimaryKey;
			this.ForeignPropertyName = ForeignPropertyName;
			this.ForeignTable = ForeignKey.Table;
			this.ForeignKey = ForeignKey;
			this.CascadeTrigger = CascadeTrigger;
		}

		public override string ToString()
		{
			return $"{ForeignTable.TableName}.{ForeignKey.ColumnName} -> {PrimaryTable.TableName}.{PrimaryKey.ColumnName}";
		}

	}
}
