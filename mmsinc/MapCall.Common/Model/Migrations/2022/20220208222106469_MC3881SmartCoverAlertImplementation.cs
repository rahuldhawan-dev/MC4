using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220208222106469), Tags("Production")]
    public class MC3881SmartCoverAlertImplementation : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string ACOUSTIC_MONITORING_TYPES = "AcousticMonitoringTypes",
                                NOTIFICATION_PURPOSES = "NotificationPurposes",
                                SMART_COVER_ALERT_ALARM_TYPES = "SmartCoverAlertAlarmTypes",
                                SMART_COVER_ALERT_APPLICATION_DESCRIPTION_TYPES = "SmartCoverAlertApplicationDescriptionTypes",
                                SMART_COVER_ALERTS = "SmartCoverAlerts",
                                SMART_COVER_ALERT_ALARMS = "SmartCoverAlertAlarms",
                                SEWER_OPENINGS = "SewerOpenings",
                                USERS = "tblPermissions",
                                WORK_ORDERS = "WorkOrders";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.SMART_COVER_ALERT_ALARM_TYPES)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.SMART_COVER_ALERT_APPLICATION_DESCRIPTION_TYPES)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.SMART_COVER_ALERTS)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("AlertId").AsInt32().NotNullable()
                  .WithForeignKeyColumn("SewerOpeningId", Tables.SEWER_OPENINGS)
                  .WithColumn("SewerOpeningNumber").AsAnsiString(50).Nullable()
                  .WithForeignKeyColumn("ApplicationDescriptionId", Tables.SMART_COVER_ALERT_APPLICATION_DESCRIPTION_TYPES)
                  .WithColumn("Latitude").AsDecimal().Nullable()
                  .WithColumn("Longitude").AsDecimal().Nullable()
                  .WithColumn("Elevation").AsDecimal().Nullable()
                  .WithColumn("HighAlarmThreshold").AsDecimal().Nullable()
                  .WithColumn("SensorToBottom").AsDecimal().Nullable()
                  .WithColumn("ManholeDepth").AsDecimal().Nullable()
                  .WithColumn("PowerPackVoltage").AsAnsiString(10).Nullable()
                  .WithColumn("WaterLevelAboveBottom").AsAnsiString(10).Nullable()
                  .WithColumn("Temperature").AsAnsiString(10).Nullable()
                  .WithColumn("SignalStrength").AsAnsiString(10).Nullable()
                  .WithColumn("SignalQuality").AsAnsiString(10).Nullable()
                  .WithColumn("DateReceived").AsDateTime2().NotNullable()
                  .WithColumn("Acknowledged").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("AcknowledgedOn").AsDateTime2().Nullable()
                  .WithForeignKeyColumn("AcknowledgedById", Tables.USERS, "RecID").Nullable();
            Alter.Table(Tables.WORK_ORDERS).AddForeignKeyColumn("SmartCoverAlertId", Tables.SMART_COVER_ALERTS);
            Insert.IntoTable(Tables.ACOUSTIC_MONITORING_TYPES).Row(new { Description = "Smart Cover" });
            Insert.IntoTable(Tables.NOTIFICATION_PURPOSES).Row(new {
                ModuleID = 73,
                Purpose = "Smart Cover Alert Acknowledged"
            });
            Create.Table(Tables.SMART_COVER_ALERT_ALARMS)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithForeignKeyColumn("SmartCoverAlertId", Tables.SMART_COVER_ALERTS).NotNullable()
                  .WithColumn("AlarmId").AsInt32().NotNullable()
                  .WithForeignKeyColumn("AlarmTypeId", Tables.SMART_COVER_ALERT_ALARM_TYPES).NotNullable()
                  .WithColumn("Value").AsDecimal().NotNullable()
                  .WithColumn("AlarmDate").AsDateTime2().NotNullable()
                  .WithColumn("Level").AsDecimal().NotNullable();

            this.AddDataType("SmartCoverAlerts");
            this.AddDocumentType("Smart Cover Alert", "SmartCoverAlerts");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Smart Cover Alert", "SmartCoverAlerts");
            this.RemoveDataType("SmartCoverAlerts");
            Delete.Table(Tables.SMART_COVER_ALERT_ALARMS);
            Delete.ForeignKeyColumn(Tables.WORK_ORDERS, "SmartCoverAlertId", Tables.SMART_COVER_ALERTS);
            Execute.Sql(@"UPDATE WorkOrders SET WorkOrders.AcousticMonitoringTypeId = null FROM WorkOrders W 
                          JOIN AcousticMonitoringTypes A ON W.AcousticMonitoringTypeId = A.Id 
                          WHERE A.Description = 'Smart Cover';");
            Delete.FromTable(Tables.ACOUSTIC_MONITORING_TYPES).Row(new { Description = "Smart Cover" });
            Delete.FromTable(Tables.NOTIFICATION_PURPOSES).Row(new { Purpose = "Smart Cover Alert Acknowledged" });
            Delete.Table(Tables.SMART_COVER_ALERTS);
            Delete.Table(Tables.SMART_COVER_ALERT_APPLICATION_DESCRIPTION_TYPES);
            Delete.Table(Tables.SMART_COVER_ALERT_ALARM_TYPES);
        }
    }
}

