using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181001102336721), Tags("Production")]
    public class AdjustColumnsForWork1V : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("QualityIssue")
                 .AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("QualityIssue").FromTable("ShortCycleWorkOrderCompletions");
        }
    }
}
