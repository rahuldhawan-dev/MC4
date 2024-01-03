using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220720101340627), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4755 : Migration
    {
        public override void Up()
        {
            /* 1. public water supplies */

            Delete.Column("DVCode").FromTable("PublicWaterSupplies");
            Delete.Column("IsNewAcquisition").FromTable("PublicWaterSupplies");
            Delete.ForeignKey("FK_PublicWaterSupplies_tblEmployee_ExecutiveDirectorEmployeeId")
                  .OnTable("PublicWaterSupplies");
            Delete.Column("ExecutiveDirectorEmployeeId").FromTable("PublicWaterSupplies");
            Rename.Column("IsConsentOrder").OnTable("PublicWaterSupplies").To("HasConsentOrder");

            Alter.Table("PublicWaterSupplies").AddColumn("DateOfOwnership").AsDateTime().Nullable();
            Alter.Table("PublicWaterSupplies").AddColumn("ConsentOrderStartDate").AsDateTime().Nullable();
            Alter.Table("PublicWaterSupplies").AddColumn("ConsentOrderEndDate").AsDateTime().Nullable();
            Alter.Table("PublicWaterSupplies").AddColumn("NewSystemInitialSafetyAssessmentCompleted").AsDateTime().Nullable();
            Alter.Table("PublicWaterSupplies").AddColumn("DateSafetyAssessmentActionItemsCompleted").AsDateTime().Nullable();
            Alter.Table("PublicWaterSupplies").AddColumn("NewSystemInitialWQEnvAssessmentCompleted").AsDateTime().Nullable();
            Alter.Table("PublicWaterSupplies").AddColumn("DateWQEnvAssessmentActionItemsCompleted").AsDateTime().Nullable();

            /* 2. waste water systems */

            Delete.Column("IsNewAcquisition").FromTable("WasteWaterSystems");
            Rename.Column("IsConsentOrder").OnTable("WasteWaterSystems").To("HasConsentOrder");
            Alter.Table("WasteWaterSystems").AddColumn("ConsentOrderStartDate").AsDateTime().Nullable();
            Alter.Table("WasteWaterSystems").AddColumn("ConsentOrderEndDate").AsDateTime().Nullable();
            Alter.Table("WasteWaterSystems").AddColumn("NewSystemInitialSafetyAssessmentCompleted").AsDateTime().Nullable();
            Alter.Table("WasteWaterSystems").AddColumn("DateSafetyAssessmentActionItemsCompleted").AsDateTime().Nullable();
            Alter.Table("WasteWaterSystems").AddColumn("NewSystemInitialWQEnvAssessmentCompleted").AsDateTime().Nullable();
            Alter.Table("WasteWaterSystems").AddColumn("DateWQEnvAssessmentActionItemsCompleted").AsDateTime().Nullable();

            /* 3. document types */

            this.AddDocumentType("Safety Assessment", "PublicWaterSupplies");
            this.AddDocumentType("WQ/Environmental Assessment", "PublicWaterSupplies");
            this.AddDocumentType("Consent Order Documents", "PublicWaterSupplies");

            this.AddDocumentType("Safety Assessment", "WasteWaterSystems");
            this.AddDocumentType("WQ/Environmental Assessment", "WasteWaterSystems");
            this.AddDocumentType("Consent Order Documents", "WasteWaterSystems");
        }

        public override void Down()
        {
            /* 3. document types */

            this.RemoveDocumentTypeAndAllRelatedDocuments("Safety Assessment", "PublicWaterSupplies");
            this.RemoveDocumentTypeAndAllRelatedDocuments("WQ/Environmental Assessment", "PublicWaterSupplies");
            this.RemoveDocumentTypeAndAllRelatedDocuments("Consent Order Documents", "PublicWaterSupplies");

            this.RemoveDocumentTypeAndAllRelatedDocuments("Safety Assessment", "WasteWaterSystems");
            this.RemoveDocumentTypeAndAllRelatedDocuments("WQ/Environmental Assessment", "WasteWaterSystems");
            this.RemoveDocumentTypeAndAllRelatedDocuments("Consent Order Documents", "WasteWaterSystems");
            
            /* 2. waste water systems */

            Alter.Table("WasteWaterSystems").AddColumn("IsNewAcquisition").AsBoolean().Nullable();
            Rename.Column("HasConsentOrder").OnTable("WasteWaterSystems").To("IsConsentOrder");
            Delete.Column("ConsentOrderStartDate").FromTable("WasteWaterSystems");
            Delete.Column("ConsentOrderEndDate").FromTable("WasteWaterSystems");
            Delete.Column("NewSystemInitialSafetyAssessmentCompleted").FromTable("WasteWaterSystems");
            Delete.Column("DateSafetyAssessmentActionItemsCompleted").FromTable("WasteWaterSystems");
            Delete.Column("NewSystemInitialWQEnvAssessmentCompleted").FromTable("WasteWaterSystems");
            Delete.Column("DateWQEnvAssessmentActionItemsCompleted").FromTable("WasteWaterSystems");

            /* 1. public water supplies */

            Alter.Table("PublicWaterSupplies").AddColumn("DvCode").AsAnsiString(15);
            Alter.Table("PublicWaterSupplies").AddColumn("IsNewAcquisition").AsBoolean();
            Alter.Table("PublicWaterSupplies").AddForeignKeyColumn("ExecutiveDirectorEmployeeId", "tblEmployee", "tblEmployeeId");
            Rename.Column("HasConsentOrder").OnTable("PublicWaterSupplies").To("IsConsentOrder");
            Delete.Column("DateOfOwnership").FromTable("PublicWaterSupplies");
            Delete.Column("ConsentOrderStartDate").FromTable("PublicWaterSupplies");
            Delete.Column("ConsentOrderEndDate").FromTable("PublicWaterSupplies");
            Delete.Column("NewSystemInitialSafetyAssessmentCompleted").FromTable("PublicWaterSupplies");
            Delete.Column("DateSafetyAssessmentActionItemsCompleted").FromTable("PublicWaterSupplies");
            Delete.Column("NewSystemInitialWQEnvAssessmentCompleted").FromTable("PublicWaterSupplies");
            Delete.Column("DateWQEnvAssessmentActionItemsCompleted").FromTable("PublicWaterSupplies");
        }
    }
}

