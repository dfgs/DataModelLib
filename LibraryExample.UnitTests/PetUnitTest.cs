using LibraryExample.Models;
using DataLib;
using System.Reflection;
using System.Diagnostics.Tracing;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[TableUnitTest, DTO("Pet"), Blueprint("TableModel.UnitTest.*"), Using("LibraryExample.Models"), MockCount(6), TestClass]
	public partial class PetUnitTest
	{
		
	
				

	


	}
}