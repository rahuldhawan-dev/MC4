using System;
using MapCall.SAP.CreateBPEMWS;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using MapCall.SAP.SAPEquipmentWS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class GetEquipmentResponseServiceInvokerTest
    {
        #region Fields

        private TestInvoker _target;
        private SAPEquipment _targetEntity;
        private SAPHttpClient _sapHttpClient;
        private Mock<Equipments_OB_SYN> _sapClient;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _sapHttpClient = new SAPHttpClient();
            _sapClient = new Mock<Equipments_OB_SYN>();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new SAPEquipment();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestInvokeSetsBPEMRequestOnQuery()
        {
            Equipments_OB_SYNRequest request = null;
            var response = new Equipments_OB_SYNResponse();
            _sapClient.Setup(x => x.Equipments_OB_SYN(It.IsAny<Equipments_OB_SYNRequest>()))
                      .Returns(response)
                      .Callback((Equipments_OB_SYNRequest x) => { request = x; });

            _target.Invoke(_targetEntity);

            Assert.IsNotNull(request, "The request should have been made");
            Assert.IsNotNull(request.Equipments_Request, "This property should have been set.");
        }

        [TestMethod]
        public void TestInvokeDoesNotSetStatusOrSAPEquipmentNumberIfResponseIsEmpty()
        {
            var response = new Equipments_OB_SYNResponse {
                Equipments_Response = new SAPEquipmentStatusEquipments[0]
            };

            _sapClient.Setup(x => x.Equipments_OB_SYN(It.IsAny<Equipments_OB_SYNRequest>()))
                      .Returns(response);

            _target.Invoke(_targetEntity);

            Assert.IsNull(_targetEntity.Status);
            Assert.IsNull(_targetEntity.SAPEquipmentNumber);
        }

        [TestMethod]
        public void TestInvokeSetsStatusAndSAPErrorCodeToFirstResponseStatus()
        {
            var status1 = new SAPEquipmentStatusEquipments {Status = "Status 1"};
            var status2 = new SAPEquipmentStatusEquipments {Status = "Status 2"};
            var response = new Equipments_OB_SYNResponse {
                Equipments_Response = new[] {status1, status2}
            };

            _sapClient.Setup(x => x.Equipments_OB_SYN(It.IsAny<Equipments_OB_SYNRequest>()))
                      .Returns(response);

            _target.Invoke(_targetEntity);

            Assert.AreEqual("Status 1", _targetEntity.Status);
            Assert.AreEqual("Status 1", _targetEntity.SAPErrorCode);
        }

        [TestMethod]
        public void TestInvokeSetsSAPEquipmentNumberToFirstResponseSAPEquipmentNumber()
        {
            var status1 = new SAPEquipmentStatusEquipments {SAPEquipmentNo = "0012345"};
            var status2 = new SAPEquipmentStatusEquipments {SAPEquipmentNo = "99999"};
            var response = new Equipments_OB_SYNResponse {
                Equipments_Response = new[] {status1, status2}
            };

            _sapClient.Setup(x => x.Equipments_OB_SYN(It.IsAny<Equipments_OB_SYNRequest>()))
                      .Returns(response);

            _target.Invoke(_targetEntity);

            Assert.AreEqual("12345", _targetEntity.SAPEquipmentNumber);
        }

        [TestMethod]
        public void TestOnExceptionSetsStatusToExceptionMessageWhenExceptionisThrown()
        {
            var expectedException = new Exception("Whoops");
            _target.CallOnException(_targetEntity, expectedException);
            Assert.AreEqual("RETRY::System.Exception: Whoops", _targetEntity.Status);
        }

        [TestMethod]
        public void TestAfterInvokeSetsSAPErrorCodeToSameValueAsStatus()
        {
            _targetEntity.Status = "This is a status";
            Assert.IsNull(_targetEntity.SAPErrorCode, "Sanity. This should be null.");

            _target.CallAfterInvoke(_targetEntity);

            Assert.AreEqual("This is a status", _targetEntity.SAPErrorCode);
        }

        #endregion

        #region Test class

        private class TestInvoker : GetEquipmentResponseServiceInvoker
        {
            public Equipments_OB_SYN TargetClient { get; set; }

            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

            protected override Equipments_OB_SYN CastServiceClientToChannel(Equipments_OB_SYNClient serviceClient)
            {
                return TargetClient;
            }

            public void CallAfterInvoke(SAPEquipment sapEntity)
            {
                AfterInvoke(sapEntity);
            }

            public void CallOnException(SAPEquipment sapEntity, Exception ex)
            {
                OnException(sapEntity, ex);
            }
        }

        #endregion
    }
}
