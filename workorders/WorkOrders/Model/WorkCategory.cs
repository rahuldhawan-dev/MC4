using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkCategories")]
    public class WorkCategory : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 35;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _workCategoryID;

        private string _description;

        private EntitySet<WorkDescription> _workDescriptions;

        #endregion

        #region Properties

        [Column(Storage = "_workCategoryID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkCategoryID
        {
            get { return _workCategoryID; }
            set
            {
                if (_workCategoryID != value)
                {
                    SendPropertyChanging();
                    _workCategoryID = value;
                    SendPropertyChanged("WorkCategoryID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(35) NOT NULL")]
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

        [Association(Name = "WorkCategory_workDescription", Storage = "_workDescriptions", OtherKey = "WorkCategoryID")]
        public EntitySet<WorkDescription> WorkDescriptions
        {
            get { return _workDescriptions; }
            set { _workDescriptions.Assign(value); }
        }

        #endregion

        #region Constructors

        public WorkCategory()
        {
            _workDescriptions = new EntitySet<WorkDescription>(attach_WorkDescriptions, detach_WorkDescriptions);
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

        private void attach_WorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.WorkCategory = this;
        }

        private void detach_WorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.WorkCategory = null;
        }

        protected void OnValidate(ChangeAction action)
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

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
