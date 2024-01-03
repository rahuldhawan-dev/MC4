using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190903090455883), Tags("Production")]
    public class CreateNoticeOfViolationTablesForMC1617 : Migration
    {
        public override void Up()
        {
            //Create Action Item Type table - editable
            this.CreateLookupTableWithValues("NoticeOfViolationActionItemTypes", "Capital Improvement",
                "Contractor Outreach",
                "Education", "Not Listed", "PM Plan Creation/Modify", "SOP Creation/Modify", "Tap Root Analysis");
            //Create Failure To table - editable
            this.CreateLookupTableWithValues("NoticeOfViolationFailureTypes", "Failure to meet limit",
                "Failure to monitor",
                "Failure to report", "Failure to report timely", "Failure to implement worksite controls",
                "Failure to maintain", "Failure to document", "Failure to file", "Failure to follow regulations",
                "Failure to collect", "Failure to oversight");
            //Create NOV Status table
            this.CreateLookupTableWithValues("NoticeOfViolationStatuses", "Pending", "Confirmed", "Rescinded");
            //Create NOV Type table - editable
            this.CreateLookupTableWithValues("NoticeOfViolationTypes", "Drinking Water NOV", "Water (Non DW) NOV",
                "Wastewater NOV", "Environmental");
            //Create NOV SubType table - editable
            Create.Table("NoticeOfViolationSubTypes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithForeignKeyColumn("NoticeOfViolationTypeId", "NoticeOfViolationTypes");
            //Create Primary Root Cause table - editable
            this.CreateLookupTableWithValues("NoticeOfViolationPrimaryRootCauses", "Lack of Communication",
                "Lack of Education \\ Knowledge", "Lack of SOP", "Failure to follow SOP", "Failure of equipment",
                "Lack of adequate oversight", "Contractor adherence to regulations",
                "Lack of adequate tracking system");
            //Create Entity Level table - editable
            this.CreateLookupTableWithValues("NoticeOfViolationEntityLevels", "EPA", "State", "County", "OSHA",
                "Other");
            //Create Responsibility table - editable
            this.CreateLookupTableWithValues("NoticeOfViolationResponsibilities", "American Water", "Third Party",
                "New Acquisition");

            //Create NOV table - Main table
            Create.Table("NoticesOfViolation")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("StateId", "States", "StateId", false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithForeignKeyColumn("SystemId", "PublicWaterSupplies", nullable: false)
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems")
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", false)
                  .WithColumn("DateReported").AsDateTime().NotNullable()
                  .WithColumn("AwarenessDate").AsDateTime().NotNullable()
                  .WithColumn("NoticeOfViolationDate").AsDateTime().NotNullable()
                  .WithColumn("DateFinalized").AsDateTime().Nullable()
                  .WithColumn("EnforcementDate").AsDateTime().Nullable()
                  .WithForeignKeyColumn("TypeId", "NoticeOfViolationTypes", nullable: false)
                  .WithForeignKeyColumn("SubTypeId", "NoticeOfViolationSubTypes")
                  .WithForeignKeyColumn("ResponsibilityId", "NoticeOfViolationResponsibilities", nullable: false)
                  .WithColumn("ResponsibilityName").AsAnsiString(50).Nullable()
                  .WithColumn("SummaryOfEvent").AsCustom("nvarchar(MAX)").NotNullable()
                  .WithForeignKeyColumn("PrimaryRootCauseId", "NoticeOfViolationPrimaryRootCauses")
                  .WithForeignKeyColumn("FailureTypeId", "NoticeOfViolationFailureTypes")
                  .WithColumn("FailureTypeDescription").AsAnsiString(255).Nullable()
                  .WithForeignKeyColumn("StatusId", "NoticeOfViolationStatuses", nullable: false)
                  .WithColumn("InitialCategory").AsAnsiString(50).Nullable()
                  .WithColumn("FinalizedCategory").AsAnsiString(50).Nullable()
                  .WithColumn("FineAmount").AsDecimal().Nullable()
                  .WithForeignKeyColumn("IssuingEntityId", "NoticeOfViolationEntityLevels", nullable: false)
                  .WithColumn("IssuingEntityName").AsAnsiString(50).Nullable();

            //Create Action Item table - separate tab on Notice of Violation page
            Create.Table("NoticeOfViolationActionItems")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("NoticeOfViolationId", "NoticesOfViolation", nullable: false)
                  .WithForeignKeyColumn("ActionItemTypeId", "NoticeOfViolationActionItemTypes", nullable: false)
                  .WithColumn("NotListedType").AsAnsiString(50).Nullable()
                  .WithColumn("ActionItem").AsAnsiString(255).NotNullable()
                  .WithForeignKeyColumn("ResponsibleOwnerId", "tblPermissions", "RecId")
                  .WithColumn("TargetedCompletionDate").AsDateTime().NotNullable()
                  .WithColumn("DateCompleted").AsDateTime().Nullable();

            this.AddDataType("NoticesOfViolation");

            this.AddDocumentType("Notice of Violation Document", "NoticesOfViolation");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Notice of Violation Document", "NoticesOfViolation");
            this.RemoveDataType("NoticesOfViolation");

            Delete.Table("NoticeOfViolationActionItems");
            Delete.Table("NoticesOfViolation");
            Delete.Table("NoticeOfViolationResponsibilities");
            Delete.Table("NoticeOfViolationEntityLevels");
            Delete.Table("NoticeOfViolationPrimaryRootCauses");
            Delete.Table("NoticeOfViolationSubTypes");
            Delete.Table("NoticeOfViolationTypes");
            Delete.Table("NoticeOfViolationStatuses");
            Delete.Table("NoticeOfViolationFailureTypes");
            Delete.Table("NoticeOfViolationActionItemTypes");
        }
    }
}
