using LibraryExample.Models;
using DataModelLib;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("Personn"), Blueprint("TableModel.UnitTest.*"), MockCount(10), TestClass]
	public partial class PersonnUnitTest
	{
		[TestMethod]
		public void ShouldReturnToString()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel? personn;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			personn = testDatabaseModel.GetPersonnTable().ElementAt(0);

			Assert.IsNotNull(personn);
			Assert.AreEqual("Homer Simpson", personn.ToString());
		}

	

		[TestMethod]
		public void ShouldGetDeliveryAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel? address;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			address=testDatabaseModel.GetPersonnTable().ElementAt(0).GetDeliveryAddress();
			
			Assert.IsNotNull(address);
			Assert.AreEqual("Home", address.Street);
		}

		[TestMethod]
		public void ShouldGetBillingAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel? address;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			address = testDatabaseModel.GetPersonnTable().ElementAt(0).GetBillingAddress();

			Assert.IsNotNull(address);
			Assert.AreEqual("School", address.Street);
		}

		[TestMethod]
		public void ShouldNotGetBillingAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel? address;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			address = testDatabaseModel.GetPersonnTable().ElementAt(2).GetBillingAddress();

			Assert.IsNull(address);
		}

		
		

		[TestMethod]
		public void ShouldRaiseDeliveryAddressChangedEvent()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel model;
			bool triggered = false;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPersonn(1);
			model.DeliveryAddressChanged += (_, e) => { triggered=true; };
			model.DeliveryAddressID = 4;
			Assert.IsTrue(triggered);
		}
		[TestMethod]
		public void ShouldRaiseBillingAddressChangedEvent()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel model;
			bool triggered = false;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPersonn(1);
			model.BillingAddressChanged += (_, e) => { triggered = true; };
			model.BillingAddressID= 4;
			Assert.IsTrue(triggered);
		}
		[TestMethod]
		public void ShouldRaisePreferedPetChangedEvent()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel model;
			bool triggered = false;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPersonn(1);
			model.PreferedPetChanged += (_, e) => { triggered = true; };
			model.PetID = 4;

			Assert.IsTrue(triggered);
		}
	}
}