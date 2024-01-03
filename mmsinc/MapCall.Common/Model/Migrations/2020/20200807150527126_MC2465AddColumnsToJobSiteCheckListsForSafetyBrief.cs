using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200807150527126), Tags("Production")]
    public class MC2465AddColumnsToJobSiteCheckListsForSafetyBrief : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("JobSiteCheckListSafetyBriefWeatherHazardTypes", "Wet", "Snow", "Ice",
                "ExcessiveHeat", "Excessive Cold", "Excessive Winds");
            Create.Table("JobSiteCheckListSafetyBriefWeatherHazardAnswers")
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable().Indexed("IX_JobSiteCheckListId")
                  .WithColumn("JobSiteCheckListSafetyBriefWeatherHazardTypeId").AsInt32().Nullable();

            this.CreateLookupTableWithValues("JobSiteCheckListSafetyBriefTimeOfDayConstraintTypes",
                "Inadequate Lighting", "Poor Visibility", "Sun Glare", "Fog", "Darkness");
            Create.Table("JobSiteCheckListSafetyBriefTimeOfDayConstraintAnswers")
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable().Indexed("IX_JobSiteCheckListId")
                  .WithColumn("JobSiteCheckListSafetyBriefTimeOfDayConstraintTypeId").AsInt32().Nullable();

            this.CreateLookupTableWithValues("JobSiteCheckListSafetyBriefTrafficHazardTypes", "In or near roadway",
                "Limited shoulder space", "High speed roadway", "Work in roadway");
            Create.Table("JobSiteCheckListSafetyBriefTrafficHazardAnswers")
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable().Indexed("IX_JobSiteCheckListId")
                  .WithColumn("JobSiteCheckListSafetyBriefTrafficHazardTypeId").AsInt32().Nullable();

            this.CreateLookupTableWithValues("JobSiteCheckListSafetyBriefOverheadHazardTypes", "Electrical Wires",
                "Tree Limbs");
            Create.Table("JobSiteCheckListSafetyBriefOverheadHazardAnswers")
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable().Indexed("IX_JobSiteCheckListId")
                  .WithColumn("JobSiteCheckListSafetyBriefOverheadHazardTypeId").AsInt32().Nullable();

            this.CreateLookupTableWithValues("JobSiteCheckListSafetyBriefUndergroundHazardTypes", "Gas", "Electrical",
                "Sewer", "Ground Water Level", "Soil Type", "Communication Lines");
            Create.Table("JobSiteCheckListSafetyBriefUndergroundHazardAnswers")
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable().Indexed("IX_JobSiteCheckListId")
                  .WithColumn("JobSiteCheckListSafetyBriefUndergroundHazardTypeId").AsInt32().Nullable();

            this.CreateLookupTableWithValues("JobSiteCheckListSafetyBriefElectricalHazardTypes", "Improper grounding",
                "High Voltage");
            Create.Table("JobSiteCheckListSafetyBriefElectricalHazardAnswers")
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable().Indexed("IX_JobSiteCheckListId")
                  .WithColumn("JobSiteCheckListSafetyBriefElectricalHazardTypeId").AsInt32().Nullable();

            Alter.Table("JobSiteCheckLists")
                 .AddColumn("SafetyBriefDateTime").AsDateTime().Nullable()
                 .AddColumn("AnyPotentialWeatherHazards").AsBoolean().Nullable()
                 .AddColumn("HadConversationAboutWeatherHazard").AsBoolean().Nullable()
                 .AddColumn("HadConversationAboutWeatherHazardNotes").AsAnsiString(255).Nullable()
                 .AddColumn("AnyTimeOfDayConstraints").AsBoolean().Nullable()
                 .AddColumn("AnyTrafficHazards").AsBoolean().Nullable()
                 .AddColumn("InvolveConfinedSpace").AsBoolean().Nullable()
                 .AddColumn("AnyPotentialOverheadHazards").AsBoolean().Nullable()
                 .AddColumn("AnyUndergroundHazards").AsBoolean().Nullable()
                 .AddColumn("AreThereElectricalHazards").AsBoolean().Nullable()
                 .AddColumn("WorkingWithACPipe").AsBoolean().Nullable()
                 .AddColumn("CrewMembersTrainedInACPipe").AsBoolean().Nullable()
                 .AddColumn("HaveEquipmentToDoJobSafely").AsBoolean().Nullable()
                 .AddColumn("HaveEquipmentToDoJobSafelyNotes").AsAnsiString(255).Nullable()
                 .AddColumn("ReviewedErgonomicHazards").AsBoolean().Nullable()
                 .AddColumn("ReviewedErgonomicHazardsNotes").AsAnsiString(255).Nullable()
                 .AddColumn("ReviewedLocationOfSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("OtherHazardsIdentified").AsBoolean().Nullable()
                 .AddColumn("OtherHazardNotes").AsAnsiString(255).Nullable()
                 .AddColumn("HadDiscussionAboutHazardsAndPrecautions").AsBoolean().Nullable()
                 .AddColumn("HadDiscussionAboutHazardsAndPrecautionsNotes").AsAnsiString(255).Nullable()
                 .AddColumn("CrewMembersRemindedOfStopWorkAuthority").AsBoolean().Nullable()
                 .AddColumn("HeadProtection").AsBoolean().Nullable()
                 .AddColumn("HandProtection").AsBoolean().Nullable()
                 .AddColumn("ElectricalProtection").AsBoolean().Nullable()
                 .AddColumn("FootProtection").AsBoolean().Nullable()
                 .AddColumn("EyeProtection").AsBoolean().Nullable()
                 .AddColumn("FaceShield").AsBoolean().Nullable()
                 .AddColumn("SafetyGarment").AsBoolean().Nullable()
                 .AddColumn("HearingProtection").AsBoolean().Nullable()
                 .AddColumn("RespiratoryProtection").AsBoolean().Nullable()
                 .AddColumn("PPEOther").AsBoolean().Nullable()
                 .AddColumn("PPEOtherNotes").AsAnsiString(255).Nullable();

            // Want to seperate out the new fields and the fields converted to nullable
            Alter.Column("AllEmployeesWearingAppropriatePersonalProtectionEquipment").OnTable("JobSiteCheckLists")
                 .AsBoolean().Nullable();
            Alter.Column("AllStructuresSupportedOrProtected").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("HasExcavation").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("IsMarkoutValidForSite").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("AllMaterialsSetBackFromEdgeOfTrenches").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("WaterControlSystemsInUse").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("AreExposedUtilitiesProtected").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("IsEmergencyMarkoutRequest").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
            Alter.Column("CompliesWithStandards").OnTable("JobSiteCheckLists").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Table("JobSiteCheckListSafetyBriefWeatherHazardAnswers");
            Delete.Table("JobSiteCheckListSafetyBriefWeatherHazardTypes");
            Delete.Table("JobSiteCheckListSafetyBriefTimeOfDayConstraintAnswers");
            Delete.Table("JobSiteCheckListSafetyBriefTimeOfDayConstraintTypes");
            Delete.Table("JobSiteCheckListSafetyBriefTrafficHazardAnswers");
            Delete.Table("JobSiteCheckListSafetyBriefTrafficHazardTypes");
            Delete.Table("JobSiteCheckListSafetyBriefOverheadHazardAnswers");
            Delete.Table("JobSiteCheckListSafetyBriefOverheadHazardTypes");
            Delete.Table("JobSiteCheckListSafetyBriefUndergroundHazardAnswers");
            Delete.Table("JobSiteCheckListSafetyBriefUndergroundHazardTypes");
            Delete.Table("JobSiteCheckListSafetyBriefElectricalHazardAnswers");
            Delete.Table("JobSiteCheckListSafetyBriefElectricalHazardTypes");

            Delete.Column("SafetyBriefDateTime").FromTable("JobSiteCheckLists");
            Delete.Column("AnyPotentialWeatherHazards").FromTable("JobSiteCheckLists");
            Delete.Column("HadConversationAboutWeatherHazard").FromTable("JobSiteCheckLists");
            Delete.Column("HadConversationAboutWeatherHazardNotes").FromTable("JobSiteCheckLists");
            Delete.Column("AnyTimeOfDayConstraints").FromTable("JobSiteCheckLists");
            Delete.Column("AnyTrafficHazards").FromTable("JobSiteCheckLists");
            Delete.Column("InvolveConfinedSpace").FromTable("JobSiteCheckLists");
            Delete.Column("AnyPotentialOverheadHazards").FromTable("JobSiteCheckLists");
            Delete.Column("AnyUndergroundHazards").FromTable("JobSiteCheckLists");
            Delete.Column("AreThereElectricalHazards").FromTable("JobSiteCheckLists");
            Delete.Column("WorkingWithACPipe").FromTable("JobSiteCheckLists");
            Delete.Column("CrewMembersTrainedInACPipe").FromTable("JobSiteCheckLists");
            Delete.Column("HaveEquipmentToDoJobSafely").FromTable("JobSiteCheckLists");
            Delete.Column("HaveEquipmentToDoJobSafelyNotes").FromTable("JobSiteCheckLists");
            Delete.Column("ReviewedErgonomicHazards").FromTable("JobSiteCheckLists");
            Delete.Column("ReviewedErgonomicHazardsNotes").FromTable("JobSiteCheckLists");
            Delete.Column("ReviewedLocationOfSafetyEquipment").FromTable("JobSiteCheckLists");
            Delete.Column("OtherHazardsIdentified").FromTable("JobSiteCheckLists");
            Delete.Column("OtherHazardNotes").FromTable("JobSiteCheckLists");
            Delete.Column("HadDiscussionAboutHazardsAndPrecautions").FromTable("JobSiteCheckLists");
            Delete.Column("HadDiscussionAboutHazardsAndPrecautionsNotes").FromTable("JobSiteCheckLists");
            Delete.Column("CrewMembersRemindedOfStopWorkAuthority").FromTable("JobSiteCheckLists");
            Delete.Column("HeadProtection").FromTable("JobSiteCheckLists");
            Delete.Column("HandProtection").FromTable("JobSiteCheckLists");
            Delete.Column("ElectricalProtection").FromTable("JobSiteCheckLists");
            Delete.Column("FootProtection").FromTable("JobSiteCheckLists");
            Delete.Column("EyeProtection").FromTable("JobSiteCheckLists");
            Delete.Column("FaceShield").FromTable("JobSiteCheckLists");
            Delete.Column("SafetyGarment").FromTable("JobSiteCheckLists");
            Delete.Column("HearingProtection").FromTable("JobSiteCheckLists");
            Delete.Column("RespiratoryProtection").FromTable("JobSiteCheckLists");
            Delete.Column("PPEOther").FromTable("JobSiteCheckLists");
            Delete.Column("PPEOtherNotes").FromTable("JobSiteCheckLists");

            // Want to seperate out the new fields and the fields converted back to not nullable
            Execute.Sql(
                $"UPDATE JobSiteCheckLists SET AllEmployeesWearingAppropriatePersonalProtectionEquipment = 0,AllStructuresSupportedOrProtected = 0,HasExcavation = 0,IsMarkoutValidForSite = 0,AllMaterialsSetBackFromEdgeOfTrenches = 0,WaterControlSystemsInUse = 0,AreExposedUtilitiesProtected = 0,IsEmergencyMarkoutRequest = 0, CompliesWithStandards = 0 WHERE AllEmployeesWearingAppropriatePersonalProtectionEquipment IS NULL OR AllStructuresSupportedOrProtected IS NULL OR HasExcavation IS NULL OR IsMarkoutValidForSite IS NULL OR AllMaterialsSetBackFromEdgeOfTrenches IS NULL OR WaterControlSystemsInUse IS NULL OR AreExposedUtilitiesProtected IS NULL OR IsEmergencyMarkoutRequest IS NULL OR CompliesWithStandards IS NULL ");
            Alter.Column("AllEmployeesWearingAppropriatePersonalProtectionEquipment").OnTable("JobSiteCheckLists")
                 .AsBoolean().NotNullable();
            Alter.Column("AllStructuresSupportedOrProtected").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
            Alter.Column("HasExcavation").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
            Alter.Column("IsMarkoutValidForSite").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
            Alter.Column("AllMaterialsSetBackFromEdgeOfTrenches").OnTable("JobSiteCheckLists").AsBoolean()
                 .NotNullable();
            Alter.Column("WaterControlSystemsInUse").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
            Alter.Column("AreExposedUtilitiesProtected").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
            Alter.Column("IsEmergencyMarkoutRequest").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
            Alter.Column("CompliesWithStandards").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
        }
    }
}
