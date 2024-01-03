using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210114110352237), Tags("Production")]
    public class MC2881AddingOwnershipAndTypeForPws : Migration
    {
        private struct Tables
        {
            public const string PUBLIC_WATER_SUPPLIES = "PublicWaterSupplies",
                                PUBLIC_WATER_SUPPLIES_OWNERSHIP = "PublicWaterSuppliesOwnerships",
                                PUBLIC_WATER_SUPPLIES_TYPE = "PublicWaterSuppliesTypes";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(Tables.PUBLIC_WATER_SUPPLIES_OWNERSHIP, "AW Owned/Operated", "AW Contract",
                "MSG", "CSG", "Other Purveyor");
            this.CreateLookupTableWithValues(Tables.PUBLIC_WATER_SUPPLIES_TYPE, "Community Water System",
                "Non-Transient Non-Community Water System", "Transient Non-Community Water System");
            Alter.Table(Tables.PUBLIC_WATER_SUPPLIES)
                 .AddForeignKeyColumn("OwnershipId", Tables.PUBLIC_WATER_SUPPLIES_OWNERSHIP).Nullable()
                 .AddForeignKeyColumn("TypeId", Tables.PUBLIC_WATER_SUPPLIES_TYPE).Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(Tables.PUBLIC_WATER_SUPPLIES, "OwnershipId",
                Tables.PUBLIC_WATER_SUPPLIES_OWNERSHIP);
            Delete.ForeignKeyColumn(Tables.PUBLIC_WATER_SUPPLIES, "TypeId", Tables.PUBLIC_WATER_SUPPLIES_TYPE);
        }
    }
}
