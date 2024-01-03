using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141229084001538), Tags("Production")]
    public class AddColumnsToSewerManholesForBug2269 : Migration
    {
        public const string TABLE_NAME = "SewerManholes";

        public struct ColumnNames
        {
            public const string ROUTE = "Route", STOP = "Stop";
        }

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(ColumnNames.ROUTE).AsInt32().Nullable()
                 .AddColumn(ColumnNames.STOP).AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.ROUTE).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.STOP).FromTable(TABLE_NAME);
        }
    }
}
