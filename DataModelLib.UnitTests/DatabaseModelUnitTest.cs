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
			model.TableModels.Add(new TableModel("ns", model.DatabaseClassName, "Personn", "People"));

			source = model.GenerateDatabaseConstructor();

			Assert.IsTrue(source.Contains("public MyDB()"));
			Assert.IsTrue(source.Contains("People = new List<Personn>();"));

		}

		[TestMethod]
		public void ShouldGenerateDatabaseModelClass()
		{
			DatabaseModel model;
			TableModel table;
			string source;

			model = new DatabaseModel("ns","MyDB");
			table = new TableModel("ns1", model.DatabaseClassName, "Personn", "People1");
			table.PrimaryKey = new ColumnModel("PersonnID", "byte", false);
			model.TableModels.Add(table);

			table = new TableModel("ns2", model.DatabaseClassName, "Personn", "People2");
			model.TableModels.Add(table);
			source = model.GenerateDatabaseModelClass();


			Assert.IsTrue(source.Contains("namespace ns"));
			Assert.IsTrue(source.Contains("using ns1"));
			Assert.IsTrue(source.Contains("using ns2"));
			Assert.IsTrue(source.Contains("public partial class MyDBModel"));
			Assert.IsTrue(source.Contains("public MyDBModel(MyDB DataSource)"));

			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetPeople1()"));
			Assert.IsTrue(source.Contains("public void AddToPeople1(Personn Item)"));
			Assert.IsTrue(source.Contains("public void RemoveFromPeople1(PersonnModel Item)"));

			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetPeople2()"));
			Assert.IsTrue(source.Contains("public void AddToPeople2(Personn Item)"));
			Assert.IsFalse(source.Contains("public void RemoveFromPeople2(PersonnModel Item)")); // no public key defined

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