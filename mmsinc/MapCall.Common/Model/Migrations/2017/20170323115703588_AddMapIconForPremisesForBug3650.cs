using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170323115703588), Tags("Production")]
    public class AddMapIconForPremisesForBug3650 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("MapIcon")
                  .Row(new {iconURL = "MapIcons/premise-blue.png", width = 24, height = 24, OffsetId = 5});
        }

        public override void Down()
        {
            Execute.Sql(
                "delete from MapIconIconSets where IconId = (select iconID from MapIcon where IconUrl = 'MapIcons/premise-blue.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/premise-blue.png'");
        }
    }
}
