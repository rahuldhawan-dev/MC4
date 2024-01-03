using System;
using System.ComponentModel.DataAnnotations;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels 
{
    public class EditService : ViewModel<Service>
    {
        #region Constants

        public struct ErrorMessages
        {
            public const string
                BLOCK_AND_LOT_OR_STREET_ADDRESS =
                    "You must enter either a 'Street Number and Name' or a 'Block and Lot'.",
                SERVICE_NUMBER_NOT_DUPLICATED =
                    "The premise number '{0}' is not unique to the operating center.",
                REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY =
                    "Required when installed and the service category is for a renewal.";
        }

        #endregion

        #region Fields

        private Service _service;

        #endregion

        #region Constructors

        public EditService(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public Service Display
        {
            get
            {
                if (_service == null)
                {
                    _service = Original ?? _container.GetInstance<IServiceRepository>().Find(Id);
                }
                return _service;
            }
        }
        
        #region depending fields that aren't being edited. Used in RequiredWhens/Cascades

        [DoesNotAutoMap]
        public int? ServiceCategory { get; set; }

        [EntityMap(MapDirections.ToViewModel)]
        public int OperatingCenter { get; set; }

        #endregion

        // 5. 
        [DropDown, EntityMap, EntityMustExist(typeof(MainType))]
        public int? MainType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? MainSize { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public bool? MeterSettingRequirement { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        // TAP SIZE
        public int? MeterSettingSize { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.SERVICE_MATERIAL)]
        [DropDown(
            "",
            "ServiceMaterial",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above")]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? ServiceMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize)), View(Service.DisplayNames.SERVICE_SIZE)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? ServiceSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSideType))]
        public int? ServiceSideType { get; set; }

        public bool? PitInstalled { get; set; }

        [DropDown, EntityMustExist(typeof(ServiceMaterial)), EntityMap]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? CustomerSideMaterial { get; set; }

        [DropDown, EntityMustExist(typeof(ServiceSize)), EntityMap]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? CustomerSideSize { get; set; }

        public bool LeadAndCopperCommunicationProvided { get; set; }
        // Sample Site

        [Multiline]
        public string TapOrderNotes { get; set; }

        // 12. 
        [DropDown, EntityMap, EntityMustExist(typeof(CustomerSideSLReplacementOfferStatus))]
        public int? CustomerSideSLReplacement { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FlushingOfCustomerPlumbingInstructions))]
        public int? FlushingOfCustomerPlumbing { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(CustomerSideSLReplacer))]
        public int? CustomerSideSLReplacedBy { get; set; }

        [RequiredWhen(
            nameof(CustomerSideSLReplacedBy),
            ComparisonType.EqualTo,
            CustomerSideSLReplacer.Indices.CONTRACTOR)]
        [EntityMap, EntityMustExist(typeof(Contractor))]
        [DropDown(
            "",
            "Contractor",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above")]
        public int? CustomerSideSLReplacementContractor { get; set; }

        public int? LengthOfCustomerSideSLReplaced { get; set; }
        public DateTime? CustomerSideReplacementDate { get; set; }

        //// CustomerSideReplacementWBSNumber

        //// 9.
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public DateTime? DateIssuedToField { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        [EntityMap, EntityMustExist(typeof(ServiceRestorationContractor))]
        [DropDown(
            "",
            "ServiceRestorationContractor",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above")]
        public int? WorkIssuedTo { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [DropDown, EntityMap, EntityMustExist(typeof(ServicePriority))]
        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public int? ServicePriority { get; set; }

        [RequiredWhen(nameof(DateInstalled), ComparisonType.NotEqualTo, null)]
        public decimal? LengthOfService { get; set; }

        public int? DepthMainFeet { get; set; }

        public int? DepthMainInches { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        public DateTime? DateInstalled { get; set; }

        //// 10.

        [StringLength(Service.StringLengths.RETIRED_ACCOUNT_NUMBER)]
        public string RetiredAccountNumber { get; set; }

        [StringLength(Service.StringLengths.RETIRE_METER_SET)]
        public string RetireMeterSet { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [View(Service.DisplayNames.PREVIOUS_SERVICE_MATERIAL)]
        [DropDown(
            "",
            "ServiceMaterial",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above")]
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [ClientCallback(
            "Services.validateInstalledAndRenewal",
            ErrorMessage = ErrorMessages.REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY)]
        public int? PreviousServiceMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(StreetMaterial))]
        public int? StreetMaterial { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [DropDown(
            "",
            "ServiceMaterial",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above")]
        [RequiredWhen(
            nameof(ServiceCategory),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE)]
        public int? PreviousServiceCustomerMaterial { get; set; }

        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [ClientCallback(
            "Services.validateInstalledAndRenewal",
            ErrorMessage = ErrorMessages.REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY)]
        public DateTime? OriginalInstallationDate { get; set; }

        public DateTime? RetiredDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [View(Service.DisplayNames.PREVIOUS_SERVICE_SIZE)]
        [RequiredWhen(nameof(RetiredDate), ComparisonType.NotEqualTo, null)]
        [ClientCallback(
            "Services.validateInstalledAndRenewal",
            ErrorMessage = ErrorMessages.REQUIRED_WHEN_INSTALLED_AND_SERVICE_RENEWAL_CATEGORY)]
        public int? PreviousServiceSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [RequiredWhen(
            nameof(ServiceCategory),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE)]
        public int? PreviousServiceCustomerSize { get; set; }

        public bool? CompanyOwned { get; set; }

        #endregion
    }
}
