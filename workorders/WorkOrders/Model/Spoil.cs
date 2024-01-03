using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Spoils")]
    public class Spoil : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _spoilID,
                    _workOrderID,
                    _spoilStorageLocationID;

        private decimal _quantity;

        private EntityRef<WorkOrder> _workOrder;
        private EntityRef<SpoilStorageLocation> _spoilStorageLocation;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_spoilID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int SpoilID
        {
            get { return _spoilID; }
            set
            {
                if (_spoilID != value)
                {
                    SendPropertyChanging();
                    _spoilID = value;
                    SendPropertyChanged("SpoilID");
                }
            }
        }

        [Column(Storage = "_workOrderID", DbType = "Int NOT NULL")]
        public int WorkOrderID
        {
            get { return _workOrderID; }
            set
            {
                if (_workOrderID != value)
                {
                    if (_workOrder.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workOrderID = value;
                SendPropertyChanged("WorkOrderID");
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

        [Column(Storage = "_spoilStorageLocationID", DbType = "Int NOT NULL")]
        public int SpoilStorageLocationID
        {
            get { return _spoilStorageLocationID; }
            set
            {
                if (_spoilStorageLocationID != value)
                {
                    if (_spoilStorageLocation.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _spoilStorageLocationID = value;
                SendPropertyChanged("SpoilStorageLocationID");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "WorkOrder_Spoil", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                WorkOrder previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.Spoils.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.Spoils.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                        _workOrderID = default(int);
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        [Association(Name = "SpoilStorageLocation_Spoil", Storage = "_spoilStorageLocation", ThisKey = "SpoilStorageLocationID", IsForeignKey = true)]
        public SpoilStorageLocation SpoilStorageLocation
        {
            get { return _spoilStorageLocation.Entity; }
            set
            {
                var previousValue = _spoilStorageLocation.Entity;
                if ((previousValue != value)
                    || (_spoilStorageLocation.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _spoilStorageLocation.Entity = null;
                        previousValue.Spoils.Remove(this);
                    }
                    _spoilStorageLocation.Entity = value;
                    if (value != null)
                    {
                        value.Spoils.Add(this);
                        _spoilStorageLocationID = value.SpoilStorageLocationID;
                    }
                    else
                        _spoilStorageLocationID = default(int);
                    SendPropertyChanged("SpoilStorageLocation");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public Spoil()
        {
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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (WorkOrder == null)
                        throw new DomainLogicException("Cannot save a Spoil record without a WorkOrder.");
                    if (SpoilStorageLocation == null)
                        throw new DomainLogicException("Cannot save a Spoil record without a SpoilStorageLocation.");
                    if (Quantity <= 0)
                        throw new DomainLogicException("Cannot save a Spoil record with a Quantity equal to or less than zero.");
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
