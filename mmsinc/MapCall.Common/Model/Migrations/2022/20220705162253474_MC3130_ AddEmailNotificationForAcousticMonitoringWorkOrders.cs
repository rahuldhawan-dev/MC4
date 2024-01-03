using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220705162253474), Tags("Production")]
    public class MC3130_AddEmailNotificationForAcousticMonitoringWorkOrders : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Field Services", "Work Management", "Acoustic Monitoring Order Created");
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose("Field Services", "Work Management", "Acoustic Monitoring Order Created");
        }
    }
}

