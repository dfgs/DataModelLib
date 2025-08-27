using LibraryExample.Models;
using DataLib;
using System.Reflection;
using BlueprintLib.Attributes;
using DataModelLib;

namespace LibraryExample.UnitTests
{
	[DatabaseUnitTest, DTO("TestDatabase"),Blueprint("DatabaseModel.UnitTest.*"), TestClass]
	public partial class TestDatabaseModelUnitTest
	{

	}
}