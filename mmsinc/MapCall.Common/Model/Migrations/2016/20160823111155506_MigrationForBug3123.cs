using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160823111155506), Tags("Production")]
    public class MigrationForBug3123 : Migration
    {
        public override void Up()
        {
            // Add columns to incidents
            Create.Column("AnyImmediateCorrectiveActionsApplied").OnTable("Incidents").AsCustom("ntext").Nullable();
            Create.Column("DateInvestigationWillBeCompleted").OnTable("Incidents").AsDateTime().Nullable();

            // Add new notification template
            this.CreateNotificationPurpose("Operations", "Incidents", "HS Incident OSHA Recordable");

            // Copy notifications from one to the other.
            Execute.Sql(@"
declare @hsincident int; set @hsincident = (select NotificationPurposeID from NotificationPurposes where Purpose = 'HS Incident');
declare @osha int; set @osha = (select NotificationPurposeID from NotificationPurposes where Purpose = 'HS Incident OSHA Recordable');

insert into NotificationConfigurations (ContactID, OperatingCenterID, NotificationPurposeID)
select
    ContactID,
    OperatingCenterID,
    @osha as NotificationPurposeID
from NotificationConfigurations where NotificationPurposeID = @hsincident
");
        }

        public override void Down()
        {
            this.DeleteNotificationPurpose("Operations", "Incidents", "HS Incident OSHA Recordable");

            Delete.Column("AnyImmediateCorrectiveActionsApplied").FromTable("Incidents");
            Delete.Column("DateInvestigationWillBeCompleted").FromTable("Incidents");
        }
    }
}
