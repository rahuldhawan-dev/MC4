using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SpoilRemovals")]
    public class SpoilRemoval : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _spoilRemovalID,
                    _removedFromID,
                    _finalDestinationID;
        private DateTime _dateRemoved;
        private decimal _quantity;

        private EntityRef<SpoilStorageLocation> _removedFrom;

        private EntityRef<SpoilFinalProcessingLocation> _finalDestination;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_spoilRemovalID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int SpoilRemovalID
        {
            get { return _spoilRemovalID; }
            set
            {
                if (_spoilRemovalID != value)
                {
                    SendPropertyChanging();
                    _spoilRemovalID = value;
                    SendPropertyChanged("SpoilRemovalID");
                }
            }
        }

        [Column(Storage = "_removedFromID", DbType = "Int NOT NULL")]
        public int RemovedFromID
        {
            get { return _removedFromID; }
            set
            {
                if (_removedFromID != value)
                {
                    if (_removedFrom.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _removedFromID = value;
                SendPropertyChanged("RemovedFromID");
            }
        }

        [Column(Storage = "_finalDestinationID", DbType = "Int NOT NULL")]
        public int FinalDestinationID
        {
            get { return _finalDestinationID; }
            set
            {
                if (_finalDestinationID != value)
                {
                    if (_finalDestination.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _finalDestinationID = value;
                SendPropertyChanged("FinalDestinationID");
            }
        }

        [Column(Storage = "_dateRemoved", DbType = "SmallDateTime NOT NULL")]
        public DateTime DateRemoved
        {
            get { return _dateRemoved; }
            set
            {
                if (_dateRemoved != value)
                {
                    SendPropertyChanging();
                    _dateRemoved = value;
                    SendPropertyChanged("DateRemoved");
                }
            }
        }

        [Column(Storage = "_quantity", DbType = "Decimal(6,2) NOT NULL")]
        public decimal Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    SendPropertyChanging();
                    _quantity = value;
                    SendPropertyChanged("Quantity");
                }
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "RemovedFrom_SpoilRemoval", Storage = "_removedFrom", ThisKey = "RemovedFromID", IsForeignKey = true)]
        public SpoilStorageLocation RemovedFrom
        {
            get { return _removedFrom.Entity; }
            set
            {
                SpoilStorageLocation previousValue = _removedFrom.Entity;
                if ((previousValue != value)
                    || (_removedFrom.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _removedFrom.Entity = null;
                        previousValue.SpoilRemovals.Remove(this);
                    }
                    _removedFrom.Entity = value;
                    if (value != null)
                    {
                        value.SpoilRemovals.Add(this);
                        _removedFromID = value.SpoilStorageLocationID;
                    }
                    else
                        _removedFromID = default(int);
                    SendPropertyChanged("RemovedFrom");
                }
            }
        }

        [Association(Name = "FinalDestination_SpoilRemoval", Storage = "_finalDestination", ThisKey = "FinalDestinationID", IsForeignKey = true)]
        public SpoilFinalProcessingLocation FinalDestination
        {
            get { return _finalDestination.Entity; }
            set
            {
                SpoilFinalProcessingLocation previousValue = _finalDestination.Entity;
                if ((previousValue != value)
                    || (_finalDestination.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _finalDestination.Entity = null;
                        previousValue.SpoilRemovals.Remove(this);
                    }
                    _finalDestination.Entity = value;
                    if (value != null)
                    {
                        value.SpoilRemovals.Add(this);
                        _finalDestinationID = value.SpoilFinalProcessingLocationID;
                    }
                    else
                        _finalDestinationID = default(int);
                    SendPropertyChanged("FinalDestination");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public SpoilRemoval()
        {
        }

        #endregion

        #region Private Methods

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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (DateRemoved == default(DateTime))
                        throw new DomainLogicException(
                            "Cannot save a SpoilRemoval record without a value for DateRemoved.");
                    if (Quantity <= 0)
                        throw new DomainLogicException(
                            "Cannot save a SpoilRemoval record with a Quantity of zero or less.");
                    if (RemovedFrom == null)
                        throw new DomainLogicException(
                            "Cannot save a SpoilRemoval record without specifying where it was removed from.");
                    if (FinalDestination == null)
                        throw new DomainLogicException(
                            "Cannot save a SpoilRemoval record without specifying where it was sent to.");
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
