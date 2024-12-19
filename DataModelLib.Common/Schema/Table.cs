using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DataModelLib.Common.Schema
{
	public class Table : SchemaObject
	{
		
		public string Namespace { get; private set; }
		public string DatabaseName { get; private set; }
		public string TableName { get; private set; }


		public Column? PrimaryKey
		{
			get; 
			set;
		}

		public List<Column> Columns
		{
			get;
			private set;
		}
		public List<Relation> Relations
		{
			get;
			private set;
		}

		public Table(string Namespace, string DatabaseName, string TableName) : base()
		{
			this.Namespace = Namespace; this.DatabaseName = DatabaseName; this.TableName = TableName;
			this.Columns= new List<Column>();
			this.Relations = new List<Relation>();
		}
		public override string ToString()
		{
			return TableName;
		}





	}
}
