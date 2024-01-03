using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190910111802879), Tags("Production")]
    public class RenameMarkoutViolationDateOfViolationNoticeForMC906 : Migration
    {
        public override void Up()
        {
            Rename.Column("DateOfViolation").OnTable("MarkoutViolations").To("DateOfViolationNotice");
        }

        public override void Down()
        {
            Rename.Column("DateOfViolationNotice").OnTable("MarkoutViolations").To("DateOfViolation");
        }
    }
}
