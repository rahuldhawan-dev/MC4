using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name="dbo.OperatingCentersUsers")]
    public class OperatingCenterUser : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _operatingCenterID,
                    _employeeID; // Employee.EmployeeID / tblPermissions.RecID

        private EntityRef<Employee> _employee;

        private EntityRef<OperatingCenter> _operatingCenter;

        #endregion

        #region Properties

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

        [Column(Storage = "_employeeID", Name="UserID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int EmployeeID
        {
            get { return _employeeID; }
            set
            {
                if ((_employeeID != value))
                {
                    if (_employee.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _employeeID = value;
                    SendPropertyChanged("employeeID");
                }
            }
        }

        #region Associations

        [Association(Name = "OperatingCenter_OperatingCentersUser", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.OperatingCentersUsers.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCentersUsers.Add(this);
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

        [Association(Name = "OperatingCenter_OperatingCentersUsers", Storage = "_employee", ThisKey = "EmployeeID", IsForeignKey = true)]
        public Employee Employee
        {
            get { return _employee.Entity; }
            set
            {
                var previousValue = _employee.Entity;
                if ((previousValue != value)
                     || (_employee.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _employee.Entity = null;
                        previousValue.OperatingCentersUsers.Remove(this);
                    }
                    _employee.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCentersUsers.Add(this);
                        _employeeID = value.EmployeeID;
                    }
                    else
                    {
                        _employeeID = default(int);
                    }
                    SendPropertyChanged("Employee");
                }
            }
        }

    	#endregion

        #endregion

        #region Constructors

        public OperatingCenterUser()
        {
            _employee = default(EntityRef<Employee>);
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
            if (Employee == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCentersUsers object without an Employee.");
            if (OperatingCenter == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCentersUsers object without an OperatingCenter.");
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
