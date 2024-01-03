using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IContainer = StructureMap.IContainer;
using ValveStatuses = MapCall.Common.Model.Entities.AssetStatus.Indices;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class ValveViewModel : ViewModel<Valve>
    {
        #region Constants

        public struct ErrorMessages
        {
            #region Constants

            public const string
                ERROR_CRITICAL_NOTES_MUST_BE_NULL = "Critical checkbox must be checked when setting critical notes.",
                CONTROLS_CROSSING_MUST_BE_CRITICAL = "Valves that control crossings must be marked as critical.",
                ERROR_TURNS_REQUIRED = "The # of Turns is required when Installed or Active and the Type is Ball, Butterfly, Gate, or Tapping.",
                SUFFIX_GREATER_THAN_ZERO = "Valve Suffix must be greater than zero.",
                SUFFIX_ALREADY_EXISTS = "A valve already exists with this valve suffix.",
                SUFFIX_TOO_LARGE = "A found valve cannot have a suffix greater than the current maximum valve number.",
                SUFFIX_REQUIRED = "Valve Suffix is required for found Valves.";

            #endregion
        }

        #endregion

        #region Properties

        // This needs to be overridden in both create and edit models.
        public virtual int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveBilling))]
        [DisplayName(Valve.Display.VALVE_BILLING)]
        [DropDown, Required]
        public virtual int? ValveBilling { get; set; }

        [EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        [DropDown]
        public virtual int? InspectionFrequencyUnit { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveNormalPosition))]
        [DropDown]
        [RequiredWhen(nameof(Status), ComparisonType.EqualToAny, nameof(GetActiveInstalledRequiredStatusIds), typeof(ValveViewModel), ErrorMessage = "Normal Position is required for active / installed valves.")]
        public virtual int? NormalPosition { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveOpenDirection))]
        [DropDown]
        public virtual int? OpenDirection { get; set; }

        [EntityMap, EntityMustExist(typeof(TownSection))]
        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public virtual int? TownSection { get; set; }

        [EntityMap, EntityMustExist(typeof(MainType))]
        [DropDown]
        public virtual int? MainType { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveControl))]
        [DropDown, Required]
        public virtual int? ValveControls { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveManufacturer))]
        [DropDown]
        public virtual int? ValveMake { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveType))]
        [DropDown]
        [RequiredWhen(nameof(Status), ComparisonType.EqualToAny, nameof(GetActiveInstalledRequiredStatusIds), typeof(ValveViewModel), ErrorMessage = "Valve Type is required for active / installed valves.")]
        public virtual int? ValveType { get; set; }

        [Required]
        [EntityMap, EntityMustExist(typeof(ValveSize))]
        [DisplayName(Valve.Display.VALVE_SIZE)]
        [DropDown]
        public virtual int? ValveSize { get; set; }

        [EntityMap(RepositoryType = typeof(IRepository<AssetStatus>)), EntityMustExist(typeof(AssetStatus))]
        [DropDown, Required]
        public virtual int? Status { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ValveZone))]
        [DropDown]
        public virtual int? ValveZone { get; set; }

        [EntityMap, EntityMustExist(typeof(WaterSystem))]
        [DropDown("Admin", "WaterSystem", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? WaterSystem { get; set; }

        // Dropdown done in overrides
        [ClientCallback("Valve.validateFunctionalLocation", ErrorMessage = "The Functional Location field is required.")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForValveAssetType", DependsOn = "Town,ValveControls", PromptText = "Please select a town above")]
        public virtual int? FunctionalLocation { get; set; }

        [EntityMap, EntityMustExist(typeof(Facility))]
        [DropDown("", "Facility", "ByTownId", DependsOn = "Town")]
        public int? Facility { get; set; }

        [Coordinate(AddressCallback = "Valve.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMap]
        [RequiredWhen(nameof(Status), MapCall.Common.Model.Entities.AssetStatus.Indices.ACTIVE, ErrorMessage = "Coordinate is required for active valves.")]
        public virtual int? Coordinate { get; set; }

        [CheckBox]
        public virtual bool? Critical { get; set; }

        [ClientCallback("Valve.validateCriticalNotes", ErrorMessage = ErrorMessages.ERROR_CRITICAL_NOTES_MUST_BE_NULL)]
        [Multiline, RequiredWhen(nameof(Critical), true), StringLength(Valve.StringLengths.CRITICAL_NOTES)]
        public string CriticalNotes { get; set; }

        [DisplayName(Valve.Display.BPUKPI)]
        public virtual bool BPUKPI { get; set; }

        [RequiredWhen(nameof(Status), ComparisonType.EqualToAny, nameof(GetDateRetiredRequiredStatusIds), typeof(ValveViewModel), ErrorMessage = "DateRetired is required for retired / removed valves.")]
        public virtual DateTime? DateRetired { get; set; }

        public virtual DateTime? DateTested { get; set; }
        public virtual decimal? Elevation { get; set; }

        [StringLength(Valve.StringLengths.GISUID, MinimumLength = 18)]
        public virtual string GISUID { get; set; }

        public virtual int? InspectionFrequency { get; set; }

        [StringLength(Valve.StringLengths.MAP_PAGE)]
        public virtual string MapPage { get; set; }

        [DisplayName(Valve.Display.ROUTE)]
        public virtual int? Route { get; set; }

        public virtual decimal? Stop { get; set; }

        [StringLength(Valve.StringLengths.SKETCH_NUMBER)]
        [DisplayName(Valve.Display.SKETCH_NUMBER)]
        public virtual string SketchNumber { get; set; }

        [StringLength(Valve.StringLengths.STREET_NUMBER)]
        public virtual string StreetNumber { get; set; }

        [DisplayName(Valve.Display.TRAFFIC)]
        public virtual bool Traffic { get; set; }

        [DisplayName(Valve.Display.TURNS)]
        [ClientCallback("Valve.validateTurns", ErrorMessage = ErrorMessages.ERROR_TURNS_REQUIRED)]
        //[RequiredWhen("ValveStatus", ComparisonType.EqualTo, ValveStatuses.ACTIVE)]
        //[RequiredWhen("ValveType", ComparisonType.EqualToAny, new[]{ MapCall.Common.Model.Entities.ValveType.Indices.BALL, MapCall.Common.Model.Entities.ValveType.Indices.BUTTERFLY, MapCall.Common.Model.Entities.ValveType.Indices.GATE, MapCall.Common.Model.Entities.ValveType.Indices.TAPPING})]
        public virtual decimal? Turns { get; set; }

        [StringLength(Valve.StringLengths.VALVE_LOCATION)]
        public virtual string ValveLocation { get; set; }

        [DropDown("", "Gradient", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above."), EntityMap, EntityMustExist(typeof(Gradient))]
        public int? Gradient { get; set; }

        //This is required in class? why?
        [RequiredWhen(nameof(Status), ComparisonType.EqualTo, ValveStatuses.ACTIVE)]
        public virtual DateTime? DateInstalled { get; set; }

        public bool ControlsCrossing { get; set; }

        [DoesNotAutoMap("Not an entity property.")]
        public bool SendToSAP { get; set; }

        public int? DepthFeet { get; set; }
        public int? DepthInches { get; set; }

        #endregion

        #region Constructors

        public ValveViewModel(IContainer container) : base(container) { }

        #endregion

        #region Protected Methods

        /*
         * not all valves are created equal, for example, a valve that is created by the importer
         * must always use whatever is in the imported file for inspection frequencies... and a
         * valve that is created by the ui must follow the logic below for setting inspection frequencies.
         *
         * therefore, to support this (mc-1059) we needed to allow for the importer to use
         * a different concrete valve view model instance than create/edits (createvalvebase), one which does not set inspection
         * frequency with the logic below.
         *
         * therefore x2, we have this method here as protected so edit/create can access it and use it. 
         */
        protected virtual void SetInspectionFrequency()
        {
            // This needs to happen *before* base.MapToEntity call.
            // OperatingCenter is a required field so if this is throwing an error you're doing something wrong.
            var opc = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.Value);
            if (!opc.UsesValveInspectionFrequency)
            {
                return;
            }

            // Inspection frequency needs to be set based on small/large valve. ValveSize is not required,
            // Alex says to use small valve if the valve size is not set.
            // Valve.IsLargeValve does this logic, but it's a formula field and isn't usable for view models, especially
            // during creates.
            var size = _container.GetInstance<IRepository<ValveSize>>().Find(ValveSize.GetValueOrDefault());
            var isLargeValve = (size != null && size.Size >= 12);

            if (!InspectionFrequency.HasValue)
            {
                InspectionFrequency = isLargeValve ? opc.LargeValveInspectionFrequency : opc.SmallValveInspectionFrequency;
            }
            if (InspectionFrequencyUnit == null)
            {
                InspectionFrequencyUnit = isLargeValve ? opc.LargeValveInspectionFrequencyUnit.Id : opc.SmallValveInspectionFrequencyUnit.Id;
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateTurns()
        {
            if ((Turns == null || Turns == 0) 
                    && (Status == ValveStatuses.ACTIVE || Status == ValveStatuses.INSTALLED)
                    && ValveType != null &&
                    MapCall.Common.Model.Entities.ValveType.VALVE_TYPES_REQUIRING_TURNS.Contains(ValveType.Value))
                yield return new ValidationResult(ErrorMessages.ERROR_TURNS_REQUIRED, new[] {"Turns"});
        }

        private IEnumerable<ValidationResult> ValidateCriticalNotes()
        {
            if (!Critical.GetValueOrDefault() && !string.IsNullOrWhiteSpace(CriticalNotes))
            {
                yield return new ValidationResult(ErrorMessages.ERROR_CRITICAL_NOTES_MUST_BE_NULL, new[] { "CriticalNotes" });
            }
        }

        private IEnumerable<ValidationResult> ValidateCriticalAndControlsCrossing()
        {
            // If the user checks the ControlsCrossing checkbox, they must also check off Critical and fill in CriticalNotes.
            if (ControlsCrossing && !Critical.GetValueOrDefault())
            {
                yield return new ValidationResult(ErrorMessages.CONTROLS_CROSSING_MUST_BE_CRITICAL, new[] { nameof(ControlsCrossing) });
            }
        }

        public static int[] GetDateRetiredRequiredStatusIds() => AssetStatus.RETIRED_STATUS_IDS;
        public static int[] GetActiveInstalledRequiredStatusIds() => new[] { ValveStatuses.ACTIVE, ValveStatuses.INSTALLED };

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateCriticalNotes())
                       .Concat(ValidateCriticalAndControlsCrossing())
                       .Concat(ValidateTurns());
        }

        public override Valve MapToEntity(Valve entity)
        {
            var previousValveStatus = entity.Status;
            
            base.MapToEntity(entity);

            SendToSAP = entity.OperatingCenter.CanSyncWithSAP;

            // if existing == Cancelled, Retired, or Removed and existing.Id == viewModel.HydrantStatusId then false
            if (previousValveStatus?.Id == Status.Value)
            {
                if (previousValveStatus.Id == ValveStatuses.CANCELLED ||
                    previousValveStatus.Id == ValveStatuses.RETIRED ||
                    previousValveStatus.Id == ValveStatuses.REMOVED)
                {
                    SendToSAP = false;
                }
            }

            if (!entity.ControlsCrossing)
            {
                // Need to clear these as a main crossing should only be attached when ControlsCrossing is true.
                // The website doesn't clear these out if they uncheck the ControlsCrossing box as that would be
                // annoying if it was an accidental uncheck(and then recheck and then having to reselect all of them).
                entity.MainCrossings.Clear();
            }

            if (entity.DateRetired.HasValue && !AssetStatus.RETIRED_STATUS_IDS.Contains(entity.Status.Id))
            {
                entity.DateRetired = null;
            }

            return entity;
        }

        #endregion
    }

    public class CopyValve : CreateValve
    {
        #region Properties

        [Required, RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [StringLength(Valve.StringLengths.VALVE_NUMBER)]
        public virtual string ValveNumber { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveBilling))]
        [DisplayName(Valve.Display.VALVE_BILLING)]
        [DropDown, Required, RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? ValveBilling { get; set; }

        [DisplayName(Valve.Display.BPUKPI), RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override bool BPUKPI { get; set; }

        //  [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForValveAssetType", DependsOn = "Town,ValveControls", PromptText = "Please select a town above")]
        [RoleSecured(ValveController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [ClientCallback("Valve.validateFunctionalLocation", ErrorMessage = "The Functional Location field is required.")]
        public override int? FunctionalLocation { get; set; }

        //Does not stink as much as Ross thinks -arr
        // I disagree -Ross
        [DoesNotAutoMap]
        public bool SendNotificationsOnSave { get; set; }

        public int? SAPEquipmentId { get; set; }

        #endregion

        #region Constructors

        public CopyValve(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) : base(container, numberValidator) { }

        #endregion

        #region Exposed Methods

        public override void Map(Valve entity)
        {
            base.Map(entity);
            ValveNumber = null;
            Coordinate = null;
            DateInstalled = null;
        }

        public override Valve MapToEntity(Valve entity)
        {
            base.MapToEntity(entity);
            entity.SAPEquipmentId = null;
            if (Status == ValveStatuses.ACTIVE)
                SendNotificationsOnSave = true;
            entity.Status = new AssetStatus {Id = ValveStatuses.PENDING};
            return entity;
        }

        #endregion
    }

    public class ReplaceValve : EditValve
    {
        #region Private Members

        private Valve _retiredValve;
        private readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Exposed Methods

        public override void Map(Valve entity)
        {
            base.Map(entity);
            _retiredValve = entity; 
            DateInstalled = null;
        }

        public override Valve MapToEntity(Valve entity)
        {
            // These need to be set prior to base.MapToEntity call because they won't get mapped in
            // time for the SendToSAP setter otherwise.
            entity.OperatingCenter = _retiredValve.OperatingCenter;
            entity.Town = _retiredValve.Town;

            // NOTE: The entity being passed to this is NOT an existing hydrant record!
            base.MapToEntity(entity);

            // Do not populate the SAP Equipment ID when creating the new pending asset record.  
            // This way when the new pending record is saved, it will automatically create a new record in SAP.
            entity.SAPEquipmentId = null;

            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            // All replacement hydrants must be set to pending initially.
            entity.Status = _container.GetInstance<IRepository<AssetStatus>>().Find(ValveStatuses.PENDING);


            // Copy the coordinate to a new coordinate record.
            var rc = _retiredValve.Coordinate;
            if (rc != null)
            {
                entity.Coordinate = new Coordinate
                {
                    // Not all Coordinate records have an icon set, but it's mapped as not nullable so set it to a default.
                    Icon = rc.Icon ?? _container.GetInstance<IRepository<IconSet>>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon,
                    Latitude = rc.Latitude,
                    Longitude = rc.Longitude
                };
            }

            // NOTE: DO NOT COPY VALVE INSPECTIONS, ONLY BLOWOFF INSPECTIONS. http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c6

            foreach (var inspection in _retiredValve.BlowOffInspections)
            {
                // NOTE: Important to use the EDIT view model as the create view model will overwrite data.
                // NOTE 2: We're not supposed to copy inspections over to SAP. http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c9
                var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);
                model.Valve = null;
                var inspectCopy = new BlowOffInspection();
                model.MapToEntity(inspectCopy);
                inspectCopy.Valve = entity;
                entity.BlowOffInspections.Add(inspectCopy);
            }

            return entity;
        }

        #endregion

        public ReplaceValve(IContainer container, IViewModelFactory viewModelFactory, AssetStatusNumberDuplicationValidator numberValidator) : base(container, numberValidator)
        {
            _viewModelFactory = viewModelFactory;
        }
    }

    public class SearchOperatingCenterValveNumber
    {
        #region Properties

        // These properties are named weirdly because they would otherwise
        // conflict with the properties/fields on BaseValveImageViewModel
        // when they're both on the same view. OperatingCenter messes up
        // the Town cascades and ValveNumber messes up autofilling in 
        // the ValveNumber textbox.
        [Required]
        public int? OperatingCenterIdentifier { get; set; }

        [Required]
        public string ValveNumberSearch { get; set; }

        #endregion
    }

    public class SearchValveBPUReport : SearchSet<ValveBPUReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        #endregion
    }

    public class SearchMunicipalValveZoneReport : SearchSet<MunicipalValveZoneReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "town", "Id")]
        public int? Town { get; set; }

        #endregion
    }

    public class SearchValvesDueInspectionReport : SearchSet<ValveDueInspectionReportItem>
    {
        #region Properties

        [MultiSelect]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int[] OperatingCenter { get; set; }

        #endregion
    }

    public class SearchValveRouteReport : SearchSet<ValveRouteReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "town", "Id")]
        public int? Town { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetStatus))]
        public int? Status { get; set; }

        #endregion
    }
}