using LibraryExample.Models;
using DataLib;
using System.Reflection;
using BlueprintLib.Attributes;
using DataModelLib;

namespace LibraryExample.UnitTests
{
	[DTO("TestDatabase"),DatabaseModel,Blueprint("DatabaseModel.UnitTest.*"), TestClass]
	public partial class TestDatabaseModelUnitTest
	{

	}
}