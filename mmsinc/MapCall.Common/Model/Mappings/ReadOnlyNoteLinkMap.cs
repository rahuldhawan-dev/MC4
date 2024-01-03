using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings.Users;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class ReadOnlyNoteLinkMap : ClassMap<ReadOnlyNoteLink>
    {
        #region Constructors

        public ReadOnlyNoteLinkMap()
        {
            Table(Migrations.CreateNoteLinkView.VIEW_NAME);

            Id(x => x.Id);
            Map(x => x.LinkedId);
            References(x => x.Note, "Id").ReadOnly();
            References(x => x.DataType);

            DiscriminateSubClassesOnColumn("TableName").AlwaysSelectWithValue();

            SchemaAction.None();
        }

        #endregion
    }

    public abstract class NoteSubclassMap<T> : SubclassMap<Note<T>>
        where T : IThingWithNotes
    {
        protected NoteSubclassMap(string tableName)
        {
            DiscriminatorValue(tableName);
            References(x => x.Entity, "LinkedId").ReadOnly();
        }
    }

    public class AbsenceNotificationNoteMap : SubclassMap<AbsenceNotificationNote>
    {
        #region Constructors

        public AbsenceNotificationNoteMap()
        {
            DiscriminatorValue("AbsenceNotifications");

            References(x => x.AbsenceNotification, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class AllocationPermitNoteMap : SubclassMap<AllocationPermitNote>
    {
        #region Constructors

        public AllocationPermitNoteMap()
        {
            DiscriminatorValue(nameof(AllocationPermit) + "s");

            References(x => x.AllocationPermit, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ApcInspectionItemNoteMap : SubclassMap<ApcInspectionItemNote>
    {
        #region Constructors

        public ApcInspectionItemNoteMap()
        {
            DiscriminatorValue(nameof(ApcInspectionItem) + "s");

            References(x => x.Item, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class AwiaComplianceNoteMap : NoteSubclassMap<AwiaCompliance>
    {
        #region Constructors

        public AwiaComplianceNoteMap() : base(nameof(AwiaCompliance) + "s") { }

        #endregion
    }

    public class BacterialWaterSampleNoteMap : NoteSubclassMap<BacterialWaterSample>
    {
        public BacterialWaterSampleNoteMap() : base(nameof(BacterialWaterSample) + "s") { }
    }

    public class BappTeamIdeaNoteMap : SubclassMap<BappTeamIdeaNote>
    {
        #region Constructors

        public BappTeamIdeaNoteMap()
        {
            DiscriminatorValue(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS);
            References(x => x.BappTeamIdea, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class BelowGroundHazardNoteMap : SubclassMap<BelowGroundHazardNote>
    {
        #region Constructors

        public BelowGroundHazardNoteMap()
        {
            DiscriminatorValue(nameof(BelowGroundHazard) + "s");
            References(x => x.BelowGroundHazard, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class BondNoteMap : SubclassMap<BondNote>
    {
        #region Constructors

        public BondNoteMap()
        {
            DiscriminatorValue(nameof(Bond) + "s");
            References(x => x.Bond, "LinkedId").ReadOnly();
        }

        #endregion
    }
    
    public class BusinessUnitNoteMap : NoteSubclassMap<BusinessUnit>
    {
        public BusinessUnitNoteMap() : base(nameof(BusinessUnit) + "s") { }
    }

    public class ChemicalNoteMap : NoteSubclassMap<Chemical>
    {
        public ChemicalNoteMap() : base(nameof(Chemical) + "s") { }
    }

    public class WasteWaterSystemBasinNoteMap : NoteSubclassMap<WasteWaterSystemBasin>
    {
        public WasteWaterSystemBasinNoteMap() : base(nameof(WasteWaterSystemBasin) + "s") { }
    }

    public class ChemicalDeliveryNoteMap : NoteSubclassMap<ChemicalDelivery>
    {
        public ChemicalDeliveryNoteMap() : base(ChemicalDeliveryMap.TABLE_NAME) { }
    }

    public class ChemicalInventoryTransactionNoteMap : NoteSubclassMap<ChemicalInventoryTransaction>
    {
        public ChemicalInventoryTransactionNoteMap() : base(nameof(ChemicalInventoryTransaction) + "s") { }
    }

    public class ChemicalStorageNoteMap : NoteSubclassMap<ChemicalStorage>
    {
        public ChemicalStorageNoteMap() : base(ChemicalStorageMap.TABLE_NAME) { }
    }

    public class ChemicalUnitCostNoteMap : NoteSubclassMap<ChemicalUnitCost>
    {
        public ChemicalUnitCostNoteMap() : base(nameof(ChemicalUnitCost) + "s") { }
    }

    public class ChemicalVendorNoteMap : NoteSubclassMap<ChemicalVendor>
    {
        public ChemicalVendorNoteMap() : base(nameof(ChemicalVendor) + "s") { }
    }

    public class ChemicalWarehouseNumberNoteMap : NoteSubclassMap<ChemicalWarehouseNumber>
    {
        public ChemicalWarehouseNumberNoteMap() : base(nameof(ChemicalWarehouseNumber) + "s") { }
    }

    public class ConfinedSpaceFormNoteMap : NoteSubclassMap<ConfinedSpaceForm>
    {
        public ConfinedSpaceFormNoteMap() : base(ConfinedSpaceFormMap.TABLE_NAME) { }
    }

    public class ContractorNoteMap : NoteSubclassMap<Contractor>
    {
        public ContractorNoteMap() : base(nameof(Contractor) + "s") { }
    }

    public class ContractorAgreementNoteMap : NoteSubclassMap<ContractorAgreement>
    {
        public ContractorAgreementNoteMap() : base(nameof(ContractorAgreement) + "s") { }
    }

    public class ContractorInsuranceNoteMap : NoteSubclassMap<ContractorInsurance>
    {
        public ContractorInsuranceNoteMap() : base(nameof(ContractorInsurance)) { }
    }

    public class CovidIssueNoteMap : NoteSubclassMap<CovidIssue>
    {
        public CovidIssueNoteMap() : base(nameof(CovidIssue) + "s") { }
    }

    public class DevelopmentProjectNoteMap : SubclassMap<DevelopmentProjectNote>
    {
        #region Constructors

        public DevelopmentProjectNoteMap()
        {
            DiscriminatorValue(nameof(DevelopmentProject) + "s");
            References(x => x.DevelopmentProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class DriversLicenseNoteMap : SubclassMap<DriversLicenseNote>
    {
        #region Constructors

        public DriversLicenseNoteMap()
        {
            DiscriminatorValue(nameof(DriversLicense) + "s");

            References(x => x.DriversLicense, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class CutoffSawQuestionnaireNoteMap : SubclassMap<CutoffSawQuestionnaireNote>
    {
        #region Constructors

        public CutoffSawQuestionnaireNoteMap()
        {
            DiscriminatorValue(nameof(CutoffSawQuestionnaire) + "s");

            References(x => x.CutoffSawQuestionnaire, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EasementNoteMap : SubclassMap<EasementNote>
    {
        #region Constructors

        public EasementNoteMap()
        {
            DiscriminatorValue(nameof(Easement) + "s");
            References(x => x.Easement, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EmergencyResponsePlanNoteMap : SubclassMap<EmergencyResponsePlanNote>
    {
        public EmergencyResponsePlanNoteMap()
        {
            DiscriminatorValue(AddEmergencyResponseTablesForBug2231.TableNames.EMERGENCY_RESPONSE_PLANS);
            References(x => x.EmergencyResponsePlan, "LinkedId").ReadOnly();
        }
    }
    
    public class EmployeeHeadCountNoteMap : NoteSubclassMap<EmployeeHeadCount>
    {
        public EmployeeHeadCountNoteMap() : base(nameof(EmployeeHeadCount) + "s") { }
    }

    public class EmployeeNoteMap : NoteSubclassMap<Employee>
    {
        public EmployeeNoteMap() : base(EmployeeMap.TABLE_NAME) { }
    }

    public class EmployeeAccountabilityActionNoteMap : NoteSubclassMap<EmployeeAccountabilityAction>
    {
        public EmployeeAccountabilityActionNoteMap() : base(nameof(EmployeeAccountabilityAction) + "s") { }
    }

    public class EnvironmentalNonComplianceEventNoteMap : NoteSubclassMap<EnvironmentalNonComplianceEvent>
    {
        public EnvironmentalNonComplianceEventNoteMap() : base(nameof(EnvironmentalNonComplianceEvent) + "s") { }
    }

    public class EndOfPipeExceedanceNoteMap : NoteSubclassMap<EndOfPipeExceedance>
    {
        public EndOfPipeExceedanceNoteMap() : base(nameof(EndOfPipeExceedance) + "s") { }
    }

    public class EnvironmentalPermitNoteMap : NoteSubclassMap<EnvironmentalPermit>
    {
        public EnvironmentalPermitNoteMap() : base(nameof(EnvironmentalPermit) + "s") { }
    }

    public class EquipmentNoteMap : SubclassMap<EquipmentNote>
    {
        #region Constructors

        public EquipmentNoteMap()
        {
            DiscriminatorValue("Equipment");
            References(x => x.Equipment, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EquipmentModelNoteMap : SubclassMap<EquipmentModelNote>
    {
        #region Constructors

        public EquipmentModelNoteMap()
        {
            DiscriminatorValue("EquipmentModels");
            References(x => x.EquipmentModel, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EstimatingProjectNoteMap : SubclassMap<EstimatingProjectNote>
    {
        #region Constructors

        public EstimatingProjectNoteMap()
        {
            DiscriminatorValue(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS);
            References(x => x.EstimatingProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class EventNoteMap : NoteSubclassMap<Event>
    {
        public EventNoteMap() : base(EventMap.TABLE_NAME) { }
    }

    public class EventDocumentNoteMap : NoteSubclassMap<EventDocument>
    {
        public EventDocumentNoteMap() : base(EventDocumentMap.TABLE_NAME) { }
    }

    public class FacilityNoteMap : NoteSubclassMap<Facility>
    {
        public FacilityNoteMap() : base(FacilityMap.TABLE_NAME) { }
    }

    public class FamilyMedicalLeaveActCaseNoteMap : SubclassMap<FamilyMedicalLeaveActCaseNote>
    {
        #region Constructors

        public FamilyMedicalLeaveActCaseNoteMap()
        {
            DiscriminatorValue(FamilyMedicalLeaveActCaseMap.TABLE_NAME);

            References(x => x.FamilyMedicalLeaveActCase, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FacilityProcessNoteMap : SubclassMap<FacilityProcessNote>
    {
        #region Constructors

        public FacilityProcessNoteMap()
        {
            DiscriminatorValue(FacilityProcessMap.TABLE_NAME);
            References(x => x.FacilityProcess, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FacilityProcessStepNoteMap : SubclassMap<FacilityProcessStepNote>
    {
        #region Constructors

        public FacilityProcessStepNoteMap()
        {
            DiscriminatorValue(nameof(FacilityProcessStep) + "s");

            References(x => x.FacilityProcessStep, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class FilterMediaNoteMap : SubclassMap<FilterMediaNote>
    {
        #region Constructors

        public FilterMediaNoteMap()
        {
            DiscriminatorValue(FilterMediaMap.TABLE_NAME);

            References(x => x.FilterMedia, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class GasMonitorNoteMap : NoteSubclassMap<GasMonitor>
    {
        public GasMonitorNoteMap() : base(GasMonitorMap.TABLE_NAME) { }
    }

    public class GasMonitorCalibrationNoteMap : NoteSubclassMap<GasMonitorCalibration>
    {
        public GasMonitorCalibrationNoteMap() : base(GasMonitorCalibrationMap.TABLE_NAME) { }
    }

    public class GeneralLiabilityClaimNoteMap : SubclassMap<GeneralLiabilityClaimNote>
    {
        public GeneralLiabilityClaimNoteMap()
        {
            DiscriminatorValue(AddTableForGeneralLiabilityFormForBug1983.TableNames.GENERAL_LIABILITY_CLAIMS);
            References(x => x.GeneralLiabilityClaim, "LinkedId").ReadOnly();
        }
    }

    public class GrievanceNoteMap : NoteSubclassMap<Grievance>
    {
        public GrievanceNoteMap() : base(GrievanceMap.TABLE_NAME) { }
    }

    public class HelpTopicNoteMap : SubclassMap<HelpTopicNote>
    {
        #region Constructors

        public HelpTopicNoteMap()
        {
            DiscriminatorValue(nameof(HelpTopic) + "s");

            References(x => x.HelpTopic, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class HepatitisBVaccinationNoteMap : SubclassMap<HepatitisBVaccinationNote>
    {
        #region Constructors

        public HepatitisBVaccinationNoteMap()
        {
            DiscriminatorValue(nameof(HepatitisBVaccination) + "s");
            References(x => x.HepatitisBVaccination, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class HydrantNoteMap : SubclassMap<HydrantNote>
    {
        #region Constructors

        public HydrantNoteMap()
        {
            DiscriminatorValue(nameof(Hydrant) + "s");
            References(x => x.Hydrant, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class RiskRegisterAssetNoteMap : NoteSubclassMap<RiskRegisterAsset> 
    { 
        public RiskRegisterAssetNoteMap() : base(nameof(RiskRegisterAsset) + "s") { } 
    }

    public class IncidentNoteMap : NoteSubclassMap<Incident>
    {
        public IncidentNoteMap() : base(nameof(Incident) + "s") { }
    }

    public class InvestmentProjectNoteMap : SubclassMap<InvestmentProjectNote>
    {
        #region Constructors

        public InvestmentProjectNoteMap()
        {
            DiscriminatorValue(nameof(InvestmentProject) + "s");
            References(x => x.InvestmentProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class JobObservationNoteMap : NoteSubclassMap<JobObservation>
    {
        public JobObservationNoteMap() : base(JobObservationMap.TABLE_NAME) { }
    }

    public class InterconnectionNoteMap : NoteSubclassMap<Interconnection>
    {
        public InterconnectionNoteMap() : base(nameof(Interconnection) + "s") { }
    }

    public class InterconnectionTestNoteMap : NoteSubclassMap<InterconnectionTest>
    {
        public InterconnectionTestNoteMap() : base(nameof(InterconnectionTest) + "s") { }
    }

    public class JobSiteCheckListNoteMap : SubclassMap<JobSiteCheckListNote>
    {
        #region Constructors

        public JobSiteCheckListNoteMap()
        {
            DiscriminatorValue(nameof(JobSiteCheckList) + "s");
            References(x => x.JobSiteCheckList, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class LargeServiceProjectNoteMap : SubclassMap<LargeServiceProjectNote>
    {
        #region Constructors

        public LargeServiceProjectNoteMap()
        {
            DiscriminatorValue(nameof(LargeServiceProject) + "s");
            References(x => x.LargeServiceProject, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class LocalNoteMap : NoteSubclassMap<Local>
    {
        public LocalNoteMap() : base(LocalMap.TABLE_NAME) { }
    }

    public class LockoutFormNoteMap : SubclassMap<LockoutFormNote>
    {
        #region Constructors

        public LockoutFormNoteMap()
        {
            DiscriminatorValue(nameof(LockoutForm) + "s");
        }

        #endregion
    }

    public class MaintenancePlanNoteMap : SubclassMap<MaintenancePlanNote>
    {
        #region Constructors

        public MaintenancePlanNoteMap()
        {
            DiscriminatorValue(nameof(MaintenancePlan) + "s");
            References(x => x.MaintenancePlan, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MainCrossingInspectionNoteMap : SubclassMap<MainCrossingInspectionNote>
    {
        #region Constructors

        public MainCrossingInspectionNoteMap()
        {
            DiscriminatorValue(nameof(MainCrossingInspection) + "s");
            References(x => x.MainCrossingInspection, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MarkoutDamageNoteMap : SubclassMap<MarkoutDamageNote>
    {
        #region Constructors

        public MarkoutDamageNoteMap()
        {
            DiscriminatorValue(nameof(MarkoutDamage) + "s");
            References(x => x.MarkoutDamage, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MarkoutViolationNoteMap : SubclassMap<MarkoutViolationNote>
    {
        #region Constructors

        public MarkoutViolationNoteMap()
        {
            DiscriminatorValue(nameof(MarkoutViolation) + "s");
            References(x => x.MarkoutViolation, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MedicalCertificateNoteMap : SubclassMap<MedicalCertificateNote>
    {
        #region Constructors

        public MedicalCertificateNoteMap()
        {
            DiscriminatorValue(nameof(MedicalCertificate) + "s");
            References(x => x.MedicalCertificate, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MeterChangeOutNoteMap : NoteSubclassMap<MeterChangeOut>
    {
        public MeterChangeOutNoteMap() : base(nameof(MeterChangeOut) + "s") { }
    }

    public class NearMissNoteMap : NoteSubclassMap<NearMiss>
    {
        public NearMissNoteMap() : base(NearMissMap.TABLE_NAME) { }
    }

    public class NpdesRegulatorInspectionNoteMap : NoteSubclassMap<NpdesRegulatorInspection>
    {
        public NpdesRegulatorInspectionNoteMap() : base(NpdesRegulatorInspectionMap.TABLE_NAME) { }
    }

    public class OperatorLicenseNoteMap : NoteSubclassMap<OperatorLicense>
    {
        public OperatorLicenseNoteMap() : base(nameof(OperatorLicense) + "s") { }
    }

    public class PositionGroupNoteMap : NoteSubclassMap<PositionGroup>
    {
        public PositionGroupNoteMap() : base(nameof(PositionGroup) + "s") { }
    }

    public class PositionHistoryNoteMap : SubclassMap<PositionHistoryNote>
    {
        #region Constructors

        public PositionHistoryNoteMap()
        {
            DiscriminatorValue(PositionHistoryMap.TABLE_NAME);
            References(x => x.PositionHistory, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ProcessNoteMap : SubclassMap<ProcessNote>
    {
        #region Constructors

        public ProcessNoteMap()
        {
            DiscriminatorValue(ProcessMap.TABLE_NAME);
            References(x => x.Process, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ProductionPreJobSafetyBriefNoteMap : NoteSubclassMap<ProductionPreJobSafetyBrief>
    {
        public ProductionPreJobSafetyBriefNoteMap() : base(nameof(ProductionPreJobSafetyBrief) + "s") { }
    }

    public class ProductionWorkOrderNoteMap : SubclassMap<ProductionWorkOrderNote>
    {
        #region Constructors

        public ProductionWorkOrderNoteMap()
        {
            DiscriminatorValue(nameof(ProductionWorkOrder) + "s");
            References(x => x.ProductionWorkOrder, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class PublicWaterSupplyNoteMap : NoteSubclassMap<PublicWaterSupply>
    {
        public PublicWaterSupplyNoteMap() : base(PublicWaterSupplyMap.TABLE_NAME) { }
    }

    public class PublicWaterSupplyFirmCapacityNoteMap : NoteSubclassMap<PublicWaterSupplyFirmCapacity>
    {
        public PublicWaterSupplyFirmCapacityNoteMap() : base(PublicWaterSupplyFirmCapacityMap.TABLE_NAME) { }
    }

    public class PublicWaterSupplyPressureZoneNoteMap : NoteSubclassMap<PublicWaterSupplyPressureZone>
    {
        public PublicWaterSupplyPressureZoneNoteMap() : base(PublicWaterSupplyPressureZoneMap.TABLE_NAME) { }
    }

    public class SamplePlanNoteMap : SubclassMap<SamplePlanNote>
    {
        #region Constructors

        public SamplePlanNoteMap()
        {
            DiscriminatorValue("SamplePlans");

            References(x => x.SamplePlan, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SampleSiteNoteMap : NoteSubclassMap<SampleSite>
    {
        public SampleSiteNoteMap() : base(nameof(SampleSite) + "s") { }
    }

    public class ServiceNoteMap : SubclassMap<ServiceNote>
    {
        #region Constructors

        public ServiceNoteMap()
        {
            DiscriminatorValue(nameof(Service) + "s");
            References(x => x.Service, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class ServiceLineProtectionInvestigationNoteMap : SubclassMap<ServiceLineProtectionInvestigationNote>
    {
        #region Constructors

        public ServiceLineProtectionInvestigationNoteMap()
        {
            DiscriminatorValue(nameof(ServiceLineProtectionInvestigation) + "s");
            References(x => x.ServiceLineProtectionInvestigation, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TownNoteMap : SubclassMap<TownNote>
    {
        #region Constructors

        public TownNoteMap()
        {
            DiscriminatorValue("Towns");

            References(x => x.Town, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TrafficControlNoteMap : SubclassMap<TrafficControlNote>
    {
        #region Constructors

        public TrafficControlNoteMap()
        {
            DiscriminatorValue(nameof(TrafficControlTicket) + "s");

            References(x => x.TrafficControl, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class MainCrossingNoteMap : SubclassMap<MainCrossingNote>
    {
        #region Constructors

        public MainCrossingNoteMap()
        {
            DiscriminatorValue("MainCrossings");
            References(x => x.MainCrossing, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class OneCallMarkoutTicketNoteMap : SubclassMap<OneCallMarkoutTicketNote>
    {
        #region Constructors

        public OneCallMarkoutTicketNoteMap()
        {
            DiscriminatorValue(nameof(OneCallMarkoutTicket) + "s");
            References(x => x.OneCallMarkoutTicket, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class RecurringProjectNoteMap : NoteSubclassMap<RecurringProject>
    {
        public RecurringProjectNoteMap() : base(nameof(RecurringProject) + "s") { }
    }

    public class RegulationNoteMap : SubclassMap<RegulationNote>
    {
        #region Constructors

        public RegulationNoteMap()
        {
            DiscriminatorValue("Regulations");
            References(x => x.Regulation, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class RestorationNoteMap : NoteSubclassMap<Restoration>
    {
        public RestorationNoteMap() : base(nameof(Restoration) + "s") { }
    }

    public class SampleIdMatrixNoteMap : SubclassMap<SampleIdMatrixNote>
    {
        #region Constructors

        public SampleIdMatrixNoteMap()
        {
            DiscriminatorValue(SampleIdMatrixMap.TABLE_NAME);

            References(x => x.SampleIdMatrix, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SewerOpeningNoteMap : NoteSubclassMap<SewerOpening>
    {
        public SewerOpeningNoteMap() : base(nameof(SewerOpening) + "s") { }
    }

    public class SewerMainCleaningNoteMap : SubclassMap<SewerMainCleaningNote>
    {
        #region Constructors

        public SewerMainCleaningNoteMap()
        {
            DiscriminatorValue(nameof(SewerMainCleaning) + "s");

            References(x => x.SewerMainCleaning, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SewerOverflowNoteMap : SubclassMap<SewerOverflowNote>
    {
        #region Constructors

        public SewerOverflowNoteMap()
        {
            DiscriminatorValue(nameof(SewerOverflow) + "s");

            References(x => x.SewerOverflow, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class SmartCoverAlertNoteMap : NoteSubclassMap<SmartCoverAlert>
    {
        public SmartCoverAlertNoteMap() : base(nameof(SmartCoverAlert) + "s") { }
    }

    public class RoadwayImprovementNotificationNoteMap : SubclassMap<RoadwayImprovementNotificationNote>
    {
        #region Constructors

        public RoadwayImprovementNotificationNoteMap()
        {
            DiscriminatorValue(nameof(RoadwayImprovementNotification) + "s");
            References(x => x.RoadwayImprovementNotification, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class StandardOperatingProcedureNoteMap : SubclassMap<StandardOperatingProcedureNote>
    {
        #region Constructors

        public StandardOperatingProcedureNoteMap()
        {
            DiscriminatorValue(StandardOperatingProcedureMap.TABLE_NAME);

            References(x => x.StandardOperatingProcedure, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TailgateTalkNoteMap : SubclassMap<TailgateTalkNote>
    {
        #region Constructors

        public TailgateTalkNoteMap()
        {
            DiscriminatorValue(TailgateTalkMap.TABLE_NAME);

            References(x => x.TailgateTalk, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TailgateTalkTopicNoteMap : SubclassMap<TailgateTalkTopicNote>
    {
        #region Constructors

        public TailgateTalkTopicNoteMap()
        {
            DiscriminatorValue(TailgateTalkTopicMap.TABLE_NAME);

            References(x => x.TailgateTalkTopic, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TankInspectionNoteMap : NoteSubclassMap<TankInspection>
    {
        public TankInspectionNoteMap() : base(nameof(TankInspection) + "s") { }
    }

    public class TrainingModuleNoteMap : SubclassMap<TrainingModuleNote>
    {
        #region Constructors

        public TrainingModuleNoteMap()
        {
            DiscriminatorValue("tblTrainingModules");
            References(x => x.TrainingModule, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class TrainingRecordNoteMap : SubclassMap<TrainingRecordNote>
    {
        #region Constructors

        public TrainingRecordNoteMap()
        {
            DiscriminatorValue(TrainingRecordMap.TABLE_NAME);
            References(x => x.TrainingRecord, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class UnionNoteMap : NoteSubclassMap<Union>
    {
        public UnionNoteMap() : base(UnionMap.TABLE_NAME) { }
    }

    public class UnionContractNoteMap : NoteSubclassMap<UnionContract>
    {
        public UnionContractNoteMap() : base(nameof(UnionContract) + "s") { }
    }

    public class UnionContractProposalNoteMap : NoteSubclassMap<UnionContractProposal>
    {
        public UnionContractProposalNoteMap() : base(nameof(UnionContractProposal) + "s") { }
    }
    
    public class UserNoteMap : NoteSubclassMap<User>
    {
        public UserNoteMap() : base(UserMap.TABLE_NAME) { }
    }

    public class ValveNoteMap : SubclassMap<ValveNote>
    {
        #region Constructors

        public ValveNoteMap()
        {
            DiscriminatorValue(nameof(Valve) + "s");
            References(x => x.Valve, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class VehicleNoteMap : NoteSubclassMap<Vehicle>
    {
        public VehicleNoteMap() : base(nameof(Vehicle) + "s") { }
    }

    public class VehicleEZPassNoteMap : NoteSubclassMap<VehicleEZPass>
    {
        public VehicleEZPassNoteMap() : base(VehicleEZPassMap.TABLE_NAME) { }
    }

    public class ViolationCertificateNoteMap : SubclassMap<ViolationCertificateNote>
    {
        #region Constructors

        public ViolationCertificateNoteMap()
        {
            DiscriminatorValue(nameof(ViolationCertificate) + "s");
            References(x => x.ViolationCertificate, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WasteWaterSystemNoteMap : NoteSubclassMap<WasteWaterSystem>
    {
        public WasteWaterSystemNoteMap() : base(nameof(WasteWaterSystem) + "s") { }
    }

    public class WaterConstituentNoteMap : SubclassMap<WaterConstituentNote>
    {
        #region Constructors

        public WaterConstituentNoteMap()
        {
            DiscriminatorValue(nameof(WaterConstituent) + "s");
            References(x => x.WaterConstituent, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WaterQualityComplaintNoteMap : SubclassMap<WaterQualityComplaintNote>
    {
        #region Constructors

        public WaterQualityComplaintNoteMap()
        {
            DiscriminatorValue(nameof(WaterQualityComplaint) + "s");
            References(x => x.Complaint, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WaterSampleComplianceFormNoteMap : NoteSubclassMap<WaterSampleComplianceForm>
    {
        public WaterSampleComplianceFormNoteMap() : base(nameof(WaterSampleComplianceForm) + "s") { }
    }

    public class WaterSampleNoteMap : SubclassMap<WaterSampleNote>
    {
        #region Constructors

        public WaterSampleNoteMap()
        {
            DiscriminatorValue(nameof(WaterSample) + "s");

            References(x => x.WaterSample, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class WorkOrderInvoiceNoteMap : SubclassMap<WorkOrderInvoiceNote>
    {
        #region Constructors

        public WorkOrderInvoiceNoteMap()
        {
            DiscriminatorValue(nameof(WorkOrderInvoice) + "s");
            References(x => x.WorkOrderInvoice, "LinkedId").ReadOnly();
        }

        #endregion
    }

    public class CommunityRightToKnowNoteMap : NoteSubclassMap<CommunityRightToKnow>
    {
        public CommunityRightToKnowNoteMap() : base(nameof(CommunityRightToKnow) + "s") { }
    }

    public class NoteLinkViewMap : NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods

        public override string SqlCreateString(NHibernate.Dialect.Dialect dialect, NHibernate.Engine.IMapping p,
            string defaultCatalog, string defaultSchema)
        {
            return Migrations.CreateNoteLinkView.CREATE_SQL;
        }

        public override string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog,
            string defaultSchema)
        {
            return Migrations.CreateNoteLinkView.DROP_SQL;
        }

        #endregion
    }
}
