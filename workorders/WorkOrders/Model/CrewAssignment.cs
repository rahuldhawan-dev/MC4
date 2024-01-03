using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using MMSINC.Exceptions;
using MapCall.Common.Utility;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.CrewAssignments")]
    public class CrewAssignment : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _crewAssignmentID,
            _crewID,
            _workOrderID,
            _priority;

        private int? _startedByID;

        private float? _employeesOnJob;

        private DateTime _assignedOn, _assignedFor;

        private DateTime? _dateStarted, _dateEnded;

        private EntityRef<Crew> _crew;
        private EntityRef<Employee> _startedBy;

        private EntityRef<WorkOrder> _workorder;
        private TimeSpan? _estimatedTimeToComplete;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_crewAssignmentID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int CrewAssignmentID
        {
            get { return _crewAssignmentID; }
            set
            {
                if (_crewAssignmentID != value)
                {
                    SendPropertyChanging();
                    _crewAssignmentID = value;
                    SendPropertyChanged("CrewAssignmentID");
                }
            }
        }

        [Column(Storage = "_crewID", DbType = "Int NOT NULL")]
        public int CrewID
        {
            get { return _crewID; }
            set
            {
                if (_crewID != value)
                {
                    if (_crew.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _crewID = value;
                SendPropertyChanged("CrewID");
            }
        }

        [Column(Storage = "_startedByID", DbType = "Int")]
        public int? StartedByID
        {
            get => _startedByID;
            set
            {
                if (_startedByID != value)
                {
                     if (_startedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _startedByID = value;
                SendPropertyChanged("StartedByID");
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
                    if (_workorder.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workOrderID = value;
                SendPropertyChanged("WorkOrderID");
            }
        }

        [Column(Storage = "_priority", DbType = "Int NOT NULL")]
        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    SendPropertyChanging();
                    _priority = value;
                    SendPropertyChanged("Priority");
                }
            }
        }

        /// <summary>
        /// The date the supervisor clicked the Date Started button in Scheduling
        /// </summary>
        [Column(Storage = "_assignedOn", DbType = "SmallDateTime NOT NULL")]
        public DateTime AssignedOn
        {
            get
            {
                if (_assignedOn == default(DateTime))
                    _assignedOn = DateTime.Now;
                return _assignedOn;
            }
            set
            {
                if (_assignedOn != value)
                {
                    SendPropertyChanging();
                    _assignedOn = value;
                    SendPropertyChanged("AssignedOn");
                }
            }
        }

        /// <summary>
        /// The date the crew is supposed to do the work
        /// </summary>
        [Column(Storage = "_assignedFor", DbType = "SmallDateTime NOT NULL")]
        public DateTime AssignedFor
        {
            get { return _assignedFor; }
            set
            {
                if (_assignedFor != value)
                {
                    SendPropertyChanging();
                    _assignedFor = value;
                    SendPropertyChanged("AssignedFor");
                }
            }
        }

        [Column(Storage = "_dateStarted", DbType = "SmallDateTime")]
        public DateTime? DateStarted
        {
            get { return _dateStarted; }
            set
            {
                if (_dateStarted != value)
                {
                    SendPropertyChanging();
                    _dateStarted = value;
                    SendPropertyChanged("DateStarted");
                }
            }
        }

        [Column(Storage = "_dateEnded", DbType = "SmallDateTime")]
        public DateTime? DateEnded
        {
            get { return _dateEnded; }
            set
            {
                if (_dateEnded != value)
                {
                    OnDateEndedChanging(value);
                    SendPropertyChanging();
                    _dateEnded = value;
                    SendPropertyChanged("DateEnded");
                }
            }
        }

        [Column(Storage = "_employeesOnJob", DbType = "Float")]
        public float? EmployeesOnJob
        {
            get { return _employeesOnJob; }
            set
            {
                if (_employeesOnJob != value)
                {
                    OnEmployeesOnJobChanging(value);
                    SendPropertyChanging();
                    _employeesOnJob = value;
                    SendPropertyChanged("EmployeesOnJob");
                }
            }
        }

        #endregion

        #region Logical Properties

        public TimeSpan EstimatedTimeToComplete
        {
            get
            {
                if (_estimatedTimeToComplete == null)
                    _estimatedTimeToComplete =
                        CalculateEstimatedTimeToComplete();
                return _estimatedTimeToComplete.Value;
            }
        }

        public TimeSpan? TimeToComplete
        {
            get
            {
                return (DateStarted == null || DateEnded == null)
                           ? null : DateEnded - DateStarted;
            }
        }

        public virtual float? TotalManHours
        {
            get
            {
                return (DateStarted == null || DateEnded == null)
                           ? null : TimeToComplete.Value.Hours * EmployeesOnJob;
            }
        }

        public virtual bool IsOpen
        {
            get { return HasStarted && DateEnded == null; }
        }

        public virtual bool HasStarted
        {
            get { return DateStarted != null; }
        }

        public virtual bool CanBeStarted
        {
            get
            {
                switch (WorkOrder.MarkoutRequirementID)
                {
                    //if we don't have markout or we have one and it's within the range
                    case (int)MarkoutRequirementEnum.Emergency:
                    case (int)MarkoutRequirementEnum.None:
                        return true;
                    case (int)MarkoutRequirementEnum.Routine:
                        return WorkOrder?.CurrentMarkout?.ReadyDate <= DateTime.Now && WorkOrder?.CurrentMarkout?.ExpirationDate >= DateTime.Now;
                    default:
                        throw new InvalidOperationException("No Markout Requirement was set.");
                }
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "Crew_CrewAssignment", Storage = "_crew", ThisKey = "CrewID", IsForeignKey = true)]
        public Crew Crew
        {
            get { return _crew.Entity; }
            set
            {
                var previousValue = _crew.Entity;
                if ((previousValue != value)
                    || (_crew.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _crew.Entity = null;
                        previousValue.CrewAssignments.Remove(this);
                    }
                    _crew.Entity = value;
                    if (value != null)
                    {
                        value.CrewAssignments.Add(this);
                        _crewID = value.CrewID;
                    }
                    else
                        _crewID = default(int);
                    SendPropertyChanged("Crew");
                }
            }
        }

        [Association(Name = "StartedBy_CrewAssignment", Storage = "_startedBy",
            ThisKey = "StartedByID", IsForeignKey = true)]
        public Employee StartedBy
        {
            get => _startedBy.Entity;
            set
            {
                var previousValue = _startedBy.Entity;
                if ((previousValue != value)
                    || (_startedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _startedBy.Entity = null;
                    }
                    _startedBy.Entity = value;
                    if (value != null)
                    {
                        _startedByID = value.EmployeeID;
                    }
                    else
                        _startedByID = default(int);
                    SendPropertyChanged("Crew");
                }
                
            }
        }

        [Association(Name = "WorkOrder_CrewAssignments", Storage = "_workorder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public virtual WorkOrder WorkOrder
        {
            get { return _workorder.Entity; }
            set
            {
                WorkOrder previousValue = _workorder.Entity;
                if ((previousValue != value)
                    || (_workorder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workorder.Entity = null;
                        previousValue.CrewAssignments.Remove(this);
                    }
                    _workorder.Entity = value;
                    if (value != null)
                    {
                        value.CrewAssignments.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                        _workOrderID = default(int);
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public CrewAssignment()
        {
        }

        #endregion

        #region Private Methods

        private void OnDateEndedChanging(DateTime? value)
        {
            if (DateStarted == null)
                throw new DomainLogicException(
                    "Cannot set DateEnded without a value for DateStarted.");
        }

        private void OnEmployeesOnJobChanging(float? value)
        {
            if (value == null)
                throw new DomainLogicException(
                    "Cannot set EmployeesOnJob to null.");
            if (value < 1)
                throw new DomainLogicException(
                    "Cannot set EmployeesOnJob to a value less than one.");
            if (value % 0.5f != 0)
                throw new DomainLogicException(
                    "Cannot set a decimal value for EmployeesOnJob other than half.");
            if (DateEnded == null)
                throw new DomainLogicException(
                    "Cannot set a value for EmployeesOnJob when DateEnded is unset.");
        }

        private void ValidateNecessaryFields()
        {
            if (AssignedFor == default(DateTime))
                throw new DomainLogicException("Cannot save without a date of assignment.");
            if (Crew == null)
                throw new DomainLogicException("Cannot save without a Crew.");
            if (WorkOrder == null)
                throw new DomainLogicException("Cannot save without a WorkOrder.");
            if (Priority == default(int))
                throw new DomainLogicException("Cannot save without a priority.");
        }

        private void ValidateAgainstWorkOrder()
        {
            if (WorkOrder.MarkoutRequirement.RequirementEnum == MarkoutRequirementEnum.Routine && WorkOrder.CurrentMarkout != null)
            {
                if (WorkOrder.Markouts.All(mo => mo.ReadyDate > AssignedFor))
                {
                    throw new DomainLogicException(
                        "Cannot create a CrewAssignment for a date prior to any Markouts' ReadyDate.");
                }
                if (WorkOrder.Markouts.All(mo => mo.ExpirationDate < AssignedFor))
                {
                    throw new DomainLogicException(
                        "Cannot create a CrewAssignment for a date after any Markout will expire.");
                }
            }
            if (WorkOrder.OperatingCenterID != Crew.OperatingCenterID && Crew.ContractorID == null)
            {
                throw new DomainLogicException(
                    "Cannot crew a CrewAssignment when the Work Order's Operating Center does not match the Crew's");
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

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                    ValidateNecessaryFields();
                    ValidateAgainstWorkOrder();
                    break;
                case ChangeAction.Update:
                    if (DateEnded != null && EmployeesOnJob == null)
                        throw new DomainLogicException(
                            "Cannot end a CrewAssignment without listing the number of employees on the job.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private TimeSpan CalculateEstimatedTimeToComplete()
        {
            if (WorkOrder == null || WorkOrder.WorkDescription == null) return new TimeSpan(0);

            return TimeSpan.FromHours((double)WorkOrder.WorkDescription.TimeToComplete);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public static class CrewAssignmentEntitySetExtensions
    {
        #region Extension Methods

        public static DateTime GetMaxDate(this IEnumerable<CrewAssignment> set)
        {
            return (from a in set select a.AssignedFor).Max();
        }

        public static CrewAssignment GetCurrent(this IEnumerable<CrewAssignment> set)
        {
            if (set == null || set.Count() < 1)
                return null;

            return
                (from a in set
                 where a.AssignedFor == set.GetMaxDate()
                 orderby a.AssignedOn descending 
                 select a).FirstOrDefault();
        }

        public static Crew GetCurrentCrew(this IEnumerable<CrewAssignment> set)
        {
            if (set == null || set.Count() <= 0)
                return null;

            return set.GetCurrent().Crew;
        }

        public static IEnumerable<CrewAssignment> GetByDate(this IEnumerable<CrewAssignment> set, DateTime date)
        {
            foreach (var assignment in set)
            {
                if (assignment.AssignedFor.Date == date.Date)
                    yield return assignment;
            }
        }

        public static decimal GetTimeToComplete(this IEnumerable<CrewAssignment> set)
        {
            return
                (from a in set select a.WorkOrder.WorkDescription.TimeToComplete)
                    .Sum();
        }

        #endregion
    }
}
