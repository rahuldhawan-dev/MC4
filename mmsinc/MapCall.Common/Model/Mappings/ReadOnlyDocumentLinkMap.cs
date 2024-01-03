using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Migrations._2023;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class ReadOnlyDocumentLinkMap : ClassMap<ReadOnlyDocumentLink>
    {
        #region Constructors

        public ReadOnlyDocumentLinkMap()
        {
            Table(Migrations.CreateDocumentLinkView.VIEW_NAME);
            
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.LinkedId);
            References(x => x.Document);
            References(x => x.DocumentType);
            References(x => x.DataType);
            Map(x => x.CreatedAt);
            Map(x => x.UpdatedAt);
            References(x => x.UpdatedBy);
            
            References(x => x.DocumentStatus);

            DiscriminateSubClassesOnColumn("TableName").AlwaysSelectWithValue();

            // Need this so when SchemaExport doesn't create a table
            // for DocumentLinkView and cause conflicts with DocumentViewLinkMap
            SchemaAction.None();
        }

        #endregion
    }

    public abstract class DocumentSubclassMap<T> : SubclassMap<Document<T>>
        where T : IThingWithDocuments
    {
        protected DocumentSubclassMap(string tableName)
        {
            DiscriminatorValue(tableName);
            References(x => x.Entity, "LinkedId").ReadOnly();
        }
    }

    public class VehicleDocumentMap : DocumentSubclassMap<Vehicle>
    {
        public VehicleDocumentMap() : base(nameof(Vehicle) + "s") { }
    }

    public class VehicleEZPassDocumentMap : DocumentSubclassMap<VehicleEZPass>
    {
        public VehicleEZPassDocumentMap() : base(VehicleEZPassMap.TABLE_NAME) { }
    }
    //  public class WorkOrderDocumentMap : DocumentSubclassMap<WorkOrder>{ public WorkOrderDocumentMap() : basenameof((WorkOrder) + "s".TABLE_NAME) { } }

    public class AbsenceNotificationDocumentMap : SubclassMap<AbsenceNotificationDocument>
    {
        #region Constructors

        public AbsenceNotificationDocumentMap()
        {
            DiscriminatorValue("AbsenceNotifications");
            References(x => x.AbsenceNotification, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class AllocationPermitDocumentMap : SubclassMap<AllocationPermitDocument>
    {
        #region Constructors

        public AllocationPermitDocumentMap()
        {
            DiscriminatorValue(nameof(AllocationPermit) + "s");
            References(x => x.AllocationPermit, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ApcInspectionItemDocumentMap : SubclassMap<ApcInspectionItemDocument>
    {
        #region Constructors

        public ApcInspectionItemDocumentMap()
        {
            DiscriminatorValue(nameof(ApcInspectionItem) + "s");
            References(x => x.ApcInspectionItem, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class AwiaComplianceDocumentMap : DocumentSubclassMap<AwiaCompliance>
    {
        public AwiaComplianceDocumentMap() : base(nameof(AwiaCompliance) + "s") { }
    }

    public class BacterialWaterSampleDocumentMap : DocumentSubclassMap<BacterialWaterSample>
    {
        public BacterialWaterSampleDocumentMap() : base(nameof(BacterialWaterSample) + "s") { }
    }

    public class BappTeamIdeaDocumentMap : SubclassMap<BappTeamIdeaDocument>
    {
        #region Constructors

        public BappTeamIdeaDocumentMap()
        {
            DiscriminatorValue(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS);
            References(x => x.BappTeamIdea, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class BelowGroundHazardDocumentMap : SubclassMap<BelowGroundHazardDocument>
    {
        #region Constructors

        public BelowGroundHazardDocumentMap()
        {
            DiscriminatorValue(nameof(BelowGroundHazard) + "s");
            References(x => x.BelowGroundHazard, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class BondDocumentMap : SubclassMap<BondDocument>
    {
        #region Constructors

        public BondDocumentMap()
        {
            DiscriminatorValue(nameof(Bond) + "s");
            References(x => x.Bond, "LinkedId").ReadOnly();
        }

        #endregion
    }
    
    public class BusinessUnitDocumentMap : DocumentSubclassMap<BusinessUnit>
    {
        public BusinessUnitDocumentMap() : base(nameof(BusinessUnit) + "s") { }
    }

    public class CommunityRightToKnowDocumentMap : DocumentSubclassMap<CommunityRightToKnow>
    {
        public CommunityRightToKnowDocumentMap() : base(CommunityRightToKnowMap.TABLE_NAME) { }
    }

    public class ConfinedSpaceFormDocumentMap : DocumentSubclassMap<ConfinedSpaceForm>
    {
        public ConfinedSpaceFormDocumentMap() : base(ConfinedSpaceFormMap.TABLE_NAME) { }
    }

    public class DevelopmentProjectDocumentMap : SubclassMap<DevelopmentProjectDocument>
    {
        #region Constructors

        public DevelopmentProjectDocumentMap()
        {
            DiscriminatorValue(nameof(DevelopmentProject) + "s");
            References(x => x.DevelopmentProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class DriversLicenseDocumentMap : SubclassMap<DriversLicenseDocument>
    {
        #region Constructors

        public DriversLicenseDocumentMap()
        {
            DiscriminatorValue(nameof(DriversLicense) + "s");

            References(x => x.DriversLicense, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ChemicalDocumentMap : DocumentSubclassMap<Chemical>
    {
        public ChemicalDocumentMap() : base(nameof(Chemical) + "s") { }
    }

    public class ChemicalDeliveryDocumentMap : DocumentSubclassMap<ChemicalDelivery>
    {
        public ChemicalDeliveryDocumentMap() : base(ChemicalDeliveryMap.TABLE_NAME) { }
    }

    public class ChemicalInventoryTransactionDocumentMap : DocumentSubclassMap<ChemicalInventoryTransaction>
    {
        public ChemicalInventoryTransactionDocumentMap() : base(nameof(ChemicalInventoryTransaction) + "s") { }
    }

    public class ChemicalStorageDocumentMap : DocumentSubclassMap<ChemicalStorage>
    {
        public ChemicalStorageDocumentMap() : base(ChemicalStorageMap.TABLE_NAME) { }
    }

    public class ChemicalUnitCostDocumentMap : DocumentSubclassMap<ChemicalUnitCost>
    {
        public ChemicalUnitCostDocumentMap() : base(nameof(ChemicalUnitCost) + "s") { }
    }

    public class ChemicalVendorDocumentMap : DocumentSubclassMap<ChemicalVendor>
    {
        public ChemicalVendorDocumentMap() : base(nameof(ChemicalVendor) + "s") { }
    }

    public class ChemicalWarehouseNumberDocumentMap : DocumentSubclassMap<ChemicalWarehouseNumber>
    {
        public ChemicalWarehouseNumberDocumentMap() : base(nameof(ChemicalWarehouseNumber) + "s") { }
    }

    public class ContractorDocumentMap : DocumentSubclassMap<Contractor>
    {
        public ContractorDocumentMap() : base(nameof(Contractor) + "s") { }
    }

    public class ContractorAgreementDocumentMap : DocumentSubclassMap<ContractorAgreement>
    {
        public ContractorAgreementDocumentMap() : base(nameof(ContractorAgreement) + "s") { }
    }

    public class ContractorInsuranceDocumentMap : DocumentSubclassMap<ContractorInsurance>
    {
        public ContractorInsuranceDocumentMap() : base(nameof(ContractorInsurance)) { }
    }

    public class CovidIssueDocumentMap : DocumentSubclassMap<CovidIssue>
    {
        public CovidIssueDocumentMap() : base(nameof(CovidIssue) + "s") { }
    }

    public class CutoffSawQuestionnaireDocumentMap : SubclassMap<CutoffSawQuestionnaireDocument>
    {
        #region Constructors

        public CutoffSawQuestionnaireDocumentMap()
        {
            DiscriminatorValue(nameof(CutoffSawQuestionnaire) + "s");

            References(x => x.CutoffSawQuestionnaire, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EasementDocumentMap : SubclassMap<EasementDocument>
    {
        #region Constructors

        public EasementDocumentMap()
        {
            DiscriminatorValue(nameof(Easement) + "s");
            References(x => x.Easement, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EmergencyResponsePlanDocumentMap : SubclassMap<EmergencyResponsePlanDocument>
    {
        #region Constructors

        public EmergencyResponsePlanDocumentMap()
        {
            DiscriminatorValue(AddEmergencyResponseTablesForBug2231.TableNames.EMERGENCY_RESPONSE_PLANS);
            References(x => x.EmergencyResponsePlan, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EmployeeDocumentMap : DocumentSubclassMap<Employee>
    {
        public EmployeeDocumentMap() : base(EmployeeMap.TABLE_NAME) { }
    }

    public class EndOfPipeExceedanceDocumentMap : DocumentSubclassMap<EndOfPipeExceedance>
    {
        public EndOfPipeExceedanceDocumentMap() : base(nameof(EndOfPipeExceedance) + "s") { }
    }

    public class EnvironmentalPermitDocumentMap : DocumentSubclassMap<EnvironmentalPermit>
    {
        public EnvironmentalPermitDocumentMap() : base(nameof(EnvironmentalPermit) + "s") { }
    }

    public class EnvironmentalNonComplianceEventDocumentMap : DocumentSubclassMap<EnvironmentalNonComplianceEvent>
    {
        public EnvironmentalNonComplianceEventDocumentMap() : base(nameof(EnvironmentalNonComplianceEvent) + "s") { }
    }

    public class EquipmentDocumentMap : SubclassMap<EquipmentDocument>
    {
        #region Constructors

        public EquipmentDocumentMap()
        {
            DiscriminatorValue("Equipment");
            References(x => x.Equiment, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EquipmentModelDocumentMap : SubclassMap<EquipmentModelDocument>
    {
        #region Constructors

        public EquipmentModelDocumentMap()
        {
            DiscriminatorValue("EquipmentModels");
            References(x => x.EquipmentModel, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EmployeeAccountabilityActionDocumentMap : DocumentSubclassMap<EmployeeAccountabilityAction>
    {
        public EmployeeAccountabilityActionDocumentMap() : base(nameof(EmployeeAccountabilityAction) + "s") { }
    }

    public class EmployeeHeadCountDocumentMap : DocumentSubclassMap<EmployeeHeadCount>
    {
        public EmployeeHeadCountDocumentMap() : base(nameof(EmployeeHeadCount) + "s") { }
    }

    public class EstimatingProjectDocumentMap : SubclassMap<EstimatingProjectDocument>
    {
        #region Constructors

        public EstimatingProjectDocumentMap()
        {
            DiscriminatorValue(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS);
            References(x => x.EstimatingProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EventDocumentsMap : DocumentSubclassMap<Event>
    {
        public EventDocumentsMap() : base(EventMap.TABLE_NAME) { }
    }

    public class EventDocumentDocumentsMap : DocumentSubclassMap<EventDocument>
    {
        public EventDocumentDocumentsMap() : base(EventDocumentMap.TABLE_NAME) { }
    }

    public class FacilityDocumentMap : DocumentSubclassMap<Facility>
    {
        public FacilityDocumentMap() : base(FacilityMap.TABLE_NAME) { }
    }

    public class FacilityProcessDocumentMap : SubclassMap<FacilityProcessDocument>
    {
        #region Constructors

        public FacilityProcessDocumentMap()
        {
            DiscriminatorValue(FacilityProcessMap.TABLE_NAME);
            References(x => x.FacilityProcess, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FacilityProcessStepDocumentMap : SubclassMap<FacilityProcessStepDocument>
    {
        #region Constructors

        public FacilityProcessStepDocumentMap()
        {
            DiscriminatorValue(nameof(FacilityProcessStep) + "s");
            References(x => x.FacilityProcessStep, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FilterMediaDocumentMap : SubclassMap<FilterMediaDocument>
    {
        #region Constructors

        public FilterMediaDocumentMap()
        {
            DiscriminatorValue(FilterMediaMap.TABLE_NAME);
            References(x => x.FilterMedia, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FamilyMedicalLeaveActCaseDocumentMap : SubclassMap<FamilyMedicalLeaveActCaseDocument>
    {
        #region Constructors

        public FamilyMedicalLeaveActCaseDocumentMap()
        {
            DiscriminatorValue(FamilyMedicalLeaveActCaseMap.TABLE_NAME);
            References(x => x.FamilyMedicalLeaveActCase, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class GasMonitorDocumentMap : DocumentSubclassMap<GasMonitor>
    {
        public GasMonitorDocumentMap() : base(GasMonitorMap.TABLE_NAME) { }
    }

    public class GasMonitorCalibrationDocumentMap : DocumentSubclassMap<GasMonitorCalibration>
    {
        public GasMonitorCalibrationDocumentMap() : base(GasMonitorCalibrationMap.TABLE_NAME) { }
    }

    public class HydrantDocumentMap : SubclassMap<HydrantDocument>
    {
        #region Constructors

        public HydrantDocumentMap()
        {
            DiscriminatorValue(nameof(Hydrant) + "s");
            References(x => x.Hydrant, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class RiskRegisterAssetDocumentMap : DocumentSubclassMap<RiskRegisterAsset>
    {
        public RiskRegisterAssetDocumentMap() : base(nameof(RiskRegisterAsset) + "s") { }
    }

    public class IncidentDocumentMap : DocumentSubclassMap<Incident>
    {
        public IncidentDocumentMap() : base(nameof(Incident) + "s") { }
    }

    public class InterconnectionDocumentMap : DocumentSubclassMap<Interconnection>
    {
        public InterconnectionDocumentMap() : base(nameof(Interconnection) + "s") { }
    }

    public class InterconnectionTestDocumentMap : DocumentSubclassMap<InterconnectionTest>
    {
        public InterconnectionTestDocumentMap() : base(nameof(InterconnectionTest) + "s") { }
    }

    public class InvestmentProjectDocumentMap : SubclassMap<InvestmentProjectDocument>
    {
        #region Constructors

        public InvestmentProjectDocumentMap()
        {
            DiscriminatorValue(nameof(InvestmentProject) + "s");
            References(x => x.InvestmentProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class JobObservationDocumentMap : DocumentSubclassMap<JobObservation>
    {
        public JobObservationDocumentMap() : base(JobObservationMap.TABLE_NAME) { }
    }

    public class JobSiteCheckListDocumentMap : SubclassMap<JobSiteCheckListDocument>
    {
        #region Constructors

        public JobSiteCheckListDocumentMap()
        {
            DiscriminatorValue(nameof(JobSiteCheckList) + "s");
            References(x => x.JobSiteCheckList, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class LargeServiceProjectDocumentMap : SubclassMap<LargeServiceProjectDocument>
    {
        #region Constructors

        public LargeServiceProjectDocumentMap()
        {
            DiscriminatorValue(nameof(LargeServiceProject) + "s");
            References(x => x.LargeServiceProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class GeneralLiabilityClaimDocumentMap : SubclassMap<GeneralLiabilityClaimDocument>
    {
        #region Constructors

        public GeneralLiabilityClaimDocumentMap()
        {
            DiscriminatorValue(AddTableForGeneralLiabilityFormForBug1983.TableNames.GENERAL_LIABILITY_CLAIMS);
            References(x => x.GeneralLiabilityClaim, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class GrievanceDocumentMap : DocumentSubclassMap<Grievance>
    {
        public GrievanceDocumentMap() : base(GrievanceMap.TABLE_NAME) { }
    }

    public class HelpTopicDocumentMap : SubclassMap<HelpTopicDocument>
    {
        #region Constructors

        public HelpTopicDocumentMap()
        {
            DiscriminatorValue(nameof(HelpTopic) + "s");

            References(x => x.HelpTopic, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class HepatitisBVaccinationDocumentMap : SubclassMap<HepatitisBVaccinationDocument>
    {
        #region Constructors

        public HepatitisBVaccinationDocumentMap()
        {
            DiscriminatorValue(nameof(HepatitisBVaccination) + "s");
            References(x => x.HepatitisBVaccination, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class LocalDocumentMap : DocumentSubclassMap<Local>
    {
        public LocalDocumentMap() : base(LocalMap.TABLE_NAME) { }
    }

    public class LockoutFormDocumentMap : SubclassMap<LockoutFormDocument>
    {
        #region Constructors

        public LockoutFormDocumentMap()
        {
            DiscriminatorValue(nameof(LockoutForm) + "s");
            References(x => x.LockoutForm, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MainCrossingInspectionDocumentMap : SubclassMap<MainCrossingInspectionDocument>
    {
        #region Constructors

        public MainCrossingInspectionDocumentMap()
        {
            DiscriminatorValue(nameof(MainCrossingInspection) + "s");
            References(x => x.MainCrossingInspection, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MaintenancePlanDocumentMap : SubclassMap<MaintenancePlanDocument>
    {
        #region Constructors
        
        public MaintenancePlanDocumentMap()
        {
            DiscriminatorValue(nameof(MaintenancePlan) + "s");
            References(x => x.MaintenancePlan, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MarkoutDamageDocumentMap : SubclassMap<MarkoutDamageDocument>
    {
        #region Constructors

        public MarkoutDamageDocumentMap()
        {
            DiscriminatorValue(nameof(MarkoutDamage) + "s");
            References(x => x.MarkoutDamage, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MarkoutViolationDocumentMap : SubclassMap<MarkoutViolationDocument>
    {
        #region Constructors

        public MarkoutViolationDocumentMap()
        {
            DiscriminatorValue(nameof(MarkoutViolation) + "s");
            References(x => x.MarkoutViolation, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MedicalCertificateDocumentMap : SubclassMap<MedicalCertificateDocument>
    {
        #region Constructors

        public MedicalCertificateDocumentMap()
        {
            DiscriminatorValue(nameof(MedicalCertificate) + "s");
            References(x => x.MedicalCertificate, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MeterChangeOutDocumentMap : DocumentSubclassMap<MeterChangeOut>
    {
        public MeterChangeOutDocumentMap() : base(nameof(MeterChangeOut) + "s") { }
    }

    public class OneCallMarkoutTicketDocumentMap : SubclassMap<OneCallMarkoutTicketDocument>
    {
        #region Constructors

        public OneCallMarkoutTicketDocumentMap()
        {
            DiscriminatorValue(nameof(OneCallMarkoutTicket) + "s");
            References(x => x.OneCallMarkoutTicket, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class OperatorLicenseDocumentMap : DocumentSubclassMap<OperatorLicense>
    {
        public OperatorLicenseDocumentMap() : base(nameof(OperatorLicense) + "s") { }
    }

    public class NearMissDocumentMap : DocumentSubclassMap<NearMiss>
    {
        public NearMissDocumentMap() : base(NearMissMap.TABLE_NAME) { }
    }

    public class NpdesRegulatorInspectionDocumentMap : DocumentSubclassMap<NpdesRegulatorInspection>
    {
        public NpdesRegulatorInspectionDocumentMap() : base(nameof(NpdesRegulatorInspection) + "s") { }
    }

    public class PositionGroupDocumentMap : DocumentSubclassMap<PositionGroup>
    {
        public PositionGroupDocumentMap() : base(nameof(PositionGroup) + "s") { }
    }

    public class PositionHistoryDocumentMap : SubclassMap<PositionHistoryDocument>
    {
        #region Constructors

        public PositionHistoryDocumentMap()
        {
            DiscriminatorValue(PositionHistoryMap.TABLE_NAME);
            References(x => x.PositionHistory, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class PremiseDocumentMap : DocumentSubclassMap<Premise>
    {
        public PremiseDocumentMap() : base(PremiseMap.TABLE_NAME) { }
    }

    public class ProcessDocumentMap : SubclassMap<ProcessDocument>
    {
        #region Constructors

        public ProcessDocumentMap()
        {
            DiscriminatorValue(ProcessMap.TABLE_NAME);
            References(x => x.Process, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ProductionPreJobSafetyBriefDocumentMap : DocumentSubclassMap<ProductionPreJobSafetyBrief>
    {
        public ProductionPreJobSafetyBriefDocumentMap() : base(nameof(ProductionPreJobSafetyBrief) + "s") { }
    }

    public class ProductionWorkOrderDocumentMap : SubclassMap<ProductionWorkOrderDocument>
    {
        public ProductionWorkOrderDocumentMap()
        {
            DiscriminatorValue(nameof(ProductionWorkOrder) + "s");
            References(x => x.ProductionWorkOrder, "LinkedId").ReadOnly();
        }
    }

    public class PublicWaterSupplyDocumentMap : DocumentSubclassMap<PublicWaterSupply>
    {
        public PublicWaterSupplyDocumentMap() : base(PublicWaterSupplyMap.TABLE_NAME) { }
    }

    public class PublicWaterSupplyFirmCapacityDocumentMap : DocumentSubclassMap<PublicWaterSupplyFirmCapacity>
    {
        public PublicWaterSupplyFirmCapacityDocumentMap() : base(PublicWaterSupplyFirmCapacityMap.TABLE_NAME) { }
    }

    public class RecurringProjectDocumentMap : DocumentSubclassMap<RecurringProject>
    {
        public RecurringProjectDocumentMap() : base(nameof(RecurringProject) + "s") { }
    }

    public class RestorationDocumentMap : DocumentSubclassMap<Restoration>
    {
        public RestorationDocumentMap() : base(nameof(Restoration) + "s") { }
    }

    public class RoadwayImprovementNotificationDocumentMap : SubclassMap<RoadwayImprovementNotificationDocument>
    {
        #region Constructors

        public RoadwayImprovementNotificationDocumentMap()
        {
            DiscriminatorValue(nameof(RoadwayImprovementNotification) + "s");
            References(x => x.RoadwayImprovementNotification, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SamplePlanDocumentMap : SubclassMap<SamplePlanDocument>
    {
        #region Constructors

        public SamplePlanDocumentMap()
        {
            DiscriminatorValue("SamplePlans");

            References(x => x.SamplePlan, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SampleSiteDocumentMap : DocumentSubclassMap<SampleSite>
    {
        public SampleSiteDocumentMap() : base(nameof(SampleSite) + "s") { }
    }

    public class ServiceDocumentMap : SubclassMap<ServiceDocument>
    {
        #region Constructors

        public ServiceDocumentMap()
        {
            DiscriminatorValue(nameof(Service) + "s");
            References(x => x.Service, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ServiceLineProtectionInvestigationDocumentMap : SubclassMap<ServiceLineProtectionInvestigationDocument>
    {
        #region Constructors

        public ServiceLineProtectionInvestigationDocumentMap()
        {
            DiscriminatorValue(nameof(ServiceLineProtectionInvestigation) + "s");
            References(x => x.ServiceLineProtectionInvestigation, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SewerOpeningInspectionDocumentMap : DocumentSubclassMap<SewerOpeningInspection>
    {
        public SewerOpeningInspectionDocumentMap() : base(nameof(SewerOpeningInspection) + "s") { }
    }

    public class TownDocumentMap : SubclassMap<TownDocument>
    {
        #region Constructors

        public TownDocumentMap()
        {
            DiscriminatorValue("Towns");

            References(x => x.Town, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TrafficControlDocumentMap : SubclassMap<TrafficControlDocument>
    {
        #region Constructors

        public TrafficControlDocumentMap()
        {
            DiscriminatorValue(nameof(TrafficControlTicket) + "s");

            References(x => x.TrafficControl, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MainCrossingDocumentMap : SubclassMap<MainCrossingDocument>
    {
        #region Constructors

        public MainCrossingDocumentMap()
        {
            DiscriminatorValue("MainCrossings");
            References(x => x.MainCrossing, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class RegulationDocumentMap : SubclassMap<RegulationDocument>
    {
        #region Constructors

        public RegulationDocumentMap()
        {
            DiscriminatorValue("Regulations");
            References(x => x.Regulation).Column("LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SampleIdMatrixDocumentMap : SubclassMap<SampleIdMatrixDocument>
    {
        #region Constructors

        public SampleIdMatrixDocumentMap()
        {
            DiscriminatorValue(SampleIdMatrixMap.TABLE_NAME);
            References(x => x.SampleIdMatrix, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SewerOpeningDocumentMap : DocumentSubclassMap<SewerOpening>
    {
        public SewerOpeningDocumentMap() : base(nameof(SewerOpening) + "s") { }
    }

    public class SewerMainCleaningDocumentMap : SubclassMap<SewerMainCleaningDocument>
    {
        #region Constructors

        public SewerMainCleaningDocumentMap()
        {
            DiscriminatorValue(nameof(SewerMainCleaning) + "s");
            References(x => x.SewerMainCleaning, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SewerOverflowDocumentMap : SubclassMap<SewerOverflowDocument>
    {
        #region Constructors

        public SewerOverflowDocumentMap()
        {
            DiscriminatorValue(nameof(SewerOverflow) + "s");
            References(x => x.SewerOverflow, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SmartCoverAlertDocumentMap : DocumentSubclassMap<SmartCoverAlert>
    {
        #region Constructors

        public SmartCoverAlertDocumentMap() : base(nameof(SmartCoverAlert) + "s") { }

        #endregion
    }

    public class StandardOperatingProcedureDocumentMap : SubclassMap<StandardOperatingProcedureDocument>
    {
        #region Constructors

        public StandardOperatingProcedureDocumentMap()
        {
            DiscriminatorValue(StandardOperatingProcedureMap.TABLE_NAME);
            References(x => x.StandardOperatingProcedure, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TailgateTalkDocumentMap : SubclassMap<TailgateTalkDocument>
    {
        #region Constructors

        public TailgateTalkDocumentMap()
        {
            DiscriminatorValue(TailgateTalkMap.TABLE_NAME);

            References(x => x.TailgateTalk, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TailgateTalkTopicDocumentMap : SubclassMap<TailgateTalkTopicDocument>
    {
        #region Constructors

        public TailgateTalkTopicDocumentMap()
        {
            DiscriminatorValue(TailgateTalkTopicMap.TABLE_NAME);

            References(x => x.TailgateTalkTopic, "LinkedId").ReadOnly();
        }

        #endregion
    } 
    public class TankInspectionDocumentMap : DocumentSubclassMap<TankInspection>
    {
        public TankInspectionDocumentMap() : base(nameof(TankInspection) + "s") { }
    }

    public class TrainingModuleDocumentMap : SubclassMap<TrainingModuleDocument>
    {
        #region Constructors

        public TrainingModuleDocumentMap()
        {
            DiscriminatorValue("tblTrainingModules");
            References(x => x.TrainingModule, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TrainingRecordDocumentMap : SubclassMap<TrainingRecordDocument>
    {
        #region Constructors

        public TrainingRecordDocumentMap()
        {
            DiscriminatorValue(TrainingRecordMap.TABLE_NAME);
            References(x => x.TrainingRecord, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class UnionDocumentMap : DocumentSubclassMap<Union>
    {
        public UnionDocumentMap() : base(UnionMap.TABLE_NAME) { }
    }

    public class UnionContractDocumentMap : DocumentSubclassMap<UnionContract>
    {
        public UnionContractDocumentMap() : base(nameof(UnionContract) + "s") { }
    }
    
    public class UnionContractProposalDocumentMap : DocumentSubclassMap<UnionContractProposal>
    {
        public UnionContractProposalDocumentMap() : base(nameof(UnionContractProposal) + "s") { }
    }

    public class UserDocumentMap : DocumentSubclassMap<User>
    {
        public UserDocumentMap() : base(UserMap.TABLE_NAME) { }
    }

    public class ValveDocumentMap : SubclassMap<ValveDocument>
    {
        #region Constructors

        public ValveDocumentMap()
        {
            DiscriminatorValue(nameof(Valve) + "s");
            References(x => x.Valve, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ViolationCertificateDocumentMap : SubclassMap<ViolationCertificateDocument>
    {
        #region Constructors

        public ViolationCertificateDocumentMap()
        {
            DiscriminatorValue(nameof(ViolationCertificate) + "s");
            References(x => x.ViolationCertificate, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WasteWaterSystemDocumentMap : DocumentSubclassMap<WasteWaterSystem>
    {
        public WasteWaterSystemDocumentMap() : base(nameof(WasteWaterSystem) + "s") { }
    }

    public class WaterConstituentDocumentMap : SubclassMap<WaterConstituentDocument>
    {
        #region Constructors

        public WaterConstituentDocumentMap()
        {
            DiscriminatorValue(nameof(WaterConstituent) + "s");
            References(x => x.WaterConstituent, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WaterQualityComplaintDocumentMap : SubclassMap<WaterQualityComplaintDocument>
    {
        #region Constructors

        public WaterQualityComplaintDocumentMap()
        {
            DiscriminatorValue(nameof(WaterQualityComplaint) + "s");
            References(x => x.Complaint, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WaterSampleComplianceFormDocumentMap : DocumentSubclassMap<WaterSampleComplianceForm>
    {
        public WaterSampleComplianceFormDocumentMap() : base(nameof(WaterSampleComplianceForm) + "s") { }
    }

    public class WaterSampleDocumentMap : SubclassMap<WaterSampleDocument>
    {
        #region Constructors

        public WaterSampleDocumentMap()
        {
            DiscriminatorValue(nameof(WaterSample) + "s");
            References(x => x.WaterSample, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WorkOrderDocumentMap : SubclassMap<WorkOrderDocument>
    {
        #region Constructors

        public WorkOrderDocumentMap()
        {
            DiscriminatorValue(nameof(WorkOrder) + "s");
            References(x => x.WorkOrder, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WorkOrderInvoiceDocumentMap : SubclassMap<WorkOrderInvoiceDocument>
    {
        #region Constructors

        public WorkOrderInvoiceDocumentMap()
        {
            DiscriminatorValue(nameof(WorkOrderInvoice) + "s");
            References(x => x.WorkOrderInvoice, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class DocumentLinkViewMap : AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods

        public override string SqlCreateString(
            Dialect dialect,
            IMapping p,
            string defaultCatalog,
            string defaultSchema)
        {
            return $"CREATE VIEW [{MC4807_AlterDocumentLinkViewToAddNewColumns.VIEW_NAME}] AS{MC4807_AlterDocumentLinkViewToAddNewColumns.NEW_VIEW_SQL}";
        }

        public override string SqlDropString(
            Dialect dialect,
            string defaultCatalog,
            string defaultSchema)
        {
            return CreateDocumentLinkView.DROP_SQL;
        }

        #endregion
    }
}
