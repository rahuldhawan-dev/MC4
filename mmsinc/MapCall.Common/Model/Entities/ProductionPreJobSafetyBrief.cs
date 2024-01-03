using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Safety questionnaire to be filled out by a <see cref="User"/> prior to performing a task.  That task
    /// may or may not be a <see cref="ProductionWorkOrder"/> as of Jira ticket MC-3236.
    /// </summary>
    /// <remarks>
    /// This entity is almost identical to, but is not the same as, the PJSB stuff for
    /// <see cref="JobSiteCheckList"/>. A lot of the questions are the same, but there are some that are
    /// removed, some that are added, and some that have different lookup types associated with it.
    ///
    /// Also, it's assumed that this safety brief is "complete" the moment it exists. Users must confirm
    /// that they've reviewed all hazards and precautions before saving is allowed.
    /// </remarks>
    [Serializable]
    public class ProductionPreJobSafetyBrief
        : IEntityWithCreationTracking<User>, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int DESCRIPTION_OF_WORK = 100,
                             NOTES = 255;
        }

        public struct Display
        {
            public const string
                CONFINED_SPACE = "Does the work performed involve entry into a confined space?",
                CONFINED_SPACE_DESCRIPTION =
                    "If Yes, The MapCall confined space form must be completed and all employees " +
                    "involved in the confined space entry must be trained and experienced in identifying " +
                    "and evaluating existing and potential hazards within the confined space.",
                CONVO_WEATHER_HAZARDS = "Did the team have a conversation about the weather hazard?",
                DOES_JOB_INVOLVE_CHEMICALS =
                    "Does this job involve use of chemicals or potential exposure to chemicals in the " +
                    "area?",
                ELECTRICAL_HAZARDS = "Are there electrical or other energy hazards present?",
                ELECTRICAL_HAZARDS_DESCRIPTION =
                    "The MapCall Lockout Form must be completed for all equipment being locked out. All " +
                    "employees involved must be trained in the lockout procedure for the specific " +
                    "equipment being worked on.",
                EQUIPMENT_TO_DO_JOB_SAFELY =
                    "Do you have the equipment you need to do the job safely and are employees trained " +
                    "on the equipment they will be using?",
                ERGO_HAZARDS = "Have you and/or all crew members reviewed potential ergonomic hazards?",
                HAS_PRE_USE_INSPECTION =
                    "Has a pre-use inspection been completed for equipment requiring a pre-use " +
                    "inspection: Cranes, Aerial Lifts, Forklifts, Scaffolding, Slings/Hoists, etc?",
                HAS_STRETCH_AND_FLEX_BEEN_PERFORMED = "Has pre-job stretching/warm up been performed?",
                IF_YES = "If Yes",
                If_NO = "If No",
                IS_SSD_AVAILABLE =
                    "Is the Safety Data Sheet for each chemical available for review before use or " +
                    "potential exposure?",
                OTHER_HAZARDS = "Are there any other hazards involved in the job not listed above?",
                OVERHEAD_HAZARDS = "Any potential overhead hazards?",
                OVERHEAD_HAZARDS_DESCRIPTION =
                    "If Yes, you can take a photo of this hazard and insert the photo file in the " +
                    "Document Tab.",
                PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS =
                    "Please note what mitigations/controls will be implemented.",
                PPE_FALL = "Fall Protection",
                PPE_HEAD = "Head Protection(Hard Hats, Bump Caps, etc)",
                PPE_HAND = "Hand Protection(Gloves, etc)",
                PPE_ELECTRICAL = "Electrical Protection(Gloves, Boots, Suit, etc)",
                PPE_FOOT = "Foot Protection (Safety Shoes, Metatarsal Proection, etc)",
                PPE_EYE = "Eye Protection(Safety Glasses, Googles, etc)",
                PPE_GARMENT = "Class 3/Class 2 Safety Garment",
                PPE_OTHER = "Other",
                PPE_OTHER_NOTES = "Enter the Additional Neccessary PPE",
                SAFETY_EQUIPMENT =
                    "Have you and/or all crew members reviewed the location of safety equipment (fire " +
                    "extinguisher, first aid kit, etc)?",
                STOP_WORK_AUTHORITY =
                    "Are you and/or all crew members aware of your responsibility to use Stop Work " +
                    "Authority?",
                TIME_CONSTRAINT = "Are there any time of day constraints?",
                TRAFFIC_HAZARDS = "Are there any traffic hazards?",
                TYPE_OF_FALL_PREVENTION = "Type of fall prevention/protection system being used",
                UNDERGROUND_HAZARDS = "Are there underground hazards?",
                UNDERGROUND_HAZARDS_DESCRIPTION =
                    "If Yes, you can take a photo of this hazard and insert the photo file in the " +
                    "Document Tab.",
                WEATHER_HAZARDS = "Are there any potential weather hazards?",
                WORK_FOUR_FEET_ABOVE_GROUND = "Will any work be performed ≥4-feet above ground level?";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility Facility { get; set; }
        
        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual IList<ProductionPreJobSafetyBriefWorker> Workers { get; set; } =
            new List<ProductionPreJobSafetyBriefWorker>();

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime SafetyBriefDateTime { get; set; }

        /// <summary>
        /// Description of work for ad-hoc/no work order safety briefs.
        /// </summary>
        public virtual string DescriptionOfWork { get; set; }

        [View(Display.WEATHER_HAZARDS)]
        public virtual bool AnyPotentialWeatherHazards { get; set; }

        public virtual IList<ProductionPreJobSafetyBriefWeatherHazardType> SafetyBriefWeatherHazardTypes
        {
            get;
            set;
        } = new List<ProductionPreJobSafetyBriefWeatherHazardType>();

        [View(Display.CONVO_WEATHER_HAZARDS)]
        public virtual bool HadConversationAboutWeatherHazard { get; set; }
        public virtual string HadConversationAboutWeatherHazardNotes { get; set; }

        [View(Display.TIME_CONSTRAINT)]
        public virtual bool AnyTimeOfDayConstraints { get; set; }
        public virtual IList<ProductionPreJobSafetyBriefTimeOfDayConstraintType>
            SafetyBriefTimeOfDayConstraintTypes { get; set; } =
            new List<ProductionPreJobSafetyBriefTimeOfDayConstraintType>();

        [View(Display.TRAFFIC_HAZARDS)]
        public virtual bool AnyTrafficHazards { get; set; }

        public virtual IList<ProductionPreJobSafetyBriefTrafficHazardType> SafetyBriefTrafficHazardTypes
        {
            get;
            set;
        } = new List<ProductionPreJobSafetyBriefTrafficHazardType>();

        [View(Display.CONFINED_SPACE, Description = Display.CONFINED_SPACE_DESCRIPTION)]
        public virtual bool InvolveConfinedSpace { get; set; }

        [View(Display.OVERHEAD_HAZARDS, Description = Display.OVERHEAD_HAZARDS_DESCRIPTION)]
        public virtual bool AnyPotentialOverheadHazards { get; set; }

        public virtual IList<ProductionPreJobSafetyBriefOverheadHazardType> SafetyBriefOverheadHazardTypes
        {
            get;
            set;
        }

        [View(Display.UNDERGROUND_HAZARDS, Description = Display.UNDERGROUND_HAZARDS_DESCRIPTION)]
        public virtual bool AnyUndergroundHazards { get; set; }
        public virtual IList<ProductionPreJobSafetyBriefUndergroundHazardType>
            SafetyBriefUndergroundHazardTypes { get; set; } =
            new List<ProductionPreJobSafetyBriefUndergroundHazardType>();

        [View(Display.ELECTRICAL_HAZARDS, Description = Display.ELECTRICAL_HAZARDS_DESCRIPTION)]
        public virtual bool AreThereElectricalOrOtherEnergyHazards { get; set; }
        public virtual IList<ProductionPreJobSafetyBriefElectricalHazardType>
            SafetyBriefElectricalHazardTypes { get; set; } =
            new List<ProductionPreJobSafetyBriefElectricalHazardType>();

        [View(Display.WORK_FOUR_FEET_ABOVE_GROUND)]
        public virtual bool AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel { get; set; }

        [View(Display.TYPE_OF_FALL_PREVENTION)]
        public virtual string TypeOfFallPreventionProtectionSystemBeingUsed { get; set; }

        [View(Display.DOES_JOB_INVOLVE_CHEMICALS)]
        public virtual bool DoesJobInvolveUseOfChemicals { get; set; }

        [View(Display.IS_SSD_AVAILABLE)]
        public virtual bool? IsSafetyDataSheetAvailableForEachChemical { get; set; }

        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string IsSafetyDataSheetAvailableForEachChemicalNotes { get; set; }

        [View(Display.EQUIPMENT_TO_DO_JOB_SAFELY)]
        public virtual bool HaveEquipmentToDoJobSafely { get; set; }

        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string HaveEquipmentToDoJobSafelyNotes { get; set; }

        [View(Display.HAS_PRE_USE_INSPECTION)]
        public virtual bool HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection
        {
            get;
            set;
        }

        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes
        {
            get;
            set;
        }

        [View(Display.ERGO_HAZARDS)]
        public virtual bool ReviewedErgonomicHazards { get; set; }

        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string ReviewedErgonomicHazardsNotes { get; set; }

        [View(Display.HAS_STRETCH_AND_FLEX_BEEN_PERFORMED)]
        public virtual bool HasStretchAndFlexBeenPerformed { get; set; }

        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string HasStretchAndFlexBeenPerformedNotes { get; set; }

        [View(Display.SAFETY_EQUIPMENT)]
        public virtual bool ReviewedLocationOfSafetyEquipment { get; set; }
        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string ReviewedLocationOfSafetyEquipmentNotes { get; set; }

        [View(Display.OTHER_HAZARDS)]
        public virtual bool OtherHazardsIdentified { get; set; }
        public virtual string OtherHazardNotes { get; set; }
        
        [View(Display.STOP_WORK_AUTHORITY)]
        public virtual bool CrewMembersRemindedOfStopWorkAuthority { get; set; }
        [View(Display.PLEASE_NOTE_MITIGATION_CONTROLS_INTEGRATIONS)]
        public virtual string CrewMembersRemindedOfStopWorkAuthorityNotes { get; set; }

        [View(Display.PPE_FALL)]
        public virtual bool FallProtection { get; set; }

        [View(Display.PPE_HEAD)]
        public virtual bool HeadProtection { get; set; }

        [View(Display.PPE_HAND)]
        public virtual bool HandProtection { get; set; }

        [View(Display.PPE_ELECTRICAL)]
        public virtual bool ElectricalProtection { get; set; }

        [View(Display.PPE_FOOT)]
        public virtual bool FootProtection { get; set; }

        [View(Display.PPE_EYE)]
        public virtual bool EyeProtection { get; set; }
        public virtual bool FaceShield { get; set; }

        [View(Display.PPE_GARMENT)]
        public virtual bool SafetyGarment { get; set; }
        public virtual bool HearingProtection { get; set; }
        public virtual bool RespiratoryProtection { get; set; }

        [View(Display.PPE_OTHER)]
        public virtual bool PPEOther { get; set; }

        [View(Display.PPE_OTHER_NOTES)]
        public virtual string PPEOtherNotes { get; set; }

        #region Notes/Docs

        public virtual IList<IDocumentLink> LinkedDocuments =>
            Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<Document<ProductionPreJobSafetyBrief>> Documents { get; set; } =
            new List<Document<ProductionPreJobSafetyBrief>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<Note<ProductionPreJobSafetyBrief>> Notes { get; set; } =
            new List<Note<ProductionPreJobSafetyBrief>>();

        [DoesNotExport]
        public virtual string TableName => nameof(ProductionPreJobSafetyBrief) + "s";

        #endregion

        #endregion
    }
}
