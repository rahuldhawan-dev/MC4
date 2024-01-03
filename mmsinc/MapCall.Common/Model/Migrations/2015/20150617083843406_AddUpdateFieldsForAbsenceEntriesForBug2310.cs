using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150617083843406), Tags("Production")]
    public class AddUpdateFieldsForAbsenceEntriesForBug2310 : Migration
    {
        public struct TableNames
        {
            public const string ABSENCE_NOTIIFICATIONS = "AbsenceNotifications",
                                EMPLOYEE_ABSENCE_CLAIM_TYPES = "EmployeeAbsenceClaimTypes",
                                FMLA_CASES = "FMLACases",
                                COMPANY_ABSENCE_CERTIFICATIONS = "CompanyAbsenceCertifications";
        }

        public struct ColumnNames
        {
            public const string EMPLOYEE_ABSENCE_CLAIM = "EmployeeAbsenceClaim",
                                COMPANY_ABSENCE_CERTIFICATION = "CompanyAbsenceCertification";
        }

        public override void Up()
        {
            // Convert EmployeeAbsenceClaim to a table
            // Add - Pending FMLA, Left Early-Sick, Late
            // Add - bool - "Return To Work Note"
            // Add Absence Entry notification type

            // EmployeeAbsenceClaimType
            this.ExtractLookupTableLookup(
                TableNames.ABSENCE_NOTIIFICATIONS,
                ColumnNames.EMPLOYEE_ABSENCE_CLAIM,
                TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES,
                50,
                ColumnNames.EMPLOYEE_ABSENCE_CLAIM,
                deleteOldForeignKey: true);

            this.ExtractLookupTableLookup(
                TableNames.FMLA_CASES,
                ColumnNames.COMPANY_ABSENCE_CERTIFICATION,
                TableNames.COMPANY_ABSENCE_CERTIFICATIONS,
                50,
                ColumnNames.COMPANY_ABSENCE_CERTIFICATION,
                deleteOldForeignKey: true);

            Execute.Sql("IF NOT EXISTS (SELECT 1 FROM [" + TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES +
                        "] WHERE Description = 'Sick') INSERT INTO [" + TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES +
                        "] Select 'Sick'");
            Execute.Sql("IF NOT EXISTS (SELECT 1 FROM [" + TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES +
                        "] WHERE Description = 'Pending FMLA') INSERT INTO [" +
                        TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES + "] Select 'Pending FMLA'");
            Execute.Sql("IF NOT EXISTS (SELECT 1 FROM [" + TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES +
                        "] WHERE Description = 'Left Early-Sick') INSERT INTO [" +
                        TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES + "] Select 'Left Early-Sick'");
            Execute.Sql("IF NOT EXISTS (SELECT 1 FROM [" + TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES +
                        "] WHERE Description = 'Late') INSERT INTO [" + TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES +
                        "] Select 'Late'");

            // SubmittedBy
            Alter.Table(TableNames.ABSENCE_NOTIIFICATIONS).AddColumn("SubmittedById").AsInt32().Nullable();
            Execute.Sql("UPDATE [" + TableNames.ABSENCE_NOTIIFICATIONS +
                        "] SET SubmittedById = (Select RecID from tblPermissions where username = SubmittedBy)");
            Alter.Table(TableNames.ABSENCE_NOTIIFICATIONS)
                 .AlterForeignKeyColumn("SubmittedById", "tblPermissions", "RecId", false);
            Delete.Column("SubmittedBy").FromTable(TableNames.ABSENCE_NOTIIFICATIONS);
            this.AddNotificationType("Operations", "Management", "Absence Notification Entry");
            this.AddNotificationType("Operations", "Management", "Absence Notification Supervisor");
            Execute.Sql(
                @"insert into NotificationConfigurations(ContactID, OperatingCenterID, NotificationPurposeID) Select (select ContactID from Contacts where Email = 'arystrom@mmsinc.com'),
											  (Select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ7'),
											  (Select NotificationPurposeId from NotificationPurposes where Purpose = 'Absence Notification Supervisor')");
            this.AddNotificationType("Operations", "Management", "FMLA Case");
            Execute.Sql(
                @"insert into NotificationConfigurations(ContactID, OperatingCenterID, NotificationPurposeID) Select (select ContactID from Contacts where Email = 'arystrom@mmsinc.com'),
											  (Select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ7'),
											  (Select NotificationPurposeId from NotificationPurposes where Purpose = 'FMLA Case')");
            Alter.Column("DateAdded").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsDateTime().NotNullable();
            Alter.Column("Occurrence").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsBoolean().NotNullable();
            Alter.Column("HRReviewed").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsBoolean().NotNullable();
            Alter.Column("CertificationExtended").OnTable(TableNames.FMLA_CASES).AsBoolean().NotNullable();
            Alter.Column("SendFMLAPackage").OnTable(TableNames.FMLA_CASES).AsBoolean().NotNullable();
            Alter.Column("ChronicCondition").OnTable(TableNames.FMLA_CASES).AsBoolean().NotNullable();
            Alter.Table(TableNames.ABSENCE_NOTIIFICATIONS).AddColumn("ReturnToWorkNote").AsBoolean().Nullable();
            Execute.Sql("UPDATE [" + TableNames.ABSENCE_NOTIIFICATIONS + "] SET ReturnToWorkNote = 0");
            Alter.Column("ReturnToWorkNote").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("ReturnToWorkNote").FromTable(TableNames.ABSENCE_NOTIIFICATIONS);
            Alter.Column("CertificationExtended").OnTable(TableNames.FMLA_CASES).AsBoolean().Nullable();
            Alter.Column("SendFMLAPackage").OnTable(TableNames.FMLA_CASES).AsBoolean().Nullable();
            Alter.Column("ChronicCondition").OnTable(TableNames.FMLA_CASES).AsBoolean().Nullable();
            Alter.Column("DateAdded").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsDateTime().Nullable();
            Alter.Column("Occurrence").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsBoolean().Nullable();
            Alter.Column("HRReviewed").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsBoolean().Nullable();
            Execute.Sql(
                "delete from NotificationConfigurations where ContactId = (select ContactID from Contacts where Email = 'arystrom@mmsinc.com') AND OperatingCenterID = (Select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ7') AND NotificationPurposeID = (Select NotificationPurposeId from NotificationPurposes where Purpose = 'FMLA Case')");
            this.RemoveNotificationType("Operations", "Management", "FMLA Case");
            Execute.Sql(
                "delete from NotificationConfigurations where ContactId = (select ContactID from Contacts where Email = 'arystrom@mmsinc.com') AND OperatingCenterID = (Select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'NJ7') AND NotificationPurposeID = (Select NotificationPurposeId from NotificationPurposes where Purpose = 'Absence Notification Supervisor')");
            this.RemoveNotificationType("Operations", "Management", "Absence Notification Supervisor");
            this.RemoveNotificationType("Operations", "Management", "Absence Notification Entry");
            //SubmittedBy
            Alter.Table(TableNames.ABSENCE_NOTIIFICATIONS).AddColumn("SubmittedBy").AsAnsiString(50).Nullable();
            Execute.Sql("UPDATE [" + TableNames.ABSENCE_NOTIIFICATIONS +
                        "] SET SubmittedBy = (Select UserName from tblPermissions where RecID = SubmittedById)");
            Alter.Column("SubmittedBy").OnTable(TableNames.ABSENCE_NOTIIFICATIONS).AsAnsiString(50).NotNullable();
            Delete.ForeignKeyColumn(TableNames.ABSENCE_NOTIIFICATIONS, "SubmittedById", "tblPermissions", "RecId");

            // EmployeeAbsenceClaimType
            this.ReplaceLookupTableLookup(
                TableNames.ABSENCE_NOTIIFICATIONS,
                ColumnNames.EMPLOYEE_ABSENCE_CLAIM,
                TableNames.EMPLOYEE_ABSENCE_CLAIM_TYPES,
                50,
                ColumnNames.EMPLOYEE_ABSENCE_CLAIM);
            Alter.Table(TableNames.ABSENCE_NOTIIFICATIONS)
                 .AlterForeignKeyColumn(ColumnNames.EMPLOYEE_ABSENCE_CLAIM, "Lookup", "LookupId");

            this.ReplaceLookupTableLookup(
                TableNames.FMLA_CASES,
                ColumnNames.COMPANY_ABSENCE_CERTIFICATION,
                TableNames.COMPANY_ABSENCE_CERTIFICATIONS,
                50,
                ColumnNames.COMPANY_ABSENCE_CERTIFICATION);
            Alter.Table(TableNames.FMLA_CASES)
                 .AlterForeignKeyColumn(ColumnNames.COMPANY_ABSENCE_CERTIFICATION, "Lookup", "LookupId");
        }
    }
}
