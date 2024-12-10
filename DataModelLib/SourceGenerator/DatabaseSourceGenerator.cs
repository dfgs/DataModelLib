﻿using DataModelLib.Common.Schema;
using DataModelLib.Common.SourceGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModelLib.Common;

namespace DataModelLib.SourceGenerator
{
	public class DatabaseSourceGenerator : SourceGenerator<Database>
	{
		public override string GenerateSource(Database Database)
		{
			string source =
			$$"""
			// <auto-generated/>
			using System;
			using System.Collections.Generic;
			{{Database.Tables.Select(item => $"using {item.Namespace};").Distinct().Join()}}
			
			namespace {{Database.Namespace}}
			{
				public partial class {{Database.DatabaseName}}
				{
			{{Database.Tables.Select(item => $$"""public List<{{item.TableName}}> {{item.TableName}}Table {get;set;}""").Join().Indent(2)}}

					public {{Database.DatabaseName}}()
					{
			{{Database.Tables.Select(item => $$"""{{item.TableName}}Table = new List<{{item.TableName}}>();""").Join().Indent(3)}}
					}

				}
			}
			""";

			return source;
		}

		

		
		
	}
}