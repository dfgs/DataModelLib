using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class ForeignKeyModel : DataModel
	{

		public string PropertyName { get; private set; }
		public string PrimaryTable { get; private set; }
		public string PrimaryKey { get; private set; }


		public ForeignKeyModel(string PropertyName, string PrimaryTable, string PrimaryKey) : base()
		{
			this.PropertyName = PropertyName;
			this.PrimaryTable = PrimaryTable;
			this.PrimaryKey = PrimaryKey;
		}
		/*public string GenerateTableModelProperties()
		{
			string source =
			$$"""
			public {{TypeName}} {{ColumnName}} 
			{
				get => DataSource.{{ColumnName}};
				set => DataSource.{{ColumnName}} = value;
			}
			""";

			return source;
		}*/
		
		

	}
}
