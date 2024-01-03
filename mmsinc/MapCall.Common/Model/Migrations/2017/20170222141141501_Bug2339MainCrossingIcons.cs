using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170222141141501), Tags("Production")]
    public class Bug2339MainCrossingIcons : Migration
    {
        public override void Up()
        {
            Execute.Sql("insert into MapIcon Values('MapIcons/maincrossing-red.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/maincrossing-red.png'), 2;");
            Execute.Sql("insert into MapIcon Values('MapIcons/maincrossing-green.png', 24, 24, 5);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/maincrossing-green.png'), 2;");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/maincrossing-red.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/maincrossing-red.png'");
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/maincrossing-green.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/maincrossing-green.png'");
        }
    }
}
