using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    /// <summary>
    /// This is a view, not an entity!
    /// </summary>
    [Table(Name = "dbo.AggregateRoles")]
    public class AggregateRole
    {
        private string _compositeId;

        private int _moduleId, _employeeId;

        private int? _operatingCenterId;
        
        [Column(Name = "CompositeId", Storage = "_compositeId", AutoSync = AutoSync.OnInsert, DbType = "varchar(max)", IsPrimaryKey = true, IsDbGenerated = true)]
        public string CompositeId
        {
            get => _compositeId;
            set => _compositeId = value;
        }

        [Column(Name = "UserId", Storage = "_employeeId", DbType = "Int NOT NULL")]
        public int UserId
        {
            get => _employeeId;
            set => _employeeId = value;
        }

        [Column(Name = "ModuleId", Storage = "_moduleId", DbType = "Int NOT NULL")]
        public int ModuleId
        {
            get => _moduleId;
            set => _moduleId = value;
        }

        [Column(Name = "OperatingCenterID", Storage = "_operatingCenterId", DbType = "Int NULL")]
        public int? OperatingCenterId
        {
            get => _operatingCenterId;
            set => _operatingCenterId = value;
        }
    }
}