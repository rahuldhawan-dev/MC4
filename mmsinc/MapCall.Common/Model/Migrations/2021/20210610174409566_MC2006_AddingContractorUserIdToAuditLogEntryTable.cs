using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210610174409566), Tags("Production")]
    public class MC2006_AddingContractorUserIdToAuditLogEntryTable : Migration
    {
        public override void Up()
        {
            Alter.Table("AuditLogEntries").AddForeignKeyColumn("ContractorUserId", "ContractorUsers", foreignId: "ContractorUserID", nullable: true);
        }

        public override void Down()
        {
            Delete.Column("ContractorUserId").FromTable("AuditLogEntries");
        }
    }
}

