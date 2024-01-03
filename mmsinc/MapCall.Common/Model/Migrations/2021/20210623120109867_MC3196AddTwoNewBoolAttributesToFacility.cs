using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210623120109867), Tags("Production")]
    public class MC3196AddTwoNewBoolAttributesToFacility : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("Radionuclides").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("tblFacilities").AddColumn("CustomerRightToKnow").AsBoolean().Nullable().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("Radionuclides").FromTable("tblFacilities");
            Delete.Column("CustomerRightToKnow").FromTable("tblFacilities");
        }
    }
}

