using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.PitcherFilterCustomerDeliveryMethods")]
    public class PitcherFilterCustomerDeliveryMethod
        : IComparable, INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        public const int MAX_DESCRIPTION_LENGTH = 50;
        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id;

        private string _description;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged(nameof(Id));
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50) NOT NULL")]
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

        #endregion

        #region Events/Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Exposed Methods

        public int CompareTo(object obj)
        {
            return obj is PitcherFilterCustomerDeliveryMethod m ? CompareTo(m) : -1;
        }

        public int CompareTo(PitcherFilterCustomerDeliveryMethod other)
        {
            return other.Id.CompareTo(Id);
        }

        public override string ToString() => Description;

        #endregion
    }
}
