using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "ValveSizes")]
    public class ValveSize
    {
        #region Private Members

        private int _id;
        private decimal _size;

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert,
            DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id => _id;
        
        [Column(Storage = "_size", DbType = "decimal(5,2)")]
        public decimal Size => _size;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Size.ToString();
        }

        #endregion
    }
}