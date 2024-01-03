using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140806111635471), Tags("Production")]
    public class AddColumnsToSampleResultsBactiForBug2017 : Migration
    {
        public const string TABLE_NAME = "tblWQSampleResultsBacti";

        public struct ColumnNames
        {
            public const string NITRITE = "Nitrite",
                                NITRATE = "Nitrate";
        }

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(ColumnNames.NITRITE).AsDecimal(18, 2).Nullable()
                 .AddColumn(ColumnNames.NITRATE).AsDecimal(18, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.NITRITE).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.NITRATE).FromTable(TABLE_NAME);
        }
    }
}
