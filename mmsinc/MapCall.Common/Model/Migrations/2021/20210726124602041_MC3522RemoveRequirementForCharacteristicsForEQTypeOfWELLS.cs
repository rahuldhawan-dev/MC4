using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210726124602041), Tags("Production")]
    public class MC3522RemoveRequirementForCharacteristicsForEQTypeOfWELLS_ : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"UPDATE EquipmentCharacteristicFields
            SET Required = 0
            WHERE FieldName = 'METHOD_OF_MEASUREMENT';");

            Execute.Sql(@"UPDATE EquipmentCharacteristicFields
            SET Required = 0
            WHERE FieldName = 'WELL_VAULTED';");

            Execute.Sql(@"UPDATE EquipmentCharacteristicFields
            SET Required = 0
            WHERE FieldName = 'WELL_TYPE';");
        }

        public override void Down()
        {
            Execute.Sql(@"UPDATE EquipmentCharacteristicFields
            SET Required = 1
            WHERE FieldName = 'METHOD_OF_MEASUREMENT';");

            Execute.Sql(@"UPDATE EquipmentCharacteristicFields
            SET Required = 1
            WHERE FieldName = 'WELL_VAULTED';");

            Execute.Sql(@"UPDATE EquipmentCharacteristicFields
            SET Required = 1
            WHERE FieldName = 'WELL_TYPE';");
        }
    }
}

