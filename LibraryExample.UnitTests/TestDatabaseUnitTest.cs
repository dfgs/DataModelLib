namespace LibraryExample.UnitTests
{
	[TestClass]
	public class TestDatabaseUnitTest
	{
		[TestMethod]
		public void ShouldGetAddresses()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddresses().ToArray();
			Assert.AreEqual(3,models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("School", models[1].Street);
			Assert.AreEqual("Work", models[2].Street);
		}

		[TestMethod]
		public void ShouldGetPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Bart", models[2].FirstName);
			Assert.AreEqual("Liza", models[3].FirstName);
		}

		[TestMethod]
		public void ShouldGetPets()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPets().ToArray();
			Assert.AreEqual(3, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Dog", models[1].Name);
			Assert.AreEqual("Turtle", models[2].Name);
		}


		[TestMethod]
		public void ShouldAddToAddresses()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());

			testDatabaseModel.AddToAddresses(new Address(3, "UnitTest"));
			models = testDatabaseModel.GetAddresses().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("School", models[1].Street);
			Assert.AreEqual("Work", models[2].Street);
			Assert.AreEqual("UnitTest", models[3].Street);
		}


		[TestMethod]
		public void ShouldAddToPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddToPeople(new Personn(4, "Maggy", "Simpson", 4));
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.AreEqual(5, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Bart", models[2].FirstName);
			Assert.AreEqual("Liza", models[3].FirstName);
			Assert.AreEqual("Maggy", models[4].FirstName);
		}

		[TestMethod]
		public void ShouldAddToPet()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.AddToPets(new Pet(3, "Bird"));
			models = testDatabaseModel.GetPets().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Dog", models[1].Name);
			Assert.AreEqual("Turtle", models[2].Name);
			Assert.AreEqual("Bird", models[3].Name);
		}

		[TestMethod]
		public void ShouldRemoveFromAddresses()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.RemoveFromAddresses(testDatabaseModel.GetAddresses().ElementAt(1) );
			models = testDatabaseModel.GetAddresses().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("Work", models[1].Street);
		}


		[TestMethod]
		public void ShouldRemoveFromPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.RemoveFromPeople(testDatabaseModel.GetPeople().ElementAt(2));
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.AreEqual(3, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Liza", models[2].FirstName);
		}

		[TestMethod]
		public void ShouldRemoveFromPets()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.RemoveFromPets(testDatabaseModel.GetPets().ElementAt(1));
			models = testDatabaseModel.GetPets().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Turtle", models[1].Name);
		}

	}
}