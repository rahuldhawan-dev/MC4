using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Entities.Tests;
using StructureMap;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Repositories;

namespace MapCall.SAP.Model.Repositories.Tests
{
    [TestClass()]
    public class SAPNotificationRepositoryTests
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPNotificationRepository _target;
        private IContainer _container;

        #region Private Method

        private SAPNotificationStatus GetTestUpdateNotificationCollection()
        {
            SAPNotificationStatus SAPNotificationStatus;

            SAPNotificationStatus = new SAPNotificationStatus {
                NotificationID = "000010001283",
                Cancel = "Yes",
                Complete = "No"
            };

            return SAPNotificationStatus;
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());

            _target = _container.GetInstance<SAPNotificationRepository>();

            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchReturnsResults()
        {
            SearchSapNotification SearchSapNotification =
                _container.GetInstance<SAPNotificationTests>().GetTestSearchSapNotification();

            SAPNotificationCollection actual = _target.Search(SearchSapNotification);

            foreach (SAPNotification sapNotification in actual)
            {
                Assert.AreEqual("Successful", sapNotification.SAPErrorCode?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchWorkOrderReturnsResults()
        {
            SAPNotification sapNotification =
                _container.GetInstance<SAPNotificationTests>().GetTestSearchSapWorkOrder();

            SAPNotificationCollection actual = _target.SearchWorkOrder(sapNotification);

            foreach (SAPNotification sapNotification1 in actual)
            {
                Assert.AreEqual("Successful", sapNotification1.SAPErrorCode?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchReturnsResultsForNoRecordFound()
        {
            var SearchSapNotification =
                _container.GetInstance<SAPNotificationTests>().GetTestSearchSapNotificationForNoData();

            SAPNotificationCollection actual = _target.Search(SearchSapNotification);

            foreach (SAPNotification sapNotification in actual)
            {
                Assert.AreEqual("Successful", sapNotification.SAPErrorCode?.ToString());
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveForSAPNotification()
        {
            SAPNotificationStatus actual = _target.Save(GetTestUpdateNotificationCollection());
            Assert.AreEqual("Enter Notification Number", actual.SAPMessage?.ToString());
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSearchSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = _container.GetInstance<SearchSapNotification>();

            var result = _target.Search(entity);

            Assert.AreEqual(SAPNotificationRepository.ERROR_NO_SITE_CONNECTION, result.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = _container.GetInstance<SAPNotificationStatus>();

            var result = _target.Save(entity);

            Assert.AreEqual(SAPNotificationRepository.ERROR_NO_SITE_CONNECTION, result.SAPMessage);
        }
    }
}
