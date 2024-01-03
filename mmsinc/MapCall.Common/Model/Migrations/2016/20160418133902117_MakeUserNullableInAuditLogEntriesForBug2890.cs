using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160418133902117), Tags("Production")]
    public class MakeUserNullableInAuditLogEntriesForBug2890 : Migration
    {
        public override void Up()
        {
            Alter.Column("UserId")
                 .OnTable("AuditLogEntries")
                 .AsInt32().Nullable();
        }

        public override void Down()
        {
            Alter.Column("UserId")
                 .OnTable("AuditLogEntries")
                 .AsInt32().NotNullable();
        }
    }
}
