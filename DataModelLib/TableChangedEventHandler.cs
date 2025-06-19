using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib
{
	public enum TableChangedActions { Add, Remove };
	public delegate void TableChangedEventHandler<T>(T Item, TableChangedActions Action, int Index);
	public delegate void RowChangedEventHandler<T>(T Item, string PropertyName, object OldValue, object NewValue);
}
