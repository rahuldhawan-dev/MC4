using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211005160431035), Tags("Production")]
    public class AddCommentsColumnToSystemDeliveryEquipmentEntriesReversalsTableForMC3143 : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryEquipmentEntriesReversals")
                 .AddColumn("Comment").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Comment").FromTable("SystemDeliveryEquipmentEntriesReversals");
        }
    }
}

