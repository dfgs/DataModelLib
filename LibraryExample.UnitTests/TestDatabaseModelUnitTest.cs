using LibraryExample.Models;
using DataModelLib;
using System.Reflection;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("TestDatabase"),Blueprint("DatabaseModel.UnitTest.*"),TestClass]
	public partial class TestDatabaseModelUnitTest
	{

		#region AddressTable
		
	
		[TestMethod]
		public void ShouldRemoveAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
		

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			testDatabaseModel.RemoveAddress(testDatabaseModel.GetAddressTable().ElementAt(1));
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("Work", models[1].Street);
		}
		[TestMethod]
		public void ShouldNotRemoveInvalidAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			AddressModel item;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			item = testDatabaseModel.GetAddressTable().ElementAt(1);
			testDatabaseModel.RemoveAddress(item);
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);
			testDatabaseModel.RemoveAddress(item);
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);
		}

		[TestMethod]
		public void ShouldRaiseAddressTableChangingOnRemove()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			Address? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddressTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };

			testDatabaseModel.RemoveAddress(testDatabaseModel.GetAddressTable().ElementAt(1));
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);


			Assert.IsNotNull(changedItem);
			Assert.AreEqual("School", changedItem.Street);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
		}
		[TestMethod]
		public void ShouldRaiseAddressTableChangedOnRemove()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			Address? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddressTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };

			testDatabaseModel.RemoveAddress(testDatabaseModel.GetAddressTable().ElementAt(1));
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);


			Assert.IsNotNull(changedItem);
			Assert.AreEqual("School", changedItem.Street);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
		}
		#endregion

		#region PersonnTable
	
	

		[TestMethod]
		public void ShouldRemovePersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			testDatabaseModel.RemovePersonn(testDatabaseModel.GetPersonnTable().ElementAt(2));
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(3, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Liza", models[2].FirstName);

		}

		[TestMethod]
		public void ShouldNotRemoveInvalidPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel item;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			item = testDatabaseModel.GetPersonnTable().ElementAt(2);
			testDatabaseModel.RemovePersonn(item);
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(3, models.Length);
			testDatabaseModel.RemovePersonn(item);
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(3, models.Length);

		}


		[TestMethod]
		public void ShouldRaisePersonnTableChangingOnRemove()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.RemovePersonn(testDatabaseModel.GetPersonnTable().ElementAt(2));
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(3, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Bart", changedItem.FirstName);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(2, changedIndex);
		}
		[TestMethod]
		public void ShouldRaisePersonnTableChangedOnRemove()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.RemovePersonn(testDatabaseModel.GetPersonnTable().ElementAt(2));
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(3, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Bart", changedItem.FirstName);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(2, changedIndex);
		}

		#endregion

		#region PetTable
			
	
		[TestMethod]
		public void ShouldRemovePet()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			testDatabaseModel.RemovePet(testDatabaseModel.GetPetTable().ElementAt(1));
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Turtle", models[1].Name);
		}
		[TestMethod]
		public void ShouldNotRemoveInvalidPet()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			PetModel item;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			item = testDatabaseModel.GetPetTable().ElementAt(1);
			testDatabaseModel.RemovePet(item);
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);
			testDatabaseModel.RemovePet(item);
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);

		}
		[TestMethod]
		public void ShouldRaisePetTableChangingOnRemoe()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PetTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.RemovePet(testDatabaseModel.GetPetTable().ElementAt(1));
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Dog", changedItem.Name);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
		}
		[TestMethod]
		public void ShouldRaisePetTableChangedOnRemoe()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PetTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.RemovePet(testDatabaseModel.GetPetTable().ElementAt(1));
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Dog", changedItem.Name);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
		}

		#endregion








	}
}