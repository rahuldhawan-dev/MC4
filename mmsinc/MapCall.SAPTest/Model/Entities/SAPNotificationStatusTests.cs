using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Model.ViewModels;

namespace MapCall.SAP.Model.Entities.Tests
{
    [TestClass()]
    public class SAPNotificationStatusTests
    {
        #region Private Members

        private SearchSapNotification SearchSapNotification;

        #endregion

        #region Private Methods

        public SearchSapNotification GetTestSearchSapNotification()
        {
            SearchSapNotification = new SearchSapNotification();

            SearchSapNotification.SAPNotificationNo = "000010000389"; //D203
            SearchSapNotification.Cancel = "Yes"; //DateTime.Now.Date.ToString("yyyyMMdd");
            SearchSapNotification.Complete = "No"; //DateTime.Now.Date.ToString("yyyyMMdd");
            return SearchSapNotification;
        }

        #endregion

        [TestMethod()]
        public void TestConstructorSetsNullForSAPNotificationStatus()
        {
            SearchSapNotification = new SearchSapNotification();
            var target = new SAPNotificationStatus(SearchSapNotification);
            Assert.AreEqual(SearchSapNotification.SAPNotificationNo, target.SAPNotificationNo);
            Assert.AreEqual(SearchSapNotification.Cancel, target.Cancel);
            Assert.AreEqual(SearchSapNotification.Complete, target.Complete);
            Assert.AreEqual(SearchSapNotification.Remarks, target.Remarks);
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForSAPNotificationStatus()
        {
            SearchSapNotification = GetTestSearchSapNotification();
            var target = new SAPNotificationStatus(SearchSapNotification);
            Assert.AreEqual(SearchSapNotification.SAPNotificationNo, target.SAPNotificationNo);
            Assert.AreEqual(SearchSapNotification.Cancel, target.Cancel);
            Assert.AreEqual(SearchSapNotification.Complete, target.Complete);
            Assert.AreEqual(SearchSapNotification.Remarks, target.Remarks);
        }
    }
}
