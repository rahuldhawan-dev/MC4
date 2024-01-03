using System;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name="dbo.Services")]
    public class Service
    {
        #region Private Members

        private int _id;
        private DateTime? _dateInstalled;

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Column(Storage = "_dateInstalled", DbType = "SmallDateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DateInstalled
        {
            get { return _dateInstalled; }
            set
            {
                if (_dateInstalled != value)
                {
                    _dateInstalled = value;
                }
            }
        }

        #endregion
    }
}