using LibraryExample.Models;
using DataModelLib;
using System.Reflection;
using System.Diagnostics.Tracing;
using BlueprintLib.Attributes;

namespace LibraryExample.UnitTests
{
	[DTO("TableWithoutPK"), Blueprint("TableModel.UnitTest.*"), MockCount(6), TestClass]
	public partial class TableWithoutPKUnitTest
	{
	


	}
}