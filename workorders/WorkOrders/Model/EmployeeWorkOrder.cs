using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.EmployeeWorkOrders")]
    public class EmployeeWorkOrder : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private bool? _workCompleted;

        private int _employeeWorkOrderID, _workOrderID;

        private int? _assignedToID, _approvedByID;

        private DateTime? _dateAssigned,
                          _timeArrivedOnJob,
                          _timeLeftJob,
                          _dateTimeArrivedOnJobSet,
                          _dateTimeLeftJobSet;

        private short _numberOfEmployees;

        private string _jobNotes;

        private short? _totalManHours;

        #pragma warning disable 649

        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<Employee> _assignedTo;
        private EntityRef<Employee> _approvedBy;

#pragma warning restore 649

        #endregion

        #region Properties

        [Column(Storage = "_employeeWorkOrderID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int EmployeeWorkOrderID
        {
            get { return _employeeWorkOrderID; }
            set
            {
                if ((_employeeWorkOrderID != value))
                {
                    SendPropertyChanging();
                    _employeeWorkOrderID = value;
                    SendPropertyChanged("EmployeeWorkOrderID");
                }
            }
        }

        [Column(Storage = "_dateAssigned", DbType = "SmallDateTime")]
        public DateTime? DateAssigned
        {
            get { return _dateAssigned; }
            set
            {
                if ((_dateAssigned != value))
                {
                    SendPropertyChanging();
                    _dateAssigned = value;
                    SendPropertyChanged("DateAssigned");
                }
            }
        }

        [Column(Storage = "_numberOfEmployees", DbType = "SmallInt NOT NULL")]
        public short NumberOfEmployees
        {
            get { return _numberOfEmployees; }
            set
            {
                if (_numberOfEmployees != value)
                {
                    SendPropertyChanging();
                    _numberOfEmployees = value;
                    SendPropertyChanged("NumberOfEmployees");
                    OnNumberOfEmployeesChanged();
                }
            }
        }

        [Column(Storage = "_timeArrivedOnJob", DbType = "SmallDateTime")]
        public DateTime? TimeArrivedOnJob
        {
            get { return _timeArrivedOnJob; }
            set
            {
                if (_timeArrivedOnJob != value)
                {
                    OnTimeArrivedOnJobChanging(value);
                    SendPropertyChanging();
                    _timeArrivedOnJob = value;
                    SendPropertyChanged("TimeArrivedOnJob");
                }
            }
        }

        [Column(Storage = "_jobNotes", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string JobNotes
        {
            get { return _jobNotes; }
            set
            {
                if (_jobNotes != value)
                {
                    SendPropertyChanging();
                    _jobNotes = value;
                    SendPropertyChanged("JobNotes");
                }
            }
        }

        [Association(Name = "WorkOrder_EmployeeWorkOrder", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a given EmployeeWorkOrder once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.EmployeeWorkOrders.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.EmployeeWorkOrders.Add(this);
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

        /*
        [Association(Name = "Employee_EmployeeWorkOrder", Storage = "_approvedBy", ThisKey = "ApprovedByID", IsForeignKey = true)]
        public Employee ApprovedBy
        {
            get { return _approvedBy.Entity; }
            set
            {
                var previousValue = _approvedBy.Entity;
                if ((previousValue != value)
                    || (_approvedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _approvedBy.Entity = null;
                        previousValue.ApprovedWorkOrders.Remove(this);
                    }
                    _approvedBy.Entity = value;
                    if (value != null)
                    {
                        value.ApprovedWorkOrders.Add(this);
                        _approvedByID = value.EmployeeID;
                    }
                    else
                    {
                        _approvedByID = default(int?);
                    }
                    SendPropertyChanged("ApprovedBy");
                }
            }
        }
        */

        [Association(Name = "Employee_EmployeeWorkOrder1", Storage = "_assignedTo", ThisKey = "AssignedToID", IsForeignKey = true)]
        public Employee AssignedTo
        {
            get { return _assignedTo.Entity; }
            set
            {
                var previousValue = _assignedTo.Entity;
                if ((previousValue != value)
                    || (_assignedTo.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _assignedTo.Entity = null;
                        previousValue.AssignedWorkOrders.Remove(this);
                        // auto-decrement if we're just un-assigning
                        if (value == null)
                            NumberOfEmployees--;
                    }
                    _assignedTo.Entity = value;
                    if (value != null)
                    {
                        value.AssignedWorkOrders.Add(this);
                        _assignedToID = value.EmployeeID;
                        // auto-increment if we're assigning for the first time
                        if (previousValue == null)
                            NumberOfEmployees++;
                    }
                    else
                    {
                        _assignedToID = default(int?);
                    }
                    SendPropertyChanged("AssignedTo");
                    if (DateAssigned == null || DateAssigned == DateTime.MinValue)
                        DateAssigned = DateTime.Now;
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
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _workOrderID = value;
                    SendPropertyChanged("WorkOrderID");
                }
            }
        }

        [Column(Name = "ApprovedBy", Storage = "_approvedByID", DbType = "Int")]
        public int? ApprovedByID
        {
            get { return _approvedByID; }
            set
            {
                if ((_approvedByID != value))
                {
                    SendPropertyChanging();
                    _approvedByID = value;
                    SendPropertyChanged("ApprovedByID");
                }
            }
        }

        [Column(Name = "AssignedTo", Storage = "_assignedToID", DbType = "Int")]
        public int? AssignedToID
        {
            get { return _assignedToID; }
            set
            {
                if (_assignedToID != value)
                {
                    SendPropertyChanging();
                    _assignedToID = value;
                    SendPropertyChanged("AssignedToID");
                }
            }
        }

        [Column(Storage = "_timeLeftJob", DbType = "SmallDateTime")]
        public DateTime? TimeLeftJob
        {
            get { return _timeLeftJob; }
            set
            {
                if (_timeLeftJob != value)
                {
                    OnTimeLeftJobChanging(value);
                    SendPropertyChanging();
                    _timeLeftJob = value;
                    SendPropertyChanged("TimeLeftJob");
                    OnTimeLeftJobChanged();
                }
            }
        }

        [Column(Storage = "_dateTimeArrivedOnJobSet", DbType = "SmallDateTime")]
        public DateTime? DateTimeArrivedOnJobSet
        {
            get { return _dateTimeArrivedOnJobSet; }
            set
            {
                if (_dateTimeArrivedOnJobSet != value)
                {
                    SendPropertyChanging();
                    _dateTimeArrivedOnJobSet = value;
                    SendPropertyChanged("DateTimeArrivedOnJobSet");
                }
            }
        }

        [Column(Storage = "_dateTimeLeftJobSet", DbType = "SmallDateTime")]
        public DateTime? DateTimeLeftJobSet
        {
            get { return _dateTimeLeftJobSet; }
            set
            {
                if (_dateTimeLeftJobSet != value)
                {
                    SendPropertyChanging();
                    _dateTimeLeftJobSet = value;
                    SendPropertyChanged("DateTimeLeftJobSet");
                }
            }
        }

        [Column(Storage = "_totalManHours", DbType = "SmallInt")]
        public short? TotalManHours
        {
            get { return _totalManHours; }
            set
            {
                if (_totalManHours != value)
                {
                    SendPropertyChanging();
                    _totalManHours = value;
                    SendPropertyChanged("TotalManHours");
                }
            }
        }

        [Column(Storage = "_workCompleted", DbType = "Bit")]
        public bool? WorkCompleted
        {
            get { return _workCompleted; }
            set
            {
                if (_workCompleted != value)
                {
                    OnWorkCompletedChanging(value);
                    SendPropertyChanging();
                    _workCompleted = value;
                    SendPropertyChanged("WorkCompleted");
                }
            }
        }

        #endregion

        #region Extension Methods

        private void OnWorkCompletedChanging(bool? value)
        {
            if (value.Value)
                OnComplete();
        }

        #pragma warning disable 168

        private void OnTimeArrivedOnJobChanging(DateTime? value)
        {
            DateTimeArrivedOnJobSet = DateTime.Now;
        }

        #pragma warning restore 168

        private void OnTimeLeftJobChanging(DateTime? value)
        {
            if (TimeArrivedOnJob == null)
                throw new DomainLogicException("Cannot set a value for TimeJobLeft without setting TimeJobLeft");
            if (value < TimeArrivedOnJob.Value)
                throw new DomainLogicException("Cannot set a value for TimeJobCompleted that lies chronologically before TimeArrivedOnJob");
            DateTimeLeftJobSet = DateTime.Now;
        }

        private void OnTimeLeftJobChanged()
        {
            SetTotalManHours();
        }

        private void OnNumberOfEmployeesChanged()
        {
            SetTotalManHours();
        }

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

        private void ValidateCreationInfo()
        {
            if (WorkOrder == null)
                throw new DomainLogicException(
                    "Cannot save an EmployeeWorkOrder object without a WorkOrder.");
        }

        #endregion

        #region Private Methods

        private void OnComplete()
        {
            if (AssignedTo == null)
                throw new DomainLogicException("Cannot complete an EmployeeWorkOrder that has not been assigned");
            if (DateAssigned == null || DateAssigned == DateTime.MinValue)
                throw new DomainLogicException("Cannot complete an EmployeeWorkOrder with no DateAssigned");
//            if (ApprovedBy == null)
//                throw new DomainLogicException("Cannot complete an EmployeeWorkOrder that has not been approved");
            WorkOrder.DateCompleted = TimeLeftJob;
        }

        private void SetTotalManHours()
        {
            short hours;
            if (TimeArrivedOnJob == null || TimeLeftJob == null)
                hours = 0;
            else
                hours =
                    (short)
                    TimeLeftJob.Value.Subtract(TimeArrivedOnJob.Value).Hours;
            TotalManHours = (short)(hours * NumberOfEmployees);
        }

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
    }
}
