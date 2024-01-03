using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public abstract class ServiceViewModel : ViewModel<Service>
    {
        #region Constants

        public struct ErrorMessages
        {
            #region Constants

            public const string
                BLOCK_AND_LOT_OR_STREET_ADDRESS =
                    "You must enter either a 'Street Number and Name' or a 'Block and Lot'.",
                SERVICE_NUMBER_NOT_DUPLICATED = "The premise number '{0}' is not unique to the operating center.",
                REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY = "Required when installed and the service category is for a renewal.",
                ORIGINAL_INSTALLATION_DATE = "The Original Installation Date field is required.",
                PREVIOUS_SERVICE_MATERIAL = "The Previous Service Material field is required.",
                PREVIOUS_SERVICE_SIZE = "The Previous Service Size field is required.",
                METER_SETTING_REQUIREMENT = "The Meter Setting Requirement field is required.",
                METER_SETTING_SIZE = "The Meter Setting Size field is required.";

            #endregion
        }

        public int[] LENGTH_OF_SERVICE_REQUIRED = {
            MapCall.Common.Model.Entities.ServiceCategory.Indices.INSTALL_METER_SET,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.REPLACE_METER_SET,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.SEWER_MEASUREMENT_ONLY,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_MEASUREMENT_ONLY,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_RELOCATE_METER_SET,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_RETIRE_METER_SET_ONLY
        };

        #endregion

        #region Constructors

        public ServiceViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override Service MapToEntity(Service entity)
        {
            entity.UpdatedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.UpdatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            // The null check is here because there's no contact required if they set the status to null.
            CustomerSideSLReplacementWasUpdated = CustomerSideSLReplacement.HasValue && CustomerSideSLReplacement.Value != entity.CustomerSideSLReplacement?.Id;

            // Do not try to match the Lead service based on Id. See the GetLeadServiceMaterial repo method for info.
            var leadServiceMaterial = _container.GetInstance<IRepository<ServiceMaterial>>().GetLeadServiceMaterial();
            PreviousServiceMaterialWasUpdatedToLead = PreviousServiceMaterial.HasValue &&
                                                      PreviousServiceMaterial.Value == leadServiceMaterial.Id &&
                                                      entity.PreviousServiceMaterial != leadServiceMaterial;

            base.MapToEntity(entity);
            var premises = _container.GetInstance<IRepository<Premise>>()
                                     .Where(x => x.Installation == Installation && x.PremiseNumber == entity.PremiseNumber);
            if (entity.ServiceCategory != null && entity.ServiceCategory.ServiceUtilityType != null)
            {
                entity.Premise = premises.Where(x =>
                                              x.ServiceUtilityType != null &&
                                              x.ServiceUtilityType.Id == entity.ServiceCategory.ServiceUtilityType.Id)
                                         .FirstOrDefault();
            }
            else
            {
                entity.Premise = premises.Where(x => x.ServiceUtilityType == null).FirstOrDefault();
            }
            return entity;
        }

        public static int[] SewerServiceCategoryIds()
        {
            var categories = MapCall.Common.Model.Entities.ServiceCategory.GetSewerCategories().ToList();
            categories.Add(MapCall.Common.Model.Entities.ServiceCategory.Indices.SEWER_RECONNECT);
            categories.Add(MapCall.Common.Model.Entities.ServiceCategory.Indices.SEWER_SERVICE_RECORD_IMPORT);
            return categories.ToArray();
        }

        #endregion

        #region Regular

        public bool? Agreement { get; set; }
        public decimal? AmountReceived { get; set; }
        public DateTime? ApplicationApprovedOn { get; set; }

        [StringLength(Service.StringLengths.APARTMENT_NUMBER)]
        [View(Service.DisplayNames.APT_ADDTL)]
        public string ApartmentNumber { get; set; }

        public DateTime? ApplicationReceivedOn { get; set; }
        public DateTime? ApplicationSentOn { get; set; }
        public bool? BureauOfSafeDrinkingWaterPermitRequired { get; set; }

        [StringLength(Service.StringLengths.BLOCK)]
        [RequiredWhen(nameof(StreetNumber),ComparisonType.EqualTo, "", ErrorMessage = ErrorMessages.BLOCK_AND_LOT_OR_STREET_ADDRESS)]
        public string Block { get; set; }

        public bool? CleanedCoordinates { get; set; }
        public DateTime? ContactDate { get; set; }
        public DateTime? DateClosed { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        public DateTime? DateInstalled { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        public DateTime? DateIssuedToField { get; set; }

        public int? DepthMainFeet { get; set; }
        public int? DepthMainInches { get; set; }

        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        public bool? DeveloperServicesDriven { get; set; }

        [StringLength(Service.StringLengths.DEVELOPMENT)]
        public string Development { get; set; }

        [StringLength(Service.StringLengths.FAX)]
        public string Fax { get; set; }

        public string GeoEFunctionalLocation { get; set; }
        public DateTime? InspectionDate { get; set; }
        public decimal? InstallationCost { get; set; }
        public DateTime? InstallationInvoiceDate { get; set; }

        [StringLength(Service.StringLengths.INSTALLATION_INVOICE_NUMBER)]
        public string InstallationInvoiceNumber { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? InactiveDate { get; set; }

        public bool LeadAndCopperCommunicationProvided { get; set; }

        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        public decimal? LengthOfService { get; set; }

        [StringLength(Service.StringLengths.LOT)]
        [RequiredWhen(nameof(StreetNumber), ComparisonType.EqualTo, "", ErrorMessage = ErrorMessages.BLOCK_AND_LOT_OR_STREET_ADDRESS)]
        public string Lot { get; set; }

        [StringLength(Service.StringLengths.MAIL_PHONE_NUMBER)]
        public string MailPhoneNumber { get; set; }

        [StringLength(Service.StringLengths.MAIL_STATE)]
        public string MailState { get; set; }

        [StringLength(Service.StringLengths.MAIL_STREET_NAME)]
        public string MailStreetName { get; set; }

        [StringLength(Service.StringLengths.MAIL_STREET_NUMBER)]
        public string MailStreetNumber { get; set; }

        [StringLength(Service.StringLengths.MAIL_TOWN)]
        public string MailTown { get; set; }

        [StringLength(Service.StringLengths.MAIL_ZIP)]
        public string MailZip { get; set; }

        [ClientCallback("Services.validateMeterSettingRequirement", ErrorMessage = ErrorMessages.METER_SETTING_REQUIREMENT)]
        public bool? MeterSettingRequirement { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [ClientCallback("Services.validateRetirementInformation", ErrorMessage = ErrorMessages.ORIGINAL_INSTALLATION_DATE)]
        public DateTime? OriginalInstallationDate { get; set; }

        [StringLength(Service.StringLengths.NAME)]
        [RequiredWhen(nameof(CustomerSideMaterial), ComparisonType.EqualTo, ServiceMaterial.Indices.LEAD)]
        public string Name { get; set; }
        public int? NSINumber { get; set; }
        public int? ObjectId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceOfferedAgreementType))]
        public int? OfferedAgreement { get; set; }
        public DateTime? OfferedAgreementDate { get; set; }

        [RequiredWhen(nameof(TerminationPoint), ServiceTerminationPoint.Indices.OTHER)]
        public string OtherPoint { get; set; }

        [StringLength(Service.StringLengths.PARENT_TASK_NUMBER)]
        public string ParentTaskNumber { get; set; }

        [StringLength(Service.StringLengths.PAYMENT_REFERENCE_NUMBER)]
        public string PaymentReferenceNumber { get; set; }

        [StringLength(Service.StringLengths.PHONE_NUMBER)]
        public string PhoneNumber { get; set; }

        [ClientCallback("Services.validatePremiseNumberUnavailable", ErrorMessage = "Please confirm that there is no premise number available or enter the premise number below.")]
        public bool PremiseNumberUnavailable { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PremiseUnavailableReason))]
        [RequiredWhen(nameof(PremiseNumberUnavailable), ComparisonType.EqualTo, true, ErrorMessage = "The Premise Number Unavailable Reason is required.")]
        public int? PremiseUnavailableReason { get; set; }

        /*Premise Number not validating 10 digits. I think we also do not allow all 0000000000, or any repeat numbers currently?*/

        [RegularExpression(RegularExpressions.NUMERICAL, ErrorMessage = "Premise Number must be numeric.")]
        [StringLength(Service.StringLengths.PREMISE_NUMBER, MinimumLength = Service.StringLengths.PREMISE_NUMBER)]
        [RequiredWhen(nameof(PremiseNumberUnavailable), ComparisonType.NotEqualTo, true, ErrorMessage = "The Premise Number field is required.")]
        [ClientCallback("Services.validatePremiseNumber", ErrorMessage = "Invalid Premise Number. Please check PremiseNumberUnavailable if you don't have a valid premise number.")]
        public string PremiseNumber { get; set; }

        [DropDown("", "User", "FieldServicesAssetsUsersByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Select an operating center"), EntityMap, EntityMustExist(typeof(User))]
        public virtual int? ProjectManager { get; set; }

        [RegularExpression(RegularExpressions.NUMERICAL, ErrorMessage = "Installation must be numeric.")]
        [StringLength(Service.StringLengths.INSTALLATION)]
        [RequiredWhen(nameof(PremiseNumberUnavailable), ComparisonType.NotEqualTo, true, ErrorMessage = "The Installation Number field is required.")]
        public string Installation { get; set; }

        [StringLength(Service.StringLengths.PURCHASE_ORDER_NUMBER)]
        public string PurchaseOrderNumber { get; set; }

        public DateTime? QuestionaireSentDate { get; set; }
        public DateTime? QuestionaireReceivedDate { get; set; }

        [StringLength(Service.StringLengths.RETIRED_ACCOUNT_NUMBER)]
        public string RetiredAccountNumber { get; set; }

        public DateTime? RetiredDate { get; set; }

        [StringLength(Service.StringLengths.RETIRE_METER_SET)]
        public string RetireMeterSet { get; set; }

        public decimal? RoadOpeningFee { get; set; }

        [Range(Service.Range.SAP_RANGE_MIN, Service.Range.SAP_RANGE_MAX)]
        public virtual long? SAPWorkOrderNumber { get; set; }

        [Range(Service.Range.SAP_RANGE_MIN, Service.Range.SAP_RANGE_MAX)]
        public virtual long? SAPNotificationNumber { get; set; }

        public decimal? ServiceInstallationFee { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceRegroundingPremiseType))]
        public int? ServiceRegroundingPremiseType { get; set; }

        [StringLength(Service.StringLengths.STREET_NUMBER)]
        [RequiredWhen(nameof(Block), ComparisonType.EqualTo, "", ErrorMessage = ErrorMessages.BLOCK_AND_LOT_OR_STREET_ADDRESS)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public string StreetNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSubfloorCondition))]
        public int? SubfloorCondition { get; set; }

        [Multiline]
        public string TapOrderNotes { get; set; }

        [StringLength(Service.StringLengths.TASK_NUMBER_1)]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        [ClientCallback("Services.validateWBSNumber", ErrorMessage = "WBS # is invalid.")]
        public string TaskNumber1 { get; set; }

        [StringLength(Service.StringLengths.TASK_NUMBER_2)]
        public string TaskNumber2 { get; set; }
        [StringLength(Service.StringLengths.WBS)]
        public virtual string LeadServiceReplacementWbs { get; set; }
        [StringLength(Service.StringLengths.WBS)]
        public virtual string LeadServiceRetirementWbs { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceTerminationPoint))]
        public int? TerminationPoint { get; set; }

        [Range(1800, 9999)]
        public int? YearOfHomeConstruction { get; set; }

        [StringLength(Service.StringLengths.ZIP)]
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        [RequiredWhen(nameof(CustomerSideMaterial), ComparisonType.EqualTo, ServiceMaterial.Indices.LEAD)]
        public string Zip { get; set; }

        [DropDown, EntityMustExist(typeof(ServiceMaterial)), EntityMap, RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? CustomerSideMaterial { get; set; }

        [DropDown, EntityMustExist(typeof(ServiceSize)), EntityMap]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        public int? CustomerSideSize { get; set; }

        public int? ServiceDwellingTypeQuantity { get; set; }

        public int? LengthOfCustomerSideSLReplaced { get; set; }
        public decimal? CustomerSideSLReplacementCost { get; set; }
        public DateTime? CustomerSideReplacementDate { get; set; }

        //We want to automatically assign this.
        [DropDown("", "WBSNumber", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        [EntityMap, EntityMustExist(typeof(WBSNumber))]
        public int? CustomerSideReplacementWBSNumber { get; set; }

        [DoesNotAutoMap("Not an actual View Property - set by MapToEntity and Used by Controller, also set in view via ajax")]
        public bool SendToSAP { get; set; }

        public bool? PitInstalled { get; set; }
        public bool? CompanyOwned { get; set; }

        [StringLength(Service.StringLengths.LEGACY_ID)]
        public string LegacyId { get; set; }

        #endregion

        #region Associations

        [DropDown, EntityMap, EntityMustExist(typeof(BackflowDevice))]
        public int? BackflowDevice { get; set; }

        [EntityMap, EntityMustExist(typeof(Coordinate))]
        [Coordinate(AddressCallback = "Services.getAddress", IconSet = IconSets.SingleDefaultIcon)]
        public int? Coordinate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        public int? MainSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainType))]
        public int? MainType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [ClientCallback("Services.validateMeterSettingSize", ErrorMessage = ErrorMessages.METER_SETTING_SIZE)]
        public int? MeterSettingSize { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.PREVIOUS_SERVICE_MATERIAL)]
        [DropDown("", "ServiceMaterial", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [ClientCallback("Services.validateRetirementInformation", ErrorMessage = ErrorMessages.PREVIOUS_SERVICE_MATERIAL)]
        public int? PreviousServiceMaterial { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [DropDown("", "ServiceMaterial", "ByOperatingCenterId", DependsOn="OperatingCenter", PromptText = "Select an operating center above")]
        [RequiredWhen(nameof(ServiceCategory), ComparisonType.EqualTo, MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE)]
        public int? PreviousServiceCustomerMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize)), View(Service.DisplayNames.PREVIOUS_SERVICE_SIZE)]
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [ClientCallback("Services.validateRetirementInformation", ErrorMessage = ErrorMessages.PREVIOUS_SERVICE_SIZE)]
        public int? PreviousServiceSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [RequiredWhen(nameof(ServiceCategory), ComparisonType.EqualTo, MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE)]
        public int? PreviousServiceCustomerSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceCategory))]
        [Required]
        public int? ServiceCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceDwellingType))]
        public int? ServiceDwellingType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationPurpose))]
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? ServiceInstallationPurpose { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [DropDown, EntityMap, EntityMustExist(typeof(ServicePriority))]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        public int? ServicePriority { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize)), View(Service.DisplayNames.SERVICE_SIZE)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? ServiceSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceStatus))]
        public int? ServiceStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(CustomerSideMaterial), ComparisonType.EqualTo, ServiceMaterial.Indices.LEAD)]
        public int? State { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(StreetMaterial))]
        public int? StreetMaterial { get; set; }

        [EntityMap, EntityMustExist(typeof(Town)), Required]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town from above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public virtual int? TownSection { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(IsInstalledNonVerification), true)]
        [EntityMap, EntityMustExist(typeof(ServiceRestorationContractor))]
        [DropDown("", "ServiceRestorationContractor", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? WorkIssuedTo { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CustomerSideSLReplacementOfferStatus))]
        public int? CustomerSideSLReplacement { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FlushingOfCustomerPlumbingInstructions))]
        public int? FlushingOfCustomerPlumbing { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CustomerSideSLReplacer))]
        public int? CustomerSideSLReplacedBy { get; set; }

        [RequiredWhen(nameof(CustomerSideSLReplacedBy), ComparisonType.EqualTo, CustomerSideSLReplacer.Indices.CONTRACTOR)]
        [EntityMap, EntityMustExist(typeof(Contractor))]
        [DropDown("Contractors", "Contractor", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? CustomerSideSLReplacementContractor { get; set; }

        [StringLength(30)]
        [RegularExpression(RegularExpressions.NUMERICAL, ErrorMessage = "Device Location must be numeric.")]
        [RequiredWhen(nameof(DeviceLocationUnavailable), ComparisonType.NotEqualTo, true, ErrorMessage = "The Device Location field is required.")]
        [ClientCallback("Services.validateDeviceLocation", ErrorMessage = "The Device Location field is required.")]
        public string DeviceLocation { get; set; }

        [ClientCallback("Services.validateDeviceLocationUnavailable", ErrorMessage = "Please confirm that there is no device location available or enter the device location above.")]
        public bool DeviceLocationUnavailable { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSideType))]
        public int? ServiceSideType { get; set; }

        #endregion

        #region Logical

        // set via the view to get the right path and all that
        [DoesNotExport, DoesNotAutoMap]
        public virtual string OperatingCenterSAPEnabledUrl { get; set; }

        [DoesNotAutoMap]
        public bool PreviousServiceMaterialWasUpdatedToLead { get; private set; }

        [DoesNotAutoMap]
        public bool CustomerSideSLReplacementWasUpdated { get; private set; }

        [DoesNotAutoMap]
        public bool IsInstalledNonVerification =>
            DateInstalled.HasValue &&
            (ServiceCategory !=
             MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_MEASUREMENT_ONLY ||
             ServiceInstallationPurpose !=
             MapCall.Common.Model.Entities.ServiceInstallationPurpose.Indices.MATERIAL_VERIFICATION);

        #endregion

        #region Validation

        private IEnumerable<ValidationResult> ValidateRetirementInformation()
        {
            if (RetiredDate.HasValue ||
                (DateInstalled.HasValue &&
                 ServiceCategory.HasValue &&
                 (MapCall.Common.Model.Entities.ServiceCategory.GetRenewalServiceCategories().Contains(ServiceCategory.Value) ||
                  ServiceCategory.Value == MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_RETIRE_SERVICE_ONLY)))
            {
                if (!RetiredDate.HasValue)
                    yield return new ValidationResult(ErrorMessages.REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY, new[] { "RetiredDate"});
                if (!OriginalInstallationDate.HasValue)
                    yield return new ValidationResult(ErrorMessages.ORIGINAL_INSTALLATION_DATE, new[] { "OriginalInstallationDate" });
                if (!PreviousServiceSize.HasValue)
                    yield return new ValidationResult(ErrorMessages.PREVIOUS_SERVICE_SIZE, new[] { "PreviousServiceSize" });
                if (!PreviousServiceMaterial.HasValue)
                    yield return new ValidationResult(ErrorMessages.PREVIOUS_SERVICE_MATERIAL, new[] { "PreviousServiceMaterial" });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateRetirementInformation())
                       .Concat(ValidateMeterSettingRequirement())
                       .Concat(ValidateMeterSettingSize());
        }

        private IEnumerable<ValidationResult> ValidateMeterSettingRequirement()
        {
            if (!MeterSettingRequirement.HasValue &&
                (RetiredDate.HasValue || IsInstalledNonVerification) &&
                !SewerServiceCategoryIds().Contains(ServiceCategory.Value))
            {
                yield return new ValidationResult(ErrorMessages.METER_SETTING_REQUIREMENT, new[] { "MeterSettingRequirement" });
            }
        }

        private IEnumerable<ValidationResult> ValidateMeterSettingSize()
        {
            if (!MeterSettingSize.HasValue &&
                IsInstalledNonVerification &&
                !SewerServiceCategoryIds().Contains(ServiceCategory.Value))
            {
                yield return new ValidationResult(ErrorMessages.METER_SETTING_SIZE, new[] { "MeterSettingSize" });
            }
        }

        #endregion
    }
}
