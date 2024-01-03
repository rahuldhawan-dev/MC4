using System; 
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Requisitions")]
    public class Requisition
    {
        #region Constants

        private const short MAX_REQUISITION_NUMBER_LENGTH = 50;
    
        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion
        
        #region Private Members

        private int _requisitionID, _requisitionTypeID, _workOrderID, _creatorID;
        private string _sapRequisitionNumber;
        private DateTime _createdOn;

        private EntityRef<RequisitionType> _requisitionType;
        private EntityRef<WorkOrder> _workOrder;
        private EntityRef<Employee> _createdBy;

        #endregion

        #region Properties

        [Column(Storage = "_requisitionID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RequisitionID
        {
            get { return _requisitionID; }
            set
            {
                if (_requisitionID != value)
                {
                    SendPropertyChanging();
                    _requisitionID = value;
                    SendPropertyChanged("RequisitionID");
                }
            }
        }

        [Column(Storage = "_sapRequisitionNumber", DbType = "VarChar(50) NOT NULL")]
        public string SAPRequisitionNumber
        {
            get { return _sapRequisitionNumber; }
            set
            {
                if (value != null && value.Length > MAX_REQUISITION_NUMBER_LENGTH)
                    throw new StringTooLongException("SAPRequisitionNumber", MAX_REQUISITION_NUMBER_LENGTH);
                if (_sapRequisitionNumber != value)
                {
                    SendPropertyChanging();
                    _sapRequisitionNumber = value;
                    SendPropertyChanged("SAPRequisitionNumber");
                }
            }
        }

        [Column(Storage = "_requisitionTypeID", DbType = "Int NOT NULL")]
        public int RequisitionTypeID
        {
            get { return _requisitionTypeID; }
            set
            {
                if (_requisitionTypeID!= value)
                {
                    if (_requisitionType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _requisitionTypeID = value;
                SendPropertyChanged("AssetTypeID");
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

        [Association(Name = "RequisitionType_WorkDescription", Storage = "_requisitionType", ThisKey = "RequisitionTypeID", IsForeignKey = true)]
        public RequisitionType RequisitionType
        {
            get { return _requisitionType.Entity; }
            set
            {
                var previousValue = _requisitionType.Entity;
                if ((previousValue != value)
                    || (_requisitionType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _requisitionType.Entity = null;
                        previousValue.Requisitions.Remove(this);
                    }
                    _requisitionType.Entity = value;
                    if (value != null)
                    {
                        value.Requisitions.Add(this);
                        _requisitionTypeID = value.RequisitionTypeID;
                    }
                    else
                        _requisitionTypeID = default(int);
                    SendPropertyChanged("RequisitionType");
                }
            }
        }

        [Association(Name = "WorkOrder_Requisition", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a Requisition record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.Requisitions.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.Requisitions.Add(this);
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

        [Column(Name = "CreatedAt", Storage = "_createdOn", DbType = "SmallDateTime NOT NULL", UpdateCheck = UpdateCheck.Never, IsDbGenerated=true)]
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                if (_createdOn != value)
                {
                    OnCreatedOnChanging(value);
                    SendPropertyChanging();
                    _createdOn = value;
                    SendPropertyChanged("CreatedOn");
                }
            }
        }

        [Column(Storage = "_creatorID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int CreatorID
        {
            get { return _creatorID; }
            set
            {
                if (_creatorID != value)
                {
                    if (_createdBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _creatorID = value;
                SendPropertyChanged("CreatorID");
            }
        }
        
        #endregion

        #region Constructors
		
        public Requisition()
        {
            _requisitionType = default(EntityRef<RequisitionType>);
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

#pragma warning disable 168
        private void OnCreatedOnChanging(DateTime value)
        {
            if (CreatedOn != DateTime.MinValue)
                throw new DomainLogicException("Cannot change the CreatedOn date once it has been set.");
        }

#pragma warning restore 168

	    #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return String.Format("{0} - {1}", RequisitionType, SAPRequisitionNumber);
        }

        #endregion
    }
}
