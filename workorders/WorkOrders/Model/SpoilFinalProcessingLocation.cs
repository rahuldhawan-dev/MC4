using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SpoilFinalProcessingLocations")]
    public class SpoilFinalProcessingLocation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        public const short MAX_NAME_LENGTH = 30;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _SpoilFinalProcessingLocationID,
                    _operatingCenterID;

        private int? _townID,
                     _streetID;

        private string _name;

        private EntityRef<OperatingCenter> _operatingCenter;

        private EntityRef<Town> _town;

        private EntityRef<Street> _street;

        private readonly EntitySet<SpoilRemoval> _spoilRemovals;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_SpoilFinalProcessingLocationID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int SpoilFinalProcessingLocationID
        {
            get { return _SpoilFinalProcessingLocationID; }
            set
            {
                if (_SpoilFinalProcessingLocationID != value)
                {
                    SendPropertyChanging();
                    _SpoilFinalProcessingLocationID = value;
                    SendPropertyChanged("SpoilFinalProcessingLocationID");
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

        #endregion

        #region Association Properties

        [Association(Name = "OperatingCenter_SpoilFinalProcessingLocation", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.SpoilFinalProcessingLocations.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.SpoilFinalProcessingLocations.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "Town_SpoilFinalProcessingLocation", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
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
                        previousValue.SpoilFinalProcessingLocations.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.SpoilFinalProcessingLocations.Add(this);
                        _townID = value.TownID;
                    }
                    else
                        _townID = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "Street_SpoilFinalProcessingLocation", Storage = "_street", ThisKey = "StreetID", IsForeignKey = true)]
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
                        previousValue.SpoilFinalProcessingLocations.Remove(this);
                    }
                    _street.Entity = value;
                    if (value != null)
                    {
                        value.SpoilFinalProcessingLocations.Add(this);
                        _streetID = value.StreetID;
                    }
                    else
                        _streetID = default(int);
                    SendPropertyChanged("Street");
                }
            }
        }

        [Association(Name = "FinalDestination_SpoilRemoval", Storage = "_spoilRemovals", OtherKey = "FinalDestinationID")]
        public EntitySet<SpoilRemoval> SpoilRemovals
        {
            get { return _spoilRemovals; }
            set { _spoilRemovals.Assign(value); }
        }

        #endregion

        #endregion

        #region Constructors

        public SpoilFinalProcessingLocation()
        {
            _spoilRemovals = new EntitySet<SpoilRemoval>(attach_SpoilRemovals,
                detach_SpoilRemovals);
        }

        #endregion

        #region Private Members

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

        private void attach_SpoilRemovals(SpoilRemoval entity)
        {
            SendPropertyChanging();
            entity.FinalDestination = this;
        }
        private void detach_SpoilRemovals(SpoilRemoval entity)
        {
            SendPropertyChanging();
            entity.FinalDestination = null;
        }

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (OperatingCenter == null)
                        throw new DomainLogicException(
                            "Cannot create a new SpoilFinalProcessingLocation without an OperatingCenter.");
                    if (String.IsNullOrEmpty(Name))
                        throw new DomainLogicException("Name cannot be null");
                    break;
            }
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
