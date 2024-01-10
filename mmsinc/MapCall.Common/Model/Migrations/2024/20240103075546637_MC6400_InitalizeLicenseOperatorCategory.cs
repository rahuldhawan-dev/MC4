using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2024
{
    [Migration(20240103075546637), Tags("Production")]
    public class MC6400_InitalizeLicenseOperatorCategory : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"
                UPDATE PublicWaterSupplies
                SET LicensedOperatorStatusId = 1;
                UPDATE WasteWaterSystems
                SET LicensedOperatorStatusId = 1;");
        }

        public override void Down()
        {
            // Cannot reverse as we don't know what they were before - this is a one-time initialization
        }
    }
}

