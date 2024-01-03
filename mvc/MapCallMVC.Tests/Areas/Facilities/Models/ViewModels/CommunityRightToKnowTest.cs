using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    [TestClass]
    public abstract class CommunityRightToKnowTestBase<TViewModel> : ViewModelTestBase<CommunityRightToKnow, TViewModel> where TViewModel : CommunityRightToKnowViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CommunityRightToKnowFacilityId);
            _vmTester.CanMapBothWays(x => x.CoMu);
            _vmTester.CanMapBothWays(x => x.NorthAmericanIndustryClassificationSystem);
            _vmTester.CanMapBothWays(x => x.NumberOfEmployees);
            _vmTester.CanMapBothWays(x => x.FacilityEmergencyContactName);
            _vmTester.CanMapBothWays(x => x.FacilityEmergencyContactPhoneNumber);
            _vmTester.CanMapBothWays(x => x.FacilityEmergencyContactEmergencyPhoneNumber);
            _vmTester.CanMapBothWays(x => x.FacilityEmergencyContactTitle);
            _vmTester.CanMapBothWays(x => x.FacilityHasNJCRTKHazardousSubstancesAboveThresholds);
            _vmTester.CanMapBothWays(x => x.FacilityHasNJCRTKHazardousSubstancesInAnyQuantity);
            _vmTester.CanMapBothWays(x => x.RDExemptionApprovalNumber);
            _vmTester.CanMapBothWays(x => x.RiskManagementPlanFacilityId);
            _vmTester.CanMapBothWays(x => x.ToxinsReleaseInventoryFacilityId);
            _vmTester.CanMapBothWays(x => x.MaximumNumberOfOccupants);
            _vmTester.CanMapBothWays(x => x.LocationIsManned);
            _vmTester.CanMapBothWays(x => x.IsSubjectToChemicalAccidentPrevention);
            _vmTester.CanMapBothWays(x => x.IsSubjectToEmergencyPlanning);
            _vmTester.CanMapBothWays(x => x.SubmissionDate);
            _vmTester.CanMapBothWays(x => x.ExpirationDate);
            _vmTester.CanMapBothWays(x => x.Facility, GetEntityFactory<Facility>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // setting values up here so Regex validation doesn't show up during model state check
            var i = 0;
            _viewModel.CommunityRightToKnowFacilityId = i.ToString().PadLeft(50, '0');
            _viewModel.CoMu = i.ToString().PadLeft(50, '0');
            _viewModel.NorthAmericanIndustryClassificationSystem = i.ToString().PadLeft(50, '0');

            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CommunityRightToKnowFacilityId, Facility.StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID, true);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CoMu, Facility.StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID, true);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FacilityEmergencyContactName, Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FacilityEmergencyContactTitle, Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_TITLE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FacilityEmergencyContactPhoneNumber, Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.FacilityEmergencyContactEmergencyPhoneNumber, Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.NorthAmericanIndustryClassificationSystem, Facility.StringLengths.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM, true);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.RDExemptionApprovalNumber, Facility.StringLengths.RD_EXEMPTION_APPROVAL_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.RiskManagementPlanFacilityId, Facility.StringLengths.RISK_MANAGEMENT_PLAN_FACILITY_ID);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ToxinsReleaseInventoryFacilityId, Facility.StringLengths.TOXINS_RELEASE_INVENTORY_FACILITY_ID);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Facility);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation() { }
        
        #endregion
    }
}
