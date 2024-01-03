using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191017111912671), Tags("Production")]
    public class MC1659AddBusinessUnitToWasteWaterSystem : Migration
    {
        public override void Up()
        {
            Alter.Table("WasteWaterSystems")
                 .AddForeignKeyColumn("BusinessUnitId", "BusinessUnits", "BusinessUnitId", true);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WasteWaterSystems", "BusinessUnitId", "BusinessUnits", "BusinessUnitId");
        }
    }
}
