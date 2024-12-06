using DataModelGenerator;
using System.Reflection;

namespace LibraryExample.UnitTests
{
	[TestClass]
	public class PetUnitTest
	{
		[TestMethod]
		public void ShouldDelete()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;
			Pet? changedItem = null;
			int changedIndex = -1;
			TableChangedActions? changedAction = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.PetTableChanged += (item, action, index) => { changedItem = item; changedAction = action; changedIndex = index; };

			testDatabaseModel.GetPetTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Turtle", models[1].Name);

			Assert.IsNotNull(changedItem);
			Assert.AreEqual("Dog", changedItem.Name);
			Assert.AreEqual(TableChangedActions.Remove, changedAction);
			Assert.AreEqual(1, changedIndex);
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

	}
}