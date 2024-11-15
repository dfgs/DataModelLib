using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryExample
{
	public class Main
	{
		public void TestMain()
		{
			TestDatabaseModel testDatabase;

			testDatabase = new TestDatabaseModel(new TestDatabase());

			testDatabase.AddToPersonns(new Personn());
			testDatabase.RemoveFromPersonns2(new Personn());

		}

	}
}
