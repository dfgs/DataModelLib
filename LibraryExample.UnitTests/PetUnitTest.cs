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

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPets().ElementAt(1).Delete();
			models = testDatabaseModel.GetPets().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Turtle", models[1].Name);
		}

		[TestMethod]
		public void ShouldCascadeUpdatePeopleUsingNotNullableForeignKey()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPets().ElementAt(1).Delete();
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.IsTrue(models.All(item=>item.PetID==1));
		}
		[TestMethod]
		public void ShouldNotCascadeUpdatePeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPets().ElementAt(2).Delete();
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.AreEqual(1, models[0].PetID);
			Assert.AreEqual(1, models[1].PetID);
			Assert.AreEqual(2, models[2].PetID);
			Assert.AreEqual(2, models[3].PetID);
		}



		[TestMethod]
		public void ShouldNotGetOwners()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPets().ElementAt(2).GetOwners().ToArray();
			
			Assert.AreEqual(0, models.Length);
		}

		[TestMethod]
		public void ShouldGetOwners()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPets().ElementAt(0).GetOwners().ToArray();

			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
		}

		

	}
}