using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140811083338869), Tags("Production")]
    public class AddAnotherColumnToSampleResultsBactiForBug2017 : Migration
    {
        public const string TABLE_NAME = "tblWQSampleResultsBacti";

        public struct ColumnNames
        {
            public const string HPC = "HPC";
        }

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(ColumnNames.HPC).AsDecimal(18, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.HPC).FromTable(TABLE_NAME);
        }
    }
}
