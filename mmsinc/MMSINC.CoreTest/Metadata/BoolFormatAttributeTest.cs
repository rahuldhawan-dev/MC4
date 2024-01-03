using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.CoreTest.Metadata
{
    [TestClass]
    public class BoolFormatAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestEmptyConstructorSetsTrueToTrue()
        {
            var target = new BoolFormatAttribute();
            Assert.AreEqual("True", target.True);
        }

        [TestMethod]
        public void TestEmptyConstructorSetsFalseToFalse()
        {
            var target = new BoolFormatAttribute();
            Assert.AreEqual("False", target.False);
        }

        [TestMethod]
        public void TestEmptyConstructorSetsNullToNull()
        {
            var target = new BoolFormatAttribute();
            Assert.IsNull(target.Null);
        }

        [TestMethod]
        public void TestConstructorSetsTrueToTrueTextValue()
        {
            var target = new BoolFormatAttribute("SHAZAM!", null, null);
            Assert.AreEqual("SHAZAM!", target.True);
        }

        [TestMethod]
        public void TestConstructorSetsFalseToFalseTextValue()
        {
            var target = new BoolFormatAttribute(null, "SHAZAM!", null);
            Assert.AreEqual("SHAZAM!", target.False);
        }

        [TestMethod]
        public void TestConstructorSetsNullToNullTextValue()
        {
            var target = new BoolFormatAttribute(null, null, "SHAZAM!");
            Assert.AreEqual("SHAZAM!", target.Null);
        }

        [TestMethod]
        public void TestConstructorSetsNullToNullByDefault()
        {
            var target = new BoolFormatAttribute("truth", "lies"); // don't use null parameter here
            Assert.IsNull(target.Null);
        }

        [TestMethod]
        public void TestFormatValueReturnsNullValueForNullNullableBool()
        {
            var target = new BoolFormatAttribute("truth", "lies", "nulls");
            Assert.AreEqual("nulls", target.FormatValue(null));
        }

        [TestMethod]
        public void TestFormatValueReturnsTrueValueForTrueNullableBool()
        {
            var target = new BoolFormatAttribute("truth", "lies", "nulls");
            Assert.AreEqual("truth", target.FormatValue(true));
        }

        [TestMethod]
        public void TestFormatValueReturnsFalseValueForFalseNullableBool()
        {
            var target = new BoolFormatAttribute("truth", "lies", "nulls");
            Assert.AreEqual("lies", target.FormatValue(false));
        }

        [TestMethod]
        public void TestFormatHasAPanicAttackAndDiesIfItReceivesSomethingNotCastableToANullableBool()
        {
            var target = new BoolFormatAttribute("truth", "lies", "nulls");
            MyAssert.Throws(() => target.FormatValue("I pity the bool!"));
        }

        #endregion
    }
}
