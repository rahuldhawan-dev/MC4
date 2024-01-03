using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230512085352273), Tags("Production")]
    public class MC5427_CreateIgnitionSystemDeliveryTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("SystemDeliveryIgnitionEntries")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("UnitOfMeasure").AsAnsiString(5).Nullable()
                  .WithColumn("EntryDate").AsDateTime().NotNullable()
                  .WithColumn("FacilityName").AsAnsiString(255).NotNullable()
                  .WithColumn("SystemDeliveryType").AsInt32().NotNullable()
                  .WithColumn("SystemDeliveryEntryType").AsInt32().NotNullable()
                  .WithColumn("EntryValue").AsDecimal(19, 5).NotNullable()
                  .WithColumn("FacilityId").AsInt32().NotNullable();

            Create.Index().OnTable("SystemDeliveryIgnitionEntries").OnColumn("FacilityId");
            Create.Index().OnTable("SystemDeliveryIgnitionEntries").OnColumn("EntryDate");
        }
    }
}
