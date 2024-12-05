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
			model.TableModels.Add(new TableModel("ns", model.DatabaseName, "Personn"));

			source = model.GenerateDatabaseConstructor();

			Assert.IsTrue(source.Contains("public MyDB()"));
			Assert.IsTrue(source.Contains("PersonnTable = new List<Personn>();"));

		}

		[TestMethod]
		public void ShouldGenerateDatabaseModelClass()
		{
			DatabaseModel model;
			TableModel table;
			string source;

			model = new DatabaseModel("ns","MyDB");
			table = new TableModel("ns1", model.DatabaseName, "Personn1");
			table.PrimaryKey = new ColumnModel("PersonnID", "byte", false);
			model.TableModels.Add(table);

			table = new TableModel("ns2", model.DatabaseName, "Personn2"); // no PK
			model.TableModels.Add(table);
			source = model.GenerateDatabaseModelClass();


			Assert.IsTrue(source.Contains("namespace ns"));
			Assert.IsTrue(source.Contains("using ns1"));
			Assert.IsTrue(source.Contains("using ns2"));
			Assert.IsTrue(source.Contains("public partial class MyDBModel"));
			Assert.IsTrue(source.Contains("public MyDBModel(MyDB DataSource)"));

			Assert.IsTrue(source.Contains("public Personn1Model GetPersonn1(byte PersonnID)"));
			Assert.IsTrue(source.Contains("public Personn1Model GetPersonn1(Func<Personn1,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public IEnumerable<Personn1Model> GetPersonn1Table()"));
			Assert.IsTrue(source.Contains("public IEnumerable<Personn1Model> GetPersonn1Table(Func<Personn1,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public void AddPersonn1(Personn1 Item)"));
			Assert.IsTrue(source.Contains("public void RemovePersonn1(Personn1Model Item)"));

			Assert.IsFalse(source.Contains("public Personn2Model GetPersonn2(byte PersonnID)")); // no PK
			Assert.IsTrue(source.Contains("public Personn2Model GetPersonn2(Func<Personn2,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public IEnumerable<Personn2Model> GetPersonn2Table()"));
			Assert.IsTrue(source.Contains("public IEnumerable<Personn2Model> GetPersonn2Table(Func<Personn2,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public void AddPersonn2(Personn2 Item)"));
			Assert.IsFalse(source.Contains("public void RemovePersonn2(Personn2Model Item)")); // no public key defined

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