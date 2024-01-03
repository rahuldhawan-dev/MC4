using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkOrderPurposes")]
    public class WorkOrderPurpose : IComparable, INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 20;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Memberes

        private int _workOrderPurposeID;

        private string _description;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        [Column(Storage = "_description", DbType = "VarChar(20) NOT NULL")]
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

        [Column(Storage = "_workOrderPurposeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkOrderPurposeID
        {
            get { return _workOrderPurposeID; }
            set
            {
                if (_workOrderPurposeID != value)
                {
                    SendPropertyChanging();
                    _workOrderPurposeID = value;
                    SendPropertyChanged("WorkOrderPurposeID");
                }
            }
        }

        [Association(Name = "Purpose_WorkOrder", Storage = "_workOrders", OtherKey = "PurposeID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        #endregion

        #region Constructors

        public WorkOrderPurpose()
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
                        throw new DomainLogicException("Description cannot be null");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.DrivenBy = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.DrivenBy = null;
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

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return (obj is WorkOrderPurpose)
                       ? CompareTo((WorkOrderPurpose)obj) : -1;
        }

        public int CompareTo(WorkOrderPurpose other)
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
            var id = WorkOrderPurposeID.CompareTo(other.WorkOrderPurposeID);
            return (desc == 0 && id == 0) ? 0 : -1;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is WorkOrderPurpose)
                       ? (CompareTo(obj) == 0) : base.Equals(obj);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
