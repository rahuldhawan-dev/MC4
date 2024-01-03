using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170724105528555), Tags("Production")]
    public class Bug3940 : Migration
    {
        public override void Up()
        {
            // This table acts like a bool but bug-3940 asks to allow additional values to be added for this later.
            this.CreateLookupTableWithValues("JobSiteCheckListPressurizedRiskRestrainedTypes", "Yes", "No");
            this.CreateLookupTableWithValues("JobSiteCheckListNoRestraintReasonTypes", "Tapping Sleeve",
                "Restraint Calculator used and Restraint not needed", "Main Shut Down", "Fittings are Mega Lugged");
            this.CreateLookupTableWithValues("JobSiteCheckListRestraintMethodTypes", "Rods Installed", "Rods Inspected",
                "Key-way Installed");

            Alter.Table("JobSiteCheckLists")
                 .AddColumn("IsPressurizedRisksRestrainedFieldRequired").AsBoolean().NotNullable()
                 .WithDefaultValue(false)
                 .AddColumn("PressurizedRiskRestrainedTypeId").AsInt32().Nullable()
                 .ForeignKey(
                      "FK_JobSiteCheckLists_JobSiteCheckListPressurizedRiskRestrainedTypes_PressurizedRiskRestrainedTypeId",
                      "JobSiteCheckListPressurizedRiskRestrainedTypes", "Id")
                 .AddColumn("NoRestraintReasonTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_JobSiteCheckLists_JobSiteCheckListNoRestraintReasonTypes_NoRestraintReasonTypeId",
                      "JobSiteCheckListNoRestraintReasonTypes", "Id")
                 .AddColumn("RestraintMethodTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_JobSiteCheckLists_JobSiteCheckListRestraintMethodTypes_RestraintMethodTypeId",
                      "JobSiteCheckListRestraintMethodTypes", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey(
                       "FK_JobSiteCheckLists_JobSiteCheckListPressurizedRiskRestrainedTypes_PressurizedRiskRestrainedTypeId")
                  .OnTable("JobSiteCheckLists");
            Delete.ForeignKey("FK_JobSiteCheckLists_JobSiteCheckListNoRestraintReasonTypes_NoRestraintReasonTypeId")
                  .OnTable("JobSiteCheckLists");
            Delete.ForeignKey("FK_JobSiteCheckLists_JobSiteCheckListRestraintMethodTypes_RestraintMethodTypeId")
                  .OnTable("JobSiteCheckLists");

            Delete.Column("IsPressurizedRisksRestrainedFieldRequired").FromTable("JobSiteCheckLists");
            Delete.Column("PressurizedRiskRestrainedTypeId").FromTable("JobSiteCheckLists");
            Delete.Column("NoRestraintReasonTypeId").FromTable("JobSiteCheckLists");
            Delete.Column("RestraintMethodTypeId").FromTable("JobSiteCheckLists");

            Delete.Table("JobSiteCheckListPressurizedRiskRestrainedTypes");
            Delete.Table("JobSiteCheckListNoRestraintReasonTypes");
            Delete.Table("JobSiteCheckListRestraintMethodTypes");
        }
    }
}
