using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130409094006), Tags("Production")]
    public class CreateAuditLogEntryTable : Migration
    {
        private const string FOREIGN_KEY = "FK_AuditLogEntries_tblPermissions_UserId";

        public override void Up()
        {
            Create.Table("AuditLogEntries")
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("UserId").AsInt32().NotNullable().ForeignKey(FOREIGN_KEY, "tblPermissions", "RecId")
                  .WithColumn("AuditEntryType").AsString().NotNullable()
                  .WithColumn("EntityName").AsString().NotNullable()
                  .WithColumn("EntityId").AsInt32().NotNullable()
                  .WithColumn("FieldName").AsString().Nullable()
                  .WithColumn("OldValue").AsCustom("text").Nullable()
                  .WithColumn("NewValue").AsCustom("text").Nullable()
                  .WithColumn("Timestamp").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey(FOREIGN_KEY).OnTable("AuditLogEntries");
            Delete.Table("AuditLogEntries");
        }
    }
}
