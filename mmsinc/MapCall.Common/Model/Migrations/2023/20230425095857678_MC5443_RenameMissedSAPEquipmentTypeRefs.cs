using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230425095857678), Tags("Production")]
    public class RenameMissedSAPEquipmentTypeRefs : Migration
    {
        public override void Up()
        {
            Rename.Column("MeasurementPointSAPEquipmentTypeId").OnTable("ProductionWorkOrderMeasurementPointValues").To("MeasurementPointEquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkOrderMeasurementPointValues_MeasurementPointsSAPEquipmentTypes_MeasurementPointSAPEquipmentTypeId', 'FK_ProductionWorkOrderMeasurementPointValues_MeasurementPointsEquipmentTypes_MeasurementPointEquipmentTypeId'");
            
            Execute.Sql("EXEC sp_rename 'FK_MeasurementPointsSAPEquipmentTypes_UnitsOfMeasure_UnitOfMeasureId', 'FK_MeasurementPointsEquipmentTypes_UnitsOfMeasure_UnitOfMeasureId'");

            Execute.Sql("EXEC sp_rename 'PK_MeasurementPointsSAPEquipmentTypes', 'PK_MeasurementPointsEquipmentTypes'");
            
            Execute.Sql("EXEC sp_rename 'FK_CorrectiveOrderProblemCodesSAPEquipmentTypes_CorrectiveOrderProblemCodes_CorrectiveOrderProblemCodeId', 'FK_CorrectiveOrderProblemCodesEquipmentTypes_CorrectiveOrderProblemCodes_CorrectiveOrderProblemCodeId'");
            
            Execute.Sql("EXEC sp_rename 'FK_SAPEquipmentTypesMaintenancePlan_MaintenancePlans_MaintenancePlanId', 'FK_EquipmentTypesMaintenancePlan_MaintenancePlans_MaintenancePlanId'");
        }

        public override void Down()
        {
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypesMaintenancePlan_MaintenancePlans_MaintenancePlanId', 'FK_SAPEquipmentTypesMaintenancePlan_MaintenancePlans_MaintenancePlanId'");

            Execute.Sql("EXEC sp_rename 'FK_CorrectiveOrderProblemCodesEquipmentTypes_CorrectiveOrderProblemCodes_CorrectiveOrderProblemCodeId', 'FK_CorrectiveOrderProblemCodesSAPEquipmentTypes_CorrectiveOrderProblemCodes_CorrectiveOrderProblemCodeId'");

            Execute.Sql("EXEC sp_rename 'PK_MeasurementPointsEquipmentTypes', 'PK_MeasurementPointsSAPEquipmentTypes'");

            Execute.Sql("EXEC sp_rename 'FK_MeasurementPointsEquipmentTypes_UnitsOfMeasure_UnitOfMeasureId', 'FK_MeasurementPointsSAPEquipmentTypes_UnitsOfMeasure_UnitOfMeasureId'");

            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkOrderMeasurementPointValues_MeasurementPointsEquipmentTypes_MeasurementPointEquipmentTypeId', 'FK_ProductionWorkOrderMeasurementPointValues_MeasurementPointsSAPEquipmentTypes_MeasurementPointSAPEquipmentTypeId'");
            Rename.Column("MeasurementPointEquipmentTypeId").OnTable("ProductionWorkOrderMeasurementPointValues").To("MeasurementPointSAPEquipmentTypeId");
        }
    }
}

