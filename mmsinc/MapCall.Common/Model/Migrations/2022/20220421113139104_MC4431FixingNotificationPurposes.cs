using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220421113139104), Tags("Production")]
    public class MC4431FixingNotificationPurposes : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update NotificationPurposes set Purpose = 'Facilities SAPErrorCode Occurred'
                          Where ModuleId = 29 and Purpose = 'SAPErrorCode Occurred'
                          update NotificationPurposes set Purpose = 'Assets SAPErrorCode Occurred'
                          Where ModuleId = 73 and Purpose = 'SAPErrorCode Occurred'");
        }

        public override void Down()
        {
            Execute.Sql(@"update NotificationPurposes set Purpose = 'SAPErrorCode Occurred'
                          Where ModuleId = 29 and Purpose = 'Facilities SAPErrorCode Occurred'
                          update NotificationPurposes set Purpose = 'SAPErrorCode Occurred'
                          Where ModuleId = 73 and Purpose = 'Assets SAPErrorCode Occurred'");
        }
    }
}

