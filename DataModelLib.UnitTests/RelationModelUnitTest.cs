using DataModelLib.DataModels;
using System.Reflection;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class RelationModelUnitTest
	{

		[TestMethod]
		public void ShouldGenerateTableModelMethods()
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
			source = relation.GenerateTableModelMethods();

			Assert.IsTrue(source.Contains("public Address GetDeliveryAddress()"));

		}

	}
}