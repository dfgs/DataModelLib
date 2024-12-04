﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModelLib.DataModels
{
	public class TableModel : DataModel
	{

		public string Namespace { get; private set; }
		public string DatabaseClassName { get; private set; }
		public string TableClassName { get; private set; }
		public string TableName { get; private set; }


		public ColumnModel? PrimaryKey
		{
			get; 
			set;
		}

		public List<ColumnModel> ColumnModels
		{
			get;
			private set;
		}
		public List<RelationModel> Relations
		{
			get;
			private set;
		}

		public TableModel(string Namespace, string DatabaseClassName, string ClassName,string TableName) : base()
		{
			this.Namespace = Namespace; this.DatabaseClassName = DatabaseClassName; this.TableClassName= ClassName;this.TableName = TableName;
			this.ColumnModels= new List<ColumnModel>();
			this.Relations = new List<RelationModel>();
		}
		public string GenerateDatabaseProperties()
		{
			string source =
			$$"""
			public List<{{TableClassName}}> {{TableName}} {get;set;}
			""";

			return source;
		}
		public string GenerateDatabaseConstructor()
		{
			string source =
			$$"""
			{{TableName}} = new List<{{TableClassName}}>();
			""";

			return source;
		}

		public string GenerateDatabaseModelMethods()
		{
			string source =
			$$"""
			public IEnumerable<{{TableClassName}}Model> Get{{TableName}}()
			{
				return dataSource.{{TableName}}.Select(item=>new {{TableClassName}}Model(this, item));
			}
			public void AddTo{{TableName}}({{TableClassName}} Item)
			{
				dataSource.{{TableName}}.Add(Item);
			}
			""";
			
			if (PrimaryKey == null) return source;
			
			source +="\r\n"+
			$$"""
			public void RemoveFrom{{TableName}}({{TableClassName}}Model Item)
			{
				{{TableClassName}} dataSourceItem;

				dataSourceItem=dataSource.{{TableName}}.First(item=>item.{{PrimaryKey.ColumnName}} == Item.{{PrimaryKey.ColumnName}});
				dataSource.{{TableName}}.Remove(dataSourceItem);
			}
			""";

			return source;
		}

		public string GenerateTableModelClass()
		{
			string source =
			$$"""
			// <auto-generated/>
			using System;
			using System.Collections.Generic;
			using System.Linq;

			namespace {{Namespace}}
			{
				public partial class {{TableClassName}}Model
				{
					private {{TableClassName}} dataSource
					{
						get;
						set;
					}

					private {{DatabaseClassName}}Model databaseModel;
			
			{{string.Join("\r\n", ColumnModels.Select(item => item.GenerateTableModelProperties())).Indent(2)}}
			{{this.GenerateTableModelConstructor().Indent(2)}}
			{{this.GenerateTableModelMethods().Indent(2)}}
									
				}
			}
			""";

			return source;
		}
		public string GenerateTableModelConstructor()
		{
			string source =
			$$"""
			public {{TableClassName}}Model({{DatabaseClassName}}Model DatabaseModel, {{TableClassName}} DataSource)
			{
				this.databaseModel=DatabaseModel;
				this.dataSource=DataSource;
			}
			""";

			return source;
		}

		public string GenerateTableModelMethods()
		{
			string source;

			if (PrimaryKey == null)
			{
				source=$$"""
				{{string.Join("\r\n", Relations.Select(item => item.GenerateTableModelMethods(this == item.PrimaryTable)))}}
				""";
			}
			else
			{
				source = $$"""
				public void Delete()
				{
					this.databaseModel.RemoveFrom{{TableName}}(this);
				}
				{{string.Join("\r\n", Relations.Select(item => item.GenerateTableModelMethods(this == item.PrimaryTable)))}}
				""";
			}

			return source;
		}

	}
}
