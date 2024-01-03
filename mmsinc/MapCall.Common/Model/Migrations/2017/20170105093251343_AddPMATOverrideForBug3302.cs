using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170105093251343), Tags("Production")]
    public class AddPMATOverrideForBug3302 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("PlantMaintenanceActivityTypeOverrideId", "PlantMaintenanceActivityTypes");
            Alter.Table("WorkOrders")
                 .AddColumn("MaterialPlanningCompletedOn")
                 .AsDateTime()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "PlantMaintenanceActivityTypeOverrideId",
                "PlantMaintenanceActivityTypes");
            Delete.Column("MaterialPlanningCompletedOn").FromTable("WorkOrders");
        }
    }
}
