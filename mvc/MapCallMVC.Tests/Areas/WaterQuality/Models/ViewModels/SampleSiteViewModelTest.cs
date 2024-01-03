using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    public abstract class SampleSiteViewModelTest<TViewModel> : ViewModelTestBase<SampleSite, TViewModel>
        where TViewModel : SampleSiteViewModel
    {
        #region Private Members

        protected Mock<IAuthenticationService<User>> _authenticationService;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected DateTime _now;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _container.GetInstance<ITestDataFactoryService>();
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply);
            _vmTester.CanMapBothWays(x => x.Status);
            _vmTester.CanMapBothWays(x => x.SampleSiteInactivationReason);
            _vmTester.CanMapBothWays(x => x.Premise);
            _vmTester.CanMapBothWays(x => x.Gradient);
            _vmTester.CanMapBothWays(x => x.Coordinate);
            _vmTester.CanMapBothWays(x => x.Town);
            _vmTester.CanMapBothWays(x => x.TownSection);
            _vmTester.CanMapBothWays(x => x.IsAlternateSite);

            _vmTester.CanMapBothWays(x => x.Availability);
            _vmTester.CanMapBothWays(x => x.IsComplianceSampleSite);
            _vmTester.CanMapBothWays(x => x.IsProcessSampleSite);
            _vmTester.CanMapBothWays(x => x.IsResearchSampleSite);
            _vmTester.CanMapBothWays(x => x.CollectionType);
            _vmTester.CanMapBothWays(x => x.SubCollectionType);
            _vmTester.CanMapBothWays(x => x.LocationType);
            _vmTester.CanMapBothWays(x => x.ParentSite);
            _vmTester.CanMapBothWays(x => x.CommonSiteName);
            _vmTester.CanMapBothWays(x => x.LocationNameDescription);
            _vmTester.CanMapBothWays(x => x.IsLimsLocation);
            _vmTester.CanMapBothWays(x => x.CustomerPlumbingMaterial);
            _vmTester.CanMapBothWays(x => x.FieldVerifiedServiceMaterial);
            _vmTester.CanMapBothWays(x => x.FieldVerifiedCustomerPlumbingMaterial);
            _vmTester.CanMapBothWays(x => x.PreviousMonitoringPeriod);
            _vmTester.CanMapBothWays(x => x.LocationType);
            _vmTester.CanMapBothWays(x => x.BactiSite);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.TownText);
            _vmTester.CanMapBothWays(x => x.ZipCode);
            _vmTester.CanMapBothWays(x => x.OutOfServiceArea);
            _vmTester.CanMapBothWays(x => x.AgencyId);
            _vmTester.CanMapBothWays(x => x.SampleSiteValidationStatus);
            _vmTester.CanMapBothWays(x => x.PointOfUseTreatmentTypeOtherReason);

            /* Lead Copper Site Properties */
            _vmTester.CanMapBothWays(x => x.LeadCopperSite);
            _vmTester.CanMapBothWays(x => x.LeadCopperTierClassification);
            _vmTester.CanMapBothWays(x => x.LeadCopperTierSampleCategory);
            _vmTester.CanMapBothWays(x => x.LeadCopperValidationMethod);
            _vmTester.CanMapBothWays(x => x.LeadCopperTierThreeExplanation);
            _vmTester.CanMapBothWays(x => x.CustomerName);
            _vmTester.CanMapBothWays(x => x.CustomerContactMethod);
            _vmTester.CanMapBothWays(x => x.CustomerHomePhone);
            _vmTester.CanMapBothWays(x => x.CustomerAltPhone);
            _vmTester.CanMapBothWays(x => x.CustomerEmail);
            _vmTester.CanMapBothWays(x => x.CustomerParticipationConfirmed);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.OperatingCenter)
               .PropertyIsRequired(x => x.Status)
               .PropertyIsRequired(x => x.Availability)
               .PropertyIsRequired(x => x.IsLimsLocation)
               .PropertyIsRequired(x => x.IsComplianceSampleSite)
               .PropertyIsRequired(x => x.IsProcessSampleSite)
               .PropertyIsRequired(x => x.IsResearchSampleSite)
               .PropertyIsRequired(x => x.SampleSiteAddressLocationType)
               .PropertyIsRequired(x => x.PublicWaterSupply)
               .PropertyIsRequired(x => x.IsAlternateSite)
               .PropertyIsRequired(x => x.AgencyId)
               .PropertyIsRequired(x => x.LocationNameDescription)
               .PropertyIsRequiredWhen(
                    x => x.PointOfUseTreatmentTypeOtherReason,
                    "because reasons",
                    x => x.SampleSitePointOfUseTreatmentType,
                    SampleSitePointOfUseTreatmentType.Indices.OTHER)
               .PropertyIsRequiredWhen(
                    x => x.Facility,
                    GetEntityFactory<Facility>().Create().Id,
                    x => x.SampleSiteAddressLocationType,
                    SampleSiteAddressLocationType.Indices.FACILITY)
               .PropertyIsRequiredWhen(
                    x => x.Valve,
                    GetEntityFactory<Valve>().Create().Id,
                    x => x.SampleSiteAddressLocationType,
                    SampleSiteAddressLocationType.Indices.VALVE)
               .PropertyIsRequiredWhen(
                    x => x.Hydrant,
                    GetEntityFactory<Hydrant>().Create().Id,
                    x => x.SampleSiteAddressLocationType,
                    SampleSiteAddressLocationType.Indices.HYDRANT)
               .PropertyIsRequiredWhen(
                    x => x.Premise,
                    GetEntityFactory<Premise>().Create().Id,
                    x => x.SampleSiteAddressLocationType,
                    SampleSiteAddressLocationType.Indices.PREMISE)
               .PropertyIsRequiredWhen(
                    x => x.ParentSite,
                    GetEntityFactory<SampleSite>().Create().Id,
                    x => x.LocationType,
                    SampleSiteLocationType.Indices.DOWNSTREAM,
                    SampleSiteLocationType.Indices.PRIMARY)
               .PropertyIsRequiredWhen(
                    x => x.ParentSite,
                    GetEntityFactory<SampleSite>().Create().Id,
                    x => x.LocationType,
                    SampleSiteLocationType.Indices.UPSTREAM,
                    SampleSiteLocationType.Indices.PRIMARY)
               .PropertyIsRequiredWhen(
                    x => x.ParentSite,
                    GetEntityFactory<SampleSite>().Create().Id,
                    x => x.LocationType,
                    SampleSiteLocationType.Indices.GROUNDWATER,
                    SampleSiteLocationType.Indices.PRIMARY)
               .PropertyIsRequiredWhen(
                    x => x.SampleSiteProfile,
                    GetEntityFactory<SampleSiteProfile>().Create().Id,
                    x => x.IsLimsLocation,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.LeadCopperTierClassification,
                    GetEntityFactory<SampleSiteLeadCopperTierClassification>().Create().Id,
                    x => x.LeadCopperSite,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.LeadCopperValidationMethod,
                    GetFactory<VisualConfirmationSampleSiteLeadCopperValidationMethodFactory>().Create().Id,
                    x => x.LeadCopperSite,
                    true,
                    false)
               .PropertyIsRequiredWhen(
                    x => x.LeadCopperTierThreeExplanation,
                    "free text field",
                    x => x.LeadCopperTierClassification,
                    GetFactory<TierThreeSampleSiteLeadCopperTierClassificationFactory>().Create().Id,
                    GetFactory<TierOneSampleSiteLeadCopperTierClassificationFactory>().Create().Id)
               .PropertyIsRequiredWhen(
                    x => x.LocationType, 
                    GetEntityFactory<SampleSiteLocationType>().Create().Id, 
                    x => x.BactiSite, 
                    true)
               .PropertyIsRequiredWhen(
                    x => x.CollectionType, 
                    GetEntityFactory<SampleSiteCollectionType>().Create().Id, 
                    x => x.BactiSite, 
                    true);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.AgencyId, SampleSite.StringLengths.AGENCY_ID)
               .PropertyHasMaxStringLength(x => x.CommonSiteName, SampleSite.StringLengths.COMMON_SITE_NAME)
               .PropertyHasMaxStringLength(x => x.LocationNameDescription, SampleSite.StringLengths.LOCATION_NAME_DESCRIPTION)
               .PropertyHasMaxStringLength(x => x.StreetNumber, SampleSite.StringLengths.STREET_NUMBER)
               .PropertyHasMaxStringLength(x => x.TownText, SampleSite.StringLengths.TOWN)
               .PropertyHasMaxStringLength(x => x.ZipCode, SampleSite.StringLengths.ZIP_CODE)
               .PropertyHasMaxStringLength(x => x.PointOfUseTreatmentTypeOtherReason, SampleSite.StringLengths.POINT_OF_USE_TREATMENT_TYPE_OTHER_REASON)
               .PropertyHasMaxStringLength(x => x.LimsFacilityId, SampleSite.StringLengths.LIMS_FACILITY_ID)
               .PropertyHasMaxStringLength(x => x.LimsPrimaryStationCode, SampleSite.StringLengths.LIMS_PRIMARY_STATION_CODE)
               .PropertyHasMaxStringLength(x => x.LimsSiteId, SampleSite.StringLengths.LIMS_SITE_ID)
               .PropertyHasMaxStringLength(x => x.LeadCopperTierThreeExplanation, SampleSite.StringLengths.LEAD_COPPER_TIER_THREE_EXPLANATION)
               .PropertyHasMaxStringLength(x => x.CustomerName, SampleSite.StringLengths.CUSTOMER_NAME)
               .PropertyHasMaxStringLength(x => x.CustomerHomePhone, SampleSite.StringLengths.CUSTOMER_HOME_PHONE)
               .PropertyHasMaxStringLength(x => x.CustomerAltPhone, SampleSite.StringLengths.CUSTOMER_ALT_PHONE)
               .PropertyHasMaxStringLength(x => x.CustomerEmail, SampleSite.StringLengths.CUSTOMER_EMAIL);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist(x => x.State, GetEntityFactory<State>().Create())
               .EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create())
               .EntityMustExist(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create())
               .EntityMustExist(x => x.Availability, GetEntityFactory<SampleSiteAvailability>().Create())
               .EntityMustExist(x => x.Status, GetEntityFactory<SampleSiteStatus>().Create())
               .EntityMustExist(x => x.SampleSiteInactivationReason, GetEntityFactory<SampleSiteInactivationReason>().Create())
               .EntityMustExist(x => x.County, GetEntityFactory<County>().Create())
               .EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create())
               .EntityMustExist(x => x.Valve, GetEntityFactory<Valve>().Create())
               .EntityMustExist(x => x.Hydrant, GetEntityFactory<Hydrant>().Create())
               .EntityMustExist(x => x.Gradient, GetEntityFactory<Gradient>().Create())
               .EntityMustExist(x => x.Premise, GetEntityFactory<Premise>().Create())
               .EntityMustExist(x => x.Coordinate, GetEntityFactory<Coordinate>().Create())
               .EntityMustExist(x => x.Town, GetEntityFactory<Town>().Create())
               .EntityMustExist(x => x.TownSection, GetEntityFactory<TownSection>().Create())
               .EntityMustExist(x => x.CustomerPlumbingMaterial, GetEntityFactory<ServiceMaterial>().Create())
               .EntityMustExist(x => x.LocationType, GetEntityFactory<SampleSiteLocationType>().Create())
               .EntityMustExist(x => x.ParentSite, GetEntityFactory<SampleSite>().Create())
               .EntityMustExist(x => x.CollectionType, GetEntityFactory<SampleSiteCollectionType>().Create())
               .EntityMustExist(x => x.SubCollectionType, GetEntityFactory<SampleSiteSubCollectionType>().Create())
               .EntityMustExist(x => x.SampleSiteValidationStatus, GetEntityFactory<SampleSiteValidationStatus>().Create())
               .EntityMustExist(x => x.SampleSiteProfile, GetEntityFactory<SampleSiteProfile>().Create())
               .EntityMustExist(x => x.LeadCopperValidationMethod, GetFactory<VisualConfirmationSampleSiteLeadCopperValidationMethodFactory>().Create())
               .EntityMustExist(x => x.LeadCopperTierClassification, GetFactory<TierOneSampleSiteLeadCopperTierClassificationFactory>().Create())
               .EntityMustExist(x => x.LeadCopperTierSampleCategory, GetFactory<SampleSiteLeadCopperTierSampleCategoryFactory>().Create())
               .EntityMustExist(x => x.CustomerContactMethod, GetFactory<EmailSampleSiteCustomerContactMethodFactory>().Create());
        }

        #region Validation

        [TestMethod]
        public void Test_Validate_SetsCorrectValidationErrorStateForPrimaryStationCode_WhenSampleSiteIsALimsLocation()
        {
            var stateIdsToTriggerError = SampleSiteViewModel.INVALID_STATES_FOR_LIMS_PRIMARY_STATION_CODE_REQUIRED_VALIDATION;

            _viewModel.IsLimsLocation = true;

            foreach (var stateId in State.AllStateIndicesValues())
            {
                _viewModel.State = stateId;

                if (stateIdsToTriggerError.Contains(stateId))
                {
                    ValidationAssert.ModelStateHasError(x => x.LimsPrimaryStationCode, SampleSiteViewModel.LIMS_PRIMARY_STATION_CODE_VALIDATION_ERROR);
                }
                else
                {
                    ValidationAssert.ModelStateIsValid(x => x.LimsPrimaryStationCode);
                }
            }
        }

        [TestMethod]
        public void Test_Validate_SetsCorrectValidationErrorStateForFacilityId_WhenSampleSiteIsALimsLocation()
        {
            var stateIdsToNotTriggerError = SampleSiteViewModel.VALID_STATES_FOR_LIMS_FACILITY_ID_REQUIRED_VALIDATION;

            _viewModel.IsLimsLocation = true;

            foreach (var stateId in State.AllStateIndicesValues())
            {
                _viewModel.State = stateId;

                if (stateIdsToNotTriggerError.Contains(stateId))
                {
                    ValidationAssert.ModelStateIsValid(x => x.LimsFacilityId);
                }
                else
                {
                    ValidationAssert.ModelStateHasError(x => x.LimsFacilityId, SampleSiteViewModel.LIMS_FACILITY_ID_VALIDATION_ERROR);
                }
            }
        }

        [TestMethod]
        public void Test_Validate_SetsCorrectValidationErrorStateForSiteId_WhenSampleSiteIsALimsLocation()
        {
            var stateIdsToNotTriggerError = SampleSiteViewModel.VALID_STATES_FOR_LIMS_SITE_ID_REQUIRED_VALIDATION;

            _viewModel.IsLimsLocation = true;

            foreach (var stateId in State.AllStateIndicesValues())
            {
                _viewModel.State = stateId;

                if (stateIdsToNotTriggerError.Contains(stateId))
                {
                    ValidationAssert.ModelStateIsValid(x => x.LimsSiteId);
                }
                else
                {
                    ValidationAssert.ModelStateHasError(x => x.LimsSiteId, SampleSiteViewModel.LIMS_SITE_ID_VALIDATION_ERROR);
                }
            }
        }

        [TestMethod]
        public void TestValidateAgencyIdReturnsTrueWhenPending()
        {
            GetFactory<ActiveSampleSiteStatusFactory>().Create();
            GetFactory<PendingSampleSiteStatusFactory>().Create();
            GetFactory<InactiveSampleSiteStatusFactory>().Create();

            _viewModel.Status = SampleSiteStatus.Indices.PENDING;
            ValidationAssert.ModelStateIsValid(x => x.Status);
        }

        [TestMethod]
        public void TestValidateAgencyIdReturnsTrueWhenActiveAndNotComplianceSite()
        {
            GetEntityFactory<SampleSiteStatus>().CreateList(5);
            _viewModel.Status = SampleSiteStatus.Indices.ACTIVE;
            _viewModel.IsComplianceSampleSite = false;

            ValidationAssert.ModelStateIsValid(x => x.Status);
        }

        [TestMethod]
        public void TestValidateAgencyIdReturnsTrueWhenActiveAndComplianceSiteAndAgencyValueEntered()
        {
            GetEntityFactory<SampleSiteStatus>().CreateList(5);
            _viewModel.Status = SampleSiteStatus.Indices.ACTIVE;
            _viewModel.IsComplianceSampleSite = true;
            _viewModel.AgencyId = "the machine";

            ValidationAssert.ModelStateIsValid(x => x.Status);
        }

        [TestMethod]
        public void TestValidateAgencyIdReturnsFalseWhenActiveAndComplianceSiteAndAgencyValueIsNotEntered()
        {
            GetEntityFactory<SampleSiteStatus>().CreateList(5);
            _viewModel.Status = SampleSiteStatus.Indices.ACTIVE;
            _viewModel.IsComplianceSampleSite = true;
            _viewModel.AgencyId = "";

            ValidationAssert.ModelStateHasError(x => x.AgencyId, SampleSiteViewModel.AGENCY_ID_VALIDATION_ERROR);
        }

        #endregion

        [TestMethod]
        public void TestToStringOverrideReturnsTheCorrectOutput()
        {
            var expected = $"{_entity.CommonSiteName},{_entity.LocationNameDescription},{_entity.Town?.ShortName},{_entity.Facility?.FacilityName}";

            Assert.AreEqual(_entity.ToString(), expected);
        }

        #region MapToEntity

        [TestMethod]
        public void TestMapToEntitySetsValidatedAtFieldsWhenValidating()
        {
            var employee = new Employee();
            var user = new User { IsAdmin = true, Employee = employee };
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _viewModel.IsBeingValidated = true;

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(_now, _entity.ValidatedAt);
            Assert.AreEqual(employee, _entity.ValidatedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsNeedsToSyncToTrue()
        {
            // sanity check
            Assert.IsFalse(_entity.NeedsToSync);

            _viewModel.MapToEntity(_entity);

            Assert.IsTrue(_entity.NeedsToSync);
        }

        [TestMethod]
        public void TestMapToEntitySetsLimsFacilityIdAndLimsSiteIdToNullFieldsWhenStateIsCaliforniaAndIsLimsLocation()
        {
            _viewModel.IsLimsLocation = true;
            _viewModel.LimsFacilityId = "facility-id";
            _viewModel.LimsSiteId = "site-id";
            _viewModel.LimsPrimaryStationCode = "primary-station-code";
            _viewModel.State = State.Indices.CA;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.LimsFacilityId);
            Assert.IsNull(_entity.LimsSiteId);
            Assert.IsNotNull(_entity.LimsPrimaryStationCode);
        }

        [TestMethod]
        public void TestMapToEntitySetsLimsPrimaryStationCodeToNullWhenStateIsNotCaliforniaAndIsLimsLocation()
        {
            _viewModel.IsLimsLocation = true;
            _viewModel.LimsFacilityId = "facility-id";
            _viewModel.LimsSiteId = "site-id";
            _viewModel.LimsPrimaryStationCode = "primary-station-code";
            _viewModel.State = State.Indices.NJ;

            _viewModel.MapToEntity(_entity);

            Assert.IsNotNull(_entity.LimsFacilityId);
            Assert.IsNotNull(_entity.LimsSiteId);
            Assert.IsNull(_entity.LimsPrimaryStationCode);
            
            _viewModel.State = State.Indices.GA;
            _viewModel.MapToEntity(_entity);
            
            Assert.IsNotNull(_entity.LimsSiteId);
            Assert.IsNotNull(_entity.LimsFacilityId);
            Assert.IsNull(_entity.LimsPrimaryStationCode);

            _viewModel.State = State.Indices.TN;
            _viewModel.MapToEntity(_entity);

            Assert.IsNotNull(_entity.LimsSiteId);
            Assert.IsNotNull(_entity.LimsFacilityId);
            Assert.IsNull(_entity.LimsPrimaryStationCode);
        }

        [TestMethod]
        public void TestMapToEntitySetsLimsFacilityIdAndLimsPrimaryStationCodeToNullWhenStateIsPennsylvaniaAndIsLimsLocation()
        {
            _viewModel.IsLimsLocation = true;
            _viewModel.LimsFacilityId = "facility-id";
            _viewModel.LimsSiteId = "site-id";
            _viewModel.LimsPrimaryStationCode = "primary-station-code";

            _viewModel.State = State.Indices.PA;
            _viewModel.MapToEntity(_entity);

            Assert.IsNotNull(_entity.LimsSiteId);
            Assert.IsNull(_entity.LimsFacilityId);
            Assert.IsNull(_entity.LimsPrimaryStationCode);
        }

        [TestMethod]
        public void TestMapToEntitySetsLimsFieldsToNullWhenItsNotALimsLocation()
        {
            _viewModel.IsLimsLocation = false;
            _viewModel.LimsFacilityId = "facility-id";
            _viewModel.LimsSiteId = "site-id";
            _viewModel.LimsPrimaryStationCode = "primary-station-code";
            _viewModel.LimsSequenceNumber = 14234;
            _viewModel.SampleSiteProfile = GetEntityFactory<SampleSiteProfile>().Create().Id;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.LimsFacilityId);
            Assert.IsNull(_entity.LimsSiteId);
            Assert.IsNull(_entity.LimsPrimaryStationCode);
            Assert.IsNull(_entity.LimsSequenceNumber);
            Assert.IsNull(_entity.SampleSiteProfile);
        }

        [TestMethod]
        public void TestMapToEntitySetsAssetsToNullWhenNeeded()
        {
            /* hydrant */

            _viewModel.Hydrant = 1;
            _viewModel.Valve = 1;
            _viewModel.Facility = 1;
            _viewModel.SampleSiteAddressLocationType = SampleSiteAddressLocationType.Indices.HYDRANT;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.Valve);
            Assert.IsNull(_entity.Facility);

            /* valve */

            _viewModel.Hydrant = 1;
            _viewModel.Valve = 1;
            _viewModel.Facility = 1;
            _viewModel.SampleSiteAddressLocationType = SampleSiteAddressLocationType.Indices.VALVE;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.Hydrant);
            Assert.IsNull(_entity.Facility);

            /* facility */

            _viewModel.Hydrant = 1;
            _viewModel.Valve = 1;
            _viewModel.Facility = 1;
            _viewModel.SampleSiteAddressLocationType = SampleSiteAddressLocationType.Indices.FACILITY;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.Hydrant);
            Assert.IsNull(_entity.Valve);

            /* sample station */

            _viewModel.Hydrant = 1;
            _viewModel.Valve = 1;
            _viewModel.Facility = 1;
            _viewModel.SampleSiteAddressLocationType = SampleSiteAddressLocationType.Indices.SAMPLE_STATION;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.Hydrant);
            Assert.IsNull(_entity.Valve);
            Assert.IsNull(_entity.Facility);

            /* custom */

            _viewModel.Hydrant = 1;
            _viewModel.Valve = 1;
            _viewModel.Facility = 1;
            _viewModel.SampleSiteAddressLocationType = SampleSiteAddressLocationType.Indices.CUSTOM;

            _viewModel.MapToEntity(_entity);

            Assert.IsNull(_entity.Hydrant);
            Assert.IsNull(_entity.Valve);
            Assert.IsNull(_entity.Facility);
        }

        #endregion

        #endregion
    }
}
