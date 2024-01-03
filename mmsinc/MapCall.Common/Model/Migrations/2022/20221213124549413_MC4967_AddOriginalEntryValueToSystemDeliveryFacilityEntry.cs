using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221213124549413), Tags("Production")]
    public class MC4967_AddOriginalEntryValueToSystemDeliveryFacilityEntry : AutoReversingMigration
    {
        public override void Up() => Create.Column("OriginalEntryValue").OnTable("SystemDeliveryFacilityEntries").AsDecimal(19, 5).Nullable();
    }
}
