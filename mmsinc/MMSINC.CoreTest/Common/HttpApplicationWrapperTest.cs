using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Common
{
    /// <summary>
    /// Summary description for HttpApplicationWrapperTest
    /// </summary>
    [TestClass]
    public class HttpApplicationWrapperTest : EventFiringTestClass
    {
        #region Additional Test Attributes

        [TestInitialize]
        public void HttpApplicationWrapperTestInitialize()
        {
            base.EventFiringTestClassInitialize();
        }

        [TestCleanup]
        public void HttpApplicationWrapperTestCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion
    }
}
