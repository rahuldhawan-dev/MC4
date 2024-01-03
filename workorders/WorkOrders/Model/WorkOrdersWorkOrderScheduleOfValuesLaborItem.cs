using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkOrdersScheduleOfValues")]
    public class WorkOrderScheduleOfValue : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        
        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id, _workOrderID, _scheduleOfValueID;
        private EntityRef<WorkOrder> _workOrder;
        private EntityRef<ScheduleOfValue> _scheduleOfValue;
        private decimal _total, _laborUnitCost;
        private bool _isOvertime;
        private string _otherDescription;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged("Id");
                }
            }
        }
        
        [Column(Storage = "_total", DbType = "decimal")]
        public decimal Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    SendPropertyChanging();
                    _total = value;
                    SendPropertyChanged("Total");
                }
            }
        }

        [Column(Storage = "_laborUnitCost", DbType = "decimal")]
        public decimal LaborUnitCost
        {
            get { return _laborUnitCost; }
            set
            {
                if (_laborUnitCost != value)
                {
                    SendPropertyChanging();
                    _laborUnitCost = value;
                    SendPropertyChanged("LaborUnitCost");
                }
            }
        }

        [Column(Storage = "_isOvertime", DbType = "bit")]
        public bool IsOvertime
        {
            get { return _isOvertime; }
            set
            {
                if (_isOvertime != value)
                {
                    SendPropertyChanging();
                    _isOvertime = value;
                    SendPropertyChanged("IsOvertime");
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

        [Association(Name = "WorkOrder_ScheduleOfValue", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a WorkOrderScheduleOfValue record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.WorkOrdersScheduleOfValues.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrdersScheduleOfValues.Add(this);
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

        [Column(Storage = "_scheduleOfValueID", DbType = "Int NOT NULL")]
        public int ScheduleOfValueID
        {
            get { return _scheduleOfValueID; }
            set
            {
                if (_scheduleOfValue.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                SendPropertyChanging();
                _scheduleOfValueID = value;
                SendPropertyChanged("ScheduleOfValueID");
            }
        }

        [Association(Name= "ScheduleOfValue_WorkOrder", Storage = "_scheduleOfValue", ThisKey= "ScheduleOfValueID", IsForeignKey = true)]
        public ScheduleOfValue ScheduleOfValue
        {
            get { return _scheduleOfValue.Entity; }
            set
            {
                var previousValue = _scheduleOfValue.Entity;
                if ((previousValue != value)
                    || (_scheduleOfValue.HasLoadedOrAssignedValue == false))
                {
                    if (previousValue != null && value != null)
                        throw new DomainLogicException("Cannot change the ScheduleOfValue of a WorkOrderScheduleOfValue record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _scheduleOfValue.Entity = null;
                    }
                    _scheduleOfValue.Entity = value;
                    if (value != null)
                    {
                        _scheduleOfValueID = value.Id;
    }
                    else
                    {
                        _scheduleOfValueID = default(int);
                    }
                    SendPropertyChanged("WorkOrderScheduleOfValue");
                }
            }
        }

        [Column(Storage = "_otherDescription", DbType = "Varchar(50) NULL")]
        public string OtherDescription
        {
            get { return _otherDescription; }
            set
            {
                if (_otherDescription != value)
                {
                    SendPropertyChanging();
                    _otherDescription = value;
                    SendPropertyChanged("OtherDescription");
                }
            }
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

        #endregion

        #region Constructors

        public WorkOrderScheduleOfValue()
        {
            _workOrder = new EntityRef<WorkOrder>();
            _scheduleOfValue = new EntityRef<ScheduleOfValue>();
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}