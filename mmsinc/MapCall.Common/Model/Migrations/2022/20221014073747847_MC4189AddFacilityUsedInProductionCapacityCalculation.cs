using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221014073747847), Tags("Production")]
    public class MC4189AddFacilityUsedInProductionCapacityCalculation : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("UsedInProductionCapacityCalculation").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("UsedInProductionCapacityCalculation").FromTable("tblFacilities");
        }
    }
}

