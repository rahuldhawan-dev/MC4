using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170609113114789), Tags("Production")]
    public class AddPlanningPlantToFacilityForBug3864 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddForeignKeyColumn("PlanningPlantId", "PlanningPlants").Nullable();
            Alter.Table("SAPEquipmentTypes").AlterColumn("Abbreviation").AsAnsiString(11).NotNullable();
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM SAPEquipmentTypes where Abbreviation = 'AMIDATACOLL') INSERT INTO SAPEquipmentTypes Values('AMIDATACOLL', 'AMIDATACOLL');" +
                "IF NOT EXISTS (SELECT 1 FROM SAPEquipmentTypes where Abbreviation = 'UV-SOUND') INSERT INTO SAPEquipmentTypes Values('UV-SOUND', 'UV-SOUND');");
            Alter.Table("EquipmentModels")
                 .AddForeignKeyColumn("SAPEquipmentManufacturerID", "SAPEquipmentManufacturers")
                 .Nullable();
            Execute.Sql(@"update
	                        Equipment
                        Set
	                        SAPEquipmentManufacturerId = (SELECT Id from SAPEquipmentManufacturers sap1 where sap1.Description = em.Description and sap1.SAPEquipmentTypeId = et.SAPEquipmentTypeId)
                        from 
	                        Equipment e
                        left join	
	                        EquipmentManufacturers em on em.EquipmentManufacturerID = ManufacturerID
                        left join
	                        EquipmentTypes et on et.EquipmentTypeID = e.TypeId
                        where 
	                        SAPEquipmentManufacturerId is null and ManufacturerID is not null
                        ");
            Execute.Sql(
                "update EquipmentModels set SAPEquipmentManufacturerId = (SELECT top 1 SAPEquipmentManufacturerID from Equipment wherE Equipment.ManufacturerId = EquipmentModels.ManufacturerID);");
            Delete.ForeignKeyColumn("EquipmentModels", "ManufacturerId", "EquipmentManufacturers");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "PlanningPlantId", "PlanningPlants");
            Alter.Table("EquipmentModels")
                 .AddForeignKeyColumn("ManufacturerId", "EquipmentManufacturers", "EquipmentManufacturerID");
            Delete.ForeignKeyColumn("EquipmentModels", "SAPEquipmentManufacturerID", "SAPEquipmentManufacturers");
            //Alter.Table("SAPEquipmentTypes").AlterColumn("Abbreviation").AsAnsiString(8).NotNullable();
        }
    }
}
