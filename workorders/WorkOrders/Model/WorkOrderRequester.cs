using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkOrderRequesters")]
    public class WorkOrderRequester : IComparable, INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 16;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _workOrderRequesterID;

        private string _description;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        [Column(Storage = "_workOrderRequesterID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkOrderRequesterID
        {
            get { return _workOrderRequesterID; }
            set
            {
                if (_workOrderRequesterID != value)
                {
                    SendPropertyChanging();
                    _workOrderRequesterID = value;
                    SendPropertyChanged("WorkOrderRequesterID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(16) NOT NULL")]
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

        [Association(Name = "Requester_WorkOrder", Storage = "_workOrders", OtherKey = "RequesterID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        #endregion

        #region Constructors

        public WorkOrderRequester()
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
                            "Description cannot be null");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.RequestedBy = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.RequestedBy = null;
        }

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

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return (obj is WorkOrderRequester)
                       ? CompareTo((WorkOrderRequester)obj) : -1;
        }

        public int CompareTo(WorkOrderRequester other)
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
            var id = WorkOrderRequesterID.CompareTo(other.WorkOrderRequesterID);
            return (desc == 0 && id == 0) ? 0 : -1;
        }

        // the compiler complains when Equals is overridden but
        // this isn't
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is WorkOrderRequester)
                       ? (CompareTo(obj) == 0) : base.Equals(obj);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
