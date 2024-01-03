using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class BlowOffInspectionViewModel : ViewModel<BlowOffInspection>
    {
        #region Constants

        public const decimal MAX_CHLORINE_LEVEL = 3.2m;
        public const decimal MIN_CHLORINE_LEVEL = 0.2m;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display")]
        public BlowOffInspection Display
        {
            get
            {
                var repo = _container.GetInstance<IBlowOffInspectionRepository>();
                return repo.Find(Id);
            }
        }

        [DoesNotAutoMap("Needed for map views")]
        public bool IsMapPopup { get; set; }

        [Required]
        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(HydrantInspectionType))]
        public int? HydrantInspectionType { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [View("Pre Residual/Free Chlorine")]
        [Range(typeof(decimal), "0", "9.99", ErrorMessage = "Residual chlorine must be between 0 and 9.99")]
        public decimal? PreResidualChlorine { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [View("Post Residual/Free Chlorine")]
        [Range(typeof(decimal), "0", "9.99", ErrorMessage = "Residual chlorine must be between 0 and 9.99")]
        public decimal? ResidualChlorine { get; set; }

        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        [Required]
        [DateTimePicker]
        [Secured(AppliesToAdmins = false)]
        public virtual DateTime? DateInspected { get; set; }

        public bool? FullFlow { get; set; }
        public int? GallonsFlowed { get; set; }

        [RequiredWhen("HydrantInspectionType", ComparisonType.EqualToAny, "GetRequiredInspectionTypes",
            typeof(BlowOffInspectionViewModel), ErrorMessage = "GPM is required for the selected inspection type.")]
        [Min(0.00001, ErrorMessage = "GPM must be greater than zero.")]
        public decimal? GPM { get; set; }

        [RequiredWhen("HydrantInspectionType", ComparisonType.EqualToAny, "GetRequiredInspectionTypes",
            typeof(BlowOffInspectionViewModel),
            ErrorMessage = "Minutes flowed is required for the selected inspection type.")]
        [Min(0.00001, ErrorMessage = "Minutes flowed must be greater than zero.")]
        public decimal? MinutesFlowed { get; set; }

        //[RequiredWhen("HydrantInspectionType", ComparisonType.EqualToAny, "GetRequiredInspectionTypes", typeof(BlowOffInspectionViewModel), ErrorMessage = "Static Pressure is required for the selected inspection type.")]
        [Range(0.00001, 300, ErrorMessage = "Static pressure must be greater than 0 and less than or equal to 300.")]
        public decimal? StaticPressure { get; set; }

        [Multiline]
        [StringLengthNotRequired]
        public string Remarks { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [View("Pre Total Chlorine")]
        [Range(typeof(decimal), "0", "4", ErrorMessage = "Total chlorine must be between 0 and 4.")]
        public decimal? PreTotalChlorine { get; set; }

        // NOTE: The RangeAttribute is stupid and will round the value being compared if it gets
        //       integers as the min/max values. To get around this, you need to specify the type as decimal.
        [View("Post Total Chlorine")]
        [Range(typeof(decimal), "0", "4", ErrorMessage = "Total chlorine must be between 0 and 4.")]
        public decimal? TotalChlorine { get; set; }

        [Required]
        [EntityMap]
        [EntityMustExist(typeof(Valve))]
        [Secured]
        public virtual int? Valve { get; set; }

        [DisplayName("Valve")]
        [DoesNotAutoMap("Display")]
        public Valve ValveDisplay => _container.GetInstance<IValveRepository>().Find(Valve.GetValueOrDefault());

        [DoesNotAutoMap("Set by MapToEntity - for controller to know if it gets sent")]
        public bool SendToSAP { get; set; }

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

        #endregion

        #region Constructors

        public BlowOffInspectionViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public static int[] GetRequiredInspectionTypes()
        {
            return HydrantInspectionViewModel.GetRequiredInspectionTypes();
        }

        #endregion

        // This gets set by the page via js.
    }

    public class CreateBlowOffInspection : BlowOffInspectionViewModel
    {
        #region Constructors

        public CreateBlowOffInspection(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateAssetIsInspectable()
        {
            var valve = _container.GetInstance<IValveRepository>().Find(Valve.GetValueOrDefault());
            if (valve == null)
            {
                yield break;
            }

            if (!valve.IsInspectable)
            {
                yield return new ValidationResult(
                    "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.",
                    new[] {"Valve"});
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            DateInspected = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public override BlowOffInspection MapToEntity(BlowOffInspection entity)
        {
            entity = base.MapToEntity(entity);
            entity.InspectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            SendToSAP = entity.Valve.OperatingCenter.SAPEnabled &&
                        !entity.Valve.OperatingCenter.IsContractedOperations;
            entity.GallonsFlowed = (int?)(MinutesFlowed * GPM);
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateAssetIsInspectable());
        }

        #endregion
    }

    public class EditBlowOffInspection : BlowOffInspectionViewModel
    {
        #region Constructors

        public EditBlowOffInspection(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override BlowOffInspection MapToEntity(BlowOffInspection entity)
        {
            entity = base.MapToEntity(entity);
            SendToSAP = entity.Valve != null && entity.Valve.OperatingCenter.SAPEnabled &&
                        !entity.Valve.OperatingCenter.IsContractedOperations && (entity.SAPNotificationNumber == null ||
                            entity.SAPNotificationNumber == "");
            entity.GallonsFlowed = (int?)(MinutesFlowed * GPM);
            return entity;
        }

        #endregion
    }

    public class SearchBlowOffInspection : SearchSet<BlowOffInspectionSearchResultViewModel>, ISearchBlowOffInspection
    {
        #region Properties

        [Required]
        [MultiSelect]
        public int[] OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DisplayName("Valve(Number Only)")]
        public int? ValveSuffix { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        public int? InspectedBy { get; set; }

        public DateRange DateInspected { get; set; }
        public DateRange CreatedAt { get; set; }

        public bool? WorkOrderRequired { get; set; }
        public int? WorkOrderRequestOne { get; set; }

        [BoolFormat(Valve.Display.SIZE_RANGE_LARGE_VALVE, Valve.Display.SIZE_RANGE_SMALL_VALVE)]
        [DisplayName(Valve.Display.IS_LARGE_VALVE)]
        [SearchAlias("Valve", "val", "IsLargeValve")]
        public bool? IsLargeValve { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }

        public string SAPErrorCode { get; set; }

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

        [DropDown]
        public int? HydrantInspectionType { get; set; }

        public bool? FullFlow { get; set; }

        public NumericRange GPM { get; set; }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (WorkOrderRequired.GetValueOrDefault())
            {
                mapper.MappedProperties["WorkOrderRequestOne"].Value = SearchMapperSpecialValues.IsNotNull;
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
    }
}
