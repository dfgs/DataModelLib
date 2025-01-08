using DataModelLib.Common;

namespace DataModelLib.UnitTests
{
	[TestClass]
	public class StringUtilsUnitTest
	{
		[TestMethod]
		public void ShouldIndentSingleLine()
		{
			Assert.AreEqual("\tText", "Text".Indent(1));
			Assert.AreEqual("\t\tText", "Text".Indent(2));
		}
		[TestMethod]
		public void ShouldIndentMultiLine()
		{
			Assert.AreEqual("\tText1\r\n\tText2", "Text1\r\nText2".Indent(1));
			Assert.AreEqual("\t\tText1\r\n\t\tText2", "Text1\r\nText2".Indent(2));
		}
		[TestMethod]
		public void ShouldConvertCamelCase()
		{
			Assert.AreEqual("Pet id", "PetID".SplitCamelCase());
			Assert.AreEqual("My value id", "MyValueID".SplitCamelCase());
		}
	}
}