using System;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using MapCall.SAP.WorkOrderStatusUpdateWS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using Moq;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class CreateShortCycleStatusUpdateInvokerTest
    {
        #region Private Members

        private TestInvoker _target;
        private SAPWorkOrderStatusUpdateRequest _targetEntity;
        private SAPHttpClient _sapHttpClient;
        private Mock<WO_StatusUpdate_OB_SYC> _sapClient;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _sapHttpClient = new SAPHttpClient();
            _sapClient = new Mock<WO_StatusUpdate_OB_SYC>();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new SAPWorkOrderStatusUpdateRequest();
        }

        [TestMethod]
        public void TestTimeOutIsSetByConstructor()
        {
            Assert.AreEqual(10, _target.SendTimeOut.Value.Seconds);
        }

        [TestMethod]
        public void TestInvokeSetsNoRecordsFoundWhenNullResponse()
        {
            WO_StatusUpdate_OB_SYCRequest request = null;
            var response = new WO_StatusUpdate_OB_SYCResponse();
            _sapClient.Setup(x => x.WO_StatusUpdate_OB_SYC(It.IsAny<WO_StatusUpdate_OB_SYCRequest>()))
                      .Returns(response);
            _target.Invoke(_targetEntity);

            Assert.AreEqual("No records found, null response or status", _targetEntity.SAPStatus);
        }

        [TestMethod]
        public void TestInvokeSetsRequestWhenCalled()
        {
            WO_StatusUpdate_OB_SYCRequest request = null;
            var response = new WO_StatusUpdate_OB_SYCResponse();
            _sapClient.Setup(x => x.WO_StatusUpdate_OB_SYC(It.IsAny<WO_StatusUpdate_OB_SYCRequest>()))
                      .Returns(response)
                      .Callback((WO_StatusUpdate_OB_SYCRequest x) => { request = x; });

            _target.Invoke(_targetEntity);

            Assert.IsNotNull(request, "The request should have been made");
            Assert.IsNotNull(request.WOStatusUpdate_Request, "This property should not be null");
        }

        [TestMethod]
        public void TestInvokeSetsSAPStatus()
        {
            WO_StatusUpdate_OB_SYCRequest request = null;
            var wostatusUpdate_response = new WOStatusUpdateStatus {SAPStatusCode = "E", SAPStatus = "Foo"};
            var response = new WO_StatusUpdate_OB_SYCResponse {WOStatusUpdate_Response = wostatusUpdate_response};
            _sapClient.Setup(x => x.WO_StatusUpdate_OB_SYC(It.IsAny<WO_StatusUpdate_OB_SYCRequest>()))
                      .Returns(response);

            _target.Invoke(_targetEntity);

            Assert.AreEqual(wostatusUpdate_response.SAPStatus, _targetEntity.SAPStatus);
        }

        [TestMethod]
        public void TestOnExceptionSetsSAPStatusToSAPErrorCodeWhenStatusIsNull()
        {
            WO_StatusUpdate_OB_SYCRequest request = null;
            var wostatusUpdate_response = new WOStatusUpdateStatus { SAPStatusCode = "E", SAPStatus = "Foo" };
            var response = new WO_StatusUpdate_OB_SYCResponse { WOStatusUpdate_Response = wostatusUpdate_response };
            var exception = "oh no!";
            _sapClient.Setup(x => x.WO_StatusUpdate_OB_SYC(It.IsAny<WO_StatusUpdate_OB_SYCRequest>()))
                      .Throws(new Exception(exception));

            _target.Invoke(_targetEntity);

            Assert.IsTrue(_targetEntity.SAPStatus.StartsWith($"RETRY::System.Exception: {exception}"));
        }

        #endregion

        #region Nested Type: TestInvoker

        private class TestInvoker : CreateShortCycleStatusUpdateInvoker
        {
            #region Properties

            public WO_StatusUpdate_OB_SYC TargetClient { get; set; }

            #endregion

            #region Constructors

            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

            #endregion

            #region Private Methods

            protected override WO_StatusUpdate_OB_SYC CastServiceClientToChannel(
                WO_StatusUpdate_OB_SYCClient serviceClient)
            {
                return TargetClient;
            }

            #endregion
        }

        #endregion
    }
}
