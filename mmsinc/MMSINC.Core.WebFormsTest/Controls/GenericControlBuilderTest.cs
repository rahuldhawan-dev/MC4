using MMSINC.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for MenuBuilderTest
    /// </summary>
    [TestClass]
    public class GenericControlBuilderTest
    {
        [TestMethod]
        public void TestGetChildControlTypeReturnsSpecifiedTypeWhenTypeNameMatchesTagName()
        {
            var expected = typeof(object);
            var actual =
                new GenericControlBuilder<object>().GetChildControlType("object", null);

            Assert.AreSame(expected, actual);

            expected = typeof(string);
            actual =
                new GenericControlBuilder<string>().GetChildControlType(
                    "string", null);

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void TestGetChildControlTypeReturnsNullWhenTypeNameDoesNotMatchTagName()
        {
            var target = new GenericControlBuilder<object>();

            Assert.IsNull(target.GetChildControlType("foo", null));
        }
    }
}
