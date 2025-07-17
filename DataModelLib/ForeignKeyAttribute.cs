using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace DataModelLib
{
	public enum CascadeTriggers { None=0, Delete=1, Update=2 };

	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public class ForeignKeyAttribute : Attribute
	{
		public required string ForeignPropertyName
		{
			get;
			set;
		}
		public required string PrimaryPropertyName
		{
			get;
			set;
		}
		public required string PrimaryTable
		{
			get;
			set;
		}
		public required string PrimaryKey
		{
			get;
			set;
		}
		public required CascadeTriggers CascadeTrigger
		{
			get;
			set;
		}

		[SetsRequiredMembers]
		public ForeignKeyAttribute(string ForeignPropertyName, string PrimaryPropertyName, string PrimaryTable, string PrimaryKey, CascadeTriggers CascadeTrigger)
		{
			this.ForeignPropertyName = ForeignPropertyName; this.PrimaryPropertyName = PrimaryPropertyName; this.PrimaryTable = PrimaryTable; this.PrimaryKey = PrimaryKey; this.CascadeTrigger = CascadeTrigger;
		}

		public ForeignKeyAttribute()
		{

		}

	}
}
