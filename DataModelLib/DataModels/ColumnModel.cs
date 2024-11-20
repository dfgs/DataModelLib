using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.DataModels
{
	public class ColumnModel : DataModel
	{

		public string ColumnName { get; private set; }
		public string TypeName { get; private set; }
		public bool IsNullable { get; private set; }
		public ForeignKeyModel? ForeignKey { get; private set; }


		public ColumnModel(string ColumnName, string TypeName, bool IsNullable,ForeignKeyModel? ForeignKey) : base()
		{
			this.ColumnName = ColumnName;
			this.TypeName = TypeName;
			this.IsNullable = IsNullable;
			this.ForeignKey = ForeignKey;
		}
		public string GenerateTableModelProperties()
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
		}

		public string GenerateTableModelMethods()
		{
			if (ForeignKey == null) return "";

			string source =
			$$"""
			public {{ForeignKey.PrimaryTable}} Get{{ForeignKey.PropertyName}}()
			{
				return dataSource.{{ForeignKey.PrimaryTable}}.Where(item=>item.{{ForeignKey.PrimaryKey}} == {{ColumnName}}).Select(item=>new {{ForeignKey.PrimaryTable}}Model(this, item));
			}
			""";

			return source;
		}

	}
}
