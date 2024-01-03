using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150821164033846), Tags("Production")]
    public class AddViewForAuditLogEntriesForBug2475 : Migration
    {
        public const string DROP_SQL = "DROP VIEW [AuditLogEntryLinkView];";

        public const string CREATE_SQL = @"
CREATE VIEW [AuditLogEntryLinkView] AS
SELECT
    Id,
    EntityId,
    EntityName    
FROM
    AuditLogEntries;
";

        public override void Up()
        {
            Execute.Sql(CREATE_SQL);
        }

        public override void Down()
        {
            Execute.Sql(DROP_SQL);
        }
    }
}
