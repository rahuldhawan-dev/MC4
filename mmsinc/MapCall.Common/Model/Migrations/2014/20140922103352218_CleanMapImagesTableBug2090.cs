using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140922103352218), Tags("Production")]
    public class CleanMapImagesTableBug2090 : Migration
    {
        public const string MAP_IMAGE = "MapImages";

        private void CleanUpBadStringData()
        {
            Execute.Sql(@"update [MapImages] set TownSection = null where TownSection = ''");
        }

        public override void Up()
        {
            // F8Label is a duplicate of DistrictID
            Delete.Column("F8Label").FromTable(MAP_IMAGE);

            // F9Label is a duplicate of County
            Delete.Column("F9Label").FromTable(MAP_IMAGE);

            // And we're removing County anyway.
            Delete.Column("County").FromTable(MAP_IMAGE);

            // TownID is fully populated
            Delete.Column("Town").FromTable(MAP_IMAGE);

            // Can get through town.
            Delete.Column("DistrictID").FromTable(MAP_IMAGE);

            // Can be gotten through Town.County.State
            Delete.Column("State").FromTable(MAP_IMAGE);

            // All rows have null values 
            Delete.Column("GeoGrid").FromTable(MAP_IMAGE);

            // Not used anymore
            Delete.Column("OldMapID").FromTable(MAP_IMAGE);

            // All nulls
            Delete.Column("DateAdded").FromTable(MAP_IMAGE);

            Alter.Column("TownID").OnTable(MAP_IMAGE)
                 .AsInt32().NotNullable();
            Alter.Column("fld").OnTable(MAP_IMAGE)
                 .AsString(30).NotNullable();
            Alter.Column("FileList").OnTable(MAP_IMAGE)
                 .AsString(50).NotNullable();

            CleanUpBadStringData();
        }

        public override void Down()
        {
            Alter.Table(MAP_IMAGE).AddColumn("F8Label").AsString(50).Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("F9Label").AsString(50).Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("County").AsString(50).Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("Town").AsString(50).Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("State").AsString(50).Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("GeoGrid").AsString(50).Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("OldMapID").AsInt32().Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("DistrictID").AsInt32().Nullable();
            Alter.Table(MAP_IMAGE).AddColumn("DateAdded").AsDateTime().Nullable();

            Alter.Column("TownID").OnTable(MAP_IMAGE)
                 .AsInt32().Nullable();
            Alter.Column("fld").OnTable(MAP_IMAGE)
                 .AsString(30).Nullable();
            Alter.Column("FileList").OnTable(MAP_IMAGE)
                 .AsString(50).Nullable();
            Alter.Column("DistrictID").OnTable(MAP_IMAGE)
                 .AsInt32().Nullable();
        }
    }
}
