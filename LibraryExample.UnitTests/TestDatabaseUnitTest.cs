namespace LibraryExample.UnitTests
{
	[TestClass]
	public class TestDatabaseUnitTest
	{
		[TestMethod]
		public void ShouldGetAddressTable()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(3,models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("School", models[1].Street);
			Assert.AreEqual("Work", models[2].Street);
		}
		[TestMethod]
		public void ShouldGetAddressTableByPredicate()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddressTable(item=>item.Street=="Home").ToArray();
			Assert.AreEqual(1, models.Length);
			Assert.AreEqual("Home", models[0].Street);
		}
		[TestMethod]
		public void ShouldGetPersonnTable()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Bart", models[2].FirstName);
			Assert.AreEqual("Liza", models[3].FirstName);
		}

		[TestMethod]
		public void ShouldGetPersonnTableByPredicate()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPersonnTable(item=>item.FirstName=="Homer").ToArray();
			Assert.AreEqual(1, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
		}

		[TestMethod]
		public void ShouldGetPetTable()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPetTable().ToArray();
			Assert.AreEqual(3, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
			Assert.AreEqual("Dog", models[1].Name);
			Assert.AreEqual("Turtle", models[2].Name);
		}
		[TestMethod]
		public void ShouldGetPetTableByPredicate()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetPetTable(item=>item.Name=="Cat").ToArray();
			Assert.AreEqual(1, models.Length);
			Assert.AreEqual("Cat", models[0].Name);
		}

		[TestMethod]
		public void ShouldGetAddressByPK()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel model;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetAddress(1);
			Assert.AreEqual("Home", model.Street);
			model = testDatabaseModel.GetAddress(2);
			Assert.AreEqual("School", model.Street);
			model = testDatabaseModel.GetAddress(3);
			Assert.AreEqual("Work", model.Street);
		}
		[TestMethod]
		public void ShouldGetAddressByPredicate()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel model;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetAddress(item=>item.Street=="Home");
			Assert.AreEqual("Home", model.Street);
			model = testDatabaseModel.GetAddress(item => item.Street == "School"); 
			Assert.AreEqual("School", model.Street);
			model = testDatabaseModel.GetAddress(item => item.Street == "Work"); 
			Assert.AreEqual("Work", model.Street);
		}
		[TestMethod]
		public void ShouldGetPersonnByPredicate()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel model;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPersonn(item=>item.FirstName=="Homer");
			Assert.AreEqual("Homer", model.FirstName);
			model = testDatabaseModel.GetPersonn(item => item.FirstName == "Marje");
			Assert.AreEqual("Marje", model.FirstName);
			model = testDatabaseModel.GetPersonn(item => item.FirstName == "Bart");
			Assert.AreEqual("Bart", model.FirstName);
			model = testDatabaseModel.GetPersonn(item => item.FirstName == "Liza");
			Assert.AreEqual("Liza", model.FirstName);
		}

		[TestMethod]
		public void ShouldGetPetByPK()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel model;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPet(1);
			Assert.AreEqual("Cat", model.Name);
			model = testDatabaseModel.GetPet(2);
			Assert.AreEqual("Dog", model.Name);
			model = testDatabaseModel.GetPet(3);
			Assert.AreEqual("Turtle", model.Name);
		}
		[TestMethod]
		public void ShouldGetPetByPredicate()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel model;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			model = testDatabaseModel.GetPet(item=>item.Name=="Cat");
			Assert.AreEqual("Cat", model.Name);
			model = testDatabaseModel.GetPet(item => item.Name == "Dog");
			Assert.AreEqual("Dog", model.Name);
			model = testDatabaseModel.GetPet(item => item.Name == "Turtle");
			Assert.AreEqual("Turtle", model.Name);
		}


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
		public void ShouldRemoveAddress()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.RemoveAddress(testDatabaseModel.GetAddressTable().ElementAt(1) );
			models = testDatabaseModel.GetAddressTable().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Home", models[0].Street);
			Assert.AreEqual("Work", models[1].Street);
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

	}
}