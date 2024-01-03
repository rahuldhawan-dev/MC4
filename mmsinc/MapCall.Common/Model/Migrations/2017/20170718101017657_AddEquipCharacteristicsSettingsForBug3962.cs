using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170718101017657), Tags("Production")]
    public class AddEquipCharacteristicsSettingsForBug3962 : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentCharacteristicFields")
                 .AddColumn("IsActive").AsBoolean().WithDefaultValue(false).NotNullable()
                 .AddColumn("OrderBy").AsInt32().Nullable();
            Execute.Sql("UPDATE EquipmentCharacteristicFields SET IsActive = 1");
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("EquipmentCharacteristicFields");
            Delete.Column("OrderBy").FromTable("EquipmentCharacteristicFields");
        }
    }
}
