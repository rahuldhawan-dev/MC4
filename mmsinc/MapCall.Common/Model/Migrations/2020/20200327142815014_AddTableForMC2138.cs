using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200327142815014), Tags("Production")]
    public class AddTableForMC2138 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("CovidRequestTypes", "Travel Related", "Sick", "Testing",
                "Contact of Contact", "Direct Contact", "Contractors", "Meetings", "Waiver Request", "Waiver General",
                "General");
            this.CreateLookupTableWithValues("CovidSubmissionStatuses", "New", "In Process", "Complete");
            this.CreateLookupTableWithValues("CovidOutcomeCategories", "Question Answered", "Cleared for Work",
                "Quarantine Travel", "Quarantine Contact", "Quarantine Positive");
            this.CreateLookupTableWithValues("CovidQuarantineStatuses", "Quarantined", "Not Quarantined", "Released");

            Create.Table("CovidIssues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeID", nullable: false)
                  .WithColumn("SupervisorsCell").AsAnsiString(22).Nullable()
                  .WithColumn("LocalEmployeeRelationsBusinessPartner").AsAnsiString(50).Nullable()
                  .WithColumn("LocalEmployeeRelationsBusinessPartnerCell").AsAnsiString(22).Nullable()
                  .WithColumn("ReportingLocation").AsAnsiString(255).Nullable()
                  .WithForeignKeyColumn("RequestTypeId", "CovidRequestTypes", nullable: false)
                  .WithColumn("SubmissionDate").AsDateTime().NotNullable()
                  .WithColumn("QuestionFromEmail").AsCustom("varchar(max)").NotNullable()
                  .WithForeignKeyColumn("SubmissionStatusId", "CovidSubmissionStatuses", nullable: false)
                  .WithColumn("OutcomeDescription").AsCustom("varchar(max)").Nullable()
                  .WithForeignKeyColumn("OutcomeCategoryId", "CovidOutcomeCategories", nullable: true)
                  .WithForeignKeyColumn("QuarantineStatusId", "CovidQuarantineStatuses")
                  .WithColumn("QuarantineStartDate").AsDateTime().Nullable()
                  .WithColumn("QuarantineReleaseDate").AsDateTime().Nullable()
                  .WithColumn("QuarantineReason").AsCustom("varchar(max)").Nullable()
                  .WithColumn("PersonalEmailAddress").AsAnsiString(255).Nullable();

            Execute.Sql("INSERT INTO Modules(ApplicationId, Name) VALUES(3, 'Covid')");

            this.AddDataType("CovidIssues");
            this.AddDocumentType("Photograph", "CovidIssues");
            this.AddDocumentType("Employee Communication", "CovidIssues");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Photograph", "CovidIssues");
            this.RemoveDocumentTypeAndAllRelatedDocuments("Employee Communication", "CovidIssues");
            this.RemoveDataType("CovidIssues");
            Execute.Sql("DELETE Roles WHERE ModuleID = 81");
            Execute.Sql("DELETE FROM Modules WHERE ApplicationId = 3 AND Name = 'Covid'");
            Delete.Table("CovidIssues");
            Delete.Table("CovidQuarantineStatuses");
            Delete.Table("CovidOutcomeCategories");
            Delete.Table("CovidSubmissionStatuses");
            Delete.Table("CovidRequestTypes");
        }
    }
}
