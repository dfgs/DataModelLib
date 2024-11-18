using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class TableModel : DataModel
	{

		public string Namespace { get; private set; }
		public string ClassName { get; private set; }
		public string TableName { get; private set; }


		public TableModel(string Namespace,string ClassName,string TableName) : base()
		{
			this.Namespace = Namespace; this.ClassName= ClassName;this.TableName = TableName;
		}
		public override string GenerateDatabaseSource()
		{
			string source =
			$$"""
			public List<{{ClassName}}> {{TableName}} {get;set;}
			""";

			return source;
		}
		public override string GenerateDatabaseConstructorSource()
		{
			string source =
			$$"""
			{{TableName}} = new List<{{ClassName}}>();
			""";

			return source;
		}

		public override string GenerateDatabaseModelSource()
		{
			string source =
			$$"""
			public IEnumerable<{{ClassName}}> Get{{TableName}}()
			{
				return dataSource.{{TableName}};
			}
			""";
			/*if (IsList)
			{
				source+= "\r\n"+
				$$"""
				public void AddTo{{ClassName}}({{ItemType}} Item)
				{
					dataSource.{{ClassName}}.Add(Item);
				}
				public void RemoveFrom{{ClassName}}({{ItemType}} Item)
				{
					dataSource.{{ClassName}}.Remove(Item);
				}
				""";
			}*/

			return source;
		}


	}
}
