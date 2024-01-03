using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140814090841852), Tags("Production")]
    public class AlterNitriteNitrateColumnsForBug2017 : Migration
    {
        public const string TABLE_NAME = "tblWQSampleResultsBacti";

        public override void Up()
        {
            Alter.Column("Nitrite").OnTable(TABLE_NAME).AsDecimal(18, 3).Nullable();
            Alter.Column("Nitrate").OnTable(TABLE_NAME).AsDecimal(18, 3).Nullable();
        }

        public override void Down()
        {
            Alter.Column("Nitrite").OnTable(TABLE_NAME).AsDecimal(18, 2).Nullable();
            Alter.Column("Nitrate").OnTable(TABLE_NAME).AsDecimal(18, 2).Nullable();
        }
    }
}
