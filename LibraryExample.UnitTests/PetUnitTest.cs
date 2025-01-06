using LibraryExample.Models;
using DataModelLib.Common;
using System.Reflection;

namespace LibraryExample.UnitTests
{
	[TestClass]
	public class PetUnitTest
	{
		[TestMethod]
		public void ShouldReturnToString()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel? pet;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			pet = testDatabaseModel.GetPetTable().ElementAt(0);

			Assert.IsNotNull(pet);
			Assert.AreEqual("Cat", pet.ToString());
		}

		[TestMethod]
		public void ShouldDelete()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;
			int eventCount = 0;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PetTableChanging += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; eventCount++; };
			testDatabaseModel.PetTableChanged += (item, action, index) => { Assert.AreEqual(changedItem, item); Assert.AreEqual(changedAction, action); Assert.AreEqual(changedIndex, index); ; eventCount++; };

			testDatabaseModel.GetPetTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Turtle", models[1].Name);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Dog", changedItem.Name);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
			Assert.AreEqual(2, eventCount);
		}

		[TestMethod]
		public void ShouldReturnIsModelOf()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel model;
			Pet pet1, pet2;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			pet1 = new Pet(5, "pet1");
			pet2 = new Pet(6, "pet2");

			model = new PetModel(testDatabaseModel, pet1);
			Assert.IsTrue(model.IsModelOf(pet1));
			Assert.IsFalse(model.IsModelOf(pet2));
		}


		[TestMethod]
		public void ShouldCascadeUpdatePersonnUsingNotNullableForeignKey()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel updatedForeignItem;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			updatedForeignItem = testDatabaseModel.GetPersonn(3);
			Assert.AreNotEqual(1, updatedForeignItem.PetID);
			updatedForeignItem.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			testDatabaseModel.GetPetTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.IsTrue(models.All(item=>item.PetID==1));
			Assert.AreEqual(1, updatedForeignItem.PetID);
			Assert.AreEqual("PetID", propertyName);



		}
		[TestMethod]
		public void ShouldNotCascadeUpdatePersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel updatedForeignItem;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			updatedForeignItem = testDatabaseModel.GetPersonn(3);
			updatedForeignItem.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			testDatabaseModel.GetPetTable().ElementAt(2).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(1, models[0].PetID);
			Assert.AreEqual(1, models[1].PetID);
			Assert.AreEqual(2, models[2].PetID);
			Assert.AreEqual(2, models[3].PetID);
			Assert.IsNull( propertyName);

		}



		[TestMethod]
		public void ShouldNotGetOwners()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPetTable().ElementAt(2).GetOwners().ToArray();
			
			Assert.AreEqual(0, models.Length);
		}

		[TestMethod]
		public void ShouldGetOwners()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPetTable().ElementAt(0).GetOwners().ToArray();

			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
		}

		[TestMethod]
		public void ShouldGetSetProperty()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel model;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPet(1);
			model.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			Assert.AreEqual("Cat", model.Name);
			model.Name = "Cat2";
			Assert.AreEqual("Cat2", model.Name);
			Assert.AreEqual("Name", propertyName);
		}

		[TestMethod]
		public void ShouldRaiseOwnersChangedWhenRemovingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn = null;
			TableChangedActions? action = null;
			int? index = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPet(1).OwnersChanged+= (item, a, i) => { personn = item; action = a; index = i; };
			testDatabaseModel.GetPet(2).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(3).OwnersChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).Delete();

			Assert.IsNotNull(personn);
			Assert.IsNotNull(action);
			Assert.IsNotNull(index);

			Assert.AreEqual("Homer", personn.FirstName);
			Assert.AreEqual(0, index);
			Assert.AreEqual(TableChangedActions.Remove, action);

		}
		[TestMethod]
		public void ShouldRaiseOwnersChangedWhenAddingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn = null;
			TableChangedActions? action = null;
			int? index = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPet(1).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(2).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(3).OwnersChanged += (item, a, i) => { personn = item; action = a; index = i;  };

			testDatabaseModel.AddPersonn(new Personn(5,"Ned","Flander",50) { PetID=3});

			Assert.IsNotNull(personn);
			Assert.IsNotNull(action);
			Assert.IsNotNull(index);

			Assert.AreEqual("Ned", personn.FirstName);
			Assert.AreEqual(0, index);
			Assert.AreEqual(TableChangedActions.Add, action);

		}

	}
}