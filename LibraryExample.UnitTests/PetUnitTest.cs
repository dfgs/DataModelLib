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

	
				

	


	}
}