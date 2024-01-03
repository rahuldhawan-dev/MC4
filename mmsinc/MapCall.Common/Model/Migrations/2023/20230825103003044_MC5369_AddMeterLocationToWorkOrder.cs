using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230825103003044), Tags("Production")]
    public class MC5369_AddMeterLocationToWorkOrder : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("MeterLocationId", "MeterLocations", "MeterLocationID");
        }

        public override void Down()
        {
            this.DeleteForeignKeyColumn("WorkOrders", "MeterLocationId", "MeterLocations");
        }
    }
}