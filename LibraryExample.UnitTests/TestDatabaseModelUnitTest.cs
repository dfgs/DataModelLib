using LibraryExample.Models;
using DataModelLib;
using System.Reflection;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("TestDatabase"),Database,Blueprint("DatabaseModel.UnitTest.*"),TestClass]
	public partial class TestDatabaseModelUnitTest
	{

	}
}