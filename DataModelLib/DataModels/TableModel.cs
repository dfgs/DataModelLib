using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class TableModel : DataModel
	{

		public string Name { get; private set; }

		public string ItemType { get; private set; }

		public bool IsEnumerable { get; private set; }
		public bool IsList{ get; private set; }

		public TableModel(string Name,string ItemType,bool IsEnumerable,bool IsList) : base()
		{
			this.Name= Name;this.ItemType = ItemType;this.IsEnumerable = IsEnumerable;this.IsList= IsList;
		}

		
		public override string GenerateCode()
		{
			string source =
			$$"""
			public IEnumerable<{{ItemType}}> Get{{Name}}()
			{
				return dataSource.{{Name}};
			}
			""";
			if (IsList)
			{
				source+=
				$$"""
				public void AddTo{{Name}}({{ItemType}} Item)
				{
					dataSource.{{Name}}.Add(Item);
				}
				public void RemoveFrom{{Name}}({{ItemType}} Item)
				{
					dataSource.{{Name}}.Remove(Item);
				}
				""";
			}

			return source;
		}


	}
}
