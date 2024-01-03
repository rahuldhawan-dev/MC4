using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210824150610369), Tags("Production")]
    public class MC2487AddAdditionalOptionToInspectionTypes : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("TankInspectionTypes").Row(new {Description = "Inspection Site Observation-Drone"});
        }

        public override void Down()
        {
            Delete.FromTable("TankInspectionTypes").Row(new {Description = "Inspection Site Observation-Drone"});
        }
    }
}

