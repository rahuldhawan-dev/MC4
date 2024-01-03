using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171214151529934), Tags("Production")]
    public class AddDescriptionColumnToWaterSystemsForWO214341 : Migration
    {
        public override void Up()
        {
            Alter.Table("WaterSystems").AddColumn("LongDescription").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LongDescription").FromTable("WaterSystems");
        }
    }
}
