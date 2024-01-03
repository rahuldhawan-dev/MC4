using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231113100900001), Tags("Production")]
    public class MC6543_AddChemicalStorageLocationToFacilityTable : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("ChemicalStorageLocationId", "ChemicalStorageLocations")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "ChemicalStorageLocationId", "ChemicalStorageLocations");
        }
    }
}

