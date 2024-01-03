using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name="dbo.BusinessUnits")]
    public class BusinessUnit : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 6;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _businessUnitID,
                    _operatingCenterID,
                    _departmentID,
                    _order;
        private string _bu;
        private bool _is271Visible;

        private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<Department> _department;

        #endregion

        #region Properties

        [Column(Storage = "_businessUnitID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int BusinessUnitID
        {
            get { return _businessUnitID; }
            set
            {
                if (_businessUnitID != value)
                {
                    SendPropertyChanging();
                    _businessUnitID = value;
                    SendPropertyChanged("BusinessUnitID");
                }
            }
        }

        [Column(Storage = "_bu", DbType = "char(6) NOT NULL")]
        public string BU
        {
            get { return _bu; }
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new StringTooLongException("BU", MAX_DESCRIPTION_LENGTH);
                if (_bu != value)
                {
                    SendPropertyChanging();
                    _bu = value;
                    SendPropertyChanged("BU");
                }
            }
        }

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
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

        [Column(Storage = "_departmentID", DbType = "Int NOT NULL")]
        public int DepartmentID
        {
            get { return _departmentID; }
            set
            {
                if (_departmentID != value)
                {
                    if (_department.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _departmentID = value;
                    SendPropertyChanged("DepartmentID");
                }
            }
        }

        [Column(Storage = "_order", DbType = "Int Null")]
        public int Order
        {
            get { return _order; }
            set
            {
                if ((_order != value))
                {
                    SendPropertyChanging();
                    _order = value;
                    SendPropertyChanged("Order");
                }
            }
        }

        [Column(Storage = "_is271Visible", DbType="bit")]
        public bool Is271Visible
        {
            get { return _is271Visible; }
        }

        [Association(Name = "OperatingCenter_BusinessUnit", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingCenter.Entity; }
            set
            {
                var previousValue = _operatingCenter.Entity;
                if (((previousValue != value)
                     || (_operatingCenter.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _operatingCenter.Entity = null;
                        previousValue.BusinessUnits.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if ((value != null))
                    {
                        value.BusinessUnits.Add(this);
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

        [Association(Name= "Department_BusinessUnit", Storage = "_department", ThisKey = "DepartmentID", IsForeignKey = true)]
        public Department Department
        {
            get { return _department.Entity; }
            set
            {
                var previousValue = _department.Entity;
                if ((previousValue != value)
                    || (_department.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _department.Entity = null;
                        previousValue.BusinessUnits.Remove(this);
                    }
                    _department.Entity = value;
                    if (value != null)
                    {
                        value.BusinessUnits.Add(this);
                        _departmentID = value.DepartmentID;
                    }
                    else
                    {
                        _departmentID = default(int);
                    }
                    SendPropertyChanged("Department");
                }
            }
        }

        #endregion

        #region Constructors

        public BusinessUnit()
        {
            _operatingCenter = default(EntityRef<OperatingCenter>);
        }

        #endregion

        #region Private Methods

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

        #region Exposed Methods

        public override string ToString()
        {
            return BU;
        }

        #endregion
    }
}
