using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231012145910354), Tags("Production")]
    public class MC6097_CreateColumnBusinessUnitOnTableHydrantInspection : AutoReversingMigration
    {
        public override void Up() =>
            Create.Column("BusinessUnit")
                  .OnTable("HydrantInspections")
                  .AsString(256)
                  .Nullable();
    }
}

