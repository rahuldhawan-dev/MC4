using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkOrderPriorities")]
    public class WorkOrderPriority : IComparable, INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 15;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _description;

        private int _workOrderPriorityID;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        [Column(Storage = "_description", DbType = "VarChar(15) NOT NULL", CanBeNull = false)]
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

        [Column(Storage = "_workOrderPriorityID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkOrderPriorityID
        {
            get { return _workOrderPriorityID; }
            set
            {
                if ((_workOrderPriorityID != value))
                {
                    SendPropertyChanging();
                    _workOrderPriorityID = value;
                    SendPropertyChanged("WorkOrderPriorityID");
                }
            }
        }

        [Association(Name = "WorkOrderPriority_WorkOrder", Storage = "_workOrders", OtherKey = "PriorityID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        #endregion

        #region Constructors

        public WorkOrderPriority()
        {
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
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
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException(
                            "Cannot save a WorkOrderPriority record without a description.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Priority = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Priority = null;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(WorkOrderPriority other)
        {
            int desc;
            if (Description == null && other.Description == null)
                desc = 0;
            else if (Description == null)
                desc = 1;
            else if (other.Description == null)
                desc = -1;
            else
                desc = Description.CompareTo(other.Description);
            var id = WorkOrderPriorityID.CompareTo(other.WorkOrderPriorityID);
            return (desc == 0 && id == 0) ? 0 : -1;
        }

        public int CompareTo(object obj)
        {
            return (obj is WorkOrderPriority)
                       ? CompareTo((WorkOrderPriority)obj) : -1;
        }

        // the compiler complains when Equals is overridden but
        // this isn't
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is WorkOrderPriority)
                       ? (CompareTo(obj) == 0) : base.Equals(obj);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
