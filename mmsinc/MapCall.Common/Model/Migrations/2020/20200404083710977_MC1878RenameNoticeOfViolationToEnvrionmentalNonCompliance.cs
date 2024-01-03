using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200404083710977)]
    [Tags("Production")]
    public class MC1878RenameNoticeOfViolationToEnvironmentalNonCompliance : Migration
    {
        #region Exposed Methods

        public override void Up()
        {
            // rename the tables
            Rename.Table("NoticesOfViolation").To("EnvironmentalNonComplianceEvents");
            Rename.Table("NoticeOfViolationActionItems").To("EnvironmentalNonComplianceEventActionItems");
            Rename.Table("NoticeOfViolationActionItemTypes").To("EnvironmentalNonComplianceEventActionItemTypes");
            Rename.Table("NoticeOfViolationEntityLevels").To("EnvironmentalNonComplianceEventEntityLevels");
            Rename.Table("NoticeOfViolationFailureTypes").To("EnvironmentalNonComplianceEventFailureTypes");
            Rename.Table("NoticeOfViolationPrimaryRootCauses").To("EnvironmentalNonComplianceEventPrimaryRootCauses");
            Rename.Table("NoticeOfViolationResponsibilities").To("EnvironmentalNonComplianceEventResponsibilities");
            Rename.Table("NoticeOfViolationStatuses").To("EnvironmentalNonComplianceEventStatuses");
            Rename.Table("NoticeOfViolationSubTypes").To("EnvironmentalNonComplianceEventSubTypes");
            Rename.Table("NoticeOfViolationTypes").To("EnvironmentalNonComplianceEventTypes");

            // Columns
            Rename.Column("ResponsibilityName").OnTable("EnvironmentalNonComplianceEvents").To("NameOfThirdParty");
            Rename.Column("IssuingEntityName").OnTable("EnvironmentalNonComplianceEvents").To("NameOfEntity");
            Rename.Column("NoticeOfViolationId").OnTable("EnvironmentalNonComplianceEventActionItems")
                  .To("EnvironmentalNonComplianceEventId");
            Rename.Column("NoticeOfViolationTypeId").OnTable("EnvironmentalNonComplianceEventSubTypes")
                  .To("EnvironmentalNonComplianceEventTypeId");
            Alter.Column("FineAmount").OnTable("EnvironmentalNonComplianceEvents").AsDecimal(19, 2).Nullable();
            Alter.Column("IssuingEntityId").OnTable("EnvironmentalNonComplianceEvents").AsInt32().Nullable();

            // Add new Issue year column
            Alter.Table("EnvironmentalNonComplianceEvents").AddColumn("IssueYear").AsInt32().Nullable();
        }

        public override void Down()
        {
            // Reverse the renaming
            Rename.Table("EnvironmentalNonComplianceEvents").To("NoticesOfViolation");
            Rename.Table("EnvironmentalNonComplianceEventActionItems").To("NoticeOfViolationActionItems");
            Rename.Table("EnvironmentalNonComplianceEventActionItemTypes").To("NoticeOfViolationActionItemTypes");
            Rename.Table("EnvironmentalNonComplianceEventEntityLevels").To("NoticeOfViolationEntityLevels");
            Rename.Table("EnvironmentalNonComplianceEventFailureTypes").To("NoticeOfViolationFailureTypes");
            Rename.Table("EnvironmentalNonComplianceEventPrimaryRootCauses").To("NoticeOfViolationPrimaryRootCauses");
            Rename.Table("EnvironmentalNonComplianceEventResponsibilities").To("NoticeOfViolationResponsibilities");
            Rename.Table("EnvironmentalNonComplianceEventStatuses").To("NoticeOfViolationStatuses");
            Rename.Table("EnvironmentalNonComplianceEventSubTypes").To("NoticeOfViolationSubTypes");
            Rename.Table("EnvironmentalNonComplianceEventTypes").To("NoticeOfViolationTypes");
            Rename.Column("NameOfThirdParty").OnTable("NoticesOfViolation").To("ResponsibilityName");
            Rename.Column("NameOfEntity").OnTable("NoticesOfViolation").To("IssuingEntityName");
            Rename.Column("EnvironmentalNonComplianceEventId").OnTable("NoticeOfViolationActionItems")
                  .To("NoticeOfViolationId");
            Rename.Column("EnvironmentalNonComplianceEventTypeId").OnTable("NoticeOfViolationSubTypes")
                  .To("NoticeOfViolationTypeId");
            Alter.Column("FineAmount").OnTable("NoticesOfViolation").AsDecimal(19, 5).Nullable();
            Alter.Column("IssuingEntityId").OnTable("NoticesOfViolation").AsInt32().NotNullable();
            Delete.Column("IssueYear").FromTable("NoticesOfViolation");
        }

        #endregion
    }
}
