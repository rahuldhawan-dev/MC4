namespace RegressionTests.Lib.TestParts
{
    public static class Globals
    {
        #region Constants

        public const string HOME_URL = "/Modules/HR/home.aspx";

        public const string MCPROD_CI_CONNECTION_STRING = "Data Source=127.0.0.1;uid=mapcalldevuser;password=M2pC@llD3vUs3r!;Initial Catalog=mapcalldev;Integrated Security=false;",
                            MCPROD_CONNECTION_STRING = "Data Source=127.0.0.1;uid=mapcalldevuser;password=M2pC@llD3vUs3r!;Initial Catalog=mapcalldev;Integrated Security=false;";

        public struct NecessaryIDs
        {
            public const string OLD_WORLD_MAPCALL = "link=MapCall™",
                                OLD_WORLD_CONTENTS_FRAME = "contents",
                                OLD_WORLD_MAIN_FRAME = "main",
                                OLD_WORLD_WATER_LEFT_FRAME = "waterLeft",
                                OLD_WORLD_WATER_RIGHT_FRAME = "_waterright",
                                RELATIVE_UP_FRAME = "relative=up",
                                RELATIVE_TOP_FRAME = "relative=top";

            public const string FIELD_OPERATIONS_LINK = "link=Field Operations",
                                IMAGES_LINK = "link=Images",
                                OLD_WORLD_DISTRIBUTION_LINK = "//a[@id='McMenu1_Hyperlink1']/img";

        }

        public struct NecessaryMenus
        {
            public const string MENU_FIELD_SERVICES = "link=Field Operations",
                                MENU_STORM_WATER_ASSETS = "link=Storm Water Assets",
                                MENU_FLEET_COMMUNICATIONS = "link=Fleet & Communications",
                                MENU_VEHICLES = "link=Vehicles",
                                MENU_WATER_QUALITY = "link=Water Quality",
                                MENU_WQ_COMPLAINTS = "link=WQ Complaints";

        }

        #endregion

        public static string GetConnectionString()
        {
            return (System.Environment.MachineName.StartsWith("FATMAN"))
                ? MCPROD_CI_CONNECTION_STRING
                : MCPROD_CONNECTION_STRING;
        }
    }
}
