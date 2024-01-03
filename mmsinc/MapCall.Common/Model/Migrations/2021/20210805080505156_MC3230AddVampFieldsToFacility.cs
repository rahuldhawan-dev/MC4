using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210805080505156), Tags("Production")]
    public class MC3230AddVampFieldsToFacility : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("IsInVamp").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("tblFacilities").AddColumn("VampUrl").AsString(2000).Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsInVamp").FromTable("tblFacilities");
            Delete.Column("VampUrl").FromTable("tblFacilities");
        }
    }
}
