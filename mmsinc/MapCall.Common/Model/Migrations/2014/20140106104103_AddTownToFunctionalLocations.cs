using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140106104103), Tags("Production")]
    public class AddTownToFunctionalLocations : Migration
    {
        public struct Tables
        {
            public const string FUNCTIONAL_LOCATIONS = "FunctionalLocations",
                                TOWNS = "Towns",
                                ASSET_TYPES = "AssetTypes";
        }

        public struct Columns
        {
            public const string TOWN_ID = "TownID",
                                ASSET_TYPE_ID = "AssetTypeID";
        }

        public override void Up()
        {
            Alter.Table(Tables.FUNCTIONAL_LOCATIONS).AddColumn(Columns.TOWN_ID).AsInt32().Nullable();
            Alter.Table(Tables.FUNCTIONAL_LOCATIONS).AddColumn(Columns.ASSET_TYPE_ID).AsInt32().Nullable();
            Create.ForeignKey("FK_FunctionalLocations_Towns_TownID")
                  .FromTable(Tables.FUNCTIONAL_LOCATIONS).ForeignColumn(Columns.TOWN_ID)
                  .ToTable(Tables.TOWNS).PrimaryColumn(Columns.TOWN_ID);
            Create.ForeignKey("FK_FunctionalLocations_AssetTypes_AssetTypeID")
                  .FromTable(Tables.FUNCTIONAL_LOCATIONS).ForeignColumn(Columns.ASSET_TYPE_ID)
                  .ToTable(Tables.ASSET_TYPES).PrimaryColumn(Columns.ASSET_TYPE_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_FunctionalLocations_Towns_TownID").OnTable(Tables.FUNCTIONAL_LOCATIONS);
            Delete.ForeignKey("FK_FunctionalLocations_AssetTypes_AssetTypeID").OnTable(Tables.FUNCTIONAL_LOCATIONS);
            Delete.Column(Columns.TOWN_ID).FromTable(Tables.FUNCTIONAL_LOCATIONS);
            Delete.Column(Columns.ASSET_TYPE_ID).FromTable(Tables.FUNCTIONAL_LOCATIONS);
        }
    }
}
