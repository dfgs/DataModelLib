using System;
using System.Collections.Generic;

namespace LibraryExample
{
	[DataModelGenerator.Database]
	public class TestDatabase
	{
		[DataModelGenerator.Table]
		public List<Personn> Personns { get; set; }

		
		public List<Personn> PersonnsNoAttribute { get; set; }

		[DataModelGenerator.Table]
		public List<Personn> Personns2 { get; set; }


		public TestDatabase()
		{
			Personns = new List<Personn>();
			Personns2 = new List<Personn>();
			PersonnsNoAttribute = new List<Personn>();
		}

	}
}
