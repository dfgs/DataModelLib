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

			model=new ColumnModel("FirstName", "string", false);
			
			source=model.GenerateTableModelProperties();

			Assert.IsTrue(source.Contains("public string FirstName"));
			Assert.IsTrue(source.Contains("get"));
			Assert.IsTrue(source.Contains("set"));
		}
		


	}
}