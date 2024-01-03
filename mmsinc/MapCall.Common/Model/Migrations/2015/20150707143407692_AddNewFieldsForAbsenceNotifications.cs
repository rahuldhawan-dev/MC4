using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150707143407692), Tags("Production")]
    public class AddNewFieldsForAbsenceNotifications : Migration
    {
        public struct TableNames
        {
            public const string
                EMPLOYEE_FMLA_NOTIFICATIONS = "EmployeeFMLANotifications",
                PROGRESSIVE_DISCIPLINES = "ProgressiveDisciplines",
                ABSENSE_NOTIFICATIONS = "AbsenceNotifications",
                ABSENCE_STATUSES = "AbsenceStatuses";
        }

        public struct ColumnNames
        {
            public const string
                EMPLOYEE_FMLA_NOTIFICATION_ID = "EmployeeFMLANotificationId",
                PROGRESSIVE_DISCIPLINE_ID = "ProgressiveDisciplineId",
                ABSENCE_STATUS_ID = "AbsenceStatusId";
        }

        public override void Up()
        {
            //EmployeeFMLANotification
            this.CreateLookupTableWithValues(TableNames.EMPLOYEE_FMLA_NOTIFICATIONS,
                new[] {"Sick Phone Message", "Verbally", "Other"});
            Alter.Table(TableNames.ABSENSE_NOTIFICATIONS)
                 .AddForeignKeyColumn(ColumnNames.EMPLOYEE_FMLA_NOTIFICATION_ID,
                      TableNames.EMPLOYEE_FMLA_NOTIFICATIONS);
            Execute.Sql("UPDATE " + TableNames.ABSENSE_NOTIFICATIONS + " SET " +
                        ColumnNames.EMPLOYEE_FMLA_NOTIFICATION_ID + " = (SELECT Id FROM " +
                        TableNames.EMPLOYEE_FMLA_NOTIFICATIONS + " WHERE Description = 'Other')");
            Alter.Column(ColumnNames.EMPLOYEE_FMLA_NOTIFICATION_ID).OnTable(TableNames.ABSENSE_NOTIFICATIONS).AsInt32()
                 .NotNullable();

            //Progressive Discipline
            this.CreateLookupTableWithValues(TableNames.PROGRESSIVE_DISCIPLINES,
                new[] {"Below Threshold", "Counseled", "Verbal Warning", "Written Warning", "Suspension", "Discharge"});
            Alter.Table(TableNames.ABSENSE_NOTIFICATIONS)
                 .AddForeignKeyColumn(ColumnNames.PROGRESSIVE_DISCIPLINE_ID, TableNames.PROGRESSIVE_DISCIPLINES);

            //Absence Status
            this.CreateLookupTableWithValues(TableNames.ABSENCE_STATUSES, new[] {"FMLA Approved", "Occurrence"});
            Alter.Table(TableNames.ABSENSE_NOTIFICATIONS)
                 .AddForeignKeyColumn(ColumnNames.ABSENCE_STATUS_ID, TableNames.ABSENCE_STATUSES);

            //Occurrence
            Execute.Sql("UPDATE " + TableNames.ABSENSE_NOTIFICATIONS + " SET " + ColumnNames.ABSENCE_STATUS_ID +
                        " = (SELECT Id from " + TableNames.ABSENCE_STATUSES +
                        " WHERE Description = 'Occurrence') WHERE Occurrence = 1");
            Execute.Sql("UPDATE " + TableNames.ABSENSE_NOTIFICATIONS + " SET " + ColumnNames.ABSENCE_STATUS_ID +
                        " = (SELECT Id from " + TableNames.ABSENCE_STATUSES +
                        " WHERE Description = 'FMLA Approved') WHERE AbsenceStatusId is null");
            Delete.Column("Occurrence").FromTable(TableNames.ABSENSE_NOTIFICATIONS);
        }

        public override void Down()
        {
            //Occurrence
            Alter.Table(TableNames.ABSENSE_NOTIFICATIONS).AddColumn("Occurrence").AsBoolean().Nullable();
            Execute.Sql("UPDATE " + TableNames.ABSENSE_NOTIFICATIONS +
                        " SET Occurrence = 1 WHERE AbsenceStatusId = (SELECT Id FROM AbsenceStatuses WHERE Description = 'Occurrence')");
            Execute.Sql("UPDATE " + TableNames.ABSENSE_NOTIFICATIONS + " SET Occurrence = 0 WHERE Occurrence is null");
            Alter.Column("Occurrence").OnTable(TableNames.ABSENSE_NOTIFICATIONS).AsBoolean().NotNullable();

            //EmployeeFMLANotification
            Delete.ForeignKeyColumn(TableNames.ABSENSE_NOTIFICATIONS, ColumnNames.EMPLOYEE_FMLA_NOTIFICATION_ID,
                TableNames.EMPLOYEE_FMLA_NOTIFICATIONS);
            Delete.Table(TableNames.EMPLOYEE_FMLA_NOTIFICATIONS);

            //Progressive Discipline
            Delete.ForeignKeyColumn(TableNames.ABSENSE_NOTIFICATIONS, ColumnNames.PROGRESSIVE_DISCIPLINE_ID,
                TableNames.PROGRESSIVE_DISCIPLINES);
            Delete.Table(TableNames.PROGRESSIVE_DISCIPLINES);

            //Absence Status
            Delete.ForeignKeyColumn(TableNames.ABSENSE_NOTIFICATIONS, ColumnNames.ABSENCE_STATUS_ID,
                TableNames.ABSENCE_STATUSES);
            Delete.Table(TableNames.ABSENCE_STATUSES);
        }
    }
}
