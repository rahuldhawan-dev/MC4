using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.Excel
{
    [TestClass]
    public class FlattenAtExportAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorThrowsIfThePropertyPathIsNullOrEmptyOrWhiteSpace()
        {
            MyAssert.Throws(() => new FlattenAtExportAttribute(null));
            MyAssert.Throws(() => new FlattenAtExportAttribute(string.Empty));
            MyAssert.Throws(() => new FlattenAtExportAttribute("    "));
        }

        [TestMethod]
        public void TestConstructorSetsPropertyPath()
        {
            Assert.AreEqual("blah", new FlattenAtExportAttribute("blah").PropertyPath);
        }

        [TestMethod]
        public void TestConstructorSetsColumnNameToNullByDefault()
        {
            Assert.IsNull(new FlattenAtExportAttribute("blah").ColumnName);
        }

        [TestMethod]
        public void TestConstructorSetsColumnName()
        {
            Assert.AreEqual("column", new FlattenAtExportAttribute("blah", "column").ColumnName);
        }

        #endregion
    }
}
