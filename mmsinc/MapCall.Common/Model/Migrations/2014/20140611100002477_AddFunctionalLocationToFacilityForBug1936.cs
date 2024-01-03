using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140611100002477), Tags("Production")]
    public class AddFunctionalLocationToFacilityForBug1936 : Migration
    {
        public const string TABLE_NAME = "tblFacilities";
        public const string COLUMN_NAME = "FunctionalLocationId";

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(COLUMN_NAME)
                 .AsInt32()
                 .ForeignKey("FK_tblFacilities_FunctionalLocations_FunctionalLocationId",
                      "FunctionalLocations", "FunctionalLocationId")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblFacilities_FunctionalLocations_FunctionalLocationId").OnTable(TABLE_NAME);
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
