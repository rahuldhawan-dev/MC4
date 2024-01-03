using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Services
{
    [TestClass]
    public abstract class ServiceViewModelTest<TViewModel> : ViewModelTestBase<Service, TViewModel>
        where TViewModel : ServiceViewModel
    {
        #region Fields

        protected OperatingCenter _operatingCenter;
        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;
        private PremiseUnavailableReason _premiseUnavailableReason;
        private ServiceMaterial _leadService;
        private ServiceSize _serviceSize;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITapImageRepository>().Mock();
        }

        protected override Service CreateEntity()
        {
            return GetEntityFactory<Service>().Create(new {
                OperatingCenter = _operatingCenter = GetEntityFactory<OperatingCenter>().Create()
            });
        }

        protected virtual User CreateUser()
        {
            return GetFactory<UserFactory>().Create();
        }

        [TestInitialize]
        public void ServiceViewModelTestInitialize()
        {
            _user = CreateUser();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _premiseUnavailableReason = GetEntityFactory<PremiseUnavailableReason>().Create(new { Description = "Killed Premise" });
            _leadService = GetFactory<ServiceMaterialFactory>().Create(new { Description = "Lead" });
            _serviceSize = GetEntityFactory<ServiceSize>().Create(new { Description = "1" });
            // This needs to exist for validation
            GetFactory<OtherServiceTerminationPointFactory>().Create();
        }

        #endregion

        #region Mapping

        #region MapToEntity and CustomSideSLReplacementWasUpdated logic

        [TestMethod]
        public void TestMapToEntitySetsCustomerSideSLReplacementWasUpdatedToTrueWhenEntityDoesNotHaveValueAndViewModelDoesHaveValue()
        {
            _entity.CustomerSideSLReplacement = null;
            _viewModel.CustomerSideSLReplacement = GetEntityFactory<CustomerSideSLReplacementOfferStatus>().Create().Id;
            _vmTester.MapToEntity();
            Assert.IsTrue(_viewModel.CustomerSideSLReplacementWasUpdated);
        }

        [TestMethod]
        public void TestTestMapToEntitySetsCustomerSideSLReplacementWasUpdatedToTrueWhenEntityDoesHaveValueAndViewModelHasDifferentValue()
        {
            _entity.CustomerSideSLReplacement = GetEntityFactory<CustomerSideSLReplacementOfferStatus>().Create();
            _viewModel.CustomerSideSLReplacement = GetEntityFactory<CustomerSideSLReplacementOfferStatus>().Create().Id;
            _vmTester.MapToEntity();
            Assert.IsTrue(_viewModel.CustomerSideSLReplacementWasUpdated);
        }

        [TestMethod]
        public void TestTestMapToEntitySetsCustomerSideSLReplacementWasUpdatedToFalseWhenEntityAndViewModelHaveSameValue()
        {
            var status = GetEntityFactory<CustomerSideSLReplacementOfferStatus>().Create();
            _entity.CustomerSideSLReplacement = status;
            _viewModel.CustomerSideSLReplacement = status.Id;
            _vmTester.MapToEntity();
            Assert.IsFalse(_viewModel.CustomerSideSLReplacementWasUpdated);
        }

        [TestMethod]
        public void TestTestMapToEntitySetsCustomerSideSLReplacementWasUpdatedToFalseWhenEntityHasValueAndViewModelHasNullValue()
        {
            _entity.CustomerSideSLReplacement = GetEntityFactory<CustomerSideSLReplacementOfferStatus>().Create();
            _viewModel.CustomerSideSLReplacement = null;
            _vmTester.MapToEntity();
            Assert.IsFalse(_viewModel.CustomerSideSLReplacementWasUpdated);
        }

        [TestMethod]
        public void TestTestMapToEntitySetsCustomerSideSLReplacementWasUpdatedToFalseWhenEntityAndViewModelBothHaveNullValue()
        {
            _entity.CustomerSideSLReplacement = null;
            _viewModel.CustomerSideSLReplacement = null;
            _vmTester.MapToEntity();
            Assert.IsFalse(_viewModel.CustomerSideSLReplacementWasUpdated);
        }

        #endregion

        #region MapToEntity and PreviousServiceMaterialWasUpdatedToLead logic

        [TestMethod]
        public void TestPreviousServiceMaterialWasUpdatedToLeadIsTrueIfEntitysPreviousMaterialIsNotLeadAndViewModelsValueIsLead()
        {
            _entity.PreviousServiceMaterial = GetFactory<ServiceMaterialFactory>().Create();
            _viewModel.PreviousServiceMaterial = _leadService.Id;
            _vmTester.MapToEntity();
            Assert.IsTrue(_viewModel.PreviousServiceMaterialWasUpdatedToLead);
        }

        [TestMethod]
        public void TestPreviousServiceMaterialWasUpdatedToLeadIsFalseIfEntitysPreviousMaterialIsLeadAndViewModelsValueIsAlsoLead()
        {
            _entity.PreviousServiceMaterial = _leadService;
            _viewModel.PreviousServiceMaterial = _leadService.Id;
            _vmTester.MapToEntity();
            Assert.IsFalse(_viewModel.PreviousServiceMaterialWasUpdatedToLead);
        }

        [TestMethod]
        public void TestPreviousServiceMaterialWasUpdatedToLeadIsFalseIfEntitysPreviousMaterialIsNotLeadAndViewModelsValueIsNotLead()
        {
            _entity.PreviousServiceMaterial = GetFactory<ServiceMaterialFactory>().Create();
            _viewModel.PreviousServiceMaterial = GetFactory<ServiceMaterialFactory>().Create().Id;
            _vmTester.MapToEntity();
            Assert.IsFalse(_viewModel.PreviousServiceMaterialWasUpdatedToLead);
        }

        #endregion

        #endregion

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.Agreement)
               .CanMapBothWays(x => x.AmountReceived)
               .CanMapBothWays(x => x.ApartmentNumber)
               .CanMapBothWays(x => x.ApplicationApprovedOn)
               .CanMapBothWays(x => x.ApplicationReceivedOn)
               .CanMapBothWays(x => x.ApplicationSentOn)
               .CanMapBothWays(x => x.BackflowDevice)
               .CanMapBothWays(x => x.Block)
               .CanMapBothWays(x => x.BureauOfSafeDrinkingWaterPermitRequired)
               .CanMapBothWays(x => x.CleanedCoordinates)
               .CanMapBothWays(x => x.CompanyOwned)
               .CanMapBothWays(x => x.ContactDate)
               .CanMapBothWays(x => x.Coordinate)
               .CanMapBothWays(x => x.CustomerSideMaterial)
               .CanMapBothWays(x => x.CustomerSideReplacementDate)
               .CanMapBothWays(x => x.CustomerSideReplacementWBSNumber)
               .CanMapBothWays(x => x.CustomerSideSLReplacedBy)
               .CanMapBothWays(x => x.CustomerSideSLReplacement)
               .CanMapBothWays(x => x.CustomerSideSLReplacementContractor)
               .CanMapBothWays(x => x.CustomerSideSLReplacementCost)
               .CanMapBothWays(x => x.CustomerSideSize)
               .CanMapBothWays(x => x.DateClosed)
               .CanMapBothWays(x => x.DateInstalled)
               .CanMapBothWays(x => x.DateIssuedToField)
               .CanMapBothWays(x => x.DepthMainFeet)
               .CanMapBothWays(x => x.DepthMainInches)
               .CanMapBothWays(x => x.DeveloperServicesDriven)
               .CanMapBothWays(x => x.Development)
               .CanMapBothWays(x => x.DeviceLocation)
               .CanMapBothWays(x => x.DeviceLocationUnavailable)
               .CanMapBothWays(x => x.Fax)
               .CanMapBothWays(x => x.FlushingOfCustomerPlumbing)
               .CanMapBothWays(x => x.GeoEFunctionalLocation)
               .CanMapBothWays(x => x.InactiveDate)
               .CanMapBothWays(x => x.InspectionDate)
               .CanMapBothWays(x => x.Installation)
               .CanMapBothWays(x => x.InstallationCost)
               .CanMapBothWays(x => x.InstallationInvoiceDate)
               .CanMapBothWays(x => x.InstallationInvoiceNumber)
               .CanMapBothWays(x => x.LeadAndCopperCommunicationProvided)
               .CanMapBothWays(x => x.LeadServiceReplacementWbs)
               .CanMapBothWays(x => x.LeadServiceRetirementWbs)
               .CanMapBothWays(x => x.LegacyId)
               .CanMapBothWays(x => x.LengthOfCustomerSideSLReplaced)
               .CanMapBothWays(x => x.LengthOfService)
               .CanMapBothWays(x => x.Lot)
               .CanMapBothWays(x => x.MailPhoneNumber)
               .CanMapBothWays(x => x.MailState)
               .CanMapBothWays(x => x.MailStreetName)
               .CanMapBothWays(x => x.MailStreetNumber)
               .CanMapBothWays(x => x.MailTown)
               .CanMapBothWays(x => x.MailZip)
               .CanMapBothWays(x => x.MainSize)
               .CanMapBothWays(x => x.MainType)
               .CanMapBothWays(x => x.MeterSettingRequirement)
               .CanMapBothWays(x => x.MeterSettingSize)
               .CanMapBothWays(x => x.NSINumber)
               .CanMapBothWays(x => x.Name)
               .CanMapBothWays(x => x.ObjectId)
               .CanMapBothWays(x => x.OfferedAgreement)
               .CanMapBothWays(x => x.OfferedAgreementDate)
               .CanMapBothWays(x => x.OriginalInstallationDate)
               .CanMapBothWays(x => x.OtherPoint)
               .CanMapBothWays(x => x.ParentTaskNumber)
               .CanMapBothWays(x => x.PaymentReferenceNumber)
               .CanMapBothWays(x => x.PhoneNumber)
               .CanMapBothWays(x => x.PitInstalled)
               .CanMapBothWays(x => x.PremiseNumber)
               .CanMapBothWays(x => x.PremiseNumberUnavailable)
               .CanMapBothWays(x => x.PremiseUnavailableReason)
               .CanMapBothWays(x => x.PreviousServiceCustomerMaterial)
               .CanMapBothWays(x => x.PreviousServiceCustomerSize)
               .CanMapBothWays(x => x.PreviousServiceMaterial)
               .CanMapBothWays(x => x.PreviousServiceSize)
               .CanMapBothWays(x => x.ProjectManager)
               .CanMapBothWays(x => x.PurchaseOrderNumber)
               .CanMapBothWays(x => x.QuestionaireReceivedDate)
               .CanMapBothWays(x => x.QuestionaireSentDate)
               .CanMapBothWays(x => x.RetireMeterSet)
               .CanMapBothWays(x => x.RetiredAccountNumber)
               .CanMapBothWays(x => x.RetiredDate)
               .CanMapBothWays(x => x.RoadOpeningFee)
               .CanMapBothWays(x => x.SAPNotificationNumber)
               .CanMapBothWays(x => x.SAPWorkOrderNumber)
               .CanMapBothWays(x => x.ServiceCategory)
               .CanMapBothWays(x => x.ServiceDwellingType)
               .CanMapBothWays(x => x.ServiceDwellingTypeQuantity)
               .CanMapBothWays(x => x.ServiceInstallationFee)
               .CanMapBothWays(x => x.ServiceInstallationPurpose)
               .CanMapBothWays(x => x.ServicePriority)
               .CanMapBothWays(x => x.ServiceRegroundingPremiseType)
               .CanMapBothWays(x => x.ServiceSideType)
               .CanMapBothWays(x => x.ServiceSize)
               .CanMapBothWays(x => x.ServiceStatus)
               .CanMapBothWays(x => x.State)
               .CanMapBothWays(x => x.StreetMaterial)
               .CanMapBothWays(x => x.StreetNumber)
               .CanMapBothWays(x => x.SubfloorCondition)
               .CanMapBothWays(x => x.TapOrderNotes)
               .CanMapBothWays(x => x.TaskNumber1)
               .CanMapBothWays(x => x.TaskNumber2)
               .CanMapBothWays(x => x.Town)
               .CanMapBothWays(x => x.TownSection)
               .CanMapBothWays(x => x.WorkIssuedTo)
               .CanMapBothWays(x => x.YearOfHomeConstruction)
               .CanMapBothWays(x => x.Zip);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<ServiceOfferedAgreementType>(x => x.OfferedAgreement)
               .EntityMustExist<PremiseUnavailableReason>(x => x.PremiseUnavailableReason)
               .EntityMustExist<User>(x => x.ProjectManager)
               .EntityMustExist<ServiceRegroundingPremiseType>(x => x.ServiceRegroundingPremiseType)
               .EntityMustExist<ServiceSubfloorCondition>(x => x.SubfloorCondition)
               .EntityMustExist<ServiceTerminationPoint>(x => x.TerminationPoint)
               .EntityMustExist<ServiceMaterial>(x => x.CustomerSideMaterial)
               .EntityMustExist<ServiceSize>(x => x.CustomerSideSize)
               .EntityMustExist<WBSNumber>(x => x.CustomerSideReplacementWBSNumber)
               .EntityMustExist<BackflowDevice>(x => x.BackflowDevice)
               .EntityMustExist<Coordinate>(x => x.Coordinate)
               .EntityMustExist<ServiceSize>(x => x.MainSize)
               .EntityMustExist<MainType>(x => x.MainType)
               .EntityMustExist<ServiceSize>(x => x.MeterSettingSize)
               .EntityMustExist<ServiceMaterial>(x => x.PreviousServiceMaterial)
               .EntityMustExist<ServiceMaterial>(x => x.PreviousServiceCustomerMaterial)
               .EntityMustExist<ServiceSize>(x => x.PreviousServiceCustomerSize)
               .EntityMustExist<ServiceCategory>(x => x.ServiceCategory)
               .EntityMustExist<ServiceDwellingType>(x => x.ServiceDwellingType)
               .EntityMustExist<ServiceInstallationPurpose>(x => x.ServiceInstallationPurpose)
               .EntityMustExist<ServicePriority>(x => x.ServicePriority)
               .EntityMustExist<ServiceSize>(x => x.ServiceSize)
               .EntityMustExist<ServiceStatus>(x => x.ServiceStatus)
               .EntityMustExist<State>(x => x.State)
               .EntityMustExist<StreetMaterial>(x => x.StreetMaterial)
               .EntityMustExist<Town>(x => x.Town)
               .EntityMustExist<TownSection>(x => x.TownSection)
               .EntityMustExist<ServiceRestorationContractor>(x => x.WorkIssuedTo)
               .EntityMustExist<CustomerSideSLReplacementOfferStatus>(x => x.CustomerSideSLReplacement)
               .EntityMustExist<FlushingOfCustomerPlumbingInstructions>(x => x.FlushingOfCustomerPlumbing)
               .EntityMustExist<CustomerSideSLReplacer>(x => x.CustomerSideSLReplacedBy)
               .EntityMustExist<Contractor>(x => x.CustomerSideSLReplacementContractor)
               .EntityMustExist<ServiceSideType>(x => x.ServiceSideType);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.ApartmentNumber, Service.StringLengths.APARTMENT_NUMBER)
               .PropertyHasMaxStringLength(x => x.Block, Service.StringLengths.BLOCK)
               .PropertyHasMaxStringLength(x => x.Development, Service.StringLengths.DEVELOPMENT)
               .PropertyHasMaxStringLength(x => x.Fax, Service.StringLengths.FAX)
               .PropertyHasMaxStringLength(
                    x => x.InstallationInvoiceNumber,
                    Service.StringLengths.INSTALLATION_INVOICE_NUMBER)
               .PropertyHasMaxStringLength(x => x.Lot, Service.StringLengths.LOT)
               .PropertyHasMaxStringLength(x => x.MailPhoneNumber, Service.StringLengths.MAIL_PHONE_NUMBER)
               .PropertyHasMaxStringLength(x => x.MailState, Service.StringLengths.MAIL_STATE)
               .PropertyHasMaxStringLength(x => x.MailStreetName, Service.StringLengths.MAIL_STREET_NAME)
               .PropertyHasMaxStringLength(
                    x => x.MailStreetNumber,
                    Service.StringLengths.MAIL_STREET_NUMBER)
               .PropertyHasMaxStringLength(x => x.MailTown, Service.StringLengths.MAIL_TOWN)
               .PropertyHasMaxStringLength(x => x.MailZip, Service.StringLengths.MAIL_ZIP)
               .PropertyHasMaxStringLength(x => x.Name, Service.StringLengths.NAME)
               .PropertyHasMaxStringLength(
                    x => x.ParentTaskNumber,
                    Service.StringLengths.PARENT_TASK_NUMBER)
               .PropertyHasMaxStringLength(
                    x => x.PaymentReferenceNumber,
                    Service.StringLengths.PAYMENT_REFERENCE_NUMBER)
               .PropertyHasMaxStringLength(x => x.PhoneNumber, Service.StringLengths.PHONE_NUMBER)
               .PropertyHasMaxStringLength(
                    x => x.PurchaseOrderNumber,
                    Service.StringLengths.PURCHASE_ORDER_NUMBER)
               .PropertyHasMaxStringLength(
                    x => x.RetiredAccountNumber,
                    Service.StringLengths.RETIRED_ACCOUNT_NUMBER)
               .PropertyHasMaxStringLength(x => x.RetireMeterSet, Service.StringLengths.RETIRE_METER_SET)
               .PropertyHasMaxStringLength(x => x.StreetNumber, Service.StringLengths.STREET_NUMBER)
               .PropertyHasMaxStringLength(x => x.TaskNumber1, Service.StringLengths.TASK_NUMBER_1)
               .PropertyHasMaxStringLength(x => x.TaskNumber2, Service.StringLengths.TASK_NUMBER_2)
               .PropertyHasMaxStringLength(x => x.LeadServiceReplacementWbs, Service.StringLengths.WBS)
               .PropertyHasMaxStringLength(x => x.LeadServiceRetirementWbs, Service.StringLengths.WBS)
               .PropertyHasMaxStringLength(x => x.Zip, Service.StringLengths.ZIP)
               .PropertyHasMaxStringLength(x => x.LegacyId, Service.StringLengths.LEGACY_ID);

            _viewModel.PremiseNumber = "9876543210";
            _viewModel.Installation = "9876543211";
            _viewModel.DeviceLocation = "987654321098765432109876543210";
            ValidationAssert
               .PropertyHasStringLength(
                    x => x.PremiseNumber,
                    Service.StringLengths.PREMISE_NUMBER,
                    Service.StringLengths.PREMISE_NUMBER,
                    useCurrentPropertyValue: true)
               .PropertyHasMaxStringLength(x => x.Installation, Service.StringLengths.INSTALLATION, true)
               .PropertyHasMaxStringLength(x => x.DeviceLocation, 30, true);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.ServiceCategory)
               .PropertyIsRequired(x => x.Town);
        }

        [TestMethod]
        public void TestValidateHasErrorsForRetirementFieldsWhenConditions()
        {
            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();

            _viewModel.DateInstalled = DateTime.Now;
            _viewModel.ServiceCategory = ServiceCategory.Indices.FIRE_SERVICE_RENEWAL;

            ValidationAssert.ModelStateHasError(x => x.RetiredDate, ServiceViewModel.ErrorMessages.REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY);
            ValidationAssert.ModelStateHasError(x => x.OriginalInstallationDate, ServiceViewModel.ErrorMessages.ORIGINAL_INSTALLATION_DATE);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceSize, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_SIZE);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceMaterial, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_MATERIAL);

            _viewModel.RetiredDate = DateTime.Now;
            ValidationAssert.ModelStateIsValid(x => x.RetiredDate);
            ValidationAssert.ModelStateHasError(x => x.OriginalInstallationDate, ServiceViewModel.ErrorMessages.ORIGINAL_INSTALLATION_DATE);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceSize, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_SIZE);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceMaterial, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_MATERIAL);

            _viewModel.OriginalInstallationDate = DateTime.Now;
            ValidationAssert.ModelStateIsValid(x => x.RetiredDate);
            ValidationAssert.ModelStateIsValid(x => x.OriginalInstallationDate);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceSize, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_SIZE);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceMaterial, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_MATERIAL);

            _viewModel.PreviousServiceSize = serviceSize.Id;
            ValidationAssert.ModelStateIsValid(x => x.RetiredDate);
            ValidationAssert.ModelStateIsValid(x => x.OriginalInstallationDate);
            ValidationAssert.ModelStateIsValid(x => x.PreviousServiceSize);
            ValidationAssert.ModelStateHasError(x => x.PreviousServiceMaterial, ServiceViewModel.ErrorMessages.PREVIOUS_SERVICE_MATERIAL);

            _viewModel.PreviousServiceMaterial = serviceMaterial.Id;
            ValidationAssert.ModelStateIsValid(x => x.RetiredDate);
            ValidationAssert.ModelStateIsValid(x => x.OriginalInstallationDate);
            ValidationAssert.ModelStateIsValid(x => x.PreviousServiceSize);
            ValidationAssert.ModelStateIsValid(x => x.PreviousServiceMaterial);
        }

        [TestMethod]
        public void TestDeviceLocationMustBeNumeric()
        {
            _viewModel.DeviceLocation = "ABC";
            ValidationAssert.ModelStateHasError(x => x.DeviceLocation, "Device Location must be numeric.");

            _viewModel.DeviceLocation = "12345";
            ValidationAssert.ModelStateIsValid(x => x.DeviceLocation);
        }

        [TestMethod]
        public void TestInstallationMustBeNumeric()
        {
            _viewModel.Installation = "ABC";
            ValidationAssert.ModelStateHasError(x => x.Installation, "Installation must be numeric.");

            _viewModel.Installation = "12345";
            ValidationAssert.ModelStateIsValid(x => x.Installation);
        }

        [TestMethod]
        public void TestPremiseNumberMustBeNumeric()
        {
            _viewModel.PremiseNumber = "ABCDEABCDE";
            ValidationAssert.ModelStateHasError(x => x.PremiseNumber, "Premise Number must be numeric.");

            _viewModel.PremiseNumber = "1234512345";
            ValidationAssert.ModelStateIsValid(x => x.PremiseNumber);
        }

        [TestMethod]
        public void TestRequiredWhenFields()
        {
            ValidationAssert.PropertyIsRequiredWhen(x => x.PremiseUnavailableReason, 1, x => x.PremiseNumberUnavailable, true, false, "The Premise Number Unavailable Reason is required.");
            ValidationAssert.PropertyIsRequiredWhen(x => x.PremiseNumber, "1234567890", x => x.PremiseNumberUnavailable, false, true, "The Premise Number field is required.");
            ValidationAssert.PropertyIsRequiredWhen(x => x.Installation, "1234567890", x => x.PremiseNumberUnavailable, false, true, "The Installation Number field is required.");
            ValidationAssert.PropertyIsRequiredWhen(x => x.DeviceLocation, "1234567890", x => x.DeviceLocationUnavailable, false, true, "The Device Location field is required.");
        }

        [TestMethod]
        public void TestMapToEntitySetsPremise()
        {
            var serviceUtilityType = GetEntityFactory<ServiceUtilityType>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create(new { ServiceUtilityType = serviceUtilityType });
            var premise = GetEntityFactory<Premise>().Create(new {
                Installation = "123456789",
                PremiseNumber = "9100327803",
                ServiceUtilityType = serviceUtilityType
            });
            _viewModel.Installation = premise.Installation;
            _viewModel.PremiseNumber = premise.PremiseNumber;
            _viewModel.ServiceCategory = serviceCategory.Id;
            
            _vmTester.MapToEntity();

            Assert.AreEqual(premise.Id, _entity.Premise.Id);
        }

        [TestMethod]
        public void Test_IsInstalledNonVerification_ReturnsFalse_WhenDateInstalledIsNull()
        {
            _viewModel.DateInstalled = null;
            _viewModel.ServiceCategory = ServiceCategory.Indices.WATER_MEASUREMENT_ONLY;
            _viewModel.ServiceInstallationPurpose =
                ServiceInstallationPurpose.Indices.MATERIAL_VERIFICATION;

            Assert.IsFalse(_viewModel.IsInstalledNonVerification);
        }

        [TestMethod]
        public void
            Test_IsInstalledNonVerification_ReturnsTrue_WhenDateInstalledIsNotNullAndCategoryMatchesButPurposeDoesNot()
        {
            _viewModel.DateInstalled = DateTime.Now;
            _viewModel.ServiceCategory = ServiceCategory.Indices.WATER_MEASUREMENT_ONLY;

            var nonMatchingPurposes =
                typeof(ServiceInstallationPurpose.Indices)
                   .GetConstantValues()
                   .Where(v => v.Value != ServiceInstallationPurpose.Indices.MATERIAL_VERIFICATION);

            foreach (var purpose in nonMatchingPurposes)
            {
                _viewModel.ServiceInstallationPurpose = purpose.Value;

                Assert.IsTrue(_viewModel.IsInstalledNonVerification);
            }
        }

        [TestMethod]
        public void
            Test_IsInstalledNonVerification_ReturnsTrue_WhenDateInstalledIsNotNullAndPurposeMatchesButCategoryDoesNot()
        {
            _viewModel.DateInstalled = DateTime.Now;
            _viewModel.ServiceInstallationPurpose =
                ServiceInstallationPurpose.Indices.MATERIAL_VERIFICATION;

            var nonMatchingCategories =
                typeof(ServiceCategory.Indices)
                   .GetConstantValues()
                   .Where(v => v.Value != ServiceCategory.Indices.WATER_MEASUREMENT_ONLY);

            foreach (var category in nonMatchingCategories)
            {
                _viewModel.ServiceCategory = category.Value;

                Assert.IsTrue(_viewModel.IsInstalledNonVerification);
            }
        }

        [TestMethod]
        public void
            Test_IsInstalledNonVerification_ReturnsFalse_WhenDateInstalledIsNotNullButCategoryAndPurposeMatch()
        {
            _viewModel.DateInstalled = DateTime.Now;

            _viewModel.ServiceCategory = ServiceCategory.Indices.WATER_MEASUREMENT_ONLY;
            _viewModel.ServiceInstallationPurpose =
                ServiceInstallationPurpose.Indices.MATERIAL_VERIFICATION;

            Assert.IsFalse(_viewModel.IsInstalledNonVerification);
        }
    }
}