using LibraryExample.Models;
using DataModelLib;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("Personn"), Blueprint("TableModel.UnitTest.*"), MockCount(10), TestClass]
	public partial class PersonnUnitTest
	{
		[TestMethod]
		public void ShouldReturnToString()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel? personn;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			personn = testDatabaseModel.GetPersonnTable().ElementAt(0);

			Assert.IsNotNull(personn);
			Assert.AreEqual("Homer Simpson", personn.ToString());
		}

		

		
	}
}