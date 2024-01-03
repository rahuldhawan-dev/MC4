using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140414132109), Tags("Production")]
    public class AddColumnToTrainingRecordsForBug1798 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblTrainingRecords")
                 .AddColumn("OutsideInstructorTitle")
                 .AsAnsiString(StringLengths.MAX_DEFAULT_VALUE)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("OutsideInstructorTitle").FromTable("tblTrainingRecords");
        }
    }
}
