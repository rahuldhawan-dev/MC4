using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230215111850161), Tags("Production")]
    public class MC_2921_AddNewEmailNotificationsForPressureZonesAndWasteWaterSystemBasins : Migration
    {
        public override void Up()
        {
            this.CreateNotificationPurpose("Environmental", "Water Systems", "Public Water Supply Pressure Zone Created");
            this.CreateNotificationPurpose("Environmental", "Waste Water Systems", "Waste Water System Basin Created");
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose("Environmental", "Waste Water Systems", "Waste Water System Basin Created");
            this.RemoveNotificationPurpose("Environmental", "Water Systems", "Public Water Supply Pressure Zone Created");
        }
    }
}