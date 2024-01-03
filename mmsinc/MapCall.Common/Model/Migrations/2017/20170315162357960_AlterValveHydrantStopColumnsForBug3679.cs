using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170315162357960), Tags("Production")]
    public class AlterValveHydrantStopColumnsForBug3679 : Migration
    {
        public override void Up()
        {
            Alter.Table("Hydrants").AlterColumn("Stop").AsDecimal().Nullable();
            Alter.Table("Valves").AlterColumn("Stop").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Alter.Table("Hydrants").AlterColumn("Stop").AsInt32().Nullable();
            Alter.Table("Valves").AlterColumn("Stop").AsInt32().Nullable();
        }
    }
}
