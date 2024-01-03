using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Models.ViewModels.WasteWaterSystems;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class WasteWaterSystemViewModelTest : ViewModelTestBase<WasteWaterSystem, WasteWaterSystemViewModel>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.BusinessUnit, GetEntityFactory<BusinessUnit>().Create());
            _vmTester.CanMapBothWays(x => x.Status, GetEntityFactory<WasteWaterSystemStatus>().Create());
            _vmTester.CanMapBothWays(x => x.Ownership, GetEntityFactory<WasteWaterSystemOwnership>().Create());
            _vmTester.CanMapBothWays(x => x.Type, GetEntityFactory<WasteWaterSystemType>().Create());
            _vmTester.CanMapBothWays(x => x.SubType, GetEntityFactory<WasteWaterSystemSubType>().Create());

            _vmTester.CanMapBothWays(x => x.WasteWaterSystemName);
            _vmTester.CanMapBothWays(x => x.PermitNumber);
            _vmTester.CanMapBothWays(x => x.DateOfOwnership);
            _vmTester.CanMapBothWays(x => x.DateOfResponsibility);
            _vmTester.CanMapBothWays(x => x.GravityLength);
            _vmTester.CanMapBothWays(x => x.ForceLength);
            _vmTester.CanMapBothWays(x => x.NumberOfLiftStations, 10, 11);
            _vmTester.CanMapBothWays(x => x.TreatmentDescription);
            _vmTester.CanMapBothWays(x => x.NumberOfCustomers);
            _vmTester.CanMapBothWays(x => x.PeakFlowMGD);
            _vmTester.CanMapBothWays(x => x.IsCombinedSewerSystem);
            _vmTester.CanMapBothWays(x => x.HasConsentOrder);
            _vmTester.CanMapBothWays(x => x.ConsentOrderStartDate);
            _vmTester.CanMapBothWays(x => x.ConsentOrderEndDate);
            _vmTester.CanMapBothWays(x => x.NewSystemInitialSafetyAssessmentCompleted);
            _vmTester.CanMapBothWays(x => x.DateSafetyAssessmentActionItemsCompleted);
            _vmTester.CanMapBothWays(x => x.NewSystemInitialWQEnvAssessmentCompleted);
            _vmTester.CanMapBothWays(x => x.DateWQEnvAssessmentActionItemsCompleted);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.WasteWaterSystemName, WasteWaterSystem.StringLengths.WASTE_WATER_SYSTEM_NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.PermitNumber, WasteWaterSystem.StringLengths.PERMIT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.TreatmentDescription, WasteWaterSystem.StringLengths.TREATMENT_DESCRIPTION);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.BusinessUnit);
            ValidationAssert.PropertyIsRequired(x => x.WasteWaterSystemName);
            ValidationAssert.PropertyIsRequired(x => x.PermitNumber);
            ValidationAssert.PropertyIsRequired(x => x.Status);
            ValidationAssert.PropertyIsRequired(x => x.Ownership);
            ValidationAssert.PropertyIsRequired(x => x.Type);
            ValidationAssert.PropertyIsRequired(x => x.SubType);
            ValidationAssert.PropertyIsRequired(x => x.HasConsentOrder);

            ValidationAssert.PropertyIsRequiredWhen(
                x => x.ConsentOrderStartDate, 
                DateTime.Now,
                x => x.HasConsentOrder, 
                true, 
                false);

            ValidationAssert.PropertyIsRequiredWhen(
                x => x.ConsentOrderEndDate, 
                DateTime.Now,
                x => x.ConsentOrderStartDate, 
                DateTime.Now, 
                null);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.BusinessUnit, GetEntityFactory<BusinessUnit>().Create());
            ValidationAssert.EntityMustExist(x => x.Status, GetEntityFactory<WasteWaterSystemStatus>().Create());
            ValidationAssert.EntityMustExist(x => x.Ownership, GetEntityFactory<WasteWaterSystemOwnership>().Create());
            ValidationAssert.EntityMustExist(x => x.Type, GetEntityFactory<WasteWaterSystemType>().Create());
            ValidationAssert.EntityMustExist(x => x.SubType, GetEntityFactory<WasteWaterSystemSubType>().Create());
        }

        #endregion

        #endregion
    }
}