using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name="dbo.SewerOpenings")]
    public class SewerOpening : INotifyPropertyChanging, INotifyPropertyChanged, IAsset
    {
        #region Constants

        private const short MAX_OPENING_NUMBER_LENGTH = 50;
        private const short MAX_MAPPAGE_LENGTH = 50;
        private const short MAX_CREATEDBY_LENGTH = 50;
        private const short MAX_TASKNUMBER_LENGTH = 50;

        #endregion
        
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id, _operatingCenterID, _townID;

        private string _openingNumber, _criticalNotes;

        private int? _streetID,
                     _assetStatusID,
                     _coordinateID,
                     _sapEquipmentID,
                     _route,
                     _stop,
                     _sewerOpeningMaterialId,
                     _lastUpdatedById;

        private decimal? _depthToInvert, _rimElevation;

        private DateTime? _dateInstalled, _lastUpdated;

        private EntityRef<Coordinate> _coordinate;
        private EntityRef<AssetStatus> _assetStatus;
        private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<Town> _town;
        private EntityRef<Street> _street;
        private EntityRef<SewerOpeningMaterial> _sewerOpeningMaterial;
        private EntityRef<Employee> _lastUpdatedBy;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if ((_id != value))
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged("Id");
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

        [Column(Name = "Stop", Storage = "_stop", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }

        [Column(Name = "DepthToInvert", Storage = "_depthToInvert", DbType = "decimal(18,2)", UpdateCheck = UpdateCheck.Never)]
        public decimal? DepthToInvert
        {
            get { return _depthToInvert; }
            set { _depthToInvert = value; }
        }

        [Column(Name = "RimElevation", Storage = "_rimElevation", DbType = "decimal(18,2)", UpdateCheck = UpdateCheck.Never)]
        public decimal? RimElevation
        {
            get { return _rimElevation; }
            set { _rimElevation = value; }
        }
        
        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
        public int OperatingCenterID
        {
            get
            {
                return _operatingCenterID;
            }
            set
            {
                if ((_operatingCenterID != value))
                {
                    SendPropertyChanging();
                    _operatingCenterID = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Column(Storage = "_townID", DbType = "Int NOT NULL")]
        public int TownID
        {
            get
            {
                return _townID;
            }
            set
            {
                if ((_townID != value))
                {
                    SendPropertyChanging();
                    _townID = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        [Column(Storage = "_openingNumber", DbType = "VarChar(50)")]
        public string OpeningNumber
        {
            get
            {
                return _openingNumber;
            }
            set
            {
                if (value != null && value.Length > MAX_OPENING_NUMBER_LENGTH)
                    throw new StringTooLongException("OpeningNumber", MAX_OPENING_NUMBER_LENGTH);
                if ((_openingNumber != value))
                {
                    SendPropertyChanging();
                    _openingNumber = value;
                    SendPropertyChanged("OpeningNumber");
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

        [Column(Storage = "_streetID", DbType = "Int")]
        public int? StreetID
        {
            get
            {
                return _streetID;
            }
            set
            {
                if ((_streetID != value))
                {
                    SendPropertyChanging();
                    _streetID = value;
                    SendPropertyChanged("StreetID");
                }
            }
        }

        [Column(Storage = "_coordinateID", DbType = "Int")]
        public int? CoordinateID
        {
            get
            {
                return _coordinateID;
            }
            set
            {
                if ((_coordinateID != value))
                {
                    if (_coordinate.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _coordinateID = value;
                    SendPropertyChanged("CoordinateID");
                }
            }
        }

        [Column(Storage = "_assetStatusID", DbType = "Int")]
        public int? AssetStatusID
        {
            get
            {
                return _assetStatusID;
            }
            set
            {
                if ((_assetStatusID != value))
                {
                    if (_assetStatus.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _assetStatusID = value;
                    SendPropertyChanged("AssetStatusID");
                }
            }
        }

        [Association(Name = "Coordinate_SewerOpening", Storage = "_coordinate", ThisKey = "CoordinateID", OtherKey = "CoordinateID", IsForeignKey = true)]
        public Coordinate Coordinate
        {
            get
            {
                return _coordinate.Entity;
            }
            set
            {
                Coordinate previousValue = _coordinate.Entity;
                if (((previousValue != value)
                            || (_coordinate.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _coordinate.Entity = null;
                        previousValue.SewerOpenings.Remove(this);
                    }
                    _coordinate.Entity = value;
                    if ((value != null))
                    {
                        value.SewerOpenings.Add(this);
                        _coordinateID = value.CoordinateID;
                    }
                    else
                    {
                        _coordinateID = default(int?);
                    }
                    SendPropertyChanged("Coordinate");
                }
            }
        }

        [Association(Name = "AssetStatuses_SewerOpening", Storage = "_assetStatus", ThisKey = "AssetStatusID", OtherKey = "AssetStatusID", IsForeignKey = true)]
        public AssetStatus AssetStatus
        {
            get
            {
                return _assetStatus.Entity;
            }
            set
            {
                var previousValue = _assetStatus.Entity;
                if (((previousValue != value)
                            || (_assetStatus.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _assetStatus.Entity = null;
                        previousValue.SewerOpenings.Remove(this);
                    }
                    _assetStatus.Entity = value;
                    if ((value != null))
                    {
                        value.SewerOpenings.Add(this);
                        _assetStatusID = value.AssetStatusID;
                    }
                    else
                    {
                        _assetStatusID = default(int?);
                    }
                    SendPropertyChanged("AssetStatus");
                }
            }
        }

        [Association(Name = "OperatingCenter_SewerOpening", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.SewerOpenings.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.SewerOpenings.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "Town_SewerOpening", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
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
                        previousValue.SewerOpenings.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.SewerOpenings.Add(this);
                        _townID = value.TownID;
                    }
                    else
                        _townID = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "Street_SewerOpening", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
        public Street Street
        {
            get { return _street.Entity; }
            set
            {
                Street previousValue = _street.Entity;
                if ((previousValue != value)
                    || (_street.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _street.Entity = null;
                        previousValue.SewerOpenings.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.SewerOpenings.Add(this);
                        _streetID = value.StreetID;
                    }
                    else
                        _streetID = default(int);
                    SendPropertyChanged("Street");
                }
            }
        }

        [Column(Name = "SewerOpeningMaterialId", Storage = "_sewerOpeningMaterialId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? SewerOpeningMaterialId
        {
            get { return _sewerOpeningMaterialId; }
            set { _sewerOpeningMaterialId = value; }
        }

        [Association(Name = "SewerOpeningMaterial_SewerOpening", Storage = "_sewerOpeningMaterial", ThisKey = "SewerOpeningMaterialId", IsForeignKey = true)]
        public SewerOpeningMaterial SewerOpeningMaterial => _sewerOpeningMaterial.Entity;

        [Column(Name = "UpdatedById", Storage = "_lastUpdatedById", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? LastUpdatedById
        {
            get { return _lastUpdatedById; }
            set { _lastUpdatedById = value; }
        }

        [Association(Name = "LastUpdatedBy_Hydrant", Storage = "_lastUpdatedBy", ThisKey = "LastUpdatedById", IsForeignKey = true)]
        public Employee LastUpdatedBy => _lastUpdatedBy.Entity;

        public Employee UpdatedBy => LastUpdatedBy;

        #region Implementation of IAsset

        [Association(Name = "SewerOpening_WorkOrder", Storage = "_workOrders", OtherKey = "SewerOpeningID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        public object AssetKey
        {
            get { return Id; }
        }

        public string AssetID
        {
            get { return OpeningNumber; }
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

        #endregion

        #region Constructors

        public SewerOpening()
        {
            _coordinate = default(EntityRef<Coordinate>);
            _assetStatus = default(EntityRef<AssetStatus>);
            _operatingCenter = default(EntityRef<OperatingCenter>);
            _town = default(EntityRef<Town>);
            _street = default(EntityRef<Street>);
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, _emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.SewerOpening = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.SewerOpening = null;
        }

        #endregion
        
        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return OpeningNumber;
        }

        #endregion
    }
}
