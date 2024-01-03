using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200131085959021), Tags("Production")]
    public class MC1590FacilityColumnsNowNotNullableToMatchWithEntity : Migration
    {
        public override void Up()
        {
            Alter.Table("tblfacilities").AlterColumn("PSM").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("tblfacilities").AlterColumn("RMP").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Alter.Table("tblfacilities").AlterColumn("PSM").AsBoolean().Nullable();
            Alter.Table("tblfacilities").AlterColumn("RMP").AsBoolean().Nullable();
        }
    }
}
