using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170502084649753), Tags("Production")]
    public class AddAssetTypesToMO10ForBug3714 : Migration
    {
        public override void Up()
        {
            this.AddAssetTypeToOperatingCenter("MO10", "Valve");
            this.AddAssetTypeToOperatingCenter("MO10", "Hydrant");
            this.AddAssetTypeToOperatingCenter("MO10", "Main");
            this.AddAssetTypeToOperatingCenter("MO10", "Service");
            this.AddAssetTypeToOperatingCenter("MO10", "Main Crossing");
        }

        public override void Down()
        {
            this.RemoveAssetTypeFromOperatingCenter("MO10", "Valve");
            this.RemoveAssetTypeFromOperatingCenter("MO10", "Hydrant");
            this.RemoveAssetTypeFromOperatingCenter("MO10", "Main");
            this.RemoveAssetTypeFromOperatingCenter("MO10", "Service");
            this.RemoveAssetTypeFromOperatingCenter("MO10", "Main Crossing");
        }
    }
}
