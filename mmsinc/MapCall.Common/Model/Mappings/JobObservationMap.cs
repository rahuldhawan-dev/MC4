using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobObservationMap : ClassMap<JobObservation>
    {
        public const string TABLE_NAME = "tblJobObservations";

        public JobObservationMap()
        {
            Table(TABLE_NAME);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("JobObservationID");
            References(x => x.Department).Column("JobCategory").Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.OverallSafetyRating).Column("OverallSafetyRating").Nullable();
            References(x => x.OverallQualityRating).Column("OverallQualityRating").Nullable();
            References(x => x.OperatingCenter).Column("OpCode").Nullable();
            References(x => x.JobObservedBy).Nullable();
            References(x => x.CreatedBy).Nullable();
            References(x => x.ProductionWorkOrder).Nullable();
            References(x => x.WorkOrder).Nullable();

            Map(x => x.ObservationDate);
            Map(x => x.Address).Column("Location").Length(JobObservation.StringLengths.ADDRESS);
            Map(x => x.TaskObserved).Column("Description");
            Map(x => x.WhyWasTaskSafeOrAtRisk).Column("Suggestions_to_Employees");
            Map(x => x.Deficiencies);
            Map(x => x.RecommendSolutions).Column("Comments");
            Map(x => x.EqTruckForkliftsHoistsLadders).Column("EQ_Truck_Forklifts_Hoists_Ladders");
            Map(x => x.EqFrontEndLoaderOrBackhoe).Column("EQ_Front_End_Loader_or_Backhoe");
            Map(x => x.EqOther).Column("EQ_Other");
            Map(x => x.CsPreEntryChecklistOrEntryPermit).Column("CS_Pre_Entry_Checklist_or_Entry_Permit");
            Map(x => x.CsAtmosphereContinuouslyMonitored).Column("CS_Atmosphere_Continuously_Monitored");
            Map(x => x.CsRetrievalEquipmentTripodHarnessWinch).Column("CS_Retrieval_Equipment_Tripod_Harness_Winch");
            Map(x => x.CsVentilationEquipment).Column("CS_Ventilation_Equipment");
            Map(x => x.PpeHardHat).Column("PPE_Hard_Hat");
            Map(x => x.PpeReflectiveVest).Column("PPE_Reflective_Vest");
            Map(x => x.PpeEyeProtection).Column("PPE_Eye_Protection");
            Map(x => x.PpeEarProtection).Column("PPE_Ear_Protection");
            Map(x => x.PpeFootProtection).Column("PPE_Foot_Protection");
            Map(x => x.PpeGloves).Column("PPE_Gloves");
            Map(x => x.TcBarricadesConesBarrels).Column("TC_Barricades_Cones_Barrels");
            Map(x => x.TcAdvancedWarningSigns).Column("TC_Advanced_Warning_Signs");
            Map(x => x.TcLightsArrowBoard).Column("TC_Lights_Arrow_Board");
            Map(x => x.TcPoliceFlagman).Column("TC_Police_Flagman");
            Map(x => x.TcWorkZoneInCompliance).Column("TC_Work_Zone_in_Compliance");
            Map(x => x.PsWalkwaysClear).Column("PS_Walkways_Clear");
            Map(x => x.PsMaterialStockpile).Column("PS_Material_Stockpile");
            Map(x => x.ExMarkoutRequestedForWorkSite).Column("EX_Markout_Requested_for_Work_Site");
            Map(x => x.ExWorkSiteSafetyCheckListUtilized).Column("EX_Work_Site_Safety_Check_List_Utilized");
            Map(x => x.ExUtilitiesSupportedProtected).Column("EX_Utilities_Supported_Protected");
            Map(x => x.ExAtmosphereTestingPerformed).Column("EX_Atmosphere_Testing_Performed");
            Map(x => x.ExSpoilPile2FeetFromEdgeOfExcavation).Column("EX_Spoil_Pile_2_Feet_From_Edge_Of_Excavation");
            Map(x => x.ExLadderUsedIfGreaterThan4FeetDeep).Column("EX_Ladder_Used_if_Greater_Than_4_Feet_Deep");
            Map(x => x.ExShoringNecessaryOver5FeetDeep).Column("EX_Shoring_Necessary_over_5_Feet_Deep");
            Map(x => x.ExProtectiveSystemInUseOver5Feet).Column("EX_Protective_System_in_use_over_5_Feet");
            Map(x => x.ExWaterControlSystemInUse).Column("EX_Water_Control_System_in_Use");
            Map(x => x.ErChecklistUtilized).Column("ER_Checklist_Utilized");
            Map(x => x.ErErgonomicFactorsProhibitingGoodBodyMechanics)
               .Column("ER_Ergonomic_Factors_prohibiting_good_body_mechanics");
            Map(x => x.ErToolsEquipmentUsedCorrectly).Column("ER_Tools_Equipment_used_correctly");

            HasMany(x => x.JobObservationDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.JobObservationNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.JobObservationEmployees).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
