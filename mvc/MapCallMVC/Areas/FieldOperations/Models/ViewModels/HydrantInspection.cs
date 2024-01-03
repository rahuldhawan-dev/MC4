using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class HydrantInspectionViewModel : ViewModel<HydrantInspection>
    {
        #region Constants

        public const decimal MAX_CHLORINE_LEVEL = BlowOffInspectionViewModel.MAX_CHLORINE_LEVEL;
        public const decimal MIN_CHLORINE_LEVEL = BlowOffInspectionViewModel.MIN_CHLORINE_LEVEL;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public HydrantInspection Display { get; set; }

        // This gets set by the page via js.
        [DoesNotAutoMap]
        public bool IsMapPopup { get; set; }

        [RequiredWhen("State", ComparisonType.EqualToAny, new[] {MapCall.Common.Model.Entities.State.Indices.NJ})]
        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(HydrantTagStatus))]
        public int? HydrantTagStatus { get; set; }

        [Required]
        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(HydrantInspectionType))]
        public int? HydrantInspectionType { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [Range(typeof(decimal), "0", "9.99", ErrorMessage = "Residual chlorine must be between 0 and 9.99")]
        public decimal? PreResidualChlorine { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [Range(typeof(decimal), "0", "9.99", ErrorMessage = "Residual chlorine must be between 0 and 9.99")]
        public decimal? ResidualChlorine { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        [Required]
        [DateTimePicker]
        [Secured(AppliesToAdmins = false)]
        public DateTime? DateInspected { get; set; }

        [RequiredWhen("ResidualChlorine", ComparisonType.EqualTo, null)]
        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(NoReadReason))]
        public int? FreeNoReadReason { get; set; }

        [RequiredWhen("TotalChlorine", ComparisonType.EqualTo, null)]
        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(NoReadReason))]
        public int? TotalNoReadReason { get; set; }

        public bool? FullFlow { get; set; }

        [RequiredWhen("HydrantInspectionType", ComparisonType.EqualToAny, "GetRequiredInspectionTypes",
            typeof(HydrantInspectionViewModel), ErrorMessage = "GPM is required for the selected inspection type.")]
        [Min(0.00001, ErrorMessage = "GPM must be greater than zero.")]
        public decimal? GPM { get; set; }

        [RequiredWhen("HydrantInspectionType", ComparisonType.EqualToAny, "GetRequiredInspectionTypes",
            typeof(HydrantInspectionViewModel),
            ErrorMessage = "Minutes flowed is required for the selected inspection type.")]
        [Min(0.00001, ErrorMessage = "Minutes flowed must be greater than zero.")]
        public decimal? MinutesFlowed { get; set; }

        [RequiredWhen("HydrantInspectionType", ComparisonType.EqualToAny, "GetRequiredInspectionTypes",
            typeof(HydrantInspectionViewModel),
            ErrorMessage = "Static Pressure is required for the selected inspection type.")]
        [Range(0.00001, 300, ErrorMessage = "Static pressure must be greater than 0 and less than or equal to 300.")]
        public decimal? StaticPressure { get; set; }

        [Multiline]
        [StringLengthNotRequired]
        public string Remarks { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [Range(typeof(decimal), "0", "4", ErrorMessage = "Total chlorine must be between 0 and 4.")]
        public decimal? PreTotalChlorine { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [Range(typeof(decimal), "0", "4", ErrorMessage = "Total chlorine must be between 0 and 4.")]
        public decimal? TotalChlorine { get; set; }

        [Required]
        [EntityMap]
        [EntityMustExist(typeof(Hydrant))]
        [Secured]
        public int? Hydrant { get; set; }

        [DisplayName("Hydrant")]
        [DoesNotAutoMap]
        public Hydrant HydrantDisplay =>
            _container.GetInstance<IRepository<Hydrant>>().Find(Hydrant.GetValueOrDefault());

        [DoesNotAutoMap("Set by MapToEntity - for controller to know if it gets sent")]
        public bool SendToSAP { get; set; }

        [DoesNotAutoMap("I do not know what this is for")]
        public int State { get; set; }

        #endregion

        #region Constructors

        public HydrantInspectionViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        #region Validation

        public static int[] GetRequiredInspectionTypes()
        {
            return new[] {
                MapCall.Common.Model.Entities.HydrantInspectionType.Indices.FLUSH,
                MapCall.Common.Model.Entities.HydrantInspectionType.Indices.INSPECT_FLUSH,
                MapCall.Common.Model.Entities.HydrantInspectionType.Indices.WATER_QUALITY
            };
        }

        #endregion

        public override void Map(HydrantInspection entity)
        {
            base.Map(entity);
            var repo = _container.GetInstance<IRepository<HydrantInspection>>();
            Display = repo.Find(Id);
            State = HydrantDisplay?.Town?.State?.Id ?? 0;
        }

        #endregion
    }

    public class CreateHydrantInspection : HydrantInspectionViewModel
    {
        #region Constructors

        public CreateHydrantInspection(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap("Count of work orders")]
        public int? WorkOrderCount { get; set; }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateAssetIsInspectable()
        {
            var hydrant = _container.GetInstance<IRepository<Hydrant>>().Find(Hydrant.GetValueOrDefault());
            if (hydrant == null)
            {
                yield break;
            }

            if (!hydrant.IsInspectable)
            {
                yield return new ValidationResult(
                    "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.",
                    new[] {"Hydrant"});
            }
        }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            DateInspected = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            State = HydrantDisplay.Town.State.Id;
            HydrantTagStatus = HydrantDisplay.HydrantTagStatus?.Id;
            WorkOrderCount = HydrantDisplay.WorkOrders?.Count();
        }

        public override HydrantInspection MapToEntity(HydrantInspection entity)
        {
            entity = base.MapToEntity(entity);
            entity.InspectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            SendToSAP = entity.Hydrant.OperatingCenter.SAPEnabled &&
                        !entity.Hydrant.OperatingCenter.IsContractedOperations;
            if (entity.Hydrant != null)
            {
                entity.OperatingCenter = entity.Hydrant.OperatingCenter;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateAssetIsInspectable());
        }

        #endregion
    }

    public class EditHydrantInspection : HydrantInspectionViewModel
    {
        #region Properties

        public User InspectedBy => _container.GetInstance<IRepository<HydrantInspection>>().Find(Id).InspectedBy;

        #endregion

        #region Constructors

        public EditHydrantInspection(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override HydrantInspection MapToEntity(HydrantInspection entity)
        {
            entity = base.MapToEntity(entity);
            SendToSAP = entity.Hydrant != null && entity.Hydrant.OperatingCenter.SAPEnabled &&
                        !entity.Hydrant.OperatingCenter.IsContractedOperations &&
                        (entity.SAPNotificationNumber == null || entity.SAPNotificationNumber == "");
            if (entity.Hydrant != null)
            {
                entity.OperatingCenter = entity.Hydrant.OperatingCenter;
            }

            return entity;
        }

        #endregion
    }

    public class ReplaceHydrantInspection : EditHydrantInspection
    {
        #region Constructors

        public ReplaceHydrantInspection(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchHydrantInspection : SearchSet<HydrantInspectionSearchResultViewModel>, ISearchHydrantInspection
    {
        #region Properties

        [Required]
        [MultiSelect]
        public int[] OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown("", "FireDistrict", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? FireDistrict { get; set; }

        [DisplayName("Hydrant(Number Only)")]
        public int? HydrantSuffix { get; set; }

        [DropDown]
        [DisplayName("Inspection Type")]
        public int? HydrantInspectionType { get; set; }

        [DropDown("FieldOperations", "Hydrant", "RouteByTownId", DependsOn = "Town",
            PromptText = "Please select a town above")]
        public int? Route { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        public int? InspectedBy { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(HydrantTagStatus))]
        public int? HydrantTagStatus { get; set; }

        public DateRange DateInspected { get; set; }
        public DateRange CreatedAt { get; set; }
        public bool? WorkOrderRequired { get; set; }

        // Needed for ModifyValues and so that the search mapper
        // has a property for this. Not part of view.
        public int? WorkOrderRequestOne { get; set; }

        [Description("Select yes if only equipment with an SAP equipment ID should be returned.")]
        public bool? SAPEquipmentOnly { get; set; }

        // Needed for ModifyValues. Not part of view at the moment.
        public int? SAPEquipmentId { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }

        public string SAPErrorCode { get; set; }
        public SearchString SAPNotificationNumber { get; set; }

        [View("Pre Residual/Free Chlorine", Description = "(0 - 9.99)")]
        public NumericRange PreResidualChlorine { get; set; }

        [View("Post Residual/Free Chlorine", Description = "(0 - 9.99)")]
        public NumericRange ResidualChlorine { get; set; }

        [View("Pre Total Chlorine", Description = "(0 - 4)")]
        public NumericRange PreTotalChlorine { get; set; }

        [View("Post Total Chlorine", Description = "(0 - 4)")]
        public NumericRange TotalChlorine { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(NoReadReason))]
        public int? FreeNoReadReason { get; set; }

        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(NoReadReason))]
        public int? TotalNoReadReason { get; set; }

        public bool? FullFlow { get; set; }

        public NumericRange GPM { get; set; }

        #endregion

        #region Exposed Methods

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (WorkOrderRequired.GetValueOrDefault())
            {
                mapper.MappedProperties["WorkOrderRequestOne"].Value = SearchMapperSpecialValues.IsNotNull;
            }

            if (SAPEquipmentOnly.GetValueOrDefault())
            {
                mapper.MappedProperties["SAPEquipmentId"].Value = SearchMapperSpecialValues.IsNotNull;
            }

            if (HasSAPErrorCode.HasValue)
            {
                if (HasSAPErrorCode.Value)
                {
                    mapper.MappedProperties["SAPErrorCode"].Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mapper.MappedProperties["SAPErrorCode"].Value = SearchMapperSpecialValues.IsNull;
                }
            }

            // Needs to sort by DateInspected by default.
            if (string.IsNullOrWhiteSpace(SortBy))
            {
                SortBy = "DateInspected";
                SortAscending = false;
            }
        }

        #endregion

        #endregion
    }

    public class SearchHydrantFlushing : SearchSet<HydrantFlushingReportItem>, ISearchHydrantFlushingReport
    {
        #region Properties

        [DropDown]
        [Required]
        public int? OperatingCenter { get; set; }

        [Required]
        public int? Year { get; set; }

        #endregion
    }

    public class SearchKPIHydrantInspectionsByMonth : SearchSet<KPIHydrantsInspectedReportItem>,
        ISearchKPIHydrantInspectionReport
    {
        #region Properties

        [MultiSelect]
        public int[] OperatingCenter { get; set; }

        [Required]
        [DropDown]
        public int? Year { get; set; }

        #endregion
    }
}
