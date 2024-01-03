using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.AccountingTypes")]
    public class AccountingType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 10;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _accountingTypeID;

        private AccountingTypeEnum? _typeEnum;

        private string _description;

        private readonly EntitySet<WorkDescription> _workDescriptions;

        #endregion

        #region Properties

        public AccountingTypeEnum TypeEnum
        {
            get
            {
                if (_typeEnum == null)
                    _typeEnum = AccountingTypeRepository.GetEnumerationValue(this);
                return _typeEnum.Value;
            }
        }

        [Column(Storage = "_accountingTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int AccountingTypeID
        {
            get { return _accountingTypeID; }
            set
            {
                if (_accountingTypeID != value)
                {
                    SendPropertyChanging();
                    _accountingTypeID = value;
                    SendPropertyChanged("AccountingTypeID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(10) NOT NULL")]
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

        //[Association(Name = "AccountingType_WorkOrder", Storage = "_workOrders", OtherKey = "AccountingTypeID")]
        //public EntitySet<WorkOrder> WorkOrders
        //{
        //    get { return _workOrders; }
        //    set { _workOrders.Assign(value); }
        //}

        [Association(Name = "AccountingType_WorkDescription", Storage = "_workDescriptions", OtherKey = "AccountingTypeID")]
        public EntitySet<WorkDescription> WorkDescriptions
        {
            get { return _workDescriptions; }
            set { _workDescriptions.Assign(value); }
        }

        #endregion

        #region Constructors

        public AccountingType()
        {
            //_workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _workDescriptions = new EntitySet<WorkDescription>(attach_WorkDescriptions, detach_WorkDescriptions);
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

        //private void attach_WorkOrders(WorkOrder entity)
        //{
        //    SendPropertyChanging();
        //    entity.AccountingType = this;
        //}

        //private void detach_WorkOrders(WorkOrder entity)
        //{
        //    SendPropertyChanging();
        //    entity.AccountingType = null;
        //}

        private void attach_WorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.AccountingType = this;
        }

        private void detach_WorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.AccountingType = null;
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

    public enum AccountingTypeEnum
    {
        Capital = 1, OAndM = 2, Retirement = 3
    }
}
