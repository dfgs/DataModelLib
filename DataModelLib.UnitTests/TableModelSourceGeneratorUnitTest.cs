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
			ModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new ModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));

			source = sourceGenerator.GenerateSource(table);


			Assert.IsTrue(source.Contains("using DataModelLib.Common;"));

		}
		[TestMethod]
		public void ShouldGenerateProperties()
		{
			ModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new ModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));

			source = sourceGenerator.GenerateSource(table);

			Assert.IsTrue(source.Contains("public string FirstName"));
			Assert.IsTrue(source.Contains("get"));
			Assert.IsTrue(source.Contains("set"));

		}

		[TestMethod]
		public void ShouldGenerateEvents()
		{
			ModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new ModelSourceGenerator();

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
			Assert.IsTrue(source.Contains("public event PropertyChangedEventHandler PropertyChanged;"));


			source = sourceGenerator.GenerateSource(primaryTable);
			Assert.IsTrue(source.Contains("public event PropertyChangedEventHandler PropertyChanged;"));
			Assert.IsTrue(source.Contains("public event TableChangedEventHandler<Personn> DeliveredPeopleChanged;"));


		}

		[TestMethod]
		public void ShouldGenerateClass()
		{
			ModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new ModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));
			
			source = sourceGenerator.GenerateSource(table);


			Assert.IsTrue(source.Contains("namespace ns.Models"));
			Assert.IsTrue(source.Contains("public partial class PersonnModel"));
		}



		[TestMethod]
		public void ShouldGenerateConstructor()
		{
			ModelSourceGenerator sourceGenerator;
			Table table;
			string source;

			sourceGenerator = new ModelSourceGenerator();

			table = new Table("ns", "MyDB", "Personn");
			table.Columns.Add(new Column(table, "FirstName", "string", false));

			source = sourceGenerator.GenerateSource(table);


			Assert.IsTrue(source.Contains("public PersonnModel(MyDBModel DatabaseModel, Personn DataSource)"));
		}



		[TestMethod]
		public void ShouldGenerateMethods()
		{
			ModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new ModelSourceGenerator();

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
			Assert.IsFalse(source.Contains("private void OnPersonnTableChanging(Personn Item,TableChangedActions Action, int Index)"));
			Assert.IsFalse(source.Contains("private void OnPersonnTableChanged(Personn Item,TableChangedActions Action, int Index)"));


			source = sourceGenerator.GenerateSource(primaryTable);
			Assert.IsTrue(source.Contains("public void Delete()"));    // PK
			Assert.IsTrue(source.Contains("public bool IsModelOf(Address Item)"));
			Assert.IsTrue(source.Contains("public override string ToString()"));
			Assert.IsTrue(source.Contains("private void OnPersonnTableChanging(Personn Item,TableChangedActions Action, int Index)"));
			Assert.IsTrue(source.Contains("private void OnPersonnTableChanged(Personn Item,TableChangedActions Action, int Index)"));


		}

		[TestMethod]
		public void ShouldGenerateNonNullableRelationMethods()
		{
			ModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new ModelSourceGenerator();

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
			ModelSourceGenerator sourceGenerator;
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			sourceGenerator = new ModelSourceGenerator();

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