using DataModelLib.DataModels;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class TableModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateDatabaseProperties()
		{
			TableModel model;
			string source;

			model = new TableModel("ns","Personn","People");
			source=model.GenerateDatabaseProperties();
			
			Assert.IsTrue(source.Contains("public List<Personn> People"));
		}
		[TestMethod]
		public void ShouldGenerateDatabaseConstructor()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "Personn", "People");
			source = model.GenerateDatabaseConstructor();

			Assert.IsTrue(source.Contains("People = new List<Personn>();"));
		}
		[TestMethod]
		public void ShouldGenerateDatabaseModelMethods()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "Personn", "People");
			source = model.GenerateDatabaseModelMethods();

			Assert.IsTrue(source.Contains("public IEnumerable<Personn> GetPeople()"));
		}

	}
}