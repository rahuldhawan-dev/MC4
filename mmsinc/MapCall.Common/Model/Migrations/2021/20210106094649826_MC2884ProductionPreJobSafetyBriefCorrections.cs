using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210106094649826), Tags("Production")]
    public class MC2884ProductionPreJobSafetyBriefCorrections : Migration
    {
        private const string PRODUCTION_PRE_JOB_SAFETY_BRIEFS = "ProductionPreJobSafetyBriefs";

        public override void Up()
        {
            Alter.Table(PRODUCTION_PRE_JOB_SAFETY_BRIEFS)
                 .AddColumn("IsSafetyDataSheetAvailableForEachChemicalNotes").AsAnsiString(255).Nullable()
                 .AddColumn("HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes").AsAnsiString(255).Nullable()
                 .AddColumn("HasStretchAndFlexBeenPerformedNotes").AsAnsiString(255).Nullable()
                 .AddColumn("ReviewedLocationOfSafetyEquipmentNotes").AsAnsiString(255).Nullable()
                 .AddColumn("CrewMembersRemindedOfStopWorkAuthorityNotes").AsAnsiString(255).Nullable();

            Delete.Column("HadDiscussionAboutHazardsAndPrecautions").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
            Delete.Column("HadDiscussionAboutHazardsAndPrecautionsNotes").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
        }

        public override void Down()
        {
            Create.Column("HadDiscussionAboutHazardsAndPrecautionsNotes").OnTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS).AsAnsiString(255).Nullable();
            Create.Column("HadDiscussionAboutHazardsAndPrecautions").OnTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS).AsBoolean().Nullable();
            Delete.Column("CrewMembersRemindedOfStopWorkAuthorityNotes").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
            Delete.Column("ReviewedLocationOfSafetyEquipmentNotes").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
            Delete.Column("HasStretchAndFlexBeenPerformedNotes").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
            Delete.Column("HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
            Delete.Column("IsSafetyDataSheetAvailableForEachChemicalNotes").FromTable(PRODUCTION_PRE_JOB_SAFETY_BRIEFS);
        }
    }
}