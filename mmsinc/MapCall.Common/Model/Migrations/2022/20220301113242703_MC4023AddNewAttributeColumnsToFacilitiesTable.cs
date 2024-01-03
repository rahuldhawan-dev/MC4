using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220301113242703), Tags("Production")]
    public class MC4023AddNewAttributeColumnsToFacilitiesTable : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities")
                 .AddColumn("BasicGroundWaterSupply")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);

            Alter.Table("tblFacilities")
                 .AddColumn("RawWaterPumpStation")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("BasicGroundWaterSupply")
                  .FromTable("tblFacilities");

            Delete.Column("RawWaterPumpStation")
                  .FromTable("tblFacilities");
        }
    }
}
