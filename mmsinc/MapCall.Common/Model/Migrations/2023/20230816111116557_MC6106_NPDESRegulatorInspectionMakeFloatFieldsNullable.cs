using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230816111116557), Tags("Production")]
    public class MC6106_NPDESRegulatorInspectionMakeFloatFieldsNullable : Migration
    {
        private const string TABLE_NAME = "NPDESRegulatorInspections";
        
        public override void Up()
        {
            Alter.Column("RainfallEstimate").OnTable(TABLE_NAME).AsFloat().Nullable();
            Alter.Column("DischargeFlow").OnTable(TABLE_NAME).AsFloat().Nullable();
            Alter.Column("DischargeDuration").OnTable(TABLE_NAME).AsFloat().Nullable();
        }

        public override void Down()
        {
            Execute.Sql($"UPDATE {TABLE_NAME} SET RainfallEstimate = 0 WHERE RainfallEstimate IS NULL");
            Alter.Column("RainfallEstimate").OnTable(TABLE_NAME).AsFloat().NotNullable().WithDefaultValue(0);

            Execute.Sql($"UPDATE {TABLE_NAME} SET DischargeFlow = 0 WHERE DischargeFlow IS NULL");
            Alter.Column("DischargeFlow").OnTable(TABLE_NAME).AsFloat().NotNullable().WithDefaultValue(0);

            Execute.Sql($"UPDATE {TABLE_NAME} SET DischargeDuration = 0 WHERE DischargeDuration IS NULL");
            Alter.Column("DischargeDuration").OnTable(TABLE_NAME).AsFloat().NotNullable().WithDefaultValue(0);
        }
    }
}

