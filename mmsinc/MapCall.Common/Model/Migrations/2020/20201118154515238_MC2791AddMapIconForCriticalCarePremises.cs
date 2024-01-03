using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201118154515238), Tags("Production")]
    public class MC2791AddMapIconForCriticalCarePremises : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("MapIcon")
                  .Row(new {iconURL = "MapIcons/premise-orange.png", width = 24, height = 24, OffsetId = 5});
        }

        public override void Down()
        {
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/premise-orange.png'");
        }
    }
}
