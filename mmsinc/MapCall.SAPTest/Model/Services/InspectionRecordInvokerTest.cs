using System;
using System.Threading.Tasks;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using MapCall.SAP.service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class InspectionRecordInvokerTest
    {
        #region Private Members

        private Mock<InspectionRecord_Create_OB_SYN> _sapClient;
        private SAPHttpClient _sapHttpClient;
        private TestInvoker _target;
        private SAPInspection _targetEntity;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _sapClient = new Mock<InspectionRecord_Create_OB_SYN>();
            _sapHttpClient = new SAPHttpClient();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new SAPInspection();
        }

        #endregion

        #region Nested Type: TestInvoker

        private class TestInvoker : InspectionRecordInvoker
        {
            #region Properties

            public InspectionRecord_Create_OB_SYN TargetClient { get; set; }

            #endregion

            #region Constructors

            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

            #endregion

            #region Private Methods

            protected override InspectionRecord_Create_OB_SYN CastServiceClientToChannel(
                InspectionRecord_Create_OB_SYNClient serviceClient)
            {
                return TargetClient;
            }

            #endregion
        }

        #endregion
        
        #region Tests

        [TestMethod]
        public void TestInvokeCatchesErrorAndSetsSapErrorCodeToRetry()
        {
            _target.Invoke(_targetEntity);

            Assert.IsTrue(_targetEntity.Status.StartsWith(SAPHttpClient.RETRY_ERROR_TEXT));
        }

        [TestMethod]
        public void TestInvokeSetsStatusToNoDataFoundWhenNoResponse()
        {
            var response = new InspectionRecord_Create_OB_SYNResponse();
            response.SAPNotificationNumber_Res = Array.Empty<SAPNotificationNumberNotificationNumber>();
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                x.InspectionRecord_Create_OB_SYNAsync(It.IsAny<InspectionRecord_Create_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);

            Assert.AreEqual(InspectionRecordInvoker.NO_DATA_FOUND, _targetEntity.Status);
        }

        [TestMethod]
        public void TestInvokeSetsValuesOnEntityFromResponse()
        {
            var response = new InspectionRecord_Create_OB_SYNResponse();
            var inspection = new SAPNotificationNumberNotificationNumber {
                SAPNotificationNumber = "314159265359",
                Status = "Success",
                CostCenter = "987654"
            };

            response.SAPNotificationNumber_Res = new[] { inspection };
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                x.InspectionRecord_Create_OB_SYNAsync(It.IsAny<InspectionRecord_Create_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);

            Assert.AreEqual(inspection.SAPNotificationNumber, _targetEntity.SAPNotificationNumber);
            Assert.AreEqual(inspection.Status, _targetEntity.Status);
            Assert.AreEqual(inspection.CostCenter, _targetEntity.CostCenter);
        }

        [TestMethod]
        public void TestTimeOutIsSetByConstructor()
        {
            Assert.AreEqual(1, _target.SendTimeOut.Value.Minutes);
        }
        
        #endregion
    }
}
