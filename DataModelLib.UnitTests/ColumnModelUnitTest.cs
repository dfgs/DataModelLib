using DataModelLib.DataModels;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class ColumnModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateTableModelProperties()
		{
			ColumnModel model;
			string source;

			model=new ColumnModel("FirstName", "string", false,null);
			
			source=model.GenerateTableModelProperties();

			Assert.IsTrue(source.Contains("public string FirstName"));
			Assert.IsTrue(source.Contains("get"));
			Assert.IsTrue(source.Contains("set"));
		}
		[TestMethod]
		public void ShouldGenerateTableModelMethods()
		{
			ForeignKeyModel foreignKey;
			ColumnModel model;
			string source;

			foreignKey = new ForeignKeyModel("DeliveryAddress", "Address", "AddressID");
			model = new ColumnModel("PersonnAddressID", "string", false, foreignKey);

			source = model.GenerateTableModelMethods();

			Assert.IsTrue(source.Contains("public Address GetDeliveryAddress()"));
			
		}


	}
}