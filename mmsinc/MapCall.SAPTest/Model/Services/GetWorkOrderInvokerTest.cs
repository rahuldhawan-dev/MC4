using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using MapCall.SAP.service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class GetWorkOrderInvokerTest
    {
        #region Private Members

        private Mock<GetNotification_OB_SYN> _sapClient;
        private SAPHttpClient _sapHttpClient;
        private TestInvoker _target;
        private GetNotificationAggregate _targetEntity;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _sapClient = new Mock<GetNotification_OB_SYN>();
            _sapHttpClient = new SAPHttpClient();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new GetNotificationAggregate();
        }

        #endregion

        #region Nested Type: TestInvoker

        private class TestInvoker : GetWorkOrderInvoker
        {
            #region Properties

            public GetNotification_OB_SYN TargetClient { get; set; }

            #endregion

            #region Constructors

            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

            #endregion

            #region Private Methods

            protected override GetNotification_OB_SYN CastServiceClientToChannel(
                GetNotification_OB_SYNClient serviceClient)
            {
                return TargetClient;
            }

            #endregion
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestInvokeSetsStatusToNoDataFoundWhenNoResponse()
        {
            var response = new GetNotification_OB_SYNResponse();
            var sapNotification = new SAPNotification {
                DateCreatedFrom = DateTime.UtcNow.ToShortDateString(),
                DateCreatedTo = DateTime.UtcNow.ToShortDateString(),
                PlanningPlant = "demo",
                SpecialInstructions = "het this is test instruction"
            };

            _targetEntity.SAPNotification = sapNotification;

            response.GetNotification_Response = Array.Empty<ReceiveNotificationNotifications>();
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.GetNotification_OB_SYNAsync(It.IsAny<GetNotification_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);

            Assert.AreEqual(GetWorkOrderInvoker.NO_DATA_FOUND, _targetEntity.SAPNotificationCollections.SAPErrorCode);
        }

        [TestMethod]
        public void TestInvokeSetsValuesOnEntityFromResponse()
        {
            var response = new GetNotification_OB_SYNResponse();
            var notifications = new ReceiveNotificationNotifications {
                CustomerName = "demo",
                AssetType = "service",
                CriticalNotes = "hey this is an instruction",
                LongText = "hey this is a demo note",
                SAPNotificationNo = "123456"
            };

            var sapNotification = new SAPNotification {
                DateCreatedFrom = DateTime.UtcNow.ToShortDateString(),
                DateCreatedTo = DateTime.UtcNow.ToShortDateString(),
                PlanningPlant = "demo",
                SpecialInstructions = "het this is test instruction"
            };

            var notificationCollection = new SAPNotificationCollection();
            notificationCollection.Items.Add(new SAPNotification(notifications));

            _targetEntity.SAPNotification = sapNotification;

            response.GetNotification_Response = new[] { notifications };
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.GetNotification_OB_SYNAsync(It.IsAny<GetNotification_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);

            Assert.AreEqual(notifications.CriticalNotes, _targetEntity.SAPNotificationCollections.Items[0].SpecialInstructions);
            Assert.AreEqual(notifications.AssetType, _targetEntity.SAPNotificationCollections.Items[0].AssetType);
        }

        [TestMethod]
        public void TestInvokeSetsValuesOnEntityWithoutSAPNumberFromResponse()
        {
            var response = new GetNotification_OB_SYNResponse();
            var notifications = new ReceiveNotificationNotifications {
                CustomerName = "demo",
                AssetType = "service",
                CriticalNotes = "hey this is an instruction",
                LongText = "hey this is a demo note",
                SAPStatusMessage = "SAP error msg"
            };

            var sapNotification = new SAPNotification {
                DateCreatedFrom = DateTime.UtcNow.ToShortDateString(),
                DateCreatedTo = DateTime.UtcNow.ToShortDateString(),
                PlanningPlant = "demo",
                SpecialInstructions = "het this is test instruction"
            };

            var notificationCollection = new SAPNotificationCollection();
            notificationCollection.Items.Add(new SAPNotification(notifications));

            _targetEntity.SAPNotification = sapNotification;

            response.GetNotification_Response = new[] { notifications };
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.GetNotification_OB_SYNAsync(It.IsAny<GetNotification_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);

            Assert.AreEqual(notifications.SAPStatusMessage, _targetEntity.SAPNotificationCollections.Items[0].SAPErrorCode);
        }

        [TestMethod]
        public void TestTimeOutIsSetByConstructor()
        {
            Assert.AreEqual(1, _target.SendTimeOut.Value.Minutes);
        }

        #endregion
    }
}
