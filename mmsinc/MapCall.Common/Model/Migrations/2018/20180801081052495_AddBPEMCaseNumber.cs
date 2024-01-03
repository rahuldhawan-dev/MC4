using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180801081052495), Tags("Production")]
    public class AddBPEMCaseNumber : Migration
    {
        public override void Up()
        {
            Alter.Table("BusinessProcessExceptionManagementCases").AddColumn("CaseNumber")
                 .AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CaseNumber").FromTable("BusinessProcessExceptionManagementCases");
        }
    }
}
