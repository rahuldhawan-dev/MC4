using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200731104121681), Tags("Production")]
    public class MC2388GasMonitorTables : Migration
    {
        private const string GAS_MONITORS = "GasMonitors",
                             GAS_MONITOR_CALIBRATIONS = "GasMonitorCalibrations";

        public override void Up()
        {
            Create.Table(GAS_MONITORS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID").NotNullable()
                  .WithColumn("CalibrationFrequencyDays").AsInt32().NotNullable()
                  .WithForeignKeyColumn("AssignedEmployeeId", "tblEmployee", "tblEmployeeID").Nullable();

            Create.Table(GAS_MONITOR_CALIBRATIONS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("GasMonitorId", GAS_MONITORS).NotNullable().Indexed()
                  .WithColumn("CalibrationDate").AsDateTime().NotNullable()
                  .WithColumn("CalibrationPassed").AsBoolean().NotNullable()
                  .WithColumn("CalibrationFailedNotes").AsCustom("varchar(max)").Nullable()
                  .WithForeignKeyColumn("CreatedByUserId", "tblPermissions", "RecID").NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable();

            this.AddDataType(GAS_MONITORS);
            this.AddDataType(GAS_MONITOR_CALIBRATIONS);
            this.AddDocumentType("Gas Monitor Document", GAS_MONITORS);
            this.AddDocumentType("Gas Monitor Calibration Document", GAS_MONITOR_CALIBRATIONS);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Gas Monitor Document", GAS_MONITORS);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Gas Monitor Calibration Document", GAS_MONITOR_CALIBRATIONS);
            this.RemoveDataType(GAS_MONITORS);
            this.RemoveDataType(GAS_MONITOR_CALIBRATIONS);

            Delete.Index("IX_GasMonitorCalibrations_GasMonitorId").OnTable(GAS_MONITOR_CALIBRATIONS);
            Delete.ForeignKeyColumn("GasMonitorCalibrations", "GasMonitorId", GAS_MONITORS);
            Delete.ForeignKeyColumn("GasMonitorCalibrations", "CreatedByUserId", "tblPermissions", "RecId");
            Delete.Table(GAS_MONITOR_CALIBRATIONS);
            Delete.Table(GAS_MONITORS);
        }
    }
}

