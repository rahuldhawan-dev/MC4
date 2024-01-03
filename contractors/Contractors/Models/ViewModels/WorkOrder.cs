using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Data.Models.ViewModels;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using NHibernate;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    #region Search

    #region Scheduling

    [Serializable, StringLengthNotRequired]
    public class WorkOrderSchedulingSearch : WorkOrderSearch
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "Id")]
        public virtual int? Town { get; set; }

        [DropDown("TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("TownSection", "Id")]
        public virtual int? TownSection { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("Street", "Id")]
        public virtual int? Street { get; set; }
        public virtual string StreetNumber { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("NearestCrossStreet", "Id")]
        public virtual int? NearestCrossStreet { get; set; }

        [DropDown, SearchAlias("AssetType", "Id")]
        public virtual int? AssetType { get; set; }

        [DropDown("WorkDescription", "ByAssetTypeId", DependsOn = "AssetType", PromptText = "Select a work description above...")]
        [SearchAlias("WorkDescription", "Id")]
        public virtual int? WorkDescription { get; set; }

        [DropDown, SearchAlias("Priority", "Id")]
        public virtual int? Priority { get; set; }

        [View("Time to Complete"), DropDown(UseNullForValueField = true)]
        public virtual string CompletionTime { get; set; }
        public virtual bool? TrafficControlRequired { get; set; }
        public virtual bool? MarkoutRequired { get; set; }
        [View(WorkOrder.DisplayNames.HAS_PENDING_ASSIGNMENTS), BoolFormat("True", "False")]
        public virtual bool? HasPendingAssignments { get; set; }
        [RegularExpression(@"^\d{1,2}$", ErrorMessage = "Must be a 1 or 2 digit numerical value.")]
        public virtual int? MarkoutExpirationDays { get; set; }

        #endregion

        #region Exposed Methods

        public override bool NonWorkOrderNumberFieldsAreNull()
        {
            return (OperatingCenter == null &&
                    Town == null &&
                    TownSection == null &&
                    Street == null &&
                    String.IsNullOrEmpty(StreetNumber) &&
                    NearestCrossStreet == null &&
                    AssetType == null &&
                    WorkDescription == null &&
                    Priority == null &&
                    String.IsNullOrEmpty(CompletionTime) &&
                    TrafficControlRequired == null &&
                    MarkoutRequired == null &&
                    HasPendingAssignments == null &&
                    MarkoutExpirationDays == null);
        }

        #endregion
    }

    #endregion

    #region Planning

    [StringLengthNotRequired]
    public class WorkOrderPlanningSearch : WorkOrderSearch
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "Id")]
        public int? Town { get; set; }

        [DropDown("TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("TownSection", "Id")]
        public int? TownSection { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("Street", "Id")]
        public int? Street { get; set; }

        public string StreetNumber { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("NearestCrossStreet", "Id")]
        public int? NearestCrossStreet { get; set; }

        [DropDown, SearchAlias("AssetType", "Id")]
        public int? AssetType { get; set; }

        [DropDown("WorkDescription", "ByAssetTypeId", DependsOn = "AssetType", PromptText = "Select a work description above...")]
        [SearchAlias("WorkDescription", "Id")]
        public int? WorkDescription { get; set; }

        [DropDown, SearchAlias("Priority", "Id")]
        public int? Priority { get; set; }

        public DateTime? DateReceived { get; set; }

        [View("Requested By"), DropDown]
        [SearchAlias("RequestedBy", "Id")]
        public int? Requester { get; set; }

        [DropDown, SearchAlias("MarkoutRequirement", "Id")]
        public int? MarkoutRequirement { get; set; }

        [BoolFormat("Required", "Not Required")]
        public bool? StreetOpeningPermitRequired { get; set; }

        [DropDown, SearchAlias("Purpose", "Id")]
        public int? Purpose { get; set; }

        #endregion

        #region Exposed Methods

        public override bool NonWorkOrderNumberFieldsAreNull()
        {
            return (OperatingCenter == null &&
                    Town == null &&
                    TownSection == null &&
                    Street == null &&
                    String.IsNullOrEmpty(StreetNumber) &&
                    NearestCrossStreet == null &&
                    AssetType == null &&
                    WorkDescription == null &&
                    Priority == null &&
                    DateReceived == null &&
                    Requester == null &&
                    MarkoutRequirement == null &&
                    StreetOpeningPermitRequired == null &&
                    Purpose == null);
        }

        #endregion
    }

    #endregion

    #region Finalization

    [Serializable, StringLengthNotRequired]
    public class WorkOrderFinalizationSearch : WorkOrderSearch
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "Id")]
        public int? OperatingCenter { get; set; }
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "Id")]
        public int? Town { get; set; }
        [DropDown("TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("TownSection", "Id")]
        public int? TownSection { get; set; }
        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("Street", "Id")]
        public int? Street { get; set; }
        public string StreetNumber { get; set; }
        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("NearestCrossStreet", "Id")]
        public int? NearestCrossStreet { get; set; }
        [DropDown, SearchAlias("AssetType", "Id")]
        public int? AssetType { get; set; }
        [DropDown("WorkDescription", "ByAssetTypeId", DependsOn = "AssetType", PromptText = "Select a work description above...")]
        [SearchAlias("WorkDescription", "Id")]
        public int? WorkDescription { get; set; }

        #endregion

        #region Exposed Methods

        public override bool NonWorkOrderNumberFieldsAreNull()
        {
            return
                OperatingCenter == null &&
                Town == null &&
                TownSection == null &&
                Street == null &&
                StreetNumber == null &&
                NearestCrossStreet == null &&
                AssetType == null &&
                WorkDescription == null;
        }

        #endregion
    }

    #endregion

    #region General

    [StringLengthNotRequired]
    public class WorkOrderGeneralSearch : WorkOrderSearch
    {
        #region Properties

        [DropDown]
        [SearchAlias("OperatingCenter", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Town", "Id")]
        public int? Town { get; set; }

        [DropDown("TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("TownSection", "Id")]
        public int? TownSection { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("Street", "Id")]
        public int? Street { get; set; }

        public string StreetNumber { get; set; }
        
        public string ApartmentAddtl { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above...")]
        [SearchAlias("NearestCrossStreet", "Id")]
        public int? NearestCrossStreet { get; set; }

        [DropDown, SearchAlias("AssetType", "Id")]
        public int? AssetType { get; set; }

        [DropDown("WorkDescription", "ByAssetTypeId", DependsOn = "AssetType", PromptText = "Select a work description above...")]
        [SearchAlias("WorkDescription", "Id")]
        public int? WorkDescription { get; set; }

        [DropDown, SearchAlias("Priority", "Id")]
        public int? Priority { get; set; }

        public DateTime? DateReceived { get; set; }

        public DateTime? DateCompleted { get; set; }

        [View("Requested By"), DropDown]
        [SearchAlias("RequestedBy", "Id")]
        public int? Requester { get; set; }

        [DropDown, SearchAlias("MarkoutRequirement", "Id")]
        public int? MarkoutRequirement { get; set; }

        [BoolFormat("Required", "Not Required")]
        public bool? StreetOpeningPermitRequired { get; set; }
        public bool? StreetOpeningPermitRequested { get; set; }
        public bool? StreetOpeningPermitIssued { get; set; }

        [DropDown, SearchAlias("Purpose", "Id")]
        public int? Purpose { get; set; }
        
        public bool? Completed { get; set; }
        
        public bool? HasMaterialsUsed { get; set; }

        public bool? DigitalAsBuiltCompleted { get; set; }
        
        public DateRange DatePitcherFilterDeliveredToCustomer { get; set; }
        
        public bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }
       
        [View("WBS Charged")]
        public SearchString AccountCharged { get; set; }

        public SearchString Notes { get; set; }
        
        [View(WorkOrder.DisplayNames.PLANNED_COMPLETION_DATE)]
        public DateRange PlannedCompletionDate { get; set; }

        #endregion

        #region Exposed Methods

        public override bool NonWorkOrderNumberFieldsAreNull()
        {
            return (OperatingCenter == null &&
                              Town == null &&
                              TownSection == null &&
                              Street == null &&
                              String.IsNullOrEmpty(StreetNumber) &&
                              NearestCrossStreet == null &&
                              AssetType == null &&
                              WorkDescription == null &&
                              Priority == null &&
                              DateReceived == null &&
                              DateCompleted == null &&
                              Requester == null &&
                              MarkoutRequirement == null &&
                              StreetOpeningPermitRequired == null &&
                              Purpose == null);
        }

        #endregion
    }

    #endregion

    public abstract class WorkOrderSearch : SearchSet<WorkOrder>, IWorkOrderSearch, IValidatableObject 
    {
        #region Consts

        public const string NO_CRITERIA_ENTERED = "No search criteria chosen.";

        #endregion

        #region Properties

        [View("Work Order Number")]
        public int? Id { get; set; }

        [MultiSelect]
        public int[] DocumentType { get; set; }

        // Without this, general search sorts differently on every page load.
        public override string DefaultSortBy => "Id";
        public override bool DefaultSortAscending => true;

        #endregion

        #region Abstract Methods

        public abstract bool NonWorkOrderNumberFieldsAreNull();

        #endregion

        #region Exposed Methods

        public virtual bool QueryIsNull()
        {
            return NonWorkOrderNumberFieldsAreNull() && !Id.HasValue;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QueryIsNull())
            {
                yield return new ValidationResult(NO_CRITERIA_ENTERED);
            }
            else if (NonWorkOrderNumberFieldsAreNull())
            {
                yield return ValidationResult.Success;
            }
            else if (Id != null)
            {
                yield return new ValidationResult(WorkOrder.SEARCH.ERROR);
            }
        }

        #endregion
    }

    #endregion

    #region Details

    public abstract class WorkOrderDetails : ViewModel<WorkOrder>
    {
        #region Constructors

        public WorkOrderDetails(IContainer container) : base(container) { }

        #endregion
    }

    public class WorkOrderTrafficDetails : WorkOrderDetails
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public bool TrafficControlRequired { get; set; }

        [Required, Min(0)] // Might be required only if true
        public int? NumberOfOfficersRequired { get; set; }

        [AutoMap(MapDirections.ToViewModel)] // Notes are appended in a special way.
        [Multiline]
        public string Notes { get; set; }

        [Multiline, DoesNotAutoMap]
        public string AppendedNotes { get; set; }

        #endregion

        #region Constructors

        public WorkOrderTrafficDetails(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            Notes = entity.Notes;
            
            if (!String.IsNullOrWhiteSpace(AppendedNotes))
            {
                entity.AppendNotes(
                    _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser, _container.GetInstance<IDateTimeProvider>().GetCurrentDate(), AppendedNotes);
            }

           // entity.NumberOfOfficersRequired = NumberOfOfficersRequired;

            base.MapToEntity(entity);
            return entity;
        }

        #endregion
    }

    public class WorkOrderFinalizationDetails : WorkOrderDetails
    {
        #region Constants

        public const string NO_MAIN_BREAK_INFO =
                                "A main break work order cannot be finalized without main break information.  Please enter some main break information in the Main Break tab.",
                            OPEN_CREW_ASSIGNMENTS =
                                "This order has one or more Crew Assignments that are not closed.  Please ensure that all end times are recorded.",
                            DISTANCE_FROM_CROSS_STREET =
                                "This order required a street opening permit, so 'Distance from Cross Street' must be filled in under the 'Additional' tab before it can be finalized.",
                            FLUSH_TIME_BELOW_MINIMUM = "Below minimum reflush of 30 minutes",
                            ERROR_METER_LOCATION_REQUIRED = "The MeterLocation field is required.";

        public const int MINIMUM_FLUSH_TIME_MINUTES = 30;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public WorkOrder WorkOrder { get; set; }

        [Required, View(FormatStyle.Date)]
        public DateTime? DateCompleted { get; set; }

        [AutoMap(MapDirections.None)]
        public int? WorkDescription => WorkOrder?.WorkDescription?.Id;

        [DropDown]
        [EntityMap, EntityMustExist(typeof(WorkOrderFlushingNoticeType))]
        public int? FlushingNoticeType { get; set; }

        // NOTE: THE DATE COMPLETED IS ALREADY REQUIRED TO GET TO THIS POINT, THESE
        // ALSO REQUIRE IT BUT ARE DISABLED BECAUSE THE VALIDATORS BELOW ARE MORE IMPORTANT
        // AND THEY STOP NORMAL ORDERS GETTING THROUGH.

        [DropDown]
        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(GetServiceLineRenewalIds),
            typeof(WorkOrderFinalizationDetails))]
        [EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? PreviousServiceLineMaterial { get; set; }

        [DropDown]
        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(GetServiceLineRenewalIds),
            typeof(WorkOrderFinalizationDetails))]
        [EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? PreviousServiceLineSize { get; set; }

        [DropDown]
        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(CompanyServiceLineInfoRequiredIds),
            typeof(WorkOrderFinalizationDetails))]
        [EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CompanyServiceLineMaterial { get; set; }

        [DropDown]
        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(CompanyServiceLineInfoRequiredIds),
            typeof(WorkOrderFinalizationDetails))]
        [EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CompanyServiceLineSize { get; set; }

        [DropDown]
        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(GetServiceLineRenewalIds),
            typeof(WorkOrderFinalizationDetails))]
        [EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CustomerServiceLineMaterial { get; set; }

        [DropDown]
        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(GetServiceLineRenewalIds),
            typeof(WorkOrderFinalizationDetails))]
        [EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CustomerServiceLineSize { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(GetServiceLineRenewalIds),
            typeof(WorkOrderFinalizationDetails))]
        public DateTime? DoorNoticeLeftDate { get; set; }

        [DoesNotAutoMap] // Not an actual View Property - set by MapToEntity and Used by Controller
        public bool SendToSAP { get; set; }

        [RequiredWhen(nameof(DisplayComplianceInfo),ComparisonType.EqualTo,true, ErrorMessage = "The InitialServiceLineFlushTime field is required.")]
        public int? InitialServiceLineFlushTime { get; set; }

        [RequiredWhen(nameof(DisplayComplianceInfo), ComparisonType.EqualTo, true)]
        public bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }

        [RequiredWhen(nameof(HasPitcherFilterBeenProvidedToCustomer),
            ComparisonType.EqualTo, true)]
        public DateTime? DatePitcherFilterDeliveredToCustomer { get; set; }

        [DropDown]
        [EntityMap,
         EntityMustExist(typeof(PitcherFilterCustomerDeliveryMethod))]
        [RequiredWhen(nameof(HasPitcherFilterBeenProvidedToCustomer),
            ComparisonType.EqualTo, true)]
        public int? PitcherFilterCustomerDeliveryMethod { get; set; }

        [View(WorkOrder.DisplayNames.PITCHER_FILTER_DELIVERY_OTHER_METHOD)]
        [StringLength(WorkOrder.StringLengths
                               .PITCHER_FILTER_CUSTOMER_DELIVERY_OTHER)]
        [RequiredWhen(nameof(PitcherFilterCustomerDeliveryMethod),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.PitcherFilterCustomerDeliveryMethod.Indices.OTHER)]
        public string PitcherFilterCustomerDeliveryOtherMethod { get; set; }

        [AutoMap(MapDirections.None)]
        public int? AssetTypeId => WorkOrder?.AssetType?.Id;

        [DropDown, EntityMap, EntityMustExist(typeof(MeterLocation))]
        [RequiredWhen(nameof(AssetTypeId), AssetType.Indices.SERVICE, ErrorMessage = ERROR_METER_LOCATION_REQUIRED)]
        [ClientCallback("WorkOrderFinalization.validateMeterLocation", ErrorMessage = ERROR_METER_LOCATION_REQUIRED)]
        public int? MeterLocation { get; set; }

        [DoesNotAutoMap]
        public bool DisplayComplianceInfo
        {
            get
            {
                return WorkDescription.HasValue && MapCall.Common.Model.Entities.WorkDescription.PITCHER_FILTER_REQUIREMENT.Contains(
                    (int)WorkDescription);
            }
        }

        public DateTime? DateCustomerProvidedAWStateLeadInformation { get; set; }

        #endregion

        #region Constructors

        public WorkOrderFinalizationDetails(IContainer container) : base(container)
        {

        }

        #endregion

        #region Private Methods

        private static int[] GetServiceLineRenewalIds() => MapCall.Common.Model.Entities.WorkDescription.SERVICE_LINE_RENEWALS;

        private static int[] GetServiceLineRetireIds() =>
            MapCall.Common.Model.Entities.WorkDescription.SERVICE_LINE_RETIRE;

        private static int[] CompanyServiceLineInfoRequiredIds() =>
            GetServiceLineRenewalIds().Concat(GetServiceLineRetireIds()).ToArray();

        private static int[] GetPitcherFilterRequirementIds() => MapCall.Common
           .Model.Entities.WorkDescription.PITCHER_FILTER_REQUIREMENT;

        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder entity)
        {
            base.Map(entity);

            WorkOrder = entity;

            // force an eager load for crew assignments, documents, main
            // breaks, markouts, markouts' markout types, materials used and
            // restorations to prevent a race condition in TeamCity
            NHibernateUtil.Initialize(entity.WorkOrderDocuments);
            entity.WorkOrderDocuments.Each(
                doc => NHibernateUtil.Initialize(doc.DocumentType));
            NHibernateUtil.Initialize(entity.CrewAssignments);
            NHibernateUtil.Initialize(entity.MainBreaks);
            NHibernateUtil.Initialize(entity.Markouts);
            entity.Markouts.Each(mo => NHibernateUtil.Initialize(mo.MarkoutType));
            NHibernateUtil.Initialize(entity.MaterialsUsed);
            NHibernateUtil.Initialize(entity.Restorations);
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            SendToSAP = entity.OperatingCenter.SAPEnabled &&
                        entity.OperatingCenter.SAPWorkOrdersEnabled && 
                        !entity.OperatingCenter.IsContractedOperations &&
                        !entity.DateCompleted.HasValue;
            
            if (InitialServiceLineFlushTime !=
                entity.InitialServiceLineFlushTime)
            {
                entity.InitialFlushTimeEnteredAt = _container
                                                              .GetInstance<IDateTimeProvider>()
                                                              .GetCurrentDate();
            }

            entity = base.MapToEntity(entity);

            if (entity.Service != null)
            {
                var service = entity.Service;
                if (entity.PreviousServiceLineMaterial != null)
                {
                    service.PreviousServiceMaterial = entity.PreviousServiceLineMaterial;
                }
                if (entity.PreviousServiceLineSize != null)
                {
                    service.PreviousServiceSize = entity.PreviousServiceLineSize;
                }
                if (entity.CustomerServiceLineMaterial != null)
                {
                    service.CustomerSideMaterial = entity.CustomerServiceLineMaterial;
                }
                if (entity.CustomerServiceLineSize != null)
                {
                    service.CustomerSideSize = entity.CustomerServiceLineSize;
                }
                if (entity.CompanyServiceLineMaterial != null)
                {
                    service.ServiceMaterial = entity.CompanyServiceLineMaterial;
                }
                if (entity.CompanyServiceLineSize != null)
                {
                    service.ServiceSize = entity.CompanyServiceLineSize;
                }

                var serviceRepo = _container.GetInstance<IRepository<Service>>();
                serviceRepo.Save(service);
            }
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model =
                (WorkOrderFinalizationDetails)validationContext.ObjectInstance;
            var results = new List<ValidationResult>();

            var original =
                _container
                    .GetInstance<IWorkOrderRepository>()
                    .Find(model.Id);

            if (new[] {
                (int)MapCall.Common.Model.Entities.WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR,
                (int)MapCall.Common.Model.Entities.WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE
            }.Contains(
                original.WorkDescription.Id) &&
                    original.MainBreaks.Count == 0)
            {
                results.Add(new ValidationResult(NO_MAIN_BREAK_INFO));
            }

            if (model.DateCompleted.HasValue &&
                original.CrewAssignments.Any(x => x.IsOpen))
            {
                results.Add(new ValidationResult(OPEN_CREW_ASSIGNMENTS));
            }

            if (original.StreetOpeningPermitRequired && !original.DistanceFromCrossStreet.HasValue)
            {
                results.Add(new ValidationResult(DISTANCE_FROM_CROSS_STREET));
            }

            if (original.AssetType.Id == AssetType.Indices.SERVICE && (MeterLocation == null
                    || MeterLocation.Value == MapCall.Common.Model.Entities.MeterLocation.Indices.UNKNOWN))
            {
                results.Add(new ValidationResult(ERROR_METER_LOCATION_REQUIRED));
            }
            
            return results;
        }

        #endregion
    }

    public class WorkOrderAdditionalFinalizationInfo : WorkOrderDetails
    {
        #region Private Members

        private int _lostWater;

        #endregion

        #region Properties

        [Numeric]
        public float? TotalManHours { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int? WorkDescription { get; set; }

        [Numeric, View("Distance From Cross Street (feet)"),
         RequiredWhen("StreetOpeningPermitRequired", true,
             ErrorMessage = "Required when a street opening permit is required."),
         RequiredWhen("Priority", (int)WorkOrderPriorityEnum.Emergency,
             ErrorMessage = "Required when the work order priority is 'Emergency'.")]
        public double? DistanceFromCrossStreet { get; set; }
        [Numeric, View("Lost Water (gpm)")]
        public int? LostWater
        {
            get { return _lostWater > 0 ? (int?)_lostWater : null; }
            set { _lostWater = value.HasValue ? value.Value : 0; }
        }

        [AutoMap(MapDirections.ToViewModel)] // These are appended in MapToEntity.
        [Multiline]
        public string Notes { get; set; }
        [Multiline, DoesNotAutoMap/* Done in MapToEntity */]
        public string AppendNotes { get; set; }

        // Only used in a hidden input for DistanceFromCrossStreet validation
        [EntityMap(MapDirections.ToViewModel)]
        public int Priority { get; set; }

        public bool StreetOpeningPermitRequired { get; set; }

        [AutoMap(MapDirections.None)]
        public DateTime? HiddenDateCompleted { get; set; }
        [AutoMap(MapDirections.None)]
        public bool WorkDescriptionEditable { get; set; }
        [AutoMap(MapDirections.None)]
        public WorkOrder DisplayWorkOrder { get; set; }

        [DoesNotAutoMap("Not an actual View Property - set by MapToEntity and Used by Controller")]
        public bool SendSewerOverflowChangedNotification { get; set; }

        #endregion

        #region Constructors

        public WorkOrderAdditionalFinalizationInfo(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder entity)
        {
            DisplayWorkOrder = entity;

            WorkDescriptionEditable = true;

            var hookedUpToSAP = (entity.OperatingCenter.SAPEnabled &&
                                 entity.OperatingCenter.SAPWorkOrdersEnabled &&
                                 !entity.OperatingCenter.IsContractedOperations);
            if (hookedUpToSAP && entity.PlantMaintenanceActivityTypeOverride != null)
                WorkDescriptionEditable = false;
            if (entity.ApprovedOn.HasValue)
                WorkDescriptionEditable = false;

            base.Map(entity);
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            if (WorkDescription == (int)MapCall.Common.Model.Entities
                                               .WorkDescription.Indices.SEWER_MAIN_OVERFLOW &&
                (entity.WorkDescription == null ||
                 entity.WorkDescription?.Id != (int)MapCall.Common.Model.Entities
                    .WorkDescription.Indices.SEWER_MAIN_OVERFLOW))
            {
                SendSewerOverflowChangedNotification = true;
            }

            base.MapToEntity(entity);
           
            if (!String.IsNullOrWhiteSpace(AppendNotes))
            {
                entity.AppendNotes(
                    _container.GetInstance<IAuthenticationService<ContractorUser>>().
                        CurrentUser,
                    _container.GetInstance<IDateTimeProvider>().
                        GetCurrentDate(),
                    AppendNotes);
            }
            
            return entity;
        }

        #endregion
    }

    public class WorkOrderWithTapImages : WorkOrderDetails
    {
        #region Properties

        [StringLength(9)]
        public virtual string PremiseNumber { get; set; }
        [StringLength(50)]
        public virtual string ServiceNumber { get; set; }
        [DoesNotAutoMap]
        public IList<TapImage> TapImages { get; set; }

        #endregion

        #region Constructors

        public WorkOrderWithTapImages(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder order)
        {
            base.Map(order);
            TapImages = _container
                       .GetInstance<ITapImageRepository>()
                       .GetTapImagesForWorkOrder(order).ToList();
        }

        #endregion
    }

    #endregion
}