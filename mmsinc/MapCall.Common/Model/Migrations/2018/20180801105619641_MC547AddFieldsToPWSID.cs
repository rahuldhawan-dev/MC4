using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180801105619641), Tags("Production")]
    public class MC547AddFieldsToPWSID : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplies")
                 .AddColumn("FreeChlorineReported").AsBoolean().NotNullable().WithDefaultValue(true)
                 .AddColumn("TotalChlorineReported").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("FreeChlorineReported").FromTable("PublicWaterSupplies");
            Delete.Column("TotalChlorineReported").FromTable("PublicWaterSupplies");
        }
    }
}
