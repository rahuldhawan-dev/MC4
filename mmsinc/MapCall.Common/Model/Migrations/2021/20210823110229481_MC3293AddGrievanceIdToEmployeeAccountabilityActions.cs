using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210823110229481), Tags("Production")]
    public class MC3293AddGrievanceIdToEmployeeAccountabilityActions : Migration
    {
        public override void Up()
        {
            Alter.Table("EmployeeAccountabilityActions")
                 .AddForeignKeyColumn("GrievanceId", "UnionGrievances", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EmployeeAccountabilityActions", "GrievanceId", "UnionGrievances");
        }
    }
}

