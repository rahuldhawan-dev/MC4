using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Towns")]
    public class Town : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Methods

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _townID, _countyID;

        private string _address, 
                       _contactName, 
                       _criticalMainBreakNotes,
                       _fD1Contact, 
                       _fD1Fax, 
                       _fD1Phone, 
                       _emergContact, 
                       _emergFax, 
                       _emergPhone, 
                       _fax, 
                       _lat, 
                       _lon, 
                       _phone, 
                       _town, 
                       _townName, 
                       _zip;

        private double? _districtID;

        private char? _link;

        private int? _abbreviationTypeID;

        private EntityRef<County> _county;
        private EntityRef<State> _state;

        private readonly EntitySet<WorkOrder> _workOrders;

        private readonly EntitySet<Valve> _valves;

        private readonly EntitySet<Hydrant> _hydrants;

        private readonly EntitySet<MainCrossing> _mainCrossings;

        private readonly EntitySet<SewerOpening> _sewerOpenings;

        private readonly EntitySet<StormCatch> _stormCatches;

        private readonly EntitySet<Facility> _facilities;

        private readonly EntitySet<SpoilStorageLocation> _spoilStorageLocations;

        private readonly EntitySet<SpoilFinalProcessingLocation> _spoilFinalProcessingLocations;

        private readonly EntitySet<OperatingCenterTown> _operatingCentersTowns;

        private readonly EntitySet<TownContact> _townsContacts;

            #endregion

        #region Properties

        [Column(Name = "TownID", Storage = "_townID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
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
        
        [Column(Storage = "_address", DbType = "VarChar(50)")]
        public string Address
        {
            get { return _address; }
            set
            {
                if ((_address != value))
                {
                    SendPropertyChanging();
                    _address = value;
                    SendPropertyChanged("Address");
                }
            }
        }

        [Column(Storage = "_countyID", DbType = "Int")]
        public int CountyID
        {
            get { return _countyID; }
            set
            {
                if ((_countyID != value))
                {
                    SendPropertyChanging();
                    _countyID = value;
                    SendPropertyChanged("CountyID");
                }
            }
        }

        [Column(Storage = "_contactName", DbType = "VarChar(100)")]
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                if ((_contactName != value))
                {
                    SendPropertyChanging();
                    _contactName = value;
                    SendPropertyChanged("ContactName");
                }
            }
        }

        [Column(Storage = "_criticalMainBreakNotes", DbType = "VarChar(255)")]
        public string CriticalMainBreakNotes
        {
            get { return _criticalMainBreakNotes; }
            set
            {
                if (_criticalMainBreakNotes != value)
                {
                    SendPropertyChanging();
                    _criticalMainBreakNotes = value;
                    SendPropertyChanged("CriticalMainBreakNotes");
                }
            }
        }

        [Column(Storage = "_districtID", DbType = "Float")]
        public double? DistrictID
        {
            get { return _districtID; }
            set
            {
                if ((_districtID != value))
                {
                    SendPropertyChanging();
                    _districtID = value;
                    SendPropertyChanged("DistrictID");
                }
            }
        }

        [Column(Storage = "_emergContact", DbType = "VarChar(25)")]
        public string EmergContact
        {
            get { return _emergContact; }
            set
            {
                if ((_emergContact != value))
                {
                    SendPropertyChanging();
                    _emergContact = value;
                    SendPropertyChanged("EmergContact");
                }
            }
        }

        [Column(Storage = "_emergFax", DbType = "VarChar(12)")]
        public string EmergFax
        {
            get { return _emergFax; }
            set
            {
                if ((_emergFax != value))
                {
                    SendPropertyChanging();
                    _emergFax = value;
                    SendPropertyChanged("EmergFax");
                }
            }
        }

        [Column(Storage = "_emergPhone", DbType = "VarChar(12)")]
        public string EmergPhone
        {
            get { return _emergPhone; }
            set
            {
                if ((_emergPhone != value))
                {
                    SendPropertyChanging();
                    _emergPhone = value;
                    SendPropertyChanged("EmergPhone");
                }
            }
        }

        [Column(Storage = "_fax", DbType = "VarChar(12)")]
        public string Fax
        {
            get { return _fax; }
            set
            {
                if ((_fax != value))
                {
                    SendPropertyChanging();
                    _fax = value;
                    SendPropertyChanged("Fax");
                }
            }
        }

        [Column(Storage = "_fD1Contact", DbType = "VarChar(25)")]
        public string FD1Contact
        {
            get { return _fD1Contact; }
            set
            {
                if ((_fD1Contact != value))
                {
                    SendPropertyChanging();
                    _fD1Contact = value;
                    SendPropertyChanged("FD1Contact");
                }
            }
        }

        [Column(Storage = "_fD1Fax", DbType = "VarChar(12)")]
        public string FD1Fax
        {
            get { return _fD1Fax; }
            set
            {
                if ((_fD1Fax != value))
                {
                    SendPropertyChanging();
                    _fD1Fax = value;
                    SendPropertyChanged("FD1Fax");
                }
            }
        }

        [Column(Storage = "_fD1Phone", DbType = "VarChar(12)")]
        public string FD1Phone
        {
            get { return _fD1Phone; }
            set
            {
                if ((_fD1Phone != value))
                {
                    SendPropertyChanging();
                    _fD1Phone = value;
                    SendPropertyChanged("FD1Phone");
                }
            }
        }

        [Column(Storage = "_lat", DbType = "VarChar(50)")]
        public string Lat
        {
            get { return _lat; }
            set
            {
                if ((_lat != value))
                {
                    SendPropertyChanging();
                    _lat = value;
                    SendPropertyChanged("Lat");
                }
            }
        }

        [Column(Storage = "_lon", DbType = "VarChar(50)")]
        public string Lon
        {
            get { return _lon; }
            set
            {
                if ((_lon != value))
                {
                    SendPropertyChanging();
                    _lon = value;
                    SendPropertyChanged("Lon");
                }
            }
        }

        [Column(Storage = "_phone", DbType = "VarChar(50)")]
        public string Phone
        {
            get { return _phone; }
            set
            {
                if ((_phone != value))
                {
                    SendPropertyChanging();
                    _phone = value;
                    SendPropertyChanged("Phone");
                }
            }
        }

        [Column(Name = "Town", Storage = "_town", DbType = "VarChar(50)")]
        public string Name
        {
            get { return _town; }
            set
            {
                if ((_town != value))
                {
                    SendPropertyChanging();
                    _town = value;
                    SendPropertyChanged("Name");
                }
            }
        }

        [Column(Name = "TownName", Storage = "_townName", DbType = "VarChar(50)")]
        public string FullName
        {
            get { return _townName; }
            set
            {
                if ((_townName != value))
                {
                    SendPropertyChanging();
                    _townName = value;
                    SendPropertyChanged("FullName");
                }
            }
        }

        [Column(Storage = "_zip", DbType = "VarChar(10)")]
        public string Zip
        {
            get { return _zip; }
            set
            {
                if ((_zip != value))
                {
                    SendPropertyChanging();
                    _zip = value;
                    SendPropertyChanged("Zip");
                }
            }
        }

        [Column(Storage = "_link", DbType = "Char(1)")]
        public char? Link
        {
            get { return _link; }
            set
            {
                if ((_link != value))
                {
                    SendPropertyChanging();
                    _link = value;
                    SendPropertyChanged("Link");
                }
            }
        }

        [Column(Storage = "_abbreviationTypeID", DbType = "Int")]
        public int? AbbreviationTypeID
        {
            get { return _abbreviationTypeID; }
            set
            {
                if ((_abbreviationTypeID != value))
                {
                    SendPropertyChanging();
                    _abbreviationTypeID = value;
                    SendPropertyChanged("AbbreviationTypeID");
                }
            }
        }

        [Association(Name = "County_Town", Storage = "_county", ThisKey = "CountyID", OtherKey="CountyID")]
        public County County
        {
            get { return _county.Entity; }
            set 
            { 
                var previousValue = _county.Entity;
                if ((previousValue != value) || (_county.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _county.Entity = null;
                    }
                    _county.Entity = value;
                    if (value != null)
                    {
                        _countyID = value.CountyID;
                    }
                    else
                        _countyID = default(int);
                    SendPropertyChanged("County");
                }
            }
        }
        
        public State State
        {
            get { return County.State; }
        }

        [Association(Name = "Town_WorkOrder", Storage = "_workOrders", OtherKey = "TownID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Association(Name = "Town_Valve", Storage = "_valves", OtherKey = "TownID")]
        public EntitySet<Valve> Valves
        {
            get { return _valves; }
            set { _valves.Assign(value); }
        }

        [Association(Name = "Town_Hydrant", Storage = "_hydrants", OtherKey = "TownID")]
        public EntitySet<Hydrant> Hydrants
        {
            get { return _hydrants; }
            set { _hydrants.Assign(value); }
        }

        [Association(Name = "Town_MainCrossings", Storage = "_mainCrossings", OtherKey = "TownID")]
        public EntitySet<MainCrossing> MainCrossings
        {
            get { return _mainCrossings;}
            set { _mainCrossings.Assign(value);}
        }

        [Association(Name = "Town_SewerOpening", Storage = "_sewerOpenings", OtherKey = "TownID")]
        public EntitySet<SewerOpening> SewerOpenings
        {
            get { return _sewerOpenings; }
            set { _sewerOpenings.Assign(value); }
        }

        [Association(Name = "Town_StormCatch", Storage = "_stormCatches", OtherKey="TownID")]
        public EntitySet<StormCatch> StormCatches
        {
            get { return _stormCatches; }
            set { _stormCatches.Assign(value); }
        }

        [Association(Name = "Town_Facilities", Storage = "_facilities", OtherKey = "TownID")]
        public EntitySet<Facility> Facilities
        {
            get { return _facilities; }
            set { _facilities.Assign(value); }
        }

        [Association(Name = "Town_SpoilStorageLocation", Storage = "_spoilStorageLocations", OtherKey = "TownID")]
        public EntitySet<SpoilStorageLocation> SpoilStorageLocations
        {
            get { return _spoilStorageLocations; }
            set { _spoilStorageLocations.Assign(value); }
        }

        [Association(Name = "Town_SpoilFinalProcessingLocation", Storage = "_spoilFinalProcessingLocations", OtherKey = "TownID")]
        public EntitySet<SpoilFinalProcessingLocation> SpoilFinalProcessingLocations
        {
            get { return _spoilFinalProcessingLocations; }
            set { _spoilFinalProcessingLocations.Assign(value); }
        }

        [Association(Name = "OperatingCenter_OperatingCentersTowns", Storage = "_operatingCentersTowns", OtherKey = "TownID")]
        public EntitySet<OperatingCenterTown> OperatingCentersTowns
        {
            get { return _operatingCentersTowns; }
            set { _operatingCentersTowns.Assign(value); }
        }

        [Association(Name = "Contact_TownsContacts", Storage = "_townsContacts", OtherKey = "TownID")]
        public EntitySet<TownContact> TownContacts
        {
            get { return _townsContacts; }
            set { _townsContacts.Assign(value); }
        }

        private readonly EntitySet<SewerOverflow> _sewerOverflows;

        [Association(Name = "Town_SewerOverflow", Storage = "_sewerOverflows", OtherKey = "TownId")]
        public EntitySet<SewerOverflow> SewerOverflows
        {
            get => _sewerOverflows;
            set => _sewerOverflows.Assign(value);
        }

        #endregion

        #region Constructors

        public Town()
        {
            _sewerOverflows = new EntitySet<SewerOverflow>();
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _valves = new EntitySet<Valve>(attach_Valves, detach_Valves);
            _hydrants = new EntitySet<Hydrant>(attach_Hydrants, detach_Hydrants);
            _mainCrossings = new EntitySet<MainCrossing>(attach_MainCrossings, detach_MainCrossings);
            _sewerOpenings = new EntitySet<SewerOpening>(attach_SewerOpenings, detach_SewerOpenings);
            _stormCatches = new EntitySet<StormCatch>(attach_StormCatches,detach_StormCatches);
            _facilities = new EntitySet<Facility>(attach_Facilities, detach_Facilities);
            _spoilStorageLocations = new EntitySet<SpoilStorageLocation>(
                attach_SpoilStorageLocations,
                detach_SpoilStorageLocations);
            _spoilFinalProcessingLocations = new EntitySet<SpoilFinalProcessingLocation>(
                attach_SpoilFinalProcessingLocations,
                detach_SpoilFinalProcessingLocations);
            _operatingCentersTowns = new EntitySet<OperatingCenterTown>(
                    attach_OperatingCentersTowns,
                    detach_OperatingCentersTowns
                );
            _townsContacts = new EntitySet<TownContact>(
                attach_TownContacts, detach_TownContacts);
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

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_MainCrossings(MainCrossing entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }

        private void detach_MainCrossings(MainCrossing entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_Facilities(Facility entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }

        private void detach_Facilities(Facility entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_SpoilStorageLocations(SpoilStorageLocation entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_SpoilStorageLocations(SpoilStorageLocation entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_SpoilFinalProcessingLocations(SpoilFinalProcessingLocation entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_SpoilFinalProcessingLocations(SpoilFinalProcessingLocation entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        private void attach_OperatingCentersTowns(OperatingCenterTown entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }
        private void detach_OperatingCentersTowns(OperatingCenterTown entity)
        {
            SendPropertyChanging();
            entity.Town= null;
        }

        private void attach_TownContacts(TownContact entity)
        {
            SendPropertyChanging();
            entity.Town = this;
        }

        private void detach_TownContacts(TownContact entity)
        {
            SendPropertyChanging();
            entity.Town = null;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
