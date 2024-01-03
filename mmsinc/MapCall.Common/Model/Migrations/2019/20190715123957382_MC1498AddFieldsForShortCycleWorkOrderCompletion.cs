using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20190715123957382), Tags("Production")]
    public class MC1498AddFieldsForShortCycleWorkOrderCompletion : Migration
    {
        public const string TABLE_NAME = "ShortCycleWorkOrderCompletions";

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn("FSRInteraction").AsAnsiString(30).Nullable();
            Create.Table("ShortCycleWorkOrderCompletionsQualityIssues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderCompletionId", "ShortCycleWorkOrderCompletions")
                  .WithColumn("QualityIssue").AsAnsiString(3).NotNullable();
            Execute.Sql(
                "INSERT INTO ShortCycleWorkOrderCompletionsQualityIssues SELECT Id, QualityIssue FROM ShortCycleWorkOrderCompletions WHERE isNull(QualityIssue,'') <> '';");
            Delete.Column("QualityIssue").FromTable(TABLE_NAME);
        }

        public override void Down()
        {
            Alter.Table(TABLE_NAME).AddColumn("QualityIssue").AsAnsiString(3).Nullable();
            //We will lose data rolling this back, this will grab the first existing value and retain it.
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletions SET QualityIssue = (SELECT TOP 1 QualityIssue FROM ShortCycleWorkOrderCompletionsQualityIssues WHERE ShortCycleWorkOrderCompletionId = ShortCycleWorkOrderCompletions.Id)");
            Delete.Table("ShortCycleWorkOrderCompletionsQualityIssues");
            Delete.Column("FSRInteraction").FromTable(TABLE_NAME);
        }
    }
}
