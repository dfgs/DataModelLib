using LibraryExample.Models;
using DataModelLib;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("Address"), Blueprint("TableModel.UnitTest.*"), MockCount(4), TestClass]
	public partial class AddressUnitTest
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
		public void ShouldRaiseDeliveredPeopleChangedWhenRemovingPersonn()
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
		public void ShouldRaiseBilledPeopleChangedWhenRemovingPersonn()
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

		
		
		[TestMethod]
		public void ShouldRaiseDeliveredPeopleChangedWhenAddingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn = null;
			TableChangedActions? action = null;
			int? index = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).DeliveredPeopleChanged += (item, a, i) => { personn = item; action = a; index = i; };
			testDatabaseModel.GetAddress(2).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.AddPersonn(new Personn(5, "Ned", "Flander", 50) { DeliveryAddressID = 1 });

			Assert.IsNotNull(personn);
			Assert.IsNotNull(action);
			Assert.IsNotNull(index);

			Assert.AreEqual("Ned", personn.FirstName);
			Assert.AreEqual(4, index);
			Assert.AreEqual(TableChangedActions.Add, action);

		}

		[TestMethod]
		public void ShouldRaiseBilledPeopleChangedWhenAddingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn = null;
			TableChangedActions? action = null;
			int? index = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).BilledPeopleChanged += (item, a, i) => { personn = item; action = a; index = i;  };

			testDatabaseModel.AddPersonn(new Personn(5, "Ned", "Flander", 50) { BillingAddressID = 3 });

			Assert.IsNotNull(personn);
			Assert.IsNotNull(action);
			Assert.IsNotNull(index);

			Assert.AreEqual("Ned", personn.FirstName);
			Assert.AreEqual(0, index);
			Assert.AreEqual(TableChangedActions.Add, action);

		}


		[TestMethod]
		public void ShouldRaiseDeliveredPeopleWhenChangingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).DeliveredPeopleChanged += (item, a, i) => { personn1 = item; action1 = a; index1 = i; };
			testDatabaseModel.GetAddress(2).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).DeliveredPeopleChanged += (item, a, i) => { personn2 = item; action2 = a; index2 = i; };

			testDatabaseModel.GetPersonn(1).DeliveryAddressID = 3;

			Assert.IsNotNull(personn1);
			Assert.IsNotNull(action1);
			Assert.IsNotNull(index1);
			Assert.AreEqual("Homer", personn1.FirstName);
			Assert.AreEqual(0, index1);
			Assert.AreEqual(TableChangedActions.Remove, action1);


			Assert.IsNotNull(personn2);
			Assert.IsNotNull(action2);
			Assert.IsNotNull(index2);
			Assert.AreEqual("Homer", personn2.FirstName);
			Assert.AreEqual(0, index2);
			Assert.AreEqual(TableChangedActions.Add, action2);
		}

		[TestMethod]
		public void ShouldRaiseDeliveredPeopleWhenChangingPersonnWithNonExistingID()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).DeliveredPeopleChanged += (item, a, i) => { personn1 = item; action1 = a; index1 = i; };
			testDatabaseModel.GetAddress(2).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).DeliveryAddressID = 33; // doesn't exist

			Assert.IsNotNull(personn1);
			Assert.IsNotNull(action1);
			Assert.IsNotNull(index1);
			Assert.AreEqual("Homer", personn1.FirstName);
			Assert.AreEqual(0, index1);
			Assert.AreEqual(TableChangedActions.Remove, action1);


			Assert.IsNull(personn2);
			Assert.IsNull(action2);
			Assert.IsNull(index2);
			
		}

		[TestMethod]
		public void ShouldRaiseDeliveredPeopleWhenKeepingSamePropertyValue()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).DeliveredPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).DeliveryAddressID = 1;

			Assert.IsNull(personn1);
			Assert.IsNull(action1);
			Assert.IsNull(index1);

			Assert.IsNull(personn2);
			Assert.IsNull(action2);
			Assert.IsNull(index2);

		}

		[TestMethod]
		public void ShouldRaiseBilledPeopleWhenChangingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).BilledPeopleChanged += (item, a, i) => { personn1 = item; action1 = a; index1 = i;  };
			testDatabaseModel.GetAddress(3).BilledPeopleChanged += (item, a, i) => { personn2 = item; action2 = a; index2 = i; };

			testDatabaseModel.GetPersonn(1).BillingAddressID = 3;

			Assert.IsNotNull(personn1);
			Assert.IsNotNull(action1);
			Assert.IsNotNull(index1);
			Assert.AreEqual("Homer", personn1.FirstName);
			Assert.AreEqual(0, index1);
			Assert.AreEqual(TableChangedActions.Remove, action1);


			Assert.IsNotNull(personn2);
			Assert.IsNotNull(action2);
			Assert.IsNotNull(index2);
			Assert.AreEqual("Homer", personn2.FirstName);
			Assert.AreEqual(0, index2);
			Assert.AreEqual(TableChangedActions.Add, action2);
		}

		[TestMethod]
		public void ShouldRaiseBilledPeopleWhenChangingPersonn2()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).BilledPeopleChanged += (item, a, i) => { personn2 = item; action2 = a; index2 = i; };

			// no previous billing ID was defined
			testDatabaseModel.GetPersonn(3).BillingAddressID = 3;

			Assert.IsNull(personn1);
			Assert.IsNull(action1);
			Assert.IsNull(index1);
		


			Assert.IsNotNull(personn2);
			Assert.IsNotNull(action2);
			Assert.IsNotNull(index2);
			Assert.AreEqual("Bart", personn2.FirstName);
			Assert.AreEqual(0, index2);
			Assert.AreEqual(TableChangedActions.Add, action2);
		}

		[TestMethod]
		public void ShouldRaiseBilledPeopleWhenChangingPersonnWithNullID()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).BilledPeopleChanged += (item, a, i) => { personn1 = item; action1 = a; index1 = i; };
			testDatabaseModel.GetAddress(3).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).BillingAddressID = null;

			Assert.IsNotNull(personn1);
			Assert.IsNotNull(action1);
			Assert.IsNotNull(index1);
			Assert.AreEqual("Homer", personn1.FirstName);
			Assert.AreEqual(0, index1);
			Assert.AreEqual(TableChangedActions.Remove, action1);


			Assert.IsNull(personn2);
			Assert.IsNull(action2);
			Assert.IsNull(index2);
			
		}


		[TestMethod]
		public void ShouldRaiseBilledPeopleWhenKeepingSamePropertyValue()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddress(1).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(2).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetAddress(3).BilledPeopleChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).BillingAddressID = 2;

			Assert.IsNull(personn1);
			Assert.IsNull(action1);
			Assert.IsNull(index1);

			Assert.IsNull(personn2);
			Assert.IsNull(action2);
			Assert.IsNull(index2);

		}

		


	}
}