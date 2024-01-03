using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    // NOTE: There is a stripped down version of this entity in 271 now. Make sure any changes do not break 271.
    [Serializable]
    public class JobSiteCheckList
        : IEntityWithCreationTimeTracking,
            IValidatableObject,
            IThingWithCoordinate,
            IThingWithNotes,
            IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int WORK_ORDER_ID = CreateJobSiteCheckListTables.MAX_WORK_ORDER_ID_LENGTH,
                             CREATED_BY = CreateJobSiteCheckListTables.MAX_CREATED_BY_ID_LENGTH,
                             MARKOUT_NUMBER = CreateJobSiteCheckListTables.MAX_MARKOUT_NUMBER_LENGTH,
                             NOTES = 255;
        }

        public struct Display
        {
            public const string ALL_EMPLOYEES_WEARING_PPE = "Are all employees wearing appropriate PPE for job tasks?",
                                HAS_TRAFFIC_CONTROL =
                                    "Is traffic control present at the site?", // TODO: Needs proper rewording for the page.
                                COMPLIES_WITH_STANDARDS =
                                    "Work zone traffic control set up in compliance with applicable standards(MUTCD or DOT)?",
                                ALL_STRUCTURES_SUPPORTED =
                                    "Are all surface structures protected/supported (i.e. Telephone Pole, Wall)?",
                                PRESSURIZED_RISK_RESTRAINED = "Are Restraints Needed for Pressurized Risks?";

            public const string SPOTTER_ASSIGNED =
                                    "Spotter assigned when overhead hazard less than 10ft away from moving equipment",
                                IS_MANUFACTURER_DATA_ONSITE_FOR_SHORING_OR_SHIELDING_EQUIPMENT =
                                    "Is the manufacturer’s tabulated data onsite for shoring/shielding equipment?",
                                IS_THE_EXCAVATION_GUARDED_FROM_ACCIDENTAL_ENTRY =
                                    "Is the excavation barricaded/guarded from accidental entry by employees or the public?",
                                THUMB_PENETRATION_TEST =
                                    "Thumb Penetration Test: Using your thumb, test several samples of soil that is representative of the soil conditions within the excavation.",
                                ARE_THERE_VISUAL_SIGNS_OF_POTENTIAL_SOIL_COLLAPSE =
                                    "Are there any visual cracks, fissures, or other signs of potential soil collapse?",
                                IS_THE_EXCAVATION_SUBJECT_TO_VIBRATION = "Is the excavation subject to vibration?";

            public const string IS_MARKOUT_VALID = "Is markout valid for work site?",
                                IS_MARKOUT_EMERGENCY = "Was markout request an emergency?";

            public const string ALL_MATERIALS_SET_BACK =
                                    "Are all materials, tools, and excavated materials set back 2 ft. from edge of trenches?",
                                WATER_CONTROL_SYSTEM_IN_USE =
                                    "Are water control systems in use to keep the excavations water free?",
                                ARE_EXPOSED_UTILITIES_PROTECTED =
                                    "If any utilities are exposed in excavation, are they supported/protected?";

            public const string IS_A_LADDER_IN_PLACE = "Is a ladder in place?",
                                LADDER_EXTENDS_ABOVE_GRADE = "Does ladder extend 3 ft. above grade?",
                                IS_LADDER_ON_SLOPE = "Is ladder set on a 1:4 slope?",
                                HAS_ATMOSPHERE_BEEN_TESTED = "If required, has the atmosphere been tested?";

            public const string PROTECTION_TYPE = "What type of protection is provided?",
                                IS_SLOPE_ANGLE_NOT_LESS =
                                    "Sloping/benching: Is slope angle not less than 1 1/2 horizontal to 1 vertical?",
                                IS_SHORING_SYSTEM_USED =
                                    "Is a shoring/shielding system used in conjunction with a sloping/benching system?",
                                SHORING_SYSTEM_EXTENDS =
                                    "If a combined system is used, do the sides of the shoring/shielding system extend 18\" above the base of the slope?",
                                SHORING_SYSTEM_INSTALLED_TWO_FEET =
                                    "Shoring/shielding installed at least 2 ft. from bottom of trench?",
                                HAS_EXCAVATION = "Is there an OSHA defined excavation or trench that will be entered by the employee?";

            public const string HAS_BARRICADES = "Barricades",
                                HAS_CONES = "Cones",
                                HAS_FLAG_PERSON = "Flag Person",
                                HAS_POLICE = "Police",
                                HAS_SIGNS = "Signs";

            public const string SAP_WORK_ORDER = "SAP Work Order",
                                MAPCALL_WORKORDER_ID = "MapCall Work Order";

            public const string SAFETY_BRIEF_WEATHER_HAZARDS = "Any Potential Weather Hazards",
                                SAFETY_BRIEF_CONVO_WEATHER_HAZARDS =
                                    "Did the team have a conversation about the weather hazard?",
                                SAFETY_BRIEF_TIME_CONSTRAINT = "Are there any time of day constraints?",
                                SAFETY_BRIEF_TRAFFIC_HAZARDS = "Are there any traffic hazards?",
                                SAFETY_BRIEF_OVERHEAD_HAZARDS = "Any potential overhead hazards?",
                                SAFETY_BRIEF_UNDERGROUND_HAZARDS = "Are there underground hazards?",
                                SAFETY_BRIEF_ELECTRICAL_HAZARDS =
                                    "Are there electrical hazards (grounding/bonding) present?",
                                SAFETY_BRIEF_AC_PIPE = "Will you be working with Asbestos Concrete (AC) pipe?",
                                SAFETY_BRIEF_CREW_TRAINED_IN_AC_PIPE =
                                    "Are the crew members trained in proper AC pipe handling and disposal?",
                                SAFETY_BRIEF_EQUIPMENT_TO_DO_JOB_SAFELY =
                                    "Do you have the equipment you need to do the job safely and are employees trained on the equipment they will be using?",
                                SAFETY_BRIEF_ERGO_HAZARDS =
                                    "Have you reviewed potential ergonomic hazards with the crew?",
                                SAFETY_BRIEF_SAFETY_EQUIPMENT =
                                    "Have you reviewed the location of safety equipment (fire extinguisher, first aid kit, etc.) with all crew members",
                                SAFETY_BRIEF_OTHER_HAZARDS = "Any other hazards identified?",
                                SAFETY_BRIEF_DISCUSSION_OTHER_HAZARDS =
                                    "Has a discussion taken place about the identified hazards and proper precautions",
                                SAFETY_BRIEF_STOP_WORK_AUTHORITY =
                                    "Have all crew members been reminded of their responsibility for Stop Work Authority",
                                SAFETY_BRIEF_PPE_HEAD = "Head Protection(Hard Hats, Bump Caps, etc)",
                                SAFETY_BRIEF_PPE_HAND = "Hand Protection(Gloves, etc)",
                                SAFETY_BRIEF_PPE_ELECTRICAL = "Electrical Protection(Gloves, Boots, Suit, etc)",
                                SAFETY_BRIEF_PPE_FOOT = "Foot Protection (Safety Shoes, Metatarsal Proection, etc)",
                                SAFETY_BRIEF_PPE_EYE = "Eye Protection(Safety Glasses, Googles, etc)",
                                SAFETY_BRIEF_PPE_GARMENT = "Class 3 Safety Garment",
                                SAFETY_BRIEF_PPE_OTHER = "Other",
                                SAFETY_BRIEF_PPE_OTHER_NOTES = "Enter the Additional Neccessary PPE",
                                SAFETY_BRIEF_IF_YES = "If Yes",
                                SAFETY_BRIEF_If_NO = "If No",
                                SAFETY_BRIEF_CONFINED_SPACE =
                                    "Does the work performed involve entry into a confined space?",
                                SAFETY_BRIEF_INSPECTED_SLINGS = "Have you Inspected all Slings/Lifts/Hoists?";
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        #region Notes/Docs

        public virtual IList<JobSiteCheckListDocument> JobSiteCheckListDocuments { get; set; }
        public virtual IList<JobSiteCheckListNote> JobSiteCheckListNotes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => JobSiteCheckListDocuments.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => JobSiteCheckListNotes.Cast<INoteLink>().ToList();

        public virtual string TableName => nameof(JobSiteCheckList) + "s";

        #endregion

        public virtual string Address { get; set; }

        // NOTE: CheckListDate is reffed by 271.
        [View(FormatStyle.Date)]
        public virtual DateTime CheckListDate { get; set; }

        public virtual IList<JobSiteCheckListComment> Comments { get; set; }

        public virtual IEnumerable<JobSiteCheckListComment> CommentsByDate
        {
            get { return Comments.OrderBy(x => x.CreatedAt); }
        }

        public virtual JobSiteCheckListComment MostRecentComment
        {
            get { return Comments.OrderByDescending(x => x.CreatedAt).FirstOrDefault(); }
        }

        // NOTE: I have no idea why they label it "Competent". -Ross 1/30/2014
        public virtual Employee CompetentEmployee { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        // NOTE: CreatedBy is referenced in 271.
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual IList<JobSiteCheckListCrewMembers> CrewMembers { get; set; }

        public virtual IEnumerable<JobSiteCheckListCrewMembers> CrewMembersByDate
        {
            get { return CrewMembers.OrderBy(x => x.CreatedAt); }
        }

        public virtual JobSiteCheckListCrewMembers MostRecentCrewMembers
        {
            get { return CrewMembers.OrderByDescending(x => x.CreatedAt).FirstOrDefault(); }
        }

        public virtual OperatingCenter OperatingCenter { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? SupervisorSignOffDate { get; set; }

        public virtual Employee SupervisorSignOffEmployee { get; set; }

        [View(Display.SAP_WORK_ORDER)]
        public virtual string SAPWorkOrderId { get; set; }

        [View(Display.MAPCALL_WORKORDER_ID)]
        public virtual WorkOrder MapCallWorkOrder { get; set; }

        // I. Work Zone Setup properties

        [YesNah, View(Display.ALL_EMPLOYEES_WEARING_PPE)]
        public virtual bool? AllEmployeesWearingAppropriatePersonalProtectionEquipment { get; set; }

        [YesNah, View(Display.HAS_TRAFFIC_CONTROL)]
        public virtual bool HasTrafficControl =>
            (HasBarricadesForTrafficControl || HasConesForTrafficControl || HasFlagPersonForTrafficControl ||
             HasPoliceForTrafficControl || HasSignsForTrafficControl);

        [YesNah, View(Display.COMPLIES_WITH_STANDARDS)]
        public virtual bool? CompliesWithStandards { get; set; }

        [YesNah, View(Display.ALL_STRUCTURES_SUPPORTED)]
        public virtual bool? AllStructuresSupportedOrProtected { get; set; }

        [View(Display.HAS_BARRICADES)]
        public virtual bool HasBarricadesForTrafficControl { get; set; }

        [View(Display.HAS_CONES)]
        public virtual bool HasConesForTrafficControl { get; set; }

        [View(Display.HAS_FLAG_PERSON)]
        public virtual bool HasFlagPersonForTrafficControl { get; set; }

        [View(Display.HAS_POLICE)]
        public virtual bool HasPoliceForTrafficControl { get; set; }

        [View(Display.HAS_SIGNS)]
        public virtual bool HasSignsForTrafficControl { get; set; }

        // II. Utility Verification

        [YesNah, View(Display.IS_MARKOUT_VALID)]
        public virtual bool? IsMarkoutValidForSite { get; set; }

        public virtual string MarkoutNumber { get; set; }

        [YesNah, View(Display.IS_MARKOUT_EMERGENCY)]
        public virtual bool? IsEmergencyMarkoutRequest { get; set; }

        [View("Electric")]
        public virtual bool? MarkedElectric { get; set; }

        [View("Fuel/Gas")]
        public virtual bool? MarkedFuelGas { get; set; }

        [View("Other")]
        public virtual bool? MarkedOther { get; set; }

        [View("Sanitary Sewer")]
        public virtual bool? MarkedSanitarySewer { get; set; }

        [View("Telephone")]
        public virtual bool? MarkedTelephone { get; set; }

        [View("Water")]
        public virtual bool? MarkedWater { get; set; }

        // III. Excavations

        public virtual IList<JobSiteExcavation> Excavations { get; set; }

        /// <summary>
        /// Returns excavations ordered by earliest ExcavationDate.
        /// </summary>
        public virtual IEnumerable<JobSiteExcavation> ExcavationsByDate
        {
            get { return Excavations.OrderBy(x => x.ExcavationDate); }
        }

        [YesNah, View(Display.SPOTTER_ASSIGNED)]
        public virtual bool? SpotterAssigned { get; set; }
        [YesNah, View(Display.IS_MANUFACTURER_DATA_ONSITE_FOR_SHORING_OR_SHIELDING_EQUIPMENT)]
        public virtual bool? IsManufacturerDataOnSiteForShoringOrShieldingEquipment { get; set; }
        [View(Display.IS_THE_EXCAVATION_GUARDED_FROM_ACCIDENTAL_ENTRY)]
        [BoolFormat("Yes", "No")]
        public virtual bool? IsTheExcavationGuardedFromAccidentalEntry { get; set; }

        [YesNah, View(Display.ALL_MATERIALS_SET_BACK)]
        public virtual bool? AllMaterialsSetBackFromEdgeOfTrenches { get; set; }

        [YesNah, View(Display.WATER_CONTROL_SYSTEM_IN_USE)]
        public virtual bool? WaterControlSystemsInUse { get; set; }

        [YesNah, View(Display.ARE_EXPOSED_UTILITIES_PROTECTED)]
        public virtual bool? AreExposedUtilitiesProtected { get; set; }

        // This would be way nicer to have as a logical property,
        // but it makes validation far more difficult to deal with.
        public virtual bool HasExcavationOverFourFeetDeep { get; set; }

        // E. If any excavations are over 4 ft deep:
        [YesNah, View(Display.IS_A_LADDER_IN_PLACE)]
        public virtual bool? IsALadderInPlace { get; set; }

        [YesNah, View(Display.LADDER_EXTENDS_ABOVE_GRADE)]
        public virtual bool? LadderExtendsAboveGrade { get; set; }

        [YesNah, View(Display.IS_LADDER_ON_SLOPE)]
        public virtual bool? IsLadderOnSlope { get; set; }

        [YesNah, View(Display.HAS_ATMOSPHERE_BEEN_TESTED)]
        public virtual bool? HasAtmosphereBeenTested { get; set; }

        [View("O2", FormatStyle.DecimalWithoutTrailingZeroes)]
        public virtual decimal? AtmosphericOxygenLevel { get; set; }

        [View("CO", FormatStyle.DecimalWithoutTrailingZeroes)]
        public virtual decimal? AtmosphericCarbonMonoxideLevel { get; set; }

        [View("LEL", FormatStyle.DecimalWithoutTrailingZeroes)]
        public virtual decimal? AtmosphericLowerExplosiveLimit { get; set; }

        // F. If any excavations are 5 ft. deep or greater:

        // This would be way nicer to have as a logical property,
        // but it makes validation far more difficult to deal with.
        public virtual bool HasExcavationFiveFeetOrDeeper { get; set; }

        [View(Display.PROTECTION_TYPE)]
        public virtual IList<JobSiteExcavationProtectionType> ProtectionTypes { get; set; }

        [YesNah, View(Display.IS_SLOPE_ANGLE_NOT_LESS)]
        public virtual bool? IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical { get; set; }

        [YesNah, View(Display.IS_SHORING_SYSTEM_USED)]
        public virtual bool? IsShoringSystemUsed { get; set; }

        [YesNah, View(Display.SHORING_SYSTEM_EXTENDS)]
        public virtual bool? ShoringSystemSidesExtendAboveBaseOfSlope { get; set; }

        [YesNah, View(Display.SHORING_SYSTEM_INSTALLED_TWO_FEET)]
        public virtual bool? ShoringSystemInstalledTwoFeetFromBottomOfTrench { get; set; }

        [View(Display.HAS_EXCAVATION)]
        [BoolFormat("Yes", "No", "Incomplete")]
        public virtual bool? HasExcavation { get; set; }

        [View(Display.ARE_THERE_VISUAL_SIGNS_OF_POTENTIAL_SOIL_COLLAPSE)]
        [BoolFormat("Yes", "No")]
        public virtual bool? AreThereAnyVisualSignsOfPotentialSoilCollapse { get; set; }

        [View(Display.IS_THE_EXCAVATION_SUBJECT_TO_VIBRATION)]
        [BoolFormat("Yes", "No")]
        public virtual bool? IsTheExcavationSubjectToVibration { get; set; }

        [View(Display.THUMB_PENETRATION_TEST)]
        [CanBeNull]
        public virtual SoilConditionsWithinExcavationType SoilConditionsWithinExcavationType { get; set; }

        [CanBeNull]
        [View(Description = "Note: If soils are layered, select the soil that is least stable.")]
        public virtual SoilCompositionExcavationType SoilCompositionExcavationType { get; set; }

        #region Restraints

        /// <summary>
        /// This field is for validation only. Records that existed before the PressurizedRisksRestrained field existed
        /// do not require the field when being edited. All new records require the PressurizedRisksRestrained field, though.
        /// Bug 3940 for details.
        /// </summary>
        public virtual bool IsPressurizedRisksRestrainedFieldRequired { get; set; }

        // This field should really be changed to a bool. It was requested this be made into a lookup but the only values are Yes/No.
        [View(Display.PRESSURIZED_RISK_RESTRAINED)]
        public virtual JobSiteCheckListPressurizedRiskRestrainedType PressurizedRiskRestrainedType { get; set; }

        public virtual JobSiteCheckListNoRestraintReasonType NoRestraintReason { get; set; }
        public virtual JobSiteCheckListRestraintMethodType RestraintMethod { get; set; }

        #endregion

        #region PrejobSafetyBrief

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? SafetyBriefDateTime { get; set; }

        [View(Display.SAFETY_BRIEF_WEATHER_HAZARDS)]
        public virtual bool? AnyPotentialWeatherHazards { get; set; }

        public virtual IList<JobSiteCheckListSafetyBriefWeatherHazardType> SafetyBriefWeatherHazardTypes { get; set; }

        [View(Display.SAFETY_BRIEF_CONVO_WEATHER_HAZARDS)]
        public virtual bool? HadConversationAboutWeatherHazard { get; set; }

        public virtual string HadConversationAboutWeatherHazardNotes { get; set; }

        [View(Display.SAFETY_BRIEF_TIME_CONSTRAINT)]
        public virtual bool? AnyTimeOfDayConstraints { get; set; }

        public virtual IList<JobSiteCheckListSafetyBriefTimeOfDayConstraintType> SafetyBriefTimeOfDayConstraintTypes
        {
            get;
            set;
        }

        [View(Display.SAFETY_BRIEF_TRAFFIC_HAZARDS)]
        public virtual bool? AnyTrafficHazards { get; set; }

        public virtual IList<JobSiteCheckListSafetyBriefTrafficHazardType> SafetyBriefTrafficHazardTypes { get; set; }

        [View(Display.SAFETY_BRIEF_CONFINED_SPACE)]
        public virtual bool? InvolveConfinedSpace { get; set; }

        [View(Display.SAFETY_BRIEF_OVERHEAD_HAZARDS)]
        public virtual bool? AnyPotentialOverheadHazards { get; set; }

        public virtual IList<JobSiteCheckListSafetyBriefOverheadHazardType> SafetyBriefOverheadHazardTypes { get; set; }

        [View(Display.SAFETY_BRIEF_UNDERGROUND_HAZARDS)]
        public virtual bool? AnyUndergroundHazards { get; set; }

        public virtual IList<JobSiteCheckListSafetyBriefUndergroundHazardType> SafetyBriefUndergroundHazardTypes
        {
            get;
            set;
        }

        [View(Display.SAFETY_BRIEF_ELECTRICAL_HAZARDS)]
        public virtual bool? AreThereElectricalHazards { get; set; }

        public virtual IList<JobSiteCheckListSafetyBriefElectricalHazardType> SafetyBriefElectricalHazardTypes
        {
            get;
            set;
        }

        [View(Display.SAFETY_BRIEF_AC_PIPE)]
        public virtual bool? WorkingWithACPipe { get; set; }

        [View(Display.SAFETY_BRIEF_INSPECTED_SLINGS)]
        public virtual bool? HaveYouInspectedSlings { get; set; }

        [View(Display.SAFETY_BRIEF_CREW_TRAINED_IN_AC_PIPE)]
        public virtual bool? CrewMembersTrainedInACPipe { get; set; }

        [View(Display.SAFETY_BRIEF_EQUIPMENT_TO_DO_JOB_SAFELY)]
        public virtual bool? HaveEquipmentToDoJobSafely { get; set; }

        public virtual string HaveEquipmentToDoJobSafelyNotes { get; set; }

        [View(Display.SAFETY_BRIEF_ERGO_HAZARDS)]
        public virtual bool? ReviewedErgonomicHazards { get; set; }

        public virtual string ReviewedErgonomicHazardsNotes { get; set; }

        [View(Display.SAFETY_BRIEF_SAFETY_EQUIPMENT)]
        public virtual bool? ReviewedLocationOfSafetyEquipment { get; set; }

        [View(Display.SAFETY_BRIEF_OTHER_HAZARDS)]
        public virtual bool? OtherHazardsIdentified { get; set; }

        public virtual string OtherHazardNotes { get; set; }

        [View(Display.SAFETY_BRIEF_DISCUSSION_OTHER_HAZARDS)]
        public virtual bool? HadDiscussionAboutHazardsAndPrecautions { get; set; }

        public virtual string HadDiscussionAboutHazardsAndPrecautionsNotes { get; set; }

        [View(Display.SAFETY_BRIEF_STOP_WORK_AUTHORITY)]
        public virtual bool? CrewMembersRemindedOfStopWorkAuthority { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_HEAD)]
        public virtual bool? HeadProtection { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_HAND)]
        public virtual bool? HandProtection { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_ELECTRICAL)]
        public virtual bool? ElectricalProtection { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_FOOT)]
        public virtual bool? FootProtection { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_EYE)]
        public virtual bool? EyeProtection { get; set; }

        public virtual bool? FaceShield { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_GARMENT)]
        public virtual bool? SafetyGarment { get; set; }

        public virtual bool? HearingProtection { get; set; }
        public virtual bool? RespiratoryProtection { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_OTHER)]
        public virtual bool? PPEOther { get; set; }

        [View(Display.SAFETY_BRIEF_PPE_OTHER_NOTES)]
        public virtual string PPEOtherNotes { get; set; }

        #endregion

        #endregion

        #region Logical Properties

        public virtual bool IsApprovedBySupervisor => SupervisorSignOffEmployee != null;

        /// <summary>
        /// A terrible terrible hack property for passing the url
        /// for this record to a notification template.
        /// </summary>
        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [DoesNotAutoMap, DoesNotExport]
        public virtual bool ShowExcavation => HasExcavation == null ? false : HasExcavation.Value;

        #endregion

        #endregion

        #region Constructor

        public JobSiteCheckList()
        {
            Comments = new List<JobSiteCheckListComment>();
            CrewMembers = new List<JobSiteCheckListCrewMembers>();
            Excavations = new List<JobSiteExcavation>();
            JobSiteCheckListDocuments = new List<JobSiteCheckListDocument>();
            JobSiteCheckListNotes = new List<JobSiteCheckListNote>();
            ProtectionTypes = new List<JobSiteExcavationProtectionType>();
            SafetyBriefWeatherHazardTypes = new List<JobSiteCheckListSafetyBriefWeatherHazardType>();
            SafetyBriefTimeOfDayConstraintTypes = new List<JobSiteCheckListSafetyBriefTimeOfDayConstraintType>();
            SafetyBriefTrafficHazardTypes = new List<JobSiteCheckListSafetyBriefTrafficHazardType>();
            SafetyBriefUndergroundHazardTypes = new List<JobSiteCheckListSafetyBriefUndergroundHazardType>();
            SafetyBriefElectricalHazardTypes = new List<JobSiteCheckListSafetyBriefElectricalHazardType>();
        }

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion

        #region Helper classes

        // Doubt anyone's gonna have a need for this outside of using this model.

        public class YesNahAttribute : BoolFormatAttribute
        {
            public YesNahAttribute()
            {
                True = "Yes";
                False = "N/A";
            }
        }

        #endregion
    }
}
