using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Streets")]
    public class Street : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _fullStName,
                       _isActive,
                       _streetName;

        private int _streetID, _townID;

        private int? _prefixId, _suffixId;

        private EntityRef<StreetPrefix> _prefix;

        private EntityRef<StreetSuffix> _suffix;

        private readonly EntitySet<WorkOrder> _workOrders, _workOrders1;

        private readonly EntitySet<Valve> _valves;

        private readonly EntitySet<Hydrant> _hydrants;

        private readonly EntitySet<SewerOpening> _sewerOpenings;

        private readonly EntitySet<StormCatch> _stormCatches;

        private readonly EntitySet<SpoilStorageLocation> _spoilStorageLocations;

        private readonly EntitySet<SpoilFinalProcessingLocation> _spoilFinalProcessingLocations;

        #endregion

        #region Properties

        [Column(Name = "StreetID", Storage = "_streetID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int StreetID
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

        [Column(Storage = "_fullStName", DbType = "VarChar(50)")]
        public string FullStName
        {
            get { return _fullStName; }
            set
            {
                if (_fullStName != value)
                {
                    SendPropertyChanging();
                    _fullStName = value;
                    SendPropertyChanged("FullStName");
                }
            }
        }

        [Column(Storage = "_isActive", DbType = "bit NOT NULL")]
        public string IsActive
        {
            get { return _isActive; }
            set
            {
                if ((_isActive!= value))
                {
                    SendPropertyChanging();
                    _isActive = value;
                    SendPropertyChanged("IsActive");
                }
            }
        }

        [Column(Storage = "_streetName", DbType = "VarChar(30)")]
        public string StreetName
        {
            get { return _streetName; }
            set
            {
                if ((_streetName != value))
                {
                    SendPropertyChanging();
                    _streetName = value;
                    SendPropertyChanged("StreetName");
                }
            }
        }

        [Column(Storage = "_townID", DbType = "Int")]
        public int TownID
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

        [Column(Storage = "_prefixId", DbType = "Int")]
        public int? PrefixId
        {
            get { return _prefixId; }
            set
            {
                if (_prefixId != value)
                {
                    SendPropertyChanging();
                    _prefixId = value;
                    SendPropertyChanged("PrefixId");
                }
            }
        }

        [Column(Storage = "_suffixId", DbType = "Int")]
        public int? SuffixId
        {
            get { return _suffixId; }
            set
            {
                if (_suffixId != value)
                {
                    SendPropertyChanging();
                    _suffixId = value;
                    SendPropertyChanged("SuffixId");
                }
            }
        }

        [Association(Name = "Prefix_Street", Storage = "_prefix", ThisKey = "PrefixId", IsForeignKey = true)]
        public StreetPrefix Prefix
        {
            get { return _prefix.Entity; }
            set
            {
                var previousValue = _prefix.Entity;
                if ((previousValue != value)
                    || (_prefix.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _prefix.Entity = null;
                    }
                    _prefix.Entity = value;
                    if (value != null)
                    {
                        _prefixId = value.Id;
                    }
                    else
                    {
                        _prefixId = default(int?);
                    }
                    SendPropertyChanged("Street");
                }
            }
        }

        [Association(Name = "Suffix_Street", Storage = "_suffix", ThisKey = "SuffixId", IsForeignKey = true)]
        public StreetSuffix Suffix
        {
            get { return _suffix.Entity; }
            set
            {
                var previousValue = _suffix.Entity;
                if ((previousValue != value)
                    || (_suffix.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _suffix.Entity = null;
                    }
                    _suffix.Entity = value;
                    if (value != null)
                    {
                        _suffixId = value.Id;
                    }
                    else
                    {
                        _suffixId = default(int);
                    }
                    SendPropertyChanged("Street");
                }
            }
        }


        [Association(Name = "Street_WorkOrder", Storage = "_workOrders", OtherKey = "NearestCrossStreetID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Association(Name = "Street_WorkOrder1", Storage = "_workOrders1", OtherKey = "StreetID")]
        public EntitySet<WorkOrder> WorkOrders1
        {
            get { return _workOrders1; }
            set { _workOrders1.Assign(value); }
        }

        [Association(Name = "Street_Valve", Storage = "_valves", OtherKey = "StreetID")]
        public EntitySet<Valve> Valves
        {
            get { return _valves; }
            set { _valves.Assign(value); }
        }

        [Association(Name = "Street_Hydrant", Storage = "_hydrants", OtherKey = "StreetID")]
        public EntitySet<Hydrant> Hydrants
        {
            get { return _hydrants; }
            set { _hydrants.Assign(value); }
        }

        [Association(Name = "Street_SewerOpening", Storage = "_sewerOpenings", OtherKey = "StreetID")]
        public EntitySet<SewerOpening> SewerOpenings
        {
            get { return _sewerOpenings; }
            set { _sewerOpenings.Assign(value); }
        }

        [Association(Name = "Street_StormCatch", Storage = "_stormCatches", OtherKey = "StreetID")]
        public EntitySet<StormCatch> StormCatches
        {
            get { return _stormCatches; }
            set { _stormCatches.Assign(value); }
        }
        
        [Association(Name = "Street_SpoilStorageLocation", Storage = "_spoilStorageLocations", OtherKey = "StreetID")]
        public EntitySet<SpoilStorageLocation> SpoilStorageLocations
        {
            get { return _spoilStorageLocations; }
            set { _spoilStorageLocations.Assign(value); }
        }

        [Association(Name = "Street_SpoilFinalProcessingLocation", Storage = "_spoilFinalProcessingLocations", OtherKey = "StreetID")]
        public EntitySet<SpoilFinalProcessingLocation> SpoilFinalProcessingLocations
        {
            get { return _spoilFinalProcessingLocations; }
            set { _spoilFinalProcessingLocations.Assign(value); }
        }

        private readonly EntitySet<SewerOverflow> _sewerOverflows;

        [Association(Name = "Street_SewerOverflow", Storage = "_sewerOverflows", OtherKey = "StreetId")]
        public EntitySet<SewerOverflow> SewerOverflows
        {
            get => _sewerOverflows;
            set => _sewerOverflows.Assign(value);
        }

        #endregion

        #region Constructors

        public Street()
        {
            _sewerOverflows = new EntitySet<SewerOverflow>();
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _workOrders1 = new EntitySet<WorkOrder>(attach_WorkOrders1, detach_WorkOrders1);
            _valves = new EntitySet<Valve>(attach_Valves, detach_Valves);
            _hydrants = new EntitySet<Hydrant>(attach_Hydrants, detach_Hydrants);
            _sewerOpenings = new EntitySet<SewerOpening>(attach_SewerOpenings, detach_SewerOpenings);
            _stormCatches = new EntitySet<StormCatch>(attach_StormCatches, detach_StormCatches);

            _spoilStorageLocations =
                new EntitySet<SpoilStorageLocation>(
                    attach_SpoilStorageLocations, detach_SpoilStorageLocations);
            _spoilFinalProcessingLocations =
                new EntitySet<SpoilFinalProcessingLocation>(
                    attach_SpoilFinalProcessingLocations,
                    detach_SpoilFinalProcessingLocations);
        }

        #endregion

        #region Private Methods

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

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.NearestCrossStreet = this;
        }
        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.NearestCrossStreet = null;
        }

        private void attach_WorkOrders1(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_WorkOrders1(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        private void attach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        private void attach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        private void attach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        private void attach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        private void attach_SpoilStorageLocations(SpoilStorageLocation entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_SpoilStorageLocations(SpoilStorageLocation entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        private void attach_SpoilFinalProcessingLocations(SpoilFinalProcessingLocation entity)
        {
            SendPropertyChanging();
            entity.Street = this;
        }
        private void detach_SpoilFinalProcessingLocations(SpoilFinalProcessingLocation entity)
        {
            SendPropertyChanging();
            entity.Street = null;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullStName;
        }

        public int CompareTo(object other)
        {
            var otherStreet = other as Street;
            return otherStreet == null ? -1 : CompareTo(otherStreet);
        }

        public int CompareTo(Street other)
        {
            return FullStName.CompareTo(other == null ? null : other.FullStName);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
