using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220615141019493), Tags("Production")]
    public class MC4357AddingTheAsFoundAsLeftConditionsToRoutineAndRoutineOffScheduledWorkOrderTypes : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrdersEquipment").AddForeignKeyColumn("AsLeftConditionId", "AsLeftConditions");
            Alter.Table("ProductionWorkOrdersEquipment").AddForeignKeyColumn("AsFoundConditionId", "AsFoundConditions");
            Alter.Table("ProductionWorkOrdersEquipment").AddColumn("AsFoundConditionComment").AsAnsiString(100).Nullable();
            Alter.Table("ProductionWorkOrdersEquipment").AddColumn("AsLeftConditionComment").AsAnsiString(100).Nullable();
            Alter.Table("ProductionWorkOrdersEquipment").AddForeignKeyColumn("AsFoundConditionReasonId", "AssetConditionReasons");
            Alter.Table("ProductionWorkOrdersEquipment").AddForeignKeyColumn("AsLeftConditionReasonId", "AssetConditionReasons");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ProductionWorkOrdersEquipment", "AsLeftConditionId", "AsLeftConditions");
            Delete.ForeignKeyColumn("ProductionWorkOrdersEquipment", "AsFoundConditionId", "AsFoundConditions");
            Delete.ForeignKeyColumn("ProductionWorkOrdersEquipment", "AsFoundConditionReasonId", "AssetConditionReasons");
            Delete.ForeignKeyColumn("ProductionWorkOrdersEquipment", "AsLeftConditionReasonId", "AssetConditionReasons");
            Delete.Column("AsFoundConditionComment").FromTable("ProductionWorkOrdersEquipment");
            Delete.Column("AsLeftConditionComment").FromTable("ProductionWorkOrdersEquipment");
        }
    }
}