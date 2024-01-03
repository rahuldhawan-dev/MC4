using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SAPGoodsIssueCollectionTest
    {
        #region Tests

        [TestMethod]
        public void TestMapToWorkOrderDoesNotMapNullValues()
        {
            var materialDocID = "123455";
            var sapErrorCode = "Success";
            var workOrder = new WorkOrder {MaterialsDocID = materialDocID, SAPErrorCode = sapErrorCode};
            var target = new SAPGoodsIssueCollection();
            target.Items.Add(new SAPGoodsIssue());

            target.MapToWorkOrder(workOrder);

            Assert.AreEqual(materialDocID, workOrder.MaterialsDocID);
            Assert.AreEqual(sapErrorCode, workOrder.SAPErrorCode);
        }

        [TestMethod]
        public void TestMapToWorkOrderDoesNotMapEmptyValues()
        {
            var materialDocID = "123455";
            var sapErrorCode = "Success";
            var workOrder = new WorkOrder {MaterialsDocID = materialDocID, SAPErrorCode = sapErrorCode};
            var target = new SAPGoodsIssueCollection();
            target.Items.Add(new SAPGoodsIssue {Status = "", MaterialDocument = ""});

            target.MapToWorkOrder(workOrder);

            Assert.AreEqual(materialDocID, workOrder.MaterialsDocID);
            Assert.AreEqual(sapErrorCode, workOrder.SAPErrorCode);
        }

        [TestMethod]
        public void TestTwoRecordsDoesNotBreakAnything()
        {
            var materialDocID = "4903008657";
            var workOrder = new WorkOrder();
            var target = new SAPGoodsIssueCollection();
            target.Items.Add(new SAPGoodsIssue
                {Status = "Created Goods Issue Successfully", MaterialDocument = materialDocID});
            target.Items.Add(new SAPGoodsIssue
                {Status = "Status of order 90696906 does not allow changes", MaterialDocument = materialDocID});

            target.MapToWorkOrder(workOrder);

            Assert.AreEqual(materialDocID, workOrder.MaterialsDocID);

            target = new SAPGoodsIssueCollection();
            target.Items.Add(new SAPGoodsIssue
                {Status = "Status of order 90696906 does not allow changes", MaterialDocument = materialDocID});
            target.Items.Add(new SAPGoodsIssue
                {Status = "Created Goods Issue Successfully", MaterialDocument = materialDocID});

            target.MapToWorkOrder(workOrder);

            Assert.AreEqual(materialDocID, workOrder.MaterialsDocID);
        }

        #endregion
    }
}
