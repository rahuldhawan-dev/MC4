using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220616135139067), Tags("Production")]
    public class MC4465AddWaterStressFieldToFacilities : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("WaterStress").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("WaterStress").FromTable("tblFacilities");
        }
    }
}

