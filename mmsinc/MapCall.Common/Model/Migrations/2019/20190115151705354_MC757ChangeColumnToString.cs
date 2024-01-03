using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190115151705354), Tags("Production")]
    public class MC757ChangeColumnToString : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletionTestResults").AlterColumn("InitialRepair").AsAnsiString(2)
                 .Nullable();
        }

        public override void Down()
        {
            // We can't roll this back it would make data invalid.
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletionTestResults SET InitialRepair = NULL WHERE IsNumeric(InitialRepair) = 0");
            Alter.Table("ShortCycleWorkOrderCompletionTestResults").AlterColumn("InitialRepair").AsInt32().Nullable();
        }
    }
}
