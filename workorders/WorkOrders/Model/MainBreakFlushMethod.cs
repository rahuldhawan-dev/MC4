using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MainBreakFlushMethods")]
    public class MainBreakFlushMethod : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const int MAX_DESCRIPTION_LENGTH = 50;

        private string _description;
        private int _mainBreakFlushMethodID;

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        [Column(Storage = "_mainBreakFlushMethodID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainBreakFlushMethodID
        {
            get { return _mainBreakFlushMethodID; }
            set
            {
                if (_mainBreakFlushMethodID != value)
                {
                    SendPropertyChanging();
                    _mainBreakFlushMethodID = value;
                    SendPropertyChanged("MainBreakFlushMethodID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50) NULL", CanBeNull = true)]
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

        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Implementation of INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (string.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Cannot save a without a description.");
                    break;
            }
        }
    }
}
