using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220804105209990), Tags("Production")]
    public class Mc4701AddEmailNotificationForAsLeftConditionNeedsReInspectionSooner : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Production", "Production Work Management", "As Left Condition Needs ReInspection Sooner");
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose("Production", "Production Work Management", "As Left Condition Needs ReInspection Sooner");
        }
    }
}
