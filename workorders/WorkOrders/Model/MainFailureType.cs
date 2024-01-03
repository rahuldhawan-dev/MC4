using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MainFailureTypes")]
    public class MainFailureType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 20;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _description;

        private int _mainFailureTypeID;

        private readonly EntitySet<MainBreak> _mainBreaks;

        #endregion

        #region Properties

        [Column(Storage = "_description", DbType = "VarChar(20) NOT NULL", CanBeNull = false)]
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

        [Column(Storage = "_mainFailureTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainFailureTypeID
        {
            get { return _mainFailureTypeID; }
            set
            {
                if ((_mainFailureTypeID != value))
                {
                    SendPropertyChanging();
                    _mainFailureTypeID = value;
                    SendPropertyChanged("MainFailureTypeID");
                }
            }
        }

        [Association(Name = "MainFailureType_MainBreak", Storage = "_mainBreaks", OtherKey = "MainFailureTypeID")]
        public EntitySet<MainBreak> MainBreaks
        {
            get { return _mainBreaks; }
            set { _mainBreaks.Assign(value); }
        }

        #endregion

        #region Extension Methods

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Cannot save a MainFailureType without a Description.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        #endregion

        #region Constructors

        public MainFailureType()
        {
            _mainBreaks = new EntitySet<MainBreak>(attach_MainBreaks, detach_MainBreaks);
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

        private void attach_MainBreaks(MainBreak entity)
        {
            SendPropertyChanging();
            entity.MainFailureType = this;
        }

        private void detach_MainBreaks(MainBreak entity)
        {
            SendPropertyChanging();
            entity.MainFailureType = null;
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
