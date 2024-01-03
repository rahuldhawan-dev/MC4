using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418790), Tags("Production")]
    public class NormalizeShortCycleWorkOrderRequestsForMC1803 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderRequests").AlterColumn("MaintenanceActivityType").AsString(3).Nullable();
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderRequests SET MaintenanceActivityType = NULL WHERE LTRIM(RTRIM(MaintenanceActivityType)) = '';");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderRequests", "MaintenanceActivityType",
                "ShortCycleWorkOrderRequestMaintenanceActivityTypes", 3, newColumnName: "MaintenanceActivityTypeId");
        }

        public override void Down()
        {
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderRequests", "MaintenanceActivityType",
                "ShortCycleWorkOrderRequestMaintenanceActivityTypes", 3, newColumnName: "MaintenanceActivityTypeId");
        }
    }
}
