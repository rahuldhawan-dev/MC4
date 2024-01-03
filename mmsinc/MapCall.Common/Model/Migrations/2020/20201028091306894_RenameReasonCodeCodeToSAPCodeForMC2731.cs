using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201028091306894), Tags("Production")]
    public class RenameReasonCodeCodeToSAPCodeForMC2731 : Migration
    {
        public override void Up()
        {
            Rename.Column("Code").OnTable("ShortCycleReasonCodes").To("SAPCode");
        }

        public override void Down()
        {
            Rename.Column("SAPCode").OnTable("ShortCycleReasonCodes").To("Code");
        }
    }
}
