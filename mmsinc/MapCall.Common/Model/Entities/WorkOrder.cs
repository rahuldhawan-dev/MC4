using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using WorkDescriptionType = MapCall.Common.Model.Entities.WorkDescription;
using MapCall.Common.Model.Mappings;

namespace MapCall.Common.Model.Entities
{
    // TODO: Bug #2927 - 
    // TODO: Bug #2927 - 
    // TODO: Bug #2927 - 
    // TODO: Bug #2927 -
    // TODO: Bug #2927 -
    // TODO: Bug #2927 - Properly link services????
    /// <summary>
    /// This class has a mapping which will filter out cancelled orders.
    /// </summary>
    [Serializable]
    public class WorkOrder
        : IEntityWithCreationTracking<User>,
            IValidatableObject,
            IThingWithCoordinate,
            ISAPEntity,
            ISapWorkOrder,
            IHasWorkOrderStatus,
            IThingWithOperatingCenter,
            IThingWithState,
            IThingWithDocuments
    {
        #region Constants

        public struct SEARCH
        {
            #region Constants

            public const string ERROR =
                "You must choose either Work Order Number or any combination of the other fields.";

            #endregion
        }

        public struct StringLengths
        {
            #region Constants

            public const int CUSTOMER_NAME = 30,
                             ALERT_ID = 20,
                             PHONE_NUMBER = 14,
                             PREMISE_NUMBER = 10,
                             SECONDARY_PHONE_NUMBER = 14,
                             SERVICE_NUMBER = 50,
                             STREET_NUMBER = 20,
                             ZIP_CODE = 10,
                             MATERIALS_DOC_ID = 15,
                             CUSTOMER_ACCOUNT_NUMBER = 12,
                             ACCOUNT_CHARGED = 30,
                             INVOICE_NUMBER = 15,
                             BUSINESS_UNIT = 256, // changed in position group.business unit MC-6136
                             METER_SERIAL_NUMBER = 30,
                             APARTMENT_ADDTL =
                                 30, // Made 30 to align with Apartment Number in Service Model, If this is changed Apartment number in service needs to be changed to match due to mapping
                             PITCHER_FILTER_CUSTOMER_DELIVERY_OTHER = 50,
                             DESCRIBE_WHICH_UNITS = 256;

            #endregion
        }

        public struct FormatStrings
        {
            #region Constants

            public const string STREET_ADDRESS = "{0} {1}",
                                TOWN_ADDRESS = "{0}, {1} {2}",
                                PREMISE_SERVICE = "p#:{0}, s#:{1}";

            #endregion
        }

        public struct DisplayNames
        {
            public const string
                ACCOUNT_CHARGED = "WBS Charged",
                ANTICIPATED_REPAIR_TIME = "Anticipated Repair Time",
                APPROVED_ON = "Supervisor Approved On",
                COMPANY_SERVICE_LINE_MATERIAL = "Service Company Material",
                COMPANY_SERVICE_LINE_SIZE = "Service Company Size",
                CREATED_AT = "Created On",
                DISTANCE_FROM_CROSS_STREET = "Distance From Cross Street (feet)",
                ESTIMATED_CUSTOMER_IMPACT = "Estimated Customer Impact",
                ID = "Order #",
                HAS_PENDING_ASSIGNMENTS = "Pending Assignment",
                LOST_WATER = "Total Gallons Lost",
                NOTES = "Notes (EST)",
                PREVIOUS_SERVICE_LINE_MATERIAL = "Previous Service Company Material",
                PREVIOUS_SERVICE_LINE_SIZE = "Previous Service Company Size",
                SAP_EQUIPMENT_NUMBER = "SAP Equipment",
                SAP_NOTIFICATION_NUMBER = "SAP Notification #",
                SAP_WORK_ORDER_NUMBER = "SAP Work Order #",
                SAP_ERROR_CODE = "SAP Status",
                SIGNIFICANT_TRAFFIC_IMPACT = "Significant Traffic Impact",
                STREET_OPENING_PERMIT_REQUIRED = "SOP Required",
                WORK_DESCRIPTION = "Description of Job",
                WORK_ORDER_CANCELLATION_REASON = "Reason for Cancellation",
                INITIAL_SERVICE_LINE_FLUSH_TIME = "Initial Service Line Flush Time (Minutes) Minimum 30-Minute Flush Required",
                INITIAL_SERVICE_LINE_FLUSH_ENTERED_BY = "Initial Service Line Flush Entered By",
                INITIAL_SERVICE_LINE_FLUSH_ENTERED_AT = "Initial Service Line Flush Entered At",
                PITCHER_FILTER_PROVIDED_TO_CUSTOMER = "Pitcher filter provided to customer?",
                PITCHER_FILTER_DELIVERY_OTHER_METHOD = "Explain Other",
                PITCHER_FILTER_CUSTOMER_DELIVERY_METHOD = "How Delivered?",
                DATE_PITCHER_FILTER_DELIVERED_TO_CUSTOMER = "Date Delivered",
                CONTRACTOR_NAME = "Contractor Name",
                PLANNED_COMPLETION_DATE = "Planned Completion Date (back office use only)",
                SPECIAL_INSTRUCTIONS = "Special Instructions";
        }

        //Works in combination with the WorkOrderStatue enum both need to be updated in unison
        public static readonly string[] WORK_ORDER_STATUS_DISPLAY_NAMES = {
            "Other", // 0 
            "Previously Scheduled", // 1
            "Currently Scheduled", // 2
            "Cancelled", // 3 - 
            "Completed", // 4
            "Requires Supervisor Approval", // 5
            "" // 6 - (Work Order With Compliance - It was requested that this status display as blank)
        };
        
        public const string APPEND_NOTES_FORMAT = "{0} - {1} {2}: {3}";
        public static readonly int[] ALL_MAIN_BREAKS = { 74, 80, 82, 83, 103 };
        public const int MAX_RESULTS = 3000;
        public const string PITCHER_FILTER_DISTRIBUTED_MESSAGE = "Pitcher filter last delivered on {0}.";
        public const int NUMBER_OF_MONTHS_TO_DISPLAY_FILTER_DISTRIBUTED_MESSAGE = 6;

        #endregion

        #region Private Members

        protected DateTime? _markoutExpirationDate;
        [NonSerialized] private IIconSetRepository _iconSetRepository;
        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        [View(DisplayNames.ID)]
        public virtual int Id { get; set; }

        [Obsolete("This only exists so that the " + nameof(ArcCollectorLink) + " can be generated.")]
        public virtual int WorkOrderID => Id;

        [View(DisplayNames.CREATED_AT)]
        public virtual DateTime CreatedAt { get; set; }

        [StringLength(StringLengths.CUSTOMER_NAME)]
        public virtual string CustomerName { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateCompleted { get; set; }

        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }

        [StringLength(StringLengths.ALERT_ID)]
        public virtual string AlertID { get; set; }

        public virtual bool? AlertIssued { get; set; }
        [View("Digital As-Built Required")]
        public virtual bool DigitalAsBuiltRequired { get; set; }
        [View("Digital As-Built Completed")]
        public virtual bool? DigitalAsBuiltCompleted { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateReceived { get; set; }
        
        [Multiline, View(DisplayNames.NOTES)]
        public virtual string Notes { get; set; }

        [Multiline, View(DisplayNames.SPECIAL_INSTRUCTIONS)]
        public virtual string SpecialInstructions { get; set; }
        public virtual string RequiredMarkoutNote { get; set; }

        [StringLength(StringLengths.PHONE_NUMBER)]
        public virtual string PhoneNumber { get; set; }

        [StringLength(StringLengths.PREMISE_NUMBER)]
        public virtual string PremiseNumber { get; set; }

        [StringLength(StringLengths.SECONDARY_PHONE_NUMBER)]
        public virtual string SecondaryPhoneNumber { get; set; }

        [StringLength(StringLengths.SERVICE_NUMBER)]
        public virtual string ServiceNumber { get; set; }

        [StringLength(StringLengths.STREET_NUMBER)]
        public virtual string StreetNumber { get; set; }
        
        public virtual bool StreetOpeningPermitRequired { get; set; }

        [StringLength(StringLengths.APARTMENT_ADDTL)]
        public virtual string ApartmentAddtl { get; set; }

        public virtual bool TrafficControlRequired { get; set; }

        [StringLength(StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? MaterialsApprovedOn { get; set; }
        public virtual User MaterialsApprovedBy { get; set; }

        /// <summary>
        /// This is a value from SAP that we get when saving the work order in SAP.
        /// </summary>
        [StringLength(StringLengths.MATERIALS_DOC_ID)]
        public virtual string MaterialsDocID { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime? DateStarted { get; set; }

        public virtual DateTime? DatePrinted { get; set; }
        public virtual DateTime? DateReportSent { get; set; }
        public virtual int? BackhoeOperator { get; set; }
        public virtual DateTime? ExcavationDate { get; set; }
        public virtual DateTime? DateCompletedPC { get; set; }
        [View(DisplayNames.LOST_WATER)]
        public virtual int? LostWater { get; set; }
        public virtual int? NumberOfOfficersRequired { get; set; }
        public virtual int? OldWorkOrderNumber { get; set; }

        [StringLength(StringLengths.CUSTOMER_ACCOUNT_NUMBER)]
        public virtual string CustomerAccountNumber { get; set; }

        [StringLength(StringLengths.ACCOUNT_CHARGED)]
        [View(DisplayNames.ACCOUNT_CHARGED)]
        public virtual string AccountCharged { get; set; }

        public virtual PlantMaintenanceActivityType PlantMaintenanceActivityTypeOverride { get; set; }

        [StringLength(StringLengths.INVOICE_NUMBER)]
        public virtual string InvoiceNumber { get; set; }

        [StringLength(StringLengths.BUSINESS_UNIT)]
        public virtual string BusinessUnit { get; set; }

        [View(DisplayNames.DISTANCE_FROM_CROSS_STREET)]
        public virtual double? DistanceFromCrossStreet { get; set; }
        public virtual DateTime? OfficeAssignedOn { get; set; }
        public virtual bool? SignificantTrafficImpact { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? MarkoutToBeCalled { get; set; }
        public virtual WorkOrderPriority Priority { get; set; }
        public virtual WorkOrderRequester RequestedBy { get; set; }
        public virtual AcousticMonitoringType AcousticMonitoringType { get; set; }
        public virtual User OfficeAssignment { get; set; }

        [View(DisplayNames.SAP_NOTIFICATION_NUMBER)]
        public virtual long? SAPNotificationNumber { get; set; }

        [View(DisplayNames.SAP_WORK_ORDER_NUMBER)]
        public virtual long? SAPWorkOrderNumber { get; set; }

        public virtual bool? RequiresInvoice { get; set; }

        public virtual DateTime? CancelledAt { get; set; }

        public virtual User CancelledBy { get; set; }

        [View(DisplayNames.SAP_ERROR_CODE)]
        public virtual string SAPErrorCode { get; set; }

        public virtual bool? HasSAPErrorCode { get; set; }
        public virtual bool? HasMaterialsUsed { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? MaterialPlanningCompletedOn { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime? MaterialPostingDate { get; set; }

        /// <summary>
        /// The date a supervisor rejected this work order.
        /// </summary>
        public virtual DateTime? DateRejected { get; set; }

        [StringLength(20)]
        public virtual string ORCOMServiceOrderNumber { get; set; }

        public virtual string MeterSerialNumber { get; set; }
        public virtual long? DeviceLocation { get; set; }
        public virtual long? Installation { get; set; }

        [View(DisplayNames.SAP_EQUIPMENT_NUMBER)]
        public virtual long? SAPEquipmentNumber { get; set; }

        public virtual bool? UpdatedMobileGIS { get; set; }

        /// <summary>
        /// This property exists solely to be set for SAP
        /// when updating an SAP record. This value is not
        /// stored in the database.
        /// </summary>
        public virtual string UserId { get; set; }

        [View(DisplayNames.INITIAL_SERVICE_LINE_FLUSH_TIME)]
        public virtual int? InitialServiceLineFlushTime { get; set; }

        [View(DisplayNames.INITIAL_SERVICE_LINE_FLUSH_ENTERED_BY)]
        public virtual User InitialFlushTimeEnteredBy { get; set; }

        [View(DisplayNames.INITIAL_SERVICE_LINE_FLUSH_ENTERED_AT)]
        public virtual DateTime? InitialFlushTimeEnteredAt { get; set; }

        [View(DisplayNames.PITCHER_FILTER_PROVIDED_TO_CUSTOMER), Required]
        public virtual bool? HasPitcherFilterBeenProvidedToCustomer { get; set; }
        
        public virtual bool? IsThisAMultiTenantFacility { get; set; }
        
        public virtual int? NumberOfPitcherFiltersDelivered { get; set; }
        
        [StringLength(StringLengths.DESCRIBE_WHICH_UNITS)]
        public virtual string DescribeWhichUnits { get; set; }

        [View(DisplayNames.DATE_PITCHER_FILTER_DELIVERED_TO_CUSTOMER, FormatStyle.Date)]
        public virtual DateTime? DatePitcherFilterDeliveredToCustomer { get; set; }

        [View(DisplayNames.PITCHER_FILTER_CUSTOMER_DELIVERY_METHOD)]
        public virtual PitcherFilterCustomerDeliveryMethod PitcherFilterCustomerDeliveryMethod { get; set; }

        [View(DisplayNames.PITCHER_FILTER_DELIVERY_OTHER_METHOD)]
        public virtual string PitcherFilterCustomerDeliveryOtherMethod { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateCustomerProvidedAWStateLeadInformation { get; set; }

        [View(DisplayNames.PLANNED_COMPLETION_DATE, DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? PlannedCompletionDate { get; set; }
        
        #region Association Properties

        public virtual IList<Requisition> Requisitions { get; set; }
        public virtual IList<StreetOpeningPermit> StreetOpeningPermits { get; set; }
        public virtual IList<CrewAssignment> CrewAssignments { get; set; }
        public virtual IList<Markout> Markouts { get; set; }
        public virtual IList<MainBreak> MainBreaks { get; set; }
        public virtual IList<Restoration> Restorations { get; set; }
        public virtual IList<Spoil> Spoils { get; set; }
        public virtual CustomerImpactRange EstimatedCustomerImpact { get; set; }
        public virtual IList<ServiceInstallation> ServiceInstallations { get; set; }
        public virtual RepairTimeRange AnticipatedRepairTime { get; set; }
        public virtual Street Street { get; set; }
        public virtual Town Town { get; set; }

        [View(DisplayNames.WORK_DESCRIPTION)]
        public virtual WorkDescription WorkDescription { get; set; }

        public virtual Contractor AssignedContractor { get; set; }
        public virtual DateTime? AssignedToContractorOn { get; set; }

        public virtual AssetType AssetType { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User CompletedBy { get; set; }
        public virtual Hydrant Hydrant { get; set; }
        public virtual MarkoutRequirement MarkoutRequirement { get; set; }
        public virtual Street NearestCrossStreet { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual WorkOrderPurpose Purpose { get; set; }
        public virtual User RequestingEmployee { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual Valve Valve { get; set; }
        public virtual SewerOpening SewerOpening { get; set; }
        public virtual StormWaterAsset StormWaterAsset { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Service Service { get; set; }
        [View(DisplayNames.PREVIOUS_SERVICE_LINE_MATERIAL)]
        public virtual ServiceMaterial PreviousServiceLineMaterial { get; set; }
        [View(DisplayNames.PREVIOUS_SERVICE_LINE_SIZE)]
        public virtual ServiceSize PreviousServiceLineSize { get; set; }
        public virtual ServiceMaterial CustomerServiceLineMaterial { get; set; }
        public virtual ServiceSize CustomerServiceLineSize { get; set; }
        [View(DisplayNames.COMPANY_SERVICE_LINE_MATERIAL)]
        public virtual ServiceMaterial CompanyServiceLineMaterial { get; set; }
        [View(DisplayNames.COMPANY_SERVICE_LINE_SIZE)]
        public virtual ServiceSize CompanyServiceLineSize { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DoorNoticeLeftDate { get; set; }
        public virtual MainCrossing MainCrossing { get; set; }
        public virtual WorkOrderCancellationReason WorkOrderCancellationReason { get; set; }
        public virtual WorkOrder OriginalOrderNumber { get; set; }
        public virtual SAPWorkOrderStep SAPWorkOrderStep { get; set; }
        public virtual WorkOrderFlushingNoticeType FlushingNoticeType { get; set; }
        public virtual EchoshoreLeakAlert EchoshoreLeakAlert { get; set; }
        public virtual SmartCoverAlert SmartCoverAlert { get; set; }

        [View(DisplayNames.CONTRACTOR_NAME)]
        public virtual string ContractorName => AssignedContractor?.Name;

        //public virtual Service Service { get; set; }
        //public virtual IList<Material> Materials { get; set; }
        public virtual IList<TrafficControlTicket> TrafficControlTickets { get; set; }
        public virtual IList<MaterialUsed> MaterialsUsed { get; set; }
        public virtual IList<WorkOrderScheduleOfValue> WorkOrdersScheduleOfValues { get; set; }
        public virtual IList<JobSiteCheckList> JobSiteCheckLists { get; set; }
        public virtual IList<BelowGroundHazard> BelowGroundHazards { get; set; }
        public virtual IList<WorkOrderInvoice> Invoices { get; set; } = new List<WorkOrderInvoice>();
        public virtual IList<SewerOverflow> SewerOverflows { get; set; } = new List<SewerOverflow>();
        public virtual IList<MarkoutDamage> MarkoutDamages { get; set; } = new List<MarkoutDamage>();
        public virtual IList<MarkoutViolation> MarkoutViolations { get; set; } = new List<MarkoutViolation>();
        public virtual IList<JobObservation> JobObservations { get; set; } = new List<JobObservation>();

        #region Documents

        public virtual IList<WorkOrderDocument> WorkOrderDocuments { get; set; } = new List<WorkOrderDocument>();

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return WorkOrderDocuments.Cast<IDocumentLink>().ToList(); }
        }

        #endregion

        #endregion

        #region Logical Properties

        // WorkOrders still have Latitude/Longitude fields
        public virtual Coordinate Coordinate
        {
            get => new WorkOrderCoordinate(this);
            set
            {
                if (Asset != null)
                {
                    Asset.Coordinate = value;
                }
            }
        }

        public virtual MapIcon Icon
        {
            get
            {
                if (Coordinate == null)
                {
                    return null;
                }

                // TODO: I feel really gross about using a repository method inside a model.
                //       Also I just feel gross about this in general. - Ross 10/29/2041 -- Wow! The future! -Ross 3/22/2016
                var iconSet = _iconSetRepository.Find(IconSets.WorkOrders);
                if (iconSet == null)
                    return null;
                if (DateCompleted.HasValue)
                    return iconSet.Icons.Single(x => x.FileName == "MapIcons/construction_green.png");
                if (CrewAssignments.Count == 0)
                    return iconSet.Icons.Single(x => x.FileName == "MapIcons/construction_gray.png");
                if (CrewAssignments.Count(x => x.DateStarted.HasValue) == 0)
                    return iconSet.Icons.Single(x => x.FileName == "MapIcons/construction_yellow.png");
                return iconSet.Icons.Single(x => x.FileName == "MapIcons/construction_red.png");
            }
        }

        public virtual bool Completed { get; set; }
        public virtual int MonthCompleted { get; set; }
        public virtual int YearCompleted { get; set; }
        public virtual bool? StreetOpeningPermitRequested { get; set; }
        public virtual bool? StreetOpeningPermitIssued { get; set; }

        public virtual int? DaysSinceCompletion =>
            DateCompleted == null
                ? default(int?)
                : (DateTime.Now - DateCompleted.Value).Days;

        public virtual decimal TotalMaterialCost => (decimal)MaterialsUsed
                                                            .Where(x => x.Material != null && x.Cost.HasValue)
                                                            .Sum(x => x.Quantity * x.Cost);

        // NOTE: This property also exists in the 271 WorkOrder class and needs to be kept in sync.
        public virtual WorkOrderStatus Status
        {
            get
            {
                if (CancelledAt.HasValue)
                {
                    return WorkOrderStatus.Cancelled;
                }

                if (DateCompleted.HasValue)
                {
                    return WorkOrderStatus.Completed;
                }

                if (CurrentAssignment != null)
                {
                    if (CurrentAssignment.AssignedFor < DateTime.Today)
                    {
                        return WorkOrderStatus.ScheduledPreviously;
                    }

                    return WorkOrderStatus.ScheduledCurrently;
                }

                return WorkOrderStatus.Other;
            }
        }

        public virtual PlanningPlant DistributionPlanningPlant =>
            TownSection?.DistributionPlanningPlant ??
            Town?.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)
                ?.DistributionPlanningPlant ??
            OperatingCenter?.DistributionPlanningPlant;

        public virtual PlanningPlant SewerPlanningPlant =>
            TownSection?.SewerPlanningPlant ??
            Town?.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)
                ?.SewerPlanningPlant ??
            OperatingCenter?.SewerPlanningPlant;

        public virtual FunctionalLocation MainSAPFunctionalLocation =>
            TownSection?.MainSAPFunctionalLocation ??
            Town?.OperatingCentersTowns?.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)
                ?.MainSAPFunctionalLocation;

        public virtual int? MainSAPEquipmentId =>
            TownSection?.MainSAPEquipmentId ??
            Town?.OperatingCentersTowns?.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)
                ?.MainSAPEquipmentId;

        public virtual FunctionalLocation SewerMainSAPFunctionalLocation =>
            TownSection?.SewerMainSAPFunctionalLocation ??
            Town?.OperatingCentersTowns?.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)
                ?.SewerMainSAPFunctionalLocation;

        public virtual int? SewerMainSAPEquipmentId =>
            TownSection?.SewerMainSAPEquipmentId ??
            Town?.OperatingCentersTowns?.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)
                ?.SewerMainSAPEquipmentId;

        public virtual PlantMaintenanceActivityType PlantMaintenanceActivityType
        {
            get
            {
                return PlantMaintenanceActivityTypeOverride ??
                       OperatingCenter?.State?.WorkDescriptionOverrides
                                      ?.SingleOrDefault(x => x.WorkDescription.Id == WorkDescription.Id)
                                      ?.PlantMaintenanceActivityType ??
                       WorkDescription?.PlantMaintenanceActivityType;
            }
        }

        public virtual bool IsEnabled
        {
            get
            {
                if (StreetOpeningPermitRequired &&
                    (string.IsNullOrWhiteSpace(CurrentStreetOpeningPermit?.StreetOpeningPermitNumber)
                     || CurrentStreetOpeningPermit.DateRequested == DateTime.MinValue
                     || CurrentStreetOpeningPermit.DateIssued == null
                     || (!string.IsNullOrWhiteSpace(CurrentStreetOpeningPermit?.StreetOpeningPermitNumber)
                         && CurrentStreetOpeningPermit.ExpirationDate != null
                         && DateTime.Compare(CurrentStreetOpeningPermit.ExpirationDate.Value, DateTime.Now) < 0)))
                {
                    return Priority.Id == (int)WorkOrderPriority.Indices.EMERGENCY && MarkoutRequirement.Id == (int)MarkoutRequirementEnum.Emergency;
                }

                return (OperatingCenter != null && !OperatingCenter.SAPWorkOrdersEnabled) || (SAPNotificationNumber != null && SAPWorkOrderNumber != null);
            }
        }

        public virtual string ArcCollectorLink
        {
            get
            {
                switch (AssetType?.Id)
                {
                    case AssetType.Indices.HYDRANT:
                        return ArcCollectorLinkGenerator.ArcCollectorHydrantLink(Hydrant, AssetType, this);
                    case AssetType.Indices.SEWER_OPENING:
                        return ArcCollectorLinkGenerator.ArcCollectorSewerOpeningLink(SewerOpening, AssetType, this);
                    case AssetType.Indices.VALVE:
                        return ArcCollectorLinkGenerator.ArcCollectorValveLink(Valve, AssetType, this);
                    default:
                        return ArcCollectorLinkGenerator.ArcCollectorLink(new NameValueCollection {
                            {"referenceContext", "center"},
                            {"itemID", OperatingCenter?.ArcMobileMapId},
                            {"center", $"{Latitude},{Longitude}"},
                            {"scale", "3000"}
                        }, this);
                }
            }
        }

        [DoesNotExport]
        public virtual string TableName => WorkOrderMap.TABLE_NAME;

        public virtual State State => Town?.State;

        /// <summary>
        /// This determines if we show the Set Meter tab on the MVC and Contractors Finalization and General Views
        /// There is similar functionality in both ProgressSAPWorkOrder classes to determine if the data should flow to SAP
        /// which should be considered if you are making changes here.
        /// </summary>
        public virtual bool IsNewServiceInstallation => OperatingCenter != null
                                                        && OperatingCenter.SAPEnabled
                                                        && OperatingCenter.SAPWorkOrdersEnabled
                                                        && !OperatingCenter.IsContractedOperations
                                                        && AssetType != null && AssetType.Id == AssetType.Indices.SERVICE
                                                        && WorkDescription != null
                                                        && WorkDescription.NEW_SERVICE_INSTALLATION.Contains(WorkDescription.Id)
                                                        && PremiseNumber != "0000000000";

        public virtual bool CanBeFinalized =>
            !(AssetType?.Id == AssetType.Indices.VALVE
              && (Valve.Status.Id == AssetStatus.Indices.RETIRED
                  || Valve.Status.Id == AssetStatus.Indices.REMOVED
                  || Valve.Status.Id == AssetStatus.Indices.CANCELLED))
            &&
            !(AssetType?.Id == AssetType.Indices.HYDRANT
              && (Hydrant.Status.Id == AssetStatus.Indices.RETIRED
                  || Hydrant.Status.Id == AssetStatus.Indices.REMOVED
                  || Hydrant.Status.Id == AssetStatus.Indices.CANCELLED))
            &&
            !(AssetType?.Id == AssetType.Indices.SEWER_OPENING
              && (SewerOpening.Status.Id == AssetStatus.Indices.RETIRED
                  || SewerOpening.Status.Id == AssetStatus.Indices.REMOVED
                  || SewerOpening.Status.Id == AssetStatus.Indices.CANCELLED)
                );

        public virtual string ActuallyCompletedBy 
        {
            get
            {
                if (!DateCompleted.HasValue)
                {
                    return string.Empty;
                }

                return CompletedBy?.FullName ?? AssignedContractor?.Name;
            }
        }

        /// <summary>
        /// Used in a number of controller actions and view models to determine if an
        /// order should be passed to SAP.
        /// Returns true if the OperatingCenter this WorkOrder is associated with
        /// supports being updated in SAP. It also requires that an order wasn't cancelled
        /// without either the SAP Notification Number or Work Order number.
        ///
        /// It does not imply that the values in this WorkOrder are ready for SAP.
        /// </summary>
        [DoesNotExport]
        public virtual bool IsSAPUpdatableWorkOrder =>
            OperatingCenter != null &&
            OperatingCenter.SAPEnabled &&
            OperatingCenter.SAPWorkOrdersEnabled &&
            !OperatingCenter.IsContractedOperations &&
            (CancelledAt == null || (CancelledAt != null && (SAPNotificationNumber.HasValue || SAPWorkOrderNumber.HasValue)));

        [DoesNotExport]
        public virtual bool HasRealSAPError =>
            IsSAPUpdatableWorkOrder &&
            !string.IsNullOrWhiteSpace(SAPErrorCode) &&
            !SAPErrorCode.ToUpper().Contains("SUCCESS");

        #region Supervisor Approval 

        public virtual User ApprovedBy { get; set; }

        [View(DisplayNames.APPROVED_ON)]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? ApprovedOn { get; set; }

        /// <summary>
        /// Returns true if the work order is ready to be approved by a supervisor.
        /// </summary>
        public virtual bool CanBeApproved
        {
            get
            {
                // NOTE: If you make any changes to this property
                // or the properties used within this property, make sure you are
                // also adding/updating the appropriate validation messages in
                // MVC's SupervisorApproveWorkOrder class.

                // NOTE 2: This property also includes the logic used to filter
                // for supervisor approval orders in the repository. Without this,
                // this value will likely be invalid during non-supervisor related
                // queries.
                if (CancelledAt.HasValue || !DateCompleted.HasValue || !IsSAPValid)
                {
                    return false;
                }
               
                // This part is the old logic for CanBeApproved that exists in 271.
                return !(HasServiceApprovalIssue
                         || HasInvestigativeWorkDescriptionApprovalIssue
                         || HasSAPNotReleased
                         || HasAssetTypeError);
            }
        }
        /// <summary>
        /// This property must match the same logic as the SAPValidCriteria property
        /// on WorkOrderRepository. Also this name is bad and should be better.
        /// </summary>
        public virtual bool IsSAPValid => OperatingCenter != null && 
                                          (OperatingCenter.SAPWorkOrdersEnabled == false 
                                           || OperatingCenter.IsContractedOperations == true 
                                           || SAPWorkOrderNumber != null);

        public virtual bool HasServiceApprovalIssue =>
            AssetType?.Id == AssetType.Indices.SERVICE
            && (WorkDescription != null && WorkDescriptionRepository.SERVICE_APPROVAL_WORK_DESCRIPTIONS.Contains(WorkDescription.Id))
            && (Service == null || !Service.DateInstalled.HasValue);
        
        public virtual bool HasInvestigativeWorkDescriptionApprovalIssue =>
            WorkDescription != null && WorkDescriptionType.INVESTIGATIVE.Contains(WorkDescription.Id);

        public virtual bool HasSAPNotReleased
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SAPErrorCode))
                {
                    return false;
                }
                var errorCode = SAPErrorCode.ToUpper();
                return errorCode.Contains("NOT RELEASED") || errorCode.Contains("RELEASE REJECTED");
            }
        }
        
        public virtual bool HasAssetTypeError => AssetType != WorkDescription?.AssetType;

        #endregion

        #region Asset

        public virtual IAsset Asset
        {
            get
            {
                switch (AssetType?.Id)
                {
                    case AssetType.Indices.VALVE:
                        return Valve;
                    case AssetType.Indices.HYDRANT:
                        return Hydrant;
                    case AssetType.Indices.EQUIPMENT:
                        return Equipment;
                    case AssetType.Indices.SEWER_OPENING:
                        return SewerOpening;
                    case AssetType.Indices.STORM_CATCH:
                        return StormWaterAsset;
                    case AssetType.Indices.MAIN_CROSSING:
                        return MainCrossing;
                    case AssetType.Indices.SERVICE:
                        return Service;
                    default:
                        return null;
                }
            }
        }

        public virtual WorkOrderAssetId AssetId { get; set; }

        public virtual string AssetKey
        {
            get
            {
                switch (AssetType.AssetTypeEnum)
                {
                    case AssetTypeEnum.Valve:
                        return Valve.ValveNumber;
                    case AssetTypeEnum.Hydrant:
                        return Hydrant.HydrantNumber;
                    case AssetTypeEnum.SewerOpening:
                        return SewerOpening.Id.ToString();
                    case AssetTypeEnum.StormCatch:
                        return StormWaterAsset.Id.ToString();
                    case AssetTypeEnum.Service:
                    case AssetTypeEnum.SewerLateral:
                        return String.Format(FormatStrings.PREMISE_SERVICE, PremiseNumber, ServiceNumber);
                    default:
                        return null;
                }
            }
        }

        public virtual string AssetCriticalNotes
        {
            get
            {
                switch (AssetType?.AssetTypeEnum)
                {
                    case AssetTypeEnum.Hydrant:
                        return Hydrant.CriticalNotes;
                    case AssetTypeEnum.Valve:
                        return Valve.CriticalNotes;
                    case AssetTypeEnum.SewerOpening:
                        return SewerOpening.CriticalNotes;
                    case AssetTypeEnum.Equipment:
                        return Equipment.CriticalNotes;
                    case AssetTypeEnum.MainCrossing:
                        return MainCrossing.IsCriticalAsset.HasValue ? MainCrossing.Comments : string.Empty;
                    default:
                        return null;
                }
            }
        }

        public virtual IList<WorkOrder> RelatedWorkOrders
        {
            get
            {
                switch (AssetType?.AssetTypeEnum)
                {
                    case AssetTypeEnum.Hydrant:
                        return Hydrant?.WorkOrders;
                    case AssetTypeEnum.Valve:
                        return Valve?.WorkOrders;
                    case AssetTypeEnum.SewerOpening:
                        return SewerOpening?.WorkOrders;
                    case AssetTypeEnum.Equipment:
                        return Equipment?.WorkOrders;
                    case AssetTypeEnum.MainCrossing:
                        return MainCrossing?.WorkOrders;
                    case AssetTypeEnum.Service:
                    case AssetTypeEnum.SewerLateral:
                        return Service?.WorkOrders;
                    default:
                        return new List<WorkOrder>();
                }
            }
        }

        #endregion

        #region Markouts

        public virtual bool MarkoutRequired => MarkoutRequirement?.MarkoutRequirementEnum != MarkoutRequirementEnum.None;

        public virtual CurrentMarkout CurrentMarkout { get; set; }

        public virtual Markout LastMarkout
        {
            get
            {
                var marks = Markouts.OrderByDescending(x => x.DateOfRequest);
                return marks.Count() > 1 ? marks.ToArray()[1] : null;
            }
        }

        public virtual DateTime? MarkoutExpirationDate
        {
            get
            {
                if (_markoutExpirationDate == null && CurrentMarkout?.Markout != null)
                    _markoutExpirationDate = CurrentMarkout.Markout.ExpirationDate;
                return _markoutExpirationDate;
            }
        }

        #endregion

        #region Processing Times

        public virtual TimeSpan? OrderProcessTime => DateCompleted?.Subtract(DateReceived.Value);

        public virtual TimeSpan? SupervisorProcessTime => ApprovedOn?.Subtract(DateCompleted.Value);

        public virtual TimeSpan? StockProcessTime => MaterialsApprovedOn?.Subtract(ApprovedOn.Value);

        #endregion

        #endregion

        #region Location/Address

        public virtual string StreetAddress =>
            (String.Format(FormatStrings.STREET_ADDRESS, StreetNumber, Street.FullStName));

        public virtual string TownAddress => String.Format(FormatStrings.TOWN_ADDRESS, Town, Town.State, ZipCode);

        #endregion

        #region Crew Assignments

        public virtual bool WorkStarted => (from ca in CrewAssignments
                                            where ca.DateStarted <= DateTime.Now
                                            select ca).Any();

        public virtual CurrentAssignment CurrentAssignment { get; set; }

        [View("Last Assigned To")]
        public virtual Crew CurrentCrew
        {
            get
            {
                if (CurrentAssignment != null)
                    return CurrentAssignment.Crew;
                return null;
            }
        }

        public virtual bool HasOpenAssignments
        {
            get { return CrewAssignments.Any(assignment => assignment.IsOpen); }
        }

        public virtual float? TotalManHours
        {
            get
            {
                return HasOpenAssignments
                    ? (float?)null
                    : CrewAssignments
                     .Where(assignment => assignment.TotalManHours.HasValue)
                     .Sum(assignment => assignment.TotalManHours.Value);
            }
        }

        public virtual StreetOpeningPermit CurrentStreetOpeningPermit
        {
            get
            {
                return
                    StreetOpeningPermits.OrderByDescending(x => x.DateRequested)
                                        .FirstOrDefault();
            }
        }

        #endregion

        #region Main Break

        public virtual bool IsMainBreak()
        {
            return WorkDescription != null && WorkDescriptionRepository.MAIN_BREAKS.Contains(WorkDescription.Id);
        }

        #endregion

        #region Sewer Overflow

        public virtual bool IsSewerOverflow()
        {
            return WorkDescription != null &&
                   (WorkDescription.Id == (int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW ||
                    WorkDescription.Id == (int)WorkDescription.Indices.SEWER_SERVICE_OVERFLOW);
        }

        #endregion

        #region Permit

        //public virtual StreetOpeningPermit CurrentStreetOpeningPermit
        //{
        //    get
        //    {
        //        return
        //            StreetOpeningPermits.OrderByDescending(x => x.DateRequested)
        //                .FirstOrDefault();
        //    }
        //}

        #endregion
        
        public virtual bool HasPendingAssignments { get; set; }
        public virtual bool? HasInvoice { get; set; }
        public virtual bool? HasSampleSite { get; set; }
        public virtual bool? HasJobSiteCheckLists { get; set; }
        public virtual bool IsConfinedSpaceFormRequired { get; set; }
        public virtual bool? HasPreJobSafetyBriefs { get; set; }
        // This property is used in Smart Cover Alert Show page - Work Orders tab
        [DoesNotExport]
        public virtual DateTime? WorkOrderDateStarted =>
            CrewAssignments.OrderBy(x => x.Id).FirstOrDefault()?.DateStarted;
        /// <summary>
        /// A terrible terrible hack property for passing the url
        /// for this record to a notification template.
        /// </summary>
        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [DoesNotExport]
        public virtual IList<SampleSite> SampleSites { get; set; }

        [DoesNotExport]
        public virtual SampleSite SampleSite => SampleSites.FirstOrDefault();

        [DoesNotExport, View(FormatStyle.Date)]
        public virtual DateTime? DateMarkoutNeeded
        {
            get
            {
                if (MarkoutToBeCalled != null)
                {
                    return WorkOrdersWorkDayEngine.GetRoutineReadyDate((DateTime)MarkoutToBeCalled);
                }

                return null;
            }
        }

        [DoesNotExport]
        public virtual MarkoutType MarkoutTypeNeeded { get; set; }

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        public virtual string PWSID => Premise?.PublicWaterSupply?.Identifier;
        [DoesNotExport]
        public virtual Premise Premise { get; set; }

        public virtual MeterLocation MeterLocation { get; set; }
        
        [DoesNotExport]
        public virtual DateTime? RecentPitcherFilterDeliveryDate =>
            Premise?.RecentPitcherFilterDeliveryDate;

        #endregion

        #region Constructors

        public WorkOrder()
        {
            Requisitions = new List<Requisition>();
            StreetOpeningPermits = new List<StreetOpeningPermit>();
            CrewAssignments = new List<CrewAssignment>();
            Markouts = new List<Markout>();
            MainBreaks = new List<MainBreak>();
            Restorations = new List<Restoration>();
            ServiceInstallations = new List<ServiceInstallation>();
            TrafficControlTickets = new List<TrafficControlTicket>();
            MaterialsUsed = new List<MaterialUsed>();
            WorkOrdersScheduleOfValues = new List<WorkOrderScheduleOfValue>();
            SampleSites = new List<SampleSite>();
            Spoils = new List<Spoil>();
            JobSiteCheckLists = new List<JobSiteCheckList>();
            BelowGroundHazards = new List<BelowGroundHazard>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual void AppendNotes(ContractorUser user, DateTime date, string additionalNotes)
        {
            if (!string.IsNullOrWhiteSpace(Notes))
            {
                Notes += Environment.NewLine;
            }

            Notes += String.Format(APPEND_NOTES_FORMAT,
                user.Contractor.Name,
                user.Email, date, additionalNotes);
        }

        public virtual void AppendNotes(User userName, DateTime date, string additionalNotes)
        {
            var authoredNotes =
                $"{userName.FullName} {date.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS)} {additionalNotes}";
            
            if (string.IsNullOrWhiteSpace(Notes))
            {
                Notes = authoredNotes;
            }
            else
            {
                Notes += $"{Environment.NewLine}{authoredNotes}";
            }
        }

        public override string ToString()
        {
            return Id.ToString(CultureInfo.InvariantCulture);
        }

        public virtual object ToJSONObject()
        {
            return new {
                Id,
                OperatingCenter = OperatingCenter.Id,
                Town = Town.Id,
                TownSection = TownSection?.Id,
                StreetNumber,
                Street = Street.Id,
                AssetType = AssetType.Id,
                Valve = Valve?.Id,
                Hydrant = Hydrant?.Id,
                SewerOpening = SewerOpening?.Id,
                StormCatch = StormWaterAsset?.Id,
                PremiseNumber,
                ServiceNumber,
                NearestCrossStreet = NearestCrossStreet?.Id,
                ZipCode,
                WorkDescription = WorkDescription?.Id,
                MarkoutRequirement,
                StreetOpeningPermitRequired,
                Priority
            };
        }

        #endregion
    }

    /// <summary>
    /// This is used to put colors in tables. If you want to add some statuses, go nuts,
    /// but make sure to update the WORK_ORDER_STATUS_DISPLAY_NAMES constant also. 
    /// </summary>
    public enum WorkOrderStatus
    {
        Other = 0, // here for statuses that don't need no colors
        ScheduledPreviously = 1, // yellow
        ScheduledCurrently = 2, // blue
        Cancelled = 3, // orange
        Completed = 4, // green
        RequiresSupervisorApproval = 5, // purple, only used on production work orders at the moment
        WithCompliance = 6 // red, only used on production work orders at the moment
    }

    public class MainBreaksAndServiceLineRepairsViewModel
    {
        #region Properties

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual WorkDescription WorkDescription { get; set; }
        public virtual int Month { get; set; }
        public virtual int MonthCompleted { get; set; }
        public virtual int Year { get; set; }

        #endregion
    }

    public class MainBreaksAndServiceLineRepairsReportViewModel : MonthlyReportViewModel
    {
        #region Properties

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual WorkDescription WorkDescription { get; set; }

        #endregion
    }

    public class MonthlyReportViewModel
    {
        #region Properties

        public virtual int Year { get; set; }
        public virtual int? Jan { get; set; }
        public virtual int? Feb { get; set; }
        public virtual int? Mar { get; set; }
        public virtual int? Apr { get; set; }
        public virtual int? May { get; set; }
        public virtual int? Jun { get; set; }
        public virtual int? Jul { get; set; }
        public virtual int? Aug { get; set; }
        public virtual int? Sep { get; set; }
        public virtual int? Oct { get; set; }
        public virtual int? Nov { get; set; }
        public virtual int? Dec { get; set; }

        public virtual int? Total => Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec;

        #endregion
    }

    public class WorkOrderCoordinate : Coordinate
    {
        #region Constructors

        public WorkOrderCoordinate(WorkOrder order)
        {
            Latitude = order.Latitude.GetValueOrDefault();
            Longitude = order.Longitude.GetValueOrDefault();
        }

        #endregion
    }

    [Serializable]
    public class CustomerImpactRange : EntityLookup
    {
        public struct Indices
        {
            public const int ZERO_TO_FIFTY = 1,
                             FIFTY_ONE_TO_ONE_HUNDRED = 2,
                             ONE_HUNDRED_ONE_TO_TWO_HUNDRED = 3,
                             GREATER_THAN_TWO_HUNDRED = 4;
        }
    }

    [Serializable]
    public class RepairTimeRange : EntityLookup
    {
        public struct Indices
        {
            public const int FOUR_TO_SIX = 1,
                             EIGHT_TO_TEN = 2,
                             TEN_TO_TWELVE = 3;
        }
    }

    [Serializable]
    public class WorkOrderPriority : EntityLookup
    {
        public enum Indices
        {
            EMERGENCY = 1,
            HIGH_PRIORITY = 2,
            ROUTINE = 4
        }

        public virtual WorkOrderPriorityEnum WorkOrderPriorityEnum =>
            (WorkOrderPriorityEnum)Id;
    }

    public interface IAsset : IValidatableObject, IThingWithCoordinate, IThingWithOperatingCenter
    {
        string Identifier { get; }
    }

    public interface IRetirableWorkOrderAsset
    {
        DateTime? DateRetired { get; }
        IList<WorkOrder> WorkOrders { get; }
    }

    public class WorkOrderNotification
    {
        public WorkOrder WorkOrder { get; set; }
        public string UserEmail { get; set; }
    }

    public interface ISapWorkOrder
    {
        string AccountCharged { get; set; }
        long? SAPWorkOrderNumber { get; set; }
        long? SAPNotificationNumber { get; set; }
        string SAPErrorCode { get; set; }
        string MaterialsDocID { get; set; }
        string BusinessUnit { get; set; }
    }
}
