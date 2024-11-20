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

			testDatabase.GetAddresses().First().Delete();
			testDatabase.GetPeople();
			testDatabase.GetPeople().First().FirstName = "FirstName";

			testDatabase.AddToPeople(new Personn("Homer","Simpson",40));
			testDatabase.RemoveFromAddresses(testDatabase.GetAddresses().First());

			
		}

	}
}
