using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SAPWorkOrderSteps")]
    public class SAPWorkOrderStep
    {
        #region Constants

        public struct Indices
        {
            public const short CREATE = 1, UPDATE = 2, COMPLETE = 3, APPROVE_GOODS = 4, UPDATE_WITH_NMI = 5, NMI = 6;
        }

        #endregion

        #region Private Members

        private int _id;
        private string _description, _status;

        #endregion

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_description", Name = "Description", DbType = "varchar(50) not null")]
        public string Description
        {
            get { return _description; }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}