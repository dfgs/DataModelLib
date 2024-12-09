using DataModelLib.Schema;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class TableModelUnitTest
	{
		[TestMethod]
		public void ShouldGenerateDatabaseProperties()
		{
			Table model;
			string source;

			model = new Table("ns", "MyDB", "Personn");
			source=model.GenerateDatabaseProperties();
			
			Assert.IsTrue(source.Contains("public List<Personn> Personn"));
		}
		[TestMethod]
		public void ShouldGenerateDatabaseConstructor()
		{
			Table model;
			string source;

			model = new Table("ns", "MyDB", "Personn");
			source = model.GenerateDatabaseConstructor();

			Assert.IsTrue(source.Contains("PersonnTable = new List<Personn>();"));
		}

		[TestMethod]
		public void ShouldGenerateTableModelClass()
		{
			Table model;
			string source;

			model = new Table("ns", "MyDB", "Personn");
			model.Columns.Add(new Column(model,"FirstName", "string", false));
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
			Table model;
			string source;

			model = new Table("ns", "MyDB", "Personn");
			source = model.GenerateTableModelConstructor();

			Assert.IsTrue(source.Contains("public PersonnModel(MyDBModel DatabaseModel, Personn DataSource)"));

		}
		[TestMethod]
		public void ShouldGenerateTableModelMethods()
		{
			Relation relation;
			Table primaryTable;
			Column primaryKey;
			Table foreignTable;
			Column foreignKey;
			string source;

			primaryTable = new Table("ns1", "db1", "Address");
			primaryKey = new Column(primaryTable,"AddressID", "byte", false);
			primaryTable.Columns.Add(primaryKey);primaryTable.PrimaryKey = primaryKey;

			foreignTable = new Table("ns1", "db1", "Personn");
			foreignKey = new Column(foreignTable,"PersonnAddressID", "byte", false);
			foreignTable.Columns.Add(foreignKey);

			relation = new Relation("DeliveredPeople",  primaryKey, "DeliveryAddress", foreignKey, CascadeTriggers.None);

			primaryTable.Relations.Add(relation);
			foreignTable.Relations.Add(relation);

			source = foreignTable.GenerateTableModelMethods();

			Assert.IsFalse(source.Contains("public void Delete()"));
			Assert.IsTrue(source.Contains("public AddressModel GetDeliveryAddress()"));
		}


		[TestMethod]
		public void ShouldGenerateDatabaseModelMethods()
		{
			Table personnModel;
			Table addressModel;
			string source;



			addressModel = new Table("ns", "MyDB", "Address");
			addressModel.PrimaryKey=new Column(addressModel,"AddressID","byte",false);
			source = addressModel.GenerateDatabaseModelMethods();

			Assert.IsTrue(source.Contains("public AddressModel GetAddress(byte AddressID)"));
			Assert.IsTrue(source.Contains("public AddressModel GetAddress(Func<Address,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public IEnumerable<AddressModel> GetAddressTable()"));
			Assert.IsTrue(source.Contains("public IEnumerable<AddressModel> GetAddressTable(Func<Address,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public void AddAddress(Address Item)"));
			Assert.IsTrue(source.Contains("public void RemoveAddress(AddressModel Item)"));


			personnModel = new Table("ns", "MyDB", "Personn");
			source = personnModel.GenerateDatabaseModelMethods();
			 
			Assert.IsFalse(source.Contains("public PersonnModel GetPersonn(byte PersonnID)"));// No primary key
			Assert.IsTrue(source.Contains("public PersonnModel GetPersonn(Func<Personn,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetPersonnTable()"));
			Assert.IsTrue(source.Contains("public IEnumerable<PersonnModel> GetPersonnTable(Func<Personn,bool> Predicate)"));
			Assert.IsTrue(source.Contains("public void AddPersonn(Personn Item)"));
			Assert.IsFalse(source.Contains("public void RemovePersonn(PersonnModel Item)"));// No primary key
		}




	}
}