using DataModelLib.DataModels;
using System.Reflection;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class RelationModelUnitTest
	{

		[TestMethod]
		public void ShouldGenerateForeignTableModelMethodsWithNotNullableForeignKey()
		{
			RelationModel relation;
			TableModel primaryTable;
			ColumnModel primaryKey;
			TableModel foreignTable;
			ColumnModel foreignKey;
			string source;

			primaryTable = new TableModel("ns1", "db1", "Address", "Addresses");
			primaryKey = new ColumnModel("AddressID", "byte", false);
			primaryTable.ColumnModels.Add(primaryKey);

			foreignTable = new TableModel("ns1", "db1", "Personn", "People");
			foreignKey = new ColumnModel("PersonnAddressID", "byte", false);
			foreignTable.ColumnModels.Add(foreignKey);

			relation = new RelationModel("DeliveryAddress", primaryTable,primaryKey,foreignTable,foreignKey);
			source = relation.GenerateTableModelMethods(false);

			Assert.IsTrue(source.Contains("public AddressModel GetDeliveryAddress()"));

		}
		[TestMethod]
		public void ShouldGenerateForeignTableModelMethodsWithNullableForeignKey()
		{
			RelationModel relation;
			TableModel primaryTable;
			ColumnModel primaryKey;
			TableModel foreignTable;
			ColumnModel foreignKey;
			string source;

			primaryTable = new TableModel("ns1", "db1", "Address", "Addresses");
			primaryKey = new ColumnModel("AddressID", "byte", false);
			primaryTable.ColumnModels.Add(primaryKey);

			foreignTable = new TableModel("ns1", "db1", "Personn", "People");
			foreignKey = new ColumnModel("DeliveryAddressID", "byte", true);
			foreignTable.ColumnModels.Add(foreignKey);

			relation = new RelationModel("DeliveryAddress", primaryTable, primaryKey, foreignTable, foreignKey);
			source = relation.GenerateTableModelMethods(false);

			Assert.IsTrue(source.Contains("public AddressModel? GetDeliveryAddress()"));

		}

		[TestMethod]
		public void ShouldGeneratePrimaryTableModelMethods()
		{
			RelationModel relation;
			TableModel primaryTable;
			ColumnModel primaryKey;
			TableModel foreignTable;
			ColumnModel foreignKey;
			string source;

			primaryTable = new TableModel("ns1", "db1", "Address", "Addresses");
			primaryKey = new ColumnModel("AddressID", "byte", false);
			primaryTable.ColumnModels.Add(primaryKey);

			foreignTable = new TableModel("ns1", "db1", "Personn", "People");
			foreignKey = new ColumnModel("PersonnAddressID", "byte", false);
			foreignTable.ColumnModels.Add(foreignKey);

			relation = new RelationModel("DeliveryAddress", primaryTable, primaryKey, foreignTable, foreignKey);
			source = relation.GenerateTableModelMethods(true);

			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetPeople()"));

		}
	}
}