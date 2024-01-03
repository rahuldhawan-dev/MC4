using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name="Departments")]
    public class Department : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 50;
        private const short MAX_CODE_LENGTH = 2;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _departmentID;
        
        private string _code, _description;

        private readonly EntitySet<BusinessUnit> _businessUnits;

        #endregion

        #region Properties

        [Column(Storage = "_departmentID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DepartmentID
        {
            get { return _departmentID; }
            set
            {
                if (_departmentID != value)
                {
                    SendPropertyChanging();
                    _departmentID = value;
                    SendPropertyChanged("DepartmentID");
                }
            }
        }

        [Column(Storage =  "_code", DbType = "char(2) NOT NULL")]
        public string Code
        {
            get { return _code; }
            set
            {
                if (value != null && value.Length > MAX_CODE_LENGTH)
                    throw new StringTooLongException("CODE", MAX_CODE_LENGTH);
                if (_code != value)
                {
                    SendPropertyChanging();
                    _code = value;
                    SendPropertyChanged("Code");
                }
            }
        }

        [Column(Storage = "_description", DbType = "varchar(50) NOT NULL")]
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

        [Association(Name = "OperatingCenter_BusinessUnit", Storage = "_businessUnits", OtherKey = "OperatingCenterID")]
        public EntitySet<BusinessUnit> BusinessUnits
        {
            get { return _businessUnits; }
            set { _businessUnits.Assign(value); }
        }

        #endregion

        #region Constructors

        public Department()
        {
            _businessUnits =
                new EntitySet<BusinessUnit>(
                    attach_BusinessUnits,
                    detach_BusinessUnits);
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

        private void attach_BusinessUnits(BusinessUnit entity)
        {
            SendPropertyChanging();
            entity.Department = this;
        }

        private void detach_BusinessUnits(BusinessUnit entity)
        {
            SendPropertyChanging();
            entity.Department = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        #endregion
    }
}
