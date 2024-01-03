using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220412170237552), Tags("Production")]
    public class MC4300AddNeedsToSyncColumnToSmartCoverAlert : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string SMART_COVER_ALERTS = "SmartCoverAlerts",
                                SMART_COVER_ALERT_TYPES = "SmartCoverAlertTypes",
                                SMART_COVER_ALERTS_SMART_COVER_ALERT_TYPES = "SmartCoverAlertSmartCoverAlertTypes";
        }

        #endregion

        public override void Up()
        {
            Create.Column("LastSyncedAt").OnTable(Tables.SMART_COVER_ALERTS).AsDateTime().Nullable();
            Create.Column("NeedsToSync").OnTable(Tables.SMART_COVER_ALERTS).AsBoolean().NotNullable().WithDefaultValue(false);
            Execute.Sql("update SmartCoverAlerts set NeedsToSync = 1 where Acknowledged = 1");
            this.CreateLookupTableWithValues(Tables.SMART_COVER_ALERT_TYPES, 50, 
                "Low Battery", "Delayed Communications", "Suspect Sensor");
            Create.Table(Tables.SMART_COVER_ALERTS_SMART_COVER_ALERT_TYPES)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithForeignKeyColumn("SmartCoverAlertId", Tables.SMART_COVER_ALERTS).NotNullable()
                  .WithForeignKeyColumn("SmartCoverAlertTypeId", Tables.SMART_COVER_ALERT_TYPES).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(Tables.SMART_COVER_ALERTS_SMART_COVER_ALERT_TYPES);
            Delete.Table(Tables.SMART_COVER_ALERT_TYPES);
            Delete.Column("LastSyncedAt").FromTable(Tables.SMART_COVER_ALERTS);
            Delete.Column("NeedsToSync").FromTable(Tables.SMART_COVER_ALERTS);
        }
    }
}

