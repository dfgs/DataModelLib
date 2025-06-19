using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public class ColumnAttribute : Attribute
	{
		public string? DisplayName
		{
			get;
			set;
		}
		public ColumnAttribute()
		{
		}
	}
}
