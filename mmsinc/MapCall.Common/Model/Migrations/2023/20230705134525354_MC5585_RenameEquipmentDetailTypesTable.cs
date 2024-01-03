using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230705134525354), Tags("Production")]
    public class MC5585_RenameEquipmentDetailTypesTable : Migration
    {
        public override void Up()
        {
            Rename.Column("EquipmentDetailTypeID").OnTable("EquipmentDetailTypes").To("Id");
            Rename.Table("EquipmentDetailTypes").To("EquipmentLifespans");
            Rename.Column("DetailTypeID").OnTable("EquipmentPurposes").To("EquipmentLifespanId");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_EquipmentDetailTypes_DetailTypeID', 'FK_EquipmentPurposes_EquipmentLifespans_LifespanId'");

            Rename.Column("EquipmentDetailTypeId").OnTable("MaintenancePlans").To("EquipmentLifespanId");
            Execute.Sql("EXEC sp_rename 'FK_MaintenancePlans_EquipmentDetailTypes_EquipmentDetailTypeId', 'FK_MaintenancePlans_EquipmentLifespans_EquipmentLifespanId'");

            Rename.Table("TaskGroupsEquipmentDetailTypes").To("TaskGroupsEquipmentLifespans");
            Rename.Column("EquipmentDetailTypeId").OnTable("TaskGroupsEquipmentLifespans").To("EquipmentLifespanId");
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentDetailTypes_EquipmentDetailTypes_EquipmentDetailTypeId', 'FK_TaskGroupsEquipmentLifespans_EquipmentLifespans_EquipmentLifespanId'");
        }

        public override void Down()
        {
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentLifespans_EquipmentLifespans_EquipmentLifespanId', 'FK_TaskGroupsEquipmentDetailTypes_EquipmentDetailTypes_EquipmentDetailTypeId'");
            Rename.Column("EquipmentLifespanId").OnTable("TaskGroupsEquipmentLifespans").To("EquipmentDetailTypeId");
            Rename.Table("TaskGroupsEquipmentLifespans").To("TaskGroupsEquipmentDetailTypes");

            Execute.Sql("EXEC sp_rename 'FK_MaintenancePlans_EquipmentLifespans_EquipmentLifespanId', 'FK_MaintenancePlans_EquipmentDetailTypes_EquipmentDetailTypeId'");
            Rename.Column("EquipmentLifespanId").OnTable("MaintenancePlans").To("EquipmentDetailTypeId");

            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_EquipmentLifespans_LifespanId', 'FK_EquipmentPurposes_EquipmentDetailTypes_DetailTypeID'");
            Rename.Column("EquipmentLifespanId").OnTable("EquipmentPurposes").To("DetailTypeID");

            Rename.Table("EquipmentLifespans").To("EquipmentDetailTypes");
            Rename.Column("Id").OnTable("EquipmentDetailTypes").To("EquipmentDetailTypeID");
        }
    }
}

