using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Roles")]
    public class Role
    {
        private int _roleId, _moduleId, _employeeId;

        private int? _operatingCenterId;

        [Column(Name = "RoleId", Storage = "_roleId", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RoleId
        {
            get => _roleId;
            set => _roleId = value;
        }

        [Column(Name = "UserId", Storage = "_employeeId", DbType = "Int NOT NULL")]
        public int EmployeeId
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