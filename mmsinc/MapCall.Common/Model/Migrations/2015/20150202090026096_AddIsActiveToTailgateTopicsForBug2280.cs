using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150202090026096), Tags("Production")]
    public class AddIsActiveToTailgateTopicsForBug2280 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblTailgateTopics")
                 .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("tblTailgateTopics")
                 .AddForeignKeyColumn("CreatedById", "tblPermissions", "RecId");

            Execute.Sql("Update {0} SET IsActive = 1 WHERE Topic <> '' AND Topic IS NOT NULL",
                "tblTailgateTopics");
            Execute.Sql(
                "UPDATE {0} SET CreatedById = AuditLogEntries.UserId FROM AuditLogEntries WHERE AuditLogEntries.EntityId = tblTailgateTopics.TailgateTopicID;",
                "tblTailgateTopics");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblTailgateTopics", "CreatedById", "tblPermissions", "RecId");

            Delete.Column("IsActive").FromTable("tblTailgateTopics");
        }
    }
}
