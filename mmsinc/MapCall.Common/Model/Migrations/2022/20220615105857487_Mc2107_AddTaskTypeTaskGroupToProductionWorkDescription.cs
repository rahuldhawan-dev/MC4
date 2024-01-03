using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220615105857487)]
    [Tags("Production")]
    public class Mc2107_AddTaskTypeTaskGroupToProductionWorkDescription : Migration
    {
        #region Exposed Methods

        public override void Up()
        {
            Alter.Table("ProductionWorkDescriptions")
                 .AddForeignKeyColumn("MaintenancePlanTaskTypeId", "MaintenancePlanTaskTypes");
            Alter.Table("ProductionWorkDescriptions")
                 .AddForeignKeyColumn("TaskGroupId", "TaskGroups");
        }

        public override void Down()
        {
            this.DeleteForeignKeyColumn("ProductionWorkDescriptions", "MaintenancePlanTaskTypeId", "MaintenancePlanTaskTypes");
            this.DeleteForeignKeyColumn("ProductionWorkDescriptions", "TaskGroupId", "TaskGroups");
        }

        #endregion
    }
}
