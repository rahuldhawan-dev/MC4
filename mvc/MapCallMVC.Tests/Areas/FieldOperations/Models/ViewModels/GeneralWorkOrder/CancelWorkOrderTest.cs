using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class CancelWorkOrderTest : ViewModelTestBase<WorkOrder, CancelWorkOrder>
    {
        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsFields()
        {
            var expectedDate = DateTime.Now;
            var cancellationReason = GetFactory<WorkOrderCancellationReasonFactory>().Create();

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _viewModel.WorkOrderCancellationReason = cancellationReason.Id;
            _entity.AssignedContractor = GetFactory<ContractorFactory>().Create();
            _entity.AssignedToContractorOn = DateTime.Now;

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.CancelledAt.Value);
            Assert.AreEqual(cancellationReason.Id, _entity.WorkOrderCancellationReason.Id);
            Assert.AreEqual(cancellationReason.Description, _entity.WorkOrderCancellationReason.Description);
            Assert.AreEqual(cancellationReason.Status, _entity.WorkOrderCancellationReason.Status);
            Assert.IsNull(_entity.AssignedToContractorOn);
            Assert.IsNull(_entity.AssignedContractor);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays() { }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.WorkOrderCancellationReason);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.WorkOrderCancellationReason, GetEntityFactory<WorkOrderCancellationReason>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate string length
        }

        #endregion
    }
}
