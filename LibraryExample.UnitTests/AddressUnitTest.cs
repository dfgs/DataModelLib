namespace LibraryExample.UnitTests
{
	[TestClass]
	public class AddressUnitTest
	{
		[TestMethod]
		public void ShouldDelete()
		{
			TestDatabaseModel testDatabaseModel;
			AddressModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddresses().ElementAt(1).Delete();
			models = testDatabaseModel.GetAddresses().ToArray();
			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Home", models[0].Street);
		}
		[TestMethod]
		public void ShouldCascadeDeletePeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddresses().ElementAt(0).Delete();
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.AreEqual(0, models.Length);
		}
		[TestMethod]
		public void ShouldNotCascadeDeletePeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddresses().ElementAt(1).Delete();
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.AreEqual(4, models.Length);
		}
		[TestMethod]
		public void ShouldCascadeUpdatePeopleUsingNullableForeignKey()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddresses().ElementAt(1).Delete();
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.IsTrue(models.All(item=>item.BillingAddressID==null));
		}
		[TestMethod]
		public void ShouldNotCascadeUpdatePeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetAddresses().ElementAt(2).Delete();
			models = testDatabaseModel.GetPeople().ToArray();
			Assert.IsNotNull(models[0].BillingAddressID);
			Assert.IsNotNull(models[1].BillingAddressID);
		}



		[TestMethod]
		public void ShouldNotGetBilledPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddresses().ElementAt(0).GetBilledPeople().ToArray();
			
			Assert.AreEqual(0, models.Length);
		}

		[TestMethod]
		public void ShouldGetBilledPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddresses().ElementAt(1).GetBilledPeople().ToArray();

			Assert.AreEqual(2, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
		}

		[TestMethod]
		public void ShouldNotGetDeliveredPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddresses().ElementAt(1).GetDeliveredPeople().ToArray();

			Assert.AreEqual(0, models.Length);
		}

		[TestMethod]
		public void ShouldGetDeliveredPeople()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			models = testDatabaseModel.GetAddresses().ElementAt(0).GetDeliveredPeople().ToArray();

			Assert.AreEqual(4, models.Length);
			Assert.AreEqual("Homer", models[0].FirstName);
			Assert.AreEqual("Marje", models[1].FirstName);
			Assert.AreEqual("Bart", models[2].FirstName);
			Assert.AreEqual("Liza", models[3].FirstName);
		}


	}
}