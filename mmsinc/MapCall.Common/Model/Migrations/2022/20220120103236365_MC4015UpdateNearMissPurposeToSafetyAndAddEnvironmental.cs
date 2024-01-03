using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220120103236365), Tags("Production")]
    public class MC4015UpdateNearMissPurposeToSafetyAndAddEnvironmental : Migration
    {
        private string sqlUpdatePurposeUp = @"UPDATE NotificationPurposes
                                 SET [Purpose] = 'Safety Near Miss'
                                 WHERE Purpose = 'Near Miss Notification'";

        private string sqlUpdatePurposeDown = @"UPDATE NotificationPurposes
                                SET[Purpose] = 'Near Miss Notification'
                                WHERE Purpose = 'Safety Near Miss'";

        public override void Up()
        {
            Execute.Sql(sqlUpdatePurposeUp); 
            this.AddNotificationType("Operations", "Health And Safety", "Environmental Near Miss");
        }

        public override void Down()
        {
            Execute.Sql(sqlUpdatePurposeDown);
            this.RemoveNotificationPurpose("Operations", "Health And Safety", "Environmental Near Miss");
        }
    }
}

