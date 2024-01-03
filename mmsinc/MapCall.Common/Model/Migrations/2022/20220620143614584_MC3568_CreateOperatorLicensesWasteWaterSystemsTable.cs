using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220620143614584), Tags("Production")]
    public class MC3568_CreateOperatorLicensesWasteWaterSystemsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("OperatorLicensesWasteWaterSystems")
                  .WithForeignKeyColumn("OperatorLicenseId", "OperatorLicenses", nullable: false)
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems", nullable: false);
        }
    }
}

