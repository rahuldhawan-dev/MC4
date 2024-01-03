using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets;

namespace MapCallMVC.Tests.Areas.Engineering.Models.ViewModels
{
    public abstract class RiskRegisterAssetViewModelTest<TViewModel> : ViewModelTestBase<RiskRegisterAsset, TViewModel> where TViewModel : RiskRegisterAssetViewModel 
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.RiskRegisterAssetGroup, GetEntityFactory<RiskRegisterAssetGroup>().Create());
            _vmTester.CanMapBothWays(x => x.RiskRegisterAssetCategory, GetEntityFactory<RiskRegisterAssetCategory>().Create());
            _vmTester.CanMapBothWays(x => x.State, GetEntityFactory<State>().Create());
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            _vmTester.CanMapBothWays(x => x.Facility, GetEntityFactory<Facility>().Create());
            _vmTester.CanMapBothWays(x => x.Equipment, GetEntityFactory<Equipment>().Create());
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            _vmTester.CanMapBothWays(x => x.Employee, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.ImpactDescription);
            _vmTester.CanMapBothWays(x => x.RiskDescription);
            _vmTester.CanMapBothWays(x => x.RiskQuadrant);
            _vmTester.CanMapBothWays(x => x.IdentifiedAt);
            _vmTester.CanMapBothWays(x => x.InterimMitigationMeasuresTaken);
            _vmTester.CanMapBothWays(x => x.InterimMitigationMeasuresTakenAt);
            _vmTester.CanMapBothWays(x => x.InterimMitigationMeasuresTakenEstimatedCosts);
            _vmTester.CanMapBothWays(x => x.FinalMitigationMeasuresTaken);
            _vmTester.CanMapBothWays(x => x.FinalMitigationMeasuresTakenAt);
            _vmTester.CanMapBothWays(x => x.FinalMitigationMeasuresTakenEstimatedCosts);
            _vmTester.CanMapBothWays(x => x.CompletionTargetDate);
            _vmTester.CanMapBothWays(x => x.CompletionActualDate);
            _vmTester.CanMapBothWays(x => x.IsProjectInComprehensivePlanningStudy);
            _vmTester.CanMapBothWays(x => x.IsProjectInCapitalPlan);
            _vmTester.CanMapBothWays(x => x.RelatedWorkBreakdownStructure);
            _vmTester.CanMapBothWays(x => x.CofMax);
            _vmTester.CanMapBothWays(x => x.LofMax);
            _vmTester.CanMapBothWays(x => x.TotalRiskWeighted);
            _vmTester.CanMapBothWays(x => x.RiskRegisterId);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.ImpactDescription, RiskRegisterAsset.StringLengths.IMPACT_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(x => x.RiskDescription, RiskRegisterAsset.StringLengths.RISK_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(x => x.InterimMitigationMeasuresTaken, RiskRegisterAsset.StringLengths.INTERIM_MITIGATION_MEASURES_TAKEN);
            ValidationAssert.PropertyHasMaxStringLength(x => x.FinalMitigationMeasuresTaken, RiskRegisterAsset.StringLengths.FINAL_MITIGATION_MEASURES_TAKEN);
            ValidationAssert.PropertyHasMaxStringLength(x => x.RelatedWorkBreakdownStructure, RiskRegisterAsset.StringLengths.RELATED_WORK_BREAKDOWN_STRUCTURE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.RiskRegisterId, RiskRegisterAsset.StringLengths.RISK_REGISTER_ID);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.RiskRegisterAssetGroup);
            ValidationAssert.PropertyIsRequired(x => x.CofMax);
            ValidationAssert.PropertyIsRequired(x => x.LofMax);
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.IdentifiedAt);
            ValidationAssert.PropertyIsRequired(x => x.Employee);
            ValidationAssert.PropertyIsRequired(x => x.TotalRiskWeighted);
            ValidationAssert.PropertyIsRequired(x => x.RiskQuadrant);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.RiskRegisterAssetGroup, GetEntityFactory<RiskRegisterAssetGroup>().Create());
            ValidationAssert.EntityMustExist(x => x.RiskRegisterAssetCategory, GetEntityFactory<RiskRegisterAssetCategory>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            ValidationAssert.EntityMustExist(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.Equipment, GetEntityFactory<Equipment>().Create());
            ValidationAssert.EntityMustExist(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            ValidationAssert.EntityMustExist(x => x.Employee, GetEntityFactory<Employee>().Create());
        }

        #endregion

        #endregion
    }
}