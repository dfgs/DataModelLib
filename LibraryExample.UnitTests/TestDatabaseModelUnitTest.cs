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
		public void ShouldAddAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			testDatabaseModel.AddAddress(new Address(3, "UnitTest"));
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("School", models[1].Street);
			Assert.AreEqual("Work", models[2].Street);
			Assert.AreEqual("UnitTest", models[3].Street);
		}
		[TestMethod]
		public void ShouldNotAddAddressTwice()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			Address newAddress;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			newAddress = new Address(3, "UnitTest");
			testDatabaseModel.AddAddress(newAddress);
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(4, models.Length);
			testDatabaseModel.AddAddress(newAddress);
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(4, models.Length);
		}
		[TestMethod]
		public void ShouldRaiseAddressTableChangingOnAdd()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			Address? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddressTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index;  };

			testDatabaseModel.AddAddress(new Address(3, "UnitTest"));
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(4, models.Length);
			

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("UnitTest", changedItem.Street);
			Assert.AreEqual(TableChangedActions.Add, changedAction);
			Assert.AreEqual(3, changedIndex);
		}
		[TestMethod]
		public void ShouldRaiseAddressTableChangedOnAdd()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;
			Address? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddressTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };

			testDatabaseModel.AddAddress(new Address(3, "UnitTest"));
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(4, models.Length);


			Assert.IsNotNull(changedItem);
			Assert.AreEqual("UnitTest", changedItem.Street);
			Assert.AreEqual(TableChangedActions.Add, changedAction);
			Assert.AreEqual(3, changedIndex);
		}

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
		public void ShouldAddPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());


			testDatabaseModel.AddPersonn(new Personn(4, "Maggy", "Simpson", 4));
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(5, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Bart", models[2].FirstName);
			Assert.AreEqual("Liza", models[3].FirstName);
			Assert.AreEqual("Maggy", models[4].FirstName);
		}
		[TestMethod]
		public void ShouldNotAddPersonnTwice()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn newPersonn;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			newPersonn = new Personn(4, "Maggy", "Simpson", 4);
			testDatabaseModel.AddPersonn(newPersonn);
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(5, models.Length);
			testDatabaseModel.AddPersonn(newPersonn);
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(5, models.Length);

		}

		[TestMethod]
		public void ShouldRaisePersonnTableChangingOnAdd()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index;  };


			testDatabaseModel.AddPersonn(new Personn(4, "Maggy", "Simpson", 4));
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(5, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Maggy", changedItem.FirstName);
			Assert.AreEqual(TableChangedActions.Add, changedAction);
			Assert.AreEqual(4, changedIndex);
		}
		[TestMethod]
		public void ShouldRaisePersonnTableChangedOnAdd()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			Personn? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PersonnTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.AddPersonn(new Personn(4, "Maggy", "Simpson", 4));
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(5, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Maggy", changedItem.FirstName);
			Assert.AreEqual(TableChangedActions.Add, changedAction);
			Assert.AreEqual(4, changedIndex);
		}

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
		public void ShouldAddPet()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());


			testDatabaseModel.AddPet(new Pet(3, "Bird"));
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Dog", models[1].Name);
			Assert.AreEqual("Turtle", models[2].Name);
			Assert.AreEqual("Bird", models[3].Name);
		}
		[TestMethod]
		public void ShouldNotAddPetTwice()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet newPet;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			newPet = new Pet(3, "Bird");
			testDatabaseModel.AddPet(newPet);
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(4, models.Length);
			testDatabaseModel.AddPet(newPet);
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(4, models.Length);

		}

		[TestMethod]
		public void ShouldRaisePetTableChangingOnAdd()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PetTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.AddPet(new Pet(3, "Bird"));
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(4, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Bird", changedItem.Name);
			Assert.AreEqual(TableChangedActions.Add, changedAction);
			Assert.AreEqual(3, changedIndex);
		}
		[TestMethod]
		public void ShouldRaisePetTableChangedOnAdd()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PetTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };


			testDatabaseModel.AddPet(new Pet(3, "Bird"));
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(4, models.Length);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Bird", changedItem.Name);
			Assert.AreEqual(TableChangedActions.Add, changedAction);
			Assert.AreEqual(3, changedIndex);
		}

		
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