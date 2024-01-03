using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140512102429599), Tags("Production")]
    public class AlterColumnsForAllocationPermitsForBug1899 : Migration
    {
        public const string TABLE_NAME = "AllocationPermits";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AlterColumn("GPD").AsDecimal(18, 2).Nullable();
            Alter.Table(TABLE_NAME).AlterColumn("MGM").AsDecimal(18, 2).Nullable();
            Alter.Table(TABLE_NAME).AlterColumn("MGY").AsDecimal(18, 2).Nullable();
        }

        public override void Down()
        {
            Alter.Table(TABLE_NAME).AlterColumn("GPD").AsFloat().Nullable();
            Alter.Table(TABLE_NAME).AlterColumn("MGM").AsFloat().Nullable();
            Alter.Table(TABLE_NAME).AlterColumn("MGY").AsFloat().Nullable();
        }
    }
}
