using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class EditWorkDescriptionTest : ViewModelTestBase<WorkDescription, EditWorkDescription>
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AccountingType);
            _vmTester.CanMapBothWays(x => x.DigitalAsBuiltRequired);
            _vmTester.CanMapBothWays(x => x.FirstRestorationAccountingCode);
            _vmTester.CanMapBothWays(x => x.FirstRestorationCostBreakdown);
            _vmTester.CanMapBothWays(x => x.FirstRestorationProductCode);
            _vmTester.CanMapBothWays(x => x.JobSiteCheckListRequired);
            _vmTester.CanMapBothWays(x => x.MaintenanceActivityType);
            _vmTester.CanMapBothWays(x => x.MarkoutRequired);
            _vmTester.CanMapBothWays(x => x.MaterialsRequired);
            _vmTester.CanMapBothWays(x => x.PlantMaintenanceActivityType);
            _vmTester.CanMapBothWays(x => x.SecondRestorationAccountingCode);
            _vmTester.CanMapBothWays(x => x.SecondRestorationCostBreakdown);
            _vmTester.CanMapBothWays(x => x.SecondRestorationProductCode);
            _vmTester.CanMapBothWays(x => x.ShowApprovalAccounting);
            _vmTester.CanMapBothWays(x => x.ShowBusinessUnit);
            _vmTester.CanMapBothWays(x => x.TimeToComplete);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.FirstRestorationAccountingCode)
               .PropertyIsRequired(x => x.FirstRestorationCostBreakdown)
               .PropertyIsRequired(x => x.FirstRestorationProductCode)
               .PropertyIsRequired(x => x.PlantMaintenanceActivityType)
               .PropertyIsRequired(x => x.ShowApprovalAccounting)
               .PropertyIsRequired(x => x.ShowBusinessUnit)
               .PropertyIsRequired(x => x.TimeToComplete);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<RestorationAccountingCode>(x => x.FirstRestorationAccountingCode)
               .EntityMustExist<RestorationProductCode>(x => x.FirstRestorationProductCode)
               .EntityMustExist<RestorationAccountingCode>(x => x.SecondRestorationAccountingCode)
               .EntityMustExist<RestorationProductCode>(x => x.SecondRestorationProductCode)
               .EntityMustExist<AccountingType>(x => x.AccountingType)
               .EntityMustExist<PlantMaintenanceActivityType>(x => x.PlantMaintenanceActivityType);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(
                x => x.MaintenanceActivityType,
                WorkDescription.StringLengths.MAINT_ACT_TYPE);
        }
    }
}
