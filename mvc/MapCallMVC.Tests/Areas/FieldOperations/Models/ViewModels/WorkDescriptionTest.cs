using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using NHibernate.Engine;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class WorkDescriptionViewModelTest : ViewModelTestBase<WorkDescription, EditWorkDescription>
    {
        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AccountingType);
            _vmTester.CanMapBothWays(x => x.FirstRestorationCostBreakdown);
            _vmTester.CanMapBothWays(x => x.FirstRestorationProductCode);
            _vmTester.CanMapBothWays(x => x.SecondRestorationCostBreakdown);
            _vmTester.CanMapBothWays(x => x.SecondRestorationProductCode);
            _vmTester.CanMapBothWays(x => x.TimeToComplete);
            _vmTester.CanMapBothWays(x => x.ShowBusinessUnit);
            _vmTester.CanMapBothWays(x => x.ShowApprovalAccounting);
            _vmTester.CanMapBothWays(x => x.MaintenanceActivityType);
            _vmTester.CanMapBothWays(x => x.PlantMaintenanceActivityType);
            _vmTester.CanMapBothWays(x => x.MarkoutRequired);
            _vmTester.CanMapBothWays(x => x.MaterialsRequired);
            _vmTester.CanMapBothWays(x => x.JobSiteCheckListRequired);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FirstRestorationCostBreakdown);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FirstRestorationAccountingCode);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FirstRestorationProductCode);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TimeToComplete);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ShowBusinessUnit);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ShowApprovalAccounting);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PlantMaintenanceActivityType);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.SecondRestorationAccountingCode);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.SecondRestorationCostBreakdown);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.SecondRestorationProductCode);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.AccountingType);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.MaintenanceActivityType);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.MarkoutRequired);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.MaterialsRequired);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.JobSiteCheckListRequired);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MaintenanceActivityType, WorkDescription.StringLengths.MAINT_ACT_TYPE);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.FirstRestorationProductCode, GetEntityFactory<RestorationProductCode>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.SecondRestorationProductCode, GetEntityFactory<RestorationProductCode>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.AccountingType, GetEntityFactory<AccountingType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.PlantMaintenanceActivityType, GetEntityFactory<PlantMaintenanceActivityType>().Create());
        }

        #endregion
    } 
}
