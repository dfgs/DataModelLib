using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib
{
	public static class EnumerableExtension
	{
		public static int FirstIndexMatch<TItem>(this IEnumerable<TItem> items, Func<TItem, bool> matchCondition)
		{
			var index = 0;
			foreach (TItem item in items)
			{
				if (matchCondition(item)) return index;
				index++;
			}
			return -1;
		}
	}
}
