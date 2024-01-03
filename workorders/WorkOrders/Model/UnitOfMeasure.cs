using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "UnitsOfMeasure")]
    public class UnitOfMeasure
    {
        #region Private Members

        private int _id;
        private string _description;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int UnitOfMeasureID
        {
            get { return _id; }
        }

        [Column(Storage = "_description", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get { return _description; }
        }

        #endregion

        public override string ToString()
        {
            return Description;
        }
    }
}