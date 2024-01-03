using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151204154250317), Tags("Production")]
    public class AddNotificationTypeForBug2297 : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Field Services", "Work Management", "Main Break Completed");
        }

        public override void Down()
        {
            this.DeleteNotificationPurpose("Field Services", "Work Management", "Main Break Completed");
        }
    }
}
