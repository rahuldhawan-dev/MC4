using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220601103538006), Tags("Production")]
    public class MC750AlterTaskGroupsCreateEquipmentTables : Migration
    {
        public override void Up()
        {
            Create.Table("TaskGroupsEquipmentDetailTypes")
                  .WithForeignKeyColumn("TaskGroupId", "TaskGroups").NotNullable()
                  .WithForeignKeyColumn("EquipmentDetailTypeId", "EquipmentDetailTypes", "EquipmentDetailTypeId").NotNullable();

            Create.Table("TaskGroupsEquipmentTypes")
                  .WithForeignKeyColumn("TaskGroupId", "TaskGroups").NotNullable()
                  .WithForeignKeyColumn("EquipmentTypeId", "EquipmentTypes", "EquipmentTypeId").NotNullable();

            Delete.DefaultConstraint().OnTable("TaskGroups").OnColumn("TaskGroupId");

            Alter.Table("TaskGroups")
                 .AddColumn("TaskDetails").AsCustom("varchar(max)").Nullable()
                 .AddColumn("TaskDetailsSummary").AsAnsiString(250).Nullable()
                 .AddColumn("Resources").AsDecimal(9, 2).Nullable()
                 .AddColumn("EstimatedHours").AsDecimal(5, 2).Nullable()
                 .AddForeignKeyColumn("TaskGroupCategoryId", "TaskGroupCategories")
                 .AddForeignKeyColumn("MaintenancePlanTaskTypeId", "MaintenancePlanTaskTypes")
                 .AddColumn("ContractorCost").AsDecimal(8, 2).Nullable()
                 .AlterColumn("TaskGroupId").AsString(10).NotNullable().WithDefaultValue("")
                 .AddForeignKeyColumn("SkillSetId", "SkillSets", nullable: true).WithDefaultValue(0);

            Rename.Column("Description").OnTable("TaskGroups").To("TaskGroupName");
            Delete.Column("Required").FromTable("TaskGroups");
            Delete.Column("Frequency").FromTable("TaskGroups");
        }

        public override void Down()
        {
            Delete.Table("TaskGroupsEquipmentDetailTypes");
            Delete.Table("TaskGroupsEquipmentTypes");

            Delete.Column("TaskDetails").FromTable("TaskGroups");
            Delete.Column("TaskDetailsSummary").FromTable("TaskGroups");
            Delete.Column("Resources").FromTable("TaskGroups");
            Delete.Column("EstimatedHours").FromTable("TaskGroups");
            Delete.ForeignKeyColumn("TaskGroups", "TaskGroupCategoryId", "TaskGroupCategories");
            Delete.ForeignKeyColumn("TaskGroups", "MaintenancePlanTaskTypeId", "MaintenancePlanTaskTypes");
            Delete.Column("ContractorCost").FromTable("TaskGroups");

            Delete.DefaultConstraint().OnTable("TaskGroups").OnColumn("TaskGroupId");

            Delete.ForeignKeyColumn("TaskGroups", "SkillSetId", "SkillSets");

            Rename.Column("TaskGroupName").OnTable("TaskGroups").To("Description");

            Alter.Table("TaskGroups")
                 .AddColumn("Required").AsBoolean().Nullable()
                 .AddColumn("Frequency").AsInt32().NotNullable().WithDefaultValue(1);
        }
    }
}
