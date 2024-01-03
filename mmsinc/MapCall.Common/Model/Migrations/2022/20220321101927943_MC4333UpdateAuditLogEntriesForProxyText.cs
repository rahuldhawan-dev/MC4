using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220321101927943), Tags("Production")]
    public class MC4333UpdateAuditLogEntriesForProxyText : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                UPDATE
                     AuditLogEntries
                SET
                    [EntityName] = REPLACE([EntityName], 'ProxyForFieldInterceptor', '')
                WHERE
                    [Timestamp] > '03/20/2022 20:00'");
        }

        public override void Down() { }
    }
}

