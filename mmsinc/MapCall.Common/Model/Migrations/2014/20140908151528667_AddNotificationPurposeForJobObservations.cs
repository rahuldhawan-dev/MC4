using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140908151528667), Tags("Production")]
    public class AddNotificationPurposeForJobObservations : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"INSERT INTO NotificationPurposes SELECT TOP 1 ModuleID, 'Job Observation' FROM [Modules] WHERE [name] = 'Health and Safety'");
        }

        public override void Down()
        {
            Execute.Sql(@"
delete from NotificationConfigurations
from NotificationConfigurations nc
inner join NotificationPurposes np on np.NotificationPurposeId = nc.NotificationPurposeId 
where np.ModuleID = (SELECT TOP 1 ModuleId FROM [Modules] WHERE [name] = 'Health and Safety')
and np.Purpose = 'Job Observation'

DELETE FROM NotificationPurposes WHERE ModuleID = (SELECT TOP 1 ModuleId FROM [Modules] WHERE [name] = 'Health and Safety') AND [Purpose] = 'Job Observation'");
        }
    }
}
