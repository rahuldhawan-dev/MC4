using MMSINC.Data;
using MMSINC.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConfinedSpaceForm : IEntityWithCreationTracking<User>, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int EMERGENCY_RESPONSE_AGENCY = 255,
                             EMERGENCY_RESPONSE_CONTACT = 50,
                             HAS_OTHER_SAFETY_EQUIPMENT_NOTES = 255,
                             LOCATION_AND_DESCRIPTION = 255,
                             METHOD_OF_COMMUNICATION_OTHER_NOTES = 255,
                             PURPOSE_OF_ENTRY = 255,
                             MIN_LENGTH = 5,
                             PERMIT_CANCELLATION_NOTE = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        #region Section 1

        public virtual State State => OperatingCenter.State;

        /// <summary>
        /// The OperatingCenter comes from the PlanningPlant, ShortCycleWorkOrder, or WorkOrder
        /// that this form is associated with.
        /// </summary>
        public virtual OperatingCenter OperatingCenter { get; set; }

        /// <summary>
        /// The OperatingCenter comes from the PlanningPlant, ShortCycleWorkOrder, or WorkOrder
        /// that this form is associated with.
        /// </summary>
        public virtual PlanningPlant PlanningPlant
        {
            get
            {
                if (ProductionWorkOrder != null)
                {
                    return ProductionWorkOrder.PlanningPlant;
                }
                if (WorkOrder != null)
                {
                    // TODO: Return whichever PlanningPlant isn't null.
                }

                return null;
            }
        }

        public virtual Facility Facility => ProductionWorkOrder.Facility;

        public virtual string Address
        {
            get
            {
                // NOTE: ProductionWorkOrder isn't included here because they
                // only care about Facility instead of Address when this form 
                // is associated with a PWO.
                if (WorkOrder != null)
                {
                    return $"{WorkOrder.StreetAddress} {WorkOrder.TownAddress}";
                }

                return null;
            }
        }
        public virtual DateTime GeneralDateTime { get; set; }
        public virtual string LocationAndDescriptionOfConfinedSpace { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual long? ShortCycleWorkOrderNumber { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public virtual string PurposeOfEntry { get; set; }

        // Making is complete props for sections to make this easier
        [DoesNotExport]
        public virtual bool IsSection1Completed =>
            true; // Everything on section 1 is required or prefilled so will always be true

        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        #endregion

        #region Section 2

        /// <summary>
        /// The gas monitor used for all atmospheric tests.
        /// </summary>
        public virtual GasMonitor GasMonitor { get; set; }

        /// <summary>
        /// A confirmed bump test must be performed before any atmospheric tests can be added.
        /// </summary>
        public virtual Employee BumpTestConfirmedBy { get; set; }

        public virtual DateTime? BumpTestConfirmedAt { get; set; }
        public virtual bool IsBumpTestConfirmed => BumpTestConfirmedBy != null;
        public virtual IList<ConfinedSpaceFormAtmosphericTest> AtmosphericTests { get; set; }

        /// <summary>
        /// Returns true if there is at least one pre-entry atmospheric test has acceptable concentrations.
        /// Returns false if there aren't any atmospheric tests at all.
        /// </summary>
        [DoesNotExport]
        public virtual bool HasAtLeastOneValidPreEntryAtmosphericTest =>
            AtmosphericTests.Any(x => x.AllReadingsHaveAcceptableConcentrations);

        [DoesNotExport]
        public virtual bool IsSection2Completed => HasAtLeastOneValidPreEntryAtmosphericTest;

        #endregion

        #region Section 3

        public virtual DateTime? ReclassificationSignedAt { get; set; }
        public virtual Employee ReclassificationSignedBy { get; set; }
        public virtual bool IsReclassificationSectionSigned => ReclassificationSignedBy != null;

        [DoesNotExport]
        public virtual bool IsSection3Completed => IsReclassificationSectionSigned;

        #endregion

        #region Section 4

        /// <summary>
        /// Section 4 is required when any atmospheric tests have readings that are
        /// outside of the acceptable range.
        /// </summary>
        public virtual bool AtleastOneTestHasAReadingOutsideOfAcceptableConcentrations =>
            AtmosphericTests.Any(x => !x.AllReadingsHaveAcceptableConcentrations);

        [View("Can this be controlled by ventilation alone?", Description = "If no, Section 5 will be required.")]
        public virtual bool? CanBeControlledByVentilationAlone { get; set; }

        public virtual DateTime? HazardSignedAt { get; set; }
        public virtual Employee HazardSignedBy { get; set; }
        public virtual bool IsHazardSectionSigned => HazardSignedBy != null;

        [DoesNotExport]
        public virtual bool IsSection4Completed => CanBeControlledByVentilationAlone != null && IsHazardSectionSigned;

        #endregion

        #region Section 5

        /// <summary>
        /// The hazards/entrants sections are only meant to be filled out if a user
        /// explicitly selects "No" for CanBeControlledByVentilationAlone.
        /// </summary>
        [DoesNotExport]
        public virtual bool IsSection5Enabled => CanBeControlledByVentilationAlone == false;

        public virtual IList<ConfinedSpaceFormHazard> Hazards { get; set; }
        public virtual IList<ConfinedSpaceFormEntrant> Entrants { get; set; }
        public virtual DateTime? PermitBeginsAt { get; set; }
        public virtual DateTime? PermitEndsAt { get; set; }

        #region Required Safety Equipment

        [View("Has warning signs, barriers, or barricades for openings")]
        public virtual bool? HasWarningSafetyEquipment { get; set; }

        [View("Access(ladders or others)")]
        public virtual bool? HasAccessSafetyEquipment { get; set; }

        [View("Lighting")]
        public virtual bool? HasLightingSafetyEquipment { get; set; }

        [View("Ventilation")]
        public virtual bool? HasVentilationSafetyEquipment { get; set; }

        [View("GFCI or other electrical shielding")]
        public virtual bool? HasGFCISafetyEquipment { get; set; }

        [View("Other")]
        public virtual bool? HasOtherSafetyEquipment { get; set; }

        [View("Head protection")]
        public virtual bool? HasHeadSafetyEquipment { get; set; }

        [View("Eye protection")]
        public virtual bool? HasEyeSafetyEquipment { get; set; }

        [View("Respiratory protection")]
        public virtual bool? HasRespiratorySafetyEquipment { get; set; }

        [View("Hand protection")]
        public virtual bool? HasHandSafetyEquipment { get; set; }

        [View("Fall protection")]
        public virtual bool? HasFallSafetyEquipment { get; set; }

        [View("Foot protection")]
        public virtual bool? HasFootSafetyEquipment { get; set; }

        /// <summary>
        /// Only has notes if HasOtherSafetyEquipmentNotes == True.
        /// </summary>
        public virtual string HasOtherSafetyEquipmentNotes { get; set; }

        #endregion

        #region Method of communication

        public virtual ConfinedSpaceFormMethodOfCommunication MethodOfCommunication { get; set; }

        /// <summary>
        /// Only has notes if MethodOfCommucation == Other.
        /// </summary>
        public virtual string MethodOfCommunicationOtherNotes { get; set; }

        #endregion

        #region Hot work permit

        public virtual bool? IsHotWorkPermitRequired { get; set; }
        public virtual bool? IsFireWatchRequired { get; set; }

        #endregion

        #region Method of rescue of entrants

        [View(Description = "Tripod, winch, full body harness")]
        public virtual bool? HasRetrievalSystem { get; set; }

        public virtual bool? HasContractRescueService { get; set; }
        public virtual string EmergencyResponseAgency { get; set; }
        public virtual string EmergencyResponseContact { get; set; }

        #endregion

        #region Authorization to begin entry operation

        public virtual DateTime? BeginEntryAuthorizedAt { get; set; }
        public virtual Employee BeginEntryAuthorizedBy { get; set; }
        public virtual bool IsBeginEntrySectionSigned => BeginEntryAuthorizedBy != null;

        #endregion

        #region Caancellation of permit

        public virtual DateTime? PermitCancelledAt { get; set; }
        public virtual Employee PermitCancelledBy { get; set; }
        public virtual bool IsPermitCancelledSectionSigned => PermitCancelledBy != null;
        public virtual string PermitCancellationNote { get; set; }

        #endregion

        [DoesNotExport]
        public virtual bool IsSection5Completed => IsSection5Complete();

        #endregion

        #region Documents

        [DoesNotExport]
        public virtual string TableName => ConfinedSpaceFormMap.TABLE_NAME;

        public virtual IList<Document<ConfinedSpaceForm>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<Note<ConfinedSpaceForm>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Logical Props

        // NOTE: The return value and this property are both functionally
        // the same, but the other requirements for this being "completed" are handled by
        // the view model and a billion RequiredWhens. I believe there's going to be 
        // further logic in here, but that depends on H&S/Production knowing what they want.
        // ex: Lori thinks they're going to want this to include permit cancellations(or reached
        // the permit's end date) at some point. 
        // NOTE: MC-2664 is going to do more with this.
        public virtual bool IsCompleted => IsFormComplete();

        #endregion

        #endregion

        #region PrivateMethods

        private bool IsFormComplete()
        {
            return IsSection1Completed && IsSection2Completed && (IsSection3Completed || IsSection4Completed);
        }

        private bool IsSection5Complete()
        {
            return IsSection5Enabled && PermitBeginsAt.HasValue && PermitEndsAt.HasValue &&
                   HasRetrievalSystem.HasValue && HasContractRescueService.HasValue &&
                   !String.IsNullOrWhiteSpace(EmergencyResponseAgency) &&
                   !String.IsNullOrWhiteSpace(EmergencyResponseContact);
        }

        #endregion

        #region Constructor

        public ConfinedSpaceForm()
        {
            AtmosphericTests = new List<ConfinedSpaceFormAtmosphericTest>();
            Hazards = new List<ConfinedSpaceFormHazard>();
            Entrants = new List<ConfinedSpaceFormEntrant>();
            Documents = new List<Document<ConfinedSpaceForm>>();
            Notes = new List<Note<ConfinedSpaceForm>>();
        }

        #endregion
    }
}
