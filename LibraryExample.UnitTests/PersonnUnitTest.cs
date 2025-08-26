using LibraryExample.Models;
using DataLib;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[TableUnitTest, DTO("Personn"), Blueprint("TableModel.UnitTest.*"), Using("LibraryExample.Models"), MockCount(10), TestClass]
	public partial class PersonnUnitTest
	{
		

		

		
	}
}