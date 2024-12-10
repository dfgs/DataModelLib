using DataModelLib.Common.Schema;
using DataModelLib.SourceGenerator;
using System.Reflection;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class DatabaseModelSourceGeneratorUnitTest
	{
		[TestMethod]
		public void ShouldGenerateUsings()
		{
			DatabaseModelSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseModelSourceGenerator();

			database = new Database("ns", "MyDB");
			database.Tables.Add(new Table("ns1", database.DatabaseName, "Personn1"));
			database.Tables.Add(new Table("ns2", database.DatabaseName, "Personn2"));
			database.Tables.Add(new Table("ns2", database.DatabaseName, "Personn3"));

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("using DataModelLib.Common;"));
			Assert.IsTrue(source.Contains("using ns1"));
			Assert.IsTrue(source.Contains("using ns2"));

		}

		[TestMethod]
		public void ShouldGenerateClass()
		{
			DatabaseModelSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseModelSourceGenerator();

			database = new Database("ns", "MyDB");

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("namespace ns.Models"));
			Assert.IsTrue(source.Contains("public partial class MyDBModel"));
		}
		[TestMethod]
		public void ShouldGenerateEvents()
		{
			DatabaseModelSourceGenerator sourceGenerator;
			Database database;
			Table table;
			string source;

			sourceGenerator = new DatabaseModelSourceGenerator();

			database = new Database("ns", "MyDB");

			table = new Table("ns1", database.DatabaseName, "Personn1");
			table.PrimaryKey = new Column(table, "PersonnID", "byte", false);
			database.Tables.Add(table);

			table = new Table("ns2", database.DatabaseName, "Personn2"); // no PK
			database.Tables.Add(table);

			table = new Table("ns", "MyDB", "Address");
			table.PrimaryKey = new Column(table, "AddressID", "byte", false);
			database.Tables.Add(table);


			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("public event TableChangedEventHandler<Address> AddressTableChanged;"));
			Assert.IsTrue(source.Contains("public event RowChangedEventHandler<Address> AddressRowChanged;"));
			Assert.IsTrue(source.Contains("public event TableChangedEventHandler<Personn1> Personn1TableChanged;"));
			Assert.IsTrue(source.Contains("public event RowChangedEventHandler<Personn1> Personn1RowChanged;"));
			Assert.IsTrue(source.Contains("public event TableChangedEventHandler<Personn2> Personn2TableChanged;"));
			Assert.IsTrue(source.Contains("public event RowChangedEventHandler<Personn2> Personn2RowChanged;"));

		}


		[TestMethod]
		public void ShouldGenerateMethods()
		{
			DatabaseModelSourceGenerator sourceGenerator;
			Database database;
			Table table;
			string source;

			sourceGenerator = new DatabaseModelSourceGenerator();

			database = new Database("ns", "MyDB");

			table = new Table("ns1", database.DatabaseName, "Personn1");
			table.PrimaryKey = new Column(table, "PersonnID", "byte", false);
			database.Tables.Add(table);

			table = new Table("ns2", database.DatabaseName, "Personn2"); // no PK
			database.Tables.Add(table);

			table = new Table("ns", "MyDB", "Address");
			table.PrimaryKey = new Column(table, "AddressID", "byte", false);
			database.Tables.Add(table);


			source = sourceGenerator.GenerateSource(database);

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


			Assert.IsTrue(source.Contains("public AddressModel GetAddress(byte AddressID)"));
			Assert.IsTrue(source.Contains("public AddressModel GetAddress(Func<Address,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public IEnumerable<AddressModel> GetAddressTable()"));
			Assert.IsTrue(source.Contains("public IEnumerable<AddressModel> GetAddressTable(Func<Address,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public void AddAddress(Address Item)"));
			Assert.IsTrue(source.Contains("public void RemoveAddress(AddressModel Item)"));

		}

		[TestMethod]
		public void ShouldGenerateConstructor()
		{
			DatabaseModelSourceGenerator sourceGenerator;
			Database database;
			string source;

			sourceGenerator = new DatabaseModelSourceGenerator();

			database = new Database("ns", "MyDB");
			database.Tables.Add(new Table("ns", database.DatabaseName, "Personn"));

			source = sourceGenerator.GenerateSource(database);

			Assert.IsTrue(source.Contains("public MyDBModel(MyDB DataSource)"));

		}






	}
}