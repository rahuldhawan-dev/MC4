using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230516142704905), Tags("Production")]
    public class MC5639_UpdateEquipmentClassRefsToEquipmentType : Migration
    {
        public override void Up()
        {
            Rename.Column("EquipmentClassId").OnTable("TaskGroupsEquipmentClasses").To("EquipmentTypeId");
            Rename.Table("TaskGroupsEquipmentClasses").To("TaskGroupsEquipmentTypes");
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentClasses_TaskGroups_TaskGroupId', 'FK_TaskGroupsEquipmentTypes_TaskGroups_TaskGroupId'");

            Rename.Column("EquipmentClassId").OnTable("ProductionWorkDescriptions").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkDescriptions_EquipmentTypes_EquipmentClassId', 'FK_ProductionWorkDescriptions_EquipmentTypes_EquipmentTypeId'");

            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentClasses_EquipmentTypes_EquipmentClassId', 'FK_TaskGroupsEquipmentTypes_EquipmentTypes_EquipmentTypeId'");

            Rename.Column("EquipmentClassId").OnTable("ProductionWorkOrders").To("EquipmentTypeId");
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkOrders_EquipmentTypes_EquipmentClassId', 'FK_ProductionWorkOrders_EquipmentTypes_EquipmentTypeId'");
        }

        public override void Down()
        {
            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkOrders_EquipmentTypes_EquipmentTypeId', 'FK_ProductionWorkOrders_EquipmentTypes_EquipmentClassId'");
            Rename.Column("EquipmentTypeId").OnTable("ProductionWorkOrders").To("EquipmentClassId");

            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentTypes_EquipmentTypes_EquipmentTypeId', 'FK_TaskGroupsEquipmentClasses_EquipmentTypes_EquipmentClassId'");

            Execute.Sql("EXEC sp_rename 'FK_ProductionWorkDescriptions_EquipmentTypes_EquipmentTypeId', 'FK_ProductionWorkDescriptions_EquipmentTypes_EquipmentClassId'");
            Rename.Column("EquipmentTypeId").OnTable("ProductionWorkDescriptions").To("EquipmentClassId");

            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentTypes_TaskGroups_TaskGroupId', 'FK_TaskGroupsEquipmentClasses_TaskGroups_TaskGroupId'");
            Rename.Table("TaskGroupsEquipmentTypes").To("TaskGroupsEquipmentClasses");
            Rename.Column("EquipmentTypeId").OnTable("TaskGroupsEquipmentClasses").To("EquipmentClassId");
        }
    }
}

