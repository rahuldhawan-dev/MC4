using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230323160711456), Tags("Production")]
    public class MC5443_RenameSAPEquipmentTypesToEquipmentTypes : Migration
    {
        public override void Up()
        {
            //SAP Equipment Type
            Rename.Table("SAPEquipmentTypes").To("EquipmentTypes");
            Execute.Sql("EXEC sp_rename 'PK_SAPEquipmentTypes', 'PK_EquipmentTypes'");

            //Corrective Order Problem Codes SAP Equipment Types
            Rename.Column("SAPEquipmentTypeId").OnTable("CorrectiveOrderProblemCodesSAPEquipmentTypes").To("EquipmentTypeId");
            Rename.Table("CorrectiveOrderProblemCodesSAPEquipmentTypes").To("CorrectiveOrderProblemCodesEquipmentTypes");
            Execute.Sql("EXEC sp_rename 'FK_CorrectiveOrderProblemCodesSAPEquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_CorrectiveOrderProblemCodesEquipmentTypes_EquipmentTypes_EquipmentTypeId'");

            //Equipment
            Rename.Column("SAPEquipmentTypeId").OnTable("Equipment").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_Equipment_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_Equipment_EquipmentTypes_EquipmentTypeId'");

            //Equipment Characteristic Fields
            Rename.Column("SAPEquipmentTypeId").OnTable("EquipmentCharacteristicFields").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentCharacteristicFields_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_EquipmentCharacteristicFields_EquipmentTypes_EquipmentTypeId'");

            //Equipment Manufacturers
            Rename.Column("SAPEquipmentTypeId").OnTable("EquipmentManufacturers").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_SAPEquipmentManufacturers_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_SAPEquipmentManufacturers_EquipmentTypes_EquipmentTypeId'");

            //Equipment Purpose (Type)
            Rename.Column("SAPEquipmentTypeId").OnTable("EquipmentPurposes").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_EquipmentPurposes_EquipmentTypes_EquipmentTypeId'");

            //Lockout Forms
            Rename.Column("SAPEquipmentTypeId").OnTable("LockoutForms").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_LockoutForms_SapEquipmentTypes_SAPEquipmentTypeId', 'FK_LockoutForms_EquipmentTypes_EquipmentTypeId'");

            //Measurement Points SAP Equipment Types
            Rename.Column("SAPEquipmentTypeId").OnTable("MeasurementPointsSAPEquipmentTypes").To("EquipmentTypeId");
            Rename.Table("MeasurementPointsSAPEquipmentTypes").To("MeasurementPointsEquipmentTypes");
            Execute.Sql("EXEC sp_rename 'FK_MeasurementPointsSAPEquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_MeasurementPointsEquipmentTypes_EquipmentTypes_EquipmentTypeId'");

            //Production Work Descriptions
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkDescriptions_SAPEquipmentTypes_EquipmentClassId', 'FK_ProductionWorkDescriptions_EquipmentTypes_EquipmentClassId'");

            //Production Work Orders
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkOrders_SAPEquipmentTypes_EquipmentClassId', 'FK_ProductionWorkOrders_EquipmentTypes_EquipmentClassId'");

            //SAP Equipment Types Maintenance Plan
            Rename.Column("SAPEquipmentTypeId").OnTable("SAPEquipmentTypesMaintenancePlan").To("EquipmentTypeId");
            Rename.Table("SAPEquipmentTypesMaintenancePlan").To("EquipmentTypesMaintenancePlan");
            Execute.Sql("EXEC sp_rename 'FK_SAPEquipmentTypesMaintenancePlan_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_EquipmentTypesMaintenancePlan_EquipmentTypes_EquipmentTypeId'");

            //Task Groups Equipment Classes
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentClasses_SAPEquipmentTypes_EquipmentClassId', 'FK_TaskGroupsEquipmentClasses_EquipmentTypes_EquipmentClassId'");
        }

        public override void Down()
        {
            //Task Groups Equipment Classes
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentClasses_EquipmentTypes_EquipmentClassId', 'FK_TaskGroupsEquipmentClasses_SAPEquipmentTypes_EquipmentClassId'");

            //SAP Equipment Types Maintenance Plan
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypesMaintenancePlan_EquipmentTypes_EquipmentTypeId', 'FK_SAPEquipmentTypesMaintenancePlan_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Table("EquipmentTypesMaintenancePlan").To("SAPEquipmentTypesMaintenancePlan");
            Rename.Column("EquipmentTypeId").OnTable("SAPEquipmentTypesMaintenancePlan").To("SAPEquipmentTypeId");

            //Production Work Orders
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkOrders_EquipmentTypes_EquipmentClassId', 'FK_ProductionWorkOrders_SAPEquipmentTypes_EquipmentClassId'");

            //Production Work Descriptions
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkDescriptions_EquipmentTypes_EquipmentClassId', 'FK_ProductionWorkDescriptions_SAPEquipmentTypes_EquipmentClassId'");

            //Measurement Points SAP Equipment Types
            Execute.Sql("EXEC sp_rename 'FK_MeasurementPointsEquipmentTypes_EquipmentTypes_EquipmentTypeId', 'FK_MeasurementPointsSAPEquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Table("MeasurementPointsEquipmentTypes").To("MeasurementPointsSAPEquipmentTypes");
            Rename.Column("EquipmentTypeId").OnTable("MeasurementPointsSAPEquipmentTypes").To("SAPEquipmentTypeId");

            //Lockout Forms
            Execute.Sql("EXEC sp_rename 'FK_LockoutForms_EquipmentTypes_EquipmentTypeId', 'FK_LockoutForms_SapEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Column("EquipmentTypeId").OnTable("LockoutForms").To("SAPEquipmentTypeId");

            //Equipment Purpose (Type)
            Rename.Column("EquipmentTypeId").OnTable("EquipmentPurposes").To("SAPEquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_EquipmentTypes_EquipmentTypeId', 'FK_EquipmentPurposes_SAPEquipmentTypes_SAPEquipmentTypeId'");

            //Equipment Manufacturers
            Execute.Sql("EXEC sp_rename 'FK_SAPEquipmentManufacturers_EquipmentTypes_EquipmentTypeId', 'FK_SAPEquipmentManufacturers_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Column("EquipmentTypeId").OnTable("EquipmentManufacturers").To("SAPEquipmentTypeId");

            //Equipment Characteristic Fields
            Execute.Sql("EXEC sp_rename 'FK_EquipmentCharacteristicFields_EquipmentTypes_EquipmentTypeId', 'FK_EquipmentCharacteristicFields_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Column("EquipmentTypeId").OnTable("EquipmentCharacteristicFields").To("SAPEquipmentTypeId");

            //Equipment
            Execute.Sql("EXEC sp_rename 'FK_Equipment_EquipmentTypes_EquipmentTypeId', 'FK_Equipment_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Column("EquipmentTypeId").OnTable("Equipment").To("SAPEquipmentTypeId");

            //Corrective Order Problem Code sSAP Equipment Types
            Execute.Sql("EXEC sp_rename 'FK_CorrectiveOrderProblemCodesEquipmentTypes_EquipmentTypes_EquipmentTypeId', 'FK_CorrectiveOrderProblemCodesSAPEquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Rename.Table("CorrectiveOrderProblemCodesEquipmentTypes").To("CorrectiveOrderProblemCodesSAPEquipmentTypes");
            Rename.Column("EquipmentTypeId").OnTable("CorrectiveOrderProblemCodesSAPEquipmentTypes").To("SAPEquipmentTypeId");

            //SAP Equipment Type
            Execute.Sql("EXEC sp_rename 'PK_EquipmentTypes', 'PK_SAPEquipmentTypes'");
            Rename.Table("EquipmentTypes").To("SAPEquipmentTypes");
        }
    }
}

