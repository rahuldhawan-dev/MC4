using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201016141747729), Tags("Production")]
    public class MC2149NewHireEmailNotification : Migration
    {
        private const string APPLICATION = "Human Resources",
                             MODULE = "Employee",
                             PURPOSE = "New Hire Email Notification";

        public override void Up()
        {
            this.AddNotificationType(APPLICATION, MODULE, PURPOSE);
        }

        public override void Down()
        {
            this.RemoveNotificationType(APPLICATION, MODULE, PURPOSE);
        }
    }
}
