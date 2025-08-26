using LibraryExample.Models;
using DataLib;
using System.Reflection;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("TestDatabase"),Database,Blueprint("DatabaseModel.UnitTest.*"), Using("LibraryExample.Models"), TestClass]
	public partial class TestDatabaseModelUnitTest
	{

	}
}