using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryExample
{
	public class Main
	{
		public void TestMain()
		{
			TestDatabaseModel testDatabase;

			testDatabase = new TestDatabaseModel(new TestDatabase());

			testDatabase.GetAddresses();
			testDatabase.GetPeople();

			//testDatabase.AddToPeople(new Personn());
			//testDatabase.RemoveFromPeople(new Personn());

			
		}

	}
}
