using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModelLib.Schema
{
	public class Database : SchemaObject
	{


		public string Namespace
		{
			get;
			private set;
		}
		public string DatabaseName
		{
			get;
			private set;
		}

		public List<Table> Tables
		{
			get;
			private set;
		}

		public Database(string Namespace,string DatabaseName) : base()
		{
			Tables = new List<Table>();
			this.Namespace = Namespace;this.DatabaseName = DatabaseName;
		}


	}
}
