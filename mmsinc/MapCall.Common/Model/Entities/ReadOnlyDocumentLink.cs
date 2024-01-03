using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReadOnlyDocumentLink
        : IValidatableObject, IDocumentLink
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Document Document { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public virtual int LinkedId { get; set; }
        
        public virtual DocumentStatus DocumentStatus { get; set; }
        public virtual string FileName { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Cannot validate a read-only object.");
        }
        
        #endregion
    }

    [Serializable]
    public class Document<T> : ReadOnlyDocumentLink
    {
        public virtual T Entity { get; set; }
    }

    [Serializable]
    public class AbsenceNotificationDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual AbsenceNotification AbsenceNotification { get; set; }
    }

    [Serializable]
    public class AllocationPermitDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual AllocationPermit AllocationPermit { get; set; }

        #endregion
    }

    [Serializable]
    public class ApcInspectionItemDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual ApcInspectionItem ApcInspectionItem { get; set; }

        #endregion
    }

    [Serializable]
    public class BappTeamIdeaDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual BappTeamIdea BappTeamIdea { get; set; }

        #endregion
    }

    [Serializable]
    public class BelowGroundHazardDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual BelowGroundHazard BelowGroundHazard { get; set; }

        #endregion
    }

    [Serializable]
    public class BondDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Bond Bond { get; set; }

        #endregion
    }
    
    [Serializable]
    public class DevelopmentProjectDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual DevelopmentProject DevelopmentProject { get; set; }

        #endregion
    }

    [Serializable]
    public class DriversLicenseDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual DriversLicense DriversLicense { get; set; }

        #endregion
    }

    [Serializable]
    public class CutoffSawQuestionnaireDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual CutoffSawQuestionnaire CutoffSawQuestionnaire { get; set; }

        #endregion
    }

    [Serializable]
    public class EasementDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Easement Easement { get; set; }

        #endregion
    }

    [Serializable]
    public class EmergencyResponsePlanDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual EmergencyResponsePlan EmergencyResponsePlan { get; set; }

        #endregion
    }

    [Serializable]
    public class EquipmentDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Equipment Equiment { get; set; }

        #endregion
    }

    [Serializable]
    public class EndOfPipeExceedanceDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties
        public virtual EndOfPipeExceedance EndOfPipeExceedance { get; set; }

        #endregion
    }

    [Serializable]
    public class EquipmentModelDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual EquipmentModel EquipmentModel { get; set; }

        #endregion
    }

    [Serializable]
    public class EstimatingProjectDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual EstimatingProject EstimatingProject { get; set; }

        #endregion
    }

    [Serializable]
    public class FacilityProcessDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual FacilityProcess FacilityProcess { get; set; }

        #endregion
    }

    [Serializable]
    public class FacilityProcessStepDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual FacilityProcessStep FacilityProcessStep { get; set; }

        #endregion
    }

    [Serializable]
    public class FilterMediaDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual FilterMedia FilterMedia { get; set; }

        #endregion
    }

    [Serializable]
    public class FamilyMedicalLeaveActCaseDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual FamilyMedicalLeaveActCase FamilyMedicalLeaveActCase { get; set; }
    }

    [Serializable]
    public class JobSiteCheckListDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual JobSiteCheckList JobSiteCheckList { get; set; }

        #endregion
    }

    [Serializable]
    public class GeneralLiabilityClaimDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual GeneralLiabilityClaim GeneralLiabilityClaim { get; set; }

        #endregion
    }

    [Serializable]
    public class HelpTopicDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual HelpTopic HelpTopic { get; set; }

        #endregion
    }

    [Serializable]
    public class HepatitisBVaccinationDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual HepatitisBVaccination HepatitisBVaccination { get; set; }

        #endregion
    }

    [Serializable]
    public class HydrantDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Hydrant Hydrant { get; set; }

        #endregion
    }

    [Serializable]
    public class InvestmentProjectDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual InvestmentProject InvestmentProject { get; set; }

        #endregion
    }

    [Serializable]
    public class LargeServiceProjectDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual LargeServiceProject LargeServiceProject { get; set; }

        #endregion
    }

    [Serializable]
    public class LockoutFormDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual LockoutForm LockoutForm { get; set; }

        #endregion
    }

    [Serializable]
    public class MainCrossingInspectionDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual MainCrossingInspection MainCrossingInspection { get; set; }

        #endregion
    }

    [Serializable]
    public class MarkoutDamageDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual MarkoutDamage MarkoutDamage { get; set; }

        #endregion
    }

    [Serializable]
    public class MarkoutViolationDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual MarkoutViolation MarkoutViolation { get; set; }

        #endregion
    }

    [Serializable]
    public class MedicalCertificateDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual MedicalCertificate MedicalCertificate { get; set; }

        #endregion
    }

    [Serializable]
    public class OneCallMarkoutTicketDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual OneCallMarkoutTicket OneCallMarkoutTicket { get; set; }

        #endregion
    }

    [Serializable]
    public class PositionHistoryDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual PositionHistory PositionHistory { get; set; }

        #endregion
    }

    [Serializable]
    public class PremiseDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Premise Premise { get; set; }

        #endregion
    }

    [Serializable]
    public class ProcessDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Process Process { get; set; }

        #endregion
    }

    [Serializable]
    public class ProductionWorkOrderDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} -> {1}", DocumentType.Name, Document.FileName);
        }
    }

    [Serializable]
    public class RoadwayImprovementNotificationDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual RoadwayImprovementNotification RoadwayImprovementNotification { get; set; }

        #endregion
    }

    [Serializable]
    public class SamplePlanDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual SamplePlan SamplePlan { get; set; }

        #endregion
    }

    [Serializable]
    public class ServiceDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Service Service { get; set; }

        #endregion
    }

    [Serializable]
    public class ServiceLineProtectionInvestigationDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual ServiceLineProtectionInvestigation ServiceLineProtectionInvestigation { get; set; }

        #endregion
    }

    [Serializable]
    public class TownDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Town Town { get; set; }

        #endregion
    }

    [Serializable]
    public class TrafficControlDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual TrafficControlTicket TrafficControl { get; set; }

        #endregion
    }

    [Serializable]
    public class MainCrossingDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual MainCrossing MainCrossing { get; set; }

        #endregion
    }

    [Serializable]
    public class MaintenancePlanDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual MaintenancePlan MaintenancePlan { get; set; }

        #endregion
    }

    [Serializable]
    public class RegulationDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual Regulation Regulation { get; set; }

        #endregion
    }

    [Serializable]
    public class SampleIdMatrixDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual SampleIdMatrix SampleIdMatrix { get; set; }
    }

    [Serializable]
    public class SewerOverflowDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual SewerOverflow SewerOverflow { get; set; }

        #endregion
    }

    [Serializable]
    public class SewerMainCleaningDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual SewerMainCleaning SewerMainCleaning { get; set; }

        #endregion
    }

    [Serializable]
    public class StandardOperatingProcedureDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual StandardOperatingProcedure StandardOperatingProcedure { get; set; }

        #endregion
    }

    [Serializable]
    public class TailgateTalkDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual TailgateTalk TailgateTalk { get; set; }

        #endregion
    }

    [Serializable]
    public class TailgateTalkTopicDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual TailgateTalkTopic TailgateTalkTopic { get; set; }

        #endregion
    }

    [Serializable]
    public class TrainingModuleDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual TrainingModule TrainingModule { get; set; }

        #endregion
    }

    [Serializable]
    public class TrainingRecordDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual TrainingRecord TrainingRecord { get; set; }

        #endregion
    }

    [Serializable]
    public class ValveDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual Valve Valve { get; set; }
    }

    [Serializable]
    public class ViolationCertificateDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual ViolationCertificate ViolationCertificate { get; set; }

        #endregion
    }

    [Serializable]
    public class WaterConstituentDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual WaterConstituent WaterConstituent { get; set; }
    }

    [Serializable]
    public class WaterQualityComplaintDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual WaterQualityComplaint Complaint { get; set; }
    }

    [Serializable]
    public class WaterSampleDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual WaterSample WaterSample { get; set; }
    }

    [Serializable]
    public class WorkOrderDocument : ReadOnlyDocumentLink, IEntity
    {
        #region Properties

        public virtual WorkOrder WorkOrder { get; set; }

        #endregion
    }

    [Serializable]
    public class WorkOrderInvoiceDocument : ReadOnlyDocumentLink, IEntity
    {
        public virtual WorkOrderInvoice WorkOrderInvoice { get; set; }
    }
}
