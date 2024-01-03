using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210303102425187), Tags("Production")]
    public class MC2086AddNotificationPurposeForSystemDeliveryEntryEmails : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Production", "System Delivery Approver", "System Delivery Entry Validation");
            this.AddNotificationType("Production", "System Delivery Entry", "System Delivery Entry Adjustment");
            this.AddNotificationType("Production", "System Delivery Entry", "System Delivery Entry Due");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Production", "System Delivery Approver", "System Delivery Entry Validation");
            this.RemoveNotificationType("Production", "System Delivery Entry", "System Delivery Entry Adjustment");
            this.RemoveNotificationType("Production", "System Delivery Entry", "System Delivery Entry Due");
        }
    }
}

