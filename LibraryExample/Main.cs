using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryExample.Models;

namespace LibraryExample
{
	public class Main
	{
		public void TestMain()
		{
			TestDatabaseModel testDatabase;

			testDatabase = new TestDatabaseModel(new TestDatabase());

			testDatabase.GetAddressTable().First().Delete();
			testDatabase.GetPersonnTable();
			testDatabase.GetPersonnTable().First().FirstName = "FirstName";
			testDatabase.GetPersonnTable().First().GetDeliveryAddress().Number = 15;
			//testDatabase.GetPeople().First().GetBillingAddress().Number = 24;

			testDatabase.AddPersonn(new Personn(1, "Homer", "Simpson", 40));
			testDatabase.RemoveAddress(testDatabase.GetAddressTable().First());

		}
			
	


	}
}
