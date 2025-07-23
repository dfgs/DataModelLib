using LibraryExample.Models;
using DataModelLib;
using System.Reflection;
using System.Diagnostics.Tracing;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("Pet"), Blueprint("TableModel.UnitTest.*"), MockCount(6), TestClass]
	public partial class PetUnitTest
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



		

	


	}
}