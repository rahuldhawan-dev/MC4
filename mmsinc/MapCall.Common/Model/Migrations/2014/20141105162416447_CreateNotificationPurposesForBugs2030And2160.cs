using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141105162416447), Tags("Production")]
    public class CreateNotificationPurposesForBugs2030And2160 : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Human Resources", "Employee", "Drivers License Renewal Due");
        }

        public override void Down()
        {
            this.DeleteNotificationPurpose("Human Resources", "Employee", "Drivers License Renewal Due");
        }
    }
}
