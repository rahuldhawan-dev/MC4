using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SpoilStorageLocations")]
    public class SpoilStorageLocation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        public const short MAX_NAME_LENGTH = 30;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _spoilStorageLocationID,
                    _operatingCenterID;

        private int? _townID,
                     _streetID;

        private string _name;

        private bool _active;

        private EntityRef<OperatingCenter> _operatingCenter;

        private EntityRef<Town> _town;

        private EntityRef<Street> _street;

        private readonly EntitySet<Spoil> _spoils;

        private readonly EntitySet<SpoilRemoval> _spoilRemovals;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_spoilStorageLocationID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int SpoilStorageLocationID
        {
            get { return _spoilStorageLocationID; }
            set
            {
                if (_spoilStorageLocationID != value)
                {
                    SendPropertyChanging();
                    _spoilStorageLocationID = value;
                    SendPropertyChanged("SpoilStorageLocationID");
                }
            }
        }

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
        public int OperatingCenterID
        {
            get { return _operatingCenterID; }
            set
            {
                if (_operatingCenterID != value)
                {
                    if (_operatingCenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _operatingCenterID = value;
                SendPropertyChanged("OperatingCenterID");
            }
        }

        [Column(Storage = "_townID", DbType = "Int NULL")]
        public int? TownID
        {
            get { return _townID; }
            set
            {
                if (_townID != value)
                {
                    if (_town.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _townID = value;
                SendPropertyChanged("TownID");
            }
        }

        [Column(Storage = "_streetID", DbType = "Int NULL")]
        public int? StreetID
        {
            get { return _streetID; }
            set
            {
                if (_streetID != value)
                {
                    if (_street.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _streetID = value;
                SendPropertyChanged("StreetID");
            }
        }

        [Column(Storage = "_name", DbType = "VarChar(30) NOT NULL")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > MAX_NAME_LENGTH)
                    throw new StringTooLongException("Name", MAX_NAME_LENGTH);
                if (_name != value)
                {
                    SendPropertyChanging();
                    _name = value;
                    SendPropertyChanged("Name");
                }
            }
        }

        [Column(Storage = "_active", DbType = "Bit NOT NULL")]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    SendPropertyChanging();
                    _active = value;
                    SendPropertyChanged("Active");
                }
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "SpoilStorageLocation_Spoil", Storage = "_spoils", OtherKey = "SpoilStorageLocationID")]
        public EntitySet<Spoil> Spoils
        {
            get { return _spoils; }
            set { _spoils.Assign(value); }
        }

        [Association(Name = "OperatingCenter_SpoilStorageLocation", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.SpoilStorageLocations.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.SpoilStorageLocations.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "Town_SpoilStorageLocation", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
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
                        previousValue.SpoilStorageLocations.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.SpoilStorageLocations.Add(this);
                        _townID = value.TownID;
                    }
                    else
                        _townID = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "Street_SpoilStorageLocation", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
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
                        previousValue.SpoilStorageLocations.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.SpoilStorageLocations.Add(this);
                        _streetID = value.StreetID;
                    }
                    else
                        _streetID = default(int);
                    SendPropertyChanged("Street");
                }
            }
        }

        [Association(Name = "RemovedFrom_SpoilRemoval", Storage = "_spoilRemovals", OtherKey = "RemovedFromID")]
        public EntitySet<SpoilRemoval> SpoilRemovals
        {
            get { return _spoilRemovals; }
            set { _spoilRemovals.Assign(value); }
        }

        #endregion

        #endregion

        #region Constructors

        public SpoilStorageLocation()
        {
            _spoils = new EntitySet<Spoil>(attach_Spoils, detach_Spoils);
            _spoilRemovals = new EntitySet<SpoilRemoval>(attach_SpoilRemovals,
                detach_SpoilRemovals);
        }

        #endregion

        #region Private Methods

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (OperatingCenter == null)
                        throw new DomainLogicException(
                            "Cannot create a new SpoilStorageLocation without an OperatingCenter.");
                    if (String.IsNullOrEmpty(Name))
                        throw new DomainLogicException("Name cannot be null");
                    break;
            }
        }

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, _emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_Spoils(Spoil entity)
        {
            SendPropertyChanging();
            entity.SpoilStorageLocation = this;
        }
        private void detach_Spoils(Spoil entity)
        {
            SendPropertyChanging();
            entity.SpoilStorageLocation = null;
        }

        private void attach_SpoilRemovals(SpoilRemoval entity)
        {
            SendPropertyChanging();
            entity.RemovedFrom = this;
        }
        private void detach_SpoilRemovals(SpoilRemoval entity)
        {
            SendPropertyChanging();
            entity.RemovedFrom = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
