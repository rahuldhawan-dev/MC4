using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220201142023041), Tags("Production")]
    public class MC3019CreateEnvironmentalRoles : Migration
    {
        public override void Up()
        {
            this.CreateModule("Water Systems", "Environmental", 97);
            this.CreateModule("Waste Water Systems", "Environmental", 98);

            this.ReassignNotificationPurpose(58, "PWSID Created", 97);
            this.ReassignNotificationPurpose(58, "PWSID Status Change", 97);
            this.ReassignNotificationPurpose(58, "Waste Water System Created", 98);
            this.ReassignNotificationPurpose(58, "Waste Water System Updated", 98);
        }

        public override void Down()
        {
            this.ReassignNotificationPurpose(98, "Waste Water System Updated", 58);
            this.ReassignNotificationPurpose(98, "Waste Water System Created", 58);
            this.ReassignNotificationPurpose(97, "PWSID Status Change", 58);
            this.ReassignNotificationPurpose(97, "PWSID Created", 58);
            
            this.DeleteModuleAndAssociatedRoles("Environmental", "Water Systems");
            this.DeleteModuleAndAssociatedRoles("Environmental", "Waste Water Systems");
        }
    }
}

