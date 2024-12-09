using DataModelLib.Schema;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class ColumnModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateTableModelProperties()
		{
			Column model;
			string source;

			model=new Column(new Table("ns","testDB","tbl1"),"FirstName", "string", false);
			
			source=model.GenerateTableModelProperties();

			Assert.IsTrue(source.Contains("public string FirstName"));
			Assert.IsTrue(source.Contains("get"));
			Assert.IsTrue(source.Contains("set"));
		}
		


	}
}