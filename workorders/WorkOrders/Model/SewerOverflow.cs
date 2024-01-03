using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SewerOverflows")]
    public class SewerOverflow : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _sewerOverflowId;

        private DateTime _createdOn;

        private DateTime? _incidentDate,
            _callReceived,
            _crewArrivedOnSite,
            _sewageContained,
            _stoppageCleared,
            _workCompleted;

        private string _talkedTo,
            _streetNumber,
            _enforcingAgencyCaseNumber,
            _locationOfStoppage,
            _truckNumber;

        private int? _gallonsOverflowedEstimated,
            _gallonsInContainedDrains,
            _gallonsFlowedIntoBodyOfWater,
            _operatingCenterId,
            _sewerClearingMethodId,
            _areaCleanedUpToId,
            _zoneTypeId,
            _townId,
            _streetId,
            _crossStreetId,
            _coordinateId,
            _createdById,
            _bodyOfWaterId,
            _workOrderId;

        private bool? _overflowCustomers;

        private EntityRef<OperatingCenter> _operatingCenter;

        private EntityRef<SewerClearingMethod> _sewerClearingMethod;

        private EntityRef<SewerOverflowArea> _areaCleanedUpTo;

        private EntityRef<ZoneType> _zoneType;

        private EntityRef<Town> _town;

        private EntityRef<Street> _street;

        private EntityRef<Street> _crossStreet;

        private EntityRef<Coordinate> _coordinate;

        private EntityRef<Employee> _createdBy;

        private EntityRef<BodyOfWater> _bodyOfWater;

        #endregion

        #region Properties

        [Column(Storage = "_sewerOverflowId", AutoSync = AutoSync.OnInsert, DbType = "int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int SewerOverflowId
        {
            get => _sewerOverflowId;
            set
            {
                if (_sewerOverflowId != value)
                {
                    SendPropertyChanging();
                    _sewerOverflowId = value;
                    SendPropertyChanged("SewerOverflowId");
                }
            }
        }

        [Column(Storage = "_incidentDate", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? IncidentDate
        {
            get => _incidentDate;
            set
            {
                if (_incidentDate != value)
                {
                    SendPropertyChanging();
                    _incidentDate = value;
                    SendPropertyChanged("IncidentDate");
                }
            }
        }

        [Column(Storage = "_callReceived", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? CallReceived
        {
            get => _callReceived;
            set
            {
                if (_callReceived != value)
                {
                    SendPropertyChanging();
                    _callReceived = value;
                    SendPropertyChanged("CallReceived");
                }
            }
        }

        [Column(Storage = "_crewArrivedOnSite", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? CrewArrivedOnSite
        {
            get => _crewArrivedOnSite;
            set
            {
                if (_crewArrivedOnSite != value)
                {
                    SendPropertyChanging();
                    _crewArrivedOnSite = value;
                    SendPropertyChanged("CrewArrivedOnSite");
                }
            }
        }

        [Column(Storage = "_sewageContained", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? SewageContained
        {
            get => _sewageContained;
            set
            {
                if (_sewageContained != value)
                {
                    SendPropertyChanging();
                    _sewageContained = value;
                    SendPropertyChanged("SewageContained");
                }
            }
        }

        [Column(Storage = "_stoppageCleared", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? StoppageCleared
        {
            get => _stoppageCleared;
            set
            {
                if (_stoppageCleared != value)
                {
                    SendPropertyChanging();
                    _stoppageCleared = value;
                    SendPropertyChanged("StoppageCleared");
                }
            }
        }

        [Column(Storage = "_workCompleted", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? WorkCompleted
        {
            get => _workCompleted;
            set
            {
                if (_workCompleted != value)
                {
                    SendPropertyChanging();
                    _workCompleted = value;
                    SendPropertyChanged("WorkCompleted");
                }
            }
        }

        [Column(Name = "CreatedOn", Storage = "_createdOn", DbType = "DateTime NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedOn
        {
            get => _createdOn;
            set
            {
                if (_createdOn != value)
                {
                    SendPropertyChanging();
                    _createdOn = value;
                    SendPropertyChanged("CreatedOn");
                }
            }
        }

        [Column(Storage = "_talkedTo", DbType = "varchar(50)", UpdateCheck = UpdateCheck.Never)]
        public string TalkedTo
        {
            get => _talkedTo;
            set
            {
                if (_talkedTo != value)
                {
                    SendPropertyChanging();
                    _talkedTo = value;
                    SendPropertyChanged("TalkedTo");
                }
            }
        }

        [Column(Storage = "_streetNumber", DbType = "varchar(20)", UpdateCheck = UpdateCheck.Never)]
        public string StreetNumber
        {
            get => _streetNumber;
            set
            {
                if (_streetNumber != value)
                {
                    SendPropertyChanging();
                    _streetNumber = value;
                    SendPropertyChanged("StreetNumber");
                }
            }
        }

        [Column(Storage = "_enforcingAgencyCaseNumber", DbType = "varchar(50)", UpdateCheck = UpdateCheck.Never)]
        public string EnforcingAgencyCaseNumber
        {
            get => _enforcingAgencyCaseNumber;
            set
            {
                if (_enforcingAgencyCaseNumber != value)
                {
                    SendPropertyChanging();
                    _enforcingAgencyCaseNumber = value;
                    SendPropertyChanged("EnforcingAgencyCaseNumber");
                }
            }
        }

        [Column(Storage = "_locationOfStoppage", DbType = "varchar(255)", UpdateCheck = UpdateCheck.Never)]
        public string LocationOfStoppage
        {
            get => _locationOfStoppage;
            set
            {
                if (_locationOfStoppage != value)
                {
                    SendPropertyChanging();
                    _locationOfStoppage = value;
                    SendPropertyChanged("LocationOfStoppage");
                }
            }
        }

        [Column(Storage = "_truckNumber", DbType = "varchar(20)", UpdateCheck = UpdateCheck.Never)]
        public string TruckNumber
        {
            get => _truckNumber;
            set
            {
                if (_truckNumber != value)
                {
                    SendPropertyChanging();
                    _truckNumber = value;
                    SendPropertyChanged("TruckNumber");
                }
            }
        }

        [Column(Storage = "_gallonsOverflowedEstimated", DbType = "int", UpdateCheck = UpdateCheck.Never)]
        public int? GallonsOverflowedEstimated
        {
            get => _gallonsOverflowedEstimated;
            set
            {
                if (_gallonsOverflowedEstimated != value)
                {
                    SendPropertyChanging();
                    _gallonsOverflowedEstimated = value;
                    SendPropertyChanged("GallonsOverflowedEstimated");
                }
            }
        }

        [Column(Storage = "_gallonsInContainedDrains", DbType = "int", UpdateCheck = UpdateCheck.Never)]
        public int? GallonsInContainedDrains
        {
            get => _gallonsInContainedDrains;
            set
            {
                if (_gallonsInContainedDrains != value)
                {
                    SendPropertyChanging();
                    _gallonsInContainedDrains = value;
                    SendPropertyChanged("GallonsInContainedDrains");
                }
            }
        }

        [Column(Storage = "_gallonsFlowedIntoBodyOfWater", DbType = "int", UpdateCheck = UpdateCheck.Never)]
        public int? GallonsFlowedIntoBodyOfWater
        {
            get => _gallonsFlowedIntoBodyOfWater;
            set
            {
                if (_gallonsFlowedIntoBodyOfWater != value)
                {
                    SendPropertyChanging();
                    _gallonsFlowedIntoBodyOfWater = value;
                    SendPropertyChanged("GallonsFlowedIntoBodyOfWater");
                }
            }
        }

        [Column(Storage = "_overflowCustomers", DbType = "bit", UpdateCheck = UpdateCheck.Never)]
        public bool? OverflowCustomers
        {
            get => _overflowCustomers;
            set
            {
                if (_overflowCustomers != value)
                {
                    SendPropertyChanging();
                    _overflowCustomers = value;
                    SendPropertyChanged("OverflowCustomers");
                }
            }
        }

        [Column(Storage = "_operatingCenterId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OperatingCenterId
        {
            get => _operatingCenterId;
            set
            {
                if (_operatingCenterId != value)
                    if (_operatingCenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _operatingCenterId = value;
                SendPropertyChanged("OperatingCenterId");
            }
        }

        [Association(Name = "OperatingCenter_SewerOverflow", Storage = "_operatingCenter", ThisKey = "OperatingCenterId", IsForeignKey = true)]
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
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _operatingCenterId = value.OperatingCenterID;
                    }
                    else
                    {
                        _operatingCenterId = default(int);
                    }

                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Column(Storage = "_sewerClearingMethodId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? SewerClearingMethodId
        {
            get => _sewerClearingMethodId;
            set
            {
                if (_sewerClearingMethodId != value)
                    if (_sewerClearingMethod.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _sewerClearingMethodId = value;
                SendPropertyChanged("SewerClearingMethodId");
            }
        }

        [Association(Name = "SewerClearingMethod_SewerOverflow", Storage = "_sewerClearingMethod", ThisKey = "SewerClearingMethodId",
            IsForeignKey = true)]
        public SewerClearingMethod SewerClearingMethod
        {
            get => _sewerClearingMethod.Entity;
            set
            {
                var previousValue = _sewerClearingMethod.Entity;
                if ((previousValue != value)
                    || (_sewerClearingMethod.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _sewerClearingMethod.Entity = null;
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _sewerClearingMethod.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _sewerClearingMethodId = value.SewerClearingMethodId;
                    }
                    else
                    {
                        _sewerClearingMethodId = default(int);
                    }

                    SendPropertyChanged("SewerClearingMethod");
                }
            }
        }

        [Column(Storage = "_areaCleanedUpToId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? AreaCleanedUpToId
        {
            get => _areaCleanedUpToId;
            set
            {
                if (_areaCleanedUpToId != value)
                    if (_areaCleanedUpTo.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _areaCleanedUpToId = value;
                SendPropertyChanged("AreaCleanedUpToId");
            }
        }

        [Association(Name = "SewerOverflowArea_SewerOverflow", Storage = "_areaCleanedUpTo", ThisKey = "AreaCleanedUpToId",
            IsForeignKey = true)]
        public SewerOverflowArea AreaCleanedUpTo
        {
            get => _areaCleanedUpTo.Entity;
            set
            {
                var previousValue = _areaCleanedUpTo.Entity;
                if ((previousValue != value)
                    || (_areaCleanedUpTo.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _areaCleanedUpTo.Entity = null;
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _areaCleanedUpTo.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _areaCleanedUpToId = value.AreaCleanedUpToId;
                    }
                    else
                    {
                        _areaCleanedUpToId = default(int);
                    }

                    SendPropertyChanged("AreaCleanedUpTo");
                }
            }
        }

        [Column(Storage = "_zoneTypeId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? ZoneTypeId
        {
            get => _zoneTypeId;
            set
            {
                if (_zoneTypeId != value)
                    if (_zoneType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _zoneTypeId = value;
                SendPropertyChanged("ZoneTypeId");
            }
        }

        [Association(Name = "ZoneType_SewerOverflow", Storage = "_zoneType", ThisKey = "ZoneTypeId",
            IsForeignKey = true)]
        public ZoneType ZoneType
        {
            get => _zoneType.Entity;
            set
            {
                var previousValue = _zoneType.Entity;
                if ((previousValue != value)
                    || (_zoneType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _zoneType.Entity = null;
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _zoneType.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _zoneTypeId = value.ZoneTypeId;
                    }
                    else
                    {
                        _zoneTypeId = default(int);
                    }

                    SendPropertyChanged("ZoneType");
                }
            }
        }

        [Column(Storage = "_townId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? TownId
        {
            get => _townId;
            set
            {
                if (_townId != value)
                    if (_town.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _townId = value;
                SendPropertyChanged("TownId");
            }
        }

        [Association(Name = "Town_SewerOverflow", Storage = "_town", ThisKey = "TownId",
            IsForeignKey = true)]
        public Town Town
        {
            get => _town.Entity;
            set
            {
                var previousValue = _town.Entity;
                if ((previousValue != value)
                    || (_town.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _town.Entity = null;
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _town.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _townId = value.TownID;
                    }
                    else
                    {
                        _townId = default(int);
                    }

                    SendPropertyChanged("Town");
                }
            }
        }

        [Column(Storage = "_streetId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? StreetId
        {
            get => _streetId;
            set
            {
                if (_streetId != value)
                    if (_street.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _streetId = value;
                SendPropertyChanged("StreetId");
            }
        }

        [Association(Name = "Street_SewerOverflow", Storage = "_street", ThisKey = "StreetId",
            IsForeignKey = true)]
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
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _street.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _streetId = value.StreetID;
                    }
                    else
                    {
                        _streetId = default(int);
                    }

                    SendPropertyChanged("Street");
                }
            }
        }

        [Column(Storage = "_crossStreetId", Name = "CrossStreet", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? CrossStreetId
        {
            get => _crossStreetId;
            set
            {
                if (_crossStreetId != value)
                    if (_crossStreet.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _crossStreetId = value;
                SendPropertyChanged("CrossStreetId");
            }
        }

        [Association(Name = "Street_SewerOverflow", Storage = "_crossStreet", ThisKey = "CrossStreetId",
            IsForeignKey = true)]
        public Street CrossStreet
        {
            get => _crossStreet.Entity;
            set
            {
                var previousValue = _crossStreet.Entity;
                if ((previousValue != value)
                    || (_crossStreet.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _crossStreet.Entity = null;
                        previousValue.SewerOverflows.Remove(this);
                    }

                    _crossStreet.Entity = value;
                    if (value != null)
                    {
                        value.SewerOverflows.Add(this);
                        _crossStreetId = value.StreetID;
                    }
                    else
                    {
                        _crossStreetId = default(int);
                    }

                    SendPropertyChanged("CrossStreet");
                }
            }
        }

        [Column(Storage = "_coordinateId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? CoordinateId
        {
            get => _coordinateId;
            set
            {
                if (_coordinateId != value)
                    if (_coordinate.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _coordinateId = value;
                SendPropertyChanged("CoordinateId");
            }
        }

        [Association(Name = "Coordinate_SewerOverflow", Storage = "_coordinate", ThisKey = "CoordinateId",
            IsForeignKey = true)]
        public Coordinate Coordinate
        {
            get => _coordinate.Entity;
            set
            {
                var previousValue = _coordinate.Entity;
                if ((previousValue != value)
                    || (_coordinate.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _coordinate.Entity = null;
                    }

                    _coordinate.Entity = value;
                    if (value != null)
                    {
                        _coordinateId = value.CoordinateID;
                    }
                    else
                    {
                        _coordinateId = default(int);
                    }

                    SendPropertyChanged("Coordinate");
                }
            }
        }

        [Column(Storage = "_createdById", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? CreatedById
        {
            get => _createdById;
            set
            {
                if (_createdById != value)
                    if (_createdBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _createdById = value;
                SendPropertyChanged("CreatedById");
            }
        }

        [Association(Name = "Employee_SewerOverflow", Storage = "_createdBy", ThisKey = "CreatedById",
            IsForeignKey = true)]
        public Employee CreatedBy
        {
            get => _createdBy.Entity;
            set
            {
                var previousValue = _createdBy.Entity;
                if ((previousValue != value)
                    || (_createdBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _createdBy.Entity = null;
                    }

                    _createdBy.Entity = value;
                    if (value != null)
                    {
                        _createdById = value.EmployeeID;
                    }
                    else
                    {
                        _createdById = default(int);
                    }

                    SendPropertyChanged("CreatedBy");
                }
            }
        }

        [Column(Storage = "_bodyOfWaterId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? BodyOfWaterId
        {
            get => _bodyOfWaterId;
            set
            {
                if (_bodyOfWaterId != value)
                    if (_bodyOfWater.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _bodyOfWaterId = value;
                SendPropertyChanged("BodyOfWaterId");
            }
        }

        [Association(Name = "BodyOfWater_SewerOverflow", Storage = "_bodyOfWater", ThisKey = "BodyOfWaterId",
            IsForeignKey = true)]
        public BodyOfWater BodyOfWater
        {
            get => _bodyOfWater.Entity;
            set
            {
                var previousValue = _bodyOfWater.Entity;
                if ((previousValue != value)
                    || (_bodyOfWater.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _bodyOfWater.Entity = null;
                    }

                    _bodyOfWater.Entity = value;
                    if (value != null)
                    {
                        _bodyOfWaterId = value.BodyOfWaterID;
                    }
                    else
                    {
                        _bodyOfWaterId = default(int);
                    }

                    SendPropertyChanged("BodyOfWater");
                }
            }
        }

        private EntityRef<WorkOrder> _workOrder;

        [Column(Storage = "_workOrderId", DbType = "Int",
            UpdateCheck = UpdateCheck.Never)]
        public int? WorkOrderId
        {
            get => _workOrderId;
            set
            {
                if (_workOrderId != value)
                    if (_workOrder.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _workOrderId = value;
                SendPropertyChanged("WorkOrderId");
            }
        }

        [Association(Name = "WorkOrder_SewerOverflow", Storage = "_workOrder", ThisKey = "WorkOrderId",
            IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get => _workOrder.Entity;
            set
            {
                var previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                    }

                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        _workOrderId = value.WorkOrderID;
                    }
                    else
                    {
                        _workOrderId = default(int);
                    }

                    SendPropertyChanged("WorkOrder");
                }
            }
        }


        #endregion

        #region Constructors

        public SewerOverflow() { }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            PropertyChanging?.Invoke(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events/Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion
    }
}
