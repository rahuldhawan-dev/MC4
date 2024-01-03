using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130515101844), Tags("Production")]
    public class AddEquipmentManufacturerTypesTableBack : Migration
    {
        public override void Up()
        {
            Create.Table(AddGeneratorsForBug1462.Tables.EQUIPMENT_MANUFACTURER_TYPES)
                  .WithColumn(AddGeneratorsForBug1462.Columns.EQUIPMENT_MANUFACTURER_TYPE_ID).AsInt32().PrimaryKey()
                  .Identity().NotNullable()
                  .WithColumn(AddGeneratorsForBug1462.Columns.DESCRIPTION)
                  .AsAnsiString(AddGeneratorsForBug1462.StringLengths.DESCRIPTION).NotNullable();
            Execute.Sql(
                "INSERT INTO EquipmentManufacturerTypes SELECT DISTINCT case when (ManufacturerTypeID = 1) then 'Generator' when (manufacturerTypeID = 2) then 'Engine' end FROM EquipmentManufacturers;");
            Create.ForeignKey(
                       AddGeneratorsForBug1462.ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES)
                  .FromTable(AddGeneratorsForBug1462.Tables.EQUIPMENT_MANUFACTURERS)
                  .ForeignColumn(AddGeneratorsForBug1462.Columns.MANUFACTURER_TYPE_ID)
                  .ToTable(AddGeneratorsForBug1462.Tables.EQUIPMENT_MANUFACTURER_TYPES)
                  .PrimaryColumn(AddGeneratorsForBug1462.Columns.EQUIPMENT_MANUFACTURER_TYPE_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(
                       AddGeneratorsForBug1462.ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES)
                  .OnTable(AddGeneratorsForBug1462.Tables.EQUIPMENT_MANUFACTURERS);
            Delete.Table(AddGeneratorsForBug1462.Tables.EQUIPMENT_MANUFACTURER_TYPES);
        }
    }
}
