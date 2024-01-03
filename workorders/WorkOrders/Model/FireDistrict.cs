using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "FireDistricts")]
    public class FireDistrict
    {
        #region Private Members

        private int _fireDistrictID;
        private string _premiseNumber;

        #endregion

        #region Properties

        [Column(Storage = "_fireDistrictID", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int FireDistrictID => _fireDistrictID;
        
        [Column(Storage = "_premiseNumber", DbType = "VarChar(11)")]
        public string PremiseNumber => _premiseNumber;

        #endregion
    }
}