using System;
using System.Threading.Tasks;
using MapCall.SAP.service;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Services;
using MapCall.SAP.service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.SAPTest.Model.Services
{
    [TestClass]
    public class CreateWorkOrderInvokerTest
    {
        #region Fields

        private TestInvoker _target;
        private SAPWorkOrder _targetEntity;
        private SAPHttpClient _sapHttpClient;
        private Mock<CreateWorkOrder_OB_SYN> _sapClient;

        #endregion

        #region Test Initialize

        [TestInitialize]
        public void InitializeTest()
        {
            _sapHttpClient = new SAPHttpClient();
            _sapClient = new Mock<CreateWorkOrder_OB_SYN>();
            _target = new TestInvoker(_sapHttpClient);
            _target.TargetClient = _sapClient.Object;
            _targetEntity = new SAPWorkOrder();
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
            
            Assert.IsTrue(_targetEntity.SAPErrorCode.StartsWith(SAPHttpClient.RETRY_ERROR_TEXT));
        }
        
        [TestMethod]
        public void TestInvokeSetsValuesOnEntityFromResponse()
        {
            var response = new CreateWorkOrder_OB_SYNResponse();
            var status = new WorkOrderStatusStatus {
                OrderNumber = "1234",
                EquipmentNo = "5678", 
                NotificationNumber = "9101112", 
                WBSElement = "goWBS!", 
                Status = "Success",
                CostCenter = "123456"
            };
            response.WorkOrderResponse = new[] { status };
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.CreateWorkOrder_OB_SYNAsync(It.IsAny<CreateWorkOrder_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);
            
            Assert.AreEqual("Success", _targetEntity.SAPErrorCode);
            Assert.AreEqual("1234", _targetEntity.OrderNumber);
            Assert.AreEqual("5678", _targetEntity.EquipmentNo);
            Assert.AreEqual("9101112", _targetEntity.NotificationNumber);
            Assert.AreEqual("goWBS!", _targetEntity.WBSElement);
            Assert.AreEqual("123456", _targetEntity.CostCenter);
        }
                
        [TestMethod]
        public void TestInvokeDoesNotOverwriteEquipmentNoIfEmpty()
        {
            _targetEntity.EquipmentNo = "123456";
            var response = new CreateWorkOrder_OB_SYNResponse();
            var status = new WorkOrderStatusStatus {
                OrderNumber = "1234",
                NotificationNumber = "9101112", 
                WBSElement = "goWBS!", 
                Status = "Success"
            };
            response.WorkOrderResponse = new[] { status };
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.CreateWorkOrder_OB_SYNAsync(It.IsAny<CreateWorkOrder_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);
            
            Assert.AreEqual("123456", _targetEntity.EquipmentNo);
        }
        
        [TestMethod]
        public void TestInvokeSetsSAPErrorCodeToNoDataFoundWhenNoResponse()
        {
            _targetEntity.EquipmentNo = "123456";
            var response = new CreateWorkOrder_OB_SYNResponse();
            response.WorkOrderResponse = Array.Empty<WorkOrderStatusStatus>();
            var task = Task.FromResult(response);
            _sapClient.Setup(x =>
                           x.CreateWorkOrder_OB_SYNAsync(It.IsAny<CreateWorkOrder_OB_SYNRequest>()))
                      .Returns(task);

            _target.Invoke(_targetEntity);
            
            Assert.AreEqual(CreateWorkOrderInvoker.NO_DATA_FOUND, _targetEntity.SAPErrorCode);
        }
        
        #endregion

        private class TestInvoker : CreateWorkOrderInvoker
        {
            public CreateWorkOrder_OB_SYN TargetClient { get; set; }
            public TestInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }
            protected override CreateWorkOrder_OB_SYN CastServiceClientToChannel(CreateWorkOrder_OB_SYNClient serviceClient)
            {
                return TargetClient;
            }
        }
    }
}