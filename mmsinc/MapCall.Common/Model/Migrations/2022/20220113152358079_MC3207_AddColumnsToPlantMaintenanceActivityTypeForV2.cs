using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220113152358079), Tags("Production")]
    public class MC3207_AddColumnsToPlantMaintenanceActivityTypeForV2 : Migration
    {
        public override void Up()
        {
            Alter.Table("PlantMaintenanceActivityTypes").AddColumn("RequiresWBSNumber").AsBoolean().WithDefaultValue(false);
            Alter.Table("PlantMaintenanceActivityTypes").AddColumn("IsOverrideCode").AsBoolean().WithDefaultValue(false);

            Execute.Sql(@"UPDATE PlantMaintenanceActivityTypes 
                          SET IsOverrideCode = 1
                          WHERE Id IN (5,9,18,19,32,33)");
            Execute.Sql(@"UPDATE PlantMaintenanceActivityTypes 
                          SET RequiresWBSNumber = 1
                          WHERE Id IN (5,9,19,32,33)");
        }

        public override void Down()
        {
            Delete.Column("RequiresWBSNumber").FromTable("PlantMaintenanceActivityTypes");
            Delete.Column("IsOverrideCode").FromTable("PlantMaintenanceActivityTypes");
        }
    }
}

