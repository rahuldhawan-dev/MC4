using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name="dbo.BodiesOfWater")]
    public class BodyOfWater
    {
        private int _BodyOfWaterID;
        private string _name;

        [Column(Storage = "_BodyOfWaterID", Name="BodyOfWaterID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int BodyOfWaterID { get { return _BodyOfWaterID;} }

        [Column(Name="Name", Storage = "_name", DbType="VARCHAR(255) NOT NULL")]
        public string Name { get { return _name; } }
    }

    [Table(Name="dbo.MainCrossings")]
    public class MainCrossing : IAsset
    {
        #region Private Members

        private int _MainCrossingID, _mainCrossingStatusId;
        private int? _coordinateId, _streetId, _closestStreetId, _townId, _operatingCenterId, _bodyOfWaterId;
        private string _comments;
        private bool? _isCriticalAsset;

        private EntityRef<Coordinate> _coordinate;
        private EntityRef<Street> _street, _closestStreet;
        private EntityRef<Town> _town;
        private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<BodyOfWater> _bodyOfWater;
        private EntityRef<MainCrossingStatus> _mainCrossingStatus;
        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Name = "Comments", Storage = "_comments", DbType = "VarChar(150)")]
        public string Comments
        {
            get { return _comments; }
            set
            {
                if (value != null && value.Length > 150)
                    throw new StringTooLongException("Comments", 150);
                if (_comments != value)
                {
                    SendPropertyChanging();
                    _comments = value;
                    SendPropertyChanged("Comments");
                }
            }
        }

        [Column(Name= "IsCriticalAsset", Storage = "_isCriticalAsset", DbType = "bit")]
        public bool? IsCriticalAsset
        {
            get { return _isCriticalAsset; }
        }

        [Association(Name = "MainCrossing_WorkOrder", Storage = "_workOrders", OtherKey = "MainCrossingID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }
        
        [Column(Storage = "_MainCrossingID", Name = "MainCrossingID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainCrossingID
        {
            get { return _MainCrossingID; }
        }

        [Column(Name = "StreetID", Storage = "_streetId", DbType = "Int")]
        public int? StreetID
        {
            get{ return _streetId;}
        }

        [Association(Name = "Street_MainCrossing", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
        public Street Street
        {
            get { return _street.Entity; }
        }

        [Column(Name = "ClosestStreetID", Storage = "_streetId", DbType = "Int")]
        public int? ClosestStreetID
        {
            get { return _closestStreetId; }
        }

        [Association(Name = "ClosestStreet_MainCrossing", Storage = "_closestStreet", ThisKey = "ClosestStreetID", IsForeignKey = true)]
        public Street ClosestStreet
        {
            get { return _closestStreet.Entity; }
        }

        [Column(Name = "TownID", Storage = "_townId", DbType = "Int")]
        public int? TownID
        {
            get { return _townId; }
            set
            {
                if (_townId != value)
                {
                    SendPropertyChanging();
                    _townId = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        [Association(Name = "Town_MainCrossing", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
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
                        previousValue.MainCrossings.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.MainCrossings.Add(this);
                        _townId = value.TownID;
                    }
                    else
                        _townId = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }
        [Column(Name = "OperatingCenterID", Storage = "_operatingCenterId", DbType = "Int")]
        public int? OperatingCenterID
        { get { return _operatingCenterId; } }

        [Association(Name = "OperatingCenter_MainCrossing",
             Storage = "_operatingCenter", ThisKey = "OperatingCenterID",
             IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingCenter.Entity; }
        }
        [Column(Name = "BodyOfWaterID", Storage = "_bodyOfWaterId", DbType = "Int")]
        public int? BodyOfWaterID
        { get { return _bodyOfWaterId; } }

        [Association(Name = "BodyOfWater_MainCrossing",
             Storage = "_bodyOfWater", ThisKey = "BodyOfWaterID",
             IsForeignKey = true)]
        public BodyOfWater BodyOfWater
        {
            get { return _bodyOfWater.Entity; }
        }

        [Column(Name = "MainCrossingStatusId", Storage = "_mainCrossingStatusId", DbType = "Int")]
        public int MainCrossingStatusID
        {
            get { return _mainCrossingStatusId; }
            set
            {
                if (_mainCrossingStatusId != value)
                {
                    SendPropertyChanging();
                    _mainCrossingStatusId = value;
                    SendPropertyChanged("MainCrossingStatusID");
                }
            }
        }

        [Association(Name = "MainCrossingStatus_MainCrossing", Storage = "_mainCrossingStatus", ThisKey = "MainCrossingStatusID", IsForeignKey = true)]
        public MainCrossingStatus MainCrossingStatus
        {
            get { return _mainCrossingStatus.Entity; }
            set
            {
                var previousValue = _mainCrossingStatus.Entity;
                if ((previousValue != value) ||
                    (_mainCrossingStatus.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _mainCrossingStatus.Entity = value;
                        previousValue.MainCrossings.Remove(this);
                    }
                    _mainCrossingStatus.Entity = value;
                    if (value != null)
                    {
                        value.MainCrossings.Add(this);
                        _mainCrossingStatusId = value.MainCrossingStatusID;
                    }
                    else
                        _mainCrossingStatusId = default(int);
                    SendPropertyChanged("MainCrossingStatus");
                }
            }
        }


        #region IAsset

        [Column(Name = "CoordinateID", Storage = "_coordinateId", DbType = "Int")]
        public int? CoordinateID
        {
            get { return _coordinateId; }
        }

        [Association(Name = "Coordinate_MainCrossing", Storage = "_coordinate", ThisKey = "CoordinateID", OtherKey = "CoordinateID", IsForeignKey = true)]
        public Coordinate Coordinate
        {
            get { return _coordinate.Entity; }
        }

        public object AssetKey
        {
            get { return MainCrossingID; }
        }

        public string AssetID
        {
            get { return MainCrossingID.ToString(); }
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

        public MainCrossing()
        {
            _workOrders = new EntitySet<WorkOrder>();
            _street = default(EntityRef<Street>);
            _town = default(EntityRef<Town>);
            _coordinate = default(EntityRef<Coordinate>);
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

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        //TODO: This is duplicated in MapCall.Common.MainCrossing
        public virtual string Display
        {
            get
            {
                var display = string.Format("CR{0} - {1} - {2}", MainCrossingID, OperatingCenter.OpCntr, Town);

                display += (BodyOfWater != null ? " - " + BodyOfWater.Name : string.Empty);
                display += (Street != null) ? " - " + Street.FullStName : string.Empty;
                display += (ClosestStreet != null) ? " - " + ClosestStreet.FullStName : string.Empty;
                return display;
            }
        }

        public override string ToString()
        {
            return Display;
        }

        #endregion

    }
}