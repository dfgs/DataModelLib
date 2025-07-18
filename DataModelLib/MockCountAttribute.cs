using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MockCountAttribute : Attribute
	{
		public byte Value
		{
			get;
			set;
		}
		public MockCountAttribute()
		{
		}
		public MockCountAttribute(byte Value)
		{
			this.Value= Value;
		}

	}
}
