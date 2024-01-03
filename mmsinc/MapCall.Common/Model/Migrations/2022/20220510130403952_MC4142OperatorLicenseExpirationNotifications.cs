using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220510130403952), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4142OperatorLicenseExpirationNotifications : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Human Resources", "Employee", "Operator License Expiration");
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose("Human Resources", "Employee", "Operator License Expiration");
        }
    }
}

