using DataModelLib.DataModels;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class DatabaseModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateCode()
		{
			DatabaseModel model;
			string source;

			model = new DatabaseModel("ns1","db1");
			source=model.GenerateCode();


			Assert.IsTrue(source.Contains("namespace ns1"));
			Assert.IsTrue(source.Contains("public partial class db1Model"));

		}

	}
}