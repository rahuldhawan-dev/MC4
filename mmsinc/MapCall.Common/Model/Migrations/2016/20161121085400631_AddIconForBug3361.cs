using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161121085400631), Tags("Production")]
    public class AddIconForBug3361 : Migration
    {
        public override void Up()
        {
            Execute.Sql("insert into MapIcon Values('MapIcons/beaker_red.png', 29, 32, 2);" +
                        "insert into MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/beaker_red.png'), 13;");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/beaker_red.png')");
            Execute.Sql("DELETE FROM MapIcon where iconUrl = 'MapIcons/beaker_red.png'");
        }
    }
}
