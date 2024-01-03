using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Crews")]
    public class Crew : INotifyPropertyChanging, INotifyPropertyChanged, IComparable<Crew>, IComparable
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 15;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _crewID;
        private int? _operatingCenterID, _contractorID;
        private string _description;
        private decimal _availability;
        private bool _active;

        private EntityRef<OperatingCenter> _operatingCenter;

        private readonly EntitySet<CrewAssignment> _crewAssignments;

        #endregion

        #region Properties

        #region Logical Properties

        public string FullDescription
        {
            get { return OperatingCenter.OpCntr + " " + OperatingCenter.OpCntrName + " - " + Description; }
        }

        #endregion

        #region Table Column Properties

        [Column(Storage = "_crewID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int CrewID
        {
            get { return _crewID; }
            set
            {
                if (_crewID != value)
                {
                    SendPropertyChanging();
                    _crewID = value;
                    SendPropertyChanged("crewID");
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

        [Column(Storage = "_description", DbType = "VarChar(15) NOT NULL")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new StringTooLongException("Description", MAX_DESCRIPTION_LENGTH);
                if (_description != value)
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
        public int? OperatingCenterID
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

        [Column(Storage = "_availability", DbType = "Decimal(18, 2) NOT NULL")]
        public decimal Availability
        {
            get { return _availability; }
            set
            {
                if (_availability != value)
                {
                    SendPropertyChanging();
                    _availability = value;
                    SendPropertyChanged("Availability");
                }
            }
        }

        [Column(Storage = "_contractorID", DbType = "Int Null")]
        public int? ContractorID
        {
            get { return _contractorID; }
            set
            {
                //if (_contractorID != value)
                //{
                //    if (_contractor.HasLoadedOrAssignedValue)
                //        throw new ForeignKeyReferenceAlreadyHasValueException();
                //}
                SendPropertyChanging();
                _contractorID = value;
                SendPropertyChanged("ContractorID");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "OperatingCenter_Crew", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.Crews.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.Crews.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "Crew_CrewAssignment", Storage = "_crewAssignments", OtherKey = "CrewID")]
        public EntitySet<CrewAssignment> CrewAssignments
        {
            get { return _crewAssignments; }
            set { _crewAssignments.Assign(value); }
        }

        #endregion

        #endregion

        #region Constructors

        public Crew()
        {
            _operatingCenter = default(EntityRef<OperatingCenter>);
            _crewAssignments = new EntitySet<CrewAssignment>(attach_CrewAssignments, detach_CrewAssignments);
        }

        #endregion

        #region Private Methods

        private int GetMaxPriorityByDate(DateTime date)
        {
            /*
            return (from a in CrewAssignments
                    where a.AssignedFor.Date == date.Date
                    select a.Priority).MaxOrDefault();
            */
            int max = 0;
            var assignments =
                (from a in CrewAssignments
                 where a.AssignedFor.Date == date.Date
                 select a);
            foreach (var assignment in assignments)
            {
                if (max < assignment.Priority)
                    max = assignment.Priority;
            }
            return max;
        }

        private void DeterminePriority(CrewAssignment entity)
        {
            // TODO: This should probably get moved to another class
            if (entity.Priority != default(int)) return;
            var priority = 1 + GetMaxPriorityByDate(entity.AssignedFor);
            entity.Priority = priority;
        }

        private void attach_CrewAssignments(CrewAssignment entity)
        {
            SendPropertyChanging();
            DeterminePriority(entity);
            entity.Crew = this;
        }

        private void detach_CrewAssignments(CrewAssignment entity)
        {
            SendPropertyChanging();
            entity.Crew = null;
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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Cannot save a Crew without a Description.");
                    if (OperatingCenter == null)
                        throw new DomainLogicException("Cannot save a Crew without an Operating Center.");
                    if (Availability == default(decimal))
                        throw new DomainLogicException("Cannot save a Crew without their shift availability.");
                    break;
            }
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public int CompareTo(object other)
        {
            var otherCrew = other as Crew;
            return otherCrew == null ? -1 : CompareTo(otherCrew);
        }

        public int CompareTo(Crew other)
        {
            return
                Description.CompareTo(other == null ? null : other.Description);
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
