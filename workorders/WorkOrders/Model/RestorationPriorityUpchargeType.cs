using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "RestorationPriorityUpchargeTypes")]
    public class RestorationPriorityUpchargeType
    {
        #region Private Members

        private int _id;
        private string _description;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_description", DbType = "VarChar(50)")]
        public string Description
        {
            get { return _description; }
        }

        #endregion

        #region Private Methods

        protected void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void SendPropertyChanged(String propertyName)
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
            return Description;
        }

        #endregion
    }
}
