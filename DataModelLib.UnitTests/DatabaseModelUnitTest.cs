using DataModelLib.DataModels;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class DatabaseModelUnitTest
	{

		[TestMethod]
		public void ShouldGenerateDatabaseClass()
		{
			DatabaseModel model;
			string source;

			model = new DatabaseModel("ns", "MyDB");
			source = model.GenerateDatabaseClass();


			Assert.IsTrue(source.Contains("namespace ns"));
			Assert.IsTrue(source.Contains("public partial class MyDB"));
			Assert.IsTrue(source.Contains("public MyDB()"));
		}

		[TestMethod]
		public void ShouldGenerateDatabaseConstructor()
		{
			DatabaseModel model;
			string source;

			model = new DatabaseModel("ns", "MyDB");
			model.TableModels.Add(new TableModel("ns", "Personn", "People"));

			source = model.GenerateDatabaseConstructor();

			Assert.IsTrue(source.Contains("public MyDB()"));
			Assert.IsTrue(source.Contains("People = new List<Personn>();"));

		}

		[TestMethod]
		public void ShouldGenerateDatabaseModelClass()
		{
			DatabaseModel model;
			string source;

			model = new DatabaseModel("ns","MyDB");
			source=model.GenerateDatabaseModelClass();


			Assert.IsTrue(source.Contains("namespace ns"));
			Assert.IsTrue(source.Contains("public partial class MyDBModel"));
			Assert.IsTrue(source.Contains("public MyDBModel(MyDB DataSource)"));

		}
		[TestMethod]
		public void ShouldGenerateDatabaseModelConstructor()
		{
			DatabaseModel model;
			string source;

			model = new DatabaseModel("ns", "MyDB");
			source = model.GenerateDatabaseModelConstructor();

			Assert.IsTrue(source.Contains("public MyDBModel(MyDB DataSource)"));

		}
	}
}