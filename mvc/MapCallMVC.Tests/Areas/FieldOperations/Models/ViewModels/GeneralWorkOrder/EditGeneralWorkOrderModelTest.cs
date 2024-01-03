using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class EditGeneralWorkOrderModelTest : WorkOrderViewModelTestBase<EditGeneralWorkOrderModel>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.MeterLocation, GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" }));
            _vmTester.CanMapBothWays(x => x.TownSection);
            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.NearestCrossStreet);
            _vmTester.CanMapBothWays(x => x.ZipCode);
            _vmTester.CanMapBothWays(x => x.Purpose);
            _vmTester.CanMapBothWays(x => x.Priority);
            _vmTester.CanMapBothWays(x => x.AlertIssued);
            _vmTester.CanMapBothWays(x => x.SignificantTrafficImpact);
            _vmTester.CanMapBothWays(x => x.MarkoutRequirement);
            _vmTester.CanMapBothWays(x => x.TrafficControlRequired);
            _vmTester.CanMapBothWays(x => x.StreetOpeningPermitRequired);
            _vmTester.CanMapBothWays(x => x.DigitalAsBuiltRequired);
            _vmTester.CanMapBothWays(x => x.DigitalAsBuiltCompleted);
            _vmTester.CanMapBothWays(x => x.WorkDescription);
            _vmTester.CanMapBothWays(x => x.RequestingEmployee);
            _vmTester.CanMapBothWays(x => x.CustomerName);
            _vmTester.CanMapBothWays(x => x.PhoneNumber);
            _vmTester.CanMapBothWays(x => x.SecondaryPhoneNumber);
            _vmTester.CanMapBothWays(x => x.PlannedCompletionDate);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.MarkoutRequirement);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.MeterLocation, GetFactory<InsideMeterLocationFactory>().Create(new { SAPCode = "c1" }));
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.ZipCode, WorkOrder.StringLengths.ZIP_CODE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CustomerName, WorkOrder.StringLengths.CUSTOMER_NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.SecondaryPhoneNumber, WorkOrder.StringLengths.SECONDARY_PHONE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.PhoneNumber, WorkOrder.StringLengths.PHONE_NUMBER);
        }

        #endregion
    }
}
