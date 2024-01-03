using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkOrderDescriptionChanges")]
    public class WorkOrderDescriptionChange : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Methods

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _workOrderDescriptionChangeID,
                    _workOrderID,
                    _toWorkDescriptionID,
                    _responsibleEmployeeID;

        private int? _fromWorkDescriptionID; 

        private DateTime _dateOfChange;

        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<WorkDescription> _toWorkDescription,
                                           _fromWorkDescription;

        private EntityRef<Employee> _responsibleEmployee;

        #endregion

        #region Properties

        [Column(Storage = "_workOrderDescriptionChangeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkOrderDescriptionChangeID
        {
            get { return _workOrderDescriptionChangeID; }
            set
            {
                if (_workOrderDescriptionChangeID != value)
                {
                    SendPropertyChanging();
                    _workOrderDescriptionChangeID = value;
                    SendPropertyChanged("WorkOrderDescriptionChangeID");
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

        [Column(Storage = "_toWorkDescriptionID", DbType = "Int NOT NULL")]
        public int ToWorkDescriptionID
        {
            get { return _toWorkDescriptionID; }
            set
            {
                if (_toWorkDescriptionID != value)
                {
                    if (_toWorkDescription.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _toWorkDescriptionID = value;
                SendPropertyChanged("ToWorkDescriptionID");
            }
        }

        [Column(Storage = "_fromWorkDescriptionID", DbType = "Int NULL")]
        public int? FromWorkDescriptionID
        {
            get { return _fromWorkDescriptionID; }
            set
            {
                if (_fromWorkDescriptionID != value)
                {
                    if (_fromWorkDescription.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _fromWorkDescriptionID = value.Value;
                SendPropertyChanged("FromWorkDescriptionID");
            }
        }

        [Column(Storage = "_responsibleEmployeeID", DbType = "Int NOT NULL")]
        public int ResponsibleEmployeeID
        {
            get { return _responsibleEmployeeID; }
            set
            {
                if (_responsibleEmployeeID != value)
                {
                    if (_responsibleEmployee.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _responsibleEmployeeID = value;
                SendPropertyChanged("ResponsibleEmployeeID");
            }
        }

        [Column(Storage = "_dateOfChange", DbType = "SmallDateTime NOT NULL")]
        public DateTime DateOfChange
        {
            get { return _dateOfChange; }
            set
            {
                if (_dateOfChange != value)
                {
                    SendPropertyChanging();
                    _dateOfChange = value;
                    SendPropertyChanged("DateOfChange");
                }
            }
        }

        [Association(Name = "WorkOrder_WorkOrderDescriptionChange", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                var previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.WorkOrderDescriptionChanges.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrderDescriptionChanges.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                        _workOrderID = default(int);
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        [Association(Name = "ToWorkDescription_WorkOrderDescriptionChange", Storage = "_toWorkDescription", ThisKey = "ToWorkDescriptionID", IsForeignKey = true)]
        public WorkDescription ToWorkDescription
        {
            get { return _toWorkDescription.Entity; }
            set
            {
                var previousValue = _toWorkDescription.Entity;
                if ((previousValue != value)
                    || (_toWorkDescription.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _toWorkDescription.Entity = null;
                        previousValue.ToWorkOrderDescriptionChanges.Remove(this);
                    }
                    _toWorkDescription.Entity = value;
                    if (value != null)
                    {
                        value.ToWorkOrderDescriptionChanges.Add(this);
                        _toWorkDescriptionID = value.WorkDescriptionID;
                    }
                    else
                        _toWorkDescriptionID = default(int);
                    SendPropertyChanged("ToWorkDescription");
                }
            }
        }

        [Association(Name = "FromWorkDescription_WorkOrderDescriptionChange", Storage = "_fromWorkDescription", ThisKey = "FromWorkDescriptionID", IsForeignKey = true)]
        public WorkDescription FromWorkDescription
        {
            get { return _fromWorkDescription.Entity; }
            set
            {
                var previousValue = _fromWorkDescription.Entity;
                if ((previousValue != value)
                    || (_fromWorkDescription.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _fromWorkDescription.Entity = null;
                        previousValue.FromWorkOrderDescriptionChanges.Remove(
                            this);
                    }
                    _fromWorkDescription.Entity = value;
                    if (value != null)
                    {
                        value.FromWorkOrderDescriptionChanges.Add(this);
                        _fromWorkDescriptionID = value.WorkDescriptionID;
                    }
                    else
                        _fromWorkDescriptionID = default(int);
                    SendPropertyChanged("FromWorkDescription");
                }
            }
        }

        [Association(Name = "ResponsibleEmployee_WorkOrderDescriptionChange", Storage = "_responsibleEmployee", ThisKey = "ResponsibleEmployeeID", IsForeignKey = true)]
        public Employee ResponsibleEmployee
        {
            get { return _responsibleEmployee.Entity; }
            set
            {
                var previousValue = _responsibleEmployee.Entity;
                if ((previousValue != value)
                    || (_responsibleEmployee.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _responsibleEmployee.Entity = null;
                        previousValue.WorkOrderDescriptionChanges.Remove(this);
                    }
                    _responsibleEmployee.Entity = value;
                    if (value != null)
                    {
                        value.WorkOrderDescriptionChanges.Add(this);
                        _responsibleEmployeeID = value.EmployeeID;
                    }
                    else
                        _responsibleEmployeeID = default(int);
                    SendPropertyChanged("ResponsibleEmployee");
                }
            }
        }

        #endregion

        #region Constructors

        public WorkOrderDescriptionChange()
        {
            _workOrder = default(EntityRef<WorkOrder>);
            _toWorkDescription = default(EntityRef<WorkDescription>);
            _fromWorkDescription = default(EntityRef<WorkDescription>);
            _responsibleEmployee = default(EntityRef<Employee>);
        }

        #endregion

        #region Private Methods

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Delete:
                    return;
                case ChangeAction.Insert:
                    ValidateProperties();
                    break;
                case ChangeAction.Update:
                    ValidateProperties();
                    break;
            }
        }

        private void ValidateProperties()
        {
            if (WorkOrder == null)
                throw new DomainLogicException("Cannot save a WorkOrderDescriptionChange without a WorkOrder.");
            if (ToWorkDescription == null)
                throw new DomainLogicException("Cannot save a WorkOrderDescriptionChange without a ToWorkDescription.");
            if (FromWorkDescription == null)
                throw new DomainLogicException("Cannot save a WorkOrderDescriptionChange without a FromWorkDescription.");
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
