using LibraryExample.Models;
using DataModelLib.Common;

namespace LibraryExample.UnitTests
{
	[TestClass]
	public class AddressUnitTest
	{
		[TestMethod]
		public void ShouldReturnToString()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel? address;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			address = testDatabaseModel.GetPersonnTable().ElementAt(0).GetBillingAddress();

			Assert.IsNotNull(address);
			Assert.AreEqual("44 School", address.ToString());
		}


		[TestMethod]
		public void ShouldDelete()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			Address? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;
			int eventCount = 0;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddressTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index;eventCount++; };
			testDatabaseModel.AddressTableChanged += (item, action, index) => { Assert.AreEqual(changedItem , item); Assert.AreEqual(changedAction , action); Assert.AreEqual( changedIndex , index); ; eventCount++; };

			testDatabaseModel.GetAddressTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Home", models[0].Street);


			Assert.IsNotNull(changedItem);
			Assert.AreEqual("School", changedItem.Street);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
			Assert.AreEqual(2, eventCount);
		}

		[TestMethod]
		public void ShouldReturnIsModelOf()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel model;
			Address address1, address2;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			address1 = new Address(5,"ST1");
			address2 = new Address(6, "ST2");

			model = new AddressModel(testDatabaseModel, address1);
			Assert.IsTrue(model.IsModelOf(address1));
			Assert.IsFalse(model.IsModelOf(address2));
		}


		[TestMethod]
		public void ShouldCascadeDeletePersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem=null;
			TableChangedActions? changedAction;
			int changedCount = 0;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedCount++; };

			testDatabaseModel.GetAddressTable().ElementAt(0).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(0, models.Length);
			Assert.IsNotNull(changedItem);
			Assert.AreEqual(4,changedCount);
		}
		[TestMethod]
		public void ShouldNotCascadeDeletePersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem = null;
			TableChangedActions? changedAction;
			int changedCount = 0;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedCount++; };

			testDatabaseModel.GetAddressTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.IsNull(changedItem);
			Assert.AreEqual(0, changedCount);
		}
		[TestMethod]
		public void ShouldCascadeUpdatePersonnUsingNullableForeignKey()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel updatedForeignItem;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			updatedForeignItem = testDatabaseModel.GetPersonn(1);
			Assert.IsNotNull(updatedForeignItem.BillingAddressID);
			updatedForeignItem.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			testDatabaseModel.GetAddressTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.IsTrue(models.All(item=>item.BillingAddressID==null));
			Assert.AreEqual("BillingAddressID", propertyName);

		}
		[TestMethod]
		public void ShouldNotCascadeUpdatePersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel updatedForeignItem;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			updatedForeignItem = testDatabaseModel.GetPersonn(1);
			Assert.IsNotNull(updatedForeignItem.BillingAddressID);
			updatedForeignItem.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			testDatabaseModel.GetAddressTable().ElementAt(2).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.IsNotNull(models[0].BillingAddressID);
			Assert.IsNotNull(models[1].BillingAddressID);
			Assert.IsNull(propertyName);
		}



		[TestMethod]
		public void ShouldNotGetBilledPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddressTable().ElementAt(0).GetBilledPeople().ToArray();
			
			Assert.AreEqual(0, models.Length);
		}

		[TestMethod]
		public void ShouldGetBilledPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddressTable().ElementAt(1).GetBilledPeople().ToArray();

			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
		}

		[TestMethod]
		public void ShouldNotGetDeliveredPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddressTable().ElementAt(1).GetDeliveredPeople().ToArray();

			Assert.AreEqual(0, models.Length);
		}

		[TestMethod]
		public void ShouldGetDeliveredPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddressTable().ElementAt(0).GetDeliveredPeople().ToArray();

			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Bart", models[2].FirstName);
			Assert.AreEqual("Liza", models[3].FirstName);
		}

		[TestMethod]
		public void ShouldGetSetProperty()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel model;
			string? propertyName=null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetAddress(1);
			model.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			Assert.AreEqual("Home", model.Street);
			model.Street = "Home2";
			Assert.AreEqual("Home2", model.Street);
			Assert.AreEqual("Street",propertyName);	
		}

		[TestMethod]
		public void ShouldRaiseDeliveredPeopleChanged()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn=null;
			TableChangedActions? action=null; 
			int? index=null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).DeliveredPeopleChanged += (item, a, i) => { personn = item; action = a; index = i; };
			testDatabaseModel.GetAddress(2).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).Delete();

			Assert.IsNotNull(personn);
			Assert.IsNotNull(action);
			Assert.IsNotNull(index);

			Assert.AreEqual("Homer", personn.FirstName);
			Assert.AreEqual(0, index);
			Assert.AreEqual(TableChangedActions.Remove,action);

		}
		[TestMethod]
		public void ShouldRaiseBilledPeopleChanged()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn = null;
			TableChangedActions? action = null;
			int? index = null;
			
			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).BilledPeopleChanged += (item, a, i) => { personn = item; action = a; index = i;  };
			testDatabaseModel.GetAddress(3).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).Delete();

			Assert.IsNotNull(personn);
			Assert.IsNotNull(action);
			Assert.IsNotNull(index);

			Assert.AreEqual("Homer", personn.FirstName);
			Assert.AreEqual(0, index);
			Assert.AreEqual(TableChangedActions.Remove, action);
		}

	}
}