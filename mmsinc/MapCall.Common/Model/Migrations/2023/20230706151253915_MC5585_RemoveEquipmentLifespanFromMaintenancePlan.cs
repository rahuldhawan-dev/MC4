using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230706151253915), Tags("Production")]
    public class MC5585_RemoveEquipmentLifespanFromMaintenancePlan_ : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("MaintenancePlans", "EquipmentLifespanId", "EquipmentLifespans");
        }

        public override void Down()
        {
            Alter.Table("MaintenancePlans")
                 .AddForeignKeyColumn("EquipmentLifespanId", "EquipmentLifespans")
                 .Nullable();
        }
    }
}
