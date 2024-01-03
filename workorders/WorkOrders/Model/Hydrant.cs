using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text.RegularExpressions;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    /// <summary>
    /// Represents the Hydrant assets scattered throughout various Towns.
    /// </summary>
    [Table(Name = "dbo.Hydrants")]
    public class Hydrant : INotifyPropertyChanging, INotifyPropertyChanged, IAsset
    {
        #region Constants

        private const short MAX_NEAREST_CROSS_STREET_NAME_LENGTH = 30,
                            MAX_OPCODE_LENGTH = 4,
                            MAX_HYDRANT_NUMBER_LENGTH = 12,
                            MAX_STATUS_LENGTH = 10;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        private static readonly Regex rgxHydrantSuffix = new Regex(@"^[a-zA-Z\d]+-(.+)$");

        #endregion

        #region Private Members

        private int _hydrantID, _assetStatusId, _operatingCenterId;

        private int? _coordinateId,
                     _streetID,
                     _crossStreetId,
                     _townID,
                     _sapEquipmentID,
                     _hydrantSuffix, 
                     _route, 
                     _hydrantBillingId, 
                     _manufacturerID,
                     _fireDistrictId,
                     _lastUpdatedById;

        private string _hydrantNumber,
            _opCode,
            _status,
            _fullHydrantSuffix,
            _criticalNotes;

        private decimal? _stop;
        private double? _latitude, _longitude;
        
        private DateTime? _dateInstalled, _lastUpdated;

        private EntityRef<Street> _street, _crossStreet;
        private EntityRef<Town> _town;
        private EntityRef<Coordinate> _coordinate;
        private EntityRef<AssetStatus> _assetStatus;
        private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<HydrantBilling> _hydrantBilling;
        private EntityRef<HydrantManufacturer> _hydrantManufacturer;
        private EntityRef<FireDistrict> _fireDistrict;
        private EntityRef<Employee> _lastUpdatedBy;
        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the full Hydrant suffix, basically the HydrantNumber
        /// without the letter prefix and hyphen.
        /// </summary>
        public string FullHydrantSuffix
        {
            get
            {
                if (_fullHydrantSuffix == null)
                    _fullHydrantSuffix =
                        rgxHydrantSuffix.Match(HydrantNumber).Groups[1].Value;
                return _fullHydrantSuffix;
            }
        }

        /// <summary>
        /// Key value of the Hydrant for persistence. (Primary Key of the
        /// Hydrant table).
        /// </summary>
        [Column(Name = "Id", Storage = "_hydrantID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int HydrantID
        {
            get { return _hydrantID; }
            set
            {
                if (_hydrantID != value)
                {
                    SendPropertyChanging();
                    _hydrantID = value;
                    SendPropertyChanged("HydrantID");
                }
            }
        }

        [Column(Name = "HydrantNumber", Storage = "_hydrantNumber", DbType = "VarChar(12)")]
        public string HydrantNumber
        {
            get { return _hydrantNumber; }
            set
            {
                if (value != null && value.Length > MAX_HYDRANT_NUMBER_LENGTH)
                    throw new StringTooLongException("HydrantNumber", MAX_HYDRANT_NUMBER_LENGTH);
                if (_hydrantNumber != value)
                {
                    SendPropertyChanging();
                    _hydrantNumber = value;
                    SendPropertyChanged("HydrantNumber");
                }
            }
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

        [Column(Name="HydrantSuffix", Storage="_hydrantSuffix", DbType="Int")]
        public int? HydrantSuffix
        {
            get { return _hydrantSuffix; }
            set
            {
                if (_hydrantSuffix != value)
                {
                    SendPropertyChanging();
                    _hydrantSuffix = value;
                    SendPropertyChanged("HydrantSuffix");
                }
            }
        }

        [Column(Name = "UpdatedAt", Storage = "_lastUpdated", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            set { _lastUpdated = value; }
        }

        public DateTime? UpdatedAt => LastUpdated;

        [Column(Name = "Route", Storage = "_route", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
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

        [Association(Name = "OperatingCenter_Hydrant", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.Hydrants.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.Hydrants.Add(this);
                        _operatingCenterId = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterId = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Column(Name = "AssetStatusId", Storage = "_assetStatusId", DbType = "Int")]
        public int AssetStatusID
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

        [Association(Name = "AssetStatus_Hydrant", Storage = "_assetStatus", ThisKey = "AssetStatusID", IsForeignKey = true)]
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
                    SendPropertyChanged("ValveStatus");
                }
            }
        }

        [Column(Name = "HydrantBillingId", Storage = "_hydrantBillingId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? HydrantBillingId
        {
            get { return _hydrantBillingId; }
            set { _hydrantBillingId = value; }
        }

        [Association(Name="HydrantBilling_Hydrant", Storage = "_hydrantBilling", ThisKey = "HydrantBillingId", IsForeignKey = true)]
        public HydrantBilling HydrantBilling => _hydrantBilling.Entity;
        
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
        
        [Association(Name = "CrossStreet", Storage = "_crossStreet", ThisKey = "CrossStreetID", IsForeignKey = true)]
        public Street CrossStreet
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
                        previousValue.Hydrants.Remove(this);
                    }
                    _crossStreet.Entity = value;
                    if (value != null)
                    {
                        value.Hydrants.Add(this);
                        _crossStreetId = value.StreetID;
                    }
                    else
                        _crossStreetId = default(int);
                    SendPropertyChanged("CrossStreet");
                }
            }
        }

        [Association(Name = "Street_Hydrant", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
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
                        previousValue.Hydrants.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.Hydrants.Add(this);
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

        [Association(Name="Coordinate_Hydrant", Storage = "_coordinate", ThisKey="CoordinateID", OtherKey = "CoordinateID", IsForeignKey = true)]
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
                        previousValve.Hydrants.Remove(this);
                    }
                    _coordinate.Entity = value;
                    if (value != null)
                    {
                        value.Hydrants.Add(this);
                        _coordinateId = value.CoordinateID;
                    }
                    else
                        _coordinateId = default(int);
                    SendPropertyChanged("Coordinate");
                }
            }
        }

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

        [Association(Name = "Town_Hydrant", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
        public Town Town
        {
            get { return _town.Entity; }
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
                        previousValue.Hydrants.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.Hydrants.Add(this);
                        _townID = value.TownID;
                    }
                    else
                        _townID = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "Hydrant_WorkOrder", Storage = "_workOrders", OtherKey = "HydrantID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Column(Name = "ManufacturerID", Storage = "_manufacturerID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? ManufacturerID
        {
            get { return _manufacturerID; }
            set { _manufacturerID = value; }
        }

        [Association(Name = "HydrantManufacturer_Hydrant", Storage = "_hydrantManufacturer", ThisKey = "ManufacturerID", IsForeignKey = true)]
        public HydrantManufacturer HydrantManufacturer => _hydrantManufacturer.Entity;

        [Column(Name = "FireDistrictId", Storage = "_fireDistrictId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? FireDistrictId
        {
            get { return _fireDistrictId; }
            set { _fireDistrictId = value; }
        }

        [Association(Name = "FireDistrict_Hydrant", Storage = "_fireDistrict", ThisKey = "FireDistrictId", IsForeignKey = true)]
        public FireDistrict FireDistrict => _fireDistrict.Entity;

        public string PremiseNumber => FireDistrict?.PremiseNumber;

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

        /// <summary>
        /// Creates a new instance of the Hydrant class.
        /// </summary>
        public Hydrant()
        {
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _street = default(EntityRef<Street>);
            _town = default(EntityRef<Town>);
            _coordinate = default(EntityRef<Coordinate>);
        }

        #endregion

        #region IAsset

        public object AssetKey
        {
            get { return HydrantID; }
        }

        public string AssetID
        {
            get { return FullHydrantSuffix; }
        }



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

        #endregion

        #region Private Methods

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Hydrant = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Hydrant = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullHydrantSuffix;
        }

        #endregion
    }
}
