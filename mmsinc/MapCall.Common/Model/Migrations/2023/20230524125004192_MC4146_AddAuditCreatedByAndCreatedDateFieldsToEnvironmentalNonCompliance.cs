using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230524125004192), Tags("Production")]
    public class MC4146AddAuditCreatedByAndCreatedDateFieldsToEnvironmentalNonCompliance : Migration
    {
        #region Constants

        public struct TableNames
        {
            public static string USER = "tblPermissions",
                                 ENVIRONMENTAL_NON_COMPLIANCE = "EnvironmentalNonComplianceEvents";
        }

        public struct ColumnNames
        {
            public static string CREATED_BY = "CreatedById",
                                 CREATED_AT = "CreatedAt",
                                 DATE_REPORTED = "DateReported";
        }

        public struct SqlCommands
        {
            public static string SET_CREATED_AT_COLUMN = "Update EnvironmentalNonComplianceEvents Set CreatedAt = DateReported",
                                 ROLLBACK_CREATED_AT_COLUMN_UPDATE = "Update EnvironmentalNonComplianceEvents Set DateReported = CreatedAt";
        }

        #endregion
        public override void Up()
        {
            Alter.Table(TableNames.ENVIRONMENTAL_NON_COMPLIANCE)
                 .AddForeignKeyColumn(ColumnNames.CREATED_BY, TableNames.USER, "RecID").Nullable();
            Alter.Table(TableNames.ENVIRONMENTAL_NON_COMPLIANCE).AddColumn(ColumnNames.CREATED_AT).AsDateTime().Nullable();
            this.Execute.Sql(SqlCommands.SET_CREATED_AT_COLUMN);
            this.Delete.Column(ColumnNames.DATE_REPORTED).FromTable(TableNames.ENVIRONMENTAL_NON_COMPLIANCE);
        }

        public override void Down()
        {
            Alter.Table(TableNames.ENVIRONMENTAL_NON_COMPLIANCE).AddColumn(ColumnNames.DATE_REPORTED).AsDateTime().NotNullable();
            this.Execute.Sql(SqlCommands.ROLLBACK_CREATED_AT_COLUMN_UPDATE);
            this.DeleteForeignKeyColumn(TableNames.ENVIRONMENTAL_NON_COMPLIANCE, ColumnNames.CREATED_BY, TableNames.USER);
            Delete.Column(ColumnNames.CREATED_AT).FromTable(TableNames.ENVIRONMENTAL_NON_COMPLIANCE);
        }
    }
}
