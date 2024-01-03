using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text.RegularExpressions;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Valves")]
    public class Valve : INotifyPropertyChanging, INotifyPropertyChanged, IAsset
    {
        #region Constants

        private const short MAX_NEAREST_CROSS_STREET_NAME_LENGTH = 30;
        private const short MAX_OPCODE_LENGTH = 4;
        private const short MAX_TOWN_SECTION_NAME_LENGTH = 30;
        private const short MAX_VALVE_NUMBER_LENGTH = 15;
        private const short MAX_STATUS_LENGTH = 10;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private static readonly Regex rgxValveSuffix = new Regex(@"^(.+)+-(.+)$", RegexOptions.Compiled);

        #endregion

        #region Private Members

        private string _nearestCrossStreetName,
            _townSectionName,
            _valveNumber,
            _fullValveSuffix,
            _status,
            _criticalNotes;

        private double? _latitude,
                        _longitude, 
                        _turns;
        
        private int? _coordinateId,
                     _streetID,
                     _townID,
                     _sapEquipmentID,
                     _crossStreetId,
                     _assetStatusId,
                     _townSectionId,
                     _valveSuffix,
                     _route, 
                     _valveControlsId,
                     _valveSizeId,
                     _valveMakeId,
                     _normalPositionId,
                     _opensId,
                     _valveTypeId,
                     _lastUpdatedById;

        private decimal? _stop;

        private int _valveID,
                    _operatingCenterId;

        private DateTime? _dateInstalled, _lastUpdated;

        private EntityRef<Street> _street;
        private EntityRef<Town> _town;
        private EntityRef<TownSection> _townSection;
        private EntityRef<Coordinate> _coordinate;
        private EntityRef<Street> _crossStreet;
        private EntityRef<AssetStatus> _assetStatus;
        private EntityRef<OperatingCenter> _operatingCenter;

        private EntityRef<ValveControl> _valveControl;
        private EntityRef<ValveSize> _valveSize;
        private EntityRef<ValveManufacturer> _valveManufacturer;
        private EntityRef<ValveNormalPosition> _valveNormalPosition;
        private EntityRef<ValveOpenDirection> _valveOpenDirection;
        private EntityRef<ValveType> _valveType;
        private EntityRef<Employee> _lastUpdatedBy;

        private readonly EntitySet<MainBreakValveOperation> _mainBreakValveOperations;

        private readonly EntitySet<DetectedLeak> _detectedLeaks;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        public object AssetKey
        {
            get { return ValveID; }
        }

        public string AssetID
        {
            get { return FullValveSuffix; }
        }

        [Column(Name = "CriticalNotes", Storage = "_criticalNotes", DbType = "VarChar(150)")]
        public string CriticalNotes
        {
            get { return _criticalNotes; }
            set
            {
                if (value != null && value.Length > 150)
                    throw new StringTooLongException("CriticalNotes", 150);
                if (_criticalNotes != value)
                {
                    SendPropertyChanging();
                    _criticalNotes = value;
                    SendPropertyChanged("CriticalNotes");
                }
            }
        }

        [Column(Name = "DateInstalled", Storage = "_dateInstalled", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateInstalled
        {
            get { return _dateInstalled; }
            set { _dateInstalled = value; }
        }

        [Column(Name = "Route", Storage = "_route", DbType = "Int",  UpdateCheck = UpdateCheck.Never)]
        public int? Route
        {
            get { return _route; }
            set { _route = value; }
        }

        [Column(Name = "Stop", Storage = "_stop", DbType = "decimal(19,5)", UpdateCheck = UpdateCheck.Never)]
        public decimal? Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }

        [Column(Name = "Turns", Storage = "_turns", DbType = "Float",  UpdateCheck = UpdateCheck.Never)]
        public double? Turns
        {
            get { return _turns; }
            set { _turns = value; }
        }

        [Column(Name = "ValveControlsId", Storage = "_valveControlsId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ValveControlsId
        {
            get { return _valveControlsId; }
            set { _valveControlsId = value; }
        }

        [Association(Name = "ValveControl_Valve", Storage = "_valveControl", ThisKey = "ValveControlsId", IsForeignKey = true)]
        public ValveControl ValveControls => _valveControl.Entity;

        [Column(Name = "ValveSizeId", Storage = "_valveSizeId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ValveSizeId
        {
            get { return _valveSizeId; }
            set { _valveSizeId = value; }
        }

        [Association(Name = "ValveSize_Valve", Storage = "_valveSize", ThisKey = "ValveSizeId", IsForeignKey = true)]
        public ValveSize ValveSize => _valveSize.Entity;

        [Column(Name = "ValveMakeId", Storage = "_valveMakeId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ValveMakeId
        {
            get { return _valveMakeId; }
            set { _valveMakeId = value; }
        }

        [Association(Name = "ValveManufacturer_Valve", Storage = "_valveManufacturer", ThisKey = "ValveMakeId", IsForeignKey = true)]
        public ValveManufacturer ValveMake => _valveManufacturer.Entity;

        [Column(Name = "NormalPositionId", Storage = "_normalPositionId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? NormalPositionId
        {
            get { return _normalPositionId; }
            set { _normalPositionId = value; }
        }

        [Association(Name = "ValveNormalPosition_Valve", Storage = "_valveNormalPosition", ThisKey = "NormalPositionId", IsForeignKey = true)]
        public ValveNormalPosition NormalPosition => _valveNormalPosition.Entity;

        [Column(Name = "OpensId", Storage = "_opensId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OpensId
        {
            get { return _opensId; }
            set { _opensId = value; }
        }

        [Association(Name = "ValveOpenDirection_Valve", Storage = "_valveOpenDirection", ThisKey = "OpensId", IsForeignKey = true)]
        public ValveOpenDirection OpenDirection => _valveOpenDirection.Entity;

        [Column(Name = "ValveTypeId", Storage = "_valveTypeId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ValveTypeId
        {
            get { return _valveTypeId; }
            set { _valveTypeId = value; }
        }

        [Association(Name = "ValveType_Valve", Storage = "_valveType", ThisKey = "ValveTypeId", IsForeignKey = true)]
        public ValveType ValveType => _valveType.Entity;
        
        public double? Latitude
        {
            get
            {
                if (Coordinate != null) return Coordinate.Latitude;
                return null;
            }
            set { Coordinate.Latitude = value.Value; }
        }

        public double? Longitude
        {
            get
            {
                if (Coordinate != null) return Coordinate.Longitude;
                return null;
            }
            set { Coordinate.Longitude = value.Value; }
        }

        [Column(Name = "UpdatedAt", Storage = "_lastUpdated", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            set { _lastUpdated = value; }
        }

        public DateTime? UpdatedAt => LastUpdated;

        public string FullValveSuffix
        {
            get
            {
                if (_fullValveSuffix == null)
                    _fullValveSuffix =
                        rgxValveSuffix.Match(ValveNumber).Groups[2].Value;
                return _fullValveSuffix;
            }
        }

        [Association(Name = "CrossStreet_Valve", Storage = "_crossStreet", ThisKey = "CrossStreetID", IsForeignKey = true)]
        public Street NearestCrossStreet
        {
            get { return _crossStreet.Entity; }
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
                        previousValue.Valves.Remove(this);
                    }
                    _crossStreet.Entity = value;
                    if (value != null)
                    {
                        value.Valves.Add(this);
                        _crossStreetId = value.StreetID;
                    }
                    else
                        _crossStreetId = default(int);
                    SendPropertyChanged("CrossStreet");
                }
            }
        }

        /// <summary>
        /// Foreign key field, used to link a valve to the street on which it resides.
        /// </summary>
        [Column(Name = "CrossStreetId", Storage = "_crossStreetId", DbType = "Int")]
        public int? CrossStreetID
        {
            get { return _crossStreetId; }
            set
            {
                if (_crossStreetId != value)
                {
                    SendPropertyChanging();
                    _crossStreetId = value;
                    SendPropertyChanged("CrossStreetID");
                }
            }
        }

        [Column(Name = "SAPEquipmentID", Storage = "_sapEquipmentID", DbType = "Int")]
        public int? SAPEquipmentID
        {
            get { return _sapEquipmentID; }
            set
            {
                if (_sapEquipmentID != value)
                {
                    SendPropertyChanging();
                    _sapEquipmentID = value;
                    SendPropertyChanged("SAPEquipmentID");
                }
            }
        }

        [Column(Storage = "_operatingCenterId", DbType = "Int NOT NULL")]
        public int OperatingCenterID
        {
            get
            {
                return _operatingCenterId;
            }
            set
            {
                if ((_operatingCenterId != value))
                {
                    SendPropertyChanging();
                    _operatingCenterId = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Association(Name = "OperatingCenter_Valve", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingCenter.Entity; }
            set
            {
                OperatingCenter previousValue = _operatingCenter.Entity;
                if ((previousValue != value)
                    || (_operatingCenter.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _operatingCenter.Entity = null;
                        previousValue.Valves.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.Valves.Add(this);
                        _operatingCenterId = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterId = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Column(Name = "AssetStatusId", Storage = "_assetStatusId", DbType = "Int")]
        public int? AssetStatusID
        {
            get { return _assetStatusId; }
            set
            {
                if (_assetStatusId != value)
                {
                    SendPropertyChanging();
                    _assetStatusId = value;
                    SendPropertyChanged("AssetStatusID");
                }
            }
        }

        [Association(Name = "AssetStatus_Valve", Storage = "_assetStatus", ThisKey = "AssetStatusID", IsForeignKey = true)]
        public AssetStatus AssetStatus
        {
            get { return _assetStatus.Entity; }
            set
            {
                var previousValue = _assetStatus.Entity;
                if ((previousValue != value) ||
                    (_assetStatus.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _assetStatus.Entity = value;
                    }
                    _assetStatus.Entity = value;
                    if (value != null)
                    {
                        _assetStatusId = value.AssetStatusID;
                    }
                    else
                        _assetStatusId = default(int);
                    SendPropertyChanged("AssetStatus");
                }
            }

        }

        [Association(Name = "Street_Valve", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
        public Street Street
        {
            get { return _street.Entity; }
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
                        previousValue.Valves.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.Valves.Add(this);
                        _streetID = value.StreetID;
                    }
                    else
                        _streetID = default(int);
                    SendPropertyChanged("Street");
                }
            }
        }

        [Column(Name = "CoordinateId", Storage = "_coordinateId", DbType = "Int")]
        public int? CoordinateID
        {
            get { return _coordinateId; }
            set
            {
                if (_coordinateId != value)
                {
                    SendPropertyChanging();
                    _coordinateId = value;
                    SendPropertyChanged("CoordinateID");
                }
            }
        }

        [Association(Name = "Coordinate_Valve", Storage = "_coordinate", ThisKey = "CoordinateID", OtherKey = "CoordinateID", IsForeignKey = true)]
        public Coordinate Coordinate
        {
            get { return _coordinate.Entity; }
            set
            {
                var previousValve = _coordinate.Entity;
                if ((previousValve != value) || (_coordinate.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValve != null)
                    {
                        _coordinate.Entity = null;
                        previousValve.Valves.Remove(this);
                    }
                    _coordinate.Entity = value;
                    if (value != null)
                    {
                        value.Valves.Add(this);
                        _coordinateId = value.CoordinateID;
                    }
                    else
                        _coordinateId = default(int);
                    SendPropertyChanged("Coordinate");
                }
            }
        }

        [Association(Name = "Town_Valve", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
        public Town Town
        {
            get { return _town.Entity; }
            set
            {
                Town previousValue = _town.Entity;
                if ((previousValue != value)
                    || (_town.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _town.Entity = null;
                        previousValue.Valves.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.Valves.Add(this);
                        _townID = value.TownID;
                    }
                    else
                        _townID = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }

        /// <summary>
        /// Foreign key field, used to link a valve to the street on which it resides.
        /// </summary>
        [Column(Name = "StreetId", Storage = "_streetID", DbType = "Int")]
        public int? StreetID
        {
            get { return _streetID; }
            set
            {
                if (_streetID != value)
                {
                    SendPropertyChanging();
                    _streetID = value;
                    SendPropertyChanged("StreetID");
                }
            }
        }

        /// <summary>
        /// Foreign key field, used to link a valve to the town in which it resides.
        /// </summary>
        [Column(Name = "Town", Storage = "_townID", DbType = "Int")]
        public int? TownID
        {
            get { return _townID; }
            set
            {
                if (_townID != value)
                {
                    SendPropertyChanging();
                    _townID = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        [Column(Name = "TownSectionId", Storage = "_townSectionId", DbType = "Int")]
        public int? TownSectionID
        {
            get { return _townSectionId; }
            set
            {
                if (_townSectionId != value)
                {
                    SendPropertyChanging();
                    _townSectionId = value;
                    SendPropertyChanged("TownSectionID");
                }
            }
        }

        [Association(Name = "TownSection_Valve", Storage = "_townSection", ThisKey = "TownSectionID", IsForeignKey = true)]
        public TownSection TownSection
        {
            get { return _townSection.Entity; }
            set
            {
                TownSection previousValue = _townSection.Entity;
                if ((previousValue != value)
                    || (_townSection.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _townSection.Entity = null;
                        previousValue.Valves.Remove(this);
                    }
                    _townSection.Entity = value;
                    if (value != null)
                    {
                        value.Valves.Add(this);
                        _townSectionId = value.TownSectionID;
                    }
                    else
                        _townSectionId = default(int);
                    SendPropertyChanged("TownSection");
                }
            }
        }
        
        /// <summary>
        /// Primary key from the valves table.
        /// </summary>
        [Column(Name = "Id", Storage = "_valveID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ValveID
        {
            get { return _valveID; }
            set
            {
                if (_valveID != value)
                {
                    SendPropertyChanging();
                    _valveID = value;
                    SendPropertyChanged("ValveID");
                }
            }
        }

        /// <summary>
        /// The entire AWW valve number used to designate a valve in the system, including
        /// a letter code prefix to designate the town in which the valve resides.
        /// </summary>
        [Column(Name = "ValveNumber", Storage = "_valveNumber", DbType = "VarChar(15)")]
        public string ValveNumber
        {
            get { return _valveNumber; }
            set
            {
                if (value != null && value.Length > MAX_VALVE_NUMBER_LENGTH)
                    throw new StringTooLongException("ValveNumber",
                        MAX_VALVE_NUMBER_LENGTH);
                if (_valveNumber != value)
                {
                    SendPropertyChanging();
                    _valveNumber = value;
                    SendPropertyChanged("ValveNumber");
                }
            }
        }

        /// <summary>
        /// Represents the numerical portion of a valve number, without the letter code
        /// prefix or suffix.
        ///
        /// Waiting to hear back from Doug on whether this field should be used to display
        /// valves in a DropDownList, or if another logical property should be created
        /// to remove the prefix from a valve number, but leave the entire suffix.
        /// </summary>
        [Column(Name = "ValveSuffix", Storage = "_valveSuffix", DbType = "Int")]
        public int? ValveSuffix
        {
            get { return _valveSuffix; }
            set
            {
                if (_valveSuffix != value)
                {
                    SendPropertyChanging();
                    _valveSuffix = value;
                    SendPropertyChanged("ValveSuffix");
                }
            }
        }

        [Association(Name = "Valve_MainBreakValveOperation", Storage = "_mainBreakValveOperations", OtherKey = "ValveID")]
        public EntitySet<MainBreakValveOperation> MainBreakValveOperations
        {
            get { return _mainBreakValveOperations; }
            set { _mainBreakValveOperations.Assign(value); }
        }

        [Association(Name = "SurveyStartingPoint_DetectedLeak", Storage = "_detectedLeaks", OtherKey = "SurveyStartingPointID")]
        public EntitySet<DetectedLeak> DetectedLeaks
        {
            get { return _detectedLeaks; }
            set { _detectedLeaks.Assign(value); }
        }

        [Association(Name = "Valve_WorkOrder", Storage = "_workOrders", OtherKey = "ValveID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Column(Name = "UpdatedById", Storage = "_lastUpdatedById", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? LastUpdatedById
        {
            get { return _lastUpdatedById; }
            set { _lastUpdatedById = value; }
        }

        [Association(Name = "LastUpdatedBy_Hydrant", Storage = "_lastUpdatedBy", ThisKey = "LastUpdatedById", IsForeignKey = true)]
        public Employee LastUpdatedBy => _lastUpdatedBy.Entity;

        public Employee UpdatedBy => LastUpdatedBy;

        #endregion

        #region Constructors

        public Valve()
        {
            _detectedLeaks = new EntitySet<DetectedLeak>(attach_DetectedLeaks, detach_DetectedLeaks);
            _mainBreakValveOperations = new EntitySet<MainBreakValveOperation>(attach_MainBreakValveOperations, detach_MainBreakValveOperations);
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _street = default(EntityRef<Street>);
            _crossStreet = default(EntityRef<Street>);
            _town = default(EntityRef<Town>);
            _townSection = default(EntityRef<TownSection>);
            _coordinate = default(EntityRef<Coordinate>);
            _operatingCenter = default(EntityRef<OperatingCenter>);
            _assetStatus = default(EntityRef<AssetStatus>);
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_MainBreakValveOperations(MainBreakValveOperation entity)
        {
            SendPropertyChanging();
            entity.Valve = this;
        }

        private void detach_MainBreakValveOperations(MainBreakValveOperation entity)
        {
            SendPropertyChanging();
            entity.Valve = null;
        }

        private void attach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.SurveyStartingPoint = this;
        }

        private void detach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.SurveyStartingPoint = null;
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Valve = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Valve = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullValveSuffix;
        }

        #endregion
    }
}
