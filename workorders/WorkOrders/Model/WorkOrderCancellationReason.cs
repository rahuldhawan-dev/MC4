using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name= "dbo.WorkOrderCancellationReasons")]
    public class WorkOrderCancellationReason
    {
        #region Private Members

        private int _id;
        private string _description, _status;
        
        #endregion

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_description", Name = "Description",DbType = "varchar(50) not null")]
        public string Description
        {
            get { return _description; }
        }

        [Column(Storage = "_status", Name = "Status", DbType = "varchar(4) not null")]
        public string Status
        {
            get { return _status; }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}