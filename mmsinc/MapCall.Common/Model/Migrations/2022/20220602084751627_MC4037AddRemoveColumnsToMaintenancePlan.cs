using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220602084751627), Tags("Production")]
    public class MC4037AddRemoveColumnsToMaintenancePlan : Migration
    {
        private const string TABLE_NAME = "MaintenancePlans";

        public override void Up()
        {
            Delete.ForeignKeyColumn(TABLE_NAME, "FacilityId", "tblFacilities", "RecordId");
            Delete.Column("Name")
                  .Column("ForecastPeriodDaysMultiplier")
                  .Column("PausedPlanResumeDate")
                  .FromTable(TABLE_NAME);

            Alter.Table(TABLE_NAME)
                 .AddColumn("TaskDetails").AsAnsiString(500).Nullable()
                 .AddColumn("PausedPlanResumeDate").AsDate().Nullable().WithDefaultValue(null)
                 .AddColumn("ForecastPeriodMultiplier").AsDecimal().NotNullable().WithDefaultValue(1.0m)
                 .AddForeignKeyColumn("EquipmentDetailTypeId", "EquipmentDetailTypes", "EquipmentDetailTypeID").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TABLE_NAME, "EquipmentDetailTypeId", "EquipmentDetailTypes", "EquipmentDetailTypeID");
            Delete.Column("ForecastPeriodMultiplier").FromTable(TABLE_NAME);
            Delete.Column("PausedPlanResumeDate").FromTable(TABLE_NAME);
            Delete.Column("TaskDetails").FromTable(TABLE_NAME);

            Alter.Table(TABLE_NAME)
                 .AddColumn("ForecastPeriodDaysMultiplier").AsInt32().NotNullable()
                 .AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId").NotNullable()
                 .AddColumn("PausedPlanResumeDate").AsDate().Nullable()
                 .AddColumn("Name").AsAnsiString(50).NotNullable().WithDefaultValue("");
        }
    }
}

