using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230811083424373), Tags("Production")]
    public class MC5548_AddEmailNotificationForPitcherFilterNotDelivered : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Field Services", "Work Management", "Pitcher Filter Not Delivered");
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose("Field Services", "Work Management", "Pitcher Filter Not Delivered");
        }
    }
}