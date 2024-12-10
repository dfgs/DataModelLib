using DataModelLib.Schema;
using DataModelLib.SourceGenerator;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class DatabaseSourceGeneratorUnitTest
	{
		[TestMethod]
		public void ShouldGenerateUsings()
		{
			DatabaseSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseSourceGenerator();

			database = new Database("ns", "MyDB");
			database.Tables.Add(new Table("ns1", database.DatabaseName, "Personn1"));
			database.Tables.Add(new Table("ns2", database.DatabaseName, "Personn2"));
			database.Tables.Add(new Table("ns2", database.DatabaseName, "Personn3"));

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("using ns1;"));
			Assert.IsTrue(source.Contains("using ns2;"));

		}

		[TestMethod]
		public void ShouldGenerateClass()
		{
			DatabaseSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseSourceGenerator();

			database = new Database("ns", "MyDB");

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("namespace ns"));
			Assert.IsTrue(source.Contains("public partial class MyDB"));
		}

		[TestMethod]
		public void ShouldGenerateProperties()
		{
			DatabaseSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseSourceGenerator();

			database = new Database("ns", "MyDB");
			database.Tables.Add(new Table("ns", database.DatabaseName, "Personn"));

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("public List<Personn> PersonnTable {get;set;}"));

		}

		[TestMethod]
		public void ShouldGenerateConstructor()
		{
			DatabaseSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseSourceGenerator();

			database = new Database("ns", "MyDB");
			database.Tables.Add(new Table("ns", database.DatabaseName, "Personn"));

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("public MyDB()"));
			Assert.IsTrue(source.Contains("PersonnTable = new List<Personn>();"));

		}


		



	}
}