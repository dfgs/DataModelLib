using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExample.UnitTests
{
	public static class Utils
	{
		public static TestDatabase CreateTestDatabase()
		{
			TestDatabase testDatabase = new TestDatabase();

			testDatabase.People.Add(new Personn(1, "Homer", "Simpson", 40) {DeliveryAddressID=1,BillingAddressID=2 });
			testDatabase.People.Add(new Personn(2,"Marje", "Simpson", 40) { DeliveryAddressID = 1, BillingAddressID = 2 });
			testDatabase.People.Add(new Personn(3,"Bart", "Simpson", 10) { DeliveryAddressID = 1 });
			testDatabase.People.Add(new Personn(4,"Liza", "Simpson", 9) { DeliveryAddressID = 1 });

			testDatabase.Addresses.Add(new Address(1, "Home") { Number = 123 });
			testDatabase.Addresses.Add(new Address(2, "School") { Number = 44 });

			return testDatabase;
		}


	}
}
