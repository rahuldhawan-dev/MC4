using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class ValveInspectionViewModel : ViewModel<ValveInspection>
    {
        public struct ErrorMessages
        {
            public const string TURNS =
                "You must enter at least the minimum number of turns or check turns not completed.";

            public const string POSITION_FOUND = "The Position Found field is required.";

            public const string POSITION_LEFT = "The Position Left field is required.";
        }

        #region Properties

        [DoesNotAutoMap]
        public ValveInspection Display
        {
            get
            {
                var repo = _container.GetInstance<IRepository<ValveInspection>>();
                return repo.Find(Id);
            }
        }

        // This gets set by the page via js.
        [DoesNotAutoMap]
        public bool IsMapPopup { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Valve)), Secured]
        public int? Valve { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        [Required, DateTimePicker, Secured(AppliesToAdmins = false)]
        public DateTime? DateInspected { get; set; }
        
        [Required, BoolFormat("Yes", "No")]
        public bool? Inspected { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ValveNormalPosition))]
        [ClientCallback("ValveInspections.validatePositionFoundRequired", ErrorMessage = ErrorMessages.POSITION_FOUND)]
        public int? PositionFound { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ValveNormalPosition))]
        [ClientCallback("ValveInspections.validatePositionLeftRequired", ErrorMessage = ErrorMessages.POSITION_LEFT)]
        public int? PositionLeft { get; set; }
        
        [Required, ClientCallback("ValveInspections.validateTurns", ErrorMessage = ErrorMessages.TURNS)]
        public decimal? Turns { get; set; }

        public bool TurnsNotCompleted { get; set; }

        [Multiline]
        public string Remarks { get; set; }

        [DoesNotAutoMap, DisplayName("Valve")]
        public Valve ValveDisplay
        {
            get
            {
                return _container.GetInstance<IRepository<Valve>>().Find(Valve.GetValueOrDefault());
            }
        }

        //Set by MapToEntity - for controller to know if it gets sent
        [DoesNotExport]
        public bool SendToSAP { get; set; }

        #endregion

        #region Constructors

        public ValveInspectionViewModel(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateConditionalPositionFoundRequired()
        {
            if (Inspected.GetValueOrDefault() && !TurnsNotCompleted && !PositionFound.HasValue)
                yield return new ValidationResult(ErrorMessages.POSITION_FOUND, new[] { nameof(PositionFound) });
        }

        private IEnumerable<ValidationResult> ValidateConditionalPositionLeftRequired()
        {
            if (Inspected.GetValueOrDefault() && !TurnsNotCompleted && !PositionLeft.HasValue)
                yield return new ValidationResult(ErrorMessages.POSITION_LEFT, new[] { nameof(PositionLeft) });
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateConditionalPositionFoundRequired())
                       .Concat(ValidateConditionalPositionLeftRequired());
        }

        #endregion
    }

    public class CreateValveInspection : ValveInspectionViewModel
    {
        //[DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS)]
        //public virtual decimal? MinimumRequiredTurns { get; set; }

        #region Constructors

		public CreateValveInspection(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateAssetIsInspectable()
        {
            var valve = _container.GetInstance<IRepository<Valve>>().Find(Valve.GetValueOrDefault());
            if (valve == null)
            {
                yield break;
            }

            if (!valve.IsInspectable)
            {
                yield return new ValidationResult("New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.", new[] { "Valve" });
            }
        }

        private IEnumerable<ValidationResult> ValidateNumberOfTurns()
        {
            if (Turns == null || ValveDisplay == null) yield break;  // testing

            if (Turns < ValveDisplay.MinimumRequiredTurns && !TurnsNotCompleted)
                yield return new ValidationResult(ErrorMessages.TURNS, new[] { "Turns" });
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            DateInspected = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public override ValveInspection MapToEntity(ValveInspection entity)
        {
            entity = base.MapToEntity(entity);
            entity.InspectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            if (ValveDisplay != null)
            {
                entity.MinimumRequiredTurns = ValveDisplay.MinimumRequiredTurns;
                entity.OperatingCenter = ValveDisplay.OperatingCenter;
            }
            
            SendToSAP = entity.Valve != null && entity.Valve.OperatingCenter.SAPEnabled &&
            !entity.Valve.OperatingCenter.IsContractedOperations;

            return entity;
        }
        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateAssetIsInspectable())
                       .Concat(ValidateNumberOfTurns());
        }

        #endregion
    }

    public class EditValveInspection : ValveInspectionViewModel
    {
        #region Properties

        #region Properties

        public User InspectedBy
        {
            get { return _container.GetInstance<IRepository<ValveInspection>>().Find(Id).InspectedBy; }
        }

        #endregion

        #endregion
        
        #region Constructors

		public EditValveInspection(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override ValveInspection MapToEntity(ValveInspection entity)
        {
            entity = base.MapToEntity(entity);
            SendToSAP = entity.Valve != null && entity.Valve.OperatingCenter.SAPEnabled && !entity.Valve.OperatingCenter.IsContractedOperations && (entity.SAPNotificationNumber == null || entity.SAPNotificationNumber == "");
            if (entity.Valve != null)
            {
                entity.OperatingCenter = entity.Valve.OperatingCenter;
            }
            return entity;
        }

        #endregion
    }

    public class SearchValveInspection : SearchSet<ValveInspectionSearchResultViewModel>, ISearchValveInspection
    {
        #region Properties

        [Required, MultiSelect]
        public int[] OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? InspectedBy { get; set; }
        public DateRange DateInspected { get; set; }
        public DateRange CreatedAt { get; set; }
        public bool? WorkOrderRequired { get; set; }

        [DropDown("FieldOperations", "Valve", "RouteByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public int? Route { get; set; }

        // Needed for ModifyValues and so that the search mapper
        // has a property for this. Not part of view.
        [SearchAlias("WorkOrderRequestOne", "wor1", "Id")]
        public int? WorkOrderRequestOne { get; set; }

        //ValveNumber
        [DisplayName("Valve Suffix(Number Only)")]
        public int? ValveSuffix { get; set; }

        [BoolFormat(Valve.Display.SIZE_RANGE_LARGE_VALVE, Valve.Display.SIZE_RANGE_SMALL_VALVE), DisplayName(Valve.Display.IS_LARGE_VALVE), SearchAlias("Valve", "val", "IsLargeValve")]
        public bool? IsLargeValve { get; set; }
       
        [Description("Select yes if only equipment with an SAP equipment ID should be returned.")]
        public bool? SAPEquipmentOnly { get; set; }

        // for modify values
        public int? SAPEquipmentId { get; set; }

        [DropDown]
        public int? ValveZone { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }
        public string SAPErrorCode { get; set; }
        public SearchString SAPNotificationNumber { get; set; }

        #endregion

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
	}

    public class BaseSearchValveInspectionsByMonth<T> : SearchSet<T>
        where T: class 
    {
        #region Properties

        [Required, MultiSelect]
        public int[] OperatingCenter { get; set; }

        [Required, DropDown]
        public int? Year { get; set; }

        #endregion
    }

    public class SearchValveInspectionsByMonth : BaseSearchValveInspectionsByMonth<ValveInspectionsByMonthReportItem>, ISearchValveInspectionsByMonthReport { }
    public class SearchValvesOperatedByMonth : BaseSearchValveInspectionsByMonth<ValvesOperatedByMonthReportItem>, ISearchValvesOperatedByMonthReport { }
    public class SearchRequiredValvesOperatedByMonth : BaseSearchValveInspectionsByMonth<RequiredValvesOperatedByMonthReportItem>, ISearchRequiredValvesOperatedByMonthReport { }
}