using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140915084038215), Tags("Production")]
    public class AddFieldForBug2094 : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentCharacteristicFields").AddColumn("IsSAPCharacteristic").AsBoolean().Nullable();

            Execute.Sql("UPDATE EquipmentCharacteristicFields SET IsSAPCharacteristic = 1;");

            Alter.Column("IsSAPCharacteristic").OnTable("EquipmentCharacteristicFields").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("IsSAPCharacteristic").FromTable("EquipmentCharacteristicFields");
        }
    }
}
