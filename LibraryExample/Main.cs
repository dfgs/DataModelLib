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

			testDatabase.GetAddress().First().Delete();
			testDatabase.GetPersonn();
			testDatabase.GetPersonn().First().FirstName = "FirstName";
			testDatabase.GetPersonn().First().GetDeliveryAddress().Number = 15;
			//testDatabase.GetPeople().First().GetBillingAddress().Number = 24;

			testDatabase.AddPersonn(new Personn(1,"Homer","Simpson",40));
			testDatabase.RemoveAddress(testDatabase.GetAddress().First());

			
		}

	}
}
