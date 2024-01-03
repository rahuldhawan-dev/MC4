using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.OrcomOrderCompletions")]
    public class OrcomOrderCompletion : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _orcomOrderCompletionID;
        private int _workOrderID;
        private int _completedByID;
        private DateTime _completedOn;

        private EntityRef<Employee> _completedBy;
        private EntityRef<WorkOrder> _workOrder;

        #endregion

        #region Properties

        #region Columns
        [Column(Storage = "_orcomOrderCompletionID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int OrcomOrderCompletionID
        {
            get { return _orcomOrderCompletionID; }
            set
            {
                if (_orcomOrderCompletionID != value)
                {
                    SendPropertyChanging();
                    _orcomOrderCompletionID = value;
                    SendPropertyChanged("OrcomOrderCompletionID");
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

        [Column(Storage = "_completedByID", DbType = "Int NOT NULL")]
        public int CompletedByID
        {
            get { return _completedByID; }
            set
            {
                if (_completedByID != value)
                {
                    if (_completedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _completedByID = value;
                SendPropertyChanged("CompletedByID");
            }
        }

        [Column(Storage = "_completedOn", DbType = "Datetime NOT NULL")]
        public DateTime CompletedOn
        {
            get { return _completedOn; }
            set
            {
                SendPropertyChanging();
                _completedOn = value;
                SendPropertyChanged("CompletedOn");
            }
        }

        #endregion

        #region Associations

        [Association(Name = "WorkOrder_OrcomOrderCompletion", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a OrcomOrderCompletion record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.OrcomOrderCompletions.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.OrcomOrderCompletions.Add(this);
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

        [Association(Name="Employee_OrcomOrderCompletion", Storage = "_completedBy", ThisKey = "CompletedByID", IsForeignKey = true)]
        public Employee CompletedBy
        {
            get { return _completedBy.Entity; }
            set
            {
                var previousValue = _completedBy.Entity;
                if ((previousValue != value)
                    || (_completedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    _completedBy.Entity = value;
                    SendPropertyChanged("CompletedBy");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public OrcomOrderCompletion()
        {
            _workOrder = default(EntityRef<WorkOrder>);
            _completedBy = default(EntityRef<Employee>);
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

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
