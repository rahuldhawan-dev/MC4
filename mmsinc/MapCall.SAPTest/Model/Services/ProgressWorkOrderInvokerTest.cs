using System;
using System.Threading.Tasks;
using MapCall.SAP.service;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class ProgressWorkOrderInvokerTest
    {
        #region Fields

        private TestInvoker _target;
        private SAPProgressWorkOrder _targetEntity;
        private SAPHttpClient _sapHttpClient;
        private Mock<ProgressWorkOrder_OB_SYN> _sapClient;

        #endregion

        #region Test Initialize

        [TestInitialize]
        public void InitializeTest()
        {
            _sapHttpClient = new SAPHttpClient();
            _sapClient = new Mock<ProgressWorkOrder_OB_SYN>();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new SAPProgressWorkOrder();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestTimeOutIsSetByConstructor()
        {
            Assert.AreEqual(1, _target.SendTimeOut.Value.Minutes);
        }

        [TestMethod]
        public void TestInvokeCatchesErrorAndSetsSAPErrorCodeToRetry()
        {
            _target.Invoke(_targetEntity);

            Assert.IsTrue(_targetEntity.Status.StartsWith(SAPHttpClient.RETRY_ERROR_TEXT));
        }

        [TestMethod]
        public void TestInvokeSetsValuesOnEntityFromResponse()
        {
            var response = new ProgressWorkOrder_OB_SYNResponse();
            var status = new ProgressWorkOrderStatusStatus {
                MaterialDocument = "1234",
                NotificationNumber = "9101112", 
                OrderNumber = "4567",
                Status = "Success",
                WBSElement = "goWBS!",
                CostCenter = "123456"
            };
            response.ProgressWorkOrderResponse = new[] { status };
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.ProgressWorkOrder_OB_SYNAsync(It.IsAny<ProgressWorkOrder_OB_SYNRequest>()))
                      .Returns(task);
        
            _target.Invoke(_targetEntity);
            
            Assert.AreEqual("1234", _targetEntity.MaterialDocument);
            Assert.AreEqual("9101112", _targetEntity.NotificationNumber);
            Assert.AreEqual("4567", _targetEntity.OrderNumber);
            Assert.AreEqual("Success", _targetEntity.Status);
            Assert.AreEqual("goWBS!", _targetEntity.WBSElement);
            Assert.AreEqual("123456", _targetEntity.CostCenter);
        }
        
        [TestMethod]
        public void TestInvokeSetsStatusToNoDataFoundWhenNoResponse()
        {
            var response = new ProgressWorkOrder_OB_SYNResponse();
            response.ProgressWorkOrderResponse = Array.Empty<ProgressWorkOrderStatusStatus>();
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.ProgressWorkOrder_OB_SYNAsync(It.IsAny<ProgressWorkOrder_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);
            
            Assert.AreEqual(ProgressWorkOrderInvoker.NO_DATA_FOUND, _targetEntity.Status);
        }

        #endregion

        private class TestInvoker : ProgressWorkOrderInvoker
        {
            public ProgressWorkOrder_OB_SYN TargetClient { get; set; }
            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

            protected override ProgressWorkOrder_OB_SYN CastServiceClientToChannel(
                ProgressWorkOrder_OB_SYNClient serviceClient)
            {
                return TargetClient;
            }
        }
    }
}