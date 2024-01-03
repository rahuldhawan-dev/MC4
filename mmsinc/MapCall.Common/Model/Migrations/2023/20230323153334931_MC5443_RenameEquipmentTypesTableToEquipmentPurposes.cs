using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230323153334931), Tags("Production")]
    public class MC5443_RenameEquipmentTypesTableToEquipmentPurposes : Migration
    {
        public override void Up()
        {
            Rename.Column("EquipmentTypeID").OnTable("EquipmentTypes").To("EquipmentPurposeId");
            Rename.Table("EquipmentTypes").To("EquipmentPurposes");
            Execute.Sql("EXEC sp_rename 'PK_EquipmentTypes', 'PK_EquipmentPurposes'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypes_EquipmentCategories_CategoryID', 'FK_EquipmentPurposes_EquipmentCategories_CategoryID'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypes_EquipmentDetailTypes_DetailTypeID', 'FK_EquipmentPurposes_EquipmentDetailTypes_DetailTypeID'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypes_EquipmentSubCategories_SubCategoryID', 'FK_EquipmentPurposes_EquipmentSubCategories_SubCategoryID'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_EquipmentPurposes_SAPEquipmentTypes_SAPEquipmentTypeId'");

            Rename.Column("EquipmentTypeId").OnTable("EquipmentTypesMaintenancePlan").To("EquipmentPurposeId");
            Rename.Table("EquipmentTypesMaintenancePlan").To("EquipmentPurposesMaintenancePlan");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypesMaintenancePlan_EquipmentTypes_EquipmentTypeId', 'FK_EquipmentPurposesMaintenancePlan_EquipmentPurposes_EquipmentPurposeId'");

            Rename.Column("TypeID").OnTable("Equipment").To("PurposeId");
            Execute.Sql("EXEC sp_rename 'FK_Equipment_EquipmentTypes_TypeID', 'FK_Equipment_EquipmentPurposes_PurposeId'");

            Rename.Column("EquipmentTypeId").OnTable("TaskGroupsEquipmentTypes").To("EquipmentPurposeId");
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentTypes_EquipmentTypes_EquipmentTypeId', 'FK_TaskGroupsEquipmentPurposes_EquipmentPurposes_EquipmentPurposeId'");
        }

        public override void Down()
        {
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentPurposes_EquipmentPurposes_EquipmentPurposeId', 'FK_TaskGroupsEquipmentTypes_EquipmentTypes_EquipmentTypeId'");
            Rename.Column("EquipmentPurposeId").OnTable("TaskGroupsEquipmentTypes").To("EquipmentTypeId");

            Execute.Sql("EXEC sp_rename 'FK_Equipment_EquipmentPurposes_PurposeId', 'FK_Equipment_EquipmentTypes_TypeID'");
            Rename.Column("PurposeId").OnTable("Equipment").To("TypeID");

            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposesMaintenancePlan_EquipmentPurposes_EquipmentPurposeId', 'FK_EquipmentTypesMaintenancePlan_EquipmentTypes_EquipmentTypeId'");
            Rename.Table("EquipmentPurposesMaintenancePlan").To("EquipmentTypesMaintenancePlan");
            Rename.Column("EquipmentPurposeId").OnTable("EquipmentTypesMaintenancePlan").To("EquipmentTypeId");

            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_SAPEquipmentTypes_SAPEquipmentTypeId', 'FK_EquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_EquipmentSubCategories_SubCategoryID', 'FK_EquipmentTypes_EquipmentSubCategories_SubCategoryID'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_EquipmentDetailTypes_DetailTypeID', 'FK_EquipmentTypes_EquipmentDetailTypes_DetailTypeID'");
            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposes_EquipmentCategories_CategoryID', 'FK_EquipmentTypes_EquipmentCategories_CategoryID'");
            Execute.Sql("EXEC sp_rename 'PK_EquipmentPurposes', 'PK_EquipmentTypes'");
            Rename.Table("EquipmentPurposes").To("EquipmentTypes");
            Rename.Column("EquipmentPurposeId").OnTable("EquipmentTypes").To("EquipmentTypeID");
        }
    }
}

