using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201218185520954), Tags("Production")]
    public class MC2633AddMapIconForMajorAccountPremises : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("MapIcon")
                  .Row(new {iconURL = "MapIcons/premise-mablue.png", width = 24, height = 24, OffsetId = 5})
                  .Row(new {iconURL = "MapIcons/premise-blueorange.png", width = 24, height = 24, OffsetId = 5});
        }

        public override void Down()
        {
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/premise-blueorange.png'");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/premise-mablue.png'");
        }
    }
}
