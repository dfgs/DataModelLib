using DataModelLib.DataModels;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class TableModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateDatabaseProperties()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "MyDB", "Personn","People");
			source=model.GenerateDatabaseProperties();
			
			Assert.IsTrue(source.Contains("public List<Personn> People"));
		}
		[TestMethod]
		public void ShouldGenerateDatabaseConstructor()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "MyDB", "Personn", "People");
			source = model.GenerateDatabaseConstructor();

			Assert.IsTrue(source.Contains("People = new List<Personn>();"));
		}

		[TestMethod]
		public void ShouldGenerateTableModelClass()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "MyDB", "Personn", "People");
			model.ColumnModels.Add(new ColumnModel("FirstName", "string", false));
			source = model.GenerateTableModelClass();


			Assert.IsTrue(source.Contains("namespace ns"));
			Assert.IsTrue(source.Contains("public partial class PersonnModel"));
			Assert.IsTrue(source.Contains("public PersonnModel(MyDBModel DatabaseModel, Personn DataSource)"));

			Assert.IsTrue(source.Contains("public string FirstName"));
			Assert.IsTrue(source.Contains("get"));
			Assert.IsTrue(source.Contains("set"));


		}
		[TestMethod]
		public void ShouldGenerateTableModelConstructor()
		{
			TableModel model;
			string source;

			model = new TableModel("ns", "MyDB", "Personn", "People");
			source = model.GenerateTableModelConstructor();

			Assert.IsTrue(source.Contains("public PersonnModel(MyDBModel DatabaseModel, Personn DataSource)"));

		}
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

			relation = new RelationModel("DeliveryAddress", primaryTable, primaryKey, foreignTable, foreignKey);

			primaryTable.Relations.Add(relation);
			foreignTable.Relations.Add(relation);

			source = foreignTable.GenerateTableModelMethods();

			Assert.IsTrue(source.Contains("public void Delete()"));
			Assert.IsTrue(source.Contains("public Address GetDeliveryAddress()"));
		}


		[TestMethod]
		public void ShouldGenerateDatabaseModelMethods()
		{
			TableModel personnModel;
			TableModel addressModel;
			string source;



			addressModel = new TableModel("ns", "MyDB", "Address", "Addresses");
			source = addressModel.GenerateDatabaseModelMethods();

			Assert.IsTrue(source.Contains("public IEnumerable<AddressModel> GetAddresses()"));
			Assert.IsTrue(source.Contains("public void AddToAddresses(AddressModel Item)"));
			Assert.IsTrue(source.Contains("public void AddToAddresses(Address Item)"));
			Assert.IsTrue(source.Contains("public void RemoveFromAddresses(AddressModel Item)"));


			personnModel = new TableModel("ns", "MyDB", "Personn", "People");
			source = personnModel.GenerateDatabaseModelMethods();

			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetPeople()"));
			Assert.IsTrue(source.Contains("public void AddToPeople(PersonnModel Item)"));
			Assert.IsTrue(source.Contains("public void AddToPeople(Personn Item)"));
			Assert.IsTrue(source.Contains("public void RemoveFromPeople(PersonnModel Item)"));
		}




	}
}