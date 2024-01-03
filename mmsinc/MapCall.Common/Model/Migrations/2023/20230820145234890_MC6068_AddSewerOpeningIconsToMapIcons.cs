using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230820145234890), Tags("Production")]
    public class MC6068_AddSewerOpeningIconsToMapIcons : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-gray.png', 24, 24, 5);" 
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-gray.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-green.png', 24, 24, 5);" 
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-green.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-purple.png', 24, 24, 5);" 
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-purple.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-red.png', 24, 24, 5);" 
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-red.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-blue.png', 24, 24, 5);" 
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-blue.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-pink.png', 24, 24, 5);"
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-pink.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-yellow.png', 24, 24, 5);"
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-yellow.png'), 2;");

            Execute.Sql($"INSERT INTO MapIcon (iconURL, width, height, OffsetId) VALUES ('MapIcons/seweropening-orange.png', 24, 24, 5);"
                        + "INSERT INTO MapIconIconSets select (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-orange.png'), 2;");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-orange.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-orange.png';");
           
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-yellow.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-yellow.png';");

            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-pink.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-pink.png';");
          
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-blue.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-blue.png';");

            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-red.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-red.png';");
            
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-purple.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-purple.png';");
            
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-green.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-green.png';");
            
            Execute.Sql(
                "DELETE FROM MapIconIconSets where IconId = (select iconId from MapIcon where iconUrl = 'MapIcons/seweropening-gray.png')");
            Execute.Sql($"DELETE FROM MapIcon WHERE iconURL = 'MapIcons/seweropening-gray.png';");
        }
    }
}

