using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170824114823258), Tags("Production")]
    public class AddActiveColumnToSpoilStorageLocationsAndTownSectionsForBug3941 : Migration
    {
        public override void Up()
        {
            Alter.Table("SpoilStorageLocations").AddColumn("Active").AsBoolean().NotNullable()
                 .WithDefaultValue(true);

            Alter.Table("TownSections").AddColumn("Active").AsBoolean().NotNullable()
                 .WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("Active").FromTable("SpoilStorageLocations");
            Delete.Column("Active").FromTable("TownSections");
        }
    }
}
