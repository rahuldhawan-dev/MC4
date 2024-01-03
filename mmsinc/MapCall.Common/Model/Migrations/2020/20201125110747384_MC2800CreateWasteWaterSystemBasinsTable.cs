using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201125110747384), Tags("Production")]
    public class MC2800CreateWasteWaterSystemBasinsTable : Migration
    {
        #region Constants

        public const int MAX_NAME_LENGTH = 100;
        private const string WASTE_WATER_SYSTEM_BASINS = "WasteWaterSystemBasins";

        #endregion

        public override void Up()
        {
            Create.Table(WASTE_WATER_SYSTEM_BASINS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems", nullable: false)
                  .WithColumn("BasinName").AsString(MAX_NAME_LENGTH).NotNullable()
                  .WithColumn("FirmCapacity").AsDecimal(6, 3).Nullable()
                  .WithColumn("FirmCapacityUnderStandbyPower").AsDecimal(6, 3).Nullable()
                  .WithColumn("FirmCapacityDateUpdated").AsDate();
            this.AddDataType(WASTE_WATER_SYSTEM_BASINS);
        }

        public override void Down()
        {
            Delete.Table(WASTE_WATER_SYSTEM_BASINS);
            this.RemoveDataType(WASTE_WATER_SYSTEM_BASINS);
        }
    }
}
