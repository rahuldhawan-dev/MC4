using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "HydrantBillings")]
    public class HydrantBilling
    {
        #region Private Members

        private int _id;
        private string _description;

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert,
            DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id => _id;

        [Column(Storage = "_description", DbType = "VarChar(50)")]
        public string Description => _description;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
