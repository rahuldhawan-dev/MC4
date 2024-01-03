using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Markouts")]
    public class Markout : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_MARKOUTNUMBER_LENGTH = 20;
        private const short MAX_STREETOPENINGNUMBER_LENGTH = 15;

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #endregion

        #region Private Members

        private int _markoutID,
                    _workOrderID;

        private int? _markoutTypeID, _creatorID;

        protected DateTime _dateOfRequest,
                           _readyDate,
                           _expirationDate;

        private string _markoutNumber, _note;

        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<MarkoutType> _markoutType;

        #endregion

        #region Properties

        [Column(Storage = "_markoutNumber", DbType = "VarChar(20) NOT NULL", CanBeNull = false)]
        public string MarkoutNumber
        {
            get { return _markoutNumber; }
            set
            {
                if (value != null && value.Length > MAX_MARKOUTNUMBER_LENGTH)
                    throw new StringTooLongException("MarkoutNumber", MAX_MARKOUTNUMBER_LENGTH);
                if (_markoutNumber != value)
                {
                    SendPropertyChanging();
                    _markoutNumber = value;
                    SendPropertyChanged("MarkoutNumber");
                }
            }
        }

        [Column(Storage = "_note", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    SendPropertyChanging();
                    _note = value;
                    SendPropertyChanged("Note");
                }
            }
        }

        [Column(Storage = "_markoutID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MarkoutID
        {
            get { return _markoutID; }
            set
            {
                if (_markoutID != value)
                {
                    SendPropertyChanging();
                    _markoutID = value;
                    SendPropertyChanged("MarkoutID");
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

        [Column(Storage = "_markoutTypeID" , DbType = "Int")]
        public int? MarkoutTypeID
        {
            get { return _markoutTypeID; }
            set
            {
                if (_markoutTypeID != value)
                {
                    SendPropertyChanging();
                    _markoutTypeID = value;
                    SendPropertyChanged("MarkoutTypeID");
                }
            }
        }

        [Column(Storage = "_dateOfRequest", DbType = "SmallDateTime NOT NULL")]
        public DateTime DateOfRequest
        {
            get { return _dateOfRequest; }
            set
            {
                if (_dateOfRequest != value)
                {
                    SendPropertyChanging();
                    _dateOfRequest = value;
                    SendPropertyChanged("DateOfRequest");
                }
            }
        }

        [Column(Storage = "_readyDate", DbType = "SmallDateTime NOT NULL")]
        public DateTime ReadyDate
        {
            get { return _readyDate; } 
            set
            {
                if (_readyDate != value)
                {
                    SendPropertyChanging();
                    _readyDate = value;
                    SendPropertyChanged("ReadyDate");
                }
            }
        }

        [Column(Storage = "_expirationDate", DbType = "SmallDateTime NOT NULL")]
        public DateTime ExpirationDate
        {
            get { return _expirationDate; } 
            set
            {
                if (_expirationDate != value)
                {
                    SendPropertyChanging();
                    _expirationDate = value;
                    SendPropertyChanged("ExpirationDate");
                }
            }
        }

        public bool IsExpired
        {
            get { return DateTime.Now > ExpirationDate; }
        }

        [Column(Storage = "_creatorID", DbType = "Int")]
        public int? CreatorID
        {
            get { return _creatorID; }
            set
            {
                if (_creatorID != value)
                {
                    SendPropertyChanging();
                    _creatorID = value;
                    SendPropertyChanged("CreatorID");
                }
            }
        }

        #endregion

        #region Constructors

        public Markout()
        {
            _workOrder = default(EntityRef<WorkOrder>);
        }

        #endregion

        #region Private Methods

        private void SetReadyAndCreationDates()
        {
            // If the operating center allows markouts to be edited, lets get out of here.
            if (WorkOrder.OperatingCenter.MarkoutsEditable)
            {
                // lets make sure we set these to the default if they haven't been entered first.
                //if (ReadyDate == DateTime.MinValue)
                //    SetReadyDate();
                //if (ExpirationDate == DateTime.MinValue)
                //    SetExpirationDate();

                //ok now we can get out
                return;
            }

            SetReadyDate();
            SetExpirationDate();
        }

        private void SetReadyDate()
        {
            ReadyDate =
                WorkOrdersWorkDayEngine.GetReadyDate(DateOfRequest,
                    WorkOrder.MarkoutRequirement.RequirementEnum);
        }

        private void SetExpirationDate()
        {
            if (WorkOrder.CrewAssignments.Count > 0)
            {
                var tmpExpDate =
                    WorkOrdersWorkDayEngine.GetExpirationDate(DateOfRequest,
                        WorkOrder.MarkoutRequirement.RequirementEnum).EndOfDay();
                // DateStarted version:
                var workStarted = (from ca in WorkOrder.CrewAssignments
                                      where
                                      ca.DateStarted >= ReadyDate &&
                                      ca.DateStarted <= tmpExpDate
                                      select ca).Count() > 0;
                // AssignedFor version:
                //var workStarted = (from ca in WorkOrder.CrewAssignments
                //                   where
                //                       ca.AssignedFor >= ReadyDate &&
                //                       ca.AssignedFor <= tmpExpDate
                //                   select ca).Count() > 0;
                ExpirationDate =
                    WorkOrdersWorkDayEngine.GetExpirationDate(DateOfRequest,
                        WorkOrder.MarkoutRequirement.RequirementEnum, workStarted);
            }
            else
                ExpirationDate =
                    WorkOrdersWorkDayEngine.GetExpirationDate(DateOfRequest,
                        WorkOrder.MarkoutRequirement.RequirementEnum);
        }

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    // NOTE: Change this if DateOfRequest ever becomes nullable
                    if (DateOfRequest == default(DateTime))
                        DateOfRequest = DateTime.Now;
                    if (MarkoutNumber == null)
                        throw new DomainLogicException(
                            "Cannot save a Markout without markout number.");
                    if (WorkOrder == null)
                        throw new DomainLogicException(
                            "Cannot save a Markout with a WorkOrder.");
                    SetReadyAndCreationDates();
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

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

        #region Associations

        [Association(Name = "WorkOrder_Markout", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a Markout record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.Markouts.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.Markouts.Add(this);
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

        [Association(Name = "MarkoutType_Markout", Storage = "_markoutType", ThisKey = "MarkoutTypeID", IsForeignKey = true)]
        public MarkoutType MarkoutType
        {
            get { return _markoutType.Entity; }
            set
            {
                var previousValue = _markoutType.Entity;
                if ((previousValue != value) 
                    || (_markoutType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _markoutType.Entity = null;
                        previousValue.Markouts.Remove(this);
                    }
                    _markoutType.Entity = value;
                    if (value != null)
                    {
                        value.Markouts.Add(this);
                        _markoutTypeID = value.MarkoutTypeID;
                    }
                    else
                        _markoutTypeID = default(int);
                    SendPropertyChanged("MarkoutType");
                }
            }
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
