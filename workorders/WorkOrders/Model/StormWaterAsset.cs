using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.StormWaterAssets")]
    public class StormCatch : INotifyPropertyChanging, INotifyPropertyChanged, IAsset
    {
        #region Constants

        private const short MAX_ASSET_NUMBER_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _stormCatchID,
                    _operatingCenterID,
                    _townID,
                    _stormWaterAssetTypeID;

        private string _assetNumber;

        private int? _streetID,
                     _assetStatusID,
                     _coordinateID;

        private EntityRef<Coordinate> _coordinate;
        private EntityRef<AssetStatus> _assetStatus;
        private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<Town> _town;
        private EntityRef<Street> _street;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        #region Table Properties

        [Column(Name="StormWaterAssetID", Storage = "_stormCatchID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int StormCatchID
        {
            get
            {
                return _stormCatchID;
            }
            set
            {
                if ((_stormCatchID != value))
                {
                    SendPropertyChanging();
                    _stormCatchID = value;
                    SendPropertyChanged("StormCatchID");
                }
            }
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

        [Column(Storage = "_assetNumber", DbType = "VarChar(50)")]
        public string AssetNumber
        {
            get
            {
                return _assetNumber;
            }
            set
            {
                if (value != null && value.Length > MAX_ASSET_NUMBER_LENGTH)
                    throw new StringTooLongException("AssetNumber", MAX_ASSET_NUMBER_LENGTH);
                if ((_assetNumber != value))
                {
                    SendPropertyChanging();
                    _assetNumber = value;
                    SendPropertyChanged("AssetNumber");
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

        #endregion

        #region Associations

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
                        previousValue.StormCatches.Remove(this);
                    }
                    _coordinate.Entity = value;
                    if ((value != null))
                    {
                        value.StormCatches.Add(this);
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
                        previousValue.StormCatches.Remove(this);
                    }
                    _assetStatus.Entity = value;
                    if ((value != null))
                    {
                        value.StormCatches.Add(this);
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

        [Association(Name = "OperatingCenter_StormCatch", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.StormCatches.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.StormCatches.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "Town_StormCatch", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
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
                        previousValue.StormCatches.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.StormCatches.Add(this);
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
                        previousValue.StormCatches.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.StormCatches.Add(this);
                        _streetID = value.StreetID;
                    }
                    else
                        _streetID = default(int);
                    SendPropertyChanged("Street");
                }
            }
        }

        #endregion 

        #region Implementation of IAsset

        [Association(Name = "StormCatch_WorkOrder", Storage = "_workOrders", OtherKey = "StormCatchID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        public object AssetKey
        {
            get { return StormCatchID; }
        }

        public string AssetID
        {
            get { return AssetNumber; }
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

        public StormCatch()
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
            entity.StormCatch = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.StormCatch = null;
        }
        
        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return AssetNumber;
        }

        #endregion
    }
}
