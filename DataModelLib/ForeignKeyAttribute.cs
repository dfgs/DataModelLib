using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib
{
	public enum CascadeTriggers { None, Delete, Update };

	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public class ForeignKeyAttribute : Attribute
	{
		public string ForeignPropertyName
		{
			get;
			private set;
		}
		public string PrimaryPropertyName
		{
			get;
			private set;
		}
		public string PrimaryTable
		{
			get;
			private set;
		}
		public string PrimaryKey
		{
			get;
			private set;
		}
		public CascadeTriggers CascadeTrigger
		{
			get;
			private set;
		}


		public ForeignKeyAttribute(string ForeignPropertyName, string PrimaryPropertyName, string PrimaryTable, string PrimaryKey, CascadeTriggers CascadeTrigger)
		{
			this.ForeignPropertyName = ForeignPropertyName; this.PrimaryPropertyName = PrimaryPropertyName; this.PrimaryTable = PrimaryTable; this.PrimaryKey = PrimaryKey; this.CascadeTrigger = CascadeTrigger;
		}
	}
}
