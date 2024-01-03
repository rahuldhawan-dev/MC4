using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.Exceptions;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text.RegularExpressions;
using WorkOrders.Library;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkOrders")]
    public class WorkOrder : IVerifiesAddress, INotifyPropertyChanging, INotifyPropertyChanged, ISapWorkOrder
    {
        #region Constants

        private const short MAX_CUSTOMERNAME_LENGTH = 30,
                            MAX_STREETNUMBER_LENGTH = 20,
                            MAX_APARTMENTADDTL_LENGTH = 30,
                            MAX_PHONENUMBER_LENGTH = 14,
                            MAX_SECONDARYPHONENUMBER_LENGTH = 14,
                            MAX_CUSTOMERACCOUNTNUMBER_LENGTH = 11,
                            MAX_SERVICENUMBER_LENGTH = 50,
                            MAX_ACCOUNTCHARGED_LENGTH = 30,
                            MAX_PREMISENUMBER_LENGTH = 10,
                            MIN_PREMISENUMBER_LENGTH = 8,
                            MAX_INVOICENUMBER_LENGTH = 15,
                            MAX_ZIP_CODE_LENGTH = 10,
                            MAX_ORCOM_SERVICE_ORDER_NUMBER_LENGTH = 20,
                            MAX_ALERT_ID_LENGTH = 20,
                            MAX_MATERIALS_DOC_ID_LENGTH = 15,
                            MAX_BUSINESS_UNIT_LENGTH = 256;

        private string PREMISE_NUMBER_PLACEHOLDER = "0000000000";

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _customerName,
                       _streetNumber,
                       _apartmentAddtl,
                       _secondaryPhoneNumber,
                       _phoneNumber,
                       _customerAccountNumber,
                       _serviceNumber,
                       _accountCharged,
                       _premiseNumber,
                       _meterSerialNumber,
                       _invoiceNumber,
                       _notes,
                       _zipCode,
                       _orcomServiceOrderNumber,
                       _materialsDocID,
                       _businessUnit,
                       _alertID,
                       _requiredMarkoutNote,
                        _sapErrorCode,
                       _pitcherFilterCustomerDeliveryOtherMethod;

        private int _assetTypeID,
                    _workOrderID,
                    _purposeID,
                    _requesterID,
                    _markoutRequirementID,
                    _workDescriptionID,
                    _creatorID,
                    _townID,
                    _priorityID;

        private DateTime _createdOn;

        private DateTime? _dateReceived,
                          _dateStarted,
                          _dateCompleted,
                          _datePrinted,
                          _dateReportSent,
                          _excavationDate,
                          _dateCompletedPC,
                          _markoutExpirationDate,
                          _materialsApprovedOn,
                          _approvedOn,
                          _OfficeAssignedOn,
                          _markoutToBeCalled,
                          _assignedToContractorOn,
                          _alertStarted,
                          _doorNoticeLeftDate,
                          _cancelledAt,
                          _materialPlanningCompletedOn,
                          _materialPostingDate,
                          _dateRejected,
                          _initialFlushTimeEnteredAt,
                          _datePitcherFilterDeliveredToCustomer,
                          _dateCustomerProvidedAWStateLeadInformation;

        private int? _streetID,
                     _nearestCrossStreetID,
                     _townSectionID,
                     _approvedByID,
                     _backhoeOperator,
                     _requestingEmployeeID,
                     _valveID,
                     _hydrantID,
                     _mainCrossingID,
                     _sewerOpeningID,
                     _stormCatchID,
                     _equipmentID,
                     _lostWater,
                     _numberOfOfficersRequired,
                     _oldWorkOrderNumber,
                     _operatingCenterID,
                     _materialsApprovedByID,
                     _completedByID,
                     _officeAssignmentID,
                     _originalOrderNumber,
                     _customerImpactRangeID,
                     _repairTimeRangeID,
                     _markoutTypeNeededID,
                     _assignedContractorID,
                     _previousServiceLineMaterialID,
                     _previousServiceLineSizeID,
                     _customerServiceLineMaterialID,
                     _customerServiceLineSizeID,
                     _companyServiceLineMaterialID,
                     _companyServiceLineSizeID,
                     _flushingNoticeTypeId,
                     _serviceID,
                     _cancelledByID,
                     _workOrderCancellationReasonID,
                     _sapWorkOrderStepID,
                     _plantMaintenanceActivityTypeOverrideID,
                     _acousticMonitoringTypeId,
                     _echoshoreLeakAlertId,
                     _initialServiceLineFlushTime,
                     _initialFlushTimeEnteredById,
                     _pitcherFilterCustomerDeliveryMethodId;

        private long? _sapNotificationNumber,
                      _sapWorkOrderNumber,
                      _deviceLocation,
                      _installation,
                      _sapEquipmentNumber;
        
        private double? _latitude, _longitude, _distanceFromCrossStreet;

        private bool _trafficControlRequired, _streetOpeningPermitRequired, _digitalAsBuiltRequired;

        private bool? _significantTrafficImpact,
                      _updatedMobileGIS,
                      _alertIssued,
                      _requiresInvoice,
                      _digitalAsBuiltCompleted,
                      _hasPitcherFilterBeenProvidedToCustomer;

        private Asset _asset;

        private EntityRef<AssetType> _assetType;

        private EntityRef<Street> _street, _nearestCrossStreet;

        private EntityRef<Town> _tblNJAWTownName;

        private EntityRef<TownSection> _tblNJAWTwnSection;

        private EntityRef<WorkOrderPriority> _priority;

        private EntityRef<WorkOrderPurpose> _drivenBy;

        private EntityRef<WorkOrderRequester> _requestedBy;

        private EntityRef<PitcherFilterCustomerDeliveryMethod> _pitcherFilterCustomerDeliveryMethod;

        private EntityRef<Employee> _requestingEmployee,
                                    _createdBy,
                                    _materialsApprovedBy,
                                    _approvedBy,
                                    _completedBy,
                                    _officeAssignment,
                                    _cancelledBy,
                                    _initialFlushTimeEnteredBy;

        private EntityRef<Contractor> _assignedContractor; 

        private EntityRef<WorkDescription> _workDescription;

        private EntityRef<MarkoutRequirement> _markoutRequirement;

        private EntityRef<Valve> _valve;

        private EntityRef<Hydrant> _hydrant;
        private EntityRef<Service> _service;

        private EntityRef<MainCrossing> _mainCrossing;

        private EntityRef<SewerOpening> _sewerOpening;

        private EntityRef<StormCatch> _stormCatch;

        private EntityRef<Equipment> _equipment; 

        private EntityRef<OperatingCenter> _operatingCenter;

        private EntityRef<WorkOrder> _originalOrder;

        private EntityRef<CustomerImpactRange> _customerImpactRange;

        private EntityRef<AcousticMonitoringType> _acousticMonitoringType;

        private EntityRef<ServiceMaterial> _previousServiceLineMaterial, _customerServiceLineMaterial, _companyServiceLineMaterial;
        private EntityRef<ServiceSize> _previousServiceLineSize, _customerServiceLineSize, _companyServiceLineSize;
        private EntityRef<WorkOrderCancellationReason>_workOrderCancellationReason;
        private EntityRef<PlantMaintenanceActivityType> _plantMaintenanceActivityTypeOverride;
        private EntityRef<SAPWorkOrderStep> _sapWorkOrderStep;
        
        private readonly EntitySet<EmployeeWorkOrder> _employeeWorkOrders;

        private readonly EntitySet<LostWater> _lostWaters;

        private readonly EntitySet<MainBreak> _mainBreaks;

        private readonly EntitySet<MaterialsUsed> _materialsUseds;

        private readonly EntitySet<SafetyMarker> _safetyMarkers;

        private readonly EntitySet<DetectedLeak> _detectedLeaks;

        private readonly EntitySet<Markout> _markouts;

        private readonly EntitySet<Requisition> _requisitions; 

        private readonly EntitySet<StreetOpeningPermit> _streetOpeningPermits;

        private readonly EntitySet<WorkOrderDescriptionChange> _workOrderDescriptionChanges;

        private readonly EntitySet<CrewAssignment> _crewAssignments;

        private readonly EntitySet<Restoration> _restorations;
        
        private readonly EntitySet<OrcomOrderCompletion> _orcomOrderCompletions;

        private readonly EntitySet<Spoil> _spoils;

        private readonly EntitySet<DocumentWorkOrder> _documentsWorkOrders;

        private readonly EntitySet<WorkOrder> _childOrders;

        private readonly EntitySet<JobSiteCheckList> _jobSiteCheckLists;

        private readonly EntitySet<WorkOrderInvoice> _workOrderInvoices; 

        private readonly EntitySet<JobObservation> _jobObservations;

        private readonly EntitySet<TrafficControlTicket> _trafficControl;

        private readonly EntitySet<MarkoutViolation> _markoutViolations;

        private readonly EntitySet<NewServiceInstallation> _newServiceInstallation;

        private readonly EntitySet<WorkOrderScheduleOfValue> _workOrdersScheduleOfValues;
        
        private EntityRef<RepairTimeRange> _repairTimeRange;

        private EntityRef<MarkoutType> _markoutTypeNeeded;

        private EntityRef<FlushingNoticeType> _flushingNoticeType;

        #endregion

        #region Properties

        #region Logical Properties

        public int Id => WorkOrderID;

        public string StreetAddress =>
            (StreetNumber + " ")
            + (Street.Prefix == null || Street.Prefix.Description.Length == 0 ? "" : Street.Prefix.Description + " ")
            + Street.StreetName 
            + (Street.Suffix == null || Street.Suffix.Description.Length==0? "" :" " + Street.Suffix.Description);

        public string TownAddress => String.Format("{0}, {1} {2}", Town, Town.State.Abbreviation, ZipCode);

        public Markout CurrentMarkout => Markouts.GetCurrent();

        public DateTime? MarkoutExpirationDate
        {
            get
            {
                if (_markoutExpirationDate == null && CurrentMarkout != null)
                    _markoutExpirationDate = CurrentMarkout.ExpirationDate;
                return _markoutExpirationDate;
            }
        }

        public Markout LastMarkout => Markouts.GetLast();

        public Crew CurrentlyAssignedCrew => CrewAssignments.GetCurrentCrew();

        public DateTime? LastAssignedDate
            => CrewAssignments.GetCurrent()?.AssignedFor;

        public CrewAssignment CurrentAssignment => CrewAssignments.GetCurrent();

        public Asset Asset
        {
            get
            {
                //TODO: Fix, hacked 20090309 ARR, 20090922 ARR
                if (_asset == null &&
                    new int[] {
                        AssetTypeRepository.Indices.HYDRANT,
                        AssetTypeRepository.Indices.VALVE,
                        AssetTypeRepository.Indices.SEWER_OPENING,
                        AssetTypeRepository.Indices.STORM_CATCH,
                        AssetTypeRepository.Indices.EQUIPMENT,
                        AssetTypeRepository.Indices.MAIN_CROSSING
                    }.Contains(_assetTypeID))
                    _asset = GetAsset();
                return _asset;
            }
        }

        public string AssetCriticalNotes
        {
            get
            {
                var ret = "";
                switch (AssetType?.TypeEnum)
                {
                    //case AssetTypeEnum.Equipment:
                    //    return Equipment.
                    case AssetTypeEnum.Hydrant:
                        ret = Hydrant.CriticalNotes;
                        break;
                    case AssetTypeEnum.Valve:
                        ret = Valve.CriticalNotes;
                        break;
                    case AssetTypeEnum.SewerOpening:
                        ret = SewerOpening.CriticalNotes;
                        break;
                    case AssetTypeEnum.Equipment:
                        ret = Equipment.CriticalNotes;
                        break;
                    case AssetTypeEnum.MainCrossing:
                        ret = MainCrossing.IsCriticalAsset.HasValue ? MainCrossing.Comments : string.Empty;
                        break;
                }
                return ret ?? string.Empty;
            }
        }

        private bool? _isPremiseLinkedToSampleSite;
        private SampleSite _sampleSite;

        public bool IsPremiseLinkedToSampleSite
        {
            get
            {
                if (!_isPremiseLinkedToSampleSite == null)
                {
                    _isPremiseLinkedToSampleSite = SampleSiteRepository.IsPremisedLinkedToSampleSite(PremiseNumber);
                }
                return _isPremiseLinkedToSampleSite.Value;
            }
            set => _isPremiseLinkedToSampleSite = value;
        }
        
        public SampleSite SampleSite
        {
            get
            {
                if (_sampleSite == null)
                {
                    _sampleSite = SampleSiteRepository.LinkedToSampleSite(PremiseNumber);
                }
                return _sampleSite;
            }
            set => _sampleSite = value;
        }

        public bool HasCriticalNotes => !string.IsNullOrWhiteSpace(AssetCriticalNotes);

        public string AssetID
        {
            get
            {
                switch (AssetType.TypeEnum)
                {
                    case AssetTypeEnum.Hydrant:
                    case AssetTypeEnum.Valve:
                    case AssetTypeEnum.SewerOpening:
                    case AssetTypeEnum.StormCatch:
                    case AssetTypeEnum.Equipment:
                    case AssetTypeEnum.MainCrossing:
                        return Asset.AssetID;
                    case AssetTypeEnum.Service:
                    case AssetTypeEnum.SewerLateral:
                        return String.Format("p#:{0}, s#:{1}", PremiseNumber, ServiceNumber);
                    case AssetTypeEnum.Main:
                    case AssetTypeEnum.SewerMain:
                        return null;
                    default:
                        return null;
                }
            }
        }

        public int? SAPEquipmentID
        {
            get
            {
                var ret = Asset != null ? Asset.SAPEquipmentID : null;
                if (ret == null)
                {
                    if (AssetType.TypeEnum == AssetTypeEnum.SewerMain)
                        ret = Town.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)?.SewerMainSAPEquipmentID;
                    if (AssetType.TypeEnum == AssetTypeEnum.Main)
                        ret = Town.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter == OperatingCenter && x.Town == Town)?.MainSAPEquipmentID;
                }
                return ret;
            }
        }

        public object AssetKey => (Asset == null) ? null : Asset.AssetKey;

        // TODO:
        // this may need to become a real bit field in the db :(
        public virtual bool MarkoutRequired =>
            MarkoutRequirement.RequirementEnum !=
            MarkoutRequirementEnum.None;

        public string MarkoutRequiredStr => MarkoutRequired.ToString("yn");

        // TODO:
        // this may need to become a real FK field in the db :(
        public WorkOrderPhase? Phase
        {
            get
            {
                if (MeetsInputRequirements())
                {
                    if (MeetsPlanningRequirements())
                    {
                        if (MeetsSchedulingRequirements())
                        {
                            return WorkOrderPhase.Finalization;
                        }
                        return WorkOrderPhase.Scheduling;
                    }
                    return WorkOrderPhase.Planning;
                }
                return WorkOrderPhase.Input;
            }
        }

        public bool MaterialsApproved =>
            (MaterialsApprovedOn != null &&
             MaterialsApprovedBy != null);

        // ReSharper disable UseMethodAny.0
        // ReSharper disable SimplifyConditionalTernaryExpression
        public bool WorkStarted =>
            (CrewAssignments.Count == 0)
                ? false
                : (from ca in CrewAssignments
                   where ca.DateStarted != null && ca.DateStarted.Value.Date <= DateTime.Today
                   select ca).Count() > 0;

        public bool WorkStartedBetween(DateTime startDate, DateTime endDate)
        {
            return (CrewAssignments.Count == 0)
                ? false
                : (from ca in CrewAssignments
                   where
                       ca.DateStarted != null &&
                       ca.DateStarted.Value.Date >= startDate &&
                       ca.DateStarted.Value.Date <= endDate
                   select ca).Count() > 0;
        }
        // ReSharper restore SimplifyConditionalTernaryExpression
        // ReSharper restore UseMethodAny.0

        public bool WorkCompleted => (DateCompleted != null);

        public float? TotalManHours
        {
            get
            {
                if (HasOpenAssignments)
                {
                    return null;
                }
                var sum = 0f;
                foreach (var assignment in CrewAssignments)
                {
                    if (assignment.HasStarted && assignment.TotalManHours.HasValue)
                    {
                        sum += assignment.TotalManHours.Value;
                    }
                }
                return sum;
            }
        }

        protected bool HasOpenAssignments
        {
            get
            {
                foreach (var assignment in CrewAssignments)
                {
                    if (assignment.IsOpen)
                        return true;
                }
                return false;
            }
        }

        public TimeSpan? ProcessTime =>
            (DateReceived == null || DateCompleted == null)
                ? (TimeSpan?)null
                : DateCompleted.Value.Subtract(DateReceived.Value);

        public TimeSpan? SupervisorProcessTime =>
            (DateCompleted == null || ApprovedOn == null)
                ? (TimeSpan?)null
                : ApprovedOn.Value.Subtract(DateCompleted.Value);

        public TimeSpan? StockProcessTime =>
            (ApprovedOn == null || MaterialsApprovedOn == null)
                ? (TimeSpan?)null
                : MaterialsApprovedOn.Value.Subtract(ApprovedOn.Value);

        // for Restoration Tickets
        public string FirstAccountingString =>
            (WorkDescription.GetAccountingTypeEnum() ==
             AccountingTypeEnum.OAndM) ?
                BusinessUnit + '.' :
                AccountCharged + '.';

        // for Restoration Tickets
        public string SecondAccountingString =>
            (!String.IsNullOrEmpty(
                WorkDescription.SecondRestorationAccountingString))
                ? FirstAccountingString : string.Empty;

        public int? DaysSinceCompletion =>
            DateCompleted == null
                ? default(int?) : (DateTime.Now - DateCompleted.Value).Days;

        public object ActuallyCompletedBy
        {
            get
            {
                if (!DateCompleted.HasValue)
                {
                    return null;
                }
                return (object)CompletedBy ?? AssignedContractor;
            }
        }

        //public bool OrcomOrderCompleted
        //{
        //    get { return (OrcomOrderCompletions.Count > 0); }
        //}
        public string UpdatedMobileGISStr => UpdatedMobileGIS.GetValueOrDefault().ToString("yn");

        /// <summary>
        /// Returns the correct username based on the work description accounting type
        /// </summary>
        public string PermitsUserName
        {
            get
            {
                if (WorkDescription.AccountingTypeID == (int)AccountingTypeEnum.OAndM)
                    return OperatingCenter.PermitsOMUserName;
                return OperatingCenter.PermitsCapitalUserName;
            }
        }

        /// <summary>
        /// If this can't be approved, an error message is display in account form
        /// and the button becomes disabled.
        /// </summary>
        public bool? CanBeApproved
        {
            get
            {
                if (HasServiceApprovalIssue)
                {
                    return false;
                }
                if (HasInvestigativeWorkDescriptionApprovalIssue)
                {
                    return false;
                }
                if (HasSapNotReleased)
                {
                    return false;
                }
                return true;
            }
        }

        public bool HasInvestigativeWorkDescriptionApprovalIssue => MapCall.Common.Model.Entities.WorkDescription.INVESTIGATIVE.Contains(WorkDescriptionID);

        public bool HasServiceApprovalIssue =>
            AssetTypeID == AssetTypeRepository.Indices.SERVICE
            && WorkDescriptionRepository
              .SERVICE_APPROVAL_WORK_DESCRIPTIONS.Contains(WorkDescriptionID)
            && (!ServiceID.HasValue || ServiceID == 0 || (Service != null && !Service.DateInstalled.HasValue));

        public string RecordUrl { get; set; }

        #region Logical Booleans for Display/Errors/SAP/etc.

        // Is this an sap updatable work order
        public bool IsSapUpdatableWorkOrder => OperatingCenter != null && OperatingCenter.SAPEnabled && OperatingCenter.SAPWorkOrdersEnabled && !OperatingCenter.IsContractedOperations;

        //(PlantMaintenanceActivityTypeOverride != null && PlantMaintenanceActivityTypeOverride.Id != PlantMaintenanceActivityType.Indices.PBC)
        public bool PlantMaintenanceActivityTypeOverrideEditable => !IsSapUpdatableWorkOrder || (PlantMaintenanceActivityTypeOverride == null && !Approved);
        public bool AssetTypeEditable => PlantMaintenanceActivityTypeOverrideEditable;
        public bool AssetEditable => (AssetTypeID == MapCall.Common.Model.Entities.AssetType.Indices.SERVICE || AssetTypeID == MapCall.Common.Model.Entities.AssetType.Indices.SEWER_LATERAL) ? AccountNumberEditable : PlantMaintenanceActivityTypeOverrideEditable;

        public bool WorkDescriptionEditable => (!IsSapUpdatableWorkOrder || (!Approved)) && (!IsNewServiceInstallation);
        public bool AccountNumberEditable
        {
            get
            {
                if (PlantMaintenanceActivityTypeOverride != null && PlantMaintenanceActivityTypeOverride.Id == PlantMaintenanceActivityType.Indices.PBC)
                    return false;
                return !IsSapUpdatableWorkOrder || (!Approved);
            }
        }

        // Cannot edit the doc id after it has been entered.
        public bool MaterialsDocIDEditable => !IsSapUpdatableWorkOrder || (!string.IsNullOrWhiteSpace(MaterialsDocID));
        public bool AssetTypeError => AssetType.AssetTypeID != WorkDescription.AssetTypeID;
        public bool HasRealSapError => IsSapUpdatableWorkOrder && !string.IsNullOrWhiteSpace(SAPErrorCode) && !SAPErrorCode.ToUpper().Contains("SUCCESS");
        public bool HasSapNotReleased => !string.IsNullOrWhiteSpace(SAPErrorCode) && (SAPErrorCode.ToUpper().Contains("NOT RELEASED") || SAPErrorCode.ToUpper().Contains("RELEASE REJECTED"));

        public bool IsNewServiceInstallation => IsSapUpdatableWorkOrder 
            && AssetTypeID == AssetTypeRepository.Indices.SERVICE
            && WorkDescriptionRepository.NEW_SERVICE_INSTALLATION.Contains(WorkDescriptionID)
            && PremiseNumber != PREMISE_NUMBER_PLACEHOLDER;

        #endregion
        //public bool CanBeApproved => !AssetTypeError;

        // NOTE: This property also exists in the MapCall.Common WorkOrder class and needs to be kept in sync.
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

        public virtual bool SewerOverflowsVisible => WorkDescription.Description.ToLower() == "sewer main overflow" || WorkDescription.Description.ToLower() == "sewer service overflow";

        public virtual bool IsRevisit => (WorkDescription != null) && WorkDescription.Revisit;

        public virtual string ArcCollectorLink
        {
            get
            {
                NameValueCollection nvc;
                switch (AssetTypeID)
                {
                    case MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT:
                        return ArcCollectorLinkGenerator.ArcCollectorHydrantLink(Hydrant, AssetType, this);
                    case MapCall.Common.Model.Entities.AssetType.Indices.SEWER_OPENING:
                        return ArcCollectorLinkGenerator.ArcCollectorSewerOpeningLink(SewerOpening, AssetType, this);
                    case MapCall.Common.Model.Entities.AssetType.Indices.VALVE:
                        return ArcCollectorLinkGenerator.ArcCollectorValveLink(Valve, AssetType, this);
                    default:
                        nvc = new NameValueCollection {
                            {"referenceContext", "center"},
                            {"itemID", OperatingCenter?.ArcMobileMapId},
                            {"center", Latitude?.ToString() + "," + Longitude?.ToString()},
                            {"scale", "3000"}
                        };
                        return ArcCollectorLinkGenerator.ArcCollectorLink(nvc, this);
                }
            }
        }


        #endregion

        #region Table Column Properties

        #region Initial Input Properties

        [Column(Storage = "_assetTypeID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int AssetTypeID
        {
            get => _assetTypeID;
            set
            {
                if (_assetTypeID != value)
                {
                    if (_assetType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _assetTypeID = value;
                SendPropertyChanged("AssetTypeID");
            }
        }

        [Column(Name = "CreatedAt", Storage = "_createdOn", DbType = "SmallDateTime NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedOn
        {
            get => _createdOn;
            set
            {
                if (_createdOn != value)
                {
                    OnCreatedOnChanging(value);
                    SendPropertyChanging();
                    _createdOn = value;
                    SendPropertyChanged("CreatedOn");
                }
            }
        }

        [Column(Storage = "_creatorID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int CreatorID
        {
            get => _creatorID;
            set
            {
                if (_creatorID != value)
                {
                    if (_createdBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _creatorID = value;
                SendPropertyChanged("CreatorID");
            }
        }

        [Column(Storage = "_customerName", DbType = "VarChar(30)", UpdateCheck = UpdateCheck.Never)]
        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (value != null && value.Length > MAX_CUSTOMERNAME_LENGTH)
                    throw new StringTooLongException("CustomerName", MAX_CUSTOMERNAME_LENGTH);
                if (_customerName != value)
                {
                    SendPropertyChanging();
                    _customerName = value;
                    SendPropertyChanged("CustomerName");
                }
            }
        }

        /// <summary>
        /// Date/Time that a given Work Order was initially recieved.
        /// </summary>
        [Column(Storage = "_dateReceived", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateReceived
        {
            get => _dateReceived;
            set
            {
                if (_dateReceived != value)
                {
                    SendPropertyChanging();
                    _dateReceived = value;
                    SendPropertyChanged("DateReceived");
                }
            }
        }

        [Column(Storage = nameof(_initialFlushTimeEnteredAt), DbType = "DateTime", CanBeNull = true,
            UpdateCheck = UpdateCheck.Never)]
        public DateTime? InitialFlushTimeEnteredAt
        {
            get => _initialFlushTimeEnteredAt;
            set
            {
                if (_initialFlushTimeEnteredAt != value)
                {
                    SendPropertyChanging();
                    _initialFlushTimeEnteredAt = value;
                    SendPropertyChanged(nameof(InitialFlushTimeEnteredAt));
                }
            }
        }

        [Column(Storage = nameof(_datePitcherFilterDeliveredToCustomer), DbType = "DateTime",
            CanBeNull = true,
            UpdateCheck = UpdateCheck.Never)]
        public DateTime? DatePitcherFilterDeliveredToCustomer
        {
            get => _datePitcherFilterDeliveredToCustomer;
            set
            {
                if (_datePitcherFilterDeliveredToCustomer != value)
                {
                    SendPropertyChanging();
                    _datePitcherFilterDeliveredToCustomer = value;
                    SendPropertyChanged(nameof(DatePitcherFilterDeliveredToCustomer));
                }
            }
        }

        [Column(Storage = nameof(_dateCustomerProvidedAWStateLeadInformation), DbType = "DateTime",
            CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateCustomerProvidedAWStateLeadInformation
        {
            get => _dateCustomerProvidedAWStateLeadInformation;
            set
            {
                if (_dateCustomerProvidedAWStateLeadInformation != value)
                {
                    SendPropertyChanging();
                    _dateCustomerProvidedAWStateLeadInformation = value;
                    SendPropertyChanged(nameof(DateCustomerProvidedAWStateLeadInformation));
                }
            }
        }

        [Column(Storage = "_hydrantID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? HydrantID
        {
            get => _hydrantID;
            set
            {
                if (_hydrantID != value)
                {
                    if (_hydrant.HasLoadedOrAssignedValue)
                        _hydrant = default(EntityRef<Hydrant>);
                }
                SendPropertyChanging();
                _hydrantID = value;
                SendPropertyChanged("HydrantID");
            }
        }

        [Column(Storage = "_mainCrossingID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? MainCrossingID
        {
            get => _mainCrossingID;
            set
            {
                if (_mainCrossingID != value)
                {
                    if (_mainCrossing.HasLoadedOrAssignedValue)
                        _mainCrossing = default(EntityRef<MainCrossing>);
                }
                SendPropertyChanging();
                _mainCrossingID = value;
                SendPropertyChanged("MainCrossingID");
            }
        }

        /// <summary>
        /// TODO:Latitude/Longitude Hacked as per 03/06/2009 Meeting
        /// These were supposed to only ever come from an Asset that 
        /// the WorkOrder was attached to. They did not have mains or
        /// services to tie those asset types to. That's where these
        /// fit in now.
        /// -ARR
        /// TODO: Tests around these Lat/Lon
        /// </summary>
        [Column(Storage = "_latitude", DbType = "Float NULL", UpdateCheck = UpdateCheck.Never)]
        public double? Latitude
        {
            get
            {
                if (AssetType != null && (AssetType.TypeEnum == AssetTypeEnum.Hydrant ||
                    AssetType.TypeEnum == AssetTypeEnum.Valve ||
                    AssetType.TypeEnum == AssetTypeEnum.SewerOpening ||
                    AssetType.TypeEnum == AssetTypeEnum.StormCatch || 
                    AssetType.TypeEnum == AssetTypeEnum.MainCrossing) && Asset != null)
                    _latitude = Asset.Latitude;
                return _latitude;
            }
            set => _latitude = value;
        }

        [Column(Storage = "_longitude", DbType = "Float NULL", UpdateCheck = UpdateCheck.Never)]
        public double? Longitude
        {
            get
            {
                if (AssetType != null && (AssetType.TypeEnum == AssetTypeEnum.Hydrant ||
                    AssetType.TypeEnum == AssetTypeEnum.Valve || 
                    AssetType.TypeEnum == AssetTypeEnum.SewerOpening ||
                    AssetType.TypeEnum == AssetTypeEnum.StormCatch ||
                    AssetType.TypeEnum == AssetTypeEnum.MainCrossing) && Asset != null)
                    _longitude = Asset.Longitude;
                return _longitude;
            }
            set => _longitude = value;
        }

        [Column(Storage = "_markoutRequirementID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int MarkoutRequirementID
        {
            get => _markoutRequirementID;
            set
            {
                if (_markoutRequirementID != value)
                {
                    if (_markoutRequirement.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _markoutRequirementID = value;
                SendPropertyChanged("MarkoutRequirementID");
            }
        }

        [Column(Storage = "_nearestCrossStreetID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? NearestCrossStreetID
        {
            get => _nearestCrossStreetID;
            set
            {
                if (_nearestCrossStreetID != value)
                {
                    if (_nearestCrossStreet.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _nearestCrossStreetID = value;
                    SendPropertyChanged("NearestCrossStreetID");
                }
            }
        }

        /* THIS ONE HAD UpdateCheck SET TO "Never" TO BEGIN WITH */
        [Column(Storage = "_notes", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    SendPropertyChanging();
                    _notes = value;
                    SendPropertyChanged("Notes");
                }
            }
        }

        [Column(Storage = "_requiredMarkoutNote", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string RequiredMarkoutNote
        {
            get => _requiredMarkoutNote;
            set
            {
                if (_requiredMarkoutNote != value)
                {
                    SendPropertyChanging();
                    _requiredMarkoutNote = value;
                    SendPropertyChanged("Notes");
                }
            }
        }

        [Column(Storage = "_operatingCenterID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OperatingCenterID
        {
            get => _operatingCenterID;
            set
            {
                // TODO: include this when the OperatingCenter is mapped
                //if (_operatingCenterID != value)
                //{
                //    if (_operatingCenter.HasLoadedOrAssignedValue)
                //        throw new ForeignKeyReferenceAlreadyHasValueException();
                //}
                SendPropertyChanging();
                _operatingCenterID = value;
                SendPropertyChanged("OperatingCenterID");
            }
        }

        [Column(Storage = "_orcomServiceOrderNumber", DbType = "VarChar(20)", UpdateCheck = UpdateCheck.Never)]
        public string ORCOMServiceOrderNumber
        {
            get => _orcomServiceOrderNumber;
            set
            {
                if (value != null && value.Length > MAX_ORCOM_SERVICE_ORDER_NUMBER_LENGTH)
                    throw new StringTooLongException("ORCOMServiceOrderNumber",
                        MAX_ORCOM_SERVICE_ORDER_NUMBER_LENGTH);
                if (_orcomServiceOrderNumber != value)
                {
                    SendPropertyChanging();
                    _orcomServiceOrderNumber = value;
                    SendPropertyChanged("ORCOMServicOrderNumber");
                }
            }
        }

        [Column(Storage = "_phoneNumber", DbType = "VarChar(14)", UpdateCheck = UpdateCheck.Never)]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value != null && value.Length > MAX_PHONENUMBER_LENGTH)
                    throw new StringTooLongException("PhoneNumber", MAX_PHONENUMBER_LENGTH);
                if (_phoneNumber != value)
                {
                    SendPropertyChanging();
                    _phoneNumber = value;
                    SendPropertyChanged("PhoneNumber");
                }
            }
        }

        /// <summary>
        /// Premise Number of a given Work Order.  Required when AssetType
        /// is "Service".
        /// </summary>
        [Column(Storage = "_premiseNumber", DbType = "VarChar(10)", UpdateCheck = UpdateCheck.Never)]
        public string PremiseNumber
        {
            get => _premiseNumber;
            set
            {
                //TODO: Review/Fix, hacked 20090309 ARR
                if (!String.IsNullOrEmpty(value))
                {
                    if (value.Length > MAX_PREMISENUMBER_LENGTH)
                        throw new StringTooLongException("PremiseNumber",
                            MAX_PREMISENUMBER_LENGTH);
                    if (value.Length < MIN_PREMISENUMBER_LENGTH)
                        throw new DomainLogicException(
                            "Cannot set the value of Premise Number to a string that's less than 8 chars long.");
                }
                if (_premiseNumber != value)
                {
                    SendPropertyChanging();
                    _premiseNumber = value;
                    SendPropertyChanged("PremiseNumber");
                }
            }
        }

        //[StringLength(WorkOrder.StringLengths.DEVICE)]
        [Column(Storage= "_meterSerialNumber", DbType = "Varchar(30)", UpdateCheck = UpdateCheck.Never)]
        public string MeterSerialNumber
        {
            get => _meterSerialNumber;
            set
            {
                if (value.Length > MapCall.Common.Model.Entities.WorkOrder.StringLengths.METER_SERIAL_NUMBER)
                    throw new StringTooLongException(MeterSerialNumber, MapCall.Common.Model.Entities.WorkOrder.StringLengths.METER_SERIAL_NUMBER);
                _meterSerialNumber = value;
            }
        }

        [Column(Storage = "_deviceLocation", DbType = "Bigint", UpdateCheck = UpdateCheck.Never)]
        public long? DeviceLocation
        {
            get => _deviceLocation;
            set => _deviceLocation = value;
        }

        [Column(Storage = "_installation", DbType = "Bigint", UpdateCheck = UpdateCheck.Never)]
        public long? Installation
        {
            get => _installation;
            set => _installation = value;
        }

        [Column(Storage = "_sapEquipmentNumber", DbType = "Bigint", UpdateCheck = UpdateCheck.Never)]
        public long? SAPEquipmentNumber
        {
            get => _sapEquipmentNumber;
            set => _sapEquipmentNumber = value;
        }

        //public virtual string Installation { get; set; }

        [Column(Storage = "_priorityID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int PriorityID
        {
            get => _priorityID;
            set
            {
                if (_priorityID != value)
                {
                    if (_priority.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _priorityID = value;
                    SendPropertyChanged("PriorityID");
                }
            }
        }

        [Column(Storage = "_purposeID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int PurposeID
        {
            get => _purposeID;
            set
            {
                if (_purposeID != value)
                    if (_drivenBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _purposeID = value;
                SendPropertyChanged("PurposeID");
            }
        }

        [Column(Storage = "_requesterID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int RequesterID
        {
            get => _requesterID;
            set
            {
                if (_requesterID != value)
                    if (_requestedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _requesterID = value;
                SendPropertyChanged("RequesterID");
            }
        }

        [Column(Storage = "_requestingEmployeeID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? RequestingEmployeeID
        {
            get => _requestingEmployeeID;
            set
            {
                if (_requestingEmployeeID != value)
                    if (_requestingEmployee.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _requestingEmployeeID = value;
                SendPropertyChanged("RequestingEmployeeID");
            }
        }

        [Column(Storage = "_secondaryPhoneNumber", DbType = "VarChar(14)", UpdateCheck = UpdateCheck.Never)]
        public string SecondaryPhoneNumber
        {
            get => _secondaryPhoneNumber;
            set
            {
                if (value != null && value.Length > MAX_SECONDARYPHONENUMBER_LENGTH)
                    throw new StringTooLongException("SecondaryPhoneNumber", MAX_SECONDARYPHONENUMBER_LENGTH);
                if (_secondaryPhoneNumber != value)
                {
                    SendPropertyChanging();
                    _secondaryPhoneNumber = value;
                    SendPropertyChanged("SecondaryPhoneNumber");
                }
            }
        }

        /// <summary>
        /// Identifier for a customer's service account, for when AssetType
        /// is "Service".  This field is not required in that case, but PremiseNumber
        /// is.
        /// </summary>
        [Column(Storage = "_serviceNumber", DbType = "VarChar(50)", UpdateCheck = UpdateCheck.Never)]
        public string ServiceNumber
        {
            get => _serviceNumber;
            set
            {
                //TODO: Review/Fix, hacked 20090309 ARR
                if (!String.IsNullOrEmpty(value) && value.Length > MAX_SERVICENUMBER_LENGTH)
                    throw new StringTooLongException("ServiceNumber", MAX_SERVICENUMBER_LENGTH);
                if (_serviceNumber != value)
                {
                    SendPropertyChanging();
                    _serviceNumber = value;
                    SendPropertyChanged("ServiceNumber");
                }
            }
        }

        [Column(Storage = "_streetID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? StreetID
        {
            get => _streetID;
            set
            {
                if (_streetID != value)
                {
                    if (_street.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _streetID = value;
                    SendPropertyChanged("StreetID");
                }
            }
        }

        [Column(Storage = "_streetNumber", DbType = "VarChar(20)", UpdateCheck = UpdateCheck.Never)]
        public string StreetNumber
        {
            get => _streetNumber;
            set
            {
                if (value != null && value.Length > MAX_STREETNUMBER_LENGTH)
                    throw new StringTooLongException("StreetNumber", MAX_STREETNUMBER_LENGTH);
                if (_streetNumber != value)
                {
                    SendPropertyChanging();
                    _streetNumber = value;
                    SendPropertyChanged("StreetNumber");
                }
            }
        }

        [Column(Storage = "_streetOpeningPermitRequired", DbType = "Bit NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public bool StreetOpeningPermitRequired
        {
            get => _streetOpeningPermitRequired;
            set
            {
                if (_streetOpeningPermitRequired != value)
                {
                    SendPropertyChanging();
                    _streetOpeningPermitRequired = value;
                    SendPropertyChanged("StreetOpeningPermitRequired");
                }
            }
        }

        [Column(Storage = "_digitalAsBuiltRequired", DbType = "Bit NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public bool DigitalAsBuiltRequired
        {
            get => _digitalAsBuiltRequired;
            set
            {
                if (_digitalAsBuiltRequired != value)
                {
                    SendPropertyChanging();
                    _digitalAsBuiltRequired = value;
                    SendPropertyChanged("DigitalAsBuiltRequired");
                }
            }
        }

        [Column(Storage = "_digitalAsBuiltCompleted", DbType = "Bit NULL", UpdateCheck = UpdateCheck.Never)]
        public bool? DigitalAsBuiltCompleted
        {
            get => _digitalAsBuiltCompleted;
            set
            {
                if (_digitalAsBuiltCompleted != value)
                {
                    SendPropertyChanging();
                    _digitalAsBuiltCompleted = value;
                    SendPropertyChanged("DigitalAsBuiltCompleted");
                }
            }
        }

        [Column(Storage = nameof(_hasPitcherFilterBeenProvidedToCustomer), DbType = "Bit NULL",
            UpdateCheck = UpdateCheck.Never)]
        public bool? HasPitcherFilterBeenProvidedToCustomer
        {
            get => _hasPitcherFilterBeenProvidedToCustomer;
            set
            {
                if (_hasPitcherFilterBeenProvidedToCustomer != value)
                {
                    SendPropertyChanging();
                    _hasPitcherFilterBeenProvidedToCustomer = value;
                    SendPropertyChanged(nameof(HasPitcherFilterBeenProvidedToCustomer));
                }
            }
        }

        [Column(Storage = "_trafficControlRequired", DbType = "Bit NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public bool TrafficControlRequired
        {
            get => _trafficControlRequired;
            set
            {
                if (_trafficControlRequired != value)
                {
                    SendPropertyChanging();
                    _trafficControlRequired = value;
                    SendPropertyChanged("TrafficControlRequired");
                }
            }
        }

        [Column(Storage = "_townID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int TownID
        {
            get => _townID;
            set
            {
                if (_townID != value)
                {
                    if (_tblNJAWTownName.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _townID = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        [Column(Storage = "_townSectionID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? TownSectionID
        {
            get => _townSectionID;
            set
            {
                if (_townSectionID != value)
                {
                    if (_tblNJAWTwnSection.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _townSectionID = value;
                    SendPropertyChanged("TownSectionID");
                }
            }
        }

        [Column(Storage = "_valveID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ValveID
        {
            get => _valveID;
            set
            {
                if (_valveID != value)
                {
                    if (_valve.HasLoadedOrAssignedValue)
                        _valve = default(EntityRef<Valve>);
                }
                SendPropertyChanging();
                _valveID = value;
                SendPropertyChanged("ValveID");
            }
        }

        [Column(Storage = "_workOrderID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkOrderID
        {
            get => _workOrderID;
            set
            {
                if (_workOrderID != value)
                {
                    SendPropertyChanging();
                    _workOrderID = value;
                    SendPropertyChanged("WorkOrderID");
                }
            }
        }

        [Column(Storage = "_workDescriptionID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int WorkDescriptionID
        {
            get => _workDescriptionID;
            set
            {
                if (_workDescriptionID != value)
                {
                    if (_workDescription.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workDescriptionID = value;
                SendPropertyChanged("WorkDescriptionID");
            }
        }

        [Column(Storage = "_zipCode", DbType = "VarChar(10)", UpdateCheck = UpdateCheck.Never)]
        public string ZipCode
        {
            get => _zipCode;
            set
            {
                if (value != null && value.Length > MAX_ZIP_CODE_LENGTH)
                    throw new StringTooLongException("ZipCode", MAX_ZIP_CODE_LENGTH);
                if (_zipCode != value)
                {
                    SendPropertyChanging();
                    _zipCode = value;
                    SendPropertyChanged("ZipCode");
                }
            }
        }

        [Column(Storage="_sewerOpeningID", DbType="Int", UpdateCheck = UpdateCheck.Never)]
        public int? SewerOpeningID
        {
            get => _sewerOpeningID;
            set
            {
                if (_sewerOpeningID != value)
                {
                    if (_sewerOpening.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _sewerOpeningID = value;
                SendPropertyChanged("SewerOpeningID");
            }

        }

        [Column(Storage="_stormCatchID", DbType="Int", UpdateCheck = UpdateCheck.Never)]
        public int? StormCatchID
        {
            get => _stormCatchID;
            set
            {
                if (_stormCatchID != value)
                {
                    if (_stormCatch.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _stormCatchID = value;
                SendPropertyChanged("StormCatchID");
            }
        }

        [Column(Storage = "_serviceID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ServiceID
        {
            get => _serviceID;
            set
            {
                if (_serviceID != value)
                {
                    if (_service.HasLoadedOrAssignedValue)
                        _service = default(EntityRef<Service>);
                }
                SendPropertyChanging();
                _serviceID = value;
                SendPropertyChanged("ServiceID");
            }
        }

        [Column(Storage = "_equipmentID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? EquipmentID
        {
            get => _equipmentID;
            set
            {
                if (_equipmentID != value)
                {
                    if (_equipment.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _equipmentID = value;
                SendPropertyChanged("EquipmentID");
            }
        }

        [Column(Storage = "_markoutTypeNeededID", DbType = "Int")]
        public int? MarkoutTypeNeededID
        {
            get => _markoutTypeNeededID;
            set
            {
                if (_markoutTypeNeededID != value)
                {
                    if (_markoutTypeNeeded.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _markoutTypeNeededID = value;
                SendPropertyChanged("MarkoutTypeNeededID");
            }
        }

        [Column(Storage = "_flushingNoticeTypeId", DbType = "Int")]
        public int? FlushingNoticeTypeId
        {
            get => _flushingNoticeTypeId;
            set
            {
                if (_flushingNoticeTypeId != value)
                {
                    if (_flushingNoticeType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _flushingNoticeTypeId = value;
                SendPropertyChanged("FlushingNoticeTypeId");
            }
        }

        #endregion

        /// <summary>
        /// Shows if the work order has been approved by a supervisor.
        /// </summary>
        public bool Approved => ApprovedOn != null;

        [Column(Storage = "_alertIssued", DbType = "Bit", UpdateCheck = UpdateCheck.Never)]
        public bool? AlertIssued
        {
            get => _alertIssued;
            set
            {
                if (_alertIssued != value)
                {
                    SendPropertyChanging();
                    _alertIssued = value;
                    SendPropertyChanged("AlertIssued");
                }
            }
        }

        [Column(Storage = "_alertStarted", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? AlertStarted
        {
            get => _alertStarted;
            set
            {
                if (_alertStarted != value)
                {
                    SendPropertyChanging();
                    _alertStarted = value;
                    SendPropertyChanged("AlertStarted");
                }
            }
        }

        [Column(Storage = "_doorNoticeLeftDate", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DoorNoticeLeftDate
        {
            get => _doorNoticeLeftDate;
            set
            {
                if (_doorNoticeLeftDate != value)
                {
                    SendPropertyChanging();
                    _doorNoticeLeftDate = value;
                    SendPropertyChanged("DoorNoticeLeftDate");
                }
            }
        }


        /// <summary>
        /// Date approved by Supervisor
        /// </summary>
        [Column(Storage = "_approvedOn", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? ApprovedOn
        {
            get => _approvedOn;
            set
            {
                if (_approvedOn != value)
                {
                    SendPropertyChanging();
                    _approvedOn = value;
                    SendPropertyChanged("ApprovedOn");
                }
            }
        }

        [Column(Storage = "_approvedByID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ApprovedByID
        {
            get => _approvedByID;
            set
            {
                if (_approvedByID != value)
                {
                    SendPropertyChanging();
                    _approvedByID = value;
                    SendPropertyChanged("ApprovedByID");
                }
            }
        }

        [Column(Storage = "_materialsApprovedByID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? MaterialsApprovedByID
        {
            get => _materialsApprovedByID;
            set
            {
                if (_materialsApprovedByID != value)
                {
                    if (_materialsApprovedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _materialsApprovedByID = value;
                SendPropertyChanged("MaterialsApprovedByID");
            }
        }

        [Column(Storage = "_materialsApprovedOn", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? MaterialsApprovedOn
        {
            get => _materialsApprovedOn;
            set
            {
                if (_materialsApprovedOn != value)
                {
                    SendPropertyChanging();
                    _materialsApprovedOn = value;
                    SendPropertyChanged("MaterialsApprovedOn");
                }
            }
        }

        [Column(Storage = "_materialsDocID", DbType = "VarChar(15)", UpdateCheck = UpdateCheck.Never)]
        public string MaterialsDocID
        {
            get => _materialsDocID;
            set
            {
                if (value != null && value.Length > MAX_MATERIALS_DOC_ID_LENGTH)
                    throw new StringTooLongException("MaterialsDocID", MAX_MATERIALS_DOC_ID_LENGTH);
                if (_materialsDocID != value)
                {
                    SendPropertyChanging();
                    _materialsDocID = value;
                    SendPropertyChanged("MaterialsDocID");
                }
            }
        }

        [Column(Storage = "_completedByID", DbType = "Int")]
        public int? CompletedByID
        {
            get => _completedByID;
            set
            {
                if (_completedByID != value)
                {
                    if (_completedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _completedByID = value;
                SendPropertyChanged("CompletedByID");
            }
        }

        [Column(Storage = "_cancelledByID", DbType = "Int")]
        public int? CancelledByID
        {
            get => _cancelledByID;
            set
            {
                if (_cancelledByID != value)
                {
                    if (_cancelledBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _cancelledByID = value;
                SendPropertyChanged("CancelledByID");
            }
        }

        [Column(Storage = "_workOrderCancellationReasonID", DbType = "Int")]
        public int? WorkOrderCancellationReasonID
        {
            get => _workOrderCancellationReasonID;
            set
            {
                if (_workOrderCancellationReasonID != value)
                {
                    if (_workOrderCancellationReason.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workOrderCancellationReasonID = value;
                SendPropertyChanged("WorkOrderCancellationReasonID");
            }
        }

        [Column(Storage = "_sapWorkOrderStepID", DbType = "Int")]
        public int? SAPWorkOrderStepID
        {
            get => _sapWorkOrderStepID;
            set
            {
                if (_sapWorkOrderStepID != value)
                {
                    if (_sapWorkOrderStep.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _sapWorkOrderStepID = value;
                SendPropertyChanged("SAPWorkOrderStepID");
            }
        }

        [Column(Storage = "_echoshoreLeakAlertId", DbType = "Int")]
        public int? EchoshoreLeakAlertId
        {
            get => _echoshoreLeakAlertId;
            set
            {
                if (_echoshoreLeakAlertId != value)
                {
                    SendPropertyChanging();
                    _echoshoreLeakAlertId = value;
                    SendPropertyChanged("EchoshoreLeakAlertId");
                }
            }

        }

        [Column(Storage = "_dateStarted", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateStarted
        {
            get => _dateStarted;
            set
            {
                if (_dateStarted != value)
                {
                    SendPropertyChanging();
                    _dateStarted = value;
                    SendPropertyChanged("DateStarted");
                }
            }
        }

        [Column(Storage = "_dateCompleted", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateCompleted
        {
            get => _dateCompleted;
            set
            {
                if (_dateCompleted != value)
                {
                    SendPropertyChanging();
                    _dateCompleted = value;
                    SendPropertyChanged("DateCompleted");
                }
            }
        }

        [Column(Storage = "_cancelledAt", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? CancelledAt
        {
            get => _cancelledAt;
            set
            {
                if (_cancelledAt != value)
                {
                    SendPropertyChanging();
                    _cancelledAt = value;
                    SendPropertyChanged("CancelledAt");
                }
            }
        }

        [Column(Storage = "_materialPlanningCompletedOn", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? MaterialPlanningCompletedOn
        {
            get => _materialPlanningCompletedOn;
            set
            {
                if (_materialPlanningCompletedOn != value)
                {
                    SendPropertyChanging();
                    _materialPlanningCompletedOn = value;
                    SendPropertyChanged("MaterialPlanningCompletedOn");
                }
            }
        }

        [Column(Storage = "_materialPostingDate", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? MaterialPostingDate
        {
            get => _materialPostingDate;
            set
            {
                if (_materialPostingDate != value)
                {
                    SendPropertyChanging();
                    _materialPostingDate = value;
                    SendPropertyChanged("MaterialPostingDate");
                }
            }
        }

        [Column(Storage = "_dateRejected", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateRejected
        {
            get => _dateRejected;
            set
            {
                if (_dateRejected != value)
                {
                    SendPropertyChanging();
                    _dateRejected = value;
                    SendPropertyChanged("DateRejected");
                }
            }
        }

        [Column(Storage = "_datePrinted", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DatePrinted
        {
            get => _datePrinted;
            set
            {
                if (_datePrinted != value)
                {
                    SendPropertyChanging();
                    _datePrinted = value;
                    SendPropertyChanged("DatePrinted");
                }
            }
        }

        [Column(Storage = "_dateReportSent", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateReportSent
        {
            get => _dateReportSent;
            set
            {
                if (_dateReportSent != value)
                {
                    SendPropertyChanging();
                    _dateReportSent = value;
                    SendPropertyChanged("DateReportSent");
                }
            }
        }

        [Column(Storage = "_backhoeOperator", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? BackhoeOperator
        {
            get => _backhoeOperator;
            set
            {
                if (_backhoeOperator != value)
                {
                    SendPropertyChanging();
                    _backhoeOperator = value;
                    SendPropertyChanged("BackhoeOperator");
                }
            }
        }

        [Column(Storage = "_excavationDate", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? ExcavationDate
        {
            get => _excavationDate;
            set
            {
                if (_excavationDate != value)
                {
                    SendPropertyChanging();
                    _excavationDate = value;
                    SendPropertyChanged("ExcavationDate");
                }
            }
        }

        [Column(Storage = "_dateCompletedPC", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateCompletedPC
        {
            get => _dateCompletedPC;
            set
            {
                if (_dateCompletedPC != value)
                {
                    SendPropertyChanging();
                    _dateCompletedPC = value;
                    SendPropertyChanged("DateCompletedPC");
                }
            }
        }

        [Column(Storage = "_lostWater", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? LostWater
        {
            get => _lostWater;
            set
            {
                if (_lostWater != value)
                {
                    SendPropertyChanging();
                    _lostWater = value;
                    SendPropertyChanged("LostWater");
                }
            }
        }

        [Column(Storage = "_numberOfOfficersRequired", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? NumberOfOfficersRequired
        {
            get => _numberOfOfficersRequired;
            set
            {
                if (_numberOfOfficersRequired != value)
                {
                    SendPropertyChanging();
                    _numberOfOfficersRequired = value;
                    SendPropertyChanged("NumberOfOfficersRequired");
                }
            }
        }

        [Column(Storage = nameof(_initialServiceLineFlushTime), DbType = "Int", CanBeNull = true,
            UpdateCheck = UpdateCheck.Never)]
        public int? InitialServiceLineFlushTime
        {
            get => _initialServiceLineFlushTime;
            set
            {
                if (_initialServiceLineFlushTime != value)
                {
                    SendPropertyChanging();
                    _initialServiceLineFlushTime = value;
                    SendPropertyChanged(nameof(InitialServiceLineFlushTime));
                }
            }
        }

        [Column(Storage = nameof(_initialFlushTimeEnteredById), DbType = "int", CanBeNull = true,
            UpdateCheck = UpdateCheck.Never)]
        public int? InitialFlushTimeEnteredById
        {
            get => _initialFlushTimeEnteredById;
            set
            {
                if (_initialFlushTimeEnteredById != value)
                {
                    SendPropertyChanging();
                    _initialFlushTimeEnteredById = value;
                    SendPropertyChanged(nameof(InitialFlushTimeEnteredById));
                }
            }
        }

        [Column(Storage = nameof(_pitcherFilterCustomerDeliveryMethodId), DbType = "int", CanBeNull = true,
            UpdateCheck = UpdateCheck.Never)]
        public int? PitcherFilterCustomerDeliveryMethodId
        {
            get => _pitcherFilterCustomerDeliveryMethodId;
            set
            {
                if (_pitcherFilterCustomerDeliveryMethodId != value)
                {
                    SendPropertyChanging();
                    _pitcherFilterCustomerDeliveryMethodId = value;
                    SendPropertyChanged(nameof(PitcherFilterCustomerDeliveryMethodId));
                }
            }
        }

        [Column(Storage = "_oldWorkOrderNumber", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OldWorkOrderNumber
        {
            get => _oldWorkOrderNumber;
            set
            {
                if (_oldWorkOrderNumber != value)
                {
                    SendPropertyChanging();
                    _oldWorkOrderNumber = value;
                    SendPropertyChanged("OldWorkOrderNumber");
                }
            }
        }

        [Column(Storage = "_sapNotificationNumber", DbType = "bigint", UpdateCheck = UpdateCheck.Never)]
        public long? SAPNotificationNumber
        {
            get => _sapNotificationNumber;
            set
            {
                if (_sapNotificationNumber != value)
                {
                    SendPropertyChanging();
                    _sapNotificationNumber = value;
                    SendPropertyChanged("SAPNotificationNumber");
                }
            }
        }

        [Column(Storage = "_sapWorkOrderNumber", DbType = "bigint", UpdateCheck = UpdateCheck.Never)]
        public long? SAPWorkOrderNumber
        {
            get => _sapWorkOrderNumber;
            set
            {
                if (_sapWorkOrderNumber != value)
                {
                    SendPropertyChanging();
                    _sapWorkOrderNumber = value;
                    SendPropertyChanged("SAPWorkOrderNumber");
                }
            }
        }
        
        /// <summary>
        /// Account number of the customer whose service is involved in a
        /// given Work Order.  Sourced from the original field "Customer
        /// Account Number", with the display text "Service Order #".
        /// </summary>
        [Column(Storage = "_customerAccountNumber", DbType = "VarChar(11)", UpdateCheck = UpdateCheck.Never)]
        public string CustomerAccountNumber
        {
            get => _customerAccountNumber;
            set
            {
                if (value != null && value.Length > MAX_CUSTOMERACCOUNTNUMBER_LENGTH)
                    throw new StringTooLongException("CustomerAccountNumber", MAX_CUSTOMERACCOUNTNUMBER_LENGTH);
                if (_customerAccountNumber != value)
                {
                    SendPropertyChanging();
                    _customerAccountNumber = value;
                    SendPropertyChanged("CustomerAccountNumber");
                }
            }
        }

        [Column(Storage = "_accountCharged", DbType = "VarChar(30)", UpdateCheck = UpdateCheck.Never)]
        public string AccountCharged
        {
            get => _accountCharged;
            set
            {
                if (value != null && value.Length > MAX_ACCOUNTCHARGED_LENGTH)
                    throw new StringTooLongException("AccountCharged", MAX_ACCOUNTCHARGED_LENGTH);
                if (_accountCharged != value)
                {
                    SendPropertyChanging();
                    _accountCharged = value;
                    SendPropertyChanged("AccountCharged");
                }
            }
        }

        [Column(Storage = "_invoiceNumber", DbType = "VarChar(15)", UpdateCheck = UpdateCheck.Never)]
        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set
            {
                if (value != null && value.Length > MAX_INVOICENUMBER_LENGTH)
                    throw new StringTooLongException("InvoiceNumber", MAX_INVOICENUMBER_LENGTH);
                if (_invoiceNumber != value)
                {
                    SendPropertyChanging();
                    _invoiceNumber = value;
                    SendPropertyChanged("InvoiceNumber");
                }
            }
        }

        [Column(Storage = "_businessUnit", DbType = "char(256)", UpdateCheck = UpdateCheck.Never)]
        public string BusinessUnit
        {
            get => _businessUnit;
            set
            {
                if (value != null && value.Length > MAX_BUSINESS_UNIT_LENGTH)
                    throw new StringTooLongException("BusinessUnit",
                        MAX_BUSINESS_UNIT_LENGTH);
                if (_businessUnit != value)
                {
                    SendPropertyChanging();
                    _businessUnit = value;
                    SendPropertyChanged("BusinessUnit");
                }
            }
        }

        [Column(Storage = "_distanceFromCrossStreet", DbType = "Decimal(18,2)", UpdateCheck = UpdateCheck.Never)]
        public double? DistanceFromCrossStreet
        {
            get => _distanceFromCrossStreet;
            set
            {
                if (_distanceFromCrossStreet != value)
                {
                    SendPropertyChanging();
                    _distanceFromCrossStreet = value;
                    SendPropertyChanged("DistanceFromCrossStreet");
                }
            }
        }

        [Column(Storage = "_officeAssignmentID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OfficeAssignmentID
        {
            get => _officeAssignmentID;
            set
            {
                if (_officeAssignmentID != value)
                {
                    if (_officeAssignment.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _officeAssignmentID = value;
                SendPropertyChanged("OfficeAssignmentID");
            }
        }

        [Column(Storage = "_assignedContractorID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? AssignedContractorID
        {
            get => _assignedContractorID;
            set
            {
                if (_assignedContractorID != value)
                {
                    if (_assignedContractor.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _assignedContractorID = value;
                SendPropertyChanged("AssignedContractorID");
            }
        }

        [Column(Storage = "_OfficeAssignedOn", DbType = "SmallDateTime")]
        public DateTime? OfficeAssignedOn
        {
            get => _OfficeAssignedOn;
            set
            {
                if (_OfficeAssignedOn != value)
                {
                    SendPropertyChanging();
                    _OfficeAssignedOn = value;
                    SendPropertyChanged("OfficeAssignedOn");
                }
            }
        }

        [Column(Storage = "_assignedToContractorOn", DbType = "SmallDateTime")]
        public DateTime? AssignedToContractorOn
        {
            get => _assignedToContractorOn;
            set
            {
                SendPropertyChanging();
                _assignedToContractorOn = value;
                SendPropertyChanged("AssignedToContractorOn");
            }
        }

        [Column(Storage = "_originalOrderNumber", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OriginalOrderNumber
        {
            get => _originalOrderNumber;
            set
            {
                if (_originalOrderNumber != value)
                {
                    if (_originalOrder.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _originalOrderNumber = value;
                SendPropertyChanged("OriginalOrderNumber");
            }
        }

        [Column(Storage = "_customerImpactRangeID", DbType = "Int")]
        public int? CustomerImpactRangeID
        {
            get => _customerImpactRangeID;
            set
            {
                if (_customerImpactRangeID != value)
                {
                    if (_customerImpactRange.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _customerImpactRangeID = value;
                SendPropertyChanged("CustomerImpactRangeID");
            }
        }

        // PreviousServiceLineMaterialID
        [Column(Storage = "_previousServiceLineMaterialID", DbType = "Int")]
        public int? PreviousServiceLineMaterialID
        {
            get => _previousServiceLineMaterialID;
            set
            {
                if (_previousServiceLineMaterialID != value)
                {
                    if (_previousServiceLineMaterial.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _previousServiceLineMaterialID = value;
                SendPropertyChanged("PreviousServiceLineMaterialID");
            }
        }
        // PreviousServiceLineSizeID
        [Column(Storage = "_previousServiceLineSizeID", DbType = "Int")]
        public int? PreviousServiceLineSizeID
        {
            get => _previousServiceLineSizeID;
            set
            {
                if (_previousServiceLineSizeID != value)
                {
                    if (_previousServiceLineSize.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _previousServiceLineSizeID = value;
                SendPropertyChanged("PreviousServiceLineSizeID");
            }
        }
        // CustomerServiceLineMaterialID
        [Column(Storage = "_customerServiceLineMaterialID", DbType = "Int")]
        public int? CustomerServiceLineMaterialID
        {
            get => _customerServiceLineMaterialID;
            set
            {
                if (_customerServiceLineMaterialID != value)
                {
                    if (_customerServiceLineMaterial.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _customerServiceLineMaterialID = value;
                SendPropertyChanged("CustomerServiceLineMaterialID");
            }
        }
        // CustomerServiceLineSizeID
        [Column(Storage = "_customerServiceLineSizeID", DbType = "Int")]
        public int? CustomerServiceLineSizeID
        {
            get => _customerServiceLineSizeID;
            set
            {
                if (_customerServiceLineSizeID != value)
                {
                    if (_customerServiceLineSize.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _customerServiceLineSizeID = value;
                SendPropertyChanged("CustomerServiceLineSizeID");
            }
        }
        // CompanyServiceLineMaterialID
        [Column(Storage = "_companyServiceLineMaterialID", DbType = "Int")]
        public int? CompanyServiceLineMaterialID
        {
            get => _companyServiceLineMaterialID;
            set
            {
                if (_companyServiceLineMaterialID != value)
                {
                    if (_companyServiceLineMaterial.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _companyServiceLineMaterialID = value;
                SendPropertyChanged("CompanyServiceLineMaterialID");
            }
        }
        // CompanyServiceLineSizeID
        [Column(Storage = "_companyServiceLineSizeID", DbType = "Int")]
        public int? CompanyServiceLineSizeID
        {
            get => _companyServiceLineSizeID;
            set
            {
                if (_companyServiceLineSizeID != value)
                {
                    if (_companyServiceLineSize.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _companyServiceLineSizeID = value;
                SendPropertyChanged("CompanyServiceLineSizeID");
            }
        }

        [Column(Storage = "_alertID", DbType = "VarChar(20)", UpdateCheck = UpdateCheck.Never)]
        public string AlertID
        {
            get => _alertID;
            set
            {
                if (value != null && value.Length > MAX_ALERT_ID_LENGTH)
                    throw new StringTooLongException("AlertID", MAX_ALERT_ID_LENGTH);
                if (_alertID != value)
                {
                    SendPropertyChanging();
                    _alertID = value;
                    SendPropertyChanged("AlertID");
                }
            }
        }

        [Column(Storage = "_repairTimeRangeID", DbType = "Int")]
        public int? RepairTimeRangeID
        {
            get => _repairTimeRangeID;
            set
            {
                if (_repairTimeRangeID != value)
                {
                    if (_repairTimeRange.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _repairTimeRangeID = value;
                SendPropertyChanged("RepairTimeRangeID");
            }
        }

        [Column(Storage = "_significantTrafficImpact", DbType = "Bit")]
        public bool? SignificantTrafficImpact
        {
            get => _significantTrafficImpact;
            set
            {
                if (_significantTrafficImpact != value)
                {
                    SendPropertyChanging();
                    _significantTrafficImpact = value;
                    SendPropertyChanged("SignificantTrafficImpact");
                }
            }
        }

        [Column(Storage = "_markoutToBeCalled", DbType = "SmallDateTime")]
        public DateTime? MarkoutToBeCalled
        {
            get => _markoutToBeCalled;
            set
            {
                if (_markoutToBeCalled != value)
                {
                    SendPropertyChanging();
                    _markoutToBeCalled = value;
                    SendPropertyChanged("MarkoutToBeCalled");
                }
            }
        }

        [Column(Storage = "_updatedMobileGIS", DbType = "Bit", CanBeNull=true)]
        public bool? UpdatedMobileGIS
        {
            get => _updatedMobileGIS;
            set
            {
                if (_updatedMobileGIS != value)
                {
                    SendPropertyChanging();
                    _updatedMobileGIS = value;
                    SendPropertyChanged("UpdatedMobileGIS");
                }
            }
        }

        [Column(Storage = "_requiresInvoice", DbType = "Bit NULL", CanBeNull=true, UpdateCheck = UpdateCheck.Never)]
        public bool? RequiresInvoice
        {
            get => _requiresInvoice;
            set
            {
                if (_requiresInvoice != value)
                {
                    SendPropertyChanging();
                    _requiresInvoice = value;
                    SendPropertyChanged("RequiresInvoice");
                }
            }
        }

        [Column(Storage = "_sapErrorCode", DbType = "TEXT", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string SAPErrorCode
        {
            get => _sapErrorCode;
            set
            {
                if (_sapErrorCode != value)
                {
                    SendPropertyChanging();
                    _sapErrorCode = value;
                    SendPropertyChanged("SAPErrorCode");
                }
            }
        }

        [Column(Storage = "_pitcherFilterCustomerDeliveryOtherMethod", DbType = "varchar(50)",
            CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string PitcherFilterCustomerDeliveryOtherMethod
        {
            get => _pitcherFilterCustomerDeliveryOtherMethod;
            set
            {
                if (_pitcherFilterCustomerDeliveryOtherMethod != value)
                {
                    SendPropertyChanging();
                    _pitcherFilterCustomerDeliveryOtherMethod = value;
                    SendPropertyChanged(nameof(PitcherFilterCustomerDeliveryOtherMethod));
                }
            }
        }

        [Column(Storage = "_acousticMonitoringTypeId", DbType = "Int")]
        public int? AcousticMonitoringTypeId
        {
            get => _acousticMonitoringTypeId;
            set
            {
                if (_acousticMonitoringTypeId != value)
                {
                    if (_acousticMonitoringType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _acousticMonitoringTypeId = value;
                SendPropertyChanged("AcousticMonitoringTypeId");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "AssetType_WorkOrder", Storage = "_assetType", ThisKey = "AssetTypeID", IsForeignKey = true)]
        public AssetType AssetType
        {
            get => _assetType.Entity;
            set
            {
                var previousValue = _assetType.Entity;
                if ((previousValue != value)
                    || (_assetType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _assetType.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _assetType.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _assetTypeID = value.AssetTypeID;
                    }
                    else
                        _assetTypeID = default(int);
                    SendPropertyChanged("AssetType");
                }
            }
        }

        [Association(Name = "Street_WorkOrder", Storage = "_nearestCrossStreet", ThisKey = "NearestCrossStreetID", IsForeignKey = true)]
        public Street NearestCrossStreet
        {
            get => _nearestCrossStreet.Entity;
            set
            {
                var previousValue = _nearestCrossStreet.Entity;
                if ((previousValue != value)
                    || (_nearestCrossStreet.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _nearestCrossStreet.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _nearestCrossStreet.Entity = value;
                    if ((value != null))
                    {
                        value.WorkOrders.Add(this);
                        _nearestCrossStreetID = value.StreetID;
                    }
                    else
                    {
                        _nearestCrossStreetID = default(int?);
                    }
                    SendPropertyChanged("NearestCrossStreet");
                }
            }
        }

        [Association(Name = "Street_WorkOrder1", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
        public Street Street
        {
            get => _street.Entity;
            set
            {
                var previousValue = _street.Entity;
                if ((previousValue != value)
                    || (_street.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _street.Entity = null;
                        previousValue.WorkOrders1.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders1.Add(this);
                        _streetID = value.StreetID;
                    }
                    else
                    {
                        _streetID = default(int?);
                    }
                    SendPropertyChanged("Street");
                }
            }
        }

        [Association(Name = "Town_WorkOrder", Storage = "_tblNJAWTownName", ThisKey = "TownID", IsForeignKey = true)]
        public Town Town
        {
            get => _tblNJAWTownName.Entity;
            set
            {
                var previousValue = _tblNJAWTownName.Entity;
                if (((previousValue != value)
                     || (_tblNJAWTownName.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _tblNJAWTownName.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _tblNJAWTownName.Entity = value;
                    if ((value != null))
                    {
                        value.WorkOrders.Add(this);
                        _townID = value.TownID;
                    }
                    else
                    {
                        _townID = default(int);
                    }
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "TownSection_WorkOrder", Storage = "_tblNJAWTwnSection", ThisKey = "TownSectionID", IsForeignKey = true)]
        public TownSection TownSection
        {
            get => _tblNJAWTwnSection.Entity;
            set
            {
                var previousValue = _tblNJAWTwnSection.Entity;
                if (((previousValue != value)
                     || (_tblNJAWTwnSection.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _tblNJAWTwnSection.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _tblNJAWTwnSection.Entity = value;
                    if ((value != null))
                    {
                        value.WorkOrders.Add(this);
                        _townSectionID = value.TownSectionID;
                    }
                    else
                    {
                        _townSectionID = default(int?);
                    }
                    SendPropertyChanged("TownSection");
                }
            }
        }

        [Association(Name = "WorkOrderPriority_WorkOrder", Storage = "_priority", ThisKey = "PriorityID", IsForeignKey = true)]
        public WorkOrderPriority Priority
        {
            get => _priority.Entity;
            set
            {
                var previousValue = _priority.Entity;
                if (((previousValue != value)
                     || (_priority.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _priority.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _priority.Entity = value;
                    if ((value != null))
                    {
                        value.WorkOrders.Add(this);
                        _priorityID = value.WorkOrderPriorityID;
                    }
                    else
                    {
                        _priorityID = default(int);
                    }
                    SendPropertyChanged("Priority");
                }
            }
        }

        [Association(Name = "WorkOrderPurpose_WorkOrder", Storage = "_drivenBy", ThisKey = "PurposeID", IsForeignKey = true)]
        public WorkOrderPurpose DrivenBy
        {
            get => _drivenBy.Entity;
            set
            {
                var previousValue = _drivenBy.Entity;
                if ((previousValue != value)
                    || (_drivenBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _drivenBy.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _drivenBy.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _purposeID = value.WorkOrderPurposeID;
                    }
                    else
                        _purposeID = default(int);
                    SendPropertyChanged("Purpose");
                }
            }
        }

        [Association(Name = "WorkOrderRequester_WorkOrder", Storage = "_requestedBy", ThisKey = "RequesterID", IsForeignKey = true)]
        public WorkOrderRequester RequestedBy
        {
            get => _requestedBy.Entity;
            set
            {
                var previousValue = _requestedBy.Entity;
                if ((previousValue != value)
                    || (_requestedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _requestedBy.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _requestedBy.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _requesterID = value.WorkOrderRequesterID;
                    }
                    else
                        _requesterID = default(int);
                    SendPropertyChanged("RequestedBy");
                }
            }
        }

        [Association(
            Name = nameof(PitcherFilterCustomerDeliveryOtherMethod) + "_" + nameof(WorkOrder),
            Storage = nameof(_pitcherFilterCustomerDeliveryMethod),
            ThisKey = nameof(PitcherFilterCustomerDeliveryMethodId),
            IsForeignKey = true)]
        public PitcherFilterCustomerDeliveryMethod PitcherFilterCustomerDeliveryMethod
        {
            get => _pitcherFilterCustomerDeliveryMethod.Entity;
            set
            {
                var previousValue = _pitcherFilterCustomerDeliveryMethod.Entity;
                if ((previousValue != value)
                    || (_pitcherFilterCustomerDeliveryMethod.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _pitcherFilterCustomerDeliveryMethod.Entity = null;
                    }
                    _pitcherFilterCustomerDeliveryMethod.Entity = value;
                    if (value != null)
                    {
                        _pitcherFilterCustomerDeliveryMethodId = value.Id;
                    }
                    else
                        _requesterID = default;
                    SendPropertyChanged(nameof(PitcherFilterCustomerDeliveryMethod));
                }
            }
        }

        [Association(Name = "Employee_WorkOrder", Storage = "_requestingEmployee", ThisKey = "RequestingEmployeeID", IsForeignKey = true)]
        public Employee RequestingEmployee
        {
            get => _requestingEmployee.Entity;
            set
            {
                var previousValue = _requestingEmployee.Entity;
                if ((previousValue != value)
                    || (_requestingEmployee.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _requestingEmployee.Entity = null;
                        previousValue.RequestedWorkOrders.Remove(this);
                    }
                    _requestingEmployee.Entity = value;
                    if (value != null)
                    {
                        value.RequestedWorkOrders.Add(this);
                        _requestingEmployeeID = value.EmployeeID;
                    }
                    else
                        _requestingEmployeeID = default(int);
                    SendPropertyChanged("RequestingEmployee");
                }
            }
        }

        [Association(Name = "CreatedBy_WorkOrder", Storage = "_createdBy", ThisKey = "CreatorID", IsForeignKey = true)]
        public Employee CreatedBy
        {
            get => _createdBy.Entity;
            set
            {
                var previousValue = _createdBy.Entity;
                if ((previousValue != value)
                    || (_createdBy.HasLoadedOrAssignedValue == false))
                {
                    OnCreatedByChanging(value);
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _createdBy.Entity = null;
                        previousValue.CreatedWorkOrders.Remove(this);
                    }
                    _createdBy.Entity = value;
                    if (value != null)
                    {
                        value.CreatedWorkOrders.Add(this);
                        _creatorID = value.EmployeeID;
                    }
                    else
                        _creatorID = default(int);
                    SendPropertyChanged("CreatedBy");
                }
            }
        }

        [Association(Name = "InitialFlushTimeEnteredBy_WorkOrder", Storage = nameof(_initialFlushTimeEnteredBy), ThisKey = nameof(InitialFlushTimeEnteredById), IsForeignKey = true)]
        public Employee InitialFlushTimeEnteredBy
        {
            get => _initialFlushTimeEnteredBy.Entity;
            set
            {
                var previousValue = _initialFlushTimeEnteredBy.Entity;
                if ((previousValue != value)
                    || (_initialFlushTimeEnteredBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _initialFlushTimeEnteredBy.Entity = null;
                    }
                    _initialFlushTimeEnteredBy.Entity = value;
                    _creatorID = value?.EmployeeID ?? default(int);
                    SendPropertyChanged(nameof(InitialFlushTimeEnteredBy));
                }
            }
        }

        [Association(Name = "WorkDescription_WorkOrder", Storage = "_workDescription", ThisKey = "WorkDescriptionID", IsForeignKey = true)]
        public WorkDescription WorkDescription
        {
            get => _workDescription.Entity;
            set
            {
                var previousValue = _workDescription.Entity;
                if ((previousValue != value)
                    || (_workDescription.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workDescription.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _workDescription.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _workDescriptionID = value.WorkDescriptionID;
                    }
                    else
                        _workDescriptionID = default(int);
                    SendPropertyChanged("WorkDescription");
                }
            }
        }

        [Association(Name = "MarkoutRequirement_WorkOrder", Storage = "_markoutRequirement", ThisKey = "MarkoutRequirementID", IsForeignKey = true)]
        public MarkoutRequirement MarkoutRequirement
        {
            get => _markoutRequirement.Entity;
            set
            {
                var previousValue = _markoutRequirement.Entity;
                if ((previousValue != value)
                    || (_markoutRequirement.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _markoutRequirement.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _markoutRequirement.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _markoutRequirementID = value.MarkoutRequirementID;
                    }
                    else
                        _markoutRequirementID = default(int);
                    SendPropertyChanged("MarkoutRequirement");
                }
            }
        }

        [Association(Name = "WorkOrder_Markout", Storage = "_markouts", OtherKey = "WorkOrderID")]
        public EntitySet<Markout> Markouts
        {
            get => _markouts;
            set => _markouts.Assign(value);
        }

        [Association(Name = "WorkOrder_Requisition", Storage = "_requisitions", OtherKey = "WorkOrderID")]
        public EntitySet<Requisition> Requisitions
        {
            get => _requisitions;
            set => _requisitions.Assign(value);
        }

        [Association(Name = "WorkOrder_StreetOpeningPermit", Storage = "_streetOpeningPermits", OtherKey = "WorkOrderID")]
        public EntitySet<StreetOpeningPermit> StreetOpeningPermits
        {
            get => _streetOpeningPermits;
            set => _streetOpeningPermits.Assign(value);
        }

        [Association(Name = "WorkOrder_EmployeWorkOrder", Storage = "_employeeWorkOrders", OtherKey = "WorkOrderID")]
        public EntitySet<EmployeeWorkOrder> EmployeeWorkOrders
        {
            get => _employeeWorkOrders;
            set => _employeeWorkOrders.Assign(value);
        }

        [Association(Name = "WorkOrder_LostWater", Storage = "_lostWaters", OtherKey = "WorkOrderID")]
        public EntitySet<LostWater> LostWaters
        {
            get => _lostWaters;
            set => _lostWaters.Assign(value);
        }

        [Association(Name = "WorkOrder_MainBreak", Storage = "_mainBreaks", OtherKey = "WorkOrderID")]
        public EntitySet<MainBreak> MainBreaks
        {
            get => _mainBreaks;
            set => _mainBreaks.Assign(value);
        }

        [Association(Name = "WorkOrder_MaterialsUsed", Storage = "_materialsUseds", OtherKey = "WorkOrderID")]
        public EntitySet<MaterialsUsed> MaterialsUseds
        {
            get => _materialsUseds;
            set => _materialsUseds.Assign(value);
        }

        [Association(Name = "WorkOrder_WorkOrdersScheduleOfValues", Storage = "_workOrdersScheduleOfValues", OtherKey = "WorkOrderID")]
        public EntitySet<WorkOrderScheduleOfValue> WorkOrdersScheduleOfValues
        {
            get => _workOrdersScheduleOfValues;
            set => _workOrdersScheduleOfValues.Assign(value);
        }

        [Association(Name = "WorkOrder_SafetyMarker", Storage = "_safetyMarkers", OtherKey = "WorkOrderID")]
        public EntitySet<SafetyMarker> SafetyMarkers
        {
            get => _safetyMarkers;
            set => _safetyMarkers.Assign(value);
        }

        [Association(Name = "WorkOrder_DetectedLeak", Storage = "_detectedLeaks", OtherKey = "WorkOrderID")]
        public EntitySet<DetectedLeak> DetectedLeaks
        {
            get => _detectedLeaks;
            set => _detectedLeaks.Assign(value);
        }

        [Association(Name = "Valve_WorkOrder", Storage = "_valve", ThisKey = "ValveID", IsForeignKey = true)]
        public Valve Valve
        {
            get => _valve.Entity;
            set
            {
                var previousValue = _valve.Entity;
                if ((previousValue != value)
                    || (_valve.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _valve.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _valve.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _valveID = value.ValveID;
                        SetCoordinateValuesFromAsset(value);
                    }
                    else
                        _valveID = default(int);
                    SendPropertyChanged("Valve");
                }
            }
        }

        [Association(Name = "Service_WorkOrder", Storage = "_service", ThisKey = "ServiceID", IsForeignKey = true)]
        public Service Service
        {
            get => _service.Entity;
            set
            {
                var previousValue = _service.Entity;
                if ((previousValue != value)
                    || (_service.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _service.Entity = null;
                    }
                    _service.Entity = value;
                    if (value != null)
                    {
                        _serviceID = value.Id;
                    }
                    else
                        _serviceID = default(int);
                    SendPropertyChanged("Service");
                }
            }
        }

        [Association(Name = "Hydrant_WorkOrder", Storage = "_hydrant", ThisKey = "HydrantID", IsForeignKey = true)]
        public Hydrant Hydrant
        {
            get => _hydrant.Entity;
            set
            {
                var previousValue = _hydrant.Entity;
                if ((previousValue != value)
                    || (_hydrant.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _hydrant.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _hydrant.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _hydrantID = value.HydrantID;
                        SetCoordinateValuesFromAsset(value);
                    }
                    else
                        _hydrantID = default(int);
                    SendPropertyChanged("Hydrant");
                }
            }
        }

        [Association(Name = "MainCrossing_WorkOrder", Storage = "_mainCrossing", ThisKey = "MainCrossingID", IsForeignKey = true)]
        public MainCrossing MainCrossing
        {
            get => _mainCrossing.Entity;
            set
            {
                var previousValue = _mainCrossing.Entity;
                if (previousValue != value ||_valve.HasLoadedOrAssignedValue == false)
                {
                    SendPropertyChanging();

                    if (previousValue != null)
                    {
                        _mainCrossing.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _mainCrossing.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _mainCrossingID = value.MainCrossingID;
                        SetCoordinateValuesFromAsset(value);
                    }
                    else
                    {
                        _mainCrossingID = default(int);
                    }
                    SendPropertyChanged("MainCrossing");
                }
            }
        }

        [Association(Name = "SewerOpening_WorkOrder", Storage = "_sewerOpening", ThisKey="SewerOpeningID", IsForeignKey = true)]
        public SewerOpening SewerOpening
        {
            get => _sewerOpening.Entity;
            set
            {
                var previousValue = _sewerOpening.Entity;
                if ((previousValue!=value) || (_sewerOpening.HasLoadedOrAssignedValue==false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _sewerOpening.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _sewerOpening.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _sewerOpeningID = value.Id;
                        SetCoordinateValuesFromAsset(value);
                    }
                    else
                        _sewerOpeningID = default(int);
                    SendPropertyChanged("SewerOpening");
                }
            }
        }

        [Association(Name = "StormCatch_WorkOrder", Storage = "_stormCatch", ThisKey="StormCatchID", IsForeignKey = true)]
        public StormCatch StormCatch
        {
            get => _stormCatch.Entity;
            set
            {
                var previousValue = _stormCatch.Entity;
                if ((previousValue != value) || (_stormCatch.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _stormCatch.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _stormCatch.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _stormCatchID = value.StormCatchID;
                        SetCoordinateValuesFromAsset(value);
                    }
                    else
                        _stormCatchID = default(int);
                    SendPropertyChanged("StormCatch");
                }                
            }
        }

        [Association(Name = "Equipment_WorkOrder", Storage = "_equipment", ThisKey = "EquipmentID", IsForeignKey = true)]
        public Equipment Equipment
        {
            get => _equipment.Entity;
            set
            {
                var previousValue = _equipment.Entity;
                if ((previousValue != value) || (_equipment.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _equipment.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _equipment.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _equipmentID = value.EquipmentID;
                        SetCoordinateValuesFromAsset(value);
                    }
                    else
                        _equipmentID = default(int);
                    SendPropertyChanged("Equipment");
                }
            }
        }

        [Association(Name = "OperatingCenter_WorkOrder", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get => _operatingCenter.Entity;
            set
            {
                var previousValue = _operatingCenter.Entity;
                if ((previousValue != value)
                    || (_operatingCenter.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _operatingCenter.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "WorkOrder_WorkOrderDescriptionChange", Storage = "_workOrderDescriptionChanges", OtherKey = "WorkOrderID")]
        public EntitySet<WorkOrderDescriptionChange> WorkOrderDescriptionChanges
        {
            get => _workOrderDescriptionChanges;
            set => _workOrderDescriptionChanges.Assign(value);
        }

        [Association(Name = "WorkOrder_CrewAssignment", Storage = "_crewAssignments", OtherKey = "WorkOrderID")]
        public EntitySet<CrewAssignment> CrewAssignments
        {
            get => _crewAssignments;
            set => _crewAssignments.Assign(value);
        }

        [Association(Name = "WorkOrder_Restoration", Storage = "_restorations", OtherKey = "WorkOrderID")]
        public EntitySet<Restoration> Restorations
        {
            get => _restorations;
            set => _restorations.Assign(value);
        }

        [Association(Name = "MaterialsApprovedBy_WorkOrder", Storage = "_materialsApprovedBy", ThisKey = "MaterialsApprovedByID", IsForeignKey = true)]
        public Employee MaterialsApprovedBy
        {
            get => _materialsApprovedBy.Entity;
            set
            {
                Employee previousValue = _materialsApprovedBy.Entity;
                if ((previousValue != value)
                    || (_materialsApprovedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _materialsApprovedBy.Entity = null;
                        previousValue.ApprovedMaterialsWorkOrders.Remove(this);
                    }
                    _materialsApprovedBy.Entity = value;
                    if (value != null)
                    {
                        value.ApprovedMaterialsWorkOrders.Add(this);
                        _materialsApprovedByID = value.EmployeeID;
                    }
                    else
                        _materialsApprovedByID = default(int);
                    SendPropertyChanged("MaterialsApprovedBy");
                }
            }
        }

        [Association(Name = "ApprovedBy_WorkOrder", Storage = "_approvedBy", ThisKey = "ApprovedByID", IsForeignKey = true)]
        public Employee ApprovedBy
        {
            get => _approvedBy.Entity;
            set
            {
                Employee previousValue = _approvedBy.Entity;
                if ((previousValue != value)
                    || (_approvedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _approvedBy.Entity = null;
                        previousValue.ApprovedWorkOrders.Remove(this);
                    }
                    _approvedBy.Entity = value;
                    if (value != null)
                    {
                        value.ApprovedWorkOrders.Add(this);
                        _approvedByID = value.EmployeeID;
                    }
                    else
                        _approvedByID = default(int);
                    SendPropertyChanged("ApprovedBy");
                }
            }
        }

        [Association(Name = "WorkOrder_Spoil", Storage = "_spoils", OtherKey = "WorkOrderID")]
        public EntitySet<Spoil> Spoils
        {
            get => _spoils;
            set => _spoils.Assign(value);
        }

        [Association(Name = "CompletedBy_WorkOrder", Storage = "_completedBy", ThisKey = "CompletedByID", IsForeignKey = true)]
        public Employee CompletedBy
        {
            get => _completedBy.Entity;
            set
            {
                Employee previousValue = _completedBy.Entity;
                if ((previousValue != value)
                    || (_completedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _completedBy.Entity = null;
                        previousValue.WorkOrdersCompleted.Remove(this);
                    }
                    _completedBy.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrdersCompleted.Add(this);
                        _completedByID = value.EmployeeID;
                    }
                    else
                        _completedByID = default(int);
                    SendPropertyChanged("CompletedBy");
                }
            }
        }

        [Association(Name = "CancelledBy_WorkOrder", Storage = "_cancelledBy", ThisKey = "CancelledByID", IsForeignKey = true)]
        public Employee CancelledBy
        {
            get => _cancelledBy.Entity;
            set
            {
                Employee previousValue = _cancelledBy.Entity;
                if ((previousValue != value)
                    || (_cancelledBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _cancelledBy.Entity = null;
                        previousValue.WorkOrdersCancelled.Remove(this);
                    }
                    _cancelledBy.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrdersCancelled.Add(this);
                        _cancelledByID = value.EmployeeID;
                    }
                    else
                        _cancelledByID = default(int);
                    SendPropertyChanged("CancelledBy");
                }
            }
        }

        [Association(Name = "WorkOrderCancellationReason_WorkOrder", Storage = "_workOrderCancellationReason", ThisKey = "WorkOrderCancellationReasonID", IsForeignKey = true)]
        public WorkOrderCancellationReason WorkOrderCancellationReason
        {
            get => _workOrderCancellationReason.Entity;
            set
            {
                WorkOrderCancellationReason previousValue = _workOrderCancellationReason.Entity;
                if ((previousValue != value) || (_workOrderCancellationReason.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrderCancellationReason.Entity = null;
                        //previousValue.WorkOrders.Remove(this);
                    }
                    _workOrderCancellationReason.Entity = value;
                    if (value != null)
                    {
                        //value.WorkOrders.Add(this);
                        _workOrderCancellationReasonID = value.Id;
                    }
                    else
                    {
                        _workOrderCancellationReasonID = default(int);
                    }
                    SendPropertyChanged("WorkOrderCancellationReason");
                }
            }
        }

        [Association(Name = "SAPWorkOrderStep_WorkOrder", Storage = "_sapWorkOrderStep", ThisKey = "SAPWorkOrderStepID", IsForeignKey = true)]
        public SAPWorkOrderStep SAPWorkOrderStep
        {
            get => _sapWorkOrderStep.Entity;
            set
            {
                SAPWorkOrderStep previousValue = _sapWorkOrderStep.Entity;
                if ((previousValue != value) || (_sapWorkOrderStep.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _sapWorkOrderStep.Entity = null;
                    }
                    _sapWorkOrderStep.Entity = value;
                    if (value != null)
                    {
                        _sapWorkOrderStepID = value.Id;
                    }
                    else
                    {
                        _sapWorkOrderStepID = default(int);
                    }
                    SendPropertyChanged("SAPWorkOrderStep");
                }
            }
        }
        
        [Column(Storage = "_plantMaintenanceActivityTypeOverrideID", DbType = "Int", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public int? PlantMaintenanceActivityTypeOverrideID
        {
            get => _plantMaintenanceActivityTypeOverrideID;
            set
            {
                if (_plantMaintenanceActivityTypeOverrideID != value)
                {
                    if (_plantMaintenanceActivityTypeOverride.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _plantMaintenanceActivityTypeOverrideID = value;
                SendPropertyChanged("PlantMaintenanceActivityTypeOverrideID");
            }
        }

        [Association(Name= "PlantMaintenanceActivityTypeOverride_WorkOrder", Storage = "_plantMaintenanceActivityTypeOverride", ThisKey = "PlantMaintenanceActivityTypeOverrideID", IsForeignKey = true)]
        public PlantMaintenanceActivityType PlantMaintenanceActivityTypeOverride
        {
            get => _plantMaintenanceActivityTypeOverride.Entity;
            set
            {
                var previousValue = _plantMaintenanceActivityTypeOverride.Entity;
                if ((previousValue != value) || (_plantMaintenanceActivityTypeOverride.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _plantMaintenanceActivityTypeOverride.Entity = null;
                    }
                    _plantMaintenanceActivityTypeOverride.Entity = value;
                    if (value != null)
                    {
                        _plantMaintenanceActivityTypeOverrideID = value.Id;
                    }
                    else
                    {
                        _plantMaintenanceActivityTypeOverrideID = default(int);
                    }
                    SendPropertyChanged("PlantMaintenanceActivityTypeOverride");
                }
            }
        }

        [Association(Name = "WorkOrder_OrcomOrderCompletion", Storage = "_orcomOrderCompletions", OtherKey = "WorkOrderID")]
        public EntitySet<OrcomOrderCompletion> OrcomOrderCompletions
        {
            get => _orcomOrderCompletions;
            set => _orcomOrderCompletions.Assign(value);
        }

        [Association(Name = "WorkOrder_DocumentWorkOrder", Storage = "_documentsWorkOrders", OtherKey = "WorkOrderID")]
        public EntitySet<DocumentWorkOrder> DocumentsWorkOrders
        {
            get => _documentsWorkOrders;
            set => _documentsWorkOrders.Assign(value);
        }

        [Association(Name = "OfficeAssignment_WorkOrder", Storage = "_officeAssignment", ThisKey = "OfficeAssignmentID", IsForeignKey = true)]
        public Employee OfficeAssignment
        {
            get => _officeAssignment.Entity;
            set
            {
                var previousValue = _officeAssignment.Entity;
                if ((previousValue != value)
                    || (_officeAssignment.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _officeAssignment.Entity = null;
                        previousValue.OfficeAssignedWorkOrders.Remove(this);
                    }
                    _officeAssignment.Entity = value;
                    if (value != null)
                    {
                        value.OfficeAssignedWorkOrders.Add(this);
                        _officeAssignmentID = value.EmployeeID;
                    }
                    else
                        _officeAssignmentID = default(int);
                    SendPropertyChanged("OfficeAssignment");
                }
            }
        }

        [Association(Name = "AssignedContractor_WorkOrder", Storage = "_assignedContractor", ThisKey = "AssignedContractorID", IsForeignKey = true)]
        public Contractor AssignedContractor
        {
            get => _assignedContractor.Entity;
            set { 
                var previousValue = _assignedContractor.Entity;
                
                if (previousValue != value || _assignedContractor.HasLoadedOrAssignedValue == false)
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _assignedContractor.Entity = null;
                        previousValue.AssignedWorkOrders.Remove(this);
                    }
                    _assignedContractor.Entity = value;
                    if (value != null)
                    {
                        value.AssignedWorkOrders.Add(this);
                        _assignedContractorID = value.ContractorID;
                    }
                    else
                        _assignedContractorID = default;
                    SendPropertyChanged("AssignedContractor");
                }
            }
        }

        [Association(Name = "OriginalOrder_WorkOrder", Storage = "_originalOrder", ThisKey="OriginalOrderNumber", IsForeignKey = true)]
        public WorkOrder OriginalOrder
        {
            get => _originalOrder.Entity;
            set
            {
                var previousValue = _originalOrder.Entity;
                if ((previousValue != value)
                    || (_originalOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _originalOrder.Entity = null;
                        previousValue.ChildOrders.Remove(this);
                    }
                    _originalOrder.Entity = value;
                    if (value != null)
                    {
                        value.ChildOrders.Add(this);
                        _originalOrderNumber = value.WorkOrderID;
                    }
                    else
                        _originalOrderNumber = default(int);
                    SendPropertyChanged("OriginalOrder");
                }
            }
        }

        [Association(Name = "AcousticMonitoringType_WorkOrder", Storage = "_acousticMonitoringType", ThisKey = "AcousticMonitoringTypeId", IsForeignKey = true)]
        public AcousticMonitoringType AcousticMonitoringType
        {
            get => _acousticMonitoringType.Entity;
            set
            {
                var previousValue = _acousticMonitoringType.Entity;
                if ((previousValue != value)
                    || (_acousticMonitoringType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _acousticMonitoringType.Entity = null;
                    }
                    _acousticMonitoringType.Entity = value;
                    if (value != null)
                    {
                        _acousticMonitoringTypeId = value.Id;
                    }
                    else
                        _acousticMonitoringTypeId = default(int);
                    SendPropertyChanged("AcousticMonitoringType");
                }
            }
        }

        [Association(Name = "WorkOrder_ChildOrder", Storage = "_childOrders", OtherKey="OriginalOrderNumber")]
        public EntitySet<WorkOrder> ChildOrders
        {
            get => _childOrders;
            set => _childOrders.Assign(value);
        }

        [Association(Name = "WorkOrder_JobSiteCheckList", Storage = "_jobSiteCheckLists", OtherKey = "WorkOrderID")]
        public EntitySet<JobSiteCheckList> JobSiteCheckLists => _jobSiteCheckLists;

        [Association(Name = "WorkOrder_JobObservation", Storage = "_jobObservations", OtherKey = "WorkOrderID")]
        public EntitySet<JobObservation> JobObservations => _jobObservations;

        [Association(Name = "WorkOrder_WorkOrderInvoices", Storage = "_workOrderInvoices", OtherKey = "WorkOrderID")]
        public EntitySet<WorkOrderInvoice> WorkOrderInvoices => _workOrderInvoices;

        [Association(Name = "WorkOrder_TrafficControl", Storage = "_trafficControl", OtherKey = "WorkOrderId")]
        public EntitySet<TrafficControlTicket> TrafficControl => _trafficControl;

        [Association(Name = "WorkOrder_MarkoutViolations", Storage = "_markoutViolations", OtherKey = "WorkOrderId")]
        public EntitySet<MarkoutViolation> MarkoutViolations => _markoutViolations;

        [Association(Name = "WorkOrder_NewServiceInstallation", Storage = "_newServiceInstallation", OtherKey = "WorkOrderId")]
        public EntitySet<NewServiceInstallation> NewServiceInstallation => _newServiceInstallation;

        public string EstimatedCustomerImpact => CustomerImpactRange?.Description;

        [Association(Name = "CustomerImpactRange_WorkOrder", Storage = "_customerImpactRange", ThisKey = "CustomerImpactRangeID", IsForeignKey = true)]
        public CustomerImpactRange CustomerImpactRange
        {
            get => _customerImpactRange.Entity;
            set
            {
                CustomerImpactRange previousValue = _customerImpactRange.Entity;
                if ((previousValue != value)
                    || (_customerImpactRange.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _customerImpactRange.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _customerImpactRange.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _customerImpactRangeID = value.CustomerImpactRangeID;
                    }
                    else
                        _customerImpactRangeID = default(int);
                    SendPropertyChanged("CustomerImpactRange");
                }
            }
        }

        // PreviousServiceLineMaterial
        [Association(Name = "PreviousServiceLineMaterial_WorkOrder", Storage = "_previousServiceLineMaterial", ThisKey = "PreviousServiceLineMaterialID", IsForeignKey = true)]
        public ServiceMaterial PreviousServiceLineMaterial
        {
            get => _previousServiceLineMaterial.Entity;
            set
            {
                ServiceMaterial previousValue = _previousServiceLineMaterial.Entity;
                if ((previousValue != value)
                    || (_previousServiceLineMaterial.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _previousServiceLineMaterial.Entity = null;
                        //previousValue.WorkOrders.Remove(this);
                    }
                    _previousServiceLineMaterial.Entity = value;
                    if (value != null)
                    {
                        //value.WorkOrders.Add(this);
                        _previousServiceLineMaterialID = value.ServiceMaterialID;
                    }
                    else
                        _previousServiceLineMaterialID = default(int);
                    SendPropertyChanged("PreviousServiceLineMaterial");
                }
            }
        }
        // PreviousServiceLineSize
        [Association(Name = "PreviousServiceLineSize_WorkOrder", Storage = "_previousServiceLineSize", ThisKey = "PreviousServiceLineSizeID", IsForeignKey = true)]
        public ServiceSize PreviousServiceLineSize
        {
            get => _previousServiceLineSize.Entity;
            set
            {
                ServiceSize previousValue = _previousServiceLineSize.Entity;
                if ((previousValue != value)
                    || (_previousServiceLineSize.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _previousServiceLineSize.Entity = null;
                        //previousValue.WorkOrders.Remove(this);
                    }
                    _previousServiceLineSize.Entity = value;
                    if (value != null)
                    {
                        //value.WorkOrders.Add(this);
                        _previousServiceLineSizeID = value.ServiceSizeID;
                    }
                    else
                        _previousServiceLineSizeID = default(int);
                    SendPropertyChanged("PreviousServiceLineSize");
                }
            }
        }
        // CustomerServiceLineMaterial
        [Association(Name = "CustomerServiceLineMaterial_WorkOrder", Storage = "_customerServiceLineMaterial", ThisKey = "CustomerServiceLineMaterialID", IsForeignKey = true)]
        public ServiceMaterial CustomerServiceLineMaterial
        {
            get => _customerServiceLineMaterial.Entity;
            set
            {
                ServiceMaterial customerValue = _customerServiceLineMaterial.Entity;
                if ((customerValue != value)
                    || (_customerServiceLineMaterial.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (customerValue != null)
                    {
                        _customerServiceLineMaterial.Entity = null;
                        //customerValue.WorkOrders.Remove(this);
                    }
                    _customerServiceLineMaterial.Entity = value;
                    if (value != null)
                    {
                        //value.WorkOrders.Add(this);
                        _customerServiceLineMaterialID = value.ServiceMaterialID;
                    }
                    else
                        _customerServiceLineMaterialID = default(int);
                    SendPropertyChanged("CustomerServiceLineMaterial");
                }
            }
        }
        // CustomerServiceLineSize
        [Association(Name = "CustomerServiceLineSize_WorkOrder", Storage = "_customerServiceLineSize", ThisKey = "CustomerServiceLineSizeID", IsForeignKey = true)]
        public ServiceSize CustomerServiceLineSize
        {
            get => _customerServiceLineSize.Entity;
            set
            {
                ServiceSize customerValue = _customerServiceLineSize.Entity;
                if ((customerValue != value)
                    || (_customerServiceLineSize.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (customerValue != null)
                    {
                        _customerServiceLineSize.Entity = null;
                        //customerValue.WorkOrders.Remove(this);
                    }
                    _customerServiceLineSize.Entity = value;
                    if (value != null)
                    {
                        //value.WorkOrders.Add(this);
                        _customerServiceLineSizeID = value.ServiceSizeID;
                    }
                    else
                        _customerServiceLineSizeID = default(int);
                    SendPropertyChanged("CustomerServiceLineSize");
                }
            }
        }
        // CompanyServiceLineMaterial
        [Association(Name = "CompanyServiceLineMaterial_WorkOrder", Storage = "_companyServiceLineMaterial", ThisKey = "CompanyServiceLineMaterialID", IsForeignKey = true)]
        public ServiceMaterial CompanyServiceLineMaterial
        {
            get => _companyServiceLineMaterial.Entity;
            set
            {
                ServiceMaterial customerValue = _companyServiceLineMaterial.Entity;
                if ((customerValue != value)
                    || (_companyServiceLineMaterial.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (customerValue != null)
                    {
                        _companyServiceLineMaterial.Entity = null;
                    }
                    _companyServiceLineMaterial.Entity = value;
                    if (value != null)
                    {
                        _companyServiceLineMaterialID = value.ServiceMaterialID;
                    }
                    else
                        _companyServiceLineMaterialID = default(int);
                    SendPropertyChanged("CompanyServiceLineMaterial");
                }
            }
        }
        // CompanyServiceLineSize
        [Association(Name = "CompanyServiceLineSize_WorkOrder", Storage = "_companyServiceLineSize", ThisKey = "CompanyServiceLineSizeID", IsForeignKey = true)]
        public ServiceSize CompanyServiceLineSize
        {
            get => _companyServiceLineSize.Entity;
            set
            {
                ServiceSize customerValue = _companyServiceLineSize.Entity;
                if ((customerValue != value)
                    || (_companyServiceLineSize.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (customerValue != null)
                    {
                        _companyServiceLineSize.Entity = null;
                    }
                    _companyServiceLineSize.Entity = value;
                    if (value != null)
                    {
                        _companyServiceLineSizeID = value.ServiceSizeID;
                    }
                    else
                        _companyServiceLineSizeID = default(int);
                    SendPropertyChanged("CompanyServiceLineSize");
                }
            }
        }
        public string AnticipatedRepairTime => RepairTimeRange?.Description;
        [Association(Name = "RepairTimeRange_WorkOrder", Storage = "_repairTimeRange", ThisKey = "RepairTimeRangeID", IsForeignKey = true)]
        public RepairTimeRange RepairTimeRange
        {
            get => _repairTimeRange.Entity;
            set
            {
                RepairTimeRange previousValue = _repairTimeRange.Entity;
                if ((previousValue != value)
                    || (_repairTimeRange.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _repairTimeRange.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _repairTimeRange.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _repairTimeRangeID = value.RepairTimeRangeID;
                    }
                    else
                        _repairTimeRangeID = default(int);
                    SendPropertyChanged("RepairTimeRange");
                }
            }
        }

        [Association(Name = "MarkoutTypeNeeded_WorkOrder", Storage = "_markoutTypeNeeded", ThisKey = "MarkoutTypeNeededID", IsForeignKey = true)]
        public MarkoutType MarkoutTypeNeeded
        {
            get => _markoutTypeNeeded.Entity;
            set
            {
                MarkoutType previousValue = _markoutTypeNeeded.Entity;
                if ((previousValue != value)
                    || (_markoutTypeNeeded.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _markoutTypeNeeded.Entity = null;
                        previousValue.WorkOrders.Remove(this);
                    }
                    _markoutTypeNeeded.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrders.Add(this);
                        _markoutTypeNeededID = value.MarkoutTypeID;
                    }
                    else
                        _markoutTypeNeededID = default(int);
                    SendPropertyChanged("MarkoutTypeNeeded");
                }
            }
        }

        [Association(Name = "FlushingNoticeType_WorkOrder", Storage = "_flushingNoticeType", ThisKey = "FlushingNoticeTypeId", IsForeignKey = true)]
        public FlushingNoticeType FlushingNoticeType
        {
            get => _flushingNoticeType.Entity;
            set
            {
                FlushingNoticeType previousValue = _flushingNoticeType.Entity;
                if ((previousValue != value)
                    || (_flushingNoticeType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _flushingNoticeType.Entity = null;
                    }
                    _flushingNoticeType.Entity = value;
                    _flushingNoticeTypeId = value != null ? value.Id : default(int);
                    SendPropertyChanged("FlushingNoticeType");
                }
            }
        }

        [Column(Storage = "_apartmentAddtl", DbType = "VarChar(30)", CanBeNull=true, UpdateCheck = UpdateCheck.Never)]
        public string ApartmentAddtl
        {
            get => _apartmentAddtl;
            set
            {
                if (value != null && value.Length > MAX_APARTMENTADDTL_LENGTH)
                    throw new StringTooLongException("ApartmentAddtl", MAX_APARTMENTADDTL_LENGTH);
                if (_apartmentAddtl != value)
                {
                    SendPropertyChanging();
                    _apartmentAddtl = value;
                    SendPropertyChanged("ApartmentAddtl");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public WorkOrder()
        {
            _jobSiteCheckLists = new EntitySet<JobSiteCheckList>();
            _jobObservations = new EntitySet<JobObservation>();
            _workOrderInvoices = new EntitySet<WorkOrderInvoice>();
            _trafficControl = new EntitySet<TrafficControlTicket>();
            _markoutViolations = new EntitySet<MarkoutViolation>();
            _newServiceInstallation = new EntitySet<NewServiceInstallation>();
            
            _lostWaters = new EntitySet<LostWater>(attach_LostWaters,
                detach_LostWaters);
            _mainBreaks = new EntitySet<MainBreak>(attach_MainBreaks,
                detach_MainBreaks);
            _markouts = new EntitySet<Markout>(attach_Markouts, detach_Markouts);
            _requisitions = new EntitySet<Requisition>(attach_Requisitions, detach_Requisitions);
            _streetOpeningPermits = new EntitySet<StreetOpeningPermit>(attach_StreetOpeningPermits, detach_StreetOpeningPermits);
            _materialsUseds = new EntitySet<MaterialsUsed>(
                attach_MaterialsUseds, detach_MaterialsUseds);
            _workOrdersScheduleOfValues = new EntitySet<WorkOrderScheduleOfValue>(
                attach_WorkOrderScheduleOfValue, detach_WorkOrderScheduleOfValue);
            _restorations = new EntitySet<Restoration>(attach_Restorations,
                detach_Restorations);
            _safetyMarkers = new EntitySet<SafetyMarker>(attach_SafetyMarkers,
                detach_SafetyMarkers);
            _spoils = new EntitySet<Spoil>(attach_Spoils, detach_Spoils);
            _detectedLeaks = new EntitySet<DetectedLeak>(attach_DetectedLeaks,
                detach_DetectedLeaks);
            _employeeWorkOrders =
                new EntitySet<EmployeeWorkOrder>(attach_EmployeeWorkOrders,
                    detach_EmployeeWorkOrders);
            _workOrderDescriptionChanges =
                new EntitySet<WorkOrderDescriptionChange>(
                    attach_WorkOrderDescriptionChanges,
                    detach_WorkOrderDescriptionChanges);
            _crewAssignments = new EntitySet<CrewAssignment>(attach_CrewAssignments, detach_CrewAssignments);
            _street = default(EntityRef<Street>);
            _nearestCrossStreet = default(EntityRef<Street>);
            _tblNJAWTownName = default(EntityRef<Town>);
            _tblNJAWTwnSection = default(EntityRef<TownSection>);
            _priority = default(EntityRef<WorkOrderPriority>);
            _drivenBy = default(EntityRef<WorkOrderPurpose>);
            _requestedBy = default(EntityRef<WorkOrderRequester>);
            _pitcherFilterCustomerDeliveryMethod = default;
            _requestingEmployee = default(EntityRef<Employee>);
            _createdBy = default(EntityRef<Employee>);
            _initialFlushTimeEnteredBy = default;
            _assetType = default(EntityRef<AssetType>);
            _workDescription = default(EntityRef<WorkDescription>);
            _markoutRequirement = default(EntityRef<MarkoutRequirement>);
            _valve = default(EntityRef<Valve>);
            _hydrant = default(EntityRef<Hydrant>);
            _service = default(EntityRef<Service>);
            _mainCrossing = default(EntityRef<MainCrossing>);
            _sewerOpening = default(EntityRef<SewerOpening>);
            _stormCatch = default(EntityRef<StormCatch>);
            _equipment = default(EntityRef<Equipment>);
            _approvedBy = default(EntityRef<Employee>);
            _materialsApprovedBy = default(EntityRef<Employee>);
            _operatingCenter = default(EntityRef<OperatingCenter>);
            _originalOrder = default(EntityRef<WorkOrder>);
            _orcomOrderCompletions = new EntitySet<OrcomOrderCompletion>(attach_OrcomOrderCompletions, detach_OrcomOrderCompletions);
            _documentsWorkOrders = new EntitySet<DocumentWorkOrder>(attach_DocumentsWorkOrders, detach_DocumentsWorkOrders);
            _childOrders = new EntitySet<WorkOrder>(attach_ChildOrders,detach_ChildOrders);
            _previousServiceLineMaterial = default(EntityRef<ServiceMaterial>);
            _previousServiceLineSize = default(EntityRef<ServiceSize>);
            _customerServiceLineMaterial = default(EntityRef<ServiceMaterial>);
            _customerServiceLineSize = default(EntityRef<ServiceSize>);
        }

        #endregion

        #region Private Methods

        #region Phase Determination

        private bool MeetsInputRequirements()
        {
            if (OperatingCenter.SAPEnabled && OperatingCenter.SAPWorkOrdersEnabled &&
                !OperatingCenter.IsContractedOperations && CreatedOn != null &&
                !SAPWorkOrderNumber.HasValue)
                return false;
            
#if DEBUG
            var addressValid = ValidateAddress();
            var requesterValid = ValidateRequesterInformation();
            var descriptionValid = ValidateWorkDescription();
            var assetValid = ValidateAssetInformation();

            return addressValid && requesterValid && descriptionValid &&
                   assetValid;
#else
            return (ValidateAddress() &&
                    ValidateRequesterInformation() &&
                    ValidateWorkDescription() &&
                    ValidateAssetInformation());
#endif
        }

        private bool MeetsPlanningRequirements()
        {
            if (MarkoutRequired)
            {
                var currentMarkout = Markouts.GetCurrent();
                if (currentMarkout == null || currentMarkout.IsExpired)
                    return false;
            }
            return true;
        }

        private bool MeetsSchedulingRequirements()
        {
            var current = CrewAssignments.GetCurrent();
            if (current != null && current.AssignedFor.Date <= DateTime.Today.Date)
                return true;
            return false;
        }

        #endregion

        #region Validation/Verification

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                    ValidateCreationInfo();
                    if (CreatedOn == DateTime.MinValue)
                        CreatedOn = DateTime.Now;
                    VerifyAddress();
                    VerifyRequesterInformation();
                    VerifyWorkDescription();
                    VerifyAssetInformation();
                    VerifyCoordinateInformation();
                    break;
                case ChangeAction.Update:
                    VerifyAddress();
                    VerifyRequesterInformation();
                    VerifyWorkDescription();
                    VerifyMarkoutNecessity();
                    VerifyInvoicingItems();
                    VerifyPMATOverride();
                    if (WorkDescription.IsMainReplaceOrRepair && DateCompleted != null &&
                        Phase == WorkOrderPhase.Finalization && MainBreaks.Count == 0)
                    {
                        //Check that we have at least one MainBreak
                        throw new DomainLogicException(
                            "WorkOrder must have at least one MainBreak object if the WorkDescription is a Main replace or repair.");
                    }
                    VerifyAssetInformation();
                    VerifyCoordinateInformation();
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void ValidateCreationInfo()
        {
            if (Priority == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for Priority.");
            if (Town == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for Town.");
            if (CreatedBy == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for CreatedBy.");
            if (DrivenBy == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for DrivenBy.");
            if (MarkoutRequirement == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for MarkoutRequirement.");
            if (RequestedBy == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for RequestedBy.");
            if (WorkDescription == null)
                throw new DomainLogicException(
                    "Cannot create a WorkOrder without a value for WorkDescription.");
            if (OperatingCenter == null)
                throw new DomainLogicException("Cannot create a WorkOrder without a value for OperatingCenter.");
        }

        private bool ValidateAddress()
        {
            return AddressVerifier.Test(this);
        }

        private void VerifyAddress()
        {
            AddressVerifier.Verify(this);
        }

        private bool ValidateRequesterInformation()
        {
            if (RequestedBy.WorkOrderRequesterID == WorkOrderRequesterRepository.Indices.CUSTOMER)
            {
                if (String.IsNullOrEmpty(CustomerName) || String.IsNullOrEmpty(StreetNumber))
                    return false;
            }
            else if (RequestedBy.WorkOrderRequesterID == WorkOrderRequesterRepository.Indices.EMPLOYEE)
            {
                if (RequestingEmployeeID == null)
                    return false;
            }
            return true;
        }

        private void VerifyRequesterInformation()
        {
            if (RequestedBy.WorkOrderRequesterID == WorkOrderRequesterRepository.Indices.CUSTOMER)
            {
                // not checking phone number
                if (String.IsNullOrEmpty(CustomerName) ||
                    String.IsNullOrEmpty(StreetNumber))
                    throw new DomainLogicException(
                        "Cannot save a work order without full customer information when requested by customer.");
            }
            // TODO:
            // Requesting Employee is currently broken
            else if (RequestedBy.WorkOrderRequesterID == WorkOrderRequesterRepository.Indices.EMPLOYEE)
            {
                if (RequestingEmployee == null)
                    throw new DomainLogicException(
                        "Cannot save a work order without requesting employee when requested by employee.");
            }
        }

        private bool ValidateWorkDescription()
        {
            return WorkDescription.AssetType == AssetType;
        }

        private void VerifyWorkDescription()
        {
            if (!ValidateWorkDescription())
                throw new DomainLogicException("The selected Work Description for a Work Order must match the Asset Type chosen.");
        }

        private bool ValidateAssetInformation()
        {
            switch (AssetType.AssetTypeID)
            {
                case AssetTypeRepository.Indices.VALVE:
                    return (ValveID != null);
                case AssetTypeRepository.Indices.HYDRANT:
                    return (HydrantID != null);
                case AssetTypeRepository.Indices.MAIN_CROSSING:
                    return (MainCrossingID != null);
            }
            return true;
        }

        private void VerifyAssetInformation()
        {
            switch (AssetType.AssetTypeID)
            {
                case AssetTypeRepository.Indices.VALVE:
                    if (ValveID == null)
                        throw new DomainLogicException(
                            "Cannot save a WorkOrder with AssetType set to Valve without attaching a Valve object.");
                    break;
                case AssetTypeRepository.Indices.HYDRANT:
                    if (HydrantID == null)
                        throw new DomainLogicException(
                            "Cannot save a WorkOrder with AssetType set to Hydrant without attaching a Hydrant object.");
                    break;
                case AssetTypeRepository.Indices.MAIN_CROSSING: 
                    if (MainCrossingID == null)
                        throw new DomainLogicException(
                            "Cannot save a WorkOrder with AssetType set to MainCrossing without attaching a Main Crossing object.");
                    break;
                case AssetTypeRepository.Indices.MAIN:
                    if (Phase == WorkOrderPhase.Finalization)
                    {

                        if (WorkDescriptionRepository.MAIN_BREAKS.Contains(WorkDescription.WorkDescriptionID) && DateCompleted != null
                            && (CustomerImpactRange == null || RepairTimeRange == null || SignificantTrafficImpact == null || LostWater == null))
                            throw new DomainLogicException(
                                String.Format("Cannot save a WorkOrder with a main break WorkDescription and no value for CustomerImpactRange, RepairTimeRange, CustomerAlert, SignificantTrafficImpact, or LostWater during {0}", Phase));
                    }
                    else
                    {
                        if (WorkDescriptionRepository.MAIN_BREAKS.Contains(WorkDescription.WorkDescriptionID)
                            && (CustomerImpactRange == null || RepairTimeRange == null || SignificantTrafficImpact == null))
                            throw new DomainLogicException(
                                String.Format("Cannot save a WorkOrder with a main break WorkDescription and no value for CustomerImpactRange, RepairTimeRange, CustomerAlert, or SignificantTrafficImpact during {0}", Phase));
                    }
                    break;
            }
            
            if (PremiseNumber != null && new Regex("^(\\d)\\1{8,9}$").IsMatch(PremiseNumber) &&
                (Notes == null || Notes.Length < 5))
            {
                throw new DomainLogicException(
                    "Cannot save a WorkOrder using a \"place holder\" premise number without entering notes.");
            }
        }

        private void VerifyInvoicingItems()
        {
            if (OperatingCenter.HasWorkOrderInvoicing && DateCompleted.HasValue && WorkOrdersScheduleOfValues.Count == 0 && Phase == WorkOrderPhase.Finalization)
                throw new DomainLogicException("You must include schedule of values with the work order.");
        }

        private void VerifyPMATOverride()
        {
            if (PlantMaintenanceActivityTypeOverride != null && PlantMaintenanceActivityTypeOverride.Id != PlantMaintenanceActivityType.Indices.PBC && String.IsNullOrEmpty(AccountCharged))
                throw new DomainLogicException("WorkOrder must include a WBS Number if PMAT is being overridden.");
        }

        private void VerifyCoordinateInformation()
        {
            if (Latitude == null || Longitude == null)
            {
                throw new DomainLogicException(
                    "Cannot save a WorkOrder record without Coordinate information.");
            }
        }

        private void VerifyMarkoutNecessity()
        {
            if (MarkoutTypeNeededID == null && MarkoutToBeCalled == null)
                return;
            if (MarkoutTypeNeededID == null || MarkoutToBeCalled == null)
                throw new DomainLogicException(
                    "Work Orders with Markout call-in requirements must have both a required Markout Type and date to be called.");
            if (MarkoutTypeNeeded.Description == "NOT LISTED" && String.IsNullOrEmpty(RequiredMarkoutNote))
                throw new DomainLogicException(
                    "Work Orders with a required markout type that's not listed need a note.");
        }

        #endregion

        #region Data Mapping

        private Asset GetAsset()
        {
            if (Valve != null || Hydrant != null || SewerOpening != null || StormCatch != null || Equipment != null || MainCrossing != null)
                return new Asset(AssetType, Valve, Hydrant, SewerOpening, StormCatch, Equipment, MainCrossing);
            if (ValveID != null || HydrantID != null || SewerOpeningID != null || StormCatchID != null || EquipmentID != null || MainCrossingID != null)
                return new Asset(AssetType, ValveID, HydrantID, SewerOpeningID, StormCatchID, EquipmentID, MainCrossingID);
            return null;
        }

#pragma warning disable 168
        private void OnCreatedOnChanging(DateTime value)
        {
            if (CreatedOn != DateTime.MinValue)
                throw new DomainLogicException("Cannot change the CreatedOn date once it has been set.");
        }

        private void OnCreatedByChanging(Employee value)
        {
            if (_createdBy.HasLoadedOrAssignedValue && value != null)
                throw new DomainLogicException("Cannot change the creator of a work order once it has been set.");
        }
#pragma warning restore 168

        /* LOST WATER */
        private void attach_LostWaters(LostWater entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_LostWaters(LostWater entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* MAIN BREAKS */
        private void attach_MainBreaks(MainBreak entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_MainBreaks(MainBreak entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* MATERIALS USED */
        private void attach_MaterialsUseds(MaterialsUsed entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_MaterialsUseds(MaterialsUsed entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* WorkOrder ScheduleOfValues */
        private void attach_WorkOrderScheduleOfValue(WorkOrderScheduleOfValue entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_WorkOrderScheduleOfValue(WorkOrderScheduleOfValue entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* SAFETY MARKERS */
        private void attach_SafetyMarkers(SafetyMarker entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_SafetyMarkers(SafetyMarker entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* DETECTED LEAKS */
        private void attach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* EMPLOYEE WORK ORDERS */
        private void attach_EmployeeWorkOrders(EmployeeWorkOrder entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_EmployeeWorkOrders(EmployeeWorkOrder entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* MARKOUTS */
        private void attach_Markouts(Markout entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_Markouts(Markout entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* REQUISITIONS */
        private void attach_Requisitions(Requisition entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }

        private void detach_Requisitions(Requisition entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* SOP */
        private void attach_StreetOpeningPermits(StreetOpeningPermit entity)
        {
            var current = StreetOpeningPermits.GetCurrent();
            if (current != null && !current.IsExpired)
                throw new DomainLogicException("Cannot add a new StreetOpeningPermit when the current StreetOpeningPermit has not yet exired.");
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_StreetOpeningPermits(StreetOpeningPermit entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* WORK ORDER DESCRIPTION CHANGES*/
        private void attach_WorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_WorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* CREW ASSIGNMENTS */
        private void attach_CrewAssignments(CrewAssignment entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_CrewAssignments(CrewAssignment entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* RESTORATIONS */
        private void attach_Restorations(Restoration entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_Restorations(Restoration entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* SPOILS (booty.  burrrried treasure, arrrrrgh.) */
        private void attach_Spoils(Spoil entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_Spoils(Spoil entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }


        /* ORCOMORDERCOMPLETIONS */
        private void attach_OrcomOrderCompletions(OrcomOrderCompletion entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_OrcomOrderCompletions(OrcomOrderCompletion entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* DOCUMENTS */
        private void attach_DocumentsWorkOrders(DocumentWorkOrder entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = this;
        }
        private void detach_DocumentsWorkOrders(DocumentWorkOrder entity)
        {
            SendPropertyChanging();
            entity.WorkOrder = null;
        }

        /* CHILD ORDERS */
        private void attach_ChildOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.OriginalOrder = this;
        }
        private void detach_ChildOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.OriginalOrder = null;
        }
        
        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetCoordinateValuesFromAsset(IAsset asset)
        {
            Latitude = (asset != null) ? asset.Latitude : null;
            Longitude = (asset != null) ? asset.Longitude : null;
        }

        #endregion

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public virtual void SetCurrentMarkoutExpiration()
        {
            // We don't want to extend if Markouts are Editable
            if (MarkoutRequired && CurrentMarkout != null && !CurrentMarkout.WorkOrder.OperatingCenter.MarkoutsEditable)
                CurrentMarkout.ExpirationDate =
                    WorkOrdersWorkDayEngine.GetExpirationDate(
                        CurrentMarkout.DateOfRequest, MarkoutRequirement.RequirementEnum,
                        WorkStarted);
        }
        
        #endregion
    }

    /// <summary>
    /// Enumeration value indicating the current phase the user is working
    /// with.
    /// </summary>
    public enum WorkOrderPhase
    {
        /// <summary>
        /// Denotes that a WorkOrder is in the Input Phase.  Only orders
        /// that have not yet been saved can be in the Input Phase, because
        /// the requirements for initially saving an order record are the
        /// same as for moving it into the Planning Phase.
        /// </summary>
        Input,
        /// <summary>
        /// Denotes that a WorkOrder is in the Planning Phase.  This is an extra,
        /// optional step where supervisors can assign orders to office workers
        /// so they can be planned (have markouts and SOPs called in, etc.)
        /// </summary>
        PrePlanning,
        /// <summary>
        /// Denotes that a WorkOrder is in the Planning Phase.  As soon
        /// as an order is initially saved to the database, it moves into
        /// the Planning phase.  If the order requires a markout, then a
        /// markout record must be entered at this phase, in order to
        /// progress into the Scheduling phase.
        /// </summary>
        Planning,
        /// <summary>
        /// Denotes that a WorkOrder is in the Scheduling Phase.  Once
        /// an order has a CrewAssignment attached, and that CrewAssignment's
        /// date comes to pass, the order can progress into the Finalization
        /// Phase.
        /// </summary>
        Scheduling,
        /// <summary>
        /// Denotes that a WorkOrder is in the Finalization Phase.  When
        /// a Crew works a CrewAssignment for a given order, they are linked
        /// to the Finaliation page and provided the ability to "complete"
        /// an order from there.  If not, the order can be bounced back into
        /// Scheduling so that further work can be done.
        /// </summary>
        Finalization,
        /// <summary>
        /// Denotes that a WorkOrder is in the Approval Phase.  When an order
        /// is given a completion date in the Finalization step, it becomes
        /// ready for general supervisor approval.  After that approval, any
        /// materials for the order can be approved by the stock clerk.  Note
        /// that orders can now be rejected from the Supervisor Approval process,
        /// so a StockApproval phase needed to be created.
        /// </summary>
        Approval,
        /// <summary>
        /// Denotes that a WorkOrder is in the StockApproval phase.  When an
        /// order gets approved by a supervisor, the work is verified as
        /// complete and satisfactorily so, and thus any stock that was used to
        /// get the job done can now be approved and charged as necessary.
        /// </summary>
        StockApproval,
        /// <summary>
        /// Denotes that a WorkOrder is in the OrcomOrderApproval phase. 
        /// These are orders that are created with the purpose of customer 
        /// service and they have orcom service # that goes in the work order 
        /// on the input screen.  When these orders are completed in the field 
        /// they get flagged and goes on the Orcom completed list so we can 
        /// update Orcom which is our customer account database system
        /// </summary>
        OrcomOrderApproval,
        /// <summary>
        /// Does not refer to a WorkOrder in any particular Phase (though
        /// orders in the Input Phase will not be available from General
        /// pages, as they will not have been persisted yet).  Merely
        /// denotes that a given View provides general view and edit
        /// functionality at any phase.
        /// </summary>
        General
    }
}

