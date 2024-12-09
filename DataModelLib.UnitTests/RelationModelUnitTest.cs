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

			primaryTable = new TableModel("ns1", "db1", "Address");
			primaryKey = new ColumnModel(primaryTable,"AddressID", "byte", false);
			primaryTable.ColumnModels.Add(primaryKey);

			foreignTable = new TableModel("ns1", "db1", "Personn");
			foreignKey = new ColumnModel(foreignTable,"PersonnAddressID", "byte", false);
			foreignTable.ColumnModels.Add(foreignKey);

			relation = new RelationModel("DeliveredPeople",  primaryKey, "DeliveryAddress",  foreignKey, CascadeTriggers.None);
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

			primaryTable = new TableModel("ns1", "db1", "Address");
			primaryKey = new ColumnModel(primaryTable, "AddressID", "byte", false);
			primaryTable.ColumnModels.Add(primaryKey);

			foreignTable = new TableModel("ns1", "db1", "Personn");
			foreignKey = new ColumnModel(foreignTable,"DeliveryAddressID", "byte", true);
			foreignTable.ColumnModels.Add(foreignKey);

			relation = new RelationModel("DeliveredPeople",  primaryKey, "DeliveryAddress",  foreignKey, CascadeTriggers.None);
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

			primaryTable = new TableModel("ns1", "db1", "Address");
			primaryKey = new ColumnModel(primaryTable, "AddressID", "byte", false);
			primaryTable.ColumnModels.Add(primaryKey);

			foreignTable = new TableModel("ns1", "db1", "Personn");
			foreignKey = new ColumnModel(foreignTable,"PersonnAddressID", "byte", false);
			foreignTable.ColumnModels.Add(foreignKey);

			relation = new RelationModel("DeliveredPeople",  primaryKey, "DeliveryAddress",  foreignKey, CascadeTriggers.None);
			source = relation.GenerateTableModelMethods(true);

			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetDeliveredPeople()"));

		}
	}
}