using DataModelLib.Common.Schema;
using DataModelLib.SourceGenerator;
using System.Reflection;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class TableModelSourceGeneratorUnitTest
	{
		[TestMethod]
		public void ShouldGenerateUsings()
		{
			TableModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));

			source = sourceGenerator.GenerateSource(table);


			Assert.IsTrue(source.Contains("using DataModelLib.Common;"));

		}
		[TestMethod]
		public void ShouldGenerateProperties()
		{
			TableModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));

			source = sourceGenerator.GenerateSource(table);

			Assert.IsTrue(source.Contains("public string FirstName"));
			Assert.IsTrue(source.Contains("get"));
			Assert.IsTrue(source.Contains("set"));

		}
		[TestMethod]
		public void ShouldGenerateClass()
		{
			TableModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));
			
			source = sourceGenerator.GenerateSource(table);


			Assert.IsTrue(source.Contains("namespace ns.Models"));
			Assert.IsTrue(source.Contains("public partial class PersonnModel"));
		}



		[TestMethod]
		public void ShouldGenerateConstructor()
		{
			TableModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));

			source = sourceGenerator.GenerateSource(table);


			Assert.IsTrue(source.Contains("public PersonnModel(MyDBModel DatabaseModel, Personn DataSource)"));
		}



		[TestMethod]
		public void ShouldGenerateMethods()
		{
			TableModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable, "AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey); primaryTable.PrimaryKey = primaryKey; // PK

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable, "PersonnAddressID", "byte", false);	// no PK
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople", primaryKey, "DeliveryAddress", foreignKey, CascadeTriggers.None);

			primaryTable.Relations.Add(relation);
			foreignTable.Relations.Add(relation);

			source = sourceGenerator.GenerateSource(foreignTable);
			Assert.IsFalse(source.Contains("public void Delete()"));    // no PK
			Assert.IsTrue(source.Contains("public bool IsModelOf(Personn Item)"));
			Assert.IsTrue(source.Contains("public override string ToString()"));


			source = sourceGenerator.GenerateSource(primaryTable);
			Assert.IsTrue(source.Contains("public void Delete()"));    // PK
			Assert.IsTrue(source.Contains("public bool IsModelOf(Address Item)"));
			Assert.IsTrue(source.Contains("public override string ToString()"));
		}

		[TestMethod]
		public void ShouldGenerateNonNullableRelationMethods()
		{
			TableModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable, "AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey); primaryTable.PrimaryKey = primaryKey; // PK

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable, "PersonnAddressID", "byte", false);   // no PK
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople", primaryKey, "DeliveryAddress", foreignKey, CascadeTriggers.None);

			primaryTable.Relations.Add(relation);
			foreignTable.Relations.Add(relation);

			source = sourceGenerator.GenerateSource(foreignTable);
			Assert.IsTrue(source.Contains("public AddressModel GetDeliveryAddress()"));


			source = sourceGenerator.GenerateSource(primaryTable);
			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetDeliveredPeople()"));
		}

		[TestMethod]
		public void ShouldGenerateNullableRelationMethods()
		{
			TableModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new TableModelSourceGenerator();

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable, "AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey); primaryTable.PrimaryKey = primaryKey; // PK

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable, "PersonnAddressID", "byte", true);   // no PK
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople", primaryKey, "DeliveryAddress", foreignKey, CascadeTriggers.None);

			primaryTable.Relations.Add(relation);
			foreignTable.Relations.Add(relation);

			source = sourceGenerator.GenerateSource(foreignTable);
			Assert.IsTrue(source.Contains("public AddressModel? GetDeliveryAddress()"));


			source = sourceGenerator.GenerateSource(primaryTable);
			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetDeliveredPeople()"));
		}

	}
}