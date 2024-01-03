using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Contractors")]
    public class Contractor : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 255;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _contractorID;
        private string _name;
        private bool? _contractorsAccess;
        private readonly EntitySet<WorkOrder> _assignedWorkOrders;
        private readonly EntitySet<ContractorOperatingCenter>
            _contractorsOperatingCenters;

            #endregion

        #region Properties

        [Column(Storage = "_contractorID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ContractorID
        {
            get { return _contractorID; }
            set
            {
                if (_contractorID != value)
                {
                    SendPropertyChanging();
                    _contractorID = value;
                    SendPropertyChanged("ContractorID");
                }
            }
        }

        [Column(Storage ="_name", DbType = "VarChar(255) NOT NULL")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new StringTooLongException("Name", MAX_DESCRIPTION_LENGTH);
                if (_name != value)
                {
                    SendPropertyChanging();
                    _name = value;
                    SendPropertyChanged("Name");
                }
            }
        }

        [Column(Storage = "_contractorsAccess", DbType = "Bit NULL")]
        public bool? ContractorsAccess
        {
            get { return _contractorsAccess; }
            set
            {
                if (_contractorsAccess != value)
                {
                    SendPropertyChanging();
                    _contractorsAccess = value;
                    SendPropertyChanged("ContractorsAccess");
                }
            }
        }

        [Association(Name = "AssignedContractor_WorkOrder", Storage = "_assignedWorkOrders", OtherKey = "AssignedContractorID")]
        public EntitySet<WorkOrder> AssignedWorkOrders
        {
            get { return _assignedWorkOrders; }
            set { _assignedWorkOrders.Assign(value); }
        }
        
        [Association(Name = "OperatingCenter_ContractorsOperatingCenters", Storage = "_contractorsOperatingCenters", OtherKey = "ContractorID")]
        public EntitySet<ContractorOperatingCenter>  ContractorsOperatingCenters
        {
            get { return _contractorsOperatingCenters; }
            set { _contractorsOperatingCenters.Assign(value); }
        }

        #endregion

        #region Contructors

        public Contractor()
        {
            _assignedWorkOrders =
                new EntitySet<WorkOrder>(
                    attach_AssignedWorkOrders,
                    detach_AssignedWorkOrders);
            _contractorsOperatingCenters = new EntitySet<ContractorOperatingCenter>(
                attach_ContractorsOperatingCenters,
                detach_ContractorsOperatingCenters);
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

        private void attach_AssignedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.AssignedContractor = this;
        }
        private void detach_AssignedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.AssignedContractor = null;
        }

        private void attach_ContractorsOperatingCenters(ContractorOperatingCenter entity)
        {
            SendPropertyChanging();
            entity.Contractor = this;
        }

        private void detach_ContractorsOperatingCenters(ContractorOperatingCenter entity)
        {
            SendPropertyChanging();
            entity.Contractor = null;
        }

        #endregion
        
        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
