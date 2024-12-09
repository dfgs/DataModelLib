using DataModelLib.Schema;
using System.Reflection;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class RelationModelUnitTest
	{

		[TestMethod]
		public void ShouldGenerateForeignTableModelMethodsWithNotNullableForeignKey()
		{
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable,"AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey);

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable,"PersonnAddressID", "byte", false);
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople",  primaryKey, "DeliveryAddress",  foreignKey, CascadeTriggers.None);
			source = relation.GenerateTableModelMethods(false);

			Assert.IsTrue(source.Contains("public AddressModel GetDeliveryAddress()"));

		}
		[TestMethod]
		public void ShouldGenerateForeignTableModelMethodsWithNullableForeignKey()
		{
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable, "AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey);

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable,"DeliveryAddressID", "byte", true);
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople",  primaryKey, "DeliveryAddress",  foreignKey, CascadeTriggers.None);
			source = relation.GenerateTableModelMethods(false);

			Assert.IsTrue(source.Contains("public AddressModel? GetDeliveryAddress()"));

		}

		[TestMethod]
		public void ShouldGeneratePrimaryTableModelMethods()
		{
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable, "AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey);

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable,"PersonnAddressID", "byte", false);
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople",  primaryKey, "DeliveryAddress",  foreignKey, CascadeTriggers.None);
			source = relation.GenerateTableModelMethods(true);

			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetDeliveredPeople()"));

		}
	}
}