using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230201150954461), Tags("Production")]
    public class MC5288_RenameSAPEquipmentManufacturersToEquipmentManufacturers : Migration
    {
        public override void Up()
        {
            Rename.Table("SAPEquipmentManufacturers").To("EquipmentManufacturers");
            Rename.Column("SAPEquipmentManufacturerId").OnTable("Equipment").To("EquipmentManufacturerId");
            Rename.Column("SAPEquipmentManufacturerId").OnTable("EquipmentModels").To("EquipmentManufacturerId");
            
            Execute.Sql("EXEC sp_rename 'FK_Equipment_SAPEquipmentManufacturers_SAPEquipmentManufacturerId', 'FK_Equipment_EquipmentManufacturers_EquipmentManufacturerId'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentModels_SAPEquipmentManufacturers_SAPEquipmentManufacturerID', 'FK_EquipmentModels_EquipmentManufacturers_EquipmentManufacturerID'");
            Execute.Sql("EXEC sp_rename 'FK_Generators_SAPEquipmentManufacturers_EngineManufacturerID', 'FK_Generators_EquipmentManufacturers_EngineManufacturerID'");
            Execute.Sql("EXEC sp_rename 'FK_Generators_SAPEquipmentManufacturers_GeneratorManufacturerID', 'FK_Generators_EquipmentManufacturers_GeneratorManufacturerID'");
        }

        public override void Down()
        {
            Rename.Column("EquipmentManufacturerId").OnTable("EquipmentModels").To("SAPEquipmentManufacturerId");
            Rename.Column("EquipmentManufacturerId").OnTable("Equipment").To("SAPEquipmentManufacturerId");
            Rename.Table("EquipmentManufacturers").To("SAPEquipmentManufacturers");
            
            Execute.Sql("EXEC sp_rename 'FK_Equipment_EquipmentManufacturers_EquipmentManufacturerId', 'FK_Equipment_SAPEquipmentManufacturers_SAPEquipmentManufacturerId'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentModels_EquipmentManufacturers_EquipmentManufacturerID', 'FK_EquipmentModels_SAPEquipmentManufacturers_SAPEquipmentManufacturerID'");
            Execute.Sql("EXEC sp_rename 'FK_Generators_EquipmentManufacturers_EngineManufacturerID', 'FK_Generators_SAPEquipmentManufacturers_EngineManufacturerID'");
            Execute.Sql("EXEC sp_rename 'FK_Generators_EquipmentManufacturers_GeneratorManufacturerID', 'FK_Generators_SAPEquipmentManufacturers_GeneratorManufacturerID'");
        }
    }
}

