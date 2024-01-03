using LINQTo271.Views.WorkOrders.SOPProcessing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SOPProcessing
{
    /// <summary>
    /// Summary description for SOPProcessingResourceViewPageTest
    /// </summary>
    [TestClass]
    public class SOPProcessingResourceViewPageTest
    {
        #region Private Members

        private SOPProcessingResourceViewPage _target;

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            MyAssert.DoesNotThrow(
                () => _target = new SOPProcessingResourceViewPage());
        }
    }
}
