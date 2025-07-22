using LibraryExample.Models;
using DataModelLib;
using System.Reflection;
using System.Diagnostics.Tracing;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("Pet"), Blueprint("TableModel.UnitTest.*"), MockCount(6), TestClass]
	public partial class PetUnitTest
	{
		[TestMethod]
		public void ShouldReturnToString()
		{
			TestDatabaseModel testDatabaseModel;
			PetModel? pet;
			
			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			pet = testDatabaseModel.GetPetTable().ElementAt(0);

			Assert.IsNotNull(pet);
			Assert.AreEqual("Cat", pet.ToString());
		}

	
		[TestMethod]
		public void ShouldCascadeUpdatePersonnUsingNotNullableForeignKey()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel updatedForeignItem;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			updatedForeignItem = testDatabaseModel.GetPersonn(3);
			Assert.AreNotEqual(1, updatedForeignItem.PetID);
			updatedForeignItem.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			testDatabaseModel.GetPetTable().ElementAt(1).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.IsTrue(models.All(item=>item.PetID==1));
			Assert.AreEqual(1, updatedForeignItem.PetID);
			Assert.AreEqual("PetID", propertyName);



		}
		[TestMethod]
		public void ShouldNotCascadeUpdatePersonn()
		{
			TestDatabaseModel testDatabaseModel;
			PersonnModel[] models;
			PersonnModel updatedForeignItem;
			string? propertyName = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			updatedForeignItem = testDatabaseModel.GetPersonn(3);
			updatedForeignItem.PropertyChanged += (_, e) => { propertyName = e.PropertyName; };

			testDatabaseModel.GetPetTable().ElementAt(2).Delete();
			models = testDatabaseModel.GetPersonnTable().ToArray();
			Assert.AreEqual(1, models[0].PetID);
			Assert.AreEqual(1, models[1].PetID);
			Assert.AreEqual(2, models[2].PetID);
			Assert.AreEqual(2, models[3].PetID);
			Assert.IsNull( propertyName);

		}



		
		[TestMethod]
		public void ShouldRaiseOwnersChangedWhenChangingPersonn()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null,personn2=null;
			TableChangedActions? action1 = null,action2=null;
			int? index1 = null,index2=null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPet(1).OwnersChanged += (item, a, i) => { personn1 = item; action1 = a; index1 = i; };
			testDatabaseModel.GetPet(2).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(3).OwnersChanged += (item, a, i) => { personn2 = item; action2 = a; index2 = i; };

			testDatabaseModel.GetPersonn(1).PetID=3;

			Assert.IsNotNull(personn1);
			Assert.IsNotNull(action1);
			Assert.IsNotNull(index1);
			Assert.AreEqual("Homer", personn1.FirstName);
			Assert.AreEqual(0, index1);
			Assert.AreEqual(TableChangedActions.Remove, action1);


			Assert.IsNotNull(personn2);
			Assert.IsNotNull(action2);
			Assert.IsNotNull(index2);
			Assert.AreEqual("Homer", personn2.FirstName);
			Assert.AreEqual(0, index2);
			Assert.AreEqual(TableChangedActions.Add, action2);
		}
		[TestMethod]
		public void ShouldRaiseOwnersChangedWhenChangingPersonnWithNonExistingID()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPet(1).OwnersChanged += (item, a, i) => { personn1 = item; action1 = a; index1 = i; };
			testDatabaseModel.GetPet(2).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(3).OwnersChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).PetID = 33; // doesn't exist

			Assert.IsNotNull(personn1);
			Assert.IsNotNull(action1);
			Assert.IsNotNull(index1);
			Assert.AreEqual("Homer", personn1.FirstName);
			Assert.AreEqual(0, index1);
			Assert.AreEqual(TableChangedActions.Remove, action1);


			Assert.IsNull(personn2);
			Assert.IsNull(action2);
			Assert.IsNull(index2);
			
		}

		[TestMethod]
		public void ShouldNotRaiseOwnersChangedWhenKeepingSamePropertyValue()
		{
			TestDatabaseModel testDatabaseModel;
			Personn? personn1 = null, personn2 = null;
			TableChangedActions? action1 = null, action2 = null;
			int? index1 = null, index2 = null;

			testDatabaseModel = new TestDatabaseModel(Utils.CreateTestDatabase());
			testDatabaseModel.GetPet(1).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(2).OwnersChanged += (item, a, i) => { Assert.Fail(); };
			testDatabaseModel.GetPet(3).OwnersChanged += (item, a, i) => { Assert.Fail(); };

			testDatabaseModel.GetPersonn(1).PetID = 1;

			Assert.IsNull(personn1);
			Assert.IsNull(action1);
			Assert.IsNull(index1);
			
			Assert.IsNull(personn2);
			Assert.IsNull(action2);
			Assert.IsNull(index2);
			
		}
	


	}
}