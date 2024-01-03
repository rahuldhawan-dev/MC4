using MapCall.Common.Testing.Selenium.TestParts;
using NUnit.Framework;

namespace RegressionTests.Lib
{
    public class SeleniumTestBase :  MMSINC.Testing.Selenium.SeleniumTestBase
    {
        #region Private Members

        protected string _userName;

        #endregion

        #region Additional test attributes

        [TestFixtureSetUp]
        public virtual void WorkOrdersTestFixtureInitialize()
        {
            _userName = Login.AsAdmin(_selenium);
        }

        #endregion
    }
}
