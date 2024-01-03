using System.Data;
using MMSINC.DataPages;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for FilterBuilderParameterTest
    /// </summary>
    [TestClass]
    public class FilterBuilderParameterTest
    {
        [TestMethod]
        public void TestConstructorThrowsExceptionOnNullNameArgument()
        {
            MyAssert.Throws(() => new FilterBuilderParameter(null, DbType.VarNumeric, null));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionOnEmptyNameArgument()
        {
            MyAssert.Throws(() => new FilterBuilderParameter(string.Empty, DbType.VarNumeric, null));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionOnWhiteSpaceNameArgument()
        {
            MyAssert.Throws(() => new FilterBuilderParameter("      ", DbType.VarNumeric, null));
        }

        [TestMethod]
        public void TestConstructorOverloadSetsArgumentProperties()
        {
            var expectedName = "I am a BANANA!";
            var expectedDbType = DbType.Int32;
            var expectedValue = 23592;

            var target = new FilterBuilderParameter(expectedName, expectedDbType,
                expectedValue);

            Assert.AreEqual(expectedName, target.Name);
            Assert.AreEqual(expectedDbType, target.DbType);
            Assert.AreEqual(expectedValue, target.Value);
        }

        [TestMethod]
        public void TestSettingNamePropertyReturnsSameValue()
        {
            var expected = "name";
            var target = new FilterBuilderParameter();

            target.Name = expected;

            Assert.AreEqual(expected, target.Name);
        }

        [TestMethod]
        public void TestSettingNamePropertyThrowsExceptionIfNameContainsBrackets()
        {
            MyAssert.Throws(() => new FilterBuilderParameter("[field]", DbType.Xml, null));
            MyAssert.Throws(() => new FilterBuilderParameter("[field", DbType.Xml, null));
            MyAssert.Throws(() => new FilterBuilderParameter("field]", DbType.Xml, null));
        }

        [TestMethod]
        public void TestSettingDbTypePropertyReturnsSameValue()
        {
            var expected = DbType.Xml;
            var target = new FilterBuilderParameter();

            target.DbType = expected;

            Assert.AreEqual(expected, target.DbType);
        }

        [TestMethod]
        public void TestSettingValuePropertyReturnsSameValue()
        {
            var expected = 24642;
            var target = new FilterBuilderParameter();

            target.Value = expected;

            Assert.AreEqual(expected, target.Value);
        }

        #region ParameterFormattedName tests

        [TestMethod]
        public void TestParameterFormattedNameReturnsExpectedValue()
        {
            TestParameterizedNameFormatting("table.field", "tablefield");
            TestParameterizedNameFormatting("table field", "tablefield");
            TestParameterizedNameFormatting("table             field", "tablefield");
            TestParameterizedNameFormatting("table.........field", "tablefield");
            TestParameterizedNameFormatting("table.field name", "tablefieldname");
        }

        private static void TestParameterizedNameFormatting(string testParamName, string expectedFormattedName)
        {
            var target = new FilterBuilderParameter();
            target.Name = testParamName;

            Assert.AreEqual(target.ParameterFormattedName, expectedFormattedName);
        }

        #endregion

        #region QualifiedFormattedName tests

        [TestMethod]
        public void TestQualifiedFormattedNameReturnsExpectedValue()
        {
            TestQualifiedNameFormatting("table", "[table]");
            TestQualifiedNameFormatting("table.field", "[table].[field]");
            TestQualifiedNameFormatting("table.field name", "[table].[field name]");
        }

        private static void TestQualifiedNameFormatting(string testParamName, string expectedFormattedName)
        {
            var target = new FilterBuilderParameter();
            target.Name = testParamName;

            Assert.AreEqual(target.QualifiedFormattedName, expectedFormattedName);
        }

        #endregion

        #region Static method tests

        [TestMethod]
        public void TestGetFullyQualifiedFieldNameThrowsExceptionForNullArgument()
        {
            MyAssert.Throws(() => FilterBuilderParameter.GetFormattedQualifiedFieldName(null));
        }

        [TestMethod]
        public void TestGetParameterizedFormattedNameReturnsNullForNullArgument()
        {
            Assert.IsNull(FilterBuilderParameter.GetParameterizedFormattedName(null));
        }

        #endregion
    }
}
