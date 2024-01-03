using System;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RedTagPermit : IEntityWithCreationTimeTracking
    {
        #region Constants

        public readonly struct Ranges
        {
            public const int
                NUMBER_OF_TURNS_TO_CLOSE_MIN = 1,
                NUMBER_OF_TURNS_TO_CLOSE_MAX = 99999;
        }

        public readonly struct DisplayNames
        {
            public const string
                PROTECTED_TYPE_ADDITIONAL_INFORMATION = "Additional Information",
                EQUIPMENT_IMPAIRED_ON = "Date Equipment Impaired",
                EQUIPMENT_RESTORED_ON = "Date Equipment Restored",
                TYPE_OF_PROTECTION = "Type of Protection",
                EQUIPMENT_RESTORED_ON_CHANGE_REASON = "Date Equipment Restored Change Reason",
                FACILITY_ADDRESS = "Facility Address";
        }

        public readonly struct StringLengths
        {
            public const int
                AREA_PROTECTED = 255,
                REASON_FOR_IMPAIRMENT = 255,
                OTHER_PRECAUTION_DESCRIPTION = 255,
                FIRE_PROTECTION_EQUIPMENT_OPERATOR = 255,
                ADDITIONAL_INFORMATION_FOR_PROTECTION_TYPE = 255,
                EQUIPMENT_RESTORED_ON_CHANGE_REASON = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual OperatingCenter OperatingCenter => ProductionWorkOrder?.OperatingCenter;

        public virtual Facility Facility => ProductionWorkOrder?.Facility;

        public virtual Employee PersonResponsible { get; set; }

        [View(DisplayNames.TYPE_OF_PROTECTION)]
        public virtual RedTagPermitProtectionType ProtectionType { get; set; }

        [View(DisplayNames.PROTECTED_TYPE_ADDITIONAL_INFORMATION)]
        public virtual string AdditionalInformationForProtectionType { get; set; }

        public virtual string AreaProtected { get; set; }

        public virtual string ReasonForImpairment { get; set; }

        public virtual int NumberOfTurnsToClose { get; set; }

        public virtual Employee AuthorizedBy { get; set; }

        public virtual string FireProtectionEquipmentOperator { get; set; }

        [View(DisplayNames.EQUIPMENT_IMPAIRED_ON)]
        public virtual DateTime EquipmentImpairedOn { get; set; }

        [View(DisplayNames.EQUIPMENT_RESTORED_ON)]
        public virtual DateTime? EquipmentRestoredOn { get; set; }

        [View(DisplayNames.EQUIPMENT_RESTORED_ON_CHANGE_REASON)]
        public virtual string EquipmentRestoredOnChangeReason { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual bool? EmergencyOrganizationNotified { get; set; }

        public virtual bool? PublicFireDepartmentNotified { get; set; }

        public virtual bool? HazardousOperationsStopped { get; set; }

        public virtual bool? HotWorkProhibited { get; set; }

        public virtual bool? SmokingProhibited { get; set; }

        public virtual bool? ContinuousWorkAuthorized { get; set; }

        public virtual bool? OngoingPatrolOfArea { get; set; }

        public virtual bool? HydrantConnectedToSprinkler { get; set; }

        public virtual bool? PipePlugsOnHand { get; set; }

        public virtual bool? FireHoseLaidOut { get; set; }

        public virtual bool? HasOtherPrecaution { get; set; }

        public virtual string OtherPrecautionDescription { get; set; }

        #endregion
    }
}