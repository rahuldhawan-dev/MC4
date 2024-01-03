using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200707164010596), Tags("Production")]
    public class AddLastUpdatedToEquipmentForMC2348 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddColumn("LastUpdated").AsDateTime().Nullable();
            //note: If Equipment.CreatedAt date is null, using  1/1/1990. Unless migration is known. 
            Execute.Sql(
                "UPDATE Equipment SET LastUpdated = COALESCE((Select Top 1 [TimeStamp] from AuditLogEntries ale where ale.EntityId = Equipment.EquipmentID and ale.EntityName = 'Equipment' and (ale.AuditEntryType = 'Update' or ale.AuditEntryType = 'Insert') order by ale.[Timestamp] desc), IsNull(CreatedAt, '1/1/1990' ))");

            Alter.Column("LastUpdated").OnTable("Equipment").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("LastUpdated").FromTable("Equipment");
        }
    }
}
