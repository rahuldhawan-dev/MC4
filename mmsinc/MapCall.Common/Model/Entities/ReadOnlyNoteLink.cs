using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReadOnlyNoteLink : IEntity, IValidatableObject, INoteLink
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Note Note { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual int LinkedId { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Cannot validate a read-only object.");
        }

        #endregion
    }

    [Serializable]
    public class Note<T> : ReadOnlyNoteLink, IEntity
    {
        public virtual T Entity { get; set; }
    }

    [Serializable]
    public class AbsenceNotificationNote : ReadOnlyNoteLink, IEntity
    {
        public virtual AbsenceNotification AbsenceNotification { get; set; }
    }

    [Serializable]
    public class AllocationPermitNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual AllocationPermit AllocationPermit { get; set; }

        #endregion
    }

    [Serializable]
    public class ApcInspectionItemNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual ApcInspectionItem Item { get; set; }

        #endregion
    }

    [Serializable]
    public class AwiaComplianceNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual AwiaCompliance Item { get; set; }

        #endregion
    }

    [Serializable]
    public class BappTeamIdeaNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual BappTeamIdea BappTeamIdea { get; set; }

        #endregion
    }

    [Serializable]
    public class BelowGroundHazardNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual BelowGroundHazard BelowGroundHazard { get; set; }

        #endregion
    }
    
    [Serializable]
    public class BondNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Bond Bond { get; set; }

        #endregion
    }

    [Serializable]
    public class DevelopmentProjectNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual DevelopmentProject DevelopmentProject { get; set; }

        #endregion
    }

    [Serializable]
    public class DriversLicenseNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual DriversLicense DriversLicense { get; set; }

        #endregion
    }

    [Serializable]
    public class CutoffSawQuestionnaireNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual CutoffSawQuestionnaire CutoffSawQuestionnaire { get; set; }

        #endregion
    }

    [Serializable]
    public class EasementNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Easement Easement { get; set; }

        #endregion
    }

    [Serializable]
    public class EmergencyResponsePlanNote : ReadOnlyNoteLink, IEntity
    {
        public virtual EmergencyResponsePlan EmergencyResponsePlan { get; set; }
    }

    [Serializable]
    public class EquipmentNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Equipment Equipment { get; set; }

        #endregion
    }

    [Serializable]
    public class EndOfPipeExceedanceNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual EndOfPipeExceedance EndOfPipeExceedance { get; set; }

        #endregion
    }

    [Serializable]
    public class EquipmentModelNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual EquipmentModel EquipmentModel { get; set; }

        #endregion
    }

    [Serializable]
    public class EstimatingProjectNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual EstimatingProject EstimatingProject { get; set; }

        #endregion
    }

    [Serializable]
    public class FamilyMedicalLeaveActCaseNote : ReadOnlyNoteLink, IEntity
    {
        public virtual FamilyMedicalLeaveActCase FamilyMedicalLeaveActCase { get; set; }
    }

    [Serializable]
    public class FacilityProcessNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual FacilityProcess FacilityProcess { get; set; }

        #endregion
    }

    [Serializable]
    public class FacilityProcessStepNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual FacilityProcessStep FacilityProcessStep { get; set; }

        #endregion
    }

    [Serializable]
    public class FilterMediaNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual FilterMedia FilterMedia { get; set; }

        #endregion
    }

    [Serializable]
    public class JobSiteCheckListNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual JobSiteCheckList JobSiteCheckList { get; set; }

        #endregion
    }

    [Serializable]
    public class GeneralLiabilityClaimNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual GeneralLiabilityClaim GeneralLiabilityClaim { get; set; }

        #endregion
    }

    [Serializable]
    public class HelpTopicNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual HelpTopic HelpTopic { get; set; }

        #endregion
    }

    [Serializable]
    public class HepatitisBVaccinationNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual HepatitisBVaccination HepatitisBVaccination { get; set; }

        #endregion
    }

    [Serializable]
    public class HydrantNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Hydrant Hydrant { get; set; }

        #endregion
    }

    [Serializable]
    public class InvestmentProjectNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual InvestmentProject InvestmentProject { get; set; }

        #endregion
    }

    [Serializable]
    public class LargeServiceProjectNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual LargeServiceProject LargeServiceProject { get; set; }

        #endregion
    }

    [Serializable]
    public class LockoutFormNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual LockoutForm LockoutForm { get; set; }

        #endregion
    }

    [Serializable]
    public class MainCrossingInspectionNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual MainCrossingInspection MainCrossingInspection { get; set; }

        #endregion
    }

    [Serializable]
    public class MarkoutDamageNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual MarkoutDamage MarkoutDamage { get; set; }

        #endregion
    }

    [Serializable]
    public class MarkoutViolationNote : ReadOnlyNoteLink, IEntity
    {
        public virtual MarkoutViolation MarkoutViolation { get; set; }
    }

    [Serializable]
    public class MedicalCertificateNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual MedicalCertificate MedicalCertificate { get; set; }

        #endregion
    }

    [Serializable]
    public class OneCallMarkoutTicketNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual OneCallMarkoutTicket OneCallMarkoutTicket { get; set; }

        #endregion
    }

    [Serializable]
    public class PositionHistoryNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual PositionHistory PositionHistory { get; set; }

        #endregion
    }

    [Serializable]
    public class ProcessNote : ReadOnlyNoteLink, IEntity
    {
        public virtual Process Process { get; set; }
    }

    [Serializable]
    public class ProductionWorkOrderNote : ReadOnlyNoteLink, IEntity
    {
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
    }

    [Serializable]
    public class SamplePlanNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual SamplePlan SamplePlan { get; set; }

        #endregion
    }

    [Serializable]
    public class ServiceNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Service Service { get; set; }

        #endregion
    }

    [Serializable]
    public class ServiceLineProtectionInvestigationNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual ServiceLineProtectionInvestigation ServiceLineProtectionInvestigation { get; set; }

        #endregion
    }

    [Serializable]
    public class TownNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Town Town { get; set; }

        #endregion
    }

    [Serializable]
    public class TrafficControlNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual TrafficControlTicket TrafficControl { get; set; }

        #endregion
    }

    [Serializable]
    public class MainCrossingNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual MainCrossing MainCrossing { get; set; }

        #endregion
    }

    [Serializable]
    public class MaintenancePlanNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual MaintenancePlan MaintenancePlan { get; set; }
        
        #endregion
    }

    [Serializable]
    public class RegulationNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual Regulation Regulation { get; set; }

        #endregion
    }

    [Serializable]
    public class SampleIdMatrixNote : ReadOnlyNoteLink, IEntity
    {
        public virtual SampleIdMatrix SampleIdMatrix { get; set; }
    }

    [Serializable]
    public class SewerOverflowNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual SewerOverflow SewerOverflow { get; set; }

        #endregion
    }   

    [Serializable]
    public class SewerMainCleaningNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual SewerMainCleaning SewerMainCleaning { get; set; }

        #endregion
    }

    [Serializable]
    public class RoadwayImprovementNotificationNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual RoadwayImprovementNotification RoadwayImprovementNotification { get; set; }

        #endregion
    }   

    [Serializable]
    public class StandardOperatingProcedureNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual StandardOperatingProcedure StandardOperatingProcedure { get; set; }

        #endregion
    }

    [Serializable]
    public class TailgateTalkNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual TailgateTalk TailgateTalk { get; set; }

        #endregion
    }

    [Serializable]
    public class TailgateTalkTopicNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual TailgateTalkTopic TailgateTalkTopic { get; set; }

        #endregion
    }

    [Serializable]
    public class TrainingModuleNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual TrainingModule TrainingModule { get; set; }

        #endregion
    }

    [Serializable]
    public class TrainingRecordNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual TrainingRecord TrainingRecord { get; set; }

        #endregion
    }

    [Serializable]
    public class ValveNote : ReadOnlyNoteLink, IEntity
    {
        public virtual Valve Valve { get; set; }
    }

    [Serializable]
    public class ViolationCertificateNote : ReadOnlyNoteLink, IEntity
    {
        #region Properties

        public virtual ViolationCertificate ViolationCertificate { get; set; }

        #endregion
    }

    [Serializable]
    public class WaterConstituentNote : ReadOnlyNoteLink, IEntity
    {
        public virtual WaterConstituent WaterConstituent { get; set; }
    }

    [Serializable]
    public class WaterQualityComplaintNote : ReadOnlyNoteLink, IEntity
    {
        public virtual WaterQualityComplaint Complaint { get; set; }
    }

    [Serializable]
    public class WaterSampleNote : ReadOnlyNoteLink, IEntity
    {
        public virtual WaterSample WaterSample { get; set; }
    }

    [Serializable]
    public class WorkOrderInvoiceNote : ReadOnlyNoteLink, IEntity
    {
        public virtual WorkOrderInvoice WorkOrderInvoice { get; set; }
    }

    [Serializable]
    public class CommunityRightToKnowNote : ReadOnlyNoteLink, IEntity
    {
        public virtual CommunityRightToKnow CommunityRightToKnow { get; set; }
    }
}
