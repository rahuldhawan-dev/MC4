using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418792), Tags("Production")]
    public class NormalizeShortCycleWorkOrdersEquipmentIdsForMC1803 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "UPDATE ShortCycleWorkOrdersEquipmentIds SET ServiceType = NULL WHERE LTRIM(RTRIM(ServiceType)) = '';");

            this.NormalizeToExistingTable("ShortCycleWorkOrdersEquipmentIds", "ServiceType",
                "ShortCycleWorkOrderServiceTypes", newColumn: "ServiceTypeId");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrdersEquipmentIds", "InstallationType",
                "ShortCycleWorkOrderEquipmentInstallationTypes", 4, newColumnName: "InstallationTypeId");
        }

        public override void Down()
        {
            this.DeNormalizeFromExistingTable("ShortCycleWorkOrdersEquipmentIds", "ServiceType",
                "ShortCycleWorkOrderServiceTypes", 2, newColumn: "ServiceTypeId");

            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrdersEquipmentIds", "InstallationType",
                "ShortCycleWorkOrderEquipmentInstallationTypes", 4, newColumnName: "InstallationTypeId");
        }
    }
}
