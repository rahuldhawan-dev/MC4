using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170403104303972), Tags("Production")]
    public class ValveIconsBug3677 : Migration
    {
        public override void Up()
        {
            Execute.Sql("insert into MapIcon Values('MapIcons/valve-orangeblack.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/valve-orangeblack.png'), 2;");
            Execute.Sql("insert into MapIcon Values('MapIcons/valve-orangewhite.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/valve-orangewhite.png'), 2;");

            Execute.Sql("insert into MapIcon Values('MapIcons/valve-purpleblack.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/valve-purpleblack.png'), 2;");
            Execute.Sql("insert into MapIcon Values('MapIcons/valve-purplewhite.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/valve-purplewhite.png'), 2;");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/valve-orangeblack.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/valve-orangeblack.png'");
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/valve-orangewhite.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/valve-orangewhite.png'");

            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/valve-purpleblack.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/valve-purpleblack.png'");
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/valve-purplewhite.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/valve-purplewhite.png'");
        }
    }
}
