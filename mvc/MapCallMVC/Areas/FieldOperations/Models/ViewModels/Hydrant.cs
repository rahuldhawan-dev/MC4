using DataAnnotationsExtensions;
using MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
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
using System.Text.RegularExpressions;
using System.Web.Mvc;
using HydrantBillings = MapCall.Common.Model.Entities.HydrantBilling.Indices;
using HydrantStatuses = MapCall.Common.Model.Entities.AssetStatus.Indices;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class HydrantViewModel : ViewModel<Hydrant>
    {
        #region Constants

        private const string ERROR_CRITICAL_NOTES_MUST_BE_NULL = "Critical checkbox must be checked when setting critical notes.",
                             YEAR_MANUFACTURED_ERROR_MESSAGE = "Year Manufactured should not be greater than the current year, and not before 1850.";

        #endregion

        #region Properties

        [DropDown("FieldOperations", "Valve", "ByTownIdAndOperatingCenterId", DependsOn = "Town, OperatingCenter", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(Valve))]
        public int? LateralValve { get; set; }

        [RequiredWhen(nameof(HydrantBilling), nameof(GetPublicHydrantBillingId), typeof(HydrantViewModel), ErrorMessage = "Fire District is required for public hydrants.")]
        [DropDown("", "FireDistrict", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(FireDistrict))]
        public virtual int? FireDistrict { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HydrantTagStatus))]
        public int? HydrantTagStatus { get; set; }

        // NOTE: Can't be required because apparently they enter hydrants in before they install them and
        //       they don't know the manufacturer until they install the hydrant for some reason.
        [DropDown, EntityMap, EntityMustExist(typeof(HydrantManufacturer))]
        public int? HydrantManufacturer { get; set; }

        // TODO: Cascade off HydrantManufacturer
        [DropDown("FieldOperations", "HydrantModel", "ByManufacturerId", DependsOn = "HydrantManufacturer")]
        [EntityMap, EntityMustExist(typeof(HydrantModel))]
        public int? HydrantModel { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(AssetStatus))]
        public int? Status { get; set; }

        [DisplayName(Hydrant.Display.LATERAL_SIZE)]
        [DropDown, EntityMap, EntityMustExist(typeof(LateralSize))]
        public int? LateralSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HydrantDirection))]
        public int? OpenDirection { get; set; }

        [DropDown("", "Gradient", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above."), EntityMap, EntityMustExist(typeof(Gradient))]
        public int? Gradient { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HydrantSize))]
        public int? HydrantSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? InspectionFrequencyUnit { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? PaintingFrequencyUnit { get; set; }

        [DisplayName(Hydrant.Display.HYDRANT_MAIN_SIZE)]
        [DropDown, EntityMap, EntityMustExist(typeof(HydrantMainSize))]
        [RequiredWhen(nameof(Status), ComparisonType.EqualToAny, nameof(GetActiveInstalledHydrantStatusId), typeof(HydrantViewModel), ErrorMessage = "Main Size is required for active / installed hydrants.")]
        public int? HydrantMainSize { get; set; }

        [DisplayName(Hydrant.Display.THREAD)]
        [DropDown, EntityMap, EntityMustExist(typeof(HydrantThreadType))]
        public int? HydrantThreadType { get; set; }

        [EntityMap, EntityMustExist(typeof(WaterSystem))]
        [DropDown("Admin", "WaterSystem", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public virtual int? WaterSystem { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town from above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public virtual int? TownSection { get; set; }

        [DropDown, EntityMap]
        [Required, EntityMustExist(typeof(HydrantBilling))]
        public virtual int? HydrantBilling { get; set; }

        [Coordinate(AddressCallback = "Hydrants.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMap]
        [RequiredWhen(nameof(Status), nameof(GetActiveHydrantStatusId), typeof(HydrantViewModel), ErrorMessage = "Coordinate is required for active hydrants.")]
        public virtual int? Coordinate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainType))]
        public int? MainType { get; set; }

        [EntityMap, EntityMustExist(typeof(Facility))]
        [DropDown("", "Facility", "ByTownId", DependsOn = "Town")]
        //[RequiredWhen("HydrantBilling", ComparisonType.EqualTo, MapCall.Common.Model.Entities.HydrantBilling.Indices.COMPANY)]
        public int? Facility { get; set; }

        [DisplayName(Hydrant.Display.IS_NON_BPUKPI)]
        [Required]
        public virtual bool IsNonBPUKPI { get; set; }

        [CheckBox] // This isn't a nullable bool so it can be a checkbox
        public bool? Critical { get; set; }

        [ClientCallback("Hydrants.validateCriticalNotes", ErrorMessage = ERROR_CRITICAL_NOTES_MUST_BE_NULL)]
        [Multiline, RequiredWhen(nameof(Critical), true), StringLength(Hydrant.StringLengths.CRITICAL_NOTES)]
        public string CriticalNotes { get; set; }

        [RequiredWhen(nameof(Status), nameof(GetActiveHydrantStatusId), typeof(HydrantViewModel), ErrorMessage = "DateInstalled is required for active hydrants.")]
        public DateTime? DateInstalled { get; set; }

        [RequiredWhen(nameof(Status), ComparisonType.EqualToAny, nameof(GetDateRetiredRequiredStatusIds), typeof(HydrantViewModel), ErrorMessage = "DateRetired is required for retired / removed hydrants.")]
        public DateTime? DateRetired { get; set; }

        [Required]
        public bool IsDeadEndMain { get; set; }

        public decimal? Elevation { get; set; }

        public int? InspectionFrequency { get; set; }

        public int? PaintingFrequency { get; set; }

        [StringLength(Hydrant.StringLengths.GISUID, MinimumLength = 18)]
        public virtual string GISUID { get; set; }

        [StringLength(Hydrant.StringLengths.LOCATION)]
        public string Location { get; set; }

        [StringLength(Hydrant.StringLengths.MAP_PAGE)]
        public string MapPage { get; set; }

        public int? Route { get; set; }
        public decimal? Stop { get; set; }

        [StringLength(Hydrant.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [StringLength(Hydrant.StringLengths.VALVE_LOCATION)]
        public string ValveLocation { get; set; }

        [ClientCallback("Hydrants.validateYearManufactured", ErrorMessage = YEAR_MANUFACTURED_ERROR_MESSAGE)]
        public int? YearManufactured { get; set; }
        public bool? ClowTagged { get; set; }

        public DateTime? BillingDate { get; set; }
        public int? BranchLengthFeet { get; set; }
        public int? BranchLengthInches { get; set; }
        public int? DepthBuryFeet { get; set; }
        public int? DepthBuryInches { get; set; }

        // DropDown is done via overrides
        [ClientCallback("Hydrants.validateFunctionalLocation", ErrorMessage = "The Functional Location field is required.")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public virtual int? FunctionalLocation { get; set; }

        public int? Zone { get; set; }

        public int? PaintingZone { get; set; }

        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller. It smells yes. -Ross 2/3/2015")]
        public bool SendNotificationOnSave { get; set; }

        [DoesNotAutoMap]
        public bool SendToSAP { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(HydrantType))]
        public int? HydrantType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HydrantOutletConfiguration))]
        public int? HydrantOutletConfiguration { get; set; }

        #endregion

        #region Constructors

        public HydrantViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateBillingDate()
        {
            // TODO: Make sure there's client-side validation for this too so things work nicely.

            if (Status == HydrantStatuses.ACTIVE && HydrantBilling == HydrantBillings.PUBLIC && !BillingDate.HasValue)
            {
                yield return new ValidationResult("BillingDate is required for public and active hydrants.", new[] { "BillingDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateCriticalNotes()
        {
            if (!Critical.GetValueOrDefault() && !string.IsNullOrWhiteSpace(CriticalNotes))
            {
                // I'd considered just nulling out CriticalNotes instead of validating it, but the user may
                // have forgotten to check the box.
                yield return new ValidationResult(ERROR_CRITICAL_NOTES_MUST_BE_NULL, new[] { "CriticalNotes" });
            }
        }

        private IEnumerable<ValidationResult> ValidateYearManufactured()
        {
            if (YearManufactured.HasValue && (1850 > YearManufactured.Value || YearManufactured.Value > DateTime.Now.Year))
            {
                yield return new ValidationResult(YEAR_MANUFACTURED_ERROR_MESSAGE, new[] { "YearManufactured" });
            }
        }

        public static int[] GetDateRetiredRequiredStatusIds() => AssetStatus.RETIRED_STATUS_IDS;

        // Overridable for testing so it can be disabled.
        protected virtual void SetSendNotificationsOnSave(Hydrant entity)
        {
            // Set to false by default so we only have to worry about true scenarios.
            SendNotificationOnSave = false;

            var newHydrantStatus = Status.GetValueOrDefault();
            var existingHydrantStatus = entity.Status;

            // Hydrants must be public for notifications to be sent.
            if (HydrantBilling.GetValueOrDefault() == HydrantBillings.PUBLIC)
            {
                // New hydrants send notifications out for retired and active since there was no previous value.
                if (Id == 0)
                {
                    if (newHydrantStatus == HydrantStatuses.RETIRED || newHydrantStatus == HydrantStatuses.ACTIVE)
                        SendNotificationOnSave = true;
                }
                else
                {
                    // Send notifications if the current hydrant status is active and is changed from active
                    if (existingHydrantStatus != null && existingHydrantStatus.Id == HydrantStatuses.ACTIVE && newHydrantStatus != HydrantStatuses.ACTIVE)
                        SendNotificationOnSave = true;
                    // Send notification if the current hydrant status is not active and is changed to active
                    if ((existingHydrantStatus == null || existingHydrantStatus.Id != HydrantStatuses.ACTIVE) && newHydrantStatus == HydrantStatuses.ACTIVE)
                        SendNotificationOnSave = true;
                }
            }

            // Bug 2432: A notification needs to be sent out when any non-adminonly status gets set.
            var notificationStatuses = _container.GetInstance<IRepository<AssetStatus>>().GetAll().Where(x => !x.IsUserAdminOnly).Select(x => x.Id).ToArray();
            var entityHydStatus = existingHydrantStatus != null ? existingHydrantStatus.Id : 0;
            if (notificationStatuses.Contains(newHydrantStatus) && newHydrantStatus != entityHydStatus)
            {
                SendNotificationOnSave = true;
            }
        }

        #endregion

        #region Public Methods

        // these three need to remain even though there are no references to them
        public static int GetActiveHydrantStatusId()
        {
            return HydrantStatuses.ACTIVE;
        }

        public static int[] GetActiveInstalledHydrantStatusId() => new[] {HydrantStatuses.ACTIVE,HydrantStatuses.INSTALLED};

        public static int GetPublicHydrantBillingId()
        {
            return HydrantBillings.PUBLIC;
        }

        public override Hydrant MapToEntity(Hydrant entity)
        {
            // This needs to happen before mapping so this can compare against
            // the previous hydrant values.
            var previousHydrantStatus = entity.Status;
            SetSendNotificationsOnSave(entity);

            // Only set values if BackInServiceDate is null!
            if (entity.CurrentOpenOutOfServiceRecord != null
                && entity.CurrentOpenOutOfServiceRecord.BackInServiceDate == null 
                && AssetStatus.RETIRED_STATUS_IDS.Contains(Status.Value))
            {
                entity.CurrentOpenOutOfServiceRecord.BackInServiceByUser =
                    _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
                entity.CurrentOpenOutOfServiceRecord.BackInServiceDate =
                    _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }
            base.MapToEntity(entity);

            SendToSAP = entity.OperatingCenter.CanSyncWithSAP;

            // if existing == Cancelled, Retired, or Removed and existing.Id == viewModel.HydrantStatusId then false
            if (previousHydrantStatus?.Id == Status)
            {
                if (previousHydrantStatus.Id == HydrantStatuses.CANCELLED ||
                    previousHydrantStatus.Id == HydrantStatuses.RETIRED ||
                    previousHydrantStatus.Id == HydrantStatuses.REMOVED)
                {
                    SendToSAP = false;
                }
            }

            if (entity.DateRetired.HasValue && !AssetStatus.RETIRED_STATUS_IDS.Contains(entity.Status.Id))
            {
                entity.DateRetired = null;
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateBillingDate()).Concat(ValidateCriticalNotes()).Concat(ValidateYearManufactured());
        }

        #endregion
    }

    public class CreateHydrantBase : HydrantViewModel
    {
        #region Private Members

        private readonly AssetStatusNumberDuplicationValidator _numberValidator;

        #endregion

        #region Properties

        // State, Town and OperatingCenter are not editable fields.

        [DoesNotAutoMap("Needed for cascades")]
        [DropDown, SearchAlias("Town", "State.Id")]
        public int? State { get; set; }

        [Required, DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State", PromptText = "Select a state above"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(Street))]
        public int? CrossStreet { get; set; }

        [EntityMap, EntityMustExist(typeof(WaterSystem))]
        [DropDown("Admin", "WaterSystem", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public override int? WaterSystem { get; set; }

        // This can't be nullable because an unchecked checkbox will come back as null instead of false.
        [DoesNotAutoMap]
        public bool IsFoundHydrant { get; set; }

        [Remote("ValidateUnusedFoundHydrantSuffix", "Hydrant", "FieldOperations", AdditionalFields = "OperatingCenter, Town, TownSection")]
        [RequiredWhen(nameof(IsFoundHydrant), true, ErrorMessage = "Hydrant Suffix is required for found hydrants.")]
        [Min(1, ErrorMessage = "HydrantSuffix must be greater than zero.")]
        public virtual int? HydrantSuffix { get; set; }

        [StringLength(Hydrant.StringLengths.LEGACY_ID)]
        public string LegacyId { get; set; }

        [DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Select a town from above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public override int? TownSection { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForHydrantAssetType", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? FunctionalLocation
        {
            get => base.FunctionalLocation;

            set => base.FunctionalLocation = value;
        }

        [StringLength(Hydrant.StringLengths.WORKORDER)]
        [Required]
        public string WorkOrderNumber { get; set; }

        #endregion

        #region Constructors

        public CreateHydrantBase(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) :
            base(container)
        {
            _numberValidator = numberValidator;
        }

        #endregion

        #region Public Methods

        // This is public so that the remote client validator can use the same logic.
        public virtual IEnumerable<ValidationResult> ValidateHydrantSuffixForFoundHydrants()
        {
            if (!IsFoundHydrant) { yield break; }

            var repo = _container.GetInstance<IRepository<Hydrant>>();
            var rawRepo = _container.GetInstance<RepositoryBase<Hydrant>>();
            var abbrRepo = _container.GetInstance<IAbbreviationTypeRepository>();
            var opc = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.GetValueOrDefault());
            var town = _container.GetInstance<IRepository<Town>>().Find(Town.GetValueOrDefault());
            var townSection = _container.GetInstance<IRepository<TownSection>>().Find(TownSection.GetValueOrDefault());

            // Operatingcenter and Town are required field, this is here for unit testing. In practice neither field will ever be null
            // by the time this method is called.
            if (town == null || opc == null) { yield break; }

            var maxHyd = repo.GetMaxHydrantNumber(rawRepo, abbrRepo, opc, town, townSection, null);
            if (HydrantSuffix > maxHyd.Suffix)
            {
                yield return
                    new ValidationResult(
                        "A found hydrant can not have a suffix greater than the current maximum hydrant number for a given area.",
                        new[] { "HydrantSuffix" });
            }
            else
            {
                // Need to change the suffix to get the proper formatting.
                maxHyd.Suffix = HydrantSuffix.GetValueOrDefault();
                var existing = repo.FindByOperatingCenterAndHydrantNumber(opc, maxHyd.FormattedNumber);
                if ((!Status.HasValue && existing.Any()) ||
                    (Status.HasValue && !_numberValidator.IsValid(Status.Value, existing)))
                {
                    yield return new ValidationResult("A hydrant already exists with this hydrant suffix.", new[] { "HydrantSuffix" });
                }
            }
        }

        public override Hydrant MapToEntity(Hydrant entity)
        {
            base.MapToEntity(entity);

            // Town is required for this to work. MapToEntity won't be called
            // if the model's invalid, but the null check's being done anyway
            // for the sake of unit testing.

            if (Town.HasValue)
            {
                var hydRepo = _container.GetInstance<IRepository<Hydrant>>();
                HydrantNumber hydNum;

                if (IsFoundHydrant)
                {
                    hydNum = new HydrantNumber {
                        Prefix = hydRepo.GenerateHydrantPrefix(entity.OperatingCenter, entity.Town,
                            entity.TownSection, null),
                        Suffix = HydrantSuffix.Value
                    };
                }
                else
                {
                    // Can use the entity refs here since they should have been set by base.MapToEntity already.
                    hydNum = hydRepo.GenerateNextHydrantNumber(
                        _container.GetInstance<IAbbreviationTypeRepository>(),
                        _container.GetInstance<RepositoryBase<Hydrant>>(),
                        entity.OperatingCenter, entity.Town, entity.TownSection, null);
                }

                // A hydrant number must be unique to the operating center. There isn't a defined behavior
                // for what to do if the generated hydrant number ends up not being unique. Throwing an exception
                // here so if it ever comes up we'll know.
                if (hydRepo.FindByOperatingCenterAndHydrantNumber(entity.OperatingCenter, hydNum.FormattedNumber).Any())
                {
                    throw ExceptionHelper.Format<InvalidOperationException>("The generated hydrant number '{0}' is not unique to the operating center '{1}'", hydNum.FormattedNumber, entity.OperatingCenter);
                }

                entity.HydrantNumber = hydNum.FormattedNumber;
                entity.HydrantSuffix = hydNum.Suffix;
            }

            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            return entity;
        }

        #endregion
    }

    public class CreateHydrant : CreateHydrantBase
    {
        #region Constructors

        public CreateHydrant(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) :
            base(container, numberValidator) { }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateHydrantSuffixForFoundHydrants()).Concat(ValidateSAPFunctionalLocation());
        }

        public IEnumerable<ValidationResult> ValidateSAPFunctionalLocation()
        {
            if (!FunctionalLocation.HasValue)
            {
                var opc = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.Value);
                if (!opc.IsContractedOperations && opc.SAPEnabled)
                {
                    yield return new ValidationResult("The Functional Location field is required.", new[] { "FunctionalLocation" });
                }
            }
        }

        public override Hydrant MapToEntity(Hydrant entity)
        {
            base.MapToEntity(entity);

            SetInspectionFrequency(entity);

            return entity;
        }

        protected virtual void SetInspectionFrequency(Hydrant entity)
        {
            if (OperatingCenter.HasValue)
            {
                // This is not editable during creation, it's supposed to be auto-populated by op center.
                // Defaults to 1 Year if the OperatingCenter does not have anything set.
                entity.InspectionFrequency = entity.OperatingCenter.HydrantInspectionFrequency;
                entity.InspectionFrequencyUnit = entity.OperatingCenter.HydrantInspectionFrequencyUnit;
            }
        }

        #endregion
    }

    public class EditHydrant : HydrantViewModel
    {
        #region Constants

        public const string ERROR_HYDRANT_NUMBER_ALREADY_USED = "A hydrant already exists for this operating center with the given hydrant number.";

        private const string ERROR_WBS_NUMBER_REQUIRED =
            "Work Order Number is required for retired, removed and active hydrants.";
        #endregion

        #region Fields

        private readonly AssetStatusNumberDuplicationValidator _numberValidator;

        #endregion

        #region Properties
        //here for cascading purposes
        [Secured(AppliesToAdmins = true), EntityMap(MapDirections.ToViewModel)]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(Street))]
        public int? CrossStreet { get; set; }

        [Required, StringLength(Hydrant.StringLengths.HYDRANT_NUMBER), RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public string HydrantNumber { get; set; }

        [Required, RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public int? HydrantSuffix { get; set; }

        [StringLength(Hydrant.StringLengths.LEGACY_ID)]
        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public string LegacyId { get; set; }

        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? FireDistrict { get; set; }

        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false), Required]
        public override bool IsNonBPUKPI { get; set; }

        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false), Required]
        public override int? HydrantBilling { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForHydrantAssetType", DependsOn = "Town", PromptText = "Select a town above")]
        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? FunctionalLocation { get; set; }

        // Bug 2589: Only user admins can edit coordinate.
        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? Coordinate
        {
            get
            {
                return base.Coordinate;
            }
            set
            {
                base.Coordinate = value;
            }
        }

        public int? SAPEquipmentId { get; set; }

        [StringLength(Hydrant.StringLengths.WORKORDER)]
        [ClientCallback("Hydrants.validateWorkOrderNumber", ErrorMessage = ERROR_WBS_NUMBER_REQUIRED)]
        public string WorkOrderNumber { get; set; }

        #endregion

        #region Constructors

        public EditHydrant(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) : base(container)
        {
            _numberValidator = numberValidator;
        }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateHydrantNumber()
        {
            // Users are allowed to edit a hydrant's HydrantNumber, but they are not allowed to set
            // it to an existing hydrant number if it conflicts with being unique to active hydrants in the operating center.
            var hydRepo = _container.GetInstance<IRepository<Hydrant>>();
            // Get existing record
            var entity = hydRepo.Find(Id);
            if (entity == null)
            {
                // Site will 404 before this method ever hits. Just here for unit testing.
                yield break;
            }

            // OperatingCenter is not editable so we can just use the one on the existing entity.
            var matches = hydRepo.FindByOperatingCenterAndHydrantNumber(entity.OperatingCenter, HydrantNumber).Where(h => h.Id != Id);

            if (!matches.Any())
            {
                // This implies that the hydrant number has been changed to an unused number. Valid.
                yield break;
            }

            if ((!Status.HasValue && matches.Any()) ||
                (Status.HasValue && !_numberValidator.IsValid(Status.Value, matches)))
            {
                yield return new ValidationResult(ERROR_HYDRANT_NUMBER_ALREADY_USED, new[] { "HydrantNumber" });
            }
        }

        private IEnumerable<ValidationResult> ValidateHydrantSuffixAndNumberMatch()
        {
            var regex = new Regex(Hydrant.HYDRANT_NUMBER_PATTERN, RegexOptions.IgnoreCase);
            var match = regex.Match(HydrantNumber);

            if (match.Groups.Count != 4 || match.Groups[3].ToString() != HydrantSuffix.ToString())
                yield return new ValidationResult(Hydrant.HYDRANT_NUMBER_PATTERN_ERROR, new[] {"HydrantNumber"});
        }

        private IEnumerable<ValidationResult> ValidateFunctionalLocation()
        {
            if (!FunctionalLocation.HasValue && !DisplayHydrant.OperatingCenter.IsContractedOperations && DisplayHydrant.OperatingCenter.SAPEnabled)
            {
                 yield return new ValidationResult("The Functional Location field is required.", new[] { "FunctionalLocation" });
            }
        }

        private IEnumerable<ValidationResult> ValidateWorkOrderNumber()
        {
            var hydrant = _container.GetInstance<IHydrantRepository>().Find(Id);

            if (Status.HasValue && hydrant.Status.Id != Status.Value && AssetStatus.WBS_STATUS_IDS.Contains(Status.Value) && WorkOrderNumber == null)
            {
                yield return new ValidationResult(ERROR_WBS_NUMBER_REQUIRED, new[] { "WorkOrderNumber" });
            }
        }

        #endregion

        #region Exposed Methods

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                            .Concat(ValidateHydrantNumber())
                            .Concat(ValidateHydrantSuffixAndNumberMatch())
                            .Concat(ValidateFunctionalLocation()
                            .Concat(ValidateWorkOrderNumber()));
        }

        // MapToEntity -
        // public virtual bool SendToSAP => OperatingCenter.SAPEnabled && !OperatingCenter.IsContractedOperations;

        public override Hydrant MapToEntity(Hydrant entity)
        {
            this.MaybeCancelWorkOrders(entity, _container.GetInstance<IRepository<WorkOrderCancellationReason>>(),
                t => t.DateRetired);

            return base.MapToEntity(entity);
        }

        #endregion

        #endregion

        #region Display only properties

        // There are a lot of properties that aren't editable by some users so this easier
        // than making a hundred display properties.
        private Hydrant _displayHydrant;

        [DoesNotAutoMap]
        public Hydrant DisplayHydrant
        {
            get
            {
                if (_displayHydrant == null)
                {
                    _displayHydrant = _container.GetInstance<IRepository<Hydrant>>().Find(Id);
                }
                return _displayHydrant;
            }
        }

        [AutoMap(MapDirections.None)] // Needed for cascades to work
        public int Town
        {
            get { return DisplayHydrant.Town.Id; }
        }

        #endregion
    }

    public class CopyHydrant : CreateHydrant
    {
        #region Properties

        [Required, StringLength(Hydrant.StringLengths.HYDRANT_NUMBER),
         RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public string HydrantNumber { get; set; }

        [Required, RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? HydrantSuffix { get; set; }

        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? FireDistrict { get; set; }

        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false), Required]
        public override bool IsNonBPUKPI { get; set; }

        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false), Required]
        public override int? HydrantBilling { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForHydrantAssetType", DependsOn = "Town",
            PromptText = "Select a town above")]
        [RoleSecured(RoleModules.FieldServicesAssets, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public override int? FunctionalLocation { get; set; }

        #endregion

        #region Constructors

        public CopyHydrant(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) : base(container, numberValidator) { }

        #endregion

        #region Private Methods

        protected override void SetInspectionFrequency(Hydrant entity)
        {
            if (!InspectionFrequency.HasValue)
            {
                base.SetInspectionFrequency(entity);
            }
        }

        #endregion

        #region Exposed Methods

        public override void Map(Hydrant entity)
        {
            base.Map(entity);
            HydrantNumber = null;
            Coordinate = null;
            DateInstalled = null;
        }

        public override Hydrant MapToEntity(Hydrant entity)
        {
            entity = base.MapToEntity(entity);
            if (Status == HydrantStatuses.ACTIVE)
            {
                SendNotificationOnSave = true;
            }
            entity.BillingDate = null;
            entity.DateInstalled = null;
            entity.Status = new AssetStatus {Id = HydrantStatuses.PENDING};
            return entity;
        }

        #endregion
    }

    public class ReplaceHydrant : EditHydrant
    {
        #region Private Members

        private Hydrant _retiredHydrant;
        private IViewModelFactory _viewModelFactory;

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public override int? OperatingCenter { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public new int? Town { get; set; }

        #endregion

        #region Constructors

        public ReplaceHydrant(IContainer container, IViewModelFactory viewModelFactory, AssetStatusNumberDuplicationValidator numberValidator) : base(container, numberValidator)
        {
            _viewModelFactory = viewModelFactory;
        }

        #endregion

        #region Exposed Methods

        public override void Map(Hydrant entity)
        {
            base.Map(entity);
            OperatingCenter = entity.OperatingCenter.Id;
            Town = entity.Town.Id;
            _retiredHydrant = entity;
            DateInstalled = null;
        }

        public override Hydrant MapToEntity(Hydrant entity)
        {
            // NOTE: The entity being passed to this is NOT an existing hydrant record!
            base.MapToEntity(entity);

            // Do not populate the SAP Equipment ID when creating the new pending asset record.
            // This way when the new pending record is saved, it will automatically create a new record in SAP.
            entity.SAPEquipmentId = null;

            entity.Initiator = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            // All replacement hydrants must be set to pending initially.
            entity.Status = _container.GetInstance<IRepository<AssetStatus>>().Find(HydrantStatuses.PENDING);

            // Copy the coordinate to a new coordinate record.
            var rc = _retiredHydrant.Coordinate;
            if (rc != null)
            {
                entity.Coordinate = new Coordinate {
                    // Not all Coordinate records have an icon set, but it's mapped as not nullable so set it to a default.
                    Icon = rc.Icon ?? _container.GetInstance<IRepository<IconSet>>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon,
                    Latitude = rc.Latitude,
                    Longitude = rc.Longitude
                };
            }

            // This is a new hydrant record so there shouldn't be any inspections yet.
            entity.HydrantInspections.Clear();

            foreach (var inspection in _retiredHydrant.HydrantInspections)
            {
                // NOTE: Important to use the EDIT view model as the create view model will overwrite data.
                // NOTE 2: We're not supposed to copy inspections over to SAP. http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c9
                var model = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>(inspection);
                model.Hydrant = null;
                var inspectCopy = new HydrantInspection();
                model.MapToEntity(inspectCopy);
                inspectCopy.Hydrant = entity;
                entity.HydrantInspections.Add(inspectCopy);
            }

            return entity;
        }

        #endregion
    }

    public class MarkOutOfServiceHydrant : ViewModel<Hydrant>
    {
        #region Properties

        [DoesNotAutoMap("Manually set on HydrantOutOfService record")]
        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? OutOfServiceDate { get; set; }

        #endregion

        #region Constructors

        public MarkOutOfServiceHydrant(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateOutOfServiceDate()
        {
            var hydrant = _container.GetInstance<IRepository<Hydrant>>().Find(Id);
            if (hydrant.OutOfService)
            {
                yield return
                    new ValidationResult("This hydrant is already out of service.",
                        new[] { "OutOfServiceDate" });
            }
        }

        #endregion

        #region Public Methods

        public override Hydrant MapToEntity(Hydrant entity)
        {
            //base.MapToEntity(entity);

            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            var oos = new HydrantOutOfService();
            oos.Hydrant = entity;
            oos.OutOfServiceDate = OutOfServiceDate.Value;
            oos.OutOfServiceByUser = currentUser;

            var fd = entity.FireDistrict;
            if (fd != null)
            {
                oos.FireDepartmentContact = fd.Contact;
                oos.FireDepartmentFax = fd.Fax;
                oos.FireDepartmentPhone = fd.Phone;
            }

            entity.OutOfServiceRecords.Add(oos);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateOutOfServiceDate());
        }

        #endregion
    }

    public class MarkBackInServiceHydrant : ViewModel<Hydrant>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually set on CurrentOpenOutOfServiceRecord")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? BackInServiceDate { get; set; }

        #endregion

        #region Constructors

        public MarkBackInServiceHydrant(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateBackInServiceDate()
        {
            var hydrant = _container.GetInstance<IRepository<Hydrant>>().Find(Id);
            if (!hydrant.OutOfService)
            {
                yield return
                    new ValidationResult("This hydrant is not currently out of service.",
                        new[] { "BackInServiceDate" });
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateBackInServiceDate());
        }

        public override Hydrant MapToEntity(Hydrant entity)
        {
           // base.MapToEntity(entity);

            var oos = entity.CurrentOpenOutOfServiceRecord;
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            // If not out of service, close existing HOOS record if one exists.
            // Only set values if BackInServiceDate is null!
            if (oos != null && !oos.BackInServiceDate.HasValue)
            {
                // BackInServiceDate should not be null and validation should catch it. Let this throw otherwise.
                oos.BackInServiceDate = BackInServiceDate.Value;
                oos.BackInServiceByUser = currentUser;
            }

            return entity;
        }

        #endregion
    }

    public class SearchActiveHydrantReport : SearchSet<ActiveHydrantReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        #endregion
    }

    public class SearchActiveHydrantDetailReport : SearchSet<ActiveHydrantDetailReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "town", "Id")]
        public int? Town { get; set; }

        [DropDown]
        [SearchAlias("LateralSize", "latSize", "Id")]
        public int? LateralSize { get; set; }

        [DropDown]
        [SearchAlias("HydrantSize", "hydSize", "Id")]
        public int? HydrantSize { get; set; }

        #endregion
    }

    public class SearchHydrantsDueInspectionReport : SearchSet<HydrantDueInspectionReportItem>
    {
        #region Properties

        [MultiSelect]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int[] OperatingCenter { get; set; }

        #endregion
    }

    public class SearchHydrantsDuePaintingReport : SearchSet<HydrantDuePaintingReportItem>
    {
        #region Properties

        [MultiSelect]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int[] OperatingCenter { get; set; }

        #endregion
    }

    public class SearchPublicHydrantCountReport : SearchSet<PublicHydrantCountReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        #endregion
    }

    public class SearchHydrantRouteReport : SearchSet<HydrantRouteReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "town", "Id")]
        public int? Town { get; set; }

        [DropDown]
        public int? Status { get; set; }

        #endregion
    }

    public class SearchAgedPendingAsset : SearchSet<AgedPendingAssetReportItem>
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        #endregion
    }

    public class SearchHydrantWorkOrdersByDescription : SearchSet<HydrantWorkOrdersByDescriptionReportItem>, ISearchHydrantWorkOrdersByDescriptionReportItem
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown]
        public int? HydrantManufacturer { get; set; }

        public IntRange YearManufactured { get; set; }
        public bool? HasWorkOrder { get; set; }

        [MultiSelect, Search(CanMap = false)]
        public int[] HasWorkOrderWithWorkDescriptions { get; set; }

        [DropDown("", "FireDistrict", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? FireDistrict { get; set; }

        [DropDown]
        public int? Status { get; set; }

        [DropDown]
        public int? HydrantBilling { get; set; }

        #endregion
    }
}
