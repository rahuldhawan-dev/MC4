using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230228104534743), Tags("Production")]
    public class MC5385_AddMapIcons : Migration
    {
        public override void Up()
        {
            Execute.Sql("insert into MapIcon Values('MapIcons/hydrant-orangeblack.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/hydrant-orangeblack.png'), 2;");
            Execute.Sql("insert into MapIcon Values('MapIcons/hydrant-orangewhite.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/hydrant-orangewhite.png'), 2;");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/hydrant-orangeblack.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/hydrant-orangeblack.png'");
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/hydrant-orangewhite.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/hydrant-orangewhite.png'");
        }
    }
}

