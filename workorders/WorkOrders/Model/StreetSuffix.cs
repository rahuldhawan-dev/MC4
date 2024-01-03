using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.StreetSuffixes")]
    public class StreetSuffix : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id;
        private string _description;

        #endregion

        #region Properties

        [Column(Name = "Id", Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged("StreetID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(10)")]
        public string Description
        {
            get { return _description; }
            set
            {
                if ((_description != value))
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("StreetSuffix");
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

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}