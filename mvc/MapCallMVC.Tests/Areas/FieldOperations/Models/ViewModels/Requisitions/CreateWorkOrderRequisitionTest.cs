using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Requisitions
{
    [TestClass]
    public class CreateWorkOrderRequisitionTest : WorkOrderRequisitionViewModelTestBase<CreateWorkOrderRequisition>
    {
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create());
        }
        
        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder);
        }

        [TestMethod]
        public void TestMapToEntiyAddsRequisitionToWorkOrder()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            _viewModel.WorkOrder = workOrder.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(workOrder.Requisitions.Contains(_entity));
        }
    }
}
