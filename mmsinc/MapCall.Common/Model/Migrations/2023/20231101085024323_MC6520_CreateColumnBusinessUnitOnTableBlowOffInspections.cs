using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231101085024323), Tags("Production")]
    public class MC6520_CreateColumnBusinessUnitOnTableBlowOffInspections : AutoReversingMigration
    {
        public override void Up() =>
            Create.Column("BusinessUnit")
                  .OnTable("BlowOffInspections")
                  .AsString(256)
                  .Nullable();
    }
}

