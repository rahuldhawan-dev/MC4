using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels.SystemDeliveryEntries;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SystemDeliveryEntryViewModel : ViewModel<SystemDeliveryEntry>
    {
        #region Properties

        [View(FormatStyle.Date), Required]
        public DateTime? WeekOf { get; set; } // Should be empty initially but required and only allow Mondays, validated in validation method
        
        [MultiSelect, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenters { get; set; }
        
        [MultiSelect("",
             "Facility",
             "GetActiveByOperatingCentersWithPointOfEntryAndSystemDeliveryType",
             DependsOn = (nameof(OperatingCenters) + "," + nameof(SystemDeliveryType))), 
         EntityMap,
         EntityMustExist(typeof(Facility)), 
         Required]
        public virtual int[] Facilities { get; set; }
        
        public bool? IsValidated { get; set; }
        
        public int? EnteredBy { get; set; }
        
        [DropDown, EntityMap, Required, EntityMustExist(typeof(SystemDeliveryType))]
        public int? SystemDeliveryType { get; set; }
        
        [DoesNotAutoMap]
        public List<DateTime> DatesForWeek =>
            WeekOf != null
                ? new List<DateTime> {
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Monday),
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Tuesday),
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Wednesday),
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Thursday),
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Friday),
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Saturday),
                    WeekOf.Value.GetDayFromWeek(DayOfWeek.Saturday).AddDays(1)
                }
                : null;
        
        #endregion

        #region Constructor

        public SystemDeliveryEntryViewModel(IContainer container) : base(container) { }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// Building up the System Delivery Facility Entries so we can save them when initially saving its parent
        /// System Delivery Entry. This used to be executed only when editing which allowed "ghost entries:" entries
        /// that have header information (System Delivery Entries), but no detail (System Delivery Facility Entries).
        /// </summary>
        /// <param name="entity">The System Delivery Entry we're building System Delivery Facility Entries for</param>
        protected void BuildFacilityEntries(SystemDeliveryEntry entity)
        {
            var facilities = _container.GetInstance<IRepository<Facility>>().FindManyByIds(Facilities);
            var defaultEntryValue = 0.00M;
            var entryDates = GetEntryDates().ToArray();

            foreach (var facility in facilities.Values)
            {
                var facilitySystemDeliveryEntryTypes = 
                    facility.FacilitySystemDeliveryEntryTypes
                            .Where(x => x.IsEnabled)
                            .OrderBy(x => x.SystemDeliveryEntryType.Description);

                foreach (var facilitySystemDeliveryEntryType in facilitySystemDeliveryEntryTypes)
                {
                    var systemDeliveryEntryTypeId =
                        facilitySystemDeliveryEntryType.SystemDeliveryEntryType?.Id ?? 0;
                    var supplierFacility = GetSupplierFacility(facility, systemDeliveryEntryTypeId);
                    
                    foreach (var date in entryDates)
                    {
                        entity.FacilityEntries.Add(new SystemDeliveryFacilityEntry {
                            SystemDeliveryType = entity.SystemDeliveryType,
                            SystemDeliveryEntry = entity,
                            SystemDeliveryEntryType = facilitySystemDeliveryEntryType.SystemDeliveryEntryType,
                            EnteredBy = entity.EnteredBy,
                            Facility = facility,
                            SupplierFacility = supplierFacility,
                            EntryDate = date,
                            EntryValue = defaultEntryValue
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Returns a collection of datetimes relevant to the number of existing entries with the same
        /// WeekOf date, Facility & Operating Center. If zero existing entries exist, we'll build up
        /// a collection of datetimes whose dates correspond with the current month, otherwise,  dates
        /// will correspond with next month. An existing entry tells us that we're dealing with a split-week
        /// </summary>
        /// <returns>a collection of DateTime</returns>
        private IEnumerable<DateTime> GetEntryDates()
        {
            // How many entries exist with a matching WeekOf, Facility, and Operating Center?
            // Count() should return either 0 or 1 - Validation will not allow more than 1
            var existingEntries = _container.GetInstance<IRepository<SystemDeliveryEntry>>().Where(x =>
                x.WeekOf == WeekOf && x.Facilities.Any(y => Facilities.Contains(y.Id)) &&
                x.OperatingCenters.Any(y => OperatingCenters.Contains(y.Id))).Count();
            
            return WeekOf != null && (existingEntries == 0 || existingEntries == 1 && WeekOf.Value.Month == WeekOf.Value.AddDays(6).Month)
                ? DatesForWeek.Where(x => WeekOf != null && x.Month == WeekOf.Value.Month)
                : DatesForWeek.Where(x => WeekOf != null && x.Month == WeekOf.Value.AddMonths(1).Month);
        }
        
        private static Facility GetSupplierFacility(Facility facility, int entryType)
        {
            if (entryType == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
            {
                return facility.FacilitySystemDeliveryEntryTypes.Any(t => t.SystemDeliveryEntryType.Id == entryType && t.IsEnabled)
                    ? facility.FacilitySystemDeliveryEntryTypes.SingleOrDefault(t => t.SystemDeliveryEntryType.Id == entryType && t.IsEnabled)?.SupplierFacility
                    : null;
            }

            return null;
        }
        
        #endregion

        #region Public methods

        public bool IsSplitWeek(DateTime date)
        {
            return date.Month != date.AddDays(6).Month;
        }

        #endregion
    }

    public class CreateSystemDeliveryEntryViewModel : SystemDeliveryEntryViewModel
    {
        #region Properties

        [MultiSelect("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenters"), EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int[] PublicWaterSupplies { get; set; }

        [MultiSelect("Environmental", "WasteWaterSystem", "ByOperatingCenters", DependsOn = "OperatingCenters"), EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        public int[] WasteWaterSystems { get; set; }

        [MultiSelect("",
             "Facility",
             "GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply",
             DependsOn = nameof(OperatingCenters) + "," + nameof(SystemDeliveryType) + "," + nameof(PublicWaterSupplies),
             DependentsRequired = DependentRequirement.One),
         EntityMap,
         EntityMustExist(typeof(Facility)),
         Required]
        public override int[] Facilities { get; set; }

        #endregion

        #region Constructor

        public CreateSystemDeliveryEntryViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateDateEnteredIsAMondayAndEntryDoesNotAlreadyExist()
        {
            var isSplit = WeekOf != null && IsSplitWeek(WeekOf.Value);

            if (DateIsNotMondayOrIsInFuture())
            {
                yield return new ValidationResult("Week Of Date must be a Monday and must not be in the future.");
            }

            if (EntryAlreadyExists() && !isSplit)
            {
                yield return
                    new ValidationResult("Entry already exists for this week for operating center / facility.");
            }

            if (isSplit && GetEntryCount() >= 2)
            {
                yield return new ValidationResult(
                    "Two entries already exist for this week for operating center / facility.");
            }
        }

        private bool DateIsNotMondayOrIsInFuture() =>
            WeekOf != null && (WeekOf.Value.DayOfWeek != DayOfWeek.Monday ||
                               WeekOf.Value.Date >
                               _container.GetInstance<IDateTimeProvider>()
                                         .GetCurrentDate()
                                         .Date);

        private bool EntryAlreadyExists()
        {
            return _container.GetInstance<IRepository<SystemDeliveryEntry>>().Any(x => x.WeekOf == WeekOf
                && x.Facilities.Any(y => Facilities.Contains(y.Id))
                && x.OperatingCenters.Any(y => OperatingCenters.Contains(y.Id)));
        }

        private int GetEntryCount()
        {
            return _container.GetInstance<IRepository<SystemDeliveryEntry>>().Where(x => x.WeekOf == WeekOf
                && x.Facilities.Any(y => Facilities.Contains(y.Id))
                && x.OperatingCenters.Any(y => OperatingCenters.Contains(y.Id))).Count();
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            base.Validate(validationContext)
                .Concat(ValidateDateEnteredIsAMondayAndEntryDoesNotAlreadyExist());

        public override SystemDeliveryEntry MapToEntity(SystemDeliveryEntry entity)
        {
            base.MapToEntity(entity);
            entity.EnteredBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser?.Employee;
            BuildFacilityEntries(entity);
            return entity;
        }

        #endregion
    }

    public class EditSystemDeliveryEntryViewModel : SystemDeliveryEntryViewModel
    {
        #region Fields

        private SystemDeliveryEntry _original;
        private Dictionary<int, SystemDeliveryEntryType> _systemDeliveryEntryTypes;

        #endregion

        #region Properties

        [MultiSelect("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenters"), EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int[] PublicWaterSupplies { get; set; }

        [MultiSelect("Environmental", "WasteWaterSystem", "ByOperatingCenters", DependsOn = "OperatingCenters"), EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        public int[] WasteWaterSystems { get; set; }

        [DoesNotAutoMap("Need the original entity to get things")]
        public new SystemDeliveryEntry Original
        {
            // need to override this so we can grab it from the repository
            // if we don't have it
            get => _original ?? (_original = _container.GetInstance<IRepository<SystemDeliveryEntry>>().Find(Id));
            protected set => _original = value;
        }

        [DoesNotAutoMap("FacilityEntries")]
        public List<CreateSystemDeliveryFacilityEntry> FacilityEntries { get; set; }

        [DoesNotAutoMap]
        public Dictionary<int, SystemDeliveryEntryType> SystemDeliveryEntryTypes =>
            _systemDeliveryEntryTypes ??
            (_systemDeliveryEntryTypes = _container.GetInstance<IRepository<SystemDeliveryEntryType>>()
                                                   .GetAll()
                                                   .ToDictionary(x => x.Id));

        #endregion
        
        #region Constructor
        public EditSystemDeliveryEntryViewModel(IContainer container) : base(container)
        {
            FacilityEntries = new List<CreateSystemDeliveryFacilityEntry>();
        }
        
        #endregion

        #region Private Methods

        private void MapFacilityEntries(SystemDeliveryEntry entity)
        {
            var enteredBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
            var facilityRepo = _container.GetInstance<IFacilityRepository>();
            Facility supplierFacility = null;
            Facility facility = null;
            SystemDeliveryEntryType systemDeliveryEntryType = null;

            var entries = FacilityEntries.OrderBy(x => x.OperatingCenterDescription).ThenBy(x => x.FacilityName);

            foreach (var entry in entries)
            {
                // Multiple entries will have the same values or are nullable,
                // so let's limit the times we have to reach out and get things

                if (entry.SupplierFacility.HasValue)
                {
                    if (supplierFacility?.Id != entry.SupplierFacility.Value)
                    {
                        supplierFacility = facilityRepo.Find(entry.SupplierFacility.Value);
                    }
                }
                else
                {
                    supplierFacility = null;
                }

                if (facility == null || entry.FacilityId != facility.Id)
                {
                    facility = facilityRepo.Find(entry.FacilityId);
                }
                
                if (systemDeliveryEntryType == null || entry.SystemDeliveryEntryType != systemDeliveryEntryType.Id)
                {
                    SystemDeliveryEntryTypes.TryGetValue(entry.SystemDeliveryEntryType, out systemDeliveryEntryType);
                }

                entity.FacilityEntries.Add(new SystemDeliveryFacilityEntry {
                    SystemDeliveryType = entity.SystemDeliveryType,
                    SystemDeliveryEntry = entity,
                    SystemDeliveryEntryType = systemDeliveryEntryType,
                    EnteredBy = enteredBy,
                    Facility = facility,
                    SupplierFacility = supplierFacility,
                    EntryDate = entry.EntryDate,
                    EntryValue = entry.EntryValue ?? 0.00m,
                    WeeklyTotal = entry.WeeklyTotal,
                    IsInjection = entry.IsInjection
                });

                // If we're transferring water from one facility to another, we'll create a matching entry 
                // with negative values for the facility receiving the transfer
                if (entry.SystemDeliveryEntryType == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                {
                    MapTransferEntry(entry, entity, supplierFacility, enteredBy);
                }
            }
        }

        private void MapTransferEntry(SystemDeliveryFacilityEntryViewModel entry,
            SystemDeliveryEntry entity,
            Facility supplierFacility,
            Employee enteredBy)
        {
            var existingTransfer = entity.FacilityEntries.FirstOrDefault(e =>
                e.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM &&
                e.Facility == supplierFacility &&
                e.EntryDate == entry.EntryDate &&
                e.SystemDeliveryType.Id == MapCall.Common.Model.Entities.SystemDeliveryType.Indices.WATER);

            if (existingTransfer != null)
            {
                existingTransfer.EntryValue -= entry.EntryValue ?? 0.00m;
            }
            else
            {
                entity.FacilityEntries.Add(new SystemDeliveryFacilityEntry {
                    SystemDeliveryType = _container.GetInstance<IRepository<SystemDeliveryType>>()
                                                   .Find(entry.SystemDeliveryType),
                    SystemDeliveryEntry = entity,
                    SystemDeliveryEntryType = _container.GetInstance<IRepository<SystemDeliveryEntryType>>()
                                                        .Find(SystemDeliveryEntryType.Indices.TRANSFERRED_FROM),
                    EnteredBy = enteredBy,
                    Facility = supplierFacility,
                    EntryDate = entry.EntryDate,
                    EntryValue = -entry.EntryValue ?? 0.00m,
                    IsInjection = entry.IsInjection
                });   
            }
        }

        private new void BuildFacilityEntries(SystemDeliveryEntry entity)
        {
            var systemDeliveryFacilityEntries =
                entity.FacilityEntries
                      .Where(x => x.SystemDeliveryEntryType.Id !=
                                  SystemDeliveryEntryType.Indices.TRANSFERRED_FROM)
                      .OrderBy(x => x.Facility.OperatingCenter.Description)
                      .ThenBy(x => x.Facility.FacilityName);

            foreach (var entry in systemDeliveryFacilityEntries)
            {
                FacilityEntries.Add(new CreateSystemDeliveryFacilityEntry(_container) {
                    OperatingCenterId = entry.Facility.OperatingCenter.Id,
                    OperatingCenterDescription = entry.Facility.OperatingCenter.Description,
                    SystemDeliveryType = entry.SystemDeliveryType.Id,
                    SystemDeliveryEntry = entry.SystemDeliveryEntry.Id,
                    SystemDeliveryEntryType = entry.SystemDeliveryEntryType.Id,
                    SystemDeliveryEntryTypeDesc = entry.SystemDeliveryEntryType.Description,
                    EnteredBy = entry.EnteredBy.Id,
                    FacilityId = entry.Facility.Id,
                    FacilityName = entry.Facility.FacilityName,
                    FacilityIdWithFacilityName = entry.Facility.FacilityId,
                    SupplierFacility = entry.SupplierFacility?.Id,
                    SupplierFacilityDesc = entry.SupplierFacility?.Description,
                    PurchaseSupplier = entry.PurchaseSupplierName,
                    EntryDate = entry.EntryDate,
                    EntryValue = entry.EntryValue,
                    IsInjection = entry.IsInjection,
                    WeeklyTotal = entry.WeeklyTotal,
                    MaxWeeklyTotal =
                        entry.Facility.FacilitySystemDeliveryEntryTypes.SingleOrDefault(t =>
                                  t.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType.Id && t.IsEnabled)
                            ?.MaximumValue * 7,
                    SystemDeliveryEntryTypeIsInjectionSite = SystemDeliveryEntryTypeIsInjectionSite(
                        entry.Facility.FacilitySystemDeliveryEntryTypes, entry.SystemDeliveryEntryType.Id)
                });
            }
        }

        private static bool SystemDeliveryEntryTypeIsInjectionSite(IEnumerable<FacilitySystemDeliveryEntryType> facilitySystemDeliveryEntryTypes, int systemDeliveryEntryTypeId) =>
            facilitySystemDeliveryEntryTypes
              ?.FirstOrDefault(x =>
                    x.IsEnabled && x.SystemDeliveryEntryType.Id ==
                    systemDeliveryEntryTypeId)?.IsInjectionSite ?? false;

        #region Validation

        private static bool CheckEntriesAreInvalid(decimal? minValue, decimal? maxValue, decimal? entryValue) =>
            entryValue != decimal.Zero &&
            (entryValue < minValue ||
             entryValue > maxValue);

        private IEnumerable<ValidationResult> ValidateMinMaxValueForFacilitySystemDeliveryTypes(List<CreateSystemDeliveryFacilityEntry> entryList)
        {
            const string errorMessage = "Value not within range, please correct.";
            var index = 0;

            foreach (var entry in entryList)
            {
                var facility = _container.GetInstance<IFacilityRepository>().Find(entry.FacilityId);
                var minValue = facility.FacilitySystemDeliveryEntryTypes.FirstOrDefault(y => y.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType && y.IsEnabled)?.MinimumValue;
                var maxValue = facility.FacilitySystemDeliveryEntryTypes.FirstOrDefault(y => y.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType && y.IsEnabled)?.MaximumValue;
                    
                if (CheckEntriesAreInvalid(minValue, maxValue, entry.EntryValue))
                {
                    var fieldName = $@"FacilityEntries[{index}].EntryValue";
                    yield return new ValidationResult(errorMessage, new[] { fieldName });
                }

                index++;
            }
        }

        private IEnumerable<ValidationResult> ValidateInjectionsAreValid(List<CreateSystemDeliveryFacilityEntry> entries)
        {
            const string errorMessage = "Injection is only valid for entries less than zero, please correct.";
            var index = 0;

            foreach (var entry in entries)
            {
                if (entry.EntryValue > 0 && entry.IsInjection)
                {
                    yield return new ValidationResult(errorMessage, new[] { $@"Entries[{index}].EntryValue" });
                }
                
                index++;
            }
        }

        #endregion

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateMinMaxValueForFacilitySystemDeliveryTypes(FacilityEntries))
                       .Concat(ValidateInjectionsAreValid(FacilityEntries)); 
        }

        public override SystemDeliveryEntry MapToEntity(SystemDeliveryEntry entity)
        {
            var previousFacilities = entity.Facilities.Select(x => x.Id).ToArray();
            base.MapToEntity(entity);

            // if they edit their facilities we need to wipe out the list so they can re-enter
            if (!Facilities.OrderBy(x => x).SequenceEqual(previousFacilities.OrderBy(x => x)))
            {
                entity.FacilityEntries.Clear(); 
                base.BuildFacilityEntries(entity);
            }
            else
            {
                // we want to clear this and re-enter the updated values
                if (entity.FacilityEntries.Count != 0)
                {
                    entity.FacilityEntries.Clear(); 
                }
                
                MapFacilityEntries(entity);
            }

            return entity;
        }

        public override void Map(SystemDeliveryEntry entity)
        {
            base.Map(entity);
            BuildFacilityEntries(entity);
        }

        #endregion
    }

    public class SearchSystemDeliveryEntryViewModel : SearchSet<SystemDeliveryEntry>
    {
        private int[] _publicWaterSupplies;
        private int[] _wasteWaterSystems;

        [MultiSelect,
         SearchAlias("critOc.State", "Id")]
        public int[] State { get; set; }
        
        [View("Entry Id")]
        public int? EntityId { get; set; }

        [MultiSelect("", nameof(OperatingCenter), "ByStateIds", DependsOn = nameof(State)),
         EntityMustExist(typeof(OperatingCenter)), SearchAlias("OperatingCenters", "critOc", "Id")]
        public int[] OperatingCenter { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(SystemDeliveryType)),
         SearchAlias("criteriaFacilityEntries.SystemDeliveryType", "Id")]
        public int? SystemDeliveryType { get; set; }

        [View("PWSIDs"), MultiSelect("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenter"), 
         EntityMap, EntityMustExist(typeof(PublicWaterSupply)), 
         SearchAlias("criteriaFacilityEntries.Facility", "PublicWaterSupply.Id")]
        public int[] PublicWaterSupplies
        {
            get => SystemDeliveryType == MapCall.Common.Model.Entities.SystemDeliveryType.Indices.WATER ? _publicWaterSupplies : null;
            set => _publicWaterSupplies = value;
        }
        
        [View("WWSIDs"), MultiSelect("Environmental", "WasteWaterSystem", "ByOperatingCenters", DependsOn = "OperatingCenter"), 
         EntityMap, EntityMustExist(typeof(WasteWaterSystem)),
         SearchAlias("WasteWaterSystems", "critWWS", "Id")]
        public int[] WasteWaterSystems
        {
            get => SystemDeliveryType == MapCall.Common.Model.Entities.SystemDeliveryType.Indices.WASTE_WATER ? _wasteWaterSystems : null;
            set => _wasteWaterSystems = value;
        }

        [MultiSelect("", "Facility", "GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId", DependentsRequired = DependentRequirement.One,
             DependsOn = (nameof(OperatingCenter) + "," + nameof(SystemDeliveryType) + "," + nameof(PublicWaterSupplies) + "," + nameof(WasteWaterSystems))),
         EntityMustExist(typeof(Facility)), SearchAlias("criteriaFacilityEntries.Facility", "Id")]
        public int[] Facility { get; set; }

        [MultiSelect("Production", "SystemDeliveryEntryType", "ByFacilitiesSystemDeliveryTypeId",
             DependsOn = nameof(Facility)), EntityMap, EntityMustExist(typeof(SystemDeliveryEntryType)),
         SearchAlias("criteriaFacilityEntries.SystemDeliveryEntryType", "Id")]
        public int[] SystemDeliveryEntryType { get; set; }
        
        [SearchAlias("FacilityEntries", "criteriaFacilityEntries", "EntryDate")]
        public DateRange EntryDate { get; set; }

        [DropDown("", "Employee", "GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEntries",
             DependsOn = nameof(OperatingCenter)), EntityMap, EntityMustExist(typeof(Employee)),
         SearchAlias("criteriaFacilityEntries.EnteredBy", "Id")]
        public int? EnteredBy { get; set; }

        public bool? IsHyperionFileCreated { get; set; }
    }

    public class ValidateSystemDeliveryEntryViewModel : ViewModel<SystemDeliveryEntry>
    {
        public ValidateSystemDeliveryEntryViewModel(IContainer container) : base(container) { }

        public override SystemDeliveryEntry MapToEntity(SystemDeliveryEntry entity)
        {
            base.MapToEntity(entity);
            entity.IsValidated = true;
            return entity;
        }
    }

    /// <summary>
    /// CreateSystemDeliveryFacilityEntry
    /// </summary>
    public class AddSystemDeliveryEquipmentEntryReversal : ViewModel<SystemDeliveryEntry>
    {
        #region Fields

        private SystemDeliveryEntry _original;
        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Properties

        [DoesNotAutoMap("Need the original entity to get things")]
        public new SystemDeliveryEntry Original
        {
            // need to override this so we can grab it from the repository
            // if we don't have it
            get => _original ?? (_original = _container.GetInstance<IRepository<SystemDeliveryEntry>>().Find(Id));
            protected set => _original = value;
        }

        [DoesNotAutoMap]
        public List<EditSystemDeliveryFacilityEntry> FacilityEntries { get; set; }

        #endregion

        public AddSystemDeliveryEquipmentEntryReversal(IContainer container) : base(container)
        {
            FacilityEntries = new List<EditSystemDeliveryFacilityEntry>();
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #region Private Methods

        private void MapFacilityEntryAdjustments(SystemDeliveryEntry entity)
        {
            var enteredBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
            var systemDeliveryFacilityEntries = FacilityEntries.Where(x =>
                x.SystemDeliveryEntryType.Id != SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            
            foreach (var adjustedEntry in systemDeliveryFacilityEntries)
            {
                if (!adjustedEntry.IsBeingAdjusted)
                {
                    continue;
                }

                var originalEntry = entity.FacilityEntries.First(t => t.Id == adjustedEntry.Id);
                originalEntry.HasBeenAdjusted = true;
                var originalEntryValue = originalEntry.EntryValue;
                originalEntry.EntryValue = adjustedEntry.AdjustedEntryValue ?? 0.00m;
                originalEntry.AdjustmentComment = adjustedEntry.AdjustmentComment;
                originalEntry.OriginalEntryValue = originalEntryValue;
                    
                originalEntry.Adjustments.Add(new SystemDeliveryFacilityEntryAdjustment {
                    SystemDeliveryFacilityEntry = originalEntry,
                    SystemDeliveryEntry = entity,
                    Facility = originalEntry.Facility,
                    AdjustedDate = adjustedEntry.EntryDate,
                    DateTimeEntered = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                    EnteredBy = enteredBy,
                    AdjustedEntryValue = adjustedEntry.AdjustedEntryValue ?? 0.00M,
                    OriginalEntryValue = originalEntryValue,
                    Comment = adjustedEntry.AdjustmentComment
                });
                
                // save the updated original entry
                _container.GetInstance<IRepository<SystemDeliveryFacilityEntry>>().Save(originalEntry);
                
                // if an adjustment is entered, we also need a second one to denote the adjustment on the Transfer From entry
                if (originalEntry.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO)
                {
                    MapFacilityEntryAdjustmentTransfer(entity, originalEntry, adjustedEntry, enteredBy);
                }
            }
        }

        private void MapFacilityEntryAdjustmentTransfer(SystemDeliveryEntry entity, SystemDeliveryFacilityEntry originalEntry, EditSystemDeliveryFacilityEntry adjustedEntry, Employee enteredBy)
        {
            var transferFromEntry =
                entity.FacilityEntries.First(x =>
                    x.Facility.Id == originalEntry.SupplierFacility.Id &&
                    x.EntryDate == adjustedEntry.EntryDate &&
                    x.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            transferFromEntry.HasBeenAdjusted = true;
            var originalTransferEntryValue = transferFromEntry.EntryValue;
            
            // We want to first subtract the original entry value
            transferFromEntry.EntryValue += originalEntry.OriginalEntryValue ?? 0.00m;
            
            // Then add the new adjusted value
            transferFromEntry.EntryValue -= adjustedEntry.AdjustedEntryValue ?? 0.00M;
            transferFromEntry.AdjustmentComment = adjustedEntry.AdjustmentComment;
            transferFromEntry.OriginalEntryValue = originalTransferEntryValue;
            
            transferFromEntry.Adjustments.Add(new SystemDeliveryFacilityEntryAdjustment {
                SystemDeliveryFacilityEntry = originalEntry,
                SystemDeliveryEntry = entity,
                Facility = transferFromEntry.Facility,
                AdjustedDate = adjustedEntry.EntryDate,
                DateTimeEntered = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                EnteredBy = enteredBy,
                AdjustedEntryValue = transferFromEntry.EntryValue,
                OriginalEntryValue = originalTransferEntryValue,
                Comment = adjustedEntry.AdjustmentComment
            });
            _container.GetInstance<IRepository<SystemDeliveryFacilityEntry>>().Save(transferFromEntry);
        }

        #endregion

        #region Validation

        private bool CheckEntriesAreInvalid(decimal? minValue, decimal? maxValue, decimal? entryValue)
        {
            return entryValue < minValue || entryValue > maxValue;
        }
        private IEnumerable<ValidationResult> ValidateMinMaxValueForFacilitySystemDeliveryTypes(List<EditSystemDeliveryFacilityEntry> entryList)
        {
            const string errorMessage = "Value not within range, please correct.";
            const string commentMaxLength = "Comment cannot exceed 100 characters.";
            var index = 0;

            foreach (var listEntry in entryList)
            {
                var entry = Original.FacilityEntries.SingleOrDefault(y => y.Id == listEntry.Id);

                var facility = entry?.Facility;
                var minValue = facility?.FacilitySystemDeliveryEntryTypes.FirstOrDefault(y => y.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType.Id && y.IsEnabled)?.MinimumValue;
                var maxValue = facility?.FacilitySystemDeliveryEntryTypes.FirstOrDefault(y => y.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType.Id && y.IsEnabled)?.MaximumValue;
                
                if (listEntry.IsBeingAdjusted && CheckEntriesAreInvalid(minValue, maxValue, listEntry.AdjustedEntryValue))
                {
                    yield return new ValidationResult(errorMessage, new[] { $@"FacilityEntries[{index}].AdjustedEntryValue" });
                }
                else if (listEntry.IsBeingAdjusted && listEntry.AdjustmentComment.Length > SystemDeliveryFacilityEntryAdjustment.StringLengths.MAX_COMMENT)
                {
                    yield return new ValidationResult(commentMaxLength, new[] { $@"FacilityEntries[{index}].AdjustedEntryValue" });
                }

                index++;
            }
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            base.Validate(validationContext).Concat(ValidateMinMaxValueForFacilitySystemDeliveryTypes(FacilityEntries));

        public override void Map(SystemDeliveryEntry entity)
        {
            base.Map(entity);
            FacilityEntries = entity.FacilityEntries.Where(x =>
                x.SystemDeliveryEntryType.Id != SystemDeliveryEntryType.Indices.TRANSFERRED_FROM).Select(x =>
                _viewModelFactory.Build<EditSystemDeliveryFacilityEntry, SystemDeliveryFacilityEntry>(x)).ToList();
            foreach (var entry in FacilityEntries)
            {
                entry.AdjustmentComment = string.Empty;
            }
        }

        public override SystemDeliveryEntry MapToEntity(SystemDeliveryEntry entity)
        {
            base.MapToEntity(entity);
            
            MapFacilityEntryAdjustments(entity);

            return entity;
        }

        #endregion
    }
}