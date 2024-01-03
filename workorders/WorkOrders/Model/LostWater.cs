using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.LostWater")]
    public class LostWater : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MIN_GALLONS_VALUE = 1;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _gallons, _lostWaterID, _workOrderID;

        private EntityRef<WorkOrder> _workOrder;

        #endregion

        #region Properties

        [Column(Storage = "_gallons", DbType = "Int NOT NULL")]
        public int Gallons
        {
            get { return _gallons; }
            set
            {
                if (value < MIN_GALLONS_VALUE)
                    throw new DomainLogicException(
                        String.Format("Cannot set Gallons to a value less than {0}.",
                            MIN_GALLONS_VALUE));
                if (_gallons != value)
                {
                    SendPropertyChanging();
                    _gallons = value;
                    SendPropertyChanged("Gallons");
                }
            }
        }

        [Association(Name = "WorkOrder_LostWater", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                var previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    if (previousValue != null && value != null)
                        throw new DomainLogicException("Cannot change the WorkOrder of a LostWater record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.LostWaters.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.LostWaters.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                    {
                        _workOrderID = default(int);
                    }
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        [Column(Storage = "_lostWaterID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int LostWaterID
        {
            get { return _lostWaterID; }
            set
            {
                if (_lostWaterID != value)
                {
                    SendPropertyChanging();
                    _lostWaterID = value;
                    SendPropertyChanged("LostWaterID");
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
                    SendPropertyChanging();
                    _workOrderID = value;
                    SendPropertyChanged("WorkOrderID");
                }
            }
        }

        #endregion

        #region Constructors

        public LostWater()
        {
            _workOrder = default(EntityRef<WorkOrder>);

        }

        #endregion

        #region Private Methods

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    ValidateCreationInfo();
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void ValidateCreationInfo()
        {
            if (Gallons < MIN_GALLONS_VALUE)
                throw new DomainLogicException(
                    "Cannot save lost water with less than one gallon.");
            if (WorkOrder == null)
                throw new DomainLogicException(
                    "Cannot save a LostWater object without a WorkOrder.");
        }

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

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
