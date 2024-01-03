using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131023093629), Tags("Production")]
    public class AddTownsPublicWaterSuppliesForBug1588 : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string TOWNS = "Towns",
                                PUBLIC_WATER_SUPPLIES = "tblPWSID",
                                PUBLIC_WATER_SUPPLIES_TOWNS = "PublicWaterSuppliesTowns";
        }

        public struct Columns
        {
            public const string TOWN_ID = "TownID",
                                PUBLIC_WATER_SUPPLY_ID_ACTUAL = "RecordId",
                                PUBLIC_WATER_SUPPLY_ID = "PublicWaterSupplyID";
        }

        public struct ForeignKeys
        {
            public const string FK_PUBLIC_WATER_SUPPLIES_TOWNS_TOWN_ID =
                                    "FK_" + Tables.PUBLIC_WATER_SUPPLIES_TOWNS + "_" + Tables.TOWNS + "_" +
                                    Columns.TOWN_ID,
                                FK_PUBLIC_WATER_SUPPLIES_TOWNS_PUBLIC_WATER_SUPPLY_ID =
                                    "FK_" + Tables.PUBLIC_WATER_SUPPLIES_TOWNS + "_" + Tables.PUBLIC_WATER_SUPPLIES +
                                    "_" + Columns.PUBLIC_WATER_SUPPLY_ID;
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.PUBLIC_WATER_SUPPLIES_TOWNS)
                  .WithColumn(Columns.PUBLIC_WATER_SUPPLY_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.TOWN_ID).AsInt32().NotNullable();

            Create.ForeignKey(ForeignKeys.FK_PUBLIC_WATER_SUPPLIES_TOWNS_PUBLIC_WATER_SUPPLY_ID)
                  .FromTable(Tables.PUBLIC_WATER_SUPPLIES_TOWNS).ForeignColumn(Columns.PUBLIC_WATER_SUPPLY_ID)
                  .ToTable(Tables.PUBLIC_WATER_SUPPLIES).PrimaryColumn(Columns.PUBLIC_WATER_SUPPLY_ID_ACTUAL);
            Create.ForeignKey(ForeignKeys.FK_PUBLIC_WATER_SUPPLIES_TOWNS_TOWN_ID)
                  .FromTable(Tables.PUBLIC_WATER_SUPPLIES_TOWNS).ForeignColumn(Columns.TOWN_ID)
                  .ToTable(Tables.TOWNS).PrimaryColumn(Columns.TOWN_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_PUBLIC_WATER_SUPPLIES_TOWNS_PUBLIC_WATER_SUPPLY_ID)
                  .OnTable(Tables.PUBLIC_WATER_SUPPLIES_TOWNS);
            Delete.ForeignKey(ForeignKeys.FK_PUBLIC_WATER_SUPPLIES_TOWNS_TOWN_ID)
                  .OnTable(Tables.PUBLIC_WATER_SUPPLIES_TOWNS);

            Delete.Table(Tables.PUBLIC_WATER_SUPPLIES_TOWNS);
        }
    }
}
