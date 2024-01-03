using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.ReportViewings")]
    public class ReportViewing : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_REPORTNAME_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _ReportViewingID, _employeeID;
        private DateTime _DateViewed;
        private string _reportname;
        private EntityRef<Employee> _employee;

        #endregion

        #region Properties

        [Column(Storage = "_ReportViewingID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ReportViewingID
        {
            get { return _ReportViewingID; }
            set
            {
                if (_ReportViewingID != value)
                {
                    SendPropertyChanging();
                    _ReportViewingID = value;
                    SendPropertyChanged("ReportViewingID");
                }
            }
        }

        [Column(Storage = "_employeeID", DbType = "Int NOT NULL")]
        public int EmployeeID
        {
            get { return _employeeID; }
            set
            {
                if (_employeeID != value)
                {
                    if (_employee.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _employeeID = value;
                SendPropertyChanged("EmployeeID");
            }
        }

        [Column(Storage = "_DateViewed", DbType = "DateTime NOT NULL")]
        public DateTime DateViewed
        {
            get { return _DateViewed; }
            set
            {
                if (_DateViewed != value)
                {
                    SendPropertyChanging();
                    _DateViewed = value;
                    SendPropertyChanged("DateViewed");
                }
            }
        }

        [Column(Storage = "_reportname", DbType = "VarChar(50) NOT NULL")]
        public string ReportName
        {
            get { return _reportname; }
            set
            {
                if (value != null && value.Length > MAX_REPORTNAME_LENGTH)
                    throw new StringTooLongException("ReportName", MAX_REPORTNAME_LENGTH);
                if (_reportname != value)
                {
                    SendPropertyChanging();
                    _reportname = value;
                    SendPropertyChanged("ReportName");
                }
            }
        }

        [Association(Name = "Employee_ReportViewing", Storage = "_employee", ThisKey = "EmployeeID", IsForeignKey = true)]
        public Employee Employee
        {
            get { return _employee.Entity; }
            set
            {
                Employee previousValue = _employee.Entity;
                if ((previousValue != value)
                    || (_employee.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _employee.Entity = null;
                        previousValue.ReportViewings.Remove(this);
                    }
                    _employee.Entity = value;
                    if (value != null)
                    {
                        value.ReportViewings.Add(this);
                        _employeeID = value.EmployeeID;
                    }
                    else
                        _employeeID = default(int);
                    SendPropertyChanged("Employee");
                }
            }
        }

        #endregion

        #region Constructors

        public ReportViewing()
        {
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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(ReportName))
                        throw new DomainLogicException("ReportName cannot be null");
                    break;
            }
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
