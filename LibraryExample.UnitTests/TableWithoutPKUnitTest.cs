using LibraryExample.Models;
using DataLib;
using System.Reflection;
using System.Diagnostics.Tracing;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("TableWithoutPK"), Blueprint("TableModel.UnitTest.*"), Using("LibraryExample.Models"), MockCount(6), TestClass]
	public partial class TableWithoutPKUnitTest
	{
	


	}
}