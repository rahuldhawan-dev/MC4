using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.ContractorsOperatingCenters")]
    public class ContractorOperatingCenter : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _contractorOperatingCenterID, _contractorID, _operatingCenterID;
        private EntityRef<Contractor> _contractor;
        private EntityRef<OperatingCenter> _operatingCenter;

        #endregion

        #region Properties

        [Column(Storage = "_contractorOperatingCenterID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ContractorOperatingCenterID
        {
            get { return _contractorOperatingCenterID; }
            set
            {
                if (_contractorOperatingCenterID != value)
                {
                    SendPropertyChanging();
                    _contractorOperatingCenterID = value;
                    SendPropertyChanged("ContractorOperatingCenterID");
                }
            }
        }

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int OperatingCenterID
        {
            get { return _operatingCenterID; }
            set
            {
                if ((_operatingCenterID != value))
                {
                    if (_operatingCenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _operatingCenterID = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Column(Storage = "_contractorID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int ContractorID
        {
            get { return _contractorID; }
            set
            {
                if ((_contractorID != value))
                {
                    if (_contractor.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _contractorID = value;
                    SendPropertyChanged("contractorID");
                }
            }
        }

        #region Associations

        [Association(Name = "OperatingCenter_OperatingCentersContractor", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingCenter.Entity; }
            set
            {
                var previousValue = _operatingCenter.Entity;
                if ((previousValue != value)
                     || (_operatingCenter.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _operatingCenter.Entity = null;
                        previousValue.ContractorsOperatingCenters.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if ((value != null))
                    {
                        value.ContractorsOperatingCenters.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                    {
                        _operatingCenterID = default(int);
                    }
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "OperatingCenter_OperatingCentersContractor", Storage = "_contractor", ThisKey = "ContractorID", IsForeignKey = true)]
        public Contractor Contractor
        {
            get { return _contractor.Entity; }
            set
            {
                var previousValue = _contractor.Entity;
                if ((previousValue != value)
                     || (_contractor.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _contractor.Entity = null;
                        previousValue.ContractorsOperatingCenters.Remove(this);
                    }
                    _contractor.Entity = value;
                    if ((value != null))
                    {
                        value.ContractorsOperatingCenters.Add(this);
                        _contractorID = value.ContractorID;
                    }
                    else
                    {
                        _contractorID = default(int);
                    }
                    SendPropertyChanged("Contractor");
                }
            }
        }

        #endregion
        
        #endregion

        #region Constructors

        public ContractorOperatingCenter()
        {
            _contractor = default(EntityRef<Contractor>);
            _operatingCenter = default(EntityRef<OperatingCenter>);
        }

        #endregion

        #region Private Methods

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
            if (Contractor == null)
                throw new DomainLogicException(
                    "Cannot save an ContractorOperatingCenter object without a Contractor.");
            if (OperatingCenter == null)
                throw new DomainLogicException(
                    "Cannot save an ContractorOperatingCenter object without an OperatingCenter.");
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}