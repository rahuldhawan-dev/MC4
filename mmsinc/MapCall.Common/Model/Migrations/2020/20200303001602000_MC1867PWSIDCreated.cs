using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200303001602000), Tags("Production")]
    public class MC1867PWSIDCreated : Migration
    {
        private const string APPLICATION = "Environmental",
                             MODULE = "General",
                             PURPOSE1 = "PWSID Created",
                             PURPOSE2 = "PWSID Status Change";

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
