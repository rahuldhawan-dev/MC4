using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230328100101862), Tags("Production")]
    public class MC5412_PlannedWorkModuleMoveFieldsFromTaskGroupToPlanTaskInformationSection : Migration
    {
        public override void Up()
        {
            Alter.Table("MaintenancePlans")
                 .AddColumn("Resources").AsDecimal(9, 2).Nullable()
                 .AddColumn("EstimatedHours").AsDecimal(5, 2).Nullable()
                 .AddColumn("ContractorCost").AsDecimal(8, 2).Nullable()
                 .AddForeignKeyColumn("SkillSetId", "SkillSets", nullable: true).WithDefaultValue(0);

            Execute.Sql(
                $"UPDATE MaintenancePlans SET MaintenancePlans.Resources = TaskGroups.Resources, MaintenancePlans.EstimatedHours = TaskGroups.EstimatedHours, MaintenancePlans.ContractorCost = TaskGroups.ContractorCost, MaintenancePlans.SkillSetId = TaskGroups.SkillSetId FROM TaskGroups, MaintenancePlans WHERE TaskGroups.Id = MaintenancePlans.TaskGroupId");

            Delete.Column("Resources").FromTable("TaskGroups");
            Delete.Column("EstimatedHours").FromTable("TaskGroups");
            Delete.Column("ContractorCost").FromTable("TaskGroups");
            Delete.ForeignKeyColumn("TaskGroups", "SkillSetId", "SkillSets");
        }

        public override void Down()
        {
            Alter.Table("TaskGroups")
                 .AddColumn("Resources").AsDecimal(9, 2).Nullable()
                 .AddColumn("EstimatedHours").AsDecimal(5, 2).Nullable()
                 .AddColumn("ContractorCost").AsDecimal(8, 2).Nullable()
                 .AddForeignKeyColumn("SkillSetId", "SkillSets", nullable: true).WithDefaultValue(0);

            Execute.Sql(
                $"UPDATE TaskGroups SET TaskGroups.Resources = MaintenancePlans.Resources, TaskGroups.EstimatedHours = MaintenancePlans.EstimatedHours, TaskGroups.ContractorCost = MaintenancePlans.ContractorCost, TaskGroups.SkillSetId = MaintenancePlans.SkillSetId FROM MaintenancePlans, TaskGroups WHERE MaintenancePlans.TaskGroupId = TaskGroups.Id");

            Delete.Column("Resources").FromTable("MaintenancePlans");
            Delete.Column("EstimatedHours").FromTable("MaintenancePlans");
            Delete.Column("ContractorCost").FromTable("MaintenancePlans");
            Delete.ForeignKeyColumn("MaintenancePlans", "SkillSetId", "SkillSets");
        }
    }
}

