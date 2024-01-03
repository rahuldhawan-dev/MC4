using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230405134000001), Tags("Production")]
    public class MC5570_AddArcFlashLabelRequiredToFacility : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("ArcFlashLabelRequired").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("ArcFlashLabelRequired").FromTable("tblFacilities");
        }
    }
}