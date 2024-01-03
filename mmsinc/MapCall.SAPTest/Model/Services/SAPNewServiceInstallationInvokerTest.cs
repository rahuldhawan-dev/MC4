using MapCall.SAP.DeviceRemoveReplaceWS;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using MapCall.SAP.service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class SAPNewServiceInstallationInvokerTest
    {
        #region Fields

        private TestInvoker _target;
        private SAPNewServiceInstallation _targetEntity;
        private SAPHttpClient _sapHttpClient;
        private Mock<W1v_New_ServiceInstallation_Get_OB_SYN> _sapClient;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _sapHttpClient = new SAPHttpClient();
            _sapClient = new Mock<W1v_New_ServiceInstallation_Get_OB_SYN>();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new SAPNewServiceInstallation();
        }

        #endregion

        [TestMethod]
        public void TestTimeOutIsSetByConstructor()
        {
            Assert.AreEqual(30, _target.SendTimeOut.Value.Seconds);
        }

        [TestMethod]
        public void TestInvokeSetsNewServiceInstallationOnQuery()
        {
            W1v_New_ServiceInstallation_Get_OB_SYNRequest request = null;
            var response = new W1v_New_ServiceInstallation_Get_OB_SYNResponse();
            _sapClient.Setup(x =>
                           x.W1v_New_ServiceInstallation_Get_OB_SYNAsync(It.IsAny<W1v_New_ServiceInstallation_Get_OB_SYNRequest>()).Result)
                      .Returns(response)
                      .Callback((W1v_New_ServiceInstallation_Get_OB_SYNRequest x) => { request = x; });

            _target.Invoke(_targetEntity);

            Assert.IsNotNull(request, "The request should have been made");
            Assert.IsNotNull(request.W1v_New_ServiceInstallation_Request, "This property should have been set.");
        }

        [TestMethod]
        public void TestInvokeSetsNoRecordsFoundWhenNullResponse()
        {
            W1v_New_ServiceInstallation_Get_OB_SYNRequest request = null;
            var response = new W1v_New_ServiceInstallation_Get_OB_SYNResponse();
            _sapClient.Setup(x =>
                           x.W1v_New_ServiceInstallation_Get_OB_SYNAsync(It.IsAny<W1v_New_ServiceInstallation_Get_OB_SYNRequest>()).Result)
                      .Returns(response);

            _target.Invoke(_targetEntity);

            Assert.AreEqual("No record was returned from the SAP Web Service", _targetEntity.SAPStatus);
        }

        [TestMethod]
        public void TestInvokeSetsStatusAndNumberWhenResponseIsValid()
        {
            var response = new W1v_New_ServiceInstallation_Get_OB_SYNResponse {
                W1v_New_ServiceInstallation_Response = new[] {
                    new W1v_New_ServiceInstallationInfoRecord {
                        SAPStatus = "Success?",
                        WorkOrderNumber = "1234817"
                    }
                }
            };
            _sapClient.Setup(x =>
                           x.W1v_New_ServiceInstallation_Get_OB_SYNAsync(It.IsAny<W1v_New_ServiceInstallation_Get_OB_SYNRequest>()).Result)
                      .Returns(response);

            _target.Invoke(_targetEntity);

            Assert.AreEqual("Success?", _targetEntity.SAPStatus);
            Assert.AreEqual("1234817", _targetEntity.WorkOrderNumber);
        }

        #region Test class

        public class TestInvoker : SAPNewServiceInstallationInvoker
        {
            public W1v_New_ServiceInstallation_Get_OB_SYN TargetClient { get; set; }

            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

            protected override W1v_New_ServiceInstallation_Get_OB_SYN CastServiceClientToChannel(
                W1v_New_ServiceInstallation_Get_OB_SYNClient serviceClient)
            {
                return TargetClient;
            }
        }

        #endregion
    }
}
