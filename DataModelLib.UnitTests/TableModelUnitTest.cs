using DataModelLib.DataModels;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class TableModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateGet()
		{
			TableModel model;
			string source;

			model = new TableModel("ns","Table1","PropertyName");
			source=model.GenerateDatabaseModelSource();
			
			Assert.IsTrue(source.Contains("public IEnumerable<string> GetTable1()"));
		}

		[TestMethod]
		public void ShouldGenerateAddTo()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "Table1", "PropertyName");
			source = model.GenerateDatabaseModelSource();

			Assert.IsTrue(source.Contains("public void AddToTable1(string Item)"));
		}

		[TestMethod]
		public void ShouldNotGenerateAddTo()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "Table1", "PropertyName");
			source = model.GenerateDatabaseModelSource();

			Assert.IsFalse(source.Contains("AddTo"));
		}

		[TestMethod]
		public void ShouldGenerateRemoveFrom()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "Table1", "PropertyName");
			source = model.GenerateDatabaseModelSource();

			Assert.IsTrue(source.Contains("public void RemoveFromTable1(string Item)"));
		}

		[TestMethod]
		public void ShouldNotGenerateRemoveFrom()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "Table1", "PropertyName");
			source = model.GenerateDatabaseModelSource();

			Assert.IsFalse(source.Contains("RemoveFrom"));
		}

	}
}