using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220816108108108)]
    [Tags("Production")]
    public class Mc4700AddEmailNotificationForAsLeftConditionNeedsEmergencyRepair : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Production", "Production Work Management",
                "As Left Condition Needs Emergency Repair");
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose("Production", "Production Work Management",
                "As Left Condition Needs Emergency Repair");
        }
    }
}