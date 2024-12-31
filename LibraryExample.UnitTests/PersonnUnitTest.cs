using LibraryExample.Models;
using DataModelLib.Common;

namespace LibraryExample.UnitTests
{
	[TestClass]
	public class PersonnUnitTest
	{
		[TestMethod]
		public void ShouldDelete()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };

			testDatabaseModel.GetPersonnTable().ElementAt(2).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(3, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Liza", models[2].FirstName);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Bart", changedItem.FirstName);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(2, changedIndex);
		}
		[TestMethod]
		public void ShouldReturnIsModelOf()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel model;
			Personn personn1, personn2;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			personn1 = new Personn(5, "FN1","LN1",1);
			personn2 = new Personn(6, "FN2","LN2",2);

			model = new PersonnModel(testDatabaseModel, personn1);
			Assert.IsTrue(model.IsModelOf(personn1));
			Assert.IsFalse(model.IsModelOf(personn2));
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
		public void ShouldGetSetProperty()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel model;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPersonn(1);
			model.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			Assert.AreEqual("Homer", model.FirstName);
			model.FirstName = "Homer2";
			Assert.AreEqual("Homer2", model.FirstName);
			Assert.AreEqual("FirstName", propertyName);
		}

	}
}