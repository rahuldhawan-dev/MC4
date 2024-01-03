using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160104111319354), Tags("Production")]
    public class AddArcFlashTableForBug2736 : Migration
    {
        public struct TableNames
        {
            public const string
                ARC_FLASH_STATUSES = "ArcFlashStatuses",
                VOLTAGES = "Voltages",
                POWER_PHASES = "PowerPhases",
                FACILITIES = "tblFacilities";
        }

        public struct ColumnNames
        {
            public const string
                ARC_FLASH_STATUS = "ArcFlashStatusId",
                POWER_COMPANY_DATA_RECEIVED = "PowerCompanyDataReceived",
                VOLTAGE = "VoltageId",
                PHASE = "PowerPhaseId",
                TRANSFORMER_KVA_RATING = "TransformerKVARating",
                TRANSFORMER_KVA_FIELD_CONFIRMED = "TransformerKVAFieldConfirmed",
                DATE_LABELS_APPLIED = "DateLabelsApplied",
                CONTRACTOR = "ArcFlashContractor",
                COST_TO_COMPLETE = "CostToComplete";
        }

        public struct StringLengths
        {
            public const int TRANSFORMER_KVA_RATING = 6, CONTRACTOR = 50;
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.VOLTAGES, "220");
            this.CreateLookupTableWithValues(TableNames.POWER_PHASES, "Single", "Three");
            this.CreateLookupTableWithValues(TableNames.ARC_FLASH_STATUSES, "Completed", "Pending", "Deffered", "N/A");
            Alter.Table(TableNames.FACILITIES)
                 .AddForeignKeyColumn(ColumnNames.ARC_FLASH_STATUS, TableNames.ARC_FLASH_STATUSES).Nullable();
            Alter.Table(TableNames.FACILITIES)
                 .AddForeignKeyColumn(ColumnNames.VOLTAGE, TableNames.VOLTAGES).Nullable();
            Alter.Table(TableNames.FACILITIES)
                 .AddForeignKeyColumn(ColumnNames.PHASE, TableNames.POWER_PHASES).Nullable();
            Alter.Table(TableNames.FACILITIES)
                 .AddColumn(ColumnNames.POWER_COMPANY_DATA_RECEIVED).AsBoolean().NotNullable().WithDefaultValue(0)
                 .AddColumn(ColumnNames.TRANSFORMER_KVA_RATING).AsAnsiString(StringLengths.TRANSFORMER_KVA_RATING)
                 .Nullable()
                 .AddColumn(ColumnNames.TRANSFORMER_KVA_FIELD_CONFIRMED).AsBoolean().NotNullable().WithDefaultValue(0)
                 .AddColumn(ColumnNames.DATE_LABELS_APPLIED).AsDateTime().Nullable()
                 .AddColumn(ColumnNames.CONTRACTOR).AsAnsiString(StringLengths.CONTRACTOR).Nullable()
                 .AddColumn(ColumnNames.COST_TO_COMPLETE).AsDecimal(18, 2).Nullable();
            Execute.Sql(
                "UPDATE tblFacilities SET ArcFlashStatusId = (Select Id from ArcFlashStatuses where Description = 'Pending');");
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.COST_TO_COMPLETE).FromTable(TableNames.FACILITIES);
            Delete.Column(ColumnNames.CONTRACTOR).FromTable(TableNames.FACILITIES);
            Delete.Column(ColumnNames.DATE_LABELS_APPLIED).FromTable(TableNames.FACILITIES);
            Delete.Column(ColumnNames.TRANSFORMER_KVA_FIELD_CONFIRMED).FromTable(TableNames.FACILITIES);
            Delete.Column(ColumnNames.TRANSFORMER_KVA_RATING).FromTable(TableNames.FACILITIES);
            Delete.Column(ColumnNames.POWER_COMPANY_DATA_RECEIVED).FromTable(TableNames.FACILITIES);

            Delete.ForeignKeyColumn(TableNames.FACILITIES, ColumnNames.PHASE, TableNames.POWER_PHASES);
            Delete.ForeignKeyColumn(TableNames.FACILITIES, ColumnNames.VOLTAGE, TableNames.VOLTAGES);
            Delete.ForeignKeyColumn(TableNames.FACILITIES, ColumnNames.ARC_FLASH_STATUS, TableNames.ARC_FLASH_STATUSES);

            Delete.Table(TableNames.ARC_FLASH_STATUSES);
            Delete.Table(TableNames.POWER_PHASES);
            Delete.Table(TableNames.VOLTAGES);
        }
    }
}
