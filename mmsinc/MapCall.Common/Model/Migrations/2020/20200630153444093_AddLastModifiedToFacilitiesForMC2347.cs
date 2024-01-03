using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200630153444093), Tags("Production")]
    public class AddLastModifiedToFacilitiesForMC2347 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities")
                 .AddColumn("LastUpdated").AsDateTime().Nullable();

            Execute.Sql(
                "UPDATE tblFacilities SET LastUpdated = COALESCE((Select Top 1 [TimeStamp] from AuditLogEntries ale where ale.EntityId = tblFacilities.RecordId and ale.EntityName = 'Facility' and (ale.AuditEntryType = 'Update' or ale.AuditEntryType = 'Insert') order by ale.[Timestamp] desc), DateCreated)");

            Alter.Column("LastUpdated").OnTable("tblFacilities").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("LastUpdated").FromTable("tblFacilities");
        }
    }
}
