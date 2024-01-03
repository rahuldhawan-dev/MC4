using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "PlantMaintenanceActivityTypes")]
    public class PlantMaintenanceActivityType
    {
        #region Constants

        public struct Indices
        {
            public const int
                BRG = 5, 
                DVA = 9,
                PBC = 18,
                RBS = 19,
                RPS = 32, 
                RPT = 33;
        }

        #endregion

        #region Private Members

        private int _id;
        private string _description, _code;

        #endregion

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Column(Storage = "_description", Name = "Description", DbType = "varchar(50) not null")]
        public string Description
        {
            get { return _description; }
        }

        [Column(Storage = "_code", Name = "Code", DbType = "varchar(50) not null")]
        public string Code
        {
            get { return _code; }
        }

        public string Display => $"{Code} - {Description}";
        public override string ToString()
        {
            return Display;
        }

        public static int[] GetOverrideCodes()
        {
            return new[] { Indices.DVA, Indices.PBC, Indices.RBS, Indices.RPS, Indices.RPT, Indices.BRG };
        }

        public static int[] GetOverrideCodesRequiringWBSNumber()
        {
            return new[] { Indices.DVA, Indices.RBS, Indices.RPS, Indices.RPT, Indices.BRG };
        }
    }
}