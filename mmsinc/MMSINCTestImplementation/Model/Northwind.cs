using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;
using MMSINC.Interface;

namespace MMSINCTestImplementation.Model
{
    partial class NorthwindDataContext : IDataContext { }

    partial class Employee : IEmployee
    {
        #region Private Members

        private EntityRef<Employee> _ReportsTo;
        private int? _ReportsToID;

        #endregion

        #region Properties

        public string FullName
        {
            get { return LastName + ", " + FirstName; }
        }

        public bool IsTopLevelSupervisor
        {
            get { return ReportsTo == null; }
            // TODO: HACK THIS IS A HACK TO GET AROUND THE PROBLEMS WITH 
            // PP:OBJECTCONTAINERDATASOURCE IF YOU TRACE IT, SOMEWHERE ALONG 
            // THE LINE IT CALLS SOMETHING THAT REFLECTS AND COPIES YOUR 
            // OBJECT AND TRIES TO SET ALL THE PROPERTIES. NICE OF THEM TO 
            // CONSIDER READONLY PROPERTIES
            set { }
        }

        [Column(Storage = "_ReportsToID", Name = "ReportsTo", DbType = "Int")]
        public int? ReportsToID
        {
            get { return this._ReportsToID; }
            set
            {
                if ((this._ReportsToID != value))
                {
                    if (this._ReportsTo.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }

                    this.OnReportsToChanging(value);
                    this.SendPropertyChanging();
                    this._ReportsToID = value;
                    this.SendPropertyChanged("ReportsToID");
                    this.OnReportsToChanged();
                }
            }
        }

        [Association(Name = "ReportsTo", Storage = "_ReportsTo", ThisKey = "ReportsToID", OtherKey = "EmployeeID",
            IsForeignKey = true)]
        public Employee ReportsTo
        {
            get { return this._ReportsTo.Entity; }
            set
            {
                Employee previousValue = this._ReportsTo.Entity;
                if (((previousValue != value)
                     || (this._ReportsTo.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._ReportsTo.Entity = null;
                        previousValue.Employees.Remove(this);
                    }

                    this._ReportsTo.Entity = value;
                    if ((value != null))
                    {
                        value.Employees.Add(this);
                        this._ReportsToID = value.EmployeeID;
                    }
                    else
                    {
                        this._ReportsToID = default(int?);
                    }

                    this.SendPropertyChanged("ReportsTo");
                }
            }
        }

        [Association(Name = "Employee_Employees", Storage = "_Employees", ThisKey = "EmployeeID",
            OtherKey = "ReportsToID")]
        public EntitySet<Employee> Employees
        {
            get { return this._Employees; }
            set { this._Employees.Assign(value); }
        }

        #endregion

        #region Extension Methods

        partial void OnBirthDateChanging(DateTime? value)
        {
            if (BirthDate != null && BirthDate.Value != DateTime.MinValue)
                throw new DomainLogicException(
                    "Cannot change the value of an Employee's BirthDate once it has been set.");
        }

        #endregion

        #region Private Methods

        // ReSharper disable UnusedMember.Local
        partial void OnValidate(ChangeAction action)
            // ReSharper restore UnusedMember.Local
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (BirthDate != null && BirthDate.Value.Date == DateTime.Today.Date.AddDays(-1))
                    {
                        throw new DomainLogicException("Employee can't be born yesterday... for some reason.");
                    }

                    break;
            }
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullName;
        }

        #endregion
    }

    // this was only created so that sonar will stop complaining about the empty setter for
    // IsTopLevelSupervisor in the above class
    public interface IEmployee
    {
        bool IsTopLevelSupervisor { set; }
    }
}
