using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220314105337070), Tags("Production")]
    public class MC4305UpdateAuditLogEntriesForProxyText : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                UPDATE
                     AuditLogEntries
                SET
                    [EntityName] = REPLACE([EntityName], 'ProxyForFieldInterceptor', '')
                WHERE
                    [Timestamp] > '03/07/2022 20:00'");
        }

        public override void Down() { }
    }
}

