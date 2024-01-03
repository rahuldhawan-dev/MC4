using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200609165119097), Tags("Production")]
    public class MC2324HydrantAndBlowOffNotification : Migration
    {
        private const string APPLICATION = "Field Services",
                             MODULE = "Assets",
                             PURPOSE1 = "B O Chlorine Reading Outside Expected Limit",
                             PURPOSE2 = "Hydrant Chlorine Reading Outside Expected Limit";

        public override void Up()
        {
            this.AddNotificationType(APPLICATION, MODULE, PURPOSE1);
            this.AddNotificationType(APPLICATION, MODULE, PURPOSE2);
        }

        public override void Down()
        {
            this.RemoveNotificationType(APPLICATION, MODULE, PURPOSE1);
            this.RemoveNotificationType(APPLICATION, MODULE, PURPOSE2);
        }
    }
}
