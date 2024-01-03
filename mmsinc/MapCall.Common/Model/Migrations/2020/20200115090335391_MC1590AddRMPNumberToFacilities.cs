using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200115090335391), Tags("Production")]
    public class MC1590AddRMPNumberToFacilities : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("RMPNumber").AsInt64().Nullable();
            Rename.Column("PSMTCPA").OnTable("tblFacilities").To("PSM");
            Rename.Column("PSM_TCPA").OnTable("tblFacilities").To("RMP");
        }

        public override void Down()
        {
            Delete.Column("RMPNumber").FromTable("tblFacilities");
            Rename.Column("PSM").OnTable("tblFacilities").To("PSMTCPA");
            Rename.Column("RMP").OnTable("tblFacilities").To("PSM_TCPA");
        }
    }
}
