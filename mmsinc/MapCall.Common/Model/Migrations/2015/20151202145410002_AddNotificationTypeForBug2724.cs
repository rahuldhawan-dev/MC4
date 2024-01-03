using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151202145410002), Tags("Production")]
    public class AddNotificationTypeForBug2724 : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Field Services", "Work Management", "Traffic Control Ticket Entered");
        }

        public override void Down()
        {
            this.DeleteNotificationPurpose("Field Services", "Work Management", "Traffic Control Ticket Entered");
        }
    }
}
