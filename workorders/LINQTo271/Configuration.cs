
namespace LINQTo271
{
    public class Configuration
    {
        public const string MENU_URL = "~/Default.aspx";
        public const string DEFAULT_OPCODE = "NJ4";

        public struct ConnectionStrings
        {
            #if DEBUG
            public const string WORK_ORDERS_TEST = "Data Source=localhost;Initial Catalog=mapcall;User Id=sa;Password=mmsisa#1";
            #else
            public const string WORK_ORDERS_TEST = "Data Source=stageburysql;Initial Catalog=McProd;User Id=mcuser;Password=3v3ry0n3";
            #endif
        }
    }
}
