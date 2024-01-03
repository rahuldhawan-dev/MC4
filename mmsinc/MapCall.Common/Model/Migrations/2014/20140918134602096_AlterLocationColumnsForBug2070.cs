using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140918134602096), Tags("Production")]
    public class AlterLocationColumnsForBug2070 : Migration
    {
        public struct TableNames
        {
            public const string VALVES = "tblNJAWValves",
                                HYDRANTS = "tblNJAWHydrant";
        }

        public struct ColumnNames
        {
            public const string VALVE_LOCATION = "ValLoc";
        }

        public struct StringLengths
        {
            public const int VALVE_LOCATION_NEW = 150,
                             VALVE_LOCATION_OLD = 40;
        }

        public override void Up()
        {
            Alter.Table(TableNames.HYDRANTS)
                 .AlterColumn(ColumnNames.VALVE_LOCATION)
                 .AsAnsiString(StringLengths.VALVE_LOCATION_NEW)
                 .Nullable();
        }

        public override void Down()
        {
            // no need for a down
        }
    }
}
