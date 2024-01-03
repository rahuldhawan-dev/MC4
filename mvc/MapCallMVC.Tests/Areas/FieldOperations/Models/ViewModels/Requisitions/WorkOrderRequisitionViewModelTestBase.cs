using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Requisitions
{
    public abstract class WorkOrderRequisitionViewModelTestBase<TViewModel> : ViewModelTestBase<Requisition, TViewModel> where TViewModel : WorkOrderRequisitionViewModelBase
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.RequisitionType);
            _vmTester.CanMapBothWays(x => x.SAPRequisitionNumber);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RequisitionType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SAPRequisitionNumber);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SAPRequisitionNumber, Requisition.StringLengths.SAP_REQUISITION_NUMBER_MAX_LENGTH);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.RequisitionType, GetEntityFactory<RequisitionType>().Create());
        }

        #endregion
    }
}
