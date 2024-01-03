using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Utility.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Utility.Permissions
{
    [TestClass]
    public class RoleApplicationNameAttributeTest
    {
        [TestMethod]
        public void TestConstructorThrowsForNullOrWhitespaceParameters()
        {
            MyAssert.Throws(() => new RoleApplicationNameAttribute(null));
            MyAssert.Throws(() => new RoleApplicationNameAttribute(string.Empty));
            MyAssert.Throws(() => new RoleApplicationNameAttribute("        "));
        }

        [TestMethod]
        public void TestConstructorSetsNameProperty()
        {
            var expected = "Name";
            var target = new RoleApplicationNameAttribute(expected);
            Assert.AreEqual(expected, target.Name);
        }
    }
}
