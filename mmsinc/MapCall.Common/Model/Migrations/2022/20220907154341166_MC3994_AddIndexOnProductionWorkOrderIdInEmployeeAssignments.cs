using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220907154341166), Tags("Production")]
    public class MC3994AddIndexOnProductionWorkOrderIdInEmployeeAssignments : Migration
    {
        public override void Up()
        {
            Execute.Sql("IF NOT Exists(select 1 from sysindexes where name = 'IX_EmployeeAssignments_PWOId') " +
                        "   CREATE INDEX IX_EmployeeAssignments_PWOId ON EmployeeAssignments(ProductionWorkOrderId)");
        }

        public override void Down()
        {
            Execute.Sql("IF Exists(select 1 from sysindexes where name = 'IX_EmployeeAssignments_PWOId') " +
                        "   DROP INDEX IX_EmployeeAssignments_PWOId ON EmployeeAssignments");
        }
    }
}
