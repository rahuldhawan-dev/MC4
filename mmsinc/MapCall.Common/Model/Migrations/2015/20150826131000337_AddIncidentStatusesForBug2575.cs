using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150826131000337), Tags("Production")]
    public class AddIncidentStatusesForBug2575 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("IncidentStatuses", "Open", "Closed-Denied",
                "Closed-Administratively Complete");
            Alter.Table("Incidents").AddForeignKeyColumn("IncidentStatusId", "IncidentStatuses").Nullable();
            Execute.Sql("UPDATE Incidents Set IncidentStatusId = 1");
            Alter.Column("IncidentStatusId").OnTable("Incidents").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Incidents", "IncidentStatusId", "IncidentStatuses");
            Delete.Table("IncidentStatuses");
        }
    }
}
