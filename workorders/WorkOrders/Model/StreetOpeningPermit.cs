using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.StreetOpeningPermits")]
    public class StreetOpeningPermit : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _streetOpeningPermitID, _workOrderID;
        private int? _permitId;
        private bool? _hasMetDrawingRequirement, _isPaidFor;

        private string _streetOpeningPermitNumber, _notes;

        private DateTime _dateRequested;
        private DateTime? _expirationDate, _dateIssued;

        private EntityRef<WorkOrder> _workOrder;

        #endregion

        #region Properties

        [Column(Storage = "_streetOpeningPermitID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int StreetOpeningPermitID
        {
            get { return _streetOpeningPermitID; }
            set
            {
                if (_streetOpeningPermitID != value)
                {
                    SendPropertyChanging();
                    _streetOpeningPermitID = value;
                    SendPropertyChanged("StreetOpeningPermitID");
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

        [Column(Storage = "_streetOpeningPermitNumber", DbType = "varchar(25) NOT NULL")]
        public string StreetOpeningPermitNumber
        {
            get { return _streetOpeningPermitNumber; }
            set
            {
                if (_streetOpeningPermitNumber != value)
                {
                    SendPropertyChanging();
                    _streetOpeningPermitNumber = value;
                    SendPropertyChanged("StreetOpeningPermitNumber");
                }
            }
        }

        [Association(Name = "WorkOrder_StreetOpeningPermit", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a StreetOpeningPermit record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.StreetOpeningPermits.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.StreetOpeningPermits.Add(this);
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

        [Column(Storage = "_dateRequested", DbType = "SmallDateTime NOT NULL")]
        public DateTime DateRequested
        {
            get { return _dateRequested; }
            set
            {
                if (_dateRequested != value)
                {
                    SendPropertyChanging();
                    _dateRequested = value;
                    SendPropertyChanged("DateRequested");
                }
            }
        }
        
        [Column(Storage = "_expirationDate", DbType = "SmallDateTime NULL")]
        public DateTime? ExpirationDate
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

        [Column(Storage = "_dateIssued", DbType = "SmallDateTime NULL")]
        public DateTime? DateIssued
        {
            get { return _dateIssued; }
            set
            {
                if (_dateIssued != value)
                {
                    SendPropertyChanging();
                    _dateIssued = value;
                    SendPropertyChanged("DateIssued");
                }
            }
        }

        [Column(Storage = "_notes", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    SendPropertyChanging();
                    _notes = value;
                    SendPropertyChanged("Notes");
                }
            }
        }

        public bool IsExpired
        {
            get { return DateTime.Now > ExpirationDate; }
        }

        [Column(Storage = "_permitId", DbType = "Int NULL")]
        public int? PermitId
        {
            get { return _permitId; }
            set
            {
                if (_permitId != value)
                {
                    SendPropertyChanging();
                    _permitId = value;
                    SendPropertyChanged("PermitID");
                }
            }
        }

        [Column(Storage="_isPaidFor", DbType = "Bit")]
        public bool? IsPaidFor
        {
            get { return _isPaidFor; }
            set
            {
                if (_isPaidFor != value)
                {
                    SendPropertyChanging();
                    _isPaidFor = value;
                    SendPropertyChanged("IsPaidFor");
                }
            }
        }

        [Column(Storage = "_hasMetDrawingRequirement", DbType = "Bit")]
        public bool? HasMetDrawingRequirement
        {
            get { return _hasMetDrawingRequirement; }
            set
            {
                if (_hasMetDrawingRequirement != value)
                {
                    SendPropertyChanging();
                    _hasMetDrawingRequirement = value;
                    SendPropertyChanged("HasMetDrawingRequirement");
                }
            }
        }

        #endregion

        #region Constructors

        public StreetOpeningPermit()
        {
        }

        #endregion

        #region Private Methods
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
                    "Cannot save a StreetOpeningPermit without a WorkOrder.");
            if(DateRequested == DateTime.MinValue )
            {
                throw new DomainLogicException("Cannot save a StreetOpeningPErmit without a DateRequested");
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

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
