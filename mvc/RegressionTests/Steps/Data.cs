using AuthorizeNet.Utility.NotProvided;
using DeleporterCore.Client;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions.StringExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using NHibernate;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.StructureMap;
using TechTalk.SpecFlow;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace RegressionTests.Steps
{
    [Binding]
    public class Data
    {
        #region Constants

        public static readonly TestTypeDictionary TYPE_DICTIONARY = new TestTypeDictionary {
            {"action item", typeof(ActionItem), CreateActionItem},
            {"abc indicator", typeof(ABCIndicator), CreateABCIndicator},
            {"accounting type", typeof(AccountingType), CreateAccountingType},
            {"acoustic monitoring", typeof(AcousticMonitoringType), CreateAcousticMonitoring},
            {"admin user", typeof(User), (v, c, s) => CreateUser(v, c, s, true)},
            {"arc flash status", typeof(ArcFlashStatus), CreateArcFlashStatus },
            {"asset reliability technology used type", typeof(AssetReliabilityTechnologyUsedType), CreateAssetReliabilityTechnologyUsedType},
            {"asset status", typeof(AssetStatus), CreateAssetStatus},
            {"asset type", typeof(AssetType), CreateAssetType},
            {"bacterial sample type", typeof(BacterialSampleType), CreateBacterialSampleType},
            {"board", typeof(Board), CreateBoard},
            {"communication type", typeof(CommunicationType), CreateCommunicationType },
            {"compliance requirement", typeof(ComplianceRequirement), CreateComplianceRequirement },
            {"confined space form entrant type", typeof(ConfinedSpaceFormEntrantType), CreateConfinedSpaceFormEntrantType},
            {"consolidated customer side material", typeof(ConsolidatedCustomerSideMaterial), CreateConsolidatedCustomerSideMaterial},
            {"contact without address", typeof(Contact), CreateContactWithoutAddress},
            {"contractor", typeof(Contractor), CreateContractor},
            {"contractor labor cost", typeof(ContractorLaborCost), CreateContractorLaborCost},
            {"corrective order problem code", typeof(CorrectiveOrderProblemCode), CreateCorrectiveOrderProblemCode},
            {"crash type", typeof(CrashType), CreateCrashType},
            {"crew assignment", typeof(CrewAssignment), CreateCrewAssignment},
            {"customer coordinate", typeof(CustomerCoordinate), CreateCustomerCoordinate},
            {"cutoff saw questionnaire", typeof(CutoffSawQuestionnaire), CreateCutoffSawQuestionnaire},
            {"data table layout", typeof(DataTableLayout), CreateDataTableLayout},
            {"discharge weather related type", typeof(DischargeWeatherRelatedType), CreateDischargeWeatherRelatedType},
            {"division", typeof(Division), CreateDivision},
            {"document", typeof(Document), CreateDocument},
            {"document type", typeof(DocumentType), CreateDocumentType},
            {"employee status", typeof(EmployeeStatus), CreateEmployeeStatus},
            {"counts against target", typeof(EnvironmentalNonComplianceEventCountsAgainstTarget), CreateEnvironmentalNonComplianceEventCountsAgainstTarget},
            {"equipment category", typeof(EquipmentCategory), CreateEquipmentCategory},
            {"equipment characteristic drop down value", typeof(EquipmentCharacteristicDropDownValue), CreateEquipmentCharacteristicDropDownValue},
            {"plan review", typeof(PlanReview), CreatePlanReview},
            {"service material", typeof(ServiceMaterial), CreateServiceMaterial},
            {"equipment document link", typeof(DocumentLink), CreateEquipmentDocumentLink},
            {"equipment lifespan", typeof(EquipmentLifespan), CreateEquipmentLifespan },
            {"equipment performance rating", typeof(EquipmentPerformanceRating), CreateEquipmentPerformanceRating},
            {"equipment likelyhood of failure rating", typeof(EquipmentLikelyhoodOfFailureRating), CreateEquipmentLikelyhoodOfFailureRating},
            {"equipment consequences of failure rating", typeof(EquipmentConsequencesOfFailureRatingFactory), CreateEquipmentConsequencesOfFailureRating},
            {"facility consequence of failure", typeof(FacilityConsequenceOfFailureFactory), CreateFacilityConsequenceOfFailure},
            {"facility asset management maintenance strategy tier", typeof(FacilityAssetManagementMaintenanceStrategyTierFactory), CreateFacilityAssetManagementMaintenanceStategyTier},
            {"facility maintenance risk of failure", typeof(FacilityMaintenanceRiskOfFailureFactory), CreateFacilityMaintenanceRiskOfFailure},
            {"engine equipment lifespan", typeof(EngineEquipmentLifespanFactory), CreateEngineEquipmentLifespan},
            {"equipment sensor", typeof(EquipmentSensor), CreateEquipmentSensor},
            {"equipment subcategory", typeof(EquipmentSubCategory), CreateEquipmentSubCategory},
            {"equipment model", typeof(EquipmentModel), CreateEquipmentModel},
            {"equipment status", typeof(EquipmentStatus), CreateEquipmentStatus },
            {"equipment purpose", typeof(EquipmentPurpose), CreateEquipmentPurpose},
            {"facility status", typeof(FacilityStatus), CreateFacilityStatus },
            {"filter equipment lifespan", typeof(FilterEquipmentLifespanFactory), CreateFilterEquipmentLifespan},
            {"fire district town", typeof(FireDistrictTown), CreateFireDistrictTown},
            {"filter media", typeof(FilterMedia), CreateFilterMedia},
            {"fuel type", typeof(FuelType), CreateFuelType},
            {"generator", typeof(Generator), CreateGenerator},
            {"generator equipment lifespan", typeof(GeneratorEquipmentLifespanFactory), CreateGeneratorEquipmentLifespan},
            {"interconnection", typeof(Interconnection), CreateInterconnection},
            {"hydrant billing", typeof(HydrantBilling), CreateHydrantBilling},
            {"hydrant inspection", typeof(HydrantInspection), CreateHydrantInspection},
            {"incident investigation root cause level2 type", typeof(IncidentInvestigationRootCauseLevel2Type), CreateIncidentInvestigationRootCauseLevel2Type},
            {"incident investigation root cause level3 type", typeof(IncidentInvestigationRootCauseLevel3Type), CreateIncidentInvestigationRootCauseLevel3Type},
            {"job site check list comment", typeof(JobSiteCheckListComment), CreateJobSiteCheckListComment},
            {"job site check list crew members", typeof(JobSiteCheckListCrewMembers), CreateJobSiteCheckListCrewMembers},
            {"job site excavation", typeof(JobSiteExcavation), CreateJobSiteExcavation},
            {"job site excavation location type", typeof(JobSiteExcavationLocationType), CreateJobSiteExcavationLocationType},
            {"job title common name", typeof(JobTitleCommonName), CreateJobTitleCommonName},
            {"l i m s status", typeof(LIMSStatus), CreateLIMSStatus},
            {"licensed operator category", typeof(LicensedOperatorCategory), CreateLicensedOperatorCategory},
            //{"lockout form", typeof(LockoutForm), CreateLockoutForm},
            {"lockout form question category", typeof(LockoutFormQuestionCategory), CreateLockoutFormQuestionCategory},
            {"facility inspection form question category", typeof(FacilityInspectionFormQuestionCategory), CreateFacilityInspectionFormQuestionCategories},
            {"facility inspection area type", typeof(FacilityInspectionAreaType), CreateFacilityInspectionAreaType},
            {"lockout device", typeof(LockoutDevice), CreateLockoutDevice},
            {"main crossing", typeof(MainCrossing), CreateMainCrossing},
            {"maintenance plan", typeof(MaintenancePlan), CreateMaintenancePlan},
            {"markout requirement", typeof(MarkoutRequirement), CreateMarkoutRequirement},
            {"markout type", typeof(MarkoutType), CreateMarkoutType},
            {"nonrevenue water adjustment", typeof(NonRevenueWaterAdjustment), CreateNonRevenueWaterAdjustment},
            {"nonrevenue water detail", typeof(NonRevenueWaterDetail), CreateNonRevenueWaterDetail},
            {"nonrevenue water entry", typeof(NonRevenueWaterEntry), CreateNonRevenueWaterEntry},
            {"note", typeof(Note), CreateNote},
            {"notification purpose", typeof(NotificationPurpose), CreateNotificationPurpose},
            {"npdes regulator inspection", typeof(NpdesRegulatorInspection), CreateNpdesRegulatorInspection},
            {"npdes regulator inspection type", typeof(NpdesRegulatorInspectionType), CreateNpdesRegulatorInspectionType},
            {"npdes regulator inspection form answer type", typeof(GateStatusAnswerType), CreateGateStatusAnswerType},
            {"one call markout ticket", typeof(OneCallMarkoutTicket), CreateOneCallMarkoutTicket},
            {"operating center", typeof(OperatingCenter), CreateOperatingCenter},
            {"order type", typeof(OrderType), CreateOrderType },
            {"osha standard", typeof(Regulation), CreateOSHAStandard},
            {"premise", typeof(Premise), CreatePremise },
            {"production prerequisite", typeof(ProductionPrerequisite), CreateProductionPrerequisite },
            {"production work order equipment", typeof(ProductionWorkOrderEquipment), CreateProductionWorkOrderEquipment },
            {"production work order frequency", typeof(ProductionWorkOrderFrequency), CreateProductionWorkOrderFrequency },
            {"project", typeof(Project), CreateProject},
            {"public water supply", typeof(PublicWaterSupply), CreatePublicWaterSupply},
            {"public water supply status", typeof(PublicWaterSupplyStatus), CreatePublicWaterSupplyStatus },
            {"recurring frequency unit", typeof(RecurringFrequencyUnit), CreateRecurringFrequencyUnit },
            {"recurring project", typeof(RecurringProject), CreateRecurringProject},
            {"recurring project status", typeof(RecurringProjectStatus), CreateRecurringProjectStatus },
            {"reading", typeof(Reading), CreateReading},
            {"restoration processing", typeof(WorkOrder), CreateWorkOrder},
            {"role", typeof(OperatingCenter), CreateRole},
            {"sample site address location type", typeof(SampleSiteAddressLocationType), CreateSampleSiteAddressLocationType },
            {"sample site collection type", typeof(SampleSiteCollectionType), CreateSampleSiteCollectionType },
            {"sample site point of use treatment type", typeof(SampleSitePointOfUseTreatmentType), CreateSampleSitePointOfUseTreatmentType },
            {"sap communication status", typeof(SapCommunicationStatus), CreateSapCommunicationStatus },
            {"equipment type", typeof(EquipmentType), CreateEquipmentType },
            {"equipment manufacturer", typeof(EquipmentManufacturer), CreateEquipmentManufacturer },
            {"sample site", typeof(SampleSite), CreateSampleSite},
            {"sample site location type", typeof(SampleSiteLocationType), CreateSampleSiteLocationType },
            {"sample site lead copper tier classification", typeof(SampleSiteLeadCopperTierClassification), CreateSampleSiteLeadCopperTierClassification},
            {"sample site lead copper validation method", typeof(SampleSiteLeadCopperValidationMethod), CreateSampleSiteLeadCopperValidationMethod},
            {"sample site lead customer contact method", typeof(SampleSiteCustomerContactMethod), CreateSampleSiteCustomerContactMethod},
            {"sample site inactivation reason", typeof(SampleSiteInactivationReason), CreateSampleSiteInactivationReason },
            {"near miss type", typeof(NearMissType), CreateNearMissType },
            {"general liability claim type", typeof(GeneralLiabilityClaimType), CreateGeneralLiabilityClaimType},
            {"near miss category", typeof(NearMissCategory), CreateNearMissCategory },
            {"sensor", typeof(Sensor), CreateSensor},
            {"sewer main inspection grade", typeof(SewerMainInspectionGrade), CreateSewerMainInspectionGrade},
            {"sewer main inspection type", typeof(SewerMainInspectionType), CreateSewerMainInspectionType},
            {"sewer opening type", typeof(SewerOpeningType), CreateSewerOpeningType},
            {"sewer overflow cause", typeof(SewerOverflowCause), CreateSewerOverflowCause},
            {"sewer overflow discharge location", typeof(SewerOverflowDischargeLocation), CreateSewerOverflowDischargeLocation},
            {"sewer overflow type", typeof(SewerOverflowType), CreateSewerOverflowType},
            {"service", typeof(Service), CreateService},
            {"service category", typeof(ServiceCategory), CreateServiceCategory},
            {"service size", typeof(ServiceSize), CreateServiceSize},
            {"service line protection investigation", typeof(ServiceLineProtectionInvestigation), CreateServiceLineProtectionInvestigation },
            {"system delivery type", typeof(SystemDeliveryType), CreateSystemDeliveryType },
            {"site", typeof(Site), CreateSite},
            {"smart cover alert application description", typeof(SmartCoverAlertApplicationDescriptionType), CreateSmartCoverAlertApplicationDescriptionType },
            {"smart cover alert", typeof(SmartCoverAlert), CreateSmartCoverAlert },
            {"state", typeof(MapCall.Common.Model.Entities.State), CreateState},
            {"task group", typeof(TaskGroup), CreateTaskGroup},
            {"town", typeof(Town), CreateTown},
            {"town document link", typeof(DocumentLink), CreateTownDocumentLink},
            {"help topic document link", typeof(DocumentLink), CreateHelpTopicDocumentLink},
            {"testing decision", typeof(IncidentDrugAndAlcoholTestingDecision), CreateTestingDecision},
            {"testing result", typeof(IncidentDrugAndAlcoholTestingResult), CreateTestingResult},
            {"user", typeof(User), (v, c, s) => CreateUser(v, c, s, false)},
            {"valve billing", typeof(ValveBilling), CreateValveBilling},
            {"valve control", typeof(ValveControl), CreateValveControl},
            {"valve inspection", typeof(ValveInspection), CreateValveInspection },
            {"valve status", typeof(AssetStatus), CreateValveStatus },
            {"weather condition", typeof(WeatherCondition), CreateWeatherCondition },
            {"wildcard role", typeof(Role), CreateWildcardRole},
            {"workers compensation claim status", typeof(WorkersCompensationClaimStatus), CreateWorkersCompensationClaimStatus},
            {"work description", typeof(WorkDescription), CreateWorkDescription},
            {"work order", typeof(WorkOrder), CreateWorkOrder},
            {"planning work order", typeof(WorkOrder), CreateWorkOrder},
            {"work order priority", typeof(WorkOrderPriority), CreateWorkOrderPriority},
            {"production work order priority", typeof(ProductionWorkOrderPriority), CreateProductionWorkOrderPriority},
            {"work order purpose", typeof(WorkOrderPurpose), CreateWorkOrderPurpose},
            {"work order requester", typeof(WorkOrderRequester), CreateWorkOrderRequester},
            {"meter location", typeof(MeterLocation), CreateMeterLocation}, 
        };

        // for the love of all that is good and just and fair in this world
        // please keep this list alphabetical.
        private static readonly NameValueCollection PAGE_STRINGS = new NameValueCollection {
            {"absence notification", "Operations/AbsenceNotification"},
            {"allocation permit", "Environmental/AllocationPermit"},
            {"allocation permit withdrawal node", "Environmental/AllocationPermitWithdrawalNode"},
            {"apc inspection item", "HealthAndSafety/ApcInspectionItem"},
            {"arc flash study", "Engineering/ArcFlashStudy"},
            {"as found condition", "Production/AsFoundCondition"},
            {"asset condition reason", "Production/AssetConditionReason"},
            {"asset reliability", "Production/AssetReliability" },
            {"as left condition", "Production/AsLeftCondition" },
            {"awia compliance", "Engineering/AwiaCompliance" },
            {"bacterial water sample", "WaterQuality/BacterialWaterSample"},
            {"below ground hazard", "Facilities/BelowGroundHazard"},
            {"billing party", "FieldOperations/BillingParty"},
            {"blow off inspection", "FieldOperations/BlowOffInspection"},
            {"bond", "FieldOperations/Bond"},
            {"chemical", "Environmental/Chemical"},
            {"chemical delivery", "Environmental/ChemicalDelivery"},
            {"chemical inventory transaction", "Environmental/ChemicalInventoryTransaction"},
            {"chemical storage", "Environmental/ChemicalStorage"},
            {"chemical storage location", "Environmental/ChemicalStorageLocation"},
            {"chemical unit cost", "Environmental/ChemicalUnitCost"},
            {"chemical vendor", "Environmental/ChemicalVendor"},
            {"chemical warehouse number", "Environmental/ChemicalWarehouseNumber"},
            {"community right to know", "Facilities/CommunityRightToKnow"},
            {"completed confined space form", "HealthAndSafety/ConfinedSpaceForm"},
            {"completed confined space form with section five", "HealthAndSafety/ConfinedSpaceForm"},
            {"confined space form", "HealthAndSafety/ConfinedSpaceForm"},
            {"contact without address", "contact"},
            {"contractor labor cost", "/ProjectManagement/ContractorLaborCost"},
            {"contractor override labor cost", "/ProjectManagement/ContractorOverrideLaborCost"},
            {"contractor agreement", "/Contractors/ContractorAgreement" },
            {"contractor insurance", "/Contractors/ContractorInsurance" },
            {"contractor user", "/Contractors/ContractorUser" },
            {"contractor", "/Contractors/Contractor" },
            {"corrective order problem code", "/Production/CorrectiveOrderProblemCode" },
            {"covid issue", "HumanResources/CovidIssue" },
            {"crew assignment", "FieldOperations/CrewAssignment"},
            {"crew assignments calendar", "FieldOperations/CrewAssignment/ShowCalendar"},
            {"development project", "ProjectManagement/DevelopmentProject"},
            {"easement", "Facilities/Easement"},
            {"echoshore leak alert", "FieldOperations/EchoshoreLeakAlert"},
            {"employee accountability action", "HumanResources/EmployeeAccountabilityAction"},
            {"employee head count", "HumanResources/EmployeeHeadCount"},
            {"end of pipe exceedance", "Environmental/EndOfPipeExceedance" },
            {"environmental permit", "Environmental/EnvironmentalPermit"},
            {"environmental permit fee", "Environmental/EnvironmentalPermitFee"},
            {"emergency response plan", "Facilities/EmergencyResponsePlan"},
            {"environmental non compliance event", "Environmental/EnvironmentalNonComplianceEvent" },
            {"estimating project", "ProjectManagement/EstimatingProject"},
            {"event", "Events/Event"},
            {"event document", "Events/EventDocument"},
            {"event type", "Events/EventType"},
            {"facility area", "Facilities/FacilityArea"},
            {"facility sub area", "Facilities/FacilitySubArea"},
            {"facility process", "Facilities/FacilityProcess"},
            {"family medical leave act case", "Operations/FamilyMedicalLeaveActCase"},
            {"field service representative non availability", "ShortCycle/FieldServiceRepresentativeNonAvailability"},
            {"functional location", "FieldOperations/FunctionalLocation"},
            {"gas monitor", "HealthAndSafety/GasMonitor"},
            {"gas monitor calibration", "HealthAndSafety/GasMonitorCalibration"},
            {"gas monitor equipment", "Equipment"}, // this is just a specific type of Equipment, it's not its own thing.
            {"general liability claim", "HealthAndSafety/GeneralLiabilityClaim"},
            {"hydrant", "FieldOperations/Hydrant"},
            {"hydrant inspection", "FieldOperations/HydrantInspection"},
            {"hydrant painting", "FieldOperations/HydrantPainting"},
            {"interconnection", "Facilities/Interconnection"},
            {"info master map", "ProjectManagement/InfoMasterMap" },
            {"job observation", "HealthAndSafety/JobObservation"},
            {"job site check list", "HealthAndSafety/JobSiteCheckList"},
            {"job site check list that is not signed off by a supervisor", "HealthAndSafety/JobSiteCheckList"},
            {"large service project", "ProjectManagement/LargeServiceProject"},
            {"lockout device", "HealthAndSafety/LockoutDevice"},
            {"lockout form", "HealthAndSafety/LockoutForm"},
            {"main crossing", "Facilities/MainCrossing"},
            {"main crossing inspection", "Facilities/MainCrossingInspection"},
            {"maintenance plan", "Production/MaintenancePlan"},
            {"maintenance plan task type", "Production/MaintenancePlanTaskType"},
            {"map image", "FieldOperations/MapImage"},
            {"markout", "FieldOperations/Markout"},
            {"markout damage", "FieldOperations/MarkoutDamage"},
            {"markout violation", "BPU/MarkoutViolation"},
            {"near miss", "HealthAndSafety/NearMiss"},
            {"npdes regulator inspection", "FieldOperations/NpdesRegulatorInspection" },
            {"one call markout audit", "FieldOperations/OneCallMarkoutAudit"},
            {"one call markout response", "FieldOperations/OneCallMarkoutResponse"},
            {"one call markout ticket", "FieldOperations/OneCallMarkoutTicket"},
            {"pipe data lookup value", "ProjectManagement/PipeDataLookupValue"},
            {"premise", "Customer/Premise" },
            {"notification configuration", "Admin/NotificationConfiguration"},
            {"plan review", "Facilities/PlanReview"},
            {"planning work order", "FieldOperations/WorkOrderPlanning" },
            {"production pre job safety brief", "Production/ProductionPreJobSafetyBrief"},
            {"production work description", "Production/ProductionWorkDescription"},
            {"production work order", "Production/ProductionWorkOrder"},
            {"public water supply firm capacity", "Environmental/PublicWaterSupplyFirmCapacity"},
            {"public water supply pressure zone", "Environmental/PublicWaterSupplyPressureZone"},
            {"recurring project", "ProjectManagement/RecurringProject" },
            {"red tag permit", "HealthAndSafety/RedTagPermit" },
            {"requisition", "FieldOperations/Requisition"},
            {"restoration", "FieldOperations/Restoration"},
            {"restoration processing", "FieldOperations/RestorationProcessing"},
            {"restoration type cost", "FieldOperations/RestorationTypeCost"},
            {"risk register asset", "Engineering/RiskRegisterAsset"},
            {"roadway improvement notification", "ProjectManagement/RoadwayImprovementNotification"},
            {"role group", "Admin/RoleGroup"},
            {"sample id matrix", "WaterQuality/SampleIdMatrix"},
            {"sample plan", "WaterQuality/SamplePlan"},
            {"sample site", "WaterQuality/SampleSite"},
            {"service", "FieldOperations/Service"},
            {"service premise contact", "FieldOperations/ServicePremiseContact"},
            {"service flush", "FieldOperations/ServiceFlush"},
            {"service installation", "FieldOperations/ServiceInstallation"},
            {"service line protection investigation", "ServiceLineProtection/ServiceLineProtectionInvestigation" },
            {"service restoration", "FieldOperations/ServiceRestoration"},
            {"sewer opening", "FieldOperations/SewerOpening"},
            {"sewer opening inspection", "FieldOperations/SewerOpeningInspection" },
            {"sewer main cleaning", "FieldOperations/SewerMainCleaning"},
            {"sewer overflow", "FieldOperations/SewerOverflow"},
            {"short cycle assignment", "ShortCycle/ShortCycleAssignment"},
            {"short cycle work order", "ShortCycle/ShortCycleWorkOrder"},
            {"short cycle work order completion", "ShortCycle/ShortCycleWorkOrderCompletion"},
            {"short cycle work order request", "ShortCycle/ShortCycleWorkOrderRequest"},
            {"short cycle work order safety brief", "HealthAndSafety/ShortCycleWorkOrderSafetyBrief"},
            {"short cycle work order status update", "ShortCycle/ShortCycleWorkOrderStatusUpdate"},
            {"short cycle work order time confirmation", "ShortCycle/ShortCycleWorkOrderTimeConfirmation"},
            {"smart cover alert", "FieldOperations/SmartCoverAlert"},
            {"spoil final processing location", "FieldOperations/SpoilFinalProcessingLocation"},
            {"spoil storage location", "FieldOperations/SpoilStorageLocation"},
            {"spoil removal", "FieldOperations/SpoilRemoval"},
            {"system delivery entry", "Production/SystemDeliveryEntry"},
            {"tailgate talk", "HealthAndSafety/TailgateTalk"},
            {"tailgate talk topic", "HealthAndSafety/TailgateTalkTopic"},
            {"tank inspection", "Facilities/TankInspection"},
            {"task group", "Production/TaskGroup" },
            {"task group category", "Production/TaskGroupCategory" },
            {"traffic control ticket", "FieldOperations/TrafficControlTicket"},
            {"traffic control ticket check", "FieldOperations/TrafficControlTicketCheck"},
            {"valve", "FieldOperations/Valve"},
            {"valve image", "FieldOperations/ValveImage"},
            {"valve inspection", "FieldOperations/ValveInspection"},
            {"waste water system", "Environmental/WasteWaterSystem" },
            {"waste water system basin", "Environmental/WasteWaterSystemBasin" },
            {"water constituent", "WaterQuality/WaterConstituent"},
            {"water quality complaint", "WaterQuality/WaterQualityComplaint"},
            {"water sample", "WaterQuality/WaterSample" },
            {"well test", "Production/WellTest" },
            {"work order", "FieldOperations/WorkOrder" },
            {"work order finalization", "FieldOperations/WorkOrderFinalization" },
            {"work order invoice", "FieldOperations/WorkOrderInvoice" },
            {"Operating Center Spoil Removal Cost", "FieldOperations/OperatingCenterSpoilRemovalCost" }
        };

        public const string CHANGE_TRACKING_USER_NAME = "change tracking user";

        public static SampleSiteCollectionType CreateSampleSiteCollectionType(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "raw":
                    return container.GetInstance<RawSampleSiteCollectionTypeFactory>().Create();
                case "in plant":
                    return container.GetInstance<InPlantSampleSiteCollectionTypeFactory>().Create();
                case "entry point":
                    return container.GetInstance<EntryPointSampleSiteCollectionTypeFactory>().Create();
                case "interconnect":
                    return container.GetInstance<InterconnectSampleSiteCollectionTypeFactory>().Create();
                case "distribution":
                    return container.GetInstance<DistributionSampleSiteCollectionTypeFactory>().Create();
                case "wastewater":
                    return container.GetInstance<WasteWaterSampleSiteCollectionTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create a sample site collection type with description '{nvc["description"]}'.");
            }
        }

        #endregion

        #region Fields

        private static string[] _ignorableCustomerProfileIds;
        private static IContainer _oldContainer;

        #endregion
        
        #region Private Methods

        public static void PreCreateObject()
        {
            Deleporter.Run(() => {
                _oldContainer = DependencyResolver.Current.GetService<IContainer>();
                
                var nested = _oldContainer.GetNestedContainer();
                
                nested.Inject<IAuthenticationService<User>>(
                    _oldContainer.GetInstance<ChangeTrackingUserAuthenticationService>());
                
                DependencyResolver.SetResolver(new StructureMapDependencyResolver(nested));
            });
        }

        public static void PostCreateObject()
        {
            Deleporter.Run(() => {
                var nested = DependencyResolver.Current.GetService<IContainer>();
                nested.Dispose();
                
                DependencyResolver.SetResolver(new StructureMapDependencyResolver(_oldContainer));
            });
        }
        
        #endregion

        #region Exposed Methods

        public static void Initialize()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetTypeDictionary(TYPE_DICTIONARY);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetFactoryAssembly(typeof(UserFactory).Assembly);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetModelAssembly(typeof(User).Assembly);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetPreCreateObjectFn(PreCreateObject);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetPostCreateObjectFn(PostCreateObject);
            MMSINC.Testing.SpecFlow.StepDefinitions.Navigation.SetPageStringDictionary(PAGE_STRINGS);
        }

        #endregion

        #region Step Definitions

        [Given("user \"([^\"]+)\" has role group \"([^\"]+)\"")]
        public static void GivenUserHasARoleGroup(string userId, string roleGroupId)
        {
            var user = TestObjectCache.Instance.Lookup<User>("user", userId);
            var roleGroup = TestObjectCache.Instance.Lookup<RoleGroup>("role group", roleGroupId);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterUser = session.Load<User>(user.Id);
                var deleporterRoleGroup = session.Load<RoleGroup>(roleGroup.Id);
                deleporterUser.RoleGroups.Add(deleporterRoleGroup);
                session.SaveOrUpdate(deleporterRoleGroup);
                session.Flush();
                session.Clear();
            });
        }

        [Given("operating center:? \"([^\"]+)\" has asset type:? \"([^\"]+)\"")]
        public static void GivenIAddAssetTypeToOperatingCenter(string operatingCenterId, string assetTypeId)
        {
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterId);
            var assetType = TestObjectCache.Instance.Lookup<AssetType>("asset type", assetTypeId);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterAssetType = session.Load<AssetType>(assetType.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterAssetType.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType {
                    AssetType = deleporterAssetType,
                    OperatingCenter = deleporterOperatingCenter
                });
                session.SaveOrUpdate(deleporterAssetType);
                session.Flush();
                session.Clear();
            });
        }

        [Given("sample site:? \"([^\"]+)\" exists in premise:? \"([^\"]+)\"")]
        public static void GivenIAddSampleSiteToPremise(string sampleSiteIdentifier, string premiseIdentifier)
        {
            var premise = TestObjectCache.Instance.Lookup<MapCall.Common.Model.Entities.Premise>("premise", premiseIdentifier);
            var sampleSite = TestObjectCache.Instance.Lookup<SampleSite>("sample site", sampleSiteIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterPremise = session.Load<MapCall.Common.Model.Entities.Premise>(premise.Id);
                var deleporterSampleSite = session.Load<SampleSite>(sampleSite.Id);
                deleporterPremise.SampleSites.Add(deleporterSampleSite);
                session.SaveOrUpdate(deleporterPremise);
            });
        }

        [Given("sample site lead copper tier classification:? \"([^\"]+)\" exists in state:? \"([^\"]+)\"")]
        public static void GivenIAddSampleSiteLeadCopperTierClassificationToState(string sampleSiteLeadCopperTierClassificationIdentifier, string stateIdentifier)
        {
            var state = TestObjectCache.Instance.Lookup<MapCall.Common.Model.Entities.State>("state", stateIdentifier);
            var sampleSiteLeadCopperTierClassification = TestObjectCache.Instance.Lookup<SampleSiteLeadCopperTierClassification>("sample site lead copper tier classification", sampleSiteLeadCopperTierClassificationIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterState = session.Load<MapCall.Common.Model.Entities.State>(state.Id);
                var deleporterSampleSiteLeadCopperTierClassification = session.Load<SampleSiteLeadCopperTierClassification>(sampleSiteLeadCopperTierClassification.Id);
                deleporterSampleSiteLeadCopperTierClassification.States.Add(deleporterState);
                session.SaveOrUpdate(deleporterSampleSiteLeadCopperTierClassification);
            });
        }

        [Given("sample site lead copper tier sample category:? \"([^\"]+)\" exists in sample site lead copper tier classification:? \"([^\"]+)\"")]
        public static void GivenIAddSampleSiteLeadCopperTierSampleCategoryToSampleSiteLeadCopperTierClassification(string sampleSiteLeadCopperTierSampleCategoryIdentifier, string sampleSiteLeadCopperTierClassificationIdentifier)
        {
            var sampleSiteLeadCopperTierSampleCategory = TestObjectCache.Instance.Lookup<SampleSiteLeadCopperTierSampleCategory>("sample site lead copper tier sample category", sampleSiteLeadCopperTierSampleCategoryIdentifier);
            var sampleSiteLeadCopperTierClassification = TestObjectCache.Instance.Lookup<SampleSiteLeadCopperTierClassification>("sample site lead copper tier classification", sampleSiteLeadCopperTierClassificationIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSampleSiteLeadCopperTierClassification = session.Load<SampleSiteLeadCopperTierClassification>(sampleSiteLeadCopperTierClassification.Id);
                var deleporterSampleSiteLeadCopperTierSampleCategory = session.Load<SampleSiteLeadCopperTierSampleCategory>(sampleSiteLeadCopperTierSampleCategory.Id);
                deleporterSampleSiteLeadCopperTierSampleCategory.TierClassifications.Add(deleporterSampleSiteLeadCopperTierClassification);
                session.SaveOrUpdate(deleporterSampleSiteLeadCopperTierSampleCategory);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in service material:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToServiceMaterial(string operatingCenterIdentifier, string serviceIdentifier)
        {
            var serviceMaterial = TestObjectCache.Instance.Lookup<ServiceMaterial>("service material", serviceIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterServiceMaterial = session.Load<ServiceMaterial>(serviceMaterial.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterServiceMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial { OperatingCenter = deleporterOperatingCenter, ServiceMaterial = deleporterServiceMaterial, NewServiceRecord = true });
                session.SaveOrUpdate(deleporterServiceMaterial);
            });
        }

        [Given("equipment:? \"([^\"]+)\" exists in facility:? \"([^\"]+)\"")]
        public static void GivenIAddEquipmentToFacility(string equipmentIdentifier, string facilityIdentifier)
        {
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);
            var facility = TestObjectCache.Instance.Lookup<Facility>("facility", facilityIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipment = session.Load<Equipment>(equipment.Id);
                var deleporterFacility = session.Load<Facility>(facility.Id);
                deleporterFacility.Equipment.Add(deleporterEquipment);
                session.SaveOrUpdate(deleporterFacility);
            });
        }

        [Given("maintenance plan:? \"([^\"]+)\" exists in equipment's facility:? \"([^\"]+)\"")]
        public static void GivenIAddMaintenancePlanToFacility(string maintenancePlanIdentifier, string equipmentIdentifier)
        {
            var maintenancePlan = TestObjectCache.Instance.Lookup<MaintenancePlan>("maintenance plan", maintenancePlanIdentifier);
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterMaintenancePlan = session.Load<MaintenancePlan>(maintenancePlan.Id);
                var deleporterEquipment = session.Load<Equipment>(equipment.Id);
                deleporterEquipment.Facility.MaintenancePlans.Add(deleporterMaintenancePlan);
                session.SaveOrUpdate(deleporterEquipment);
            });
        }
        
        [Given("equipment:? \"([^\"]+)\" exists in maintenance plan:? \"([^\"]+)\"")]
        public static void GivenIAddEquipmentToMaintenancePlan(string equipmentIdentifier, string maintenancePlanIdentifier)
        {
            var maintenancePlan = TestObjectCache.Instance.Lookup<MaintenancePlan>("maintenance plan", maintenancePlanIdentifier);
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);
            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterMaintenancePlan = session.Load<MaintenancePlan>(maintenancePlan.Id);
                var deleporterEquipment = session.Load<Equipment>(equipment.Id);
                deleporterMaintenancePlan.Equipment.Add(deleporterEquipment);
                session.SaveOrUpdate(deleporterMaintenancePlan);
            });
        }

        [Given("maintenance plan:? \"([^\"]+)\" exists in equipment:? \"([^\"]+)\"")]
        public static void GivenIAddMaintenancePlanToEquipment(string maintenancePlanIdentifier, string equipmentIdentifier)
        {
            var maintenancePlan = TestObjectCache.Instance.Lookup<MaintenancePlan>("maintenance plan", maintenancePlanIdentifier);
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);
            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterMaintenancePlan = session.Load<MaintenancePlan>(maintenancePlan.Id);
                var deleporterEquipment = session.Load<Equipment>(equipment.Id);
                deleporterEquipment.MaintenancePlans.Add(new EquipmentMaintenancePlan {
                    MaintenancePlan = deleporterMaintenancePlan,
                    Equipment = deleporterEquipment
                });
                session.SaveOrUpdate(deleporterEquipment);
            });
        }

        [Given("equipment: \"([^\"]+)\" exists in production work order: \"([^\"]+)\"")]
        public static void GivenIAddEquipmentToProductionWorkOrder(string equipmentIdentifier, string productionWorkOrderIdentifier)
        {
            var productionWorkOrder = TestObjectCache.Instance.Lookup<ProductionWorkOrder>("production work order", productionWorkOrderIdentifier);
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterProductionWorkOrder = session.Load<ProductionWorkOrder>(productionWorkOrder.Id);
                var deleporterEquipment = session.Load<Equipment>(equipment.Id);
                deleporterProductionWorkOrder.Equipments.Add(new ProductionWorkOrderEquipment { ProductionWorkOrder = deleporterProductionWorkOrder, Equipment = deleporterEquipment });
                session.SaveOrUpdate(deleporterProductionWorkOrder);
            });
        }

        [Given("employee assignment: \"([^\"]+)\" exists in production work order: \"([^\"]+)\"")]
        public static void GivenIAddEmplyoyeeAssignmentToProductionWorkOrder(string assignmentIdentifier, string productionWorkOrderIdentifier)
        {
            var productionWorkOrder = TestObjectCache.Instance.Lookup<ProductionWorkOrder>("production work order", productionWorkOrderIdentifier);
            var assignment = TestObjectCache.Instance.Lookup<EmployeeAssignment>("employee assignment", assignmentIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterProductionWorkOrder = session.Load<ProductionWorkOrder>(productionWorkOrder.Id);
                var deleporterAssignment = session.Load<EmployeeAssignment>(assignment.Id);
                deleporterAssignment.ProductionWorkOrder = deleporterProductionWorkOrder;
                deleporterProductionWorkOrder.EmployeeAssignments.Add(deleporterAssignment);
                session.SaveOrUpdate(deleporterProductionWorkOrder);
            });
        }

        [Given("markout damage \"([^\"]+)\" has ([^\"]+) markout damage utility damage type \"([^\"]+)\"")]
        public static void GivenMarkoutDamageHasMarkoutDamageUtilityDamageType(string markoutDamageName, string damageFactoryTypeNamePrefix, string utilityDamageName)
        {
            var markoutDamage = TestObjectCache.Instance.Lookup<MarkoutDamage>("markout damage", markoutDamageName);
            var utilityDamage = TestObjectCache.Instance.Lookup<MarkoutDamageUtilityDamageType>($"{damageFactoryTypeNamePrefix} markout damage utility damage type", utilityDamageName);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterMarkoutDamage = session.Load<MarkoutDamage>(markoutDamage.Id);
                var deleporterUtilityDamage = session.Load<MarkoutDamageUtilityDamageType>(utilityDamage.Id);
                deleporterMarkoutDamage.UtilityDamages.Add(deleporterUtilityDamage);
                session.SaveOrUpdate(deleporterMarkoutDamage);
                session.Flush();
                session.Clear();
            });
        }

        [Given("facility system delivery entry type:? \"([^\"]+)\" exists in facility:? \"([^\"]+)\"")]
        public static void GivenIAddFacilitySysDelEntryToFacility(string sysdelIdentenfier, string facilityIdentifier)
        {
            var sysdel = TestObjectCache.Instance.Lookup<FacilitySystemDeliveryEntryType>("facility system delivery entry type", sysdelIdentenfier);
            var facility = TestObjectCache.Instance.Lookup<Facility>("facility", facilityIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSysDel = session.Load<FacilitySystemDeliveryEntryType>(sysdel.Id);
                var deleporterFacility = session.Load<Facility>(facility.Id);
                deleporterSysDel.Facility = deleporterFacility;
                deleporterFacility.FacilitySystemDeliveryEntryTypes.Add(deleporterSysDel);
                session.SaveOrUpdate(deleporterFacility);
            });
        }

        [Given("facility:? \"([^\"]+)\" exists in system delivery entry:? \"([^\"]+)\"")]
        public static void GivenIAddFacilityToSysDelEntry(string facilityIdentifier, string sysdelIdentenfier)
        {
            var sysdel = TestObjectCache.Instance.Lookup<SystemDeliveryEntry>("system delivery entry", sysdelIdentenfier);
            var facility = TestObjectCache.Instance.Lookup<Facility>("facility", facilityIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSysDel = session.Load<SystemDeliveryEntry>(sysdel.Id);
                var deleporterFacility = session.Load<Facility>(facility.Id);
                deleporterSysDel.Facilities.Add(deleporterFacility);
                session.SaveOrUpdate(deleporterSysDel);
            });
        }

        [Given("notification purpose:? \"([^\"]+)\" exists in notification configuration:? \"([^\"]+)\"")]
        public static void GivenANotificationPurposeExistsInANotificationConfiguration(
            string notificationPurposeIdentifier, 
            string notificationConfigurationIdentifier)
        {
            var cachedNotificationPurpose = TestObjectCache.Instance.Lookup<NotificationPurpose>("notification purpose", notificationPurposeIdentifier);
            var cachedNotificationConfiguration = TestObjectCache.Instance.Lookup<NotificationConfiguration>("notification configuration", notificationConfigurationIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var notificationPurpose = session.Load<NotificationPurpose>(cachedNotificationPurpose.Id);
                var notificationConfiguration = session.Load<NotificationConfiguration>(cachedNotificationConfiguration.Id);

                notificationConfiguration.NotificationPurposes.Add(notificationPurpose);
                session.SaveOrUpdate(notificationConfiguration);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in system delivery entry:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToSysDelEntry(string ocIdentifier, string sysdelIdentenfier)
        {
            var sysdel = TestObjectCache.Instance.Lookup<SystemDeliveryEntry>("system delivery entry", sysdelIdentenfier);
            var oc = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", ocIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSysDel = session.Load<SystemDeliveryEntry>(sysdel.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(oc.Id);
                deleporterSysDel.OperatingCenters.Add(deleporterOperatingCenter);
                session.SaveOrUpdate(deleporterSysDel);
            });
        }

        [Given("equipment purpose:? \"([^\"]+)\" has equipment sub category:? \"([^\"]+)\"")]
        public static void IAddEquipmentSubCategoryToEquipmentPurpose(string equipmentPurpose, string equipmentSubType)
        {
            var equipmentSub = TestObjectCache.Instance.Lookup<EquipmentSubCategory>("equipment subcategory", equipmentSubType);
            var equipmentPurposeactual = TestObjectCache.Instance.Lookup<EquipmentPurpose>("equipment purpose", equipmentPurpose);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipmentSubType = session.Load<EquipmentSubCategory>(equipmentSub.Id);
                var deleporterEquipmentPurpose = session.Load<EquipmentPurpose>(equipmentPurposeactual.Id);
                deleporterEquipmentPurpose.EquipmentSubCategory = deleporterEquipmentSubType;
                session.SaveOrUpdate(deleporterEquipmentPurpose);
            });
        }
        
        [Given("system delivery entry:? \"([^\"]+)\" has system delivery facility entry:? \"([^\"]+)\"")]
        public static void IAddSystemDeliveryFacilityEntryToSystemDeliveryEntry(string sysDelEntry, string sysDelFacilityEntry)
        {
            var systemDeliveryEntry = TestObjectCache.Instance.Lookup<SystemDeliveryEntry>("system delivery entry", sysDelEntry);
            var systemDeliveryFacilityEntry = TestObjectCache.Instance.Lookup<SystemDeliveryFacilityEntry>("system delivery facility entry", sysDelFacilityEntry);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSystemDeliveryEntry = session.Load<SystemDeliveryEntry>(systemDeliveryEntry.Id);
                var deleporterSystemDeliveryFacilityEntry = session.Load<SystemDeliveryFacilityEntry>(systemDeliveryFacilityEntry.Id);
                deleporterSystemDeliveryEntry.FacilityEntries.Add(deleporterSystemDeliveryFacilityEntry);
                session.SaveOrUpdate(deleporterSystemDeliveryEntry);
            });
        }

        [Given("water system:? \"([^\"]+)\" exists in operating center:? \"([^\"]+)\"")]
        public static void GivenIAddWaterSystemToOperatingCenter(string waterSystemIdentifier, string operatingCenterIdentifier)
        {
            var waterSystem = TestObjectCache.Instance.Lookup<WaterSystem>("water system", waterSystemIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterWaterSystem = session.Load<WaterSystem>(waterSystem.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterWaterSystem.OperatingCenters.Add(deleporterOperatingCenter);
                session.SaveOrUpdate(deleporterWaterSystem);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in town:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToTown(string operatingCenterIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterTown.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = deleporterOperatingCenter, Town = deleporterTown, Abbreviation = "XX" });
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in town:? \"([^\"]+)\" with abbreviation:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToTown(string operatingCenterIdentifier, string townIdentifier, string abbreviation)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterTown.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = deleporterOperatingCenter, Town = deleporterTown, Abbreviation = abbreviation });
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("waste water system: \"([^\"]+)\" exists in town: \"([^\"]+)\"")]
        public static void GivenIAddWasteWaterSystemToTown(string wasteWaterSystemIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var wasteWaterSystem = TestObjectCache.Instance.Lookup<WasteWaterSystem>("waste water system", wasteWaterSystemIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterWasteWaterSystem = session.Load<WasteWaterSystem>(wasteWaterSystem.Id);
                deleporterTown.WasteWaterSystems.Add(deleporterWasteWaterSystem);
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("public water supply: \"([^\"]+)\" exists in town: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyToTown(string publicWaterSupplyIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterTown.PublicWaterSupplies.Add(deleporterPublicWaterSupply);
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("public water supply firm capacity: \"([^\"]+)\" exists in public water supply: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyFirmCapacityToPublicWaterSupply(string publicWaterSupplyFirmCapacityIdentifier, string publicWaterSupplyIdentifier)
        {
            var firmCapacity = TestObjectCache.Instance.Lookup<PublicWaterSupplyFirmCapacity>("public water supply firm capacity", publicWaterSupplyFirmCapacityIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterFirmCapacity = session.Load<PublicWaterSupplyFirmCapacity>(firmCapacity.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterPublicWaterSupply.FirmCapacities.Add(deleporterFirmCapacity);
                session.SaveOrUpdate(deleporterPublicWaterSupply);
            });
        }

        [Given("public water supply pressure zone: \"([^\"]+)\" exists in public water supply: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyPressureZoneToPublicWaterSupply(string publicWaterSupplyPressureZoneIdentifier, string publicWaterSupplyIdentifier)
        {
            var pressureZone = TestObjectCache.Instance.Lookup<PublicWaterSupplyPressureZone>("public water supply pressure zone", publicWaterSupplyPressureZoneIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterPressureZone = session.Load<PublicWaterSupplyPressureZone>(pressureZone.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterPublicWaterSupply.PressureZones.Add(deleporterPressureZone);
                session.SaveOrUpdate(deleporterPublicWaterSupply);
            });
        }

        [Given("operating center: \"([^\"]+)\" exists in public water supply: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyToOperatingCenter(string operatingCenterIdentifier, string publicWaterSupplyIdentifier)
        {
            var opc = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterOperatingCenter = session.Load<OperatingCenter>(opc.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterPublicWaterSupply.OperatingCenterPublicWaterSupplies.Add(new OperatingCenterPublicWaterSupply { OperatingCenter = deleporterOperatingCenter, PublicWaterSupply = deleporterPublicWaterSupply });
                session.SaveOrUpdate(deleporterPublicWaterSupply);
            });
        }

        [Given("meter: \"([^\"]+)\" exists in interconnection: \"([^\"]+)\"")]
        public static void GivenIAddMeterToInterconnection(string meterIdentifier, string interconnectionIdentifier)
        {
            var interconnection = TestObjectCache.Instance.Lookup<Interconnection>("interconnection", interconnectionIdentifier);
            var meter = TestObjectCache.Instance.Lookup<Meter>("meter", meterIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterInterconnection = session.Load<Interconnection>(interconnection.Id);
                var deleporterMeter = session.Load<Meter>(meter.Id);
                deleporterInterconnection.Meters.Add(deleporterMeter);
                session.SaveOrUpdate(deleporterInterconnection);
            });
        }

        //And operating center: "nj7" exists in environmental permit: "one"
        [Given("operating center: \"([^\"]+)\" exists in environmental permit: \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToEnviromentalPermit(string operatingCenterIdentifier, string environmentalPermitIdentifier)
        {
            var opc = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);
            var permit = TestObjectCache.Instance.Lookup<EnvironmentalPermit>("environmental permit", environmentalPermitIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterOperatingCenter = session.Load<OperatingCenter>(opc.Id);
                var deleporterPermit = session.Load<EnvironmentalPermit>(permit.Id);
                deleporterPermit.OperatingCenters.Add(deleporterOperatingCenter);
                session.SaveOrUpdate(deleporterPermit);
            });
        }

	    //And equipment type "rtu" exists in corrective order problem code "two"
        [Given("equipment type: \"([^\"]+)\" exists in corrective order problem code: \"([^\"]+)\"")]
        public static void GivenIAddEquipmentTypeToCorrectiveOrderProblemCode(string sapEnvironmentType, string correctiveOrderProblemCode)
        {
            var sap = TestObjectCache.Instance.Lookup<EquipmentType>("equipment type", sapEnvironmentType);
            var correctiveOrderProblem = TestObjectCache.Instance.Lookup<CorrectiveOrderProblemCode>("corrective order problem code", correctiveOrderProblemCode);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipmentType = session.Load<EquipmentType>(sap.Id);
                var deleporterCorrectiveOrderProblemCode = session.Load<CorrectiveOrderProblemCode>(correctiveOrderProblem.Id);
                deleporterCorrectiveOrderProblemCode.EquipmentTypes.Add(deleporterEquipmentType);
                session.SaveOrUpdate(deleporterCorrectiveOrderProblemCode);
            });
        }


        [Given("position group common name: \"([^\"]+)\" exists in training requirement: \"([^\"]+)\"")]
        public static void GivenIAddPositionGroupCommonNameToTrainingRequirement(string positionGroupCommonNameName, string trainingRequirementName)
        {
            var trainingRequirementId = TestObjectCache.Instance.Lookup<TrainingRequirement>("training requirement", trainingRequirementName).Id;
            var positionGroupCommonNameId = TestObjectCache.Instance.Lookup<PositionGroupCommonName>("position group common name", positionGroupCommonNameName).Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var trainingRequirement = session.Load<TrainingRequirement>(trainingRequirementId);
                var jobTitleCommonName = session.Load<PositionGroupCommonName>(positionGroupCommonNameId);
                trainingRequirement.PositionGroupCommonNames.Add(jobTitleCommonName);
                session.SaveOrUpdate(trainingRequirement);
            });
        }

        [Given("^employee:? \"([^\"]+)\" has production skill set:? \"([^\"]+)\"$")]
        public static void GivenEmployeeHasProductionSkillSet(string employeeName, string productionSkillSetName)
        {
            var employeeId = TestObjectCache.Instance.Lookup<Employee>("employee", employeeName).Id;
            var productionSkillSetId =
                TestObjectCache.Instance.Lookup<ProductionSkillSet>("production skill set", productionSkillSetName).Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var employee = session.Load<Employee>(employeeId);
                var productionSkillSet = session.Load<ProductionSkillSet>(productionSkillSetId);
                session.Save(new EmployeeProductionSkillSet {
                    Employee = employee,
                    ProductionSkillSet = productionSkillSet
                });
            });
        }

        [Given("^employee: \"([^\"]+)\" is scheduled for training record: \"([^\"]+)\"$")]
        public static void GivenIAddEmployeeToScheduleForTrainingRecord(string employeeName, string trainingRecordName)
        {
            var employeeId = TestObjectCache.Instance.Lookup<Employee>("employee", employeeName).Id;
            var trainingRecordId = TestObjectCache.Instance.Lookup<TrainingRecord>("training record", trainingRecordName).Id;
            var dataTypeId = TestObjectCache.Instance.Lookup<DataType>("data type", "employees scheduled").Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var employee = session.Load<Employee>(employeeId);
                var dataType = session.Load<DataType>(dataTypeId);
                session.Save(new EmployeeLink {
                    LinkedId = trainingRecordId,
                    DataType = dataType,
                    Employee = employee,
                    LinkedBy = "regression test step",
                    LinkedOn = DateTime.Now
                });
            });
        }

        [Given("^employee: \"([^\"]+)\" attended training record: \"([^\"]+)\"$")]
        public static void GivenANamedEmployeeAttendedATrainingRecord(string employeeName, string trainingRecordName)
        {
            var employeeId = TestObjectCache.Instance.Lookup<Employee>("employee", employeeName).Id;
            var trainingRecordId = TestObjectCache.Instance.Lookup<TrainingRecord>("training record", trainingRecordName).Id;
            var dataTypeId = TestObjectCache.Instance.Lookup<DataType>("data type", "employees attended").Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var employee = session.Load<Employee>(employeeId);
                var dataType = session.Load<DataType>(dataTypeId);
                session.Save(new EmployeeLink {
                    LinkedId = trainingRecordId,
                    DataType = dataType,
                    Employee = employee,
                    LinkedBy = "regression test step",
                    LinkedOn = DateTime.Now
                });
            });
        }

        [Given("equipment: \"([^\"]+)\" has production prerequisite: \"([^\"]+)\"")]
        public static void GivenIAddAProductionPrerequisiteToEquipment(string equipmentIdentifier, string productionprereqIdentifier)
        {
            var eq = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);
            var prereq = TestObjectCache.Instance.Lookup<ProductionPrerequisite>("production prerequisite", productionprereqIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipment = session.Load<Equipment>(eq.Id);
                var deleporterPrereq = session.Load<ProductionPrerequisite>(prereq.Id);
                deleporterEquipment.ProductionPrerequisites.Add(deleporterPrereq);
                session.SaveOrUpdate(deleporterEquipment);
            });
        }

        [Given("voltage:? \"([^\"]+)\" exists in utility transformer k v a rating:? \"([^\"]+)\"")]
        public static void GivenVoltageContainsUtilityTransformerKRVRating(string voltage, string rating)
        {
            var voltageId = TestObjectCache.Instance.Lookup<Voltage>("voltage", voltage).Id;
            var ratingId = TestObjectCache.Instance.Lookup<UtilityTransformerKVARating>("utility transformer k v a rating", rating).Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterVoltage = session.Load<Voltage>(voltageId);
                var deleporterRating = session.Load<UtilityTransformerKVARating>(ratingId);
                deleporterVoltage.UtilityTransformerKVARatings.Add(deleporterRating);
                deleporterRating.Voltages.Add(deleporterVoltage);
                session.SaveOrUpdate(deleporterVoltage);
            });
        }

        [Given("meter supplemental location:? \"([^\"]+)\" exists in small meter location:? \"([^\"]+)\"")]
        public static void GivenIAddAMeterSupplementalLocationToAServiceInstallationPosition(string meterSupplementalLocationIdentifier, string smallMeterLocationIdentifier)
        {
            var smallMeterLocation = TestObjectCache.Instance.Lookup<SmallMeterLocation>("small meter location", smallMeterLocationIdentifier);
            var meterSupplementalLocation = TestObjectCache.Instance.Lookup<MeterSupplementalLocation>("meter supplemental location", meterSupplementalLocationIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSmallMeterLocation = session.Load<SmallMeterLocation>(smallMeterLocation.Id);
                var deleporterMeterSupplementalLocation = session.Load<MeterSupplementalLocation>(meterSupplementalLocation.Id);
                deleporterSmallMeterLocation.MeterSupplementalLocations.Add(deleporterMeterSupplementalLocation);
                session.SaveOrUpdate(deleporterSmallMeterLocation);
            });
        }

        #region AccountingType stuff that exists solely for one workorder supervisor approval test

        public static AccountingType CreateAccountingType(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException(
                    "You must provide a description.");
            }

            switch (nvc["description"].ToLowerInvariant())
            {
                case "capital":
                    return container.GetInstance<CapitalAccountingTypeFactory>().Create();
                case "o and m":
                    return container.GetInstance<OAndMAccountingTypeFactory>().Create();
                case "retirement":
                    return container.GetInstance<RetirementAccountingTypeFactory>().Create();
               
                default:
                    throw new InvalidOperationException(
                        $"Unable to create accounting type with description '{nvc["description"]}'.");
            }
        }

        [Given("accounting types exist")]
        public static void GivenAccountingTypesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createAccountingType = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };
            
            createAccountingType("capital");
            createAccountingType("o and m");
            createAccountingType("retirement");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("accounting type", nameValues);
        }

        [Given("accounting type \"([^\"]+)\" is set for work description \"([^\"]+)\"")]
        public static void GivenISetAccountingTypeOnWorkDescription(string accountingTypeName, string workDescriptionName)
        {
            var accountingType = TestObjectCache.Instance.Lookup<AccountingType>("accounting type", accountingTypeName);
            var workDescription = TestObjectCache.Instance.Lookup<WorkDescription>("work description", workDescriptionName);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterAccountingType = session.Load<AccountingType>(accountingType.Id);
                var deleporterWorkDescription = session.Load<WorkDescription>(workDescription.Id);
                deleporterWorkDescription.AccountingType = deleporterAccountingType;
                session.SaveOrUpdate(deleporterWorkDescription);
            });
        }

        #endregion

        [Given("states of matter exist")]
        public static void GivenStatesofMatterExist()
        {
            Action<string> createChemicalFormType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("state of matter",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createChemicalFormType("Solid");
            createChemicalFormType("Liquid");
            createChemicalFormType("Gas");
        }
        
        [Given("packaging types exist")]
        public static void GivenPackagingTypesExist()
        {
            Action<string> createpackagingType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("packaging type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createpackagingType("Bag");
            createpackagingType("Bulk");
            createpackagingType("Cylinder");
            createpackagingType("Drum");
            createpackagingType("Mini-Bulk");
            createpackagingType("Pail/carboy");
            createpackagingType("Sack");
            createpackagingType("Tote");
        }

        [Given("chemical types exist")]
        public static void GivenChemicalTypesExist()
        {
            Action<string> createChemicalTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("chemical type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createChemicalTypes("Algicide");
            createChemicalTypes("Coagulant");
            createChemicalTypes("Disinfectant");
            createChemicalTypes("Oxidizer");
            createChemicalTypes("Polymer");
            createChemicalTypes("Sequestrant");
        }

        [Given("sample site address location types exist")]
        public static void GivenSampleSiteAddressLocationTypesExist()
        {
            Action<string> createSampleSiteAddressLocationType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sample site address location type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createSampleSiteAddressLocationType("Facility");
            createSampleSiteAddressLocationType("Premise");
            createSampleSiteAddressLocationType("Hydrant");
            createSampleSiteAddressLocationType("Valve");
            createSampleSiteAddressLocationType("Custom");
            createSampleSiteAddressLocationType("Pending Acquisition");
        }

        [Given("sample site inactivation reasons exist")]
        public static void GivenSampleSiteInactivationReasonsExist()
        {
            Action<string> createEntityLookup = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sample site inactivation reason",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createEntityLookup("customer declined program");
            createEntityLookup("customer opted out");
            createEntityLookup("customer service line replaced");
            createEntityLookup("company service line replaced");
            createEntityLookup("internal plumbing replaced");
            createEntityLookup("building demolished");
            createEntityLookup("other");
            createEntityLookup("new service details");
        }

        [Given("sample site point of use treatment types exist")]
        public static void GivenSampleSitePointOfUseTreatmentTypesExist()
        {
            Action<string> createSampleSitePointOfUseTreatmentType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sample site point of use treatment type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createSampleSitePointOfUseTreatmentType("None");
            createSampleSitePointOfUseTreatmentType("Entire Building");
            createSampleSitePointOfUseTreatmentType("Individual Taps");
            createSampleSitePointOfUseTreatmentType("Faucet Filter");
            createSampleSitePointOfUseTreatmentType("Water Softener");
            createSampleSitePointOfUseTreatmentType("Whole Home Filter");
            createSampleSitePointOfUseTreatmentType("Other");
        }

        [Given("service utility types exist")]
        public static void GivenServiceUtilityTypesExist()
        {
            Action<string> createServiceUtilityType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("service utility type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createServiceUtilityType("Service Utility Type1");
            createServiceUtilityType("Public Fire Service");
            createServiceUtilityType("Private Fire Service");
            createServiceUtilityType("Irrigation");
            createServiceUtilityType("Domestic Water");
            createServiceUtilityType("Domestic Wastewater");
        }

        [Given("sample site location types exist")]
        public static void GivenSampleSiteLocationTypesExist()
        {
            Action<string> createEntity = (description) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sample site location type",
                    description.ToLowerInvariant(),
                    $"description: \"{description}\"", TestObjectCache.Instance);
            createEntity("Primary");
            createEntity("Upstream");
            createEntity("Downstream");
            createEntity("Groundwater");
        }

        [Given("public water supply statuses exist")]
        public static void GivenPublicWaterSupplyStatusesExist()
        {
            Action<string> create = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("public water supply status",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            create("Active");
            create("Pending");
            create("Pending Merger");
            create("Inactive");
            create("Inactive -see note");
        }

        [Given("waste water system statuses exist")]
        public static void GivenWasteWaterSystemStatusesExist()
        {
            Action<string> create = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("waste water system status",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            create("Active");
            create("Pending");
            create("Pending Merger");
            create("Inactive");
            create("Inactive-see note");
        }

        [Given("communication types exist")]
        public static void GivenCommunicationTypesExist()
        {
            Action<string> createCommunicationType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("communication type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createCommunicationType("Letter");
            createCommunicationType("Electronic");
            createCommunicationType("Upload");
            createCommunicationType("Email");
            createCommunicationType("PDF");
            createCommunicationType("Agency Submittal Form");
            createCommunicationType("Other");
        }

        [Given("work order requesters exist")]
        public static void GivenWorkOrderRequestersExist()
        {
            Action<string> createRequester = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("work order requester",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createRequester("Customer");
            createRequester("Employee");
            createRequester("Local Government");
            createRequester("Call Center");
            createRequester("FRCC");
            createRequester("Acoustic Monitoring");
            createRequester("NSI");
        }

        [Given("work order purposes exist")]
        public static void GivenWorkOrderPurposesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createPurpose = (desc) =>
            {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createPurpose("customer");
            createPurpose("compliance");
            createPurpose("safety");
            createPurpose("leak detection");
            createPurpose("revenue 150 to 500");
            createPurpose("revenue 500 to 1000");
            createPurpose("revenue above 1000");
            createPurpose("damaged billable");
            createPurpose("estimates");
            createPurpose("water quality");
            createPurpose("asset record control");
            createPurpose("seasonal");
            createPurpose("demolition");
            createPurpose("bpu");
            createPurpose("hurricane sandy");
            createPurpose("equip reliability");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("work order purpose", nameValues);
        }

        [Given("work order priorities exist")]
        public static void GivenWorkOrderPrioritiesExist()
        {
            Action<string> createPriority = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("work order priority",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createPriority("emergency");
            createPriority("high priority");
            createPriority("routine");
        }

        [Given("production work order frequencies exist")]
        public static void GivenProductionWorkOrderFrequenciesExist()
        {
            Action<string> createFrequency = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("production work order frequency",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createFrequency("daily");
            createFrequency("weekly");
            createFrequency("twice per month");
            createFrequency("monthly");
            createFrequency("quarterly");
            createFrequency("every four months");
            createFrequency("every six months");
            createFrequency("annual");
            createFrequency("every two years");
            createFrequency("every three years");
            createFrequency("every four years");
            createFrequency("every five years");
            createFrequency("every ten years");
            createFrequency("every fifteen years");
        }

        [Given("production work order priorities exist")]
        public static void GivenProductionWorkOrderPrioritiesExist()
        {
            Action<string> createPriority = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("production work order priority",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createPriority("emergency");
            createPriority("high");
            createPriority("routine");
            createPriority("medium");
            createPriority("low");
            createPriority("routine - off scheduled");
        }

        [Given("equipment statuses exist")]
        public static void GivenEquipmentStatusesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createEquipmentStatus = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createEquipmentStatus("in service");
            createEquipmentStatus("out of service");
            createEquipmentStatus("pending");
            createEquipmentStatus("retired");
            createEquipmentStatus("pending retirement");
            createEquipmentStatus("cancelled");
            createEquipmentStatus("field installed");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("equipment status", nameValues);
        }

        [Given("facility statuses exist")]
        public static void GivenFacilityStatusesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createFacilityStatus = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createFacilityStatus("active");
            createFacilityStatus("inactive");
            createFacilityStatus("pending");
            createFacilityStatus("pending_retirement");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("facility status", nameValues);
        }

        [Given("work descriptions exist")]
        public static void GivenWorkDescriptionsExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createDescription = (desc) =>
            {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createDescription("water main bleeders");
            createDescription("change burst meter");
            createDescription("check no water");
            createDescription("curb box repair");
            createDescription("ball curb stop repair");
            createDescription("excavate meter box setter");
            createDescription("service line flow test");
            createDescription("hydrant frozen");
            createDescription("frozen meter set");
            createDescription("frozen service line company side");
            createDescription("frozen service line cust side");
            createDescription("ground water service");
            createDescription("hydrant flushing");
            createDescription("hydrant investigation");
            createDescription("hydrant installation");
            createDescription("hydrant leaking");
            createDescription("hydrant no drip");
            createDescription("hydrant repair");
            createDescription("hydrant replacement");
            createDescription("hydrant retirement");
            createDescription("inactive account");
            createDescription("valve blow off installation");
            createDescription("fire service installation");
            createDescription("install line stopper");
            createDescription("install meter");
            createDescription("interior setting repair");
            createDescription("service investigation");
            createDescription("main investigation");
            createDescription("leak in meter box inlet");
            createDescription("leak in meter box outlet");
            createDescription("leak survey");
            createDescription("meter box setter installation");
            createDescription("meter change");
            createDescription("meter box adjustment resetter");
            createDescription("new main flushing");
            createDescription("service line installation");
            createDescription("service line leak cust side");
            createDescription("service line renewal");
            createDescription("service line retire");
            createDescription("sump pump");
            createDescription("test shut down");
            createDescription("valve box repair");
            createDescription("valve box blow off repair");
            createDescription("service line valve box repair");
            createDescription("valve investigation");
            createDescription("valve leaking");
            createDescription("valve repair");
            createDescription("valve blow off repair");
            createDescription("valve replacement");
            createDescription("valve retirement");
            createDescription("water ban restriction violator");
            createDescription("water main break repair");
            createDescription("water main installation");
            createDescription("water main retirement");
            createDescription("flushing service");
            createDescription("water main break replace");
            createDescription("meter box setter replace");
            createDescription("sewer main break repair");
            createDescription("sewer main break replace");
            createDescription("sewer main retirement");
            createDescription("sewer main installation");
            createDescription("sewer main cleaning");
            createDescription("sewer lateral installation");
            createDescription("sewer lateral repair");
            createDescription("sewer lateral replace");
            createDescription("sewer lateral retire");
            createDescription("sewer lateral customer side");
            createDescription("sewer opening repair");
            createDescription("sewer opening replace");
            createDescription("sewer opening installation");
            createDescription("sewer main overflow");
            createDescription("sewer backup company side");
            createDescription("hydraulic flow test");
            createDescription("markout crew");
            createDescription("valve box replacement");
            createDescription("site inspection survey new service");
            createDescription("site inspection survey service renewal");
            createDescription("service line repair");
            createDescription("sewer clean out installation");
            createDescription("sewer clean out repair");
            createDescription("sewer camera service");
            createDescription("sewer camera main");
            createDescription("sewer demolition inspection");
            createDescription("sewer main test holes");
            createDescription("water main test holes");
            createDescription("valve broken");
            createDescription("ground water main");
            createDescription("service turn on");
            createDescription("service turn off");
            createDescription("meter obtain read");
            createDescription("meter final start read");
            createDescription("meter repair touch pad");
            createDescription("valve installation");
            createDescription("valve blow off replacement");
            createDescription("hydrant paint");
            createDescription("ball curb stop replace");
            createDescription("valve blow off retirement");
            createDescription("valve blow off broken");
            createDescription("water main relocation");
            createDescription("hydrant relocation");
            createDescription("service relocation");
            createDescription("sewer investigation main");
            createDescription("sewer service overflow");
            createDescription("sewer investigation lateral");
            createDescription("sewer investigation opening");
            createDescription("sewer lift station repair");
            createDescription("curb box replace");
            createDescription("service line valve box replace");
            createDescription("storm catch repair");
            createDescription("storm catch replace");
            createDescription("storm catch installation");
            createDescription("storm catch investigation");
            createDescription("hydrant landscaping");
            createDescription("hydrant restoration investigation");
            createDescription("hydrant restoration repair");
            createDescription("main landscaping");
            createDescription("main restoration investigation");
            createDescription("main restoration repair");
            createDescription("service landscaping");
            createDescription("service restoration investigation");
            createDescription("service restoration repair");
            createDescription("sewer lateral landscaping");
            createDescription("sewer lateral restoration investigation");
            createDescription("sewer lateral restoration repair");
            createDescription("sewer main landscaping");
            createDescription("sewer main restoration investigation");
            createDescription("sewer main restoration repair");
            createDescription("sewer opening landscaping");
            createDescription("sewer opening restoration investigation");
            createDescription("sewer opening restoration repair");
            createDescription("valve landscaping");
            createDescription("valve restoration investigation");
            createDescription("valve restoration repair");
            createDescription("storm catch landscaping");
            createDescription("storm catch restoration investigation");
            createDescription("storm catch restoration repair");
            createDescription("rstrn restoration inquiry");
            createDescription("service off at main storm restoration");
            createDescription("service off at curb stop storm restoration");
            createDescription("service off at meter pit storm restoration");
            createDescription("valve turned off storm restoration");
            createDescription("main repair storm restoration");
            createDescription("main replace storm restoration");
            createDescription("hydrant turned off storm restoration");
            createDescription("hydrant replace storm restoration");
            createDescription("valve installation storm restoration");
            createDescription("valve replacement storm restoration");
            createDescription("curb box locate storm restoration");
            createDescription("meter pit locate storm restoration");
            createDescription("valve retirement storm restoration");
            createDescription("excavate meter pit  storm restoration");
            createDescription("service line renewal storm restoration");
            createDescription("curb box replacement storm restoration");
            createDescription("water main retirement storm restoration");
            createDescription("service line retirement storm restoration");
            createDescription("frame and cover replace storm restoration");
            createDescription("pump repair");
            createDescription("line stop repair");
            createDescription("saw repair");
            createDescription("vehicle repair");
            createDescription("misc repair");
            createDescription("z lwc ew4 3 consecutive mths of 0 usage zero");
            createDescription("z lwc ew4 check meter non emergency ckmtr");
            createDescription("z lwc ew4 demolition closed account democ");
            createDescription("z lwc ew4 meter change out mtrch");
            createDescription("z lwc ew4 read mr edit local ops only mredt");
            createDescription("z lwc ew4 read to stop estimate est");
            createDescription("z lwc ew4 repair install reading device rem");
            createDescription("z lwc ew4 reread and or inspect for leak hilow");
            createDescription("z lwc ew4 set meter turn on and read onset");
            createDescription("z lwc ew4 turn on water on");
            createDescription("hydrant nozzle replacement");
            createDescription("hydrant nozzle investigation");
            createDescription("crossing investigation");
            createDescription("service line installation partial");
            createDescription("service line installation complete partial");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("work description", nameValues);
        }

        [Given("markout requirements exist")]
        public static void GivenMarkoutRequirementsExist()
        {
            Action<string> createRequirement = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("markout requirement",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createRequirement("none");
            createRequirement("routine");
            createRequirement("emergency");
        }

        [Given("markout types exist")]
        public static void GivenMarkoutTypesExist()
        {
            Action<string> createType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("markout type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createType("c to c");
            createType("none");
        }

        [Given("meter supplemental locations exist")]
        public static void GivenMeterSupplementalLocationsExist()
        {
            Action<string> createMeterSupplementalLocation = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("meter supplemental location",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createMeterSupplementalLocation("Inside");
            createMeterSupplementalLocation("Outside");
            createMeterSupplementalLocation("Secure Access");
        }

        [Given("meter directions exist")]
        public static void GivenMeterDirectionsExist()
        {
            Action<string> createMeterDirection = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("meter direction",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createMeterDirection("Front");
            createMeterDirection("Left Side");
            createMeterDirection("Rear");
            createMeterDirection("Right");
            createMeterDirection("Unable to Verify");
        }

        [Given("order types exist")]
        public static void GivenOrderTypesExist()
        {
            Action<string> createObject = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("order type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createObject("operational activity");
            createObject("pm work order");
            createObject("corrective action");
            createObject("rp capital");
            createObject("routine");
        }

        [Given("small meter locations exist")]
        public static void GivenSmallMeterLocationsExist()
        {
            Action<string> createSmallMeterLocation = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("small meter location",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createSmallMeterLocation("Cellar / Basement");
            createSmallMeterLocation("Curb");
            createSmallMeterLocation("Utility Room");
        }

        [Given("equipment types exist")]
        public static void GivenEquipmentTypesExist()
        {
            Action<string> createEquipmentType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createEquipmentType("GENERATOR");
            createEquipmentType("ENGINE");
            createEquipmentType("AERATOR");
            createEquipmentType("Filter");
            createEquipmentType("RTU");
            createEquipmentType("FLOW METER");
            createEquipmentType("fire suppression");
            createEquipmentType("WELL");
            createEquipmentType("TNK");
            createEquipmentType("TNK-WPOT");
        }

        [Given(@"equipment categories exist")]
        public static void GivenEquipmentCategoriesExist()
        {
            Action<string> createEquipmentCategory = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment category",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createEquipmentCategory("HVAC");
            createEquipmentCategory("GENERAL EQUIPMENT");
            createEquipmentCategory("FLOW METER");
            createEquipmentCategory("SAFETY");
        }

        [Given("asset upload statuses exist")]
        public static void GivenAssetUploadStatusesExist()
        {
            Action<string> createAssetUploadStatus = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("asset upload status",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createAssetUploadStatus("Pending");
            createAssetUploadStatus("Success");
            createAssetUploadStatus("Error");
        }

        [Given("production prerequisites exist")]
        public static void GivenProductionPrerequisitesExist()
        {
            Action<string> createProductionPrerequisites = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("production prerequisite",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createProductionPrerequisites("has lockout requirement");
            createProductionPrerequisites("is confined space");
            createProductionPrerequisites("job safety list");
            createProductionPrerequisites("air permit");
            createProductionPrerequisites("hot work");
            createProductionPrerequisites("red tag permit");
            createProductionPrerequisites("pre job safety brief");
        }

        [Given("system delivery entry types exist with system delivery type:? \"([^\"]+)\"")]
        public static void GivenSystemDeliveryEntryTypesExist(string systemDeliveryType)
        {
            Action<string, string> createEntryType = (desc, systype) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("system delivery entry type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\", system delivery type: \"{systype}\"", TestObjectCache.Instance);

            var systemDelType = TestObjectCache.Instance.Lookup<SystemDeliveryType>("system delivery type", systemDeliveryType);
            switch (systemDelType.Id)
            {
                case SystemDeliveryType.Indices.WATER:
                    createEntryType("Purchased Water", "water");
                    createEntryType("Delivered Water", "water"); // Water only for now
                    createEntryType("Transferred To", "water");
                    createEntryType("Transferred From", "water");
                    break;
                case SystemDeliveryType.Indices.WASTE_WATER:
                    createEntryType("WasteWater collected", "waste water");
                    createEntryType("WasteWater treated", "waste water"); // Water only for now
                    createEntryType("Untreated Eff. Discharged", "waste water");
                    createEntryType("Treated Eff. Discharged", "waste water");
                    createEntryType("Treated Eff. Reused", "waste water");
                    createEntryType("Biochemical Oxygen Demand", "waste water");
                    break;
            }
        }

        [Given("equipment subcategories exist")]
        public static void GivenEquipmentSubcategoriesExist()
        {
            Action<string> createCategory = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment subcategory",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            // lets just set all of them up here
            createCategory("Air Pneumatics");
            createCategory("Back Flow Preventer");
            createCategory("Building Facilities");
            createCategory("Buildings & Grounds");
            createCategory("Chemical Feed");
            createCategory("Chemical Scale");
            createCategory("Chemical Storage Tank");
            createCategory("Chlorination");
            createCategory("Comminutor");
            createCategory("Computer Systems");
            createCategory("Electrical");
            createCategory("Emergency Power");
            createCategory("Filtration");
            createCategory("Fire Protection");
            createCategory("Flocculation - Clarification");
            createCategory("HVAC");
            createCategory("Instrumentation");
            createCategory("Laboratory Equipment");
            createCategory("Lift Equipment");
            createCategory("Lifting Equipment");
            createCategory("Mixer");
            createCategory("Personal Protective Equipment");
            createCategory("Piping");
            createCategory("Plumbing");
            createCategory("Pump");
            createCategory("Residual Processing");
            createCategory("Screens");
            createCategory("Structure");
            createCategory("Tools");
            createCategory("Transportation");
            createCategory("Valve");
            createCategory("Water Storage");
            createCategory("Well");
            createCategory("Safety");
            createCategory("Dam");
            createCategory("Delivered Water");
            createCategory("Purchased Water");
            createCategory("Transferred Water");
            createCategory("WasteWater");
        }

        [Given("short cycle work order safety brief location types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefLocationTypesExist()
        {
            Action<string> createLocationTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief location type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createLocationTypes("Meter Reading");
            createLocationTypes("FSR Work (Residence)");
            createLocationTypes("FSR Work (Businesses)");
            createLocationTypes("Vault(s)");
            createLocationTypes("Booster Station(s)");
        }

        [Given("short cycle work order safety brief hazard types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefHazardTypesExist()
        {
            Action<string> createHazardTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief hazard type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createHazardTypes("Slips,Trips,Falls");
            createHazardTypes("Cuts/Abrasions");
            createHazardTypes("Ergonomic/Lifting");
            createHazardTypes("Traffic");
            createHazardTypes("Tools");
            createHazardTypes("Electrical Grounding");
            createHazardTypes("Limited Workspace");
            createHazardTypes("Weather/Lighting");
            createHazardTypes("Heat/Cold Stress");
            createHazardTypes("Poisonous Plants");
            createHazardTypes("Animals/Insects");
            createHazardTypes("Confined Space");
            createHazardTypes("Ladder Safety");
            createHazardTypes("Pandemic Precaution");
            createHazardTypes("Other");
        }

        [Given("short cycle work order safety brief tool types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefToolTypesExist()
        {
            Action<string> createToolTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief tool type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createToolTypes("Hand Tools");
            createToolTypes("Pump/Vaccum");
            createToolTypes("Multimeter");
            createToolTypes("Air Monitor");
            createToolTypes("Tripod");
            createToolTypes("Meter Reading Equipment");
            createToolTypes("Other");
        }

        [Given("short cycle work order safety brief ppe types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefPPETypesExist()
        {
            Action<string> createPPETypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief PPE type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createPPETypes("Hardhat");
            createPPETypes("Hearing Protection");
            createPPETypes("Class III Apparel");
            createPPETypes("Harness");
            createPPETypes("Gloves");
            createPPETypes("Safety-Toe Shoes");
            createPPETypes("00 Electrical Gloves");
            createPPETypes("Other");
        }

        [Given("equipment manufacturers exist")]
        public static void GivenEquipmentManufacturersExist()
        {
            Action<string, string> createEquipmentManufacturer = (desc, type) =>
                 MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment manufacturer",
                     desc.ToLowerInvariant(),
                     $"description: \"{desc}\", equipment type: {type}, is active: true", TestObjectCache.Instance);
            createEquipmentManufacturer("fire suppression", "fire suppression");
            createEquipmentManufacturer("generator", "generator");
            createEquipmentManufacturer("engine", "engine");
            createEquipmentManufacturer("lowry", "aerator");
            createEquipmentManufacturer("unknown", "aerator");
            createEquipmentManufacturer("tnk", "tnk");
            createEquipmentManufacturer("other", "aerator");
            createEquipmentManufacturer("master meter", "flow meter");
        }

        [Given("compliance requirements exist")]
        public static void ComplianceRequirementsExist()
        {
            Action<string> createComplianceRequirement = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("compliance requirement",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"");

            createComplianceRequirement("Company");
            createComplianceRequirement("OSHA");
            createComplianceRequirement("PSM");
            createComplianceRequirement("Regulatory");
            createComplianceRequirement("TCPA");
        }

        [Given("equipment risk characteristics exist")]
        public static void GivenEquipmentRiskCharacteristicsExist()
        {
            Action<string, string> createThing = (thing, desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(thing,
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("equipment condition", "Poor");
            createThing("equipment condition", "Average");
            createThing("equipment condition", "Good");
            createThing("equipment performance rating", "Good");
            createThing("equipment performance rating", "Average");
            createThing("equipment performance rating", "Poor");
            createThing("equipment static dynamic type", "Static");
            createThing("equipment static dynamic type", "Dynamic");
            createThing("equipment consequences of failure rating", "Low");
            createThing("equipment consequences of failure rating", "Medium");
            createThing("equipment consequences of failure rating", "High");
            createThing("equipment likelyhood of failure rating", "Low");
            createThing("equipment likelyhood of failure rating", "Medium");
            createThing("equipment likelyhood of failure rating", "High");
            createThing("equipment reliability rating", "Low");
            createThing("equipment reliability rating", "Medium");
            createThing("equipment reliability rating", "High");
            createThing("equipment failure risk rating", "Low");
            createThing("equipment failure risk rating", "Medium");
            createThing("equipment failure risk rating", "High");
        }

        [Given("facility risk characteristics exist")]
        public static void GivenFacilityRiskCharacteristicsExist()
        {
            Action<string, string> createThing = (thing, desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(thing,
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("facility consequence of failure", "Low");
            createThing("facility consequence of failure", "Medium");
            createThing("facility consequence of failure", "High");
        }

        [Given("facility likelihood of failures exist")]
        public static void GivenFacilityLikelihoodOfFailuresExist()
        {
            Action<string, string> createThing = (thing, desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(thing,
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("facility likelihood of failure", "Low");
            createThing("facility likelihood of failure", "Medium");
            createThing("facility likelihood of failure", "High");
        }

        [Given("equipment failure risk ratings exist")]
        public static void GivenEquipmentFailureRiskRatingsExist()
        {
            Action<string, string> createThing = (thing, desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(thing,
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("equipment failure risk rating", "Low");
            createThing("equipment failure risk rating", "Medium");
            createThing("equipment failure risk rating", "High");
        }

        [Given("maintenance risk of failures exist")]
        public static void GivenMaintenanceRiskOfFailuresExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("facility maintenance risk of failure",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createThing("low");
            createThing("lowmod");
            createThing("mod");
            createThing("modhigh");
            createThing("high");
            createThing("critical");
        }

        [Given("facility asset management maintenance strategy tiers exist")]
        public static void GivenFacilityAssetManagementMaintenanceStategyTiersExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("facility asset management maintenance strategy tier",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("one");
            createThing("two");
            createThing("three");
        }

        [Given("environmental non compliance event statuses exist")]
        public static void GivenEnvironmentalNonComplianceEventStatusesExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event status", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Pending");
            createThing("Confirmed");
            createThing("Rescinded");
        }

        [Given("environmental non compliance event types exist")]
        public static void GivenEnvironmentalNonComplianceEventTypesExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event type", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Drinking Water NOV");
            createThing("Water (Non DW) NOV");
            createThing("Wastewater NOV");
            createThing("Environmental");
        }

        [Given("environmental non compliance event entity levels exist")]
        public static void GivenEnvironmentalNonComplianceEventEntityLevelsExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event entity level", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("EPA");
            createThing("State");
            createThing("County");
            createThing("OSHA");
            createThing("Other");
        }

        [Given("environmental non compliance event responsibilities exist")]
        public static void GivenEnvironmentalNonComplianceEventResponsibilitiesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event responsibility",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("American Water");
            createThing("Third Party NOV");
            createThing("New Acquisition NOV");
            createThing("TBD");
            createThing("Administrative NOV");
            createThing("Consent Agreement NOV");
            createThing("Erroneous NOV");
            createThing("External Inspection with NOV");
            createThing("Long-Term Recurrence (LRAA)");
            createThing("Not Applicable");
        }

        [Given("environmental non compliance event action item types exist")]
        public static void GivenEnvironmentalNonComplianceEventActionItemTypesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event action item type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Capital Improvement");
            createThing("Contractor Outreach");
            createThing("Education");
            createThing("Not Listed");
            createThing("PM Plan Creation/Modify");
            createThing("SOP Creation/Modify");
            createThing("Tap Root Analysis");
        }

        [Given("environmental non compliance event root causes exist")]
        public static void GivenEnvironmentalNonComplianceEvenRootCausesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event root cause",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Contractor adherence to regulations");
            createThing("Failure of equipment");
            createThing("Failure to follow SOP");
            createThing("Lack of adequate oversight");
            createThing("Lack of adequate tracking system");
            createThing("Lack of Communication");
            createThing("Lack of Education \\ Knowledge");
            createThing("Lack of SOP");
            createThing("TBD");
        }
        
        [Given("environmental non compliance event counts against targets exist")]
        public static void GivenEnvironmentalNonComplianceCountsAgainstTargetsExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event counts against target", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Expected to Count");
            createThing("Not Expected to Count");
            createThing("Yes");
            createThing("No");
        }

        [Given("end of pipe exceedance types exist")]
        public static void GivenEndOfPipeExceedanceTypesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("end of pipe exceedance type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("BOD/CBOD");
            createThing("Fecal/E Coli/Total Coliform");
            createThing("Plant Flow");
            createThing("Nitrate/Total N");
            createThing("Ammonia");
            createThing("Phosphorous");
            createThing("pH");
            createThing("TSS/TDS");
            createThing("Dissolved Oxygen");
            createThing("Other");
        }

        [Given("end of pipe exceedance root causes exist")]
        public static void GivenEndOfPipeExceedanceRootCausesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("end of pipe exceedance root cause",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Power failure");
            createThing("Mechanical failure");
            createThing("Treatment disruption");
            createThing("Treatment limitation");
            createThing("Plant capacity");
            createThing("Weather event");
            createThing("Unknown");
            createThing("Other");
        }

        [Given("task group categories exist")]
        public static void GivenTaskGroupCategoriesExist()
        {
            Action<string> createThing = tng =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("task group category",
                    tng.ToLowerInvariant(),
                    $"task group name: \"{tng}\"", TestObjectCache.Instance);

            createThing("Chemical");
            createThing("Electrical");
            createThing("Mechanical");
            createThing("Safety");
        }

        [Given("covid answer types exist")]
        public static void GivenCovidAnswerTypesExist()
        {
            Action<string> createCovidAnswerTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("covid answer type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createCovidAnswerTypes("Yes");
            createCovidAnswerTypes("No");
            createCovidAnswerTypes("TBD");
            createCovidAnswerTypes("Contact Tracer Must Complete");
        }

        [Given("asset reliability technology used types exist")]
        public static void GivenAssetReliabilityTechnologyUsedTypesExist()
        {
            Action<string> createAssetReliabilityTechnologyUsedTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("asset reliability technology used type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createAssetReliabilityTechnologyUsedTypes("Infrared Thermography");
            createAssetReliabilityTechnologyUsedTypes("Vibration Analysis");
            createAssetReliabilityTechnologyUsedTypes("Motor Winding Analysis/Insulation Resistance");
            createAssetReliabilityTechnologyUsedTypes("Visual Inspection");
            createAssetReliabilityTechnologyUsedTypes("Airborne Ultrasound");
            createAssetReliabilityTechnologyUsedTypes("Laser Alignment");
            createAssetReliabilityTechnologyUsedTypes("Earth Ground Testing");
            createAssetReliabilityTechnologyUsedTypes("Electrical Testing");
            createAssetReliabilityTechnologyUsedTypes("Wire to Water/Pump Performance");
            createAssetReliabilityTechnologyUsedTypes("Motion Amplification");
            createAssetReliabilityTechnologyUsedTypes("Protective Relay Testing");
            createAssetReliabilityTechnologyUsedTypes("Battery Testing");
            createAssetReliabilityTechnologyUsedTypes("Other");
            createAssetReliabilityTechnologyUsedTypes("Dynamic Motor Testing/ESA");
            createAssetReliabilityTechnologyUsedTypes("Micro-Ohmmeter Testing");
        }

        [Given("workers compensation claim statuses exist")]
        public static void GivenWorkersCompensationClaimStatusesExist()
        {
            Action<string> CreateWorkersCompensationClaimStatuses = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("workers compensation claim status",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            CreateWorkersCompensationClaimStatuses("No Claim Created");
            CreateWorkersCompensationClaimStatuses("Open");
            CreateWorkersCompensationClaimStatuses("Closed - Denied (Not Compensable)");
            CreateWorkersCompensationClaimStatuses("Closed - Accepted (Compensable)");
        }

        [Given("service categories exist")]
        public static void ServiceCategoriesExist()
        {
            Action<string> createServiceCategory = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("service category",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"");

            createServiceCategory("Fire Retire Service Only");
            createServiceCategory("Fire Service Installation");
            createServiceCategory("Fire Service Renewal");
            createServiceCategory("Install Meter Set");
            createServiceCategory("Irrigation New");
            createServiceCategory("Irrigation Renewal");
            createServiceCategory("Replace Meter Set");
            createServiceCategory("Sewer Measurement Only");
            createServiceCategory("Sewer Reconnect");
            createServiceCategory("Sewer Retire Service Only");
            createServiceCategory("Sewer Service Increase Size");
            createServiceCategory("Sewer Service New");
            createServiceCategory("Sewer Service Renewal");
            createServiceCategory("Sewer Service Split");
            createServiceCategory("Water Measurement Only");
            createServiceCategory("Water Reconnect");
            createServiceCategory("Water Relocate Meter Set");
            createServiceCategory("Water Retire Meter Set Only");
            createServiceCategory("Water Retire Service Only");
            createServiceCategory("Water Service Increase Size");
            createServiceCategory("Water Service New Commercial");
            createServiceCategory("Water Service New Domestic");
            createServiceCategory("Water Service Renewal");
            createServiceCategory("Water Service Split");
            createServiceCategory("Sewer Install Clean Out");
            createServiceCategory("Sewer Replace Clean Out");
            createServiceCategory("Water Service Renewal Cust Side");
            createServiceCategory("Water Commercial Record Import");
            createServiceCategory("Water Domestic Record Import");
            createServiceCategory("Fire Service Record Import");
            createServiceCategory("Irrigation Service Record Import");
            createServiceCategory("Fire Service Reconnect");
        }

        [Given("service installation purposes exist")]
        public static void ServiceInstallationPurposesExist()
        {
            Action<string> createServiceInstallationPurpose = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("service installation purpose",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"");

            createServiceInstallationPurpose("Standard Renewal");
            createServiceInstallationPurpose("Make Exterior Setting");
            createServiceInstallationPurpose("New Service");
            createServiceInstallationPurpose("Main Replacement");
            createServiceInstallationPurpose("Service Line Leak");
            createServiceInstallationPurpose("Measurement Only");
            createServiceInstallationPurpose("Retirement Only");
            createServiceInstallationPurpose("Main Extension");
            createServiceInstallationPurpose("Customer Request");
            createServiceInstallationPurpose("Sample Site");
            createServiceInstallationPurpose("Main Cleaning and Lining");
            createServiceInstallationPurpose("Material Verification");
        }

        [Given("sample site collection types exist")]
        public static void GivenSampleSiteCollectionTypesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createSampleSiteCollectionType = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createSampleSiteCollectionType("raw");
            createSampleSiteCollectionType("in plant");
            createSampleSiteCollectionType("entry point");
            createSampleSiteCollectionType("interconnect");
            createSampleSiteCollectionType("distribution");
            createSampleSiteCollectionType("wastewater");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("sample site collection type", nameValues);
        }
        
        [Given("sample site lead copper tier classifications exist")]
        public static void GivenSampleSiteLeadCopperTierClassificationsExist()
        {
            Action<string> Create = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(
                    "sample site lead copper tier classification",
                    desc.ToLowerInvariant(), 
                    $"description: \"{desc}\"");

            Create("Tier One");
            Create("Tier Two");
            Create("Tier Three");
            Create("Other");
        }

        [Given("sample site lead copper validation methods exist")]
        public static void GivenSampleSiteLeadCopperValidationMethodsExist()
        {
            Action<string> Create = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(
                    "sample site lead copper validation method",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"");

            Create("Visual Confirmation");
            Create("Lead Swab Test");
            Create("Building Construction Document");
            Create("Customer Survey Results");
            Create("Historic Documentation");
        }

        [Given("sample site customer contact methods exist")]
        public static void GivenSampleSiteCustomerContactMethodsExist()
        {
            Action<string> Create = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(
                    "sample site customer contact method",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"");

            Create("Email");
            Create("Mail");
            Create("Phone");
            Create("Text");
        }

        [Given("acoustic monitoring exist")]
        public static void GivenAcousticMonitoringExist()
        {
            Action<string> createAcousticMonitoring = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("acoustic monitoring",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createAcousticMonitoring("smart cover");
        }

        [Given("miu install reason codes exist")]
        public static void GivenMiuInstallReasonCodesExist()
        {
            Action<string> createMiuInstallReasonCode = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("miu install reason code",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createMiuInstallReasonCode("new install ami");
            createMiuInstallReasonCode(@"New Install RF\AMR\Touchpad");
        }

        [Given("arc flash statuses exist")]
        public static void GivenArcFlashStatusesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createArcFlashStatus = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createArcFlashStatus("completed");
            createArcFlashStatus("pending");
            createArcFlashStatus("deferred");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("arc flash status", nameValues);
        }

        [Given("npdes regulator inspection form answer types exist")]
        public static void GivenGateStatusAnswerTypesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createGateStatusAnswerType = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createGateStatusAnswerType("yes");
            createGateStatusAnswerType("no");
            createGateStatusAnswerType("n/a");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("npdes regulator inspection form answer type", nameValues);
        }

        [Given("as left conditions exist")]
        public static void GivenAsLeftConditionsExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createAsLeftConditions = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createAsLeftConditions("Unable to Inspect");
            createAsLeftConditions("Needs Emergency Repair");
            createAsLeftConditions("Needs Repair");
            createAsLeftConditions("Needs Re-Inspection");
            createAsLeftConditions("Needs Re-Inspection Sooner than Normal");
            createAsLeftConditions("Acceptable / Good");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("as left condition", nameValues);
        }

        [Given("chemical storage locations exist")]
        public static void GivenChemicalStorageLocationsExist()
        {
            Action<string> createChemicalStorageLocations = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("chemical storage location",
                    desc.ToLowerInvariant(),
                    $"storage location description: \"{desc}\"", TestObjectCache.Instance);

            createChemicalStorageLocations("Farmer_Lock #1");
            createChemicalStorageLocations("Farmer_Lock #2");
            createChemicalStorageLocations("Rey_Lock #1");
            createChemicalStorageLocations("Rey_Lock #2");
        }

        #endregion

        #region Event-Driven Functionality

        // ReSharper disable ReturnTypeCanBeEnumerable.Local
        // ReSharper wants an IEnumerable<string> but that will blow up in Deleporter
        // because it's all interfacey and stuff.
        private static string[] GetIgnorableCustomerProfiles()
        // ReSharper restore ReturnTypeCanBeEnumerable.Local
        {
            if (_ignorableCustomerProfileIds == null)
            {
                var ignorable = new List<string>();
                var sb = new StringBuilder();
                Console.WriteLine("Here's a list of profiles that will be ignored through out the rest of the regression tests!");

                Deleporter.Run(() => {
                    // Testing this as a temporary fix
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var gateway = DependencyResolver.Current.GetService<IExtendedCustomerGateway>();
                    // This loop is kinda slow. There's no method to just get all the customers, so you have
                    // to contact the authorize.net API for each customer just to get their email address.
                    // This takes roughly 20 seconds to run.
                    // So why we do we need to do this in the first place? Because CreateUser is creating
                    // a customer profile. By the time this method is called, we have no way of knowing
                    // that profile was created. 
                    gateway.GetCustomerIDs().Each(id =>
                    {
                        var sw = Stopwatch.StartNew();

                        var customer = gateway.GetCustomer(id);
                        if (customer.Email?.StartsWith(Environment.MachineName.ToLowerInvariant()) != true)
                        {
                            ignorable.Add(id);
                        }

                        sw.Stop();
                        sb.AppendFormat("id: {0}, email: {1}, {2}ms, ticks: {3}", id, customer.Email, sw.ElapsedMilliseconds, sw.ElapsedTicks);
                        sb.AppendLine();
                    });
                });

                Console.WriteLine(sb.ToString());
                Console.WriteLine("");

                _ignorableCustomerProfileIds = ignorable.ToArray();
            }

            return _ignorableCustomerProfileIds;
        }

        /// <summary>
        /// Delete any test users that have been created at Authorize.Net.
        /// </summary>
        [BeforeScenario("cleanup_users")]
        public void BeforeScenario_CleanupUsers()
        {
            // "Why are we running this both before and after a scenario?"
            // So we can clean up any customers that were created but were
            // not deleted due to a test failing or something that otherwise
            // prevented the AfterScenario from running.
            var ignorable = GetIgnorableCustomerProfiles();
            Deleporter.Run(() => {
                var gateway = DependencyResolver.Current.GetService<IExtendedCustomerGateway>();
                gateway.GetCustomerIDs().Except(ignorable).Each(id =>
                {
                    var customer = gateway.GetCustomer(id);
                    if (customer.Email.StartsWith(Environment.MachineName.ToLowerInvariant()))
                    {
                        gateway.DeleteCustomer(customer.ProfileID);
                    }
                });
            });
        }

        [AfterScenario("cleanup_users")]
        public void AfterScenario_CleanupUsers()
        {
            var ignorable = GetIgnorableCustomerProfiles();
            var noEmailUserIds = new List<int>();
            Deleporter.Run(() => {
                var gateway = DependencyResolver.Current.GetService<IExtendedCustomerGateway>();
                gateway.GetCustomerIDs().Except(ignorable).Each(id =>
                {
                    var customer = gateway.GetCustomer(id);
                    if (String.IsNullOrWhiteSpace(customer.Email))
                    {
                        // As far as I know, the only reason we don't just automatically delete these
                        // is because the customer may have been created outside of the test. The sandbox
                        // site exists for QA and is also used by the Permits API site, so we don't
                        // have a lot of control over that data. If you know for certain that the customer
                        // profile is one that was created by a test, you can uncomment the line
                        // below and run the test again. It'll still fail, but the next run should pass.
                        // Be sure to comment the line out again! -Ross 9/6/2023

                        //gateway.DeleteCustomer(customer.ProfileID);
                        noEmailUserIds.Add(Int32.Parse(id));
                    }
                });
            });

            if (noEmailUserIds.Count > 0)
            {
                Assert.Fail(
                    "This test created one or more customer profiles with no email address!  These will need to be deleted manually from the sandbox.authorize.net site. Or you can uncomment out the DeleteCustomer line. The offending customer id(s) is/are {0}",
                    string.Join(", ", noEmailUserIds));
            }
        }

        [BeforeScenario]
        public static void CreateAndSpoofTestUser()
        {
            var objectCache = TestObjectCache.Instance;
            
            Deleporter.Run(() => {
                var container = DependencyResolver.Current.GetService<IContainer>();
                const string opCode = "!@#$"; 

                var opCenter = CreateOperatingCenter(
                    new NameValueCollection { { "opcode", opCode } },
                    objectCache,
                    container);

                objectCache.EnsureDictionary("operating center").Add(opCode, opCenter);

                // we need a user to inject into the change tracking interceptor so that any records created
                // from feature steps which track CreatedBy or UpdatedBy don't error 
                CreateUser(
                    new NameValueCollection {
                        { "username", CHANGE_TRACKING_USER_NAME },
                        { "default operating center", opCode }
                    },
                    objectCache,
                    container,
                    true);
            });
        }

        #endregion

        #region Object Creation

        private static object CreateActionItem(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var ignoreKeys = new[] { "note", "data type" };

            var linkedItemKey = nvc.AllKeys.Where(k => !ignoreKeys.Contains(k)).First();
            var linkedItem = (IThingWithActionItems)cache.GetOrNull(linkedItemKey, nvc);

            var args = new
            {
                Note = nvc.GetValueOrDefault("note"),
                DataType = cache.GetValueOrDefault<DataTypeFactory>("data type", nvc),
                LinkedId = linkedItem.Id
            };

            return container.GetInstance<ActionItemFactory>().Create(args);
        }

        private static object CreateTownDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var town = (Town)cache.GetValueOrDefault<TownFactory>("town", nvc);
            return CreateDocumentLink(nvc, cache, container, town.Id);
        }

        private static object CreateHelpTopicDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var helpTopic = (HelpTopic)cache.GetValueOrDefault<HelpTopicFactory>("help topic", nvc);
            return CreateDocumentLink(nvc, cache, container, helpTopic.Id);
        }

        private static object CreateDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container,
            int linkedId)
        {
            var document = (Document)cache.GetValueOrDefault<DocumentFactory>("document", nvc);
            var status = cache.GetValueOrDefault<DocumentStatusFactory>("document status", nvc);
            RecurringFrequencyUnit recurringFrequencyUnit = null;
            if (nvc["recurring frequency unit"] != null)
            {
                switch (nvc["recurring frequency unit"].ToUpper())
                {
                    case "YEAR":
                        recurringFrequencyUnit = container.GetInstance<YearlyRecurringFrequencyUnitFactory>().Create();
                        break;
                    case "MONTH":
                        recurringFrequencyUnit = container.GetInstance<MonthlyRecurringFrequencyUnitFactory>().Create();
                        break;
                    case "WEEK":
                        recurringFrequencyUnit = container.GetInstance<WeeklyRecurringFrequencyUnitFactory>().Create();
                        break;
                    default:
                        recurringFrequencyUnit = container.GetInstance<DailyRecurringFrequencyUnitFactory>().Create();
                        break;
                }
            }

            var overrides = new {
                document.DocumentType,
                document.DocumentType.DataType,
                Document = document,
                LinkedId = linkedId,
                DocumentStatus = status,
                ReviewFrequency = nvc.GetValueAs<int>("review frequency"),
                ReviewFrequencyUnit = recurringFrequencyUnit,
            };
            return container.GetInstance<DocumentLinkFactory>().Create(overrides);
        }

        private static object CreateEquipmentDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var document = (Document)cache.GetValueOrDefault<DocumentFactory>("document", nvc);
            var equipment = (Equipment)cache.GetValueOrDefault<EquipmentFactory>("equipment", nvc);
            var documentType = (DocumentType)cache.GetValueOrDefault<DocumentTypeFactory>("document type", nvc);
            var dataType = (DataType)cache.GetValueOrDefault<DataTypeFactory>("data type", nvc);

            return container.GetInstance<DocumentLinkFactory>().Create(new
            {
                DocumentType = documentType,
                DataType = dataType,
                Document = document,
                LinkedId = equipment.Id
            });
        }

        public static FireDistrictTown CreateFireDistrictTown(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var fireDistrict = (FireDistrict)cache.GetValueOrDefault<FireDistrictFactory>("fire district", nvc);
            var town = (Town)cache.GetValueOrDefault<TownFactory>("town", nvc);

            return container.GetInstance<FireDistrictTownFactory>().Create(new
            {
                Town = town,
                FireDistrict = fireDistrict
            });
        }

        public static SystemDeliveryType CreateSystemDeliveryType(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "water":
                    return container.GetInstance<SystemDeliveryTypeWaterFactory>().Create();
                case "waste water":
                    return container.GetInstance<SystemDeliveryTypeWasteWaterFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create system delivery type with description '{nvc["description"]}'.");
            }
        }

        public static SampleSitePointOfUseTreatmentType CreateSampleSitePointOfUseTreatmentType(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "none":
                    return container.GetInstance<NoneSampleSitePointOfUseTreatmentTypeFactory>().Create();
                case "entire building":
                    return container.GetInstance<EntireBuildingSampleSitePointOfUseTreatmentTypeFactory>().Create();
                case "individual taps":
                    return container.GetInstance<IndividualTapsSampleSitePointOfUseTreatmentTypeFactory>().Create();
                case "faucet filter":
                    return container.GetInstance<FaucetFilterSampleSitePointOfUseTreatmentTypeFactory>().Create();
                case "water softener":
                    return container.GetInstance<WaterSoftenerSampleSitePointOfUseTreatmentTypeFactory>().Create();
                case "whole home filter":
                    return container.GetInstance<WholeHomeFilterSampleSitePointOfUseTreatmentTypeFactory>().Create();
                case "other":
                    return container.GetInstance<OtherSampleSitePointOfUseTreatmentTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create sample site point of use treatment type with description '{nvc["description"]}'.");
            }
        }

        public static ABCIndicator CreateABCIndicator(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ABCIndicatorFactory>().Create();
        }

        private static object CreateWorkersCompensationClaimStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            WorkersCompensationClaimStatusFactory factory;

            switch (nvc["description"])
            {
                case "No Claim Created":
                    factory = container.GetInstance<NoClaimWorkersCompensationClaimStatusFactory>();
                    break;
                case "Open":
                    factory = container.GetInstance<OpenWorkersCompensationClaimStatusFactory>();
                    break;
                case "Closed - Denied (Not Compensable)":
                    factory = container.GetInstance<ClosedDeniedWorkersCompensationClaimStatusFactory>();
                    break;
                case "Closed - Accepted (Compensable)":
                    factory = container.GetInstance<ClosedAcceptedWorkersCompensationClaimStatusFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Unable to create workers compensation claim status with description '{nvc["description"]}'");
            }

            return factory.Create();
        }

        private static object CreateAssetReliabilityTechnologyUsedType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            AssetReliabilityTechnologyUsedTypeFactory factory;

            switch (nvc["description"])
            {
                case "Infrared Thermography":
                    factory = container.GetInstance<InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Vibration Analysis":
                    factory = container.GetInstance<VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Motor Winding Analysis/Insulation Resistance":
                    factory = container.GetInstance<MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Visual Inspection":
                    factory = container.GetInstance<VisualInspectionAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Airborne Ultrasound":
                    factory = container.GetInstance<AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Laser Alignment":
                    factory = container.GetInstance<LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Earth Ground Testing":
                    factory = container.GetInstance<EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Electrical Testing":
                    factory = container.GetInstance<ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Wire to Water/Pump Performance":
                    factory = container.GetInstance<WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Motion Amplification":
                    factory = container.GetInstance<MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Protective Relay Testing":
                    factory = container.GetInstance<ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Other":
                    factory = container.GetInstance<OtherAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Battery Testing":
                    factory = container.GetInstance<BatteryTestingAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Dynamic Motor Testing/ESA":
                    factory = container.GetInstance<DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                case "Micro-Ohmmeter Testing":
                    factory = container.GetInstance<MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create asset reliability technology used type from description string '{nvc["description"]}'");
            }

            return factory.Create();
        }

        //{"equipment asset type", typeof(AssetType), CreateEquipmentAssetType},
        public static AssetType CreateAssetType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "valve":
                    return container.GetInstance<ValveAssetTypeFactory>().Create();
                case "hydrant":
                    return container.GetInstance<HydrantAssetTypeFactory>().Create();
                case "main":
                    return container.GetInstance<MainAssetTypeFactory>().Create();
                case "service":
                    return container.GetInstance<ServiceAssetTypeFactory>().Create();
                case "sewer opening":
                    return container.GetInstance<SewerOpeningAssetTypeFactory>().Create();
                case "sewer lateral":
                    return container.GetInstance<SewerLateralAssetTypeFactory>().Create();
                case "sewer main":
                    return container.GetInstance<SewerMainAssetTypeFactory>().Create();
                case "storm catch":
                    return container.GetInstance<StormCatchAssetTypeFactory>().Create();
                case "equipment":
                case "":
                    return container.GetInstance<EquipmentAssetTypeFactory>().Create();
                case "facility":
                    return container.GetInstance<FacilityAssetTypeFactory>().Create();
                case "main crossing":
                    return container.GetInstance<MainCrossingAssetTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create asset type from description string '{nvc["description"]}'");
            }
        }

        private static object CreateBacterialSampleType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            BacterialSampleTypeFactory factory;

            switch (nvc["description"])
            {
                case "Confirmation":
                    factory = container.GetInstance<ConfirmationBacterialSampleTypeFactory>();
                    break;
                case "New Main":
                    factory = container.GetInstance<NewMainBacterialSampleTypeFactory>();
                    break;
                case "Process Control":
                    factory = container.GetInstance<ProcessControlBacterialSampleTypeFactory>();
                    break;
                case "Repeat":
                    factory = container.GetInstance<RepeatBacterialSampleTypeFactory>();
                    break;
                case "Routine":
                    factory = container.GetInstance<RoutineBacterialSampleTypeFactory>();
                    break;
                case "Shipping Blank":
                    factory = container.GetInstance<ShippingBlankBacterialSampleTypeFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating bacterial sample type for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        public static Board CreateBoard(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<BoardFactory>().Create(new
            {
                Name = nvc["name"],
                Site = objectCache.GetValueOrDefault<SiteFactory>("site", nvc)
            });
        }

        public static CommunicationType CreateCommunicationType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "letter":
                    return container.GetInstance<LetterCommunicationTypeFactory>().Create();
                case "electronic":
                    return container.GetInstance<ElectronicCommunicationTypeFactory>().Create();
                case "upload":
                    return container.GetInstance<UploadCommunicationTypeFactory>().Create();
                case "email":
                    return container.GetInstance<EmailCommunicationTypeFactory>().Create();
                case "pdf":
                    return container.GetInstance<PdfCommunicationTypeFactory>().Create();
                case "agency submittal form":
                    return container.GetInstance<AgencySubmittalFormCommunicationTypeFactory>().Create();
                case "other":
                    return container.GetInstance<OtherCommunicationTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create communication type with description '{nvc["description"]}'");
            }
        }
        
        public static ConsolidatedCustomerSideMaterial CreateConsolidatedCustomerSideMaterial(NameValueCollection nvc,
            TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ConsolidatedCustomerSideMaterialFactory>().Create(new {
                ConsolidatedEPACode = (nvc["consolidated e p a code"] == "")
                    ? objectCache.Lookup<EPACode>("e p a code", "lead status unknown")
                    : objectCache.Lookup<EPACode>("e p a code", nvc["consolidated e p a code"]),
                CustomerSideEPACode = (nvc["customer side e p a code"] == "")
                    ? objectCache.Lookup<EPACode>("e p a code", "lead status unknown")
                    : objectCache.Lookup<EPACode>("e p a code", nvc["customer side e p a code"]),
                CustomerSideExternalEPACode = (nvc["customer side external e p a code"] == "")
                    ? objectCache.Lookup<EPACode>("e p a code", "lead status unknown")
                    : objectCache.Lookup<EPACode>("e p a code", nvc["customer side external e p a code"])
            });
        }

        public static Contact CreateContactWithoutAddress(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ContactWithoutAddressFactory>().Create(new
            {
                FirstName = nvc["firstname"],
                LastName = nvc["lastname"] ?? "",
                Email = nvc["email"],
                BusinessPhoneNumber = nvc["businessphone"],
                HomePhoneNumber = nvc["homephone"],
                MobilePhoneNumber = nvc["mobile"],
                FaxNumber = nvc["fax"]
            });
        }

        public static Contractor CreateContractor(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<ContractorFactory>();
            var session = container.GetInstance<ISession>();

            Contractor contractor;

            if (nvc.ContainsKey("name"))
            {
                contractor = factory.Build(new {
                    Name = nvc["name"],
                    CreatedBy = nvc["name"],
                    AWR = nvc.GetValueAs<bool>("awr").GetValueOrDefault(false),
                    IsActive = nvc.GetValueAs<bool>("is active").GetValueOrDefault(false)
                });
            }
            else
            {
                contractor = factory.Build(new {
                    AWR = nvc.GetValueAs<bool>("awr").GetValueOrDefault(false),
                    IsActive = nvc.GetValueAs<bool>("is active").GetValueOrDefault(false)
                });
            }

            if (!String.IsNullOrWhiteSpace(nvc["operating center"]))
            {
                var operatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"]);
                operatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                contractor.OperatingCenters.Add(operatingCenter);
            }

            if (!String.IsNullOrWhiteSpace(nvc["framework operating center"]))
            {
                var operatingCenter = objectCache.Lookup<OperatingCenter>("operating center",
                    nvc["framework operating center"]);
                operatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                contractor.FrameworkOperatingCenters.Add(operatingCenter);
            }

            session.Save(contractor);
            session.Flush();
            return contractor;
        }

        public static object CreateContractorLaborCost(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var factory = container.GetInstance<ContractorLaborCostFactory>();
            var session = container.GetInstance<ISession>();

            var laborCost = factory.Build();

            if (!String.IsNullOrWhiteSpace(nvc["operating centers"]))
            {
                foreach (var opCenter in nvc["operating centers"].Split(new[] { ',' }))
                {
                    var operatingCenter = cache.Lookup<OperatingCenter>("operating center", opCenter);
                    operatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                    laborCost.OperatingCenters.Add(operatingCenter);
                }
            }

            session.Save(laborCost);

            foreach (var opCenter in laborCost.OperatingCenters)
            {
                opCenter.ContractorLaborCosts.Add(laborCost);
                session.Save(opCenter);
            }

            return laborCost;
        }

        public static CorrectiveOrderProblemCode CreateCorrectiveOrderProblemCode(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<CorrectiveOrderProblemCodeFactory>();
            var model = factory.Create(new {
                Code = nvc["code"],
                Description = nvc["description"]
            });

            return model;
        }

        public static CrashType CreateCrashType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "rear-end":
                    return container.GetInstance<RearEndCrashTypeFactory>().Create();
                case "sideswipe":
                    return container.GetInstance<SideswipeCrashTypeFactory>().Create();
                case "frontal":
                    return container.GetInstance<FrontalCrashTypeFactory>().Create();
                case "side":
                    return container.GetInstance<SideCrashTypeFactory>().Create();
                case "other":
                    return container.GetInstance<OtherCrashTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create crash type from description string '{nvc["description"]}'");
            }
        }

        public static CrewAssignment CreateCrewAssignment(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<CrewAssignmentFactory>();
            var model = factory.Create(new {
                WorkOrder = obj.GetValueOrDefault<WorkOrderFactory>("work order", nvc),
                Crew = obj.GetValueOrDefault<CrewFactory>("crew", nvc),
                EmployeesOnJob = nvc.GetValueAs<float>("employees on job"),
                AssignedFor = nvc.ContainsKey("assigned for") ? nvc.GetValueAs<DateTime>("assigned for") : DateTime.Now,
                AssignedOn = nvc.ContainsKey("assigned on") ? nvc.GetValueAs<DateTime>("assigned on") : DateTime.Now,
                DateStarted = nvc.ContainsKey("date started") ? nvc.GetValueAs<DateTime>("date started") : null,
                DateEnded = nvc.ContainsKey("date ended") ? nvc.GetValueAs<DateTime>("date ended") : null,
                Priority = nvc.ContainsKey("priority") ? nvc.GetValueAs<int>("priority") : CrewAssignmentFactory.DEFAULT_PRIORITY
            });

            return model;
        }

        public static CustomerCoordinate CreateCustomerCoordinate(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // need to build and save separately, rather than create, to prevent extra customer locations from being created
            var coordinate = container.GetInstance<CustomerCoordinateFactory>().Build(new
            {
                Latitude = nvc.GetValueAs<float>("latitude"),
                Longitude = nvc.GetValueAs<float>("longitude"),
                CustomerLocation = objectCache.Lookup("customer location", nvc["customer location"]),
                Source = Enum.Parse(typeof(CoordinateSource), nvc["source"])
            });

            container.GetInstance<ISession>().Save(coordinate);

            return coordinate;
        }

        public static CutoffSawQuestionnaire CreateCutoffSawQuestionnaire(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<CutoffSawQuestionnaireFactory>().Create(new
            {
                LeadPerson = obj.GetValueOrDefault<EmployeeFactory>("employee", "lead person", nvc),
                SawOperator = obj.GetValueOrDefault<EmployeeFactory>("employee", "saw operator", nvc)
            });
        }

        public static DataTableLayout CreateDataTableLayout(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<DataTableLayoutFactory>();

            var model = factory.Create(new
            {
                Area = nvc["area"],
                Controller = nvc["controller"],
                LayoutName = nvc["layout name"]
            });

            var propertyNames = nvc["properties"];
            if (string.IsNullOrWhiteSpace(propertyNames))
            {
                throw new NotSupportedException("You need to supply comma separated property names.");
            }

            var propFactory = container.GetInstance<TestDataFactory<DataTableLayoutProperty>>();
            foreach (var propName in propertyNames.Split(','))
            {
                var dtlProp = propFactory.Create(new {
                    DataTableLayout = model,
                    PropertyName = propName
                });
                model.Properties.Add(dtlProp);
            }

            return model;
        }
        
        [Given("discharge weather related types exist")]
        public static void GivenDischargeWeatherRelatedTypesExist()
        {
            Action<string> createObject = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("discharge weather related type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
        
            createObject("dry");
            createObject("wet");
        }
        
        private static object CreateDischargeWeatherRelatedType(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("Please enter a description to create the discharge weather related type");
            }
        
            switch (nvc["description"])
            {
                case "dry":
                    return container.GetInstance<DryDischargeWeatherRelatedTypeFactory>().Create();
                case "wet":
                    return container.GetInstance<WetDischargeWeatherRelatedTypeFactory>().Create();
            
                default:
                    throw new InvalidOperationException(
                        $"Unable to create discharge weather related type with description '{nvc["description"]}'.");
            }
        }

        public static Division CreateDivision(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<DivisionFactory>();

            var model = factory.Create(new {
                Description = nvc["description"]
            });

            return model;
        }

        // things fail without this one, not sure why
        public static object CreateDocument(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var fileName = nvc["file name"];
            var documentType = obj.GetValueOrDefault<DocumentTypeFactory>("document type", nvc);

            return container.GetInstance<DocumentFactory>().Create(new
            {
                FileName = fileName,
                DocumentType = documentType
            });
        }

        public static DocumentType CreateDocumentType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var dataType = obj.GetValueOrDefault<DataTypeFactory>("data type", nvc);

            if (nvc["name"] == "sop")
            {
                return container.GetInstance<DocumentTypeFactory>().EnsureSpecificThing("DocumentType", DocumentType.Indices.STANDARD_OPERATING_PROCEDURE,
                    new Dictionary<string,object> {
                        {"DocumentTypeID",DocumentType.Indices.STANDARD_OPERATING_PROCEDURE},
                        {"Document_Type","Standard Operating Procedure"},
                        { "DataTypeId", ((IEntity)dataType).Id }
                    });
            }

            return container.GetInstance<DocumentTypeFactory>().Create(new { DataType = dataType, Name = nvc["name"] });
        }

        public static ServiceMaterial CreateServiceMaterial(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var isEditEnabled = nvc["is edit enabled"];
            return container.GetInstance<ServiceMaterialFactory>().Create(new {
                Description = nvc["description"],
                IsEditEnabled = String.IsNullOrWhiteSpace(isEditEnabled) ? false : bool.Parse(isEditEnabled),
                CustomerEPACode = objectCache.GetValueOrDefault<EPACodeFactory>("e p a code", "customer e p a code", nvc),
                CompanyEPACode = objectCache.GetValueOrDefault<EPACodeFactory>("e p a code", "company e p a code", nvc)
            });
        }
        
        public static EquipmentCategory CreateEquipmentCategory(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            if (nvc["description"].ToLowerInvariant() == "flow meter")
            {
                return container.GetInstance<EquipmentCategoryFlowMeterFactory>().Create();
            }
            else
            {
                var factory = container.GetInstance<EquipmentCategoryFactory>();

                var model = factory.Create(new
                {
                    Description = nvc["description"]
                });

                return model;
            }
        }

        public static EmployeeStatus CreateEmployeeStatus(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToLower())
            {
                case "active":
                    return container.GetInstance<ActiveEmployeeStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactiveEmployeeStatusFactory>().Create();
                default:
                    throw new ArgumentException($"Not sure how to create employee status with description {nvc["description"]}.");
            }
        }

        public static EquipmentCharacteristicDropDownValue CreateEquipmentCharacteristicDropDownValue(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<TestDataFactory<EquipmentCharacteristicDropDownValue>>();

            var model = factory.Create(new
            {
                Value = nvc["value"],
                Field = objectCache.GetValueOrDefault<TestDataFactory<EquipmentCharacteristicField>>("equipment characteristic field", "field", nvc)
            });

            return model;
        }

        public static PlanReview CreatePlanReview(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<TestDataFactory<PlanReview>>();

            var model = factory.Create(new {
                ReviewDate = nvc["review date"],
                CreatedAt = nvc["review date"],
                NextReviewDate = nvc["next review date"],
                ReviewChangeNotes = nvc["review change notes"],
                ReviewedBy = objectCache.GetValueOrDefault<EmployeeFactory>("employee", "employee", nvc),
                CreatedBy = objectCache.GetValueOrDefault<UserFactory>("user", "created by", nvc),
                Plan = objectCache.GetValueOrDefault<TestDataFactory<EmergencyResponsePlan>>("emergency response plan", "plan", nvc)
            });

            return model;
        }

        public static EquipmentSensor CreateEquipmentSensor(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            if (!nvc.ContainsKey("equipment") || !nvc.ContainsKey("sensor"))
            {
                throw new InvalidOperationException("CreateEquipmentSensor requires both equipment and sensor values.");
            }
            var factory = container.GetInstance<EquipmentSensorFactory>();
            return factory.Create(new
            {
                Equipment = objectCache.GetValueOrDefault<EquipmentFactory>("equipment", nvc),
                Sensor = objectCache.GetValueOrDefault<SensorFactory>("sensor", nvc)
            });
        }

        public static EquipmentSubCategory CreateEquipmentSubCategory(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<EquipmentSubCategoryFactory>();

            var model = factory.Create(new
            {
                Description = nvc["description"]
            });

            return model;
        }

        public static EquipmentLifespan CreateEquipmentLifespan(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            EquipmentLifespan equipmentLifespan;
            if (nvc["description"].ToLower() == "engine")
                equipmentLifespan = container.GetInstance<EngineEquipmentLifespanFactory>().Create();
            else if (nvc["description"].ToLower() == "generator")
                equipmentLifespan = container.GetInstance<GeneratorEquipmentLifespanFactory>().Create();
            else if (nvc["description"].ToLower() == "tnk")
                equipmentLifespan = container.GetInstance<GeneratorEquipmentLifespanFactory>().Create();
            else if (nvc["description"].ToLower() == "filter")
                equipmentLifespan = container.GetInstance<FilterEquipmentLifespanFactory>().Create();
            else
                equipmentLifespan = container.GetInstance<ChemicalFeedDryEquipmentLifespanFactory>().Create();
            
            return equipmentLifespan;
        }

        public static EquipmentLifespan CreateEngineEquipmentLifespan(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<EngineEquipmentLifespanFactory>();
            var model = factory.Create();

            return model;
        }

        private static object CreateEquipmentLikelyhoodOfFailureRating(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            EquipmentLikelyhoodOfFailureRatingFactory factory;

            switch (nvc["description"].ToLower())
            {
                case "low":
                    factory = container.GetInstance<LowEquipmentLikelyhoodOfFailureRatingFactory>();
                    break;
                case "medium":
                    factory = container.GetInstance<MediumEquipmentLikelyhoodOfFailureRatingFactory>();
                    break;
                case "high":
                    factory = container.GetInstance<HighEquipmentLikelyhoodOfFailureRatingFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(EquipmentLikelyhoodOfFailureRating)} with value {nvc["description"]}");
            }

            return factory.Create();
        }

        private static object CreateEquipmentConsequencesOfFailureRating(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            EquipmentConsequencesOfFailureRatingFactory factory;

            switch (nvc["description"].ToLower())
            {
                case "low":
                    factory = container.GetInstance<LowEquipmentConsequencesOfFailureRatingFactory>();
                    break;
                case "medium":
                    factory = container.GetInstance<MediumEquipmentConsequencesOfFailureRatingFactory>();
                    break;
                case "high":
                    factory = container.GetInstance<HighEquipmentConsequencesOfFailureRatingFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(EquipmentConsequencesOfFailureRating)} with value {nvc["description"]}");
            }

            return factory.Create();
        }

        private static object CreateFacilityConsequenceOfFailure(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            FacilityConsequenceOfFailureFactory factory;

            switch (nvc["description"].ToLower())
            {
                case "low":
                    factory = container.GetInstance<LowFacilityConsequenceOfFailureFactory>();
                    break;
                case "medium":
                    factory = container.GetInstance<MediumFacilityConsequenceOfFailureFactory>();
                    break;
                case "high":
                    factory = container.GetInstance<HighFacilityConsequenceOfFailureFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(FacilityConsequenceOfFailure)} with value {nvc["description"]}");
            }

            return factory.Create();
        }

        private static object CreateFacilityAssetManagementMaintenanceStategyTier(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            FacilityAssetManagementMaintenanceStrategyTierFactory factory;

            switch (nvc["description"].ToLower())
            {
                case "one":
                    factory = container.GetInstance<Tier1FacilityAssetManagementMaintenanceStrategyTierFactory>();
                    break;
                case "two":
                    factory = container.GetInstance<Tier2FacilityAssetManagementMaintenanceStrategyTierFactory>();
                    break;
                case "three":
                    factory = container.GetInstance<Tier3FacilityAssetManagementMaintenanceStrategyTierFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(FacilityAssetManagementMaintenanceStrategyTier)} with value {nvc["description"]}");
            }

            return factory.Create();
        }

        private static object CreateFacilityMaintenanceRiskOfFailure(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            FacilityMaintenanceRiskOfFailureFactory factory;

            switch (nvc["description"].ToLower())
            {
                case "low":
                    factory = container.GetInstance<FacilityLowMaintenanceRiskOfFailureFactory>();
                    break;
                case "lowmod":
                    factory = container.GetInstance<FacilityLowModMaintenanceRiskOfFailureFactory>();
                    break;
                case "mod":
                    factory = container.GetInstance<FacilityModMaintenanceRiskOfFailureFactory>();
                    break;
                case "modhigh":
                    factory = container.GetInstance<FacilityModHighMaintenanceRiskOfFailureFactory>();
                    break;
                case "high":
                    factory = container.GetInstance<FacilityHighMaintenanceRiskOfFailureFactory>();
                    break;
                case "critical":
                    factory = container.GetInstance<FacilityHighCriticalMaintenanceRiskOfFailureFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(FacilityMaintenanceRiskOfFailure)} with value {nvc["description"]}");
            }

            return factory.Create();
        }

        public static object CreateEquipmentPerformanceRating(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            EquipmentPerformanceRatingFactory factory;

            switch (nvc["description"].ToLower())
            {
                case "poor":
                    factory = container.GetInstance<PoorEquipmentPerformanceRatingFactory>();
                    break;
                case "average":
                    factory = container.GetInstance<AverageEquipmentPerformanceRatingFactory>();
                    break;
                case "good":
                    factory = container.GetInstance<GoodEquipmentPerformanceRatingFactory>();
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(EquipmentPerformanceRating)} with value {nvc["description"]}");
            }

            return factory.Create();
        }

        public static EquipmentLifespan CreateGeneratorEquipmentLifespan(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<GeneratorEquipmentLifespanFactory>();
            var model = factory.Create();
            return model;
        }

        public static EquipmentLifespan CreateFilterEquipmentLifespan(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<FilterEquipmentLifespanFactory>();
            var model = factory.Create();
            return model;
        }

        public static EquipmentLifespan CreateEquipmentManufacturerType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<EquipmentLifespanFactory>();

            var model = factory.Create(new
            {
                Description = nvc["description"]
            });

            return model;
        }

        private static object CreateFilterMedia(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var eq = nvc["equipment"] == "null" ?
                null : obj.GetValueOrDefault<EquipmentFactory>("equipment", nvc);
            return container.GetInstance<FilterMediaFactory>().Create(new {
                Equipment = eq
            });
        }

        public static Generator CreateGenerator(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<GeneratorFactory>();

            var model = factory.Create(new {
                Equipment = obj.GetValueOrDefault<EquipmentFactory>("equipment", nvc)
            });

            return model;
        }

        public static EquipmentModel CreateEquipmentModel(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var engman = obj.GetOrNull("equipment manufacturer", "engine", nvc);
            var genman = obj.GetOrNull("equipment manufacturer", "generator", nvc);
            var filman = obj.GetOrNull("equipment manufacturer", "filter", nvc);
            var manufacturer = engman ?? genman ?? filman;

            if (manufacturer == null)
            {
                return container.GetInstance<EquipmentModelFactory>().Create(new {
                    Description = nvc["description"]
                });
            }

            return container.GetInstance<EquipmentModelFactory>().Create(new {
                EquipmentManufacturer = manufacturer, 
                Description = nvc["description"]
            });
        }

        public static EquipmentStatus CreateEquipmentStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLower())
            {
                case "in service":
                    return container.GetInstance<InServiceEquipmentStatusFactory>().Create();
                case "out of service":
                    return container.GetInstance<OutOfServiceEquipmentStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingEquipmentStatusFactory>().Create();
                case "retired":
                    return container.GetInstance<RetiredEquipmentStatusFactory>().Create();
                case "pending retirement":
                    return container.GetInstance<PendingRetirementEquipmentStatusFactory>().Create();
                case "cancelled":
                    return container.GetInstance<CancelledEquipmentStatusFactory>().Create();
                case "field installed":
                    return container.GetInstance<FieldInstalledEquipmentStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create {nameof(EquipmentStatusFactory)} with value {nvc["description"]}");
            }
        }

        public static EquipmentPurpose CreateEquipmentPurpose(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var engineEquipmentLifespan = obj.GetOrNull("engine equipment lifespan", nvc);
            var generatorEquipmentLifespan = obj.GetOrNull("generator equipment lifespan", nvc);
            var tankEquipmentLifespan = obj.GetOrNull("tank equipment lifespan", nvc);
            var filterEquipmentLifespan = obj.GetOrNull("filter equipment lifespan", nvc);
            var equipmentLifespan = engineEquipmentLifespan ?? generatorEquipmentLifespan ?? filterEquipmentLifespan ?? tankEquipmentLifespan ??
                container.GetInstance<EngineEquipmentLifespanFactory>().Create();

            if (nvc.ContainsKey("abbreviation"))
            {
                return container.GetInstance<EquipmentPurposeFactory>().Create(new {
                    Description = nvc["description"],
                    Abbreviation = nvc["abbreviation"],
                    EquipmentCategory = obj.GetValueOrDefault<EquipmentCategoryFactory>("equipment category", nvc),
                    EquipmentSubCategory = obj.GetValueOrDefault<EquipmentSubCategoryFactory>("equipment subcategory", nvc),
                    EquipmentType = obj.GetOrNull("equipment type", nvc),
                    EquipmentLifespan = equipmentLifespan
                });
            }

            return container.GetInstance<EquipmentPurposeFactory>().Create(new {
                Description = nvc["description"],
                EquipmentCategory = obj.GetValueOrDefault<EquipmentCategoryFactory>("equipment category", nvc),
                EquipmentSubCategory = obj.GetValueOrDefault<EquipmentSubCategoryFactory>("equipment subcategory", nvc),
                EquipmentType = obj.GetOrNull("equipment type", nvc),
                EquipmentLifespan = equipmentLifespan
            });
        }

        public static FacilityStatus CreateFacilityStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "active":
                    return container.GetInstance<ActiveFacilityStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactiveFacilityStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingFacilityStatusFactory>().Create();
                case "pending_retirement":
                    return container.GetInstance<PendingRetirementFacilityStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a facility status with description '{nvc["description"]}'");
            }
        }

        public static FuelType CreateFuelType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<FuelTypeFactory>();

            var model = factory.Create(new
            {
                Description = nvc["description"]
            });

            return model;
        }

        public static Interconnection CreateInterconnection(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<InterconnectionFactory>();

            var model = factory.Create(new
            {
                DEPDesignation = nvc["dep designation"],
                Facility = obj.GetValueOrDefault<FacilityFactory>("facility", nvc)
            });

            return model;
        }

        public static HydrantBilling CreateHydrantBilling(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLower())
            {
                case "public":
                    return container.GetInstance<PublicHydrantBillingFactory>().Create();
                case "company":
                    return container.GetInstance<CompanyHydrantBillingFactory>().Create();
                case "private":
                    return container.GetInstance<PrivateHydrantBillingFactory>().Create();
                case "municipal":
                    return container.GetInstance<MunicipalHydrantBillingFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to build hydrant billing '{nvc["description"]}'");
            }
        }

        public static HydrantInspection CreateHydrantInspection(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<HydrantInspectionFactory>();
            var session = container.GetInstance<ISession>();

            var model = factory.Create(new
            {
                Hydrant = obj.GetValueOrDefault<HydrantFactory>("hydrant", nvc),
                DateInspected = nvc.GetValueAs<DateTime>("date inspected"),
                MinutesFlowed = nvc.GetValueAs<decimal>("minutes flowed"),
                GPM = nvc.GetValueAs<decimal>("gpm"),
                HydrantInspectionType = obj.GetValueOrDefault<HydrantInspectionTypeFactory>("hydrant inspection type", nvc)
            });

            // This needs to be optional so that it's null if it isn't set, rather than some default factory value.

            var wor1 = obj.GetValueOrDefault<WorkOrderRequestFactory>("work order request", nvc);
            if (wor1.GetType() == typeof(WorkOrderRequest))
            {
                model.WorkOrderRequestOne = (WorkOrderRequest)wor1;
                session.Save(model);
                session.Flush();
            }

            return model;
        }

        public static NpdesRegulatorInspection CreateNpdesRegulatorInspection(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<NpdesRegulatorInspectionFactory>();
            var session = container.GetInstance<ISession>();

            var model = factory.Create(new {
                SewerOpening = obj.GetValueOrDefault<SewerOpeningFactory>("sewer opening", nvc),
                ArrivalDateTime = nvc.GetValueAs<DateTime>("arrival date time"),
                DepartureDateTime = nvc.GetValueAs<DateTime>("departure date time"),
                InspectedBy = obj.GetValueOrDefault<UserFactory>("user", "inspected by", nvc),
                BlockCondition = obj.GetValueOrDefault<OutBlockConditionFactory>("block condition", nvc),
                OutfallCondition = obj.GetValueOrDefault<OutfallConditionFactory>("outfall condition", nvc),
                WeatherCondition = obj.GetValueOrDefault<WeatherConditionFactory>("weather condition", nvc),
                NpdesRegulatorInspectionType = obj.GetValueOrDefault<StandardNpdesRegulatorInspectionTypeFactory>("npdes regulator inspection type", nvc),
                IsDischargePresent = nvc.GetValueAs<bool>("is discharge present")
            });

            return model;
        }

        public static NpdesRegulatorInspectionType CreateNpdesRegulatorInspectionType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLower())
            {
                case "standard":
                    return container.GetInstance<StandardNpdesRegulatorInspectionTypeFactory>().Create();
                case "rain event":
                    return container.GetInstance<RainEventNpdesRegulatorInspectionTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to build npdes regulator inspection type '{nvc["description"]}'");
            }
        }

        public static GateStatusAnswerType CreateGateStatusAnswerType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLower())
            {
                case "yes":
                    return container.GetInstance<YesGateStatusAnswerTypeFactory>().Create();
                case "no":
                    return container.GetInstance<NoGateStatusAnswerTypeFactory>().Create();
                case "n/a":
                    return container.GetInstance<NotAvailableGateStatusAnswerTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to build npdes regulator inspection form answer type '{nvc["description"]}'");
            }
        }

        private static object CreateAssetStatus(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            AssetStatusFactory factory;

            switch (nvc["description"].ToUpper())
            {
                case "ACTIVE":
                    factory = container.GetInstance<ActiveAssetStatusFactory>();
                    break;
                case "INACTIVE":
                    factory = container.GetInstance<InactiveAssetStatusFactory>();
                    break;
                case "RETIRED":
                    factory = container.GetInstance<RetiredAssetStatusFactory>();
                    break;
                case "REMOVED":
                    factory = container.GetInstance<RemovedAssetStatusFactory>();
                    break;
                case "REQUEST RETIREMENT":
                    factory = container.GetInstance<RequestRetirementAssetStatusFactory>();
                    break;
                case "CANCELLED":
                    factory = container.GetInstance<CancelledAssetStatusFactory>();
                    break;
                case "PENDING":
                    factory = container.GetInstance<PendingAssetStatusFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating asset status for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        private static object CreateComplianceRequirement(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            ComplianceRequirementFactory factory;
            switch (nvc["description"].ToUpper())
            {
                case "COMPANY":
                    factory = container.GetInstance<CompanyComplianceRequirementFactory>();
                    break;
                case "OSHA":
                    factory = container.GetInstance<OSHAComplianceRequirementFactory>();
                    break;
                case "PSM":
                    factory = container.GetInstance<PSMComplianceRequirementFactory>();
                    break;
                case "REGULATORY":
                    factory = container.GetInstance<RegulatoryComplianceRequirementFactory>();
                    break;
                case "TCPA":
                    factory = container.GetInstance<TCPAComplianceRequirementFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating compliance requirement for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        private static object CreateConfinedSpaceFormEntrantType(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            ConfinedSpaceFormEntrantTypeFactory factory;

            switch (nvc["description"].ToUpper())
            {
                case "ENTRANT":
                    factory = container.GetInstance<EntrantConfinedSpaceFormEntrantTypeFactory>();
                    break;
                case "ATTENDANT":
                    factory = container.GetInstance<AttendantConfinedSpaceFormEntrantTypeFactory>();
                    break;
                case "SUPERVISOR":
                case "ENTRY SUPERVISOR":
                    factory = container.GetInstance<EntrySupervisorConfinedSpaceFormEntrantTypeFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating confined space form entrant type for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        public static ConfinedSpaceForm CreateCompletedConfinedSpaceForm(NameValueCollection nvc, TestObjectCache obj,
            IContainer container)
        {
            var factory = container.GetInstance<CompletedConfinedSpaceFormFactory>();
            var model = factory.Create();
            return model;
        }

        public static JobSiteExcavation CreateJobSiteExcavation(NameValueCollection nvc, TestObjectCache obj,
            IContainer container)
        {
            var factory = container.GetInstance<JobSiteExcavationFactory>();
            JobSiteCheckList checklist = null;
            if (nvc.ContainsKey("job site check list"))
            {
                checklist = (JobSiteCheckList)obj.GetValueOrDefault<JobSiteCheckListFactory>("job site check list", nvc);
            }
            else if (nvc.ContainsKey("job site check list that is not signed off by a supervisor"))
            {
                checklist =
                    (JobSiteCheckList)
                        obj.GetValueOrDefault<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>(
                            "job site check list that is not signed off by a supervisor", nvc);
            }

            var model = factory.Create(new
            {
                JobSiteCheckList = checklist,
                ExcavationDate = nvc.GetValueAs<DateTime>("excavation date"),
                WidthInFeet = nvc.GetValueAs<decimal>("width in feet"),
                LengthInFeet = nvc.GetValueAs<decimal>("length in feet"),
                DepthInInches = nvc.GetValueAs<decimal>("depth in inches"),
                LocationType = obj.GetValueOrDefault<JobSiteExcavationLocationTypeFactory>("job site excavation location type", nvc),
                SoilType = obj.GetValueOrDefault<JobSiteExcavationSoilTypeFactory>("job site excavation soil type", nvc)
            });
            return model;
        }

        public static JobSiteCheckListComment CreateJobSiteCheckListComment(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            JobSiteCheckList checklist = null;
            if (nvc.ContainsKey("job site check list"))
            {
                checklist = (JobSiteCheckList)obj.GetValueOrDefault<JobSiteCheckListFactory>("job site check list", nvc);
            }
            else if (nvc.ContainsKey("job site check list that is not signed off by a supervisor"))
            {
                checklist =
                    (JobSiteCheckList)
                        obj.GetValueOrDefault<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>(
                            "job site check list that is not signed off by a supervisor", nvc);
            }

            var factory = container.GetInstance<JobSiteCheckListCommentFactory>();
            return factory.Create(new
            {
                JobSiteCheckList = checklist,
                Comments = nvc["comments"]
            });
        }

        public static JobSiteCheckListCrewMembers CreateJobSiteCheckListCrewMembers(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            JobSiteCheckList checklist = null;
            if (nvc.ContainsKey("job site check list"))
            {
                checklist = (JobSiteCheckList)obj.GetValueOrDefault<JobSiteCheckListFactory>("job site check list", nvc);
            }
            else if (nvc.ContainsKey("job site check list that is not signed off by a supervisor"))
            {
                checklist =
                    (JobSiteCheckList)
                        obj.GetValueOrDefault<JobSiteCheckListThatIsNotSignedOffByASupervisorFactory>(
                            "job site check list that is not signed off by a supervisor", nvc);
            }

            var factory = container.GetInstance<JobSiteCheckListCrewMembersFactory>();
            return factory.Create(new
            {
                JobSiteCheckList = checklist,
                CrewMembers = nvc["crew members"]
            });
        }

        public static JobSiteExcavationLocationType CreateJobSiteExcavationLocationType(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<JobSiteExcavationLocationTypeFactory>();

            return factory.Create(new
            {
                Description = nvc["description"]
            });
        }

        public static JobTitleCommonName CreateJobTitleCommonName(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<JobTitleCommonNameFactory>().Create(new
            {
                Description = nvc["description"]
            });
        }

        private static object CreateLIMSStatus(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            LIMSStatusFactory factory;

            switch (nvc["description"])
            {
                case "Not Ready":
                    factory = container.GetInstance<NotReadyLIMSStatusFactory>();
                    break;
                case "Ready to Send":
                    factory = container.GetInstance<ReadyToSendLIMSStatusFactory>();
                    break;
                case "Sent Successfully":
                    factory = container.GetInstance<SentSuccessfullyLIMSStatusFactory>();
                    break;
                case "Send Failed":
                    factory = container.GetInstance<SendFailedLIMSStatusFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating bacterial sample type for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        public static LockoutDevice CreateLockoutDevice(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<LockoutDeviceFactory>();
            var model = factory.Create(new
            {
                OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                Person = obj.GetValueOrDefault<UserFactory>("user", "person", nvc)
            });
            return model;
        }

        //public static LockoutForm CreateLockoutForm(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        //{
        //    var factory = container.GetInstance<LockoutFormFactory>();

        //    var model = factory.Create(new
        //    {
        //        OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
        //        Facility = obj.GetValueOrDefault<FacilityFactory>("facility", nvc),
        //        EquipmentType = obj.GetValueOrDefault<EquipmentTypeFactory>("equipment type", nvc)
        //    });

        //    return model;
        //}

        public static LockoutFormQuestionCategory CreateLockoutFormQuestionCategory(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "lockout conditions":
                    return container.GetInstance<LockoutConditionsLockoutFormQuestionCategoryFactory>().Create();
                case "out of service":
                    return container.GetInstance<OutOfServiceLockoutFormQuestionCategoryFactory>().Create();
                case "management":
                    return container.GetInstance<ManagementLockoutFormQuestionCategoryFactory>().Create();
                case "return to service":
                    return container.GetInstance<ReturnToServiceLockoutFormQuestionCategoryFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create lockout form question category from description string '{nvc["description"]}'");
            }
        }

        public static FacilityInspectionFormQuestionCategory CreateFacilityInspectionFormQuestionCategories(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "general work area/Conditions":
                    return container.GetInstance<GeneralWorkAreaConditionsFacilityInspectionFormQuestionCategoriesFactory>().Create();
                case "emergency response first aid":
                    return container.GetInstance<EmergencyResponseFirstAidFacilityInspectionFormQuestionCategoriesFactory>().Create();
                case "security":
                    return container.GetInstance<SecurityFacilityInspectionFormQuestionCategoriesFactory>().Create();
                case "fire safety":
                    return container.GetInstance<FireSafetyFacilityInspectionFormQuestionCategoriesFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create facility inspection form question category from description string '{nvc["description"]}'");
            }
        }

        public static FacilityInspectionAreaType CreateFacilityInspectionAreaType(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "lab":
                    return container.GetInstance<LabFacilityInspectionAreaTypeFactory>().Create();
                case "grounds":
                    return container.GetInstance<GroundsFacilityInspectionAreaTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create facility inspection area type from description string '{nvc["description"]}'");
            }
        }
        public static MainCrossing CreateMainCrossing(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<MainCrossingFactory>();
            object frequencyUnit = null;
            if (nvc["inspection frequency unit"] == "month")
                frequencyUnit = objectCache.GetValueOrDefault<MonthlyRecurringFrequencyUnitFactory>("inspection frequency unit", "month", nvc);

            MainCrossing model;

            if (frequencyUnit == null)
            {
                model = factory.Create(new {
                    LengthOfSegment = nvc.GetValueAs<decimal>("length of segment"),
                    InspectionFrequency = nvc.GetValueAs<int>("inspection frequency"),
                    OperatingCenter = objectCache.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                    Town = objectCache.GetValueOrDefault<OperatingCenterFactory>("town", nvc),
                    CrossingType = objectCache.GetValueOrDefault<CrossingTypeFactory>("crossing type", nvc),
                    SupportStructure = objectCache.GetValueOrDefault<SupportStructureFactory>("support structure", nvc)
                });
            }
            else
            {
                model = factory.Create(new {
                    LengthOfSegment = nvc.GetValueAs<decimal>("length of segment"),
                    InspectionFrequency = nvc.GetValueAs<int>("inspection frequency"),
                    InspectionFrequencyUnit = frequencyUnit,
                    OperatingCenter = objectCache.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                    Town = objectCache.GetValueOrDefault<OperatingCenterFactory>("town", nvc),
                    CrossingType = objectCache.GetValueOrDefault<CrossingTypeFactory>("crossing type", nvc),
                    SupportStructure = objectCache.GetValueOrDefault<SupportStructureFactory>("support structure", nvc)
                });
            }

            return model;
        }

        public static MarkoutRequirement CreateMarkoutRequirement(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "none":
                    return container.GetInstance<NoneMarkoutRequirementFactory>().Create();
                case "routine":
                    return container.GetInstance<RoutineMarkoutRequirementFactory>().Create();
                case "emergency":
                    return container.GetInstance<EmergencyMarkoutRequirementFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create requirement with description '{nvc["description"]}'");
            }
        }

        public static MarkoutType CreateMarkoutType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "c to c":
                    return container.GetInstance<CtoCMarkoutTypeFactory>().Create();
                case "none":
                    return container.GetInstance<NoneMarkoutTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create markout type with description '{nvc["description"]}'");
            }
        }

        public static NonRevenueWaterAdjustment CreateNonRevenueWaterAdjustment(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<NonRevenueWaterAdjustmentFactory>().Create(new {
                NonRevenueWaterEntry = objectCache.GetValueOrDefault<NonRevenueWaterEntryFactory>("nonrevenue water entry", nvc),
                Comments = nvc["comments"],
                BusinessUnit = nvc["business unit"],
                TotalGallons = nvc.GetValueAs<long>("total gallons")
            });
        }
        public static NonRevenueWaterDetail CreateNonRevenueWaterDetail(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<NonRevenueWaterDetailFactory>().Create(new {
                NonRevenueWaterEntry = objectCache.GetValueOrDefault<NonRevenueWaterEntryFactory>("nonrevenue water entry", nvc),
                Month = nvc["month"],
                Year = nvc["year"],
                BusinessUnit = nvc["business unit"],
                WorkDescription = nvc["work description"],
                TotalGallons = nvc.GetValueAs<long>("total gallons")
            });
        }
        public static NonRevenueWaterEntry CreateNonRevenueWaterEntry(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<NonRevenueWaterEntryFactory>().Create(new {
                OperatingCenter = objectCache.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                CreatedAt = nvc.GetValueAs<DateTime>("created at"),
                CreatedBy = objectCache.GetValueOrDefault<UserFactory>("user", nvc),
                Month = nvc.GetValueAs<int>("month"),
                Year = nvc.GetValueAs<int>("year")
            });
        }

        private static object CreateNote(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var ignoreKeys = new[] { "text", "data type" };

            var linkedItemKey = nvc.AllKeys.Where(k => !ignoreKeys.Contains(k)).First();
            var linkedItem = (IThingWithNotes)cache.GetOrNull(linkedItemKey, nvc);

            var args = new
            {
                Text = nvc.GetValueOrDefault("text"),
                DataType = cache.GetValueOrDefault<DataTypeFactory>("data type", nvc),
                LinkedId = linkedItem.Id
            };

            return container.GetInstance<NoteFactory>().Create(args);
        }

        public static NotificationPurpose CreateNotificationPurpose(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var moduleEnum = nvc.GetValueAs<RoleModules>("module");
            var appEnum = EnumExtensions.GetValues<RoleApplications>().Single(x => moduleEnum.ToString().StartsWith(x.ToString()));
            var app = container.GetInstance<ApplicationFactory>().Create(new { Id = appEnum });
            var module = container.GetInstance<ModuleFactory>().Create(new { Id = moduleEnum, Application = app });

            return container.GetInstance<NotificationPurposeFactory>().Create(new
            {
                Purpose = nvc["purpose"],
                //Module = (Module)objectCache.GetValueOrDefault<ModuleFactory>("module", nvc)
                //Module = objectCache.Lookup<Module>("module", nvc["module"])
                Module = module
            });
        }


        public static OneCallMarkoutTicket CreateOneCallMarkoutTicket(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            OneCallMarkoutTicket foo;

            if (nvc.ContainsKey("c d c code"))
            {
                foo = container.GetInstance<OneCallMarkoutTicketFactory>().Create(new {
                    MessageType = objectCache.GetValueOrDefault<OneCallMarkoutMessageTypeFactory>("one call markout message type", "message type", nvc),
                    RequestNumber = nvc.GetValueAs<int>("request number"),
                    RelatedRequestNumber = nvc.GetValueAs<int>("related request number"),
                    CDCCode = nvc["c d c code"]
                });
            }
            else
            {
                foo = container.GetInstance<OneCallMarkoutTicketFactory>().Create(new {
                    MessageType = objectCache.GetValueOrDefault<OneCallMarkoutMessageTypeFactory>("one call markout message type", "message type", nvc),
                    RequestNumber = nvc.GetValueAs<int>("request number"),
                    RelatedRequestNumber = nvc.GetValueAs<int>("related request number")
                });
            }

            return foo;
        }

        public static OperatingCenter CreateOperatingCenter(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // NOTE: If you're here because you're setting values for an operating center and they're not persisting
            //       for the regression test it's due to an operating center already existing with the same opcode. The
            //       OperatingCenterFactory will return the existing record that matches the opcode rather than create a
            //       new one. To fix this in your test you need to change the opcode to something else. -Ross 1/10/2018

            // NOTE: For the above, you'll definitely see that if you're creating a user and then trying to create 
            //       an operating center for NJ7. CreateUser sets DefaultOperatingCenter, which defaults to making NJ7.

            const string recFreqType = "recurring frequency unit";

            // TestObjectCachce.GetValueOrDefault chokes and dies if you try to use it when no type is created, rather than
            // returning the default(since there aren't any matching values). So this is what gets to be done:
            object hydrantPaintUnit, largeValveUnit, smallValveUnit;
            object hydrantUnit = hydrantPaintUnit = largeValveUnit = smallValveUnit = typeof(YearlyRecurringFrequencyUnitFactory);

            if (objectCache.ContainsKey(recFreqType))
            {
                hydrantUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "hydrantInspFreqUnit", nvc);
                hydrantPaintUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "hydrantPaintFreqUnit", nvc);
                largeValveUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "largeValveInspFreqUnit", nvc);
                smallValveUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "smallValveInspFreqUnit", nvc);
            }
            int? gallons = null;
            if (!String.IsNullOrWhiteSpace(nvc["maximum overflow gallons"]))
                gallons = Int32.Parse(nvc["maximum overflow gallons"]);

            var oc = container.GetInstance<OperatingCenterFactory>().Create(new {
                OperatingCenterCode = nvc["opcode"],
                OperatingCenterName = nvc["name"],
                CompanyInfo = nvc["companyInfo"],
                PhoneNumber = nvc["phone"],
                FaxNumber = nvc["fax"],
                MailingAddressName = nvc["mailingName"],
                MailingAddressStreet = nvc["mailingStreet"],
                MailingAddressCityStateZip = nvc["mailingCSZ"],
                ServiceContactPhoneNumber = nvc["servicePhone"],
                HydrantInspectionFrequency = nvc.GetValueAs<int>("hydrantInspFreq"),
                HydrantInspectionFrequencyUnit = hydrantUnit,
                HydrantPaintingFrequency = nvc.GetValueAs<int>("hydrantPaintFreq"),
                HydrantPaintingFrequencyUnit = hydrantPaintUnit,
                LargeValveInspectionFrequency = nvc.GetValueAs<int>("largeValveInspFreq"),
                LargeValveInspectionFrequencyUnit = largeValveUnit,
                SmallValveInspectionFrequency = nvc.GetValueAs<int>("smallValveInspFreq"),
                SmallValveInspectionFrequencyUnit = smallValveUnit,
                State = objectCache.GetValueOrDefault<StateFactory>("state", nvc),
                PermitsOMUserName = nvc["permitsOmUserName"],
                PermitsCapitalUserName = nvc["permitsCapitalUserName"],
                WorkOrdersEnabled = nvc.GetValueAs<bool>("workOrdersEnabled").GetValueOrDefault(true),
                HasWorkOrderInvoicing = nvc.GetValueAs<bool>("hasWorkOrderInvoicing").GetValueOrDefault(false),
                DefaultServiceReplacementWBSNumber =
                    objectCache.GetValueOrDefault<WBSNumberFactory>("w b s number", nvc),
                IsContractedOperations = nvc.GetValueAs<bool>("is contracted operations"),
                SAPEnabled = nvc.GetValueAs<bool>("sap enabled"),
                SAPWorkOrdersEnabled = nvc.GetValueAs<bool>("sap work orders enabled"),
                UsesValveInspectionFrequency = nvc.GetValueAs<bool>("uses valve inspection frequency"),
                ZoneStartYear = nvc.GetValueAs<int>("zone start year"),
                IsActive = nvc.GetValueAs<bool>("is active") ?? true,
                ArcMobileMapId = nvc["arc mobile map id"] ?? "some map id",
                MapId = nvc["mapId"],
                MarkoutsEditable = nvc.GetValueAs<bool>("markoutsEditable").GetValueOrDefault(false),
            });

            if (gallons != null)
            {
                oc.MaximumOverflowGallons = gallons.Value;
            }
            return oc;
        }

        private static object CreateOrderType(NameValueCollection nvc, TestObjectCache _,
            IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("Please enter a description to create the order type");
            }

            switch (nvc["description"])
            {
                case "operational activity":
                    return container.GetInstance<OperationalOrderTypeFactory>().Create();
                case "pm work order":
                    return container.GetInstance<PlantMaintenanceOrderTypeFactory>().Create();
                case "corrective action":
                    return container.GetInstance<CorrectiveActionOrderTypeFactory>().Create();
                case "rp capital":
                    return container.GetInstance<RpCapitalOrderTypeFactory>().Create();
                case "routine":
                    return container.GetInstance<RoutineOrderTypeFactory>().Create();

                default:
                    throw new InvalidOperationException($"Unable to create order type with description '{nvc["description"]}'.");
            }
        }
        public static object CreateOSHAStandard(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var osha = CreateEntityLookup<RegulationAgency, RegulationAgencyFactory>(cache, container.GetInstance<ISession>(), "regulation agency", "OSHA");
            var active = CreateEntityLookup<RegulationStatus, RegulationStatusFactory>(cache, container.GetInstance<ISession>(), "regulation status",
                "Active");

            return container.GetInstance<RegulationFactory>().Create(new
            {
                Agency = osha,
                Status = active
            });
        }

        private static object CreatePremise(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            // premise number: "1234567890", service address house number: "7", service address street: "Easy St", service address fraction: "1/2", meter serial number: "123", operating center: "nj7", region code: "one", device location: "1234"
            var opCenter = cache.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc);
            var regionCode = cache.GetOrNull("region code", nvc);

            var premise = container.GetInstance<PremiseFactory>().Create(new {
                PremiseNumber = nvc["premise number"],
                ServiceAddressStreet = nvc["service address street"],
                ServiceAddressFraction = nvc["service address fraction"],
                ServiceAddressApartment = nvc["service address apartment"],
                ServiceAddressHouseNumber = nvc["service address house number"],
                MeterSerialNumber = nvc["meter serial number"],
                DeviceLocation = nvc["device location"],
                Equipment = nvc["equipment"],
                DeviceCategory = nvc["device category"],
                OperatingCenter = opCenter,
                IsMajorAccount = nvc.GetValueAs<bool>("is major account"),
                RegionCode = regionCode,
                Installation = nvc["installation"],
                ConnectionObject = nvc["connection object"],
                ServiceCity = cache.GetValueOrDefault<TownFactory>("town", "service city", nvc),
                ServiceZip = nvc["service zip"],
                Coordinate = cache.GetValueOrDefault<CoordinateFactory>("coordinate", nvc),
                ServiceUtilityType = cache.GetValueOrDefault<ServiceUtilityTypeFactory>("service utility type", nvc)
            });
            return premise;
        }

        private static object CreateProductionPrerequisite(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("Please enter a description to create a production prerequisite");
            }

            switch (nvc["description"])
            {
                case "has lockout requirement":
                    return container.GetInstance<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
                case "is confined space":
                    return container.GetInstance<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
                case "job safety list":
                    return container.GetInstance<JobSafetyChecklistProductionPrerequisiteFactory>().Create();
                case "air permit":
                    return container.GetInstance<AirPermitProductionPrerequisiteFactory>().Create();
                case "hot work":
                    return container.GetInstance<HotWorkProductionPrerequisiteFactory>().Create();
                case "red tag permit":
                    return container.GetInstance<RedTagPermitProductionPrerequisiteFactory>().Create();
                case "pre job safety brief":
                    return container.GetInstance<PreJobSafetyBriefProductionPrerequisiteFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create production prerequisite with description '{nvc["description"]}'.");
            }
        }

        private static object CreateProductionWorkOrderEquipment(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            var productionWorkOrderEquipment = container.GetInstance<ProductionWorkOrderEquipmentFactory>().Create(new {
                ProductionWorkOrder = objectCache.GetValueOrDefault<ProductionWorkOrderFactory>("production work order", nvc),
                Equipment = objectCache.GetOrNull("equipment", nvc),
                SAPNotificationNumber = nvc.GetValueAs<long>("s a p notification number"),
                CompletedOn = nvc.GetValueAs<DateTime>("completed on"),
                SAPEquipmentId = nvc.GetValueAs<int>("s a p equipment id"),
                IsParent = nvc.GetValueAs<bool>("is parent").GetValueOrDefault(false)
            });

            return productionWorkOrderEquipment;
        }

        private static object CreateProductionWorkOrderFrequency(NameValueCollection nvc, TestObjectCache _,
            IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("Please enter a description to create a production work order frequency");
            }

            switch (nvc["description"])
            {
                case "daily":
                    return container.GetInstance<DailyProductionWorkOrderFrequencyFactory>().Create();
                case "weekly":
                    return container.GetInstance<WeeklyProductionWorkOrderFrequencyFactory>().Create();
                case "twice per month":
                    return container.GetInstance<TwicePerMonthProductionWorkOrderFrequencyFactory>().Create();
                case "monthly":
                    return container.GetInstance<MonthlyProductionWorkOrderFrequencyFactory>().Create();
                case "every two months":
                    return container.GetInstance<EveryTwoMonthsProductionWorkOrderFrequencyFactory>().Create();
                case "quarterly":
                    return container.GetInstance<QuarterlyProductionWorkOrderFrequencyFactory>().Create();
                case "every four months":
                    return container.GetInstance<EveryFourMonthsProductionWorkOrderFrequencyFactory>().Create();
                case "every six months":
                    return container.GetInstance<EverySixMonthsProductionWorkOrderFrequencyFactory>().Create();
                case "annual":
                    return container.GetInstance<AnnualProductionWorkOrderFrequencyFactory>().Create();
                case "every two years":
                    return container.GetInstance<EveryTwoYearsProductionWorkOrderFrequencyFactory>().Create();
                case "every three years":
                    return container.GetInstance<EveryThreeYearsProductionWorkOrderFrequencyFactory>().Create();
                case "every four years":
                    return container.GetInstance<EveryFourYearsProductionWorkOrderFrequencyFactory>().Create();
                case "every five years":
                    return container.GetInstance<EveryFiveYearsProductionWorkOrderFrequencyFactory>().Create();
                case "every ten years":
                    return container.GetInstance<EveryTenYearsProductionWorkOrderFrequencyFactory>().Create();
                case "every fifteen years":
                    return container.GetInstance<EveryFifteenYearsProductionWorkOrderFrequencyFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create production work order frequency with description '{nvc["description"]}'.");
            }
        }

        private static TEntity CreateEntityLookup<TEntity, TFactory>(TestObjectCache cache, ISession sesh, string dictionaryName, string description, string name = null)
            where TEntity : class, new()
            where TFactory : TestDataFactory<TEntity>
        {
            object obj;
            name = name ?? description.SanitizeAndDowncase();
            var agencyDictionary = cache.EnsureDictionary(dictionaryName);
            if (!agencyDictionary.TryGetValue(name, out obj))
            {
                var factory = (TFactory)DependencyResolver.Current.GetService(typeof(TFactory));
                obj = factory.Create(new { Description = description });
                agencyDictionary[name] = obj;
            }
            return (TEntity)obj;
        }

        public static Project CreateProject(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ProjectFactory>().Create(new {
                Name = nvc["name"]
            });
        }

        public static PublicWaterSupply CreatePublicWaterSupply(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<PublicWaterSupplyFactory>();

            int? usage = null;
            if (!String.IsNullOrWhiteSpace(nvc["usage last year"]))
            {
                usage = Int32.Parse(nvc["usage last year"]);
            }

            var model = factory.Create(new {
                Identifier = nvc["identifier"],
                OperatingArea = nvc["operating area"],
                Status = objectCache.GetValueOrDefault<PublicWaterSupplyStatusFactory>("public water supply status", "status", nvc),
                JanuaryRequiredBacterialWaterSamples = nvc.GetValueAs<int>("january required bacterial water samples"),
                MarchRequiredBacterialWaterSamples = nvc.GetValueAs<int>("march required bacterial water samples"),
                DecemberRequiredBacterialWaterSamples = nvc.GetValueAs<int>("december required bacterial water samples"),
                State = objectCache.GetValueOrDefault<StateFactory>("state", nvc),
                AWOwned = nvc.GetValueAs<bool>("aw owned"),
                Ownership = objectCache.GetValueOrDefault<PublicWaterSupplyOwnershipFactory>("public water supply ownership", "ownership", nvc),
                Type = objectCache.GetValueOrDefault<PublicWaterSupplyTypeFactory>("public water supply type", "type", nvc),
                System = nvc["system"],
                UsageLastYear = usage
            });

            return model;
        }

        public static Reading CreateReading(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<ReadingFactory>();
            return factory.Create(new
            {
                Sensor = objectCache.GetValueOrDefault<SensorFactory>("sensor", nvc),
                DateTimeStamp = nvc["date"].ToDateTime(),
                RawData = nvc.GetValueAs<int>("rawdata"),
                ScaledData = nvc.GetValueAs<float>("scaleddata"),
                CheckSum = nvc.GetValueAs<int>("checksum"),
                Interpolate = nvc.GetValueAs<int>("interpolate")
            });
        }

        public static RecurringFrequencyUnit CreateRecurringFrequencyUnit(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToUpper())
            {
                case "YEAR":
                    return container.GetInstance<YearlyRecurringFrequencyUnitFactory>().Create();
                case "MONTH":
                    return container.GetInstance<MonthlyRecurringFrequencyUnitFactory>().Create();
                case "WEEK":
                    return container.GetInstance<WeeklyRecurringFrequencyUnitFactory>().Create();
                default:
                    return container.GetInstance<DailyRecurringFrequencyUnitFactory>().Create();
            }
        }

        public static RecurringProject CreateRecurringProject(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<RecurringProjectFactory>();

            if (nvc.ContainsKey("project title"))
            {
                return factory.Create(new {
                    ProjectTitle = nvc["project title"],
                    Coordinate = objectCache.GetValueOrDefault<CoordinateFactory>("coordinate", nvc)
                });
            }

            return factory.Create(new {
                Coordinate = objectCache.GetValueOrDefault<CoordinateFactory>("coordinate", nvc)
            });
        }

        public static RecurringProjectStatus CreateRecurringProjectStatus(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            RecurringProjectStatus status;
            if (nvc["description"] == "complete")
                status = container.GetInstance<CompleteRecurringProjectStatusFactory>().Create();
            else
                status = container.GetInstance<ProposedRecurringProjectStatusFactory>().Create();
            return status;
        }

        public static Role CreateRole(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var actionEnum = nvc.GetValueAs<RoleActions>("action");
            var moduleEnum = nvc.GetValueAs<RoleModules>("module");
            var appEnum = EnumExtensions.GetValues<RoleApplications>().Single(x => moduleEnum.ToString().StartsWith(x.ToString()));
            var app = container.GetInstance<ApplicationFactory>().Create(new { Id = appEnum });
            var module = container.GetInstance<ModuleFactory>().Create(new { Id = moduleEnum, Application = app });
            var action = container.GetInstance<ActionFactory>().Create(new { Id = actionEnum });
            var opCenter = objectCache.GetOrNull("operating center", nvc);

            return container.GetInstance<RoleFactory>().Create(new {
                Application = app,
                Module = module,
                Action = action,
                OperatingCenter = opCenter,
                User = objectCache.GetValueOrDefault<UserFactory>("user", nvc)
            });
        }

        public static Role CreateWildcardRole(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var actionEnum = nvc.GetValueAs<RoleActions>("action");
            var moduleEnum = nvc.GetValueAs<RoleModules>("module");
            var appEnum = EnumExtensions.GetValues<RoleApplications>().Single(x => moduleEnum.ToString().StartsWith(x.ToString()));
            var app = container.GetInstance<ApplicationFactory>().Create(new { Id = appEnum });
            var module = container.GetInstance<ModuleFactory>().Create(new { Id = moduleEnum, Application = app });
            var action = container.GetInstance<ActionFactory>().Create(new { Id = actionEnum });

            return container.GetInstance<WildcardOpCenterRoleFactory>().Create(new {
                Application = app,
                Module = module,
                Action = action,
                User = objectCache.GetValueOrDefault<UserFactory>("user", nvc)
            });
        }

        public static SampleSite CreateSampleSite(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<SampleSiteFactory>().Create(new {
                State = objectCache.GetValueOrDefault<StateFactory>("state", nvc),
                Status = objectCache.GetValueOrDefault<SampleSiteStatusFactory>("sample site status", nvc),
                OperatingCenter = objectCache.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                Town = objectCache.GetValueOrDefault<TownFactory>("town", nvc),
                Premise = objectCache.GetValueOrDefault<PremiseFactory>("premise", nvc),
                Street = objectCache.GetValueOrDefault<StreetFactory>("street", nvc),
                Coordinate = objectCache.GetValueOrDefault<CoordinateFactory>("coordinate", nvc),
                PublicWaterSupply = objectCache.GetValueOrDefault<PublicWaterSupplyFactory>("public water supply", nvc),
                BactiSite = nvc.GetValueAs<bool>("bacti site").GetValueOrDefault(),
                CertifiedBy = objectCache.GetValueOrDefault<UserFactory>("user", "certified by", nvc),
                CertifiedDate = nvc.GetValueAs<DateTime>("certified date"),
                AgencyId = nvc.GetValueOrDefault("agency id"),
                LocationNameDescription = nvc.GetValueOrDefault("location name description"),
                LimsFacilityId = nvc.GetValueOrDefault("lims facility id"),
                LimsSiteId = nvc.GetValueOrDefault("lims site id"),
                LeadCopperSite = nvc.GetValueAs<bool>("lead copper site"),
                IsLimsLocation = nvc.GetValueAs<bool>("is lims location").GetValueOrDefault(),
                IsComplianceSampleSite = nvc.GetValueAs<bool>("is compliance sample site").GetValueOrDefault(),
                SampleSiteProfile = objectCache.GetValueOrDefault<SampleSiteProfileFactory>("sample site profile", nvc),
                Availability = objectCache.GetValueOrDefault<SampleSiteAvailabilityFactory>("sample site availability", nvc),
                LocationType = objectCache.GetValueOrDefault<SampleSiteLocationTypeFactory>("sample site location type", nvc),
                SampleSiteAddressLocationType = objectCache.GetValueOrDefault<SampleSiteAddressLocationTypeFactory>("sample site address location type", nvc),
                CollectionType = objectCache.GetValueOrDefault<SampleSiteCollectionTypeFactory>("sample site collection type", nvc)
            });
        }

        private static object CreateSampleSiteCustomerContactMethod(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            SampleSiteCustomerContactMethodFactory factory;

            switch (nvc["description"].ToLowerInvariant())
            {
                case "email":
                    factory = container.GetInstance<EmailSampleSiteCustomerContactMethodFactory>();
                    break;

                case "mail":
                    factory = container.GetInstance<MailSampleSiteCustomerContactMethodFactory>();
                    break;

                case "phone":
                    factory = container.GetInstance<PhoneSampleSiteCustomerContactMethodFactory>();
                    break;

                case "text":
                    factory = container.GetInstance<TextMessageSampleSiteCustomerContactMethodFactory>();
                    break;

                default:
                    throw new InvalidOperationException($"Not sure how to create sample site custom contact method from description string '{nvc["description"]}'");
            }

            return factory.Create();
        }

        private static object CreateSampleSiteLeadCopperTierClassification(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            SampleSiteLeadCopperTierClassificationFactory factory;

            switch (nvc["description"].ToLowerInvariant())
            {
                case "tier one":
                    factory = container.GetInstance<TierOneSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                case "tier two":
                    factory = container.GetInstance<TierTwoSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                case "tier three":
                    factory = container.GetInstance<TierThreeSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                case "other":
                    factory = container.GetInstance<OtherSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                case "tier one no text":
                    factory = container.GetInstance<TierOneNoTextSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                case "tier two no text":
                    factory = container.GetInstance<TierTwoNoTextSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                case "tier three no text":
                    factory = container.GetInstance<TierThreeNoTextSampleSiteLeadCopperTierClassificationFactory>();
                    break;

                default:
                    throw new InvalidOperationException($"Not sure how to create sample site lead copper tier classification from description string '{nvc["description"]}'");
            }

            return factory.Create();
        }

        private static object CreateSampleSiteLeadCopperValidationMethod(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            SampleSiteLeadCopperValidationMethodFactory factory;

            switch (nvc["description"].ToLowerInvariant())
            {
                case "visual confirmation":
                    factory = container.GetInstance<VisualConfirmationSampleSiteLeadCopperValidationMethodFactory>();
                    break;

                case "lead swab test":
                    factory = container.GetInstance<LeadSwapTestSampleSiteLeadCopperValidationMethodFactory>();
                    break;

                case "building construction document":
                    factory = container.GetInstance<BuildingConstructionDocumentSampleSiteLeadCopperValidationMethodFactory>();
                    break;

                case "customer survey results":
                    factory = container.GetInstance<CustomerSurveyResultsSampleSiteLeadCopperValidationMethodFactory>();
                    break;

                case "historic documentation":
                    factory = container.GetInstance<HistoricDocumentationSampleSiteLeadCopperValidationMethodFactory>();
                    break;

                default:
                    throw new InvalidOperationException($"Not sure how to create sample site lead copper validation method from description string '{nvc["description"]}'");
            }

            return factory.Create();
        }

        public static object CreateSampleSiteLocationType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            SampleSiteLocationTypeFactory factory;
            switch (nvc["description"].ToUpper())
            {
                case "PRIMARY":
                    factory = container.GetInstance<PrimarySampleSiteLocationTypeFactory>();
                    break;
                case "UPSTREAM":
                    factory = container.GetInstance<UpstreamSampleSiteLocationTypeFactory>();
                    break;
                case "DOWNSTREAM":
                    factory = container.GetInstance<DownstreamSampleSiteLocationTypeFactory>();
                    break;
                case "GROUNDWATER":
                    factory = container.GetInstance<GroundwaterSampleSiteLocationTypeFactory>();
                    break;
                default:
                    throw new NotImplementedException(
                        $"Creating sample site location type for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        public static object CreateSampleSiteInactivationReason(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            SampleSiteInactivationReasonFactory factory;
            switch (nvc["description"].ToLower())
            {
                case "customer declined program":
                    factory = container.GetInstance<CustomerDeclinedProgramSampleSiteInactivationReasonFactory>();
                    break;
                case "customer opted out":
                    factory = container.GetInstance<CustomerOptedOutSampleSiteInactivationReasonFactory>();
                    break;
                case "customer service line replaced":
                    factory = container.GetInstance<CustomerServiceLineReplacedSampleSiteInactivationReasonFactory>();
                    break;
                case "company service line replaced":
                    factory = container.GetInstance<CompanyServiceLineReplacedSampleSiteInactivationReasonFactory>();
                    break;
                case "internal plumbing replaced":
                    factory = container.GetInstance<InternalPlumbingReplacedSampleSiteInactivationReasonFactory>();
                    break;
                case "building demolished":
                    factory = container.GetInstance<BuildingDemolishedSampleSiteInactivationReasonFactory>();
                    break;
                case "other":
                    factory = container.GetInstance<OtherSampleSiteInactivationReasonFactory>();
                    break;
                case "new service details":
                    factory = container.GetInstance<NewServiceDetailsSampleSiteInactivationReasonFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating sample site inactivation reason for '{nvc["description"]}' has not yet been implemented.");
            }

            return factory.Create();
        }

        public static SampleSiteStatus CreateSampleSiteStatus(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "active":
                    return container.GetInstance<ActiveSampleSiteStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactiveSampleSiteStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingSampleSiteStatusFactory>().Create();
                case "archived - duplicate site":
                    return container.GetInstance<ArchivedDuplicateSiteSampleSiteStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create sample site status from description string '{nvc["description"]}'");
            }
        }

        public static EquipmentType CreateEquipmentType(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("You must provide a description to create an equipment type");
            }

            switch (nvc["description"].ToLowerInvariant())
            {
                case "generator":
                    return container.GetInstance<EquipmentTypeGeneratorFactory>().Create();
                case "engine":
                    return container.GetInstance<EquipmentTypeEngineFactory>().Create();
                case "aerator":
                    return container.GetInstance<EquipmentTypeAeratorFactory>().Create();
                case "et01 - aerator":
                    return container.GetInstance<EquipmentTypeAeratorFactory>().Create(new { Description = nvc["description"].ToUpper() });
                case "rtu":
                    return container.GetInstance<EquipmentTypeRTUFactory>().Create();
                case "fire suppression":
                    return container.GetInstance<EquipmentTypeFireSuppressionFactory>().Create();
                case "flow meter":
                    return container.GetInstance<EquipmentTypeFlowMeterFactory>().Create();
                case "tnk":
                    return container.GetInstance<EquipmentTypeTankFactory>().Create();
                case "filter":
                    return container.GetInstance<EquipmentTypeFactory>().Create();
                case "well":
                    return container.GetInstance<EquipmentTypeWellFactory>().Create();
                case "tnk-wpot":
                    return container.GetInstance<EquipmentTypeWaterTankFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create equipment type with description '{nvc["description"]}'. You probably need to add it to CreateEquipmentType in Data.cs");
            }
        }

        public static EquipmentManufacturer CreateEquipmentManufacturer(NameValueCollection nvc,
            TestObjectCache objectCache, IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("You must provide a description to create an equipment manufacturer");
            }

            EquipmentType equipType = null;

            switch (nvc["equipment type"].ToLowerInvariant())
            {
                case "generator":
                    equipType = container.GetInstance<EquipmentTypeGeneratorFactory>().Create();
                    break;
                case "engine":
                    equipType = container.GetInstance<EquipmentTypeEngineFactory>().Create();
                    break;
                case "aerator":
                    equipType = container.GetInstance<EquipmentTypeAeratorFactory>().Create();
                    break;
                case "flow meter":
                    equipType = container.GetInstance<EquipmentTypeFlowMeterFactory>().Create();
                    break;
                case "fire suppression":
                    equipType = container.GetInstance<EquipmentTypeFireSuppressionFactory>().Create();
                    break;
                case "tnk":
                    equipType = container.GetInstance<EquipmentTypeTankFactory>().Create();
                    break;
                case "filter":
                    equipType = container.GetInstance<EquipmentTypeFactory>().Create();
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unable to create equipment type with description '{nvc["equipment type"]}'. You probably need to add it to CreateEquipmentManufacturer in Data.cs");
            }

            var desc = nvc["description"].ToUpper();
            return container.GetInstance<EquipmentManufacturerFactory>().Create(new {
                EquipmentType = equipType, 
                Description = desc, 
                MapCallDescription = desc
            });
        }

        public static NearMissType CreateNearMissType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "safety":
                    return container.GetInstance<SafetyNearMissTypeFactory>().Create();
                case "environmental":
                    return container.GetInstance<EnvironmentalNearMissTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create near miss type from description string '{nvc["description"]}'");
            }
        }

        public static GeneralLiabilityClaimType CreateGeneralLiabilityClaimType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "preventable":
                    return container.GetInstance<PreventableGeneralLiabilityClaimTypeFactory>().Create();
                case "non-preventable":
                    return container.GetInstance<NonPreventableGeneralLiabilityClaimTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create claim type from description string '{nvc["description"]}'");
            }
        }

        public static NearMissCategory CreateNearMissCategory(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "other":
                    return container.GetInstance<OtherNearMissCategoryFactory>().Create();
                case "stormwater":
                    return container.GetInstance<StormWaterNearMissCategoryFactory>().Create();
                case "ergonomics":
                    return container.GetInstance<ErgonomicsNearMissCategoryFactory>().Create();

                default:
                    throw new InvalidOperationException($"Not sure how to create near miss category from description string '{nvc["description"]}'");
            }
        }

        public static Sensor CreateSensor(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<SensorFactory>().Create(new
            {
                Name = nvc["name"],
                Description = nvc["description"],
                Location = nvc["location"],
                MeasurementType = objectCache.GetValueOrDefault<SensorMeasurementTypeFactory>("sensor measurement type", nvc),
                Board = objectCache.GetValueOrDefault<BoardFactory>("board", nvc)
            });
        }

        public static Service CreateService(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var session = container.GetInstance<ISession>();
            Service service;
            DateTime? dateInstalled = null;

            var fromNvcDateInstalled = nvc["date installed"];
            if (!string.IsNullOrWhiteSpace(fromNvcDateInstalled))
            {
                dateInstalled = fromNvcDateInstalled.ToDateTime();
            }

            //
            // A service has both a premise number and a premise, instead of inferring hte premise number from the premise
            // If the developer supplied either, then create a service with a premise and premise number, else no premise/premise number
            //
            if (nvc.ContainsKey("premise number") || nvc.ContainsKey("premise"))
            {
                service = container.GetInstance<ServiceFactory>().Create(new {
                    ServiceCategory = obj.GetValueOrDefault<ServiceCategoryFactory>("service category", nvc),
                    ServiceSize = obj.GetValueOrDefault<ServiceSizeFactory>("service size", nvc),
                    Town = obj.GetValueOrDefault<TownFactory>("town", nvc),
                    TownSection = obj.GetValueOrDefault<TownSectionFactory>("town section", nvc),
                    DateInstalled = dateInstalled,
                    WorkIssuedTo = obj.GetValueOrDefault<ServiceRestorationContractorFactory>("service restoration contractor", "work issued to", nvc),
                    PremiseNumber = nvc["premise number"],
                    ServiceNumber = nvc.GetValueAs<long>("service number"),
                    CrossStreet = obj.GetValueOrDefault<StreetFactory>("street", "cross street", nvc),
                    ServiceMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", nvc),
                    Street = obj.GetValueOrDefault<StreetFactory>("street", nvc),
                    StreetNumber = nvc["street number"],
                    Zip = nvc["zip"],
                    OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                    Lot = nvc["lot"],
                    Block = nvc["block"],
                    ApartmentNumber = nvc["apartment number"],
                    PreviousServiceSize = obj.GetValueOrDefault<ServiceSizeFactory>("service size", "previous service size", nvc),
                    PreviousServiceMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "previous service material", nvc),
                    LengthOfService = nvc.GetValueAs<decimal>("length of service"),
                    Premise = obj.GetValueOrDefault<PremiseFactory>("premise", nvc),
                    RetiredDate = nvc.ContainsKey("date retired") ? nvc.GetValueAs<DateTime>("date retired") : (DateTime?)null,
                    Installation = nvc["installation"],
                    UpdatedAt = nvc.ContainsKey("last updated") ? nvc.GetValueAs<DateTime>("last updated") : (DateTime?)null,
                    MeterSettingSize = obj.GetValueOrDefault<ServiceSizeFactory>("service size", "meter setting size", nvc),
                    CustomerSideMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "customer side material", nvc),
                    CustomerSideSize = obj.GetValueOrDefault<ServiceSizeFactory>("service size", "customer side size", nvc),
                    SAPWorkOrderNumber = nvc.GetValueAs<long>("s a p work order number"),
                    SAPNotificationNumber = nvc.GetValueAs<long>("s a p notification number")
                });
            }
            else
            {
                service = container.GetInstance<ServiceFactory>().Create(new {
                    ServiceCategory = obj.GetValueOrDefault<ServiceCategoryFactory>("service category", nvc),
                    ServiceSize = obj.GetValueOrDefault<ServiceSizeFactory>("service size", nvc),
                    Town = obj.GetValueOrDefault<TownFactory>("town", nvc),
                    TownSection = obj.GetValueOrDefault<TownSectionFactory>("town section", nvc),
                    DateInstalled = dateInstalled,
                    WorkIssuedTo = obj.GetValueOrDefault<ServiceRestorationContractorFactory>("service restoration contractor", "work issued to", nvc),
                    ServiceNumber = nvc.GetValueAs<long>("service number"),
                    CrossStreet = obj.GetValueOrDefault<StreetFactory>("street", "cross street", nvc),
                    ServiceMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", nvc),
                    CustomerSideMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "customer side material", nvc),
                    Street = obj.GetValueOrDefault<StreetFactory>("street", nvc),
                    StreetNumber = nvc["street number"],
                    Zip = nvc["zip"],
                    OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                    Lot = nvc["lot"],
                    Block = nvc["block"],
                    ApartmentNumber = nvc["apartment number"],
                    PreviousServiceSize = obj.GetValueOrDefault<ServiceSizeFactory>("service size", "previous service size", nvc),
                    PreviousServiceMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "previous service material", nvc),
                    LengthOfService = nvc.GetValueAs<decimal>("length of service"),
                    PremiseUnavailableReason = obj.GetValueOrDefault<PremiseUnavailableReasonFactory>("premise unavailable reason", nvc),
                    PremiseNumberUnavailable = nvc.ContainsKey("premise number unavailable") ? nvc.GetValueAs<bool>("premise number unavailable") : (bool?)null,
                    RetiredDate = nvc.ContainsKey("date retired") ? nvc.GetValueAs<DateTime>("date retired") : (DateTime?)null,
                    Installation = nvc["installation"],
                    SAPWorkOrderNumber = nvc.GetValueAs<long>("s a p work order number"),
                    SAPNotificationNumber = nvc.GetValueAs<long>("s a p notification number")
                });
            }

            // Refresh is needed in order to get the ServiceType populated correctly. It's a sorta-formula-but-not column.
            session.Refresh(service);

            return service;
        }

        public static ServiceRestoration CreateServiceRestoration(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<ServiceRestorationFactory>().Create(new
            {
                InitiatedBy = obj.GetValueOrDefault<UserFactory>("user", "initiated by", nvc)
            });
        }

        public static ServiceCategory CreateServiceCategory(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<ServiceCategoryFactory>().Create(new
            {
                Description = nvc["description"]
            });
        }

        public static ServiceSize CreateServiceSize(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<ServiceSizeFactory>().Create(new
            {
                Size = nvc.GetValueAs<decimal>("size"),
                ServiceSizeDescription = nvc["service size description"],
                Main = nvc.GetValueAs<bool>("main"),
                Service = nvc.GetValueAs<bool>("service"),
                Meter = nvc.GetValueAs<bool>("meter")
            });
        }

        public static ServiceLineProtectionInvestigation CreateServiceLineProtectionInvestigation(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<ServiceLineProtectionInvestigationFactory>().Create(new
            {
                CustomerCity = obj.GetValueOrDefault<TownFactory>("town", "customer city", nvc),
                Street = obj.GetValueOrDefault<StreetFactory>("street", nvc),
                WorkType = obj.GetValueOrDefault<ServiceLineProtectionWorkTypeFactory>("service line protection work type", "work type", nvc),
                CustomerServiceSize = obj.GetValueOrDefault<ServiceSizeFactory>("customer service size", "service size", nvc),
                Contractor = obj.GetValueOrDefault<ContractorFactory>("contractor", nvc)
            });
        }

        public static Site CreateSite(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<SiteFactory>().Create(new
            {
                Name = nvc["name"],
                Project = objectCache.GetValueOrDefault<ProjectFactory>("project", nvc)
            });
        }

        public static MapCall.Common.Model.Entities.State CreateState(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<StateFactory>();

            if (nvc.ContainsKey("abbreviation"))
            {
                return factory.Create(new {
                    Name = nvc["name"],
                    Abbreviation = nvc["abbreviation"],
                    ScadaTable = nvc["scada table"]
                });
            }

            return factory.Create(new {
                Name = nvc["name"],
                ScadaTable = nvc["scada table"]
            });
        }

        public static Town CreateTown(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            if (nvc.ContainsKey("name"))
            {
                return container.GetInstance<TownFactory>().Create(new {
                    ShortName = nvc["name"],
                    County = objectCache.GetValueOrDefault<CountyFactory>("county", nvc)
                });
            }

            return container.GetInstance<TownFactory>().Create(new {
                County = objectCache.GetValueOrDefault<CountyFactory>("county", nvc)
            });
        }

        public static IncidentInvestigationRootCauseLevel2Type CreateIncidentInvestigationRootCauseLevel2Type(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<IncidentInvestigationRootCauseLevel2TypeFactory>().Create(new
            {
                Description = nvc["description"],
                IncidentInvestigationRootCauseLevel1Type = objectCache.GetValueOrDefault<IncidentInvestigationRootCauseLevel1TypeFactory>("incident investigation root cause level1 type", nvc)
            });
        }

        public static IncidentInvestigationRootCauseLevel3Type CreateIncidentInvestigationRootCauseLevel3Type(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<IncidentInvestigationRootCauseLevel3TypeFactory>().Create(new
            {
                Description = nvc["description"],
                IncidentInvestigationRootCauseLevel2Type = objectCache.GetValueOrDefault<IncidentInvestigationRootCauseLevel2TypeFactory>("incident investigation root cause level2 type", nvc)
            });
        }

        public static IncidentDrugAndAlcoholTestingDecision CreateTestingDecision(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<IncidentDrugAndAlcoholTestingDecisionFactory>().Create(new
            {
                Description = nvc["description"]
            });
        }

        public static IncidentDrugAndAlcoholTestingResult CreateTestingResult(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            return container.GetInstance<IncidentDrugAndAlcoholTestingResultFactory>().Create(new
            {
                Description = nvc["description"]
            });
        }

        public static User CreateUser(NameValueCollection nvc, TestObjectCache objectCache, IContainer container, bool isAdmin)
        {
            var factory = isAdmin ? container.GetInstance<AdminUserFactory>() : container.GetInstance<UserFactory>();
            var roleFactory = container.GetInstance<RoleFactory>();
            var email = nvc.ContainsKey("special_email")
                ? nvc["special_email"].PrependCurrentHostname()
                : nvc["email"];

            var employee = (Employee)objectCache.GetOrNull("employee", nvc);

            var user = factory.Create(new
            {
                Email = email,
                //EmployeeId = nvc["employee id"],
                Employee = employee,
                UserName = nvc["username"] ?? "some user",
               // Roles = roles,
                FullName = nvc["full name"],
                DefaultOperatingCenter = objectCache.GetValueOrDefault<OperatingCenterFactory>("operating center", "default operating center", nvc),
                //CustomerProfileId = nvc.GetValueAs<int>("customer profile id"),
                ProfileLastVerified =
                    nvc["profile last verified"] != null &&
                        nvc["profile last verified"] == "today"
                        ? DateTime.Now
                        : (DateTime?)null,
                CustomerProfileId =
                    nvc.ContainsKey("has profileId") &&
                        nvc["has profileId"] == "true"
                        ? Convert.ToInt32(
                            DependencyResolver.Current.GetService<IExtendedCustomerGateway>()
                                .CreateCustomer(email, "regression test user")
                                .ProfileID)
                        : (int?)null,
                IsUserAdmin = nvc.GetValueAs<bool>("is user admin")
            });
         
            // Roles must be created with the User set. Otherwise, due to the Cascade.AllDeleteOrphan()
            // on the User.Roles map, the roles are either deleted or otherwise never attached to the User.
            var roles =
                (nvc["roles"] ?? String.Empty).Split(";".ToCharArray())
                    .Where(roleName => !String.IsNullOrWhiteSpace(roleName))
                    .Select(roleName => objectCache.Lookup<Role>("role", roleName))
                    .Select(r => roleFactory.Create(new
                    {
                        Application = r.Module.Application,
                        r.Module,
                        r.Action,
                        r.OperatingCenter,
                        User = user
                    })).ToList();

            //if (employee != null)
            //{
            //    // In order for employees-filtered-by-user-role dropdowns to work, both the User and Employee records
            //    // must be linked together. For some reason, when trying to set employee.User from inside regression tests,
            //    // the Employee instance we receive from the objectCache is not the nhibernate session cached instance.
            //    // Because of that, we have to re-query for the employee, set its User, and then save that.
            //    //
            //    // If you try to save the Employee record from the objectCache, nhibernate will attempt to insert a new
            //    // instance and you'll get a unique constraint error related to the employee number.
            //    //
            //    // If you don't session.Save the Employee, the Employee record in the database will always have a null UserId.
            //    //
            //    // I think this may have to do with MagicalBuilderThingy creating the Employee but I'm really not sure.
            //    employee = session.QueryOver<Employee>().Where(x => x.Id == employee.Id).SingleOrDefault();
            //    employee.User = user;
            //    session.Save(employee);
            //}

            if (nvc.ContainsKey("credit card number"))
            {
                CreatePaymentProfile(user, nvc["credit card number"]);
            }

            return user;
        }

        #region Helper Methods

        private static void CreatePaymentProfile(User user, string cardNumber)
        {
            var expiration = DateTime.Now.AddMonths(1);
            DependencyResolver.Current.GetService<IExtendedCustomerGateway>()
                .AddCreditCard(user.CustomerProfileId.ToString(), cardNumber, expiration.Month, expiration.Year,
                    "123");
        }

        #endregion

        public static ValveBilling CreateValveBilling(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToUpper())
            {
                //PUBLIC = "Public", MUNICIPAL = "Municipal", O_AND_M = "O & M", COMPANY = "Company";
                case "PUBLIC": // ((string)ValveBilling.PUBLIC).ToUpper():
                    return container.GetInstance<PublicValveBillingFactory>().Create();
                case "COMPANY": // ValveBilling.COMPANY:
                    return container.GetInstance<CompanyValveBillingFactory>().Create();
                case "MUNICIPAL": // ValveBilling.MUNICIPAL:
                    return container.GetInstance<MunicipalValveBillingFactory>().Create();
                case "O & M": // ValveBilling.O_AND_M:
                    return container.GetInstance<OAndMValveBillingFactory>().Create();
                default:
                    throw new InvalidOperationException("You need to use the actual name here, not one or two.");
            }
        }

        public static ValveControl CreateValveControl(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            ValveControlFactory factory;

            switch (nvc["description"])
            {
                case ValveControl.BLOW_OFF_WITH_FLUSHING:
                    factory = container.GetInstance<BlowOffWithFlushingValveControlFactory>();
                    break;
                case "Foo":
                    factory = container.GetInstance<FooValveControlFactory>();
                    break;
                case "Bar":
                    factory = container.GetInstance<BarValveControlFactory>();
                    break;
                default:
                    throw new NotImplementedException($"Creating valve control with description '{nvc["description"]}' has not been implemented");
            }

            return factory.Create();
        }

        public static ValveInspection CreateValveInspection(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var inspection = container.GetInstance<ValveInspectionFactory>().Create(new
            {
                Valve = obj.GetValueOrDefault<ValveFactory>("valve", nvc),
                MinimumRequiredTurns = nvc.GetValueAs<decimal>("minimum required turns"),
                DateInspected = nvc.GetValueAs<DateTime>("date inspected"),
                Inspected = nvc["inspected"] == "true",
                InspectedBy = obj.GetValueOrDefault<UserFactory>("user", "inspected by", nvc)
            });
            return inspection;
        }

        public static AssetStatus CreateValveStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToUpper())
            {
                case AssetStatus.ACTIVE:
                    return container.GetInstance<ActiveAssetStatusFactory>().Create(new
                    {
                        IsUserAdminOnly = nvc.GetValueAs<bool>("is user admin only").GetValueOrDefault(true)
                    });
                case AssetStatus.RETIRED:
                    return container.GetInstance<RetiredAssetStatusFactory>().Create(new
                    {
                        IsUserAdminOnly = nvc.GetValueAs<bool>("is user admin only").GetValueOrDefault(true)
                    });
                case AssetStatus.PENDING:
                    return container.GetInstance<PendingAssetStatusFactory>().Create(new
                    {
                        IsUserAdminOnly = nvc.GetValueAs<bool>("is user admin only").GetValueOrDefault(true)
                    });
                case AssetStatus.INACTIVE:
                    return container.GetInstance<InactiveAssetStatusFactory>().Create(new
                    {
                        IsUserAdminOnly = nvc.GetValueAs<bool>("is user admin only").GetValueOrDefault(true)
                    });
                default:
                    throw new InvalidOperationException("You need to use the actual name here , not one or two.");
            }
        }

        public static WeatherCondition CreateWeatherCondition(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "dry":
                    return container.GetInstance<WeatherConditionDryFactory>().Create();
                case "raining":
                    return container.GetInstance<WeatherConditionRainingFactory>().Create();
                case "snowing":
                    return container.GetInstance<WeatherConditionSnowingFactory>().Create();

                default:
                    throw new InvalidOperationException($"Not sure how to create weather condition from description string '{nvc["description"]}'");
            }
        }

        public static WorkDescription CreateWorkDescription(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException(
                    "To create a work description you must provide the required description as an attribute (i.e. a work description \"blah\" exists with description: \"hydrant repair\").");
            }

            switch (nvc["description"].ToLowerInvariant())
            {
                case "water main bleeders":
                    return container.GetInstance<WaterMainBleedersWorkDescriptionFactory>().Create();
                case "change burst meter":
                    return container.GetInstance<ChangeBurstMeterWorkDescriptionFactory>().Create();
                case "check no water":
                    return container.GetInstance<CheckNoWaterWorkDescriptionFactory>().Create();
                case "curb box repair":
                    return container.GetInstance<CurbBoxRepairWorkDescriptionFactory>().Create();
                case "ball curb stop repair":
                    return container.GetInstance<BallCurbStopRepairWorkDescriptionFactory>().Create();
                case "excavate meter box setter":
                    return container.GetInstance<ExcavateMeterBoxSetterWorkDescriptionFactory>().Create();
                case "service line flow test":
                    return container.GetInstance<ServiceLineFlowTestWorkDescriptionFactory>().Create();
                case "hydrant frozen":
                    return container.GetInstance<HydrantFrozenWorkDescriptionFactory>().Create();
                case "frozen meter set":
                    return container.GetInstance<FrozenMeterSetWorkDescriptionFactory>().Create();
                case "frozen service line company side":
                    return container.GetInstance<FrozenServiceLineCompanySideWorkDescriptionFactory>().Create();
                case "frozen service line cust side":
                    return container.GetInstance<FrozenServiceLineCustSideWorkDescriptionFactory>().Create();
                case "ground water service":
                    return container.GetInstance<GroundWaterServiceWorkDescriptionFactory>().Create();
                case "hydrant flushing":
                    return container.GetInstance<HydrantFlushingWorkDescriptionFactory>().Create();
                case "hydrant investigation":
                    return container.GetInstance<HydrantInvestigationWorkDescriptionFactory>().Create();
                case "hydrant installation":
                    return container.GetInstance<HydrantInstallationWorkDescriptionFactory>().Create();
                case "hydrant leaking":
                    return container.GetInstance<HydrantLeakingWorkDescriptionFactory>().Create();
                case "hydrant no drip":
                    return container.GetInstance<HydrantNoDripWorkDescriptionFactory>().Create();
                case "hydrant repair":
                    return container.GetInstance<HydrantRepairWorkDescriptionFactory>().Create();
                case "hydrant replacement":
                    return container.GetInstance<HydrantReplacementWorkDescriptionFactory>().Create();
                case "hydrant retirement":
                    return container.GetInstance<HydrantRetirementWorkDescriptionFactory>().Create();
                case "inactive account":
                    return container.GetInstance<InactiveAccountWorkDescriptionFactory>().Create();
                case "valve blow off installation":
                    return container.GetInstance<ValveBlowOffInstallationWorkDescriptionFactory>().Create();
                case "fire service installation":
                    return container.GetInstance<FireServiceInstallationWorkDescriptionFactory>().Create();
                case "install line stopper":
                    return container.GetInstance<InstallLineStopperWorkDescriptionFactory>().Create();
                case "install meter":
                    return container.GetInstance<InstallMeterWorkDescriptionFactory>().Create();
                case "interior setting repair":
                    return container.GetInstance<InteriorSettingRepairWorkDescriptionFactory>().Create();
                case "service investigation":
                    return container.GetInstance<ServiceInvestigationWorkDescriptionFactory>().Create();
                case "main investigation":
                    return container.GetInstance<MainInvestigationWorkDescriptionFactory>().Create();
                case "leak in meter box inlet":
                    return container.GetInstance<LeakInMeterBoxInletWorkDescriptionFactory>().Create();
                case "leak in meter box outlet":
                    return container.GetInstance<LeakInMeterBoxOutletWorkDescriptionFactory>().Create();
                case "leak survey":
                    return container.GetInstance<LeakSurveyWorkDescriptionFactory>().Create();
                case "meter box setter installation":
                    return container.GetInstance<MeterBoxSetterInstallationWorkDescriptionFactory>().Create();
                case "meter change":
                    return container.GetInstance<MeterChangeWorkDescriptionFactory>().Create();
                case "meter box adjustment resetter":
                    return container.GetInstance<MeterBoxAdjustmentResetterWorkDescriptionFactory>().Create();
                case "new main flushing":
                    return container.GetInstance<NewMainFlushingWorkDescriptionFactory>().Create();
                case "service line installation":
                    return container.GetInstance<ServiceLineInstallationWorkDescriptionFactory>().Create();
                case "service line leak cust side":
                    return container.GetInstance<ServiceLineLeakCustSideWorkDescriptionFactory>().Create();
                case "service line renewal":
                    return container.GetInstance<ServiceLineRenewalWorkDescriptionFactory>().Create();
                case "service line retire":
                    return container.GetInstance<ServiceLineRetireWorkDescriptionFactory>().Create();
                case "sump pump":
                    return container.GetInstance<SumpPumpWorkDescriptionFactory>().Create();
                case "test shut down":
                    return container.GetInstance<TestShutDownWorkDescriptionFactory>().Create();
                case "valve box repair":
                    return container.GetInstance<ValveBoxRepairWorkDescriptionFactory>().Create();
                case "valve box blow off repair":
                    return container.GetInstance<ValveBoxBlowOffRepairWorkDescriptionFactory>().Create();
                case "service line valve box repair":
                    return container.GetInstance<ServiceLineValveBoxRepairWorkDescriptionFactory>().Create();
                case "valve investigation":
                    return container.GetInstance<ValveInvestigationWorkDescriptionFactory>().Create();
                case "valve leaking":
                    return container.GetInstance<ValveLeakingWorkDescriptionFactory>().Create();
                case "valve repair":
                    return container.GetInstance<ValveRepairWorkDescriptionFactory>().Create();
                case "valve blow off repair":
                    return container.GetInstance<ValveBlowOffRepairWorkDescriptionFactory>().Create();
                case "valve replacement":
                    return container.GetInstance<ValveReplacementWorkDescriptionFactory>().Create();
                case "valve retirement":
                    return container.GetInstance<ValveRetirementWorkDescriptionFactory>().Create();
                case "water ban restriction violator":
                    return container.GetInstance<WaterBanRestrictionViolatorWorkDescriptionFactory>().Create();
                case "water main break repair":
                    return container.GetInstance<WaterMainBreakRepairWorkDescriptionFactory>().Create();
                case "water main installation":
                    return container.GetInstance<WaterMainInstallationWorkDescriptionFactory>().Create();
                case "water main retirement":
                    return container.GetInstance<WaterMainRetirementWorkDescriptionFactory>().Create();
                case "flushing service":
                    return container.GetInstance<FlushingServiceWorkDescriptionFactory>().Create();
                case "water main break replace":
                    return container.GetInstance<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
                case "meter box setter replace":
                    return container.GetInstance<MeterBoxSetterReplaceWorkDescriptionFactory>().Create();
                case "sewer main break repair":
                    return container.GetInstance<SewerMainBreakRepairWorkDescriptionFactory>().Create();
                case "sewer main break replace":
                    return container.GetInstance<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
                case "sewer main retirement":
                    return container.GetInstance<SewerMainRetirementWorkDescriptionFactory>().Create();
                case "sewer main installation":
                    return container.GetInstance<SewerMainInstallationWorkDescriptionFactory>().Create();
                case "sewer main cleaning":
                    return container.GetInstance<SewerMainCleaningWorkDescriptionFactory>().Create();
                case "sewer lateral installation":
                    return container.GetInstance<SewerLateralInstallationWorkDescriptionFactory>().Create();
                case "sewer lateral repair":
                    return container.GetInstance<SewerLateralRepairWorkDescriptionFactory>().Create();
                case "sewer lateral replace":
                    return container.GetInstance<SewerLateralReplaceWorkDescriptionFactory>().Create();
                case "sewer lateral retire":
                    return container.GetInstance<SewerLateralRetireWorkDescriptionFactory>().Create();
                case "sewer lateral customer side":
                    return container.GetInstance<SewerLateralCustomerSideWorkDescriptionFactory>().Create();
                case "sewer opening repair":
                    return container.GetInstance<SewerOpeningRepairWorkDescriptionFactory>().Create();
                case "sewer opening replace":
                    return container.GetInstance<SewerOpeningReplaceWorkDescriptionFactory>().Create();
                case "sewer opening installation":
                    return container.GetInstance<SewerOpeningInstallationWorkDescriptionFactory>().Create();
                case "sewer main overflow":
                    return container.GetInstance<SewerMainOverflowWorkDescriptionFactory>().Create();
                case "sewer backup company side":
                    return container.GetInstance<SewerBackupCompanySideWorkDescriptionFactory>().Create();
                case "hydraulic flow test":
                    return container.GetInstance<HydraulicFlowTestWorkDescriptionFactory>().Create();
                case "markout crew":
                    return container.GetInstance<MarkoutCrewWorkDescriptionFactory>().Create();
                case "valve box replacement":
                    return container.GetInstance<ValveBoxReplacementWorkDescriptionFactory>().Create();
                case "site inspection survey new service":
                    return container.GetInstance<SiteInspectionSurveyNewServiceWorkDescriptionFactory>().Create();
                case "site inspection survey service renewal":
                    return container.GetInstance<SiteInspectionSurveyServiceRenewalWorkDescriptionFactory>().Create();
                case "service line repair":
                    return container.GetInstance<ServiceLineRepairWorkDescriptionFactory>().Create();
                case "sewer clean out installation":
                    return container.GetInstance<SewerCleanOutInstallationWorkDescriptionFactory>().Create();
                case "sewer clean out repair":
                    return container.GetInstance<SewerCleanOutRepairWorkDescriptionFactory>().Create();
                case "sewer camera service":
                    return container.GetInstance<SewerCameraServiceWorkDescriptionFactory>().Create();
                case "sewer camera main":
                    return container.GetInstance<SewerCameraMainWorkDescriptionFactory>().Create();
                case "sewer demolition inspection":
                    return container.GetInstance<SewerDemolitionInspectionWorkDescriptionFactory>().Create();
                case "sewer main test holes":
                    return container.GetInstance<SewerMainTestHolesWorkDescriptionFactory>().Create();
                case "water main test holes":
                    return container.GetInstance<WaterMainTestHolesWorkDescriptionFactory>().Create();
                case "valve broken":
                    return container.GetInstance<ValveBrokenWorkDescriptionFactory>().Create();
                case "ground water main":
                    return container.GetInstance<GroundWaterMainWorkDescriptionFactory>().Create();
                case "service turn on":
                    return container.GetInstance<ServiceTurnonWorkDescriptionFactory>().Create();
                case "service turn off":
                    return container.GetInstance<ServiceTurnOffWorkDescriptionFactory>().Create();
                case "meter obtain read":
                    return container.GetInstance<MeterObtainReadWorkDescriptionFactory>().Create();
                case "meter final start read":
                    return container.GetInstance<MeterFinalStartReadWorkDescriptionFactory>().Create();
                case "meter repair touch pad":
                    return container.GetInstance<MeterRepairTouchPadWorkDescriptionFactory>().Create();
                case "valve installation":
                    return container.GetInstance<ValveInstallationWorkDescriptionFactory>().Create();
                case "valve blow off replacement":
                    return container.GetInstance<ValveBlowOffReplacementWorkDescriptionFactory>().Create();
                case "hydrant paint":
                    return container.GetInstance<HydrantPaintWorkDescriptionFactory>().Create();
                case "ball curb stop replace":
                    return container.GetInstance<BallCurbStopReplaceWorkDescriptionFactory>().Create();
                case "valve blow off retirement":
                    return container.GetInstance<ValveBlowOffRetirementWorkDescriptionFactory>().Create();
                case "valve blow off broken":
                    return container.GetInstance<ValveBlowOffBrokenWorkDescriptionFactory>().Create();
                case "water main relocation":
                    return container.GetInstance<WaterMainRelocationWorkDescriptionFactory>().Create();
                case "hydrant relocation":
                    return container.GetInstance<HydrantRelocationWorkDescriptionFactory>().Create();
                case "service relocation":
                    return container.GetInstance<ServiceRelocationWorkDescriptionFactory>().Create();
                case "sewer investigation main":
                    return container.GetInstance<SewerInvestigationMainWorkDescriptionFactory>().Create();
                case "sewer service overflow":
                    return container.GetInstance<SewerServiceOverflowWorkDescriptionFactory>().Create();
                case "sewer investigation lateral":
                    return container.GetInstance<SewerInvestigationLateralWorkDescriptionFactory>().Create();
                case "sewer investigation opening":
                    return container.GetInstance<SewerInvestigationOpeningWorkDescriptionFactory>().Create();
                case "sewer lift station repair":
                    return container.GetInstance<SewerLiftStationRepairWorkDescriptionFactory>().Create();
                case "curb box replace":
                    return container.GetInstance<CurbBoxReplaceWorkDescriptionFactory>().Create();
                case "service line valve box replace":
                    return container.GetInstance<ServiceLineValveBoxReplaceWorkDescriptionFactory>().Create();
                case "storm catch repair":
                    return container.GetInstance<StormCatchRepairWorkDescriptionFactory>().Create();
                case "storm catch replace":
                    return container.GetInstance<StormCatchReplaceWorkDescriptionFactory>().Create();
                case "storm catch installation":
                    return container.GetInstance<StormCatchInstallationWorkDescriptionFactory>().Create();
                case "storm catch investigation":
                    return container.GetInstance<StormCatchInvestigationWorkDescriptionFactory>().Create();
                case "hydrant landscaping":
                    return container.GetInstance<HydrantLandscapingWorkDescriptionFactory>().Create();
                case "hydrant restoration investigation":
                    return container.GetInstance<HydrantRestorationInvestigationWorkDescriptionFactory>().Create();
                case "hydrant restoration repair":
                    return container.GetInstance<HydrantRestorationRepairWorkDescriptionFactory>().Create();
                case "main landscaping":
                    return container.GetInstance<MainLandscapingWorkDescriptionFactory>().Create();
                case "main restoration investigation":
                    return container.GetInstance<MainRestorationInvestigationWorkDescriptionFactory>().Create();
                case "main restoration repair":
                    return container.GetInstance<MainRestorationRepairWorkDescriptionFactory>().Create();
                case "service landscaping":
                    return container.GetInstance<ServiceLandscapingWorkDescriptionFactory>().Create();
                case "service restoration investigation":
                    return container.GetInstance<ServiceRestorationInvestigationWorkDescriptionFactory>().Create();
                case "service restoration repair":
                    return container.GetInstance<ServiceRestorationRepairWorkDescriptionFactory>().Create();
                case "sewer lateral landscaping":
                    return container.GetInstance<SewerLateralLandscapingWorkDescriptionFactory>().Create();
                case "sewer lateral restoration investigation":
                    return container.GetInstance<SewerLateralRestorationInvestigationWorkDescriptionFactory>().Create();
                case "sewer lateral restoration repair":
                    return container.GetInstance<SewerLateralRestorationRepairWorkDescriptionFactory>().Create();
                case "sewer main landscaping":
                    return container.GetInstance<SewerMainLandscapingWorkDescriptionFactory>().Create();
                case "sewer main restoration investigation":
                    return container.GetInstance<SewerMainRestorationInvestigationWorkDescriptionFactory>().Create();
                case "sewer main restoration repair":
                    return container.GetInstance<SewerMainRestorationRepairWorkDescriptionFactory>().Create();
                case "sewer opening landscaping":
                    return container.GetInstance<SewerOpeningLandscapingWorkDescriptionFactory>().Create();
                case "sewer opening restoration investigation":
                    return container.GetInstance<SewerOpeningRestorationInvestigationWorkDescriptionFactory>().Create();
                case "sewer opening restoration repair":
                    return container.GetInstance<SewerOpeningRestorationRepairWorkDescriptionFactory>().Create();
                case "valve landscaping":
                    return container.GetInstance<ValveLandscapingWorkDescriptionFactory>().Create();
                case "valve restoration investigation":
                    return container.GetInstance<ValveRestorationInvestigationWorkDescriptionFactory>().Create();
                case "valve restoration repair":
                    return container.GetInstance<ValveRestorationRepairWorkDescriptionFactory>().Create();
                case "storm catch landscaping":
                    return container.GetInstance<StormCatchLandscapingWorkDescriptionFactory>().Create();
                case "storm catch restoration investigation":
                    return container.GetInstance<StormCatchRestorationInvestigationWorkDescriptionFactory>().Create();
                case "storm catch restoration repair":
                    return container.GetInstance<StormCatchRestorationRepairWorkDescriptionFactory>().Create();
                case "rstrn restoration inquiry":
                    return container.GetInstance<RstrnRestorationInquiryWorkDescriptionFactory>().Create();
                case "service off at main storm restoration":
                    return container.GetInstance<ServiceOffAtMainStormRestorationWorkDescriptionFactory>().Create();
                case "service off at curb stop storm restoration":
                    return container.GetInstance<ServiceOffAtCurbStopStormRestorationWorkDescriptionFactory>().Create();
                case "service off at meter pit storm restoration":
                    return container.GetInstance<ServiceOffAtMeterPitStormRestorationWorkDescriptionFactory>().Create();
                case "valve turned off storm restoration":
                    return container.GetInstance<ValveTurnedOffStormRestorationWorkDescriptionFactory>().Create();
                case "main repair storm restoration":
                    return container.GetInstance<MainRepairStormRestorationWorkDescriptionFactory>().Create();
                case "main replace storm restoration":
                    return container.GetInstance<MainReplaceStormRestorationWorkDescriptionFactory>().Create();
                case "hydrant turned off storm restoration":
                    return container.GetInstance<HydrantTurnedOffStormRestorationWorkDescriptionFactory>().Create();
                case "hydrant replace storm restoration":
                    return container.GetInstance<HydrantReplaceStormRestorationWorkDescriptionFactory>().Create();
                case "valve installation storm restoration":
                    return container.GetInstance<ValveInstallationStormRestorationWorkDescriptionFactory>().Create();
                case "valve replacement storm restoration":
                    return container.GetInstance<ValveReplacementStormRestorationWorkDescriptionFactory>().Create();
                case "curb box locate storm restoration":
                    return container.GetInstance<CurbBoxLocateStormRestorationWorkDescriptionFactory>().Create();
                case "meter pit locate storm restoration":
                    return container.GetInstance<MeterPitLocateStormRestorationWorkDescriptionFactory>().Create();
                case "valve retirement storm restoration":
                    return container.GetInstance<ValveRetirementStormRestorationWorkDescriptionFactory>().Create();
                case "excavate meter pit  storm restoration":
                    return container.GetInstance<ExcavateMeterPitStormRestorationWorkDescriptionFactory>().Create();
                case "service line renewal storm restoration":
                    return container.GetInstance<ServiceLineRenewalStormRestorationWorkDescriptionFactory>().Create();
                case "curb box replacement storm restoration":
                    return container.GetInstance<CurbBoxReplacementStormRestorationWorkDescriptionFactory>().Create();
                case "water main retirement storm restoration":
                    return container.GetInstance<WaterMainRetirementStormRestorationWorkDescriptionFactory>().Create();
                case "service line retirement storm restoration":
                    return container.GetInstance<ServiceLineRetirementStormRestorationWorkDescriptionFactory>().Create();
                case "frame and cover replace storm restoration":
                    return container.GetInstance<FrameAndCoverReplaceStormRestorationWorkDescriptionFactory>().Create();
                case "pump repair":
                    return container.GetInstance<PumpRepairWorkDescriptionFactory>().Create();
                case "line stop repair":
                    return container.GetInstance<LineStopRepairWorkDescriptionFactory>().Create();
                case "saw repair":
                    return container.GetInstance<SawRepairWorkDescriptionFactory>().Create();
                case "vehicle repair":
                    return container.GetInstance<VehicleRepairWorkDescriptionFactory>().Create();
                case "misc repair":
                    return container.GetInstance<MiscRepairWorkDescriptionFactory>().Create();
                case "z lwc ew4 3 consecutive mths of 0 usage zero":
                    return container.GetInstance<ZLwcEw43ConsecutiveMthsOf0UsageZeroWorkDescriptionFactory>().Create();
                case "z lwc ew4 check meter non emergency ckmtr":
                    return container.GetInstance<ZLwcEw4CheckMeterNonEmergencyCkmtrWorkDescriptionFactory>().Create();
                case "z lwc ew4 demolition closed account democ":
                    return container.GetInstance<ZLwcEw4DemolitionClosedAccountDemocWorkDescriptionFactory>().Create();
                case "z lwc ew4 meter change out mtrch":
                    return container.GetInstance<ZLwcEw4MeterChangeOutMtrchWorkDescriptionFactory>().Create();
                case "z lwc ew4 read mr edit local ops only mredt":
                    return container.GetInstance<ZLwcEw4ReadMrEditLocalOpsOnlyMredtWorkDescriptionFactory>().Create();
                case "z lwc ew4 read to stop estimate est":
                    return container.GetInstance<ZLwcEw4ReadToStopEstimateEstWorkDescriptionFactory>().Create();
                case "z lwc ew4 repair install reading device rem":
                    return container.GetInstance<ZLwcEw4RepairInstallReadingDeviceRemWorkDescriptionFactory>().Create();
                case "z lwc ew4 reread and or inspect for leak hilow":
                    return container.GetInstance<ZLwcEw4RereadAndOrInspectForLeakHilowWorkDescriptionFactory>().Create();
                case "z lwc ew4 set meter turn on and read onset":
                    return container.GetInstance<ZLwcEw4SetMeterTurnOnAndReadOnsetWorkDescriptionFactory>().Create();
                case "z lwc ew4 turn on water on":
                    return container.GetInstance<ZLwcEw4TurnOnWateronWorkDescriptionFactory>().Create();
                case "hydrant nozzle replacement":
                    return container.GetInstance<HydrantNozzleReplacementWorkDescriptionFactory>().Create();
                case "service line installation partial":
                    return container.GetInstance<ServiceLineInstallationPartialWorkDescriptionFactory>().Create();
                case "service line installation complete partial":
                    return container.GetInstance<ServiceLineInstallationCompletePartialWorkDescriptionFactory>().Create();
                case "hydrant nozzle investigation":
                    return container.GetInstance<HydrantNozzleInvestigationWorkDescriptionFactory>().Create();
                case "crossing investigation":
                    return container.GetInstance<CrossingInvestigationWorkDescriptionFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create work description with description '{nvc["description"]}'.");
            }
        }

        public static WorkOrder CreateWorkOrder(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<WorkOrderFactory>();
            WorkDescription workDescription = null;
            AssetType assetType = null;
            MarkoutRequirement markoutRequirement = null;
            WorkOrder model;

            if (nvc["work description"] != string.Empty)
            {
                if (nvc["work description"] == "repair")
                    workDescription = (WorkDescription)obj.GetValueOrDefault<WaterMainBreakRepairWorkDescriptionFactory>("work description", nvc);
                else if (nvc["work description"] == "hydrant")
                    workDescription = (WorkDescription)obj.GetValueOrDefault<HydrantRepairWorkDescriptionFactory>("work description", nvc);
                else if (nvc["work description"] == "replace")
                    workDescription =
                        (WorkDescription)obj.GetValueOrDefault<WaterMainBreakReplaceWorkDescriptionFactory>(
                            "work description", nvc);
                else if (nvc["work description"] != null)
                    workDescription = (WorkDescription)obj.GetOrNull("work description", nvc);
            }

            if (nvc["markout requirement"] != null)
            {
                markoutRequirement = (MarkoutRequirement)obj.GetValueOrDefault<MarkoutRequirementFactory>("markout requirement", nvc);
            }
            if (nvc["asset type"] != null)
            {
                switch (nvc["asset type"])
                {
                    case "hydrant":
                        assetType = (AssetType)obj
                           .GetValueOrDefault<HydrantAssetTypeFactory>("asset type", nvc);
                        break;
                    case "service":
                        assetType = (AssetType)obj
                           .GetValueOrDefault<ServiceAssetTypeFactory>("asset type", nvc);
                        break;
                    case "sewer opening":
                        assetType = (AssetType)obj
                            .GetValueOrDefault<SewerOpeningAssetTypeFactory>("asset type", nvc);
                        break;
                    case "valve":
                    default:
                        assetType = (AssetType)obj
                           .GetValueOrDefault<ValveAssetTypeFactory>("asset type", nvc);
                        break;
                }

                model = factory.Create(new {
                    AssetType = assetType,
                    ApprovedBy = obj.GetOrNull("user", "approved by", nvc),
                    ApartmentAddtl = nvc["apartment addtl"],
                    AlertID = nvc["alert id"],
                    AlertIssued = nvc.ContainsKey("alert issued") ? nvc.GetValueAs<bool>("alert issued") : null,
                    AnticipatedRepairTime = obj.GetValueOrDefault<FourToSixRepairTimeRangeFactory>("anticipated repair time", nvc),
                    ApprovedOn = nvc.GetValueAs<DateTime>("approved on"),
                    AssignedContractor = nvc.ContainsKey("assigned contractor") ? obj.GetValueOrDefault<ContractorFactory>("contractor", "assigned contractor", nvc) : null,
                    AssignedToContractorOn = nvc.GetValueAs<DateTime>("assigned to contractor on"),
                    BusinessUnit = nvc["business unit"],
                    CancelledAt = nvc.ContainsKey("cancelled at") ? DateTime.Now : (DateTime?)null,
                    CancelledBy = nvc.ContainsKey("cancelled by") ? obj.Lookup("user", nvc["cancelled by"]) : null,
                    WorkOrderCancellationReason = nvc.ContainsKey("cancellation reason") ? obj.Lookup("work order cancellation reason", nvc["cancellation reason"]) : null,
                    CreatedBy = obj.GetValueOrDefault<UserFactory>("user", "created by", nvc),
                    CompletedBy = obj.GetValueOrDefault<UserFactory>("user", "completed by", nvc),
                    CustomerName = nvc["customer name"],
                    CustomerServiceLineMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "customer service material", nvc),
                    CompanyServiceLineMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "company service material", nvc),
                    DateCompleted = nvc.GetValueAs<DateTime>("date completed"),
                    DistanceFromCrossStreet = nvc.GetValueAs<double>("distance from cross street"),
                    LostWater = nvc.GetValueAs<int>("lost water"),
                    EstimatedCustomerImpact = obj.GetValueOrDefault<ZeroToFiftyCustomerImpactRangeFactory>("estimated customer impact", nvc),
                    Hydrant = assetType.Id == AssetType.Indices.HYDRANT
                        ? obj.GetValueOrDefault<HydrantFactory>("hydrant", nvc)
                        : obj.GetOrNull("hydrant", nvc),
                    MarkoutRequirement = markoutRequirement,
                    MaterialsDocID = nvc["materials doc id"],
                    MaterialPostingDate = nvc.GetValueAs<DateTime>("material posting date"),
                    MaterialsApprovedBy = obj.GetOrNull("user", "materials approved by", nvc),
                    MaterialsApprovedOn = nvc.GetValueAs<DateTime>("materials approved on"),
                    MaterialPlanningCompletedOn = nvc.GetValueAs<DateTime>("material planning completed on"),
                    NearestCrossStreet = obj.GetValueOrDefault<StreetFactory>("street", "nearest cross street", nvc),
                    OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                    PremiseNumber = nvc["premise number"],
                    Priority = obj.GetValueOrDefault<RoutineWorkOrderPriorityFactory>("work order priority", nvc),
                    RequestedBy = obj.GetValueOrDefault<WorkOrderRequesterFactory>("work order requester", nvc),
                    SAPErrorCode = nvc["s a p error code"],
                    SAPNotificationNumber = nvc.GetValueAs<Int64>("sap notification number"),
                    SAPWorkOrderNumber = nvc.GetValueAs<Int64>("sap work order number"),
                    Street = obj.GetValueOrDefault<StreetFactory>("street", nvc),
                    StreetOpeningPermitRequired = nvc.ContainsKey("s o p required")
                        ? nvc.GetValueAs<bool>("s o p required")
                        : null,
                    Service = obj.GetOrNull("service", nvc),
                    SecondaryPhoneNumber = nvc["secondary phone number"],
                    SewerOpening = assetType.Id == AssetType.Indices.SEWER_OPENING
                        ? obj.GetValueOrDefault<SewerOpeningFactory>("sewer opening", nvc)
                        : obj.GetOrNull("sewer opening", nvc),
                    SignificantTrafficImpact = nvc.ContainsKey("significant traffic impact") ? nvc.GetValueAs<bool>("significant traffic impact") : null,
                    Town = obj.GetValueOrDefault<TownFactory>("town", nvc),
                    TownSection = obj.GetValueOrDefault<TownSectionFactory>("town section", nvc),
                    Valve = assetType.Id == AssetType.Indices.VALVE
                        ? obj.GetValueOrDefault<ValveFactory>("valve", nvc)
                        : obj.GetOrNull("valve", nvc),
                    WorkDescription = workDescription,
                    ZipCode = nvc["zip code"],
                    HasPitcherFilterBeenProvidedToCustomer = nvc.GetValueAs<bool>("has pitcher filter been provided to customer"),
                    DatePitcherFilterDeliveredToCustomer = nvc.GetValueAs<DateTime>("date pitcher filter delivered to customer"),
                    MarkoutTypeNeeded = obj.GetValueOrDefault<MarkoutTypeFactory>("markout type", "markout type needed", nvc),
                    RequiredMarkoutNote = nvc["required markout note"],
                    Premise = obj.GetValueOrDefault<PremiseFactory>("premise", nvc)
                });
            }
            else
            {
                model = factory.Create(new {
                    WorkDescription = workDescription,
                    SAPNotificationNumber = nvc.GetValueAs<Int64>("sap notification number"),
                    SAPWorkOrderNumber = nvc.GetValueAs<Int64>("sap work order number"),
                    OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                    BusinessUnit = nvc["business unit"],
                    DateCompleted = nvc.GetValueAs<DateTime>("date completed"),
                    LostWater = nvc.GetValueAs<int>("lost water"),
                    CancelledAt = nvc.ContainsKey("cancelled at") ? DateTime.Now : (DateTime?)null,
                    CancelledBy = nvc.ContainsKey("cancelled by") ? obj.Lookup("user", nvc["cancelled by"]) : null,
                    WorkOrderCancellationReason = nvc.ContainsKey("cancellation reason") ? obj.Lookup("work order cancellation reason", nvc["cancellation reason"]) : null,
                    Hydrant = obj.GetValueOrDefault<HydrantFactory>("hydrant", nvc),
                    Town = obj.GetValueOrDefault<TownFactory>("town", nvc),
                    TownSection = obj.GetValueOrDefault<TownSectionFactory>("town section", nvc),
                    MarkoutRequirement = markoutRequirement,
                    StreetOpeningPermitRequired = nvc.ContainsKey("s o p required") ? nvc.GetValueAs<bool>("s o p required") : (bool?)null,
                    DeviceLocation = nvc.ContainsKey("device location") ? nvc.GetValueAs<long>("device location") : (long?)null,
                    PremiseNumber = nvc["premise number"],
                    Service = obj.GetValueOrDefault<ServiceFactory>("service", nvc),
                    DateReceived = nvc.ContainsKey("date received") ? nvc.GetValueAs<DateTime>("date received") : DateTime.Now,
                    Purpose = obj.GetValueOrDefault<RevenueAbove1000WorkOrderPurposeFactory>("work order purpose", nvc),
                    Priority = obj.GetValueOrDefault<RoutineWorkOrderPriorityFactory>("work order priority", nvc),
                    RequestedBy = obj.GetValueOrDefault<WorkOrderRequesterFactory>("work order requester", nvc),
                    AccountCharged = nvc["account charged"],
                    ApartmentAddtl = nvc["apartment addtl"],
                    CustomerServiceLineMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "customer service material", nvc),
                    CompanyServiceLineMaterial = obj.GetValueOrDefault<ServiceMaterialFactory>("service material", "company service material", nvc),
                    MarkoutTypeNeeded = obj.GetValueOrDefault<MarkoutTypeFactory>("markout type", "markout type needed", nvc),
                    RequiredMarkoutNote = nvc["required markout note"],
                    MaterialsApprovedOn = nvc.GetValueAs<DateTime>("materials approved on")
                });
            }

            if (!String.IsNullOrWhiteSpace(nvc["approved on"]))
                model.ApprovedOn = nvc["approved on"].ToDateTime();

            if (nvc["smart cover alert"] != null)
            {
                model.SmartCoverAlert = (SmartCoverAlert)obj.GetValueOrDefault<SmartCoverAlertFactory>("smart cover alert", nvc);
            }

            return model;
        }

        public static SampleSiteAddressLocationType CreateSampleSiteAddressLocationType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "facility":
                    return container.GetInstance<FacilitySampleSiteAddressLocationTypeFactory>().Create();
                case "premise":
                    return container.GetInstance<PremiseSampleSiteAddressLocationTypeFactory>().Create();
                case "hydrant":
                    return container.GetInstance<HydrantSampleSiteAddressLocationTypeFactory>().Create();
                case "valve":
                    return container.GetInstance<ValveSampleSiteAddressLocationTypeFactory>().Create();
                case "custom":
                    return container.GetInstance<CustomSampleSiteAddressLocationTypeFactory>().Create();
                case "pending acquisition":
                    return container.GetInstance<PendingAcquisitionSampleSiteAddressLocationTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a sample site address location with description '{nvc["description"]}'");
            }
        }

        public static PublicWaterSupplyStatus CreatePublicWaterSupplyStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "active":
                    return container.GetInstance<ActivePublicWaterSupplyStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingPublicWaterSupplyStatusFactory>().Create();
                case "pending merger":
                    return container.GetInstance<PendingMergerPublicWaterSupplyStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactivePublicWaterSupplyStatusFactory>().Create();
                case "inactive -see note":
                    return container.GetInstance<InactiveSeeNotePublicWaterSupplyStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a public water supply status with description '{nvc["description"]}'");
            }
        }

        public static WasteWaterSystemStatus CreateWasteWaterSystemStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "active":
                    return container.GetInstance<ActiveWasteWaterSystemStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingWasteWaterSystemStatusFactory>().Create();
                case "pending merger":
                    return container.GetInstance<PendingMergerWasteWaterSystemStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactiveWasteWaterSystemStatusFactory>().Create();
                case "inactive -see note":
                    return container.GetInstance<InactiveSeeNoteWasteWaterSystemStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a waste water system status with description '{nvc["description"]}'");
            }
        }

        public static SapCommunicationStatus CreateSapCommunicationStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "pending":
                    return container.GetInstance<PendingSapCommunicationStatusFactory>().Create();
                case "retry":
                    return container.GetInstance<RetrySapCommunicationStatusFactory>().Create();
                case "success":
                    return container.GetInstance<SuccessSapCommunicationStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a sap communication status with description '{nvc["description"]}'");
            }
        }
        
        public static EnvironmentalNonComplianceEventCountsAgainstTarget CreateEnvironmentalNonComplianceEventCountsAgainstTarget(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<EnvironmentalNonComplianceEventCountsAgainstTargetFactory>().Create();
        }

        public static SewerOpeningType CreateSewerOpeningType(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "catch basin":
                    return container.GetInstance<CatchBasinSewerOpeningTypeFactory>().Create();
                case "clean out":
                    return container.GetInstance<CleanOutSewerOpeningTypeFactory>().Create();
                case "lamphole":
                    return container.GetInstance<LampholeSewerOpeningTypeFactory>().Create();
                case "manhole":
                    return container.GetInstance<ManholeSewerOpeningTypeFactory>().Create();
                case "outfall":
                    return container.GetInstance<OutfallSewerOpeningTypeFactory>().Create();
                case "npdes regulator":
                    return container.GetInstance<NpdesRegulatorSewerOpeningTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create sewer opening type from description string '{nvc["description"]}'");
            }
        }

        [Given("states exist")]
        public static void GivenStatesExist()
        {
            //    Given a state "one" exists with name: "Pennsylvania", abbreviation: "PA"	
            var nameValues = new Dictionary<string, string>();
            Action<string, string> createState = (abbreviation, name) => {
                nameValues.Add(abbreviation.ToLower(), $"name: \"{name}\", abbreviation: \"{abbreviation}\"");
            };

            createState("NJ", "New Jersey"); // Id:	1
            createState("NY", "New York"); // Id:"	2
            createState("PA", "Pennsylvania"); // Id:	3
            createState("IL", "Illinois"); // Id:"	4
            createState("CA", "California"); // Id:	5

            createState("FL", "Florida"); // Id:"	6
            createState("HI", "Hawaii"); // Id:	7
            createState("IA", "Iowa"); // Id:"	8
            createState("IN", "Indiana"); // Id:	9
            createState("KY", "Kentucky"); // Id: 10

            createState("MD", "Maryland"); // Id: 11
            createState("MO", "Missouri"); // Id:	12
            createState("TN", "Tennessee"); // Id:13
            createState("VA", "Virginia"); // Id:	14
            createState("WV", "West Virginia"); // Id: 15

            // TODO: When we fix the StateFactory these can be removed
            createState("AA", "16"); // Id: 16
            createState("BB", "17"); // Id: 17
            createState("CC", "18"); // Id: 18
            createState("DD", "19"); // Id: 19
            createState("WA", "Washington"); // Id: 20

            createState("OT", "Ontario"); // Id: 21
            createState("GA", "Georgia"); // Id:22
            createState("AL", "Alabama"); // Id:	23
            createState("AK", "Alaska"); // Id:24
            createState("AZ", "Arizona"); // Id:	25

            createState("AR", "Arkansas"); // Id:26
            createState("CO", "Colorado"); // Id:	27
            createState("CT", "Connecticut"); // Id:28
            createState("DE", "Delaware"); // Id:	29
            createState("ID", "Idaho"); // Id:30

            createState("KS", "Kansas"); // Id:	31
            createState("LA", "Louisiana"); // Id:32
            createState("ME", "Maine"); // Id:	33
            createState("MA", "Massachusetts"); // Id:34
            createState("MI", "Michigan"); // Id:	35

            createState("MN", "Minnesota"); // Id:36
            createState("MS", "Mississippi"); // Id:	37
            createState("MT", "Montana"); // Id:38
            createState("NE", "Nebraska"); // Id:	39
            createState("NV", "Nevada"); // Id:40

            createState("NH", "New Hampshire"); // Id:41
            createState("NM", "New Mexico"); // Id:42
            createState("NC", "North Carolina"); // Id:43
            createState("ND", "North Dakota"); // Id:44
            createState("OH", "Ohio"); // Id:	45

            createState("OK", "Oklahoma"); // Id:46
            createState("OR", "Oregon"); // Id:	47
            createState("RI", "Rhode Island"); // Id:	48
            createState("SC", "South Carolina"); // Id:	49
            createState("SD", "South Dakota"); // Id:	50

            createState("TX", "Texas"); // Id:51
            createState("UT", "Utah"); // Id:	52
            createState("VT", "Vermont"); // Id:53
            createState("WI", "Wisconsin"); // Id:	54
            createState("WY", "Wyoming"); // Id:55

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("state", nameValues);
        }

        [Given("role actions exist")]
        public static void GivenRoleActionsExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<RoleActions> createAction = (roleAction) => {
                var roleActionName = roleAction.ToString();
                nameValues.Add(roleActionName, $"id: \"{(int)roleAction}\", name: \"{roleActionName}\"");
            };

            foreach (var roleAction in Enum.GetValues(typeof(RoleActions)))
            {
                createAction((RoleActions)roleAction);
            }

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("role action", nameValues);
        }
        
        [Given("role modules exist")]
        public static void GivenRoleModulesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<RoleModules> createModule = (roleModule) => {
                var roleModuleName = roleModule.ToString();
                var appName = GetRoleApplicationsForRoleModules(roleModule);
                nameValues.Add(roleModuleName, $"id: \"{(int)roleModule}\", name: \"{roleModuleName}\", application: \"{appName}\"");
            };

            foreach (var roleModule in Enum.GetValues(typeof(RoleModules)))
            {
                createModule((RoleModules)roleModule);
            }

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("module", nameValues);
        }
        
        [Given("role applications exist")]
        public static void GivenRoleApplicationsExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<RoleApplications> createApp = (roleApp) => {
                var roleAppName = roleApp.ToString();
                nameValues.Add(roleAppName, $"id: \"{(int)roleApp}\", name: \"{roleAppName}\"");
            };

            foreach (var roleApp in Enum.GetValues(typeof(RoleApplications)))
            {
                createApp((RoleApplications)roleApp);
            }

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("application", nameValues);
        }

        private static RoleApplications GetRoleApplicationsForRoleModules(RoleModules module)
        {
            var allApplications = Enum.GetValues(typeof(RoleApplications)).OfType<RoleApplications>();
            var matchesByName = allApplications.Where(x => module.ToString().StartsWith(x.ToString())).ToList();
            if (matchesByName.Count() == 1)
            {
                return matchesByName.Single();
            }
            else if (matchesByName.Count() == 0)
            {
                throw new Exception($"No RoleApplications enum value could be found that matches for the RoleModules value: {module}");
            }
            else
            {
                throw new Exception($"Multiple RoleApplications enum values were found that matches for the RoleModules value: {module}");
            }
        }

        [Given("sap communication statuses exist")]
        public static void GivenSapCommunicationStatusesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createSapCommunicationStatus = (desc) => {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createSapCommunicationStatus("pending");
            createSapCommunicationStatus("retry");
            createSapCommunicationStatus("success");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("sap communication status", nameValues);
        }

        public static SewerMainInspectionType CreateSewerMainInspectionType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "acoustic":
                    return container.GetInstance<AcousticSewerMainInspectionTypeFactory>().Create();
                case "cctv":
                    return container.GetInstance<CCTVSewerMainInspectionTypeFactory>().Create();
                case "main cleaning pm":
                    return container.GetInstance<MainCleaningPMSewerMainInspectionTypeFactory>().Create();
                case "smoke test":
                    return container.GetInstance<SmokeTestSewerMainInspectionTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a sewer main inspection type with description '{nvc["description"]}'");
            }
        }

        [Given("sewer main inspection types exist")]
        public static void GivenSewerMainInspectionTypesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createInspectionType =
                desc => nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");

            createInspectionType("ACOUSTIC");
            createInspectionType("CCTV");
            createInspectionType("MAIN CLEANING PM");
            createInspectionType("SMOKE TEST");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("sewer main inspection type",
                nameValues);
        }

        public static SewerMainInspectionGrade CreateSewerMainInspectionGrade(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "excellent":
                    return container.GetInstance<ExcellentSewerMainInspectionGradeFactory>().Create();
                case "good":
                    return container.GetInstance<GoodSewerMainInspectionGradeFactory>().Create();
                case "fair":
                    return container.GetInstance<FairSewerMainInspectionGradeFactory>().Create();
                case "poor":
                    return container.GetInstance<PoorSewerMainInspectionGradeFactory>().Create();
                case "immediate attention":
                    return container.GetInstance<ImmediateAttentionSewerMainInspectionGradeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a sewer main inspection grade with description '{nvc["description"]}'");
            }
        }

        [Given("sewer main inspection grades exist")]
        public static void GivenSewerMainInspectionGradesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createInspectionGrade =
                desc => nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");

            createInspectionGrade("EXCELLENT");
            createInspectionGrade("GOOD");
            createInspectionGrade("FAIR");
            createInspectionGrade("POOR");
            createInspectionGrade("IMMEDIATE ATTENTION");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("sewer main inspection grade",
                nameValues);
        }

        public static WorkOrderPriority CreateWorkOrderPriority(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "emergency":
                    return container.GetInstance<EmergencyWorkOrderPriorityFactory>().Create();
                case "high priority":
                    return container.GetInstance<HighPriorityWorkOrderPriorityFactory>().Create();
                case "routine":
                    return container.GetInstance<RoutineWorkOrderPriorityFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create priority with description '{nvc["description"]}'");
            }
        }

        public static ProductionWorkOrderPriority CreateProductionWorkOrderPriority(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "emergency":
                    return container.GetInstance<EmergencyProductionWorkOrderPriorityFactory>().Create();
                case "high":
                    return container.GetInstance<HighProductionWorkOrderPriorityFactory>().Create();
                case "medium":
                    return container.GetInstance<MediumProductionWorkOrderPriorityFactory>().Create();
                case "low":
                    return container.GetInstance<LowProductionWorkOrderPriorityFactory>().Create();
                case "routine":
                    return container.GetInstance<RoutineProductionWorkOrderPriorityFactory>().Create();
                case "routine - off scheduled":
                    return container.GetInstance<RoutineOffScheduledProductionWorkOrderPriorityFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create priority with description '{nvc["description"]}'");
            }
        }

        public static WorkOrderPurpose CreateWorkOrderPurpose(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "customer":
                    return container.GetInstance<CustomerWorkOrderPurposeFactory>().Create();
                case "compliance":
                    return container.GetInstance<ComplianceWorkOrderPurposeFactory>().Create();
                case "safety":
                    return container.GetInstance<SafetyWorkOrderPurposeFactory>().Create();
                case "leak detection":
                    return container.GetInstance<LeakDetectionWorkOrderPurposeFactory>().Create();
                case "revenue 150 to 500":
                    return container.GetInstance<Revenue150To500WorkOrderPurposeFactory>().Create();
                case "revenue 500 to 1000":
                    return container.GetInstance<Revenue500To1000WorkOrderPurposeFactory>().Create();
                case "revenue above 1000":
                    return container.GetInstance<RevenueAbove1000WorkOrderPurposeFactory>().Create();
                case "damaged billable":
                    return container.GetInstance<DamagedBillableWorkOrderPurposeFactory>().Create();
                case "estimates":
                    return container.GetInstance<EstimatesWorkOrderPurposeFactory>().Create();
                case "water quality":
                    return container.GetInstance<WaterQualityWorkOrderPurposeFactory>().Create();
                case "asset record control":
                    return container.GetInstance<AssetRecordControlWorkOrderPurposeFactory>().Create();
                case "seasonal":
                    return container.GetInstance<SeasonalWorkOrderPurposeFactory>().Create();
                case "demolition":
                    return container.GetInstance<DemolitionWorkOrderPurposeFactory>().Create();
                case "bpu":
                    return container.GetInstance<BPUWorkOrderPurposeFactory>().Create();
                case "hurricane sandy":
                    return container.GetInstance<HurricaneSandyWorkOrderPurposeFactory>().Create();
                case "equip reliability":
                    return container.GetInstance<EquipReliabilityWorkOrderPurposeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create purpose with description '{nvc["description"]}'");
            }
        }

        public static WorkOrderRequester CreateWorkOrderRequester(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "customer":
                    return container.GetInstance<CustomerWorkOrderRequesterFactory>().Create();
                case "employee":
                    return container.GetInstance<EmployeeWorkOrderRequesterFactory>().Create();
                case "local government":
                    return container.GetInstance<LocalGovernmentWorkOrderRequesterFactory>().Create();
                case "call center":
                    return container.GetInstance<CallCenterWorkOrderRequesterFactory>().Create();
                case "frcc":
                    return container.GetInstance<FRCCWorkOrderRequesterFactory>().Create();
                case "acoustic monitoring":
                    return container.GetInstance<AcousticMonitoringWorkOrderRequesterFactory>().Create();
                case "nsi":
                    return container.GetInstance<NSIWorkOrderRequesterFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create requester with description '{nvc["description"]}'");
            }
        }

        public static MeterLocation CreateMeterLocation(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            return container.GetInstance<MeterLocationFactory>().Create(new {
                Description = nvc["description"],
                SAPCode = nvc["code"],
            });
        }

        public static TaskGroup CreateTaskGroup(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<TaskGroupFactory>();

            var taskGroup = factory.Create(new {
                TaskGroupId = nvc["task group id"],
                TaskGroupName = nvc["task group name"],
                TaskDetails = nvc["task details"],
                TaskDetailsSummary = nvc["task details summary"],
                TaskGroupCategory = objectCache.GetValueOrDefault<TaskGroupCategoryFactory>("task group category", nvc),
                MaintenancePlanTaskType = objectCache.GetValueOrDefault<MaintenancePlanTaskTypeFactory>("maintenance plan task type", nvc),
                EquipmentTypes = (nvc["equipment types"] ?? String.Empty).Split(";".ToCharArray()).Select(x => objectCache.Lookup<EquipmentType>("equipment type", x)).ToList()
            });

            return taskGroup;
        }

        //with start: "02/24/2020", operating center: "nj7", planning plant: "one", facilities: "one", equipment types: "rtu"
        public static MaintenancePlan CreateMaintenancePlan(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            ISet<EquipmentPurpose> EquipmentPurposeSet = new HashSet<EquipmentPurpose>();
            var factory = container.GetInstance<MaintenancePlanFactory>();
            var equipmentTypeStrings = (nvc["equipment types"] ?? string.Empty).Split(';');
            var equipmentTypes = equipmentTypeStrings
                                .Select(type => obj.Lookup<EquipmentType>("equipment type", type))
                                .ToList();

            var equipmentPurposesList = (nvc["equipment purposes"] ?? string.Empty).Split(';');
            var equipmentPurposes = equipmentPurposesList
                                   .Select(type => obj.Lookup<EquipmentPurpose>("equipment purpose", type))
                                   .ToList();
            
            equipmentPurposes.ForEach(eq => { EquipmentPurposeSet.Add(eq); });

            var mp = factory.Create(new {
                OperatingCenter = obj.GetValueOrDefault<OperatingCenterFactory>("operating center", nvc),
                EquipmentTypes = equipmentTypes,
                EquipmentPurposes = EquipmentPurposeSet,
                Facility = obj.GetValueOrDefault<FacilityFactory>("facility", nvc),
                PlanningPlant = obj.GetValueOrDefault<PlanningPlantFactory>("planning plant", nvc),
                TaskGroupCategory = obj.GetValueOrDefault<TaskGroupCategoryFactory>("task group category", nvc),
                ProductionWorkOrderFrequency = obj.GetValueOrDefault<ProductionWorkOrderFrequencyFactory>("production work order frequency", nvc),
                WorkDescription = obj.GetValueOrDefault<ProductionWorkDescriptionFactory>("production work description", nvc),
                HasACompletionRequirement = nvc.GetValueAs<bool>("has a completion requirement"),
                HasCompanyRequirement = nvc.GetValueAs<bool>("has company requirement"),
                IsActive = nvc.GetValueAs<bool>("is active"),
                Start = nvc.GetValueAs<DateTime>("start"),
                Resources = nvc.GetValueAs<decimal>("resources"),
                EstimatedHours = nvc.GetValueAs<decimal>("estimated hours"),
                ContractorCost = nvc.GetValueAs<decimal>("contractor cost"),
                SkillSet = obj.GetValueOrDefault<SkillSetFactory>("skill set", nvc),
                TaskGroup = obj.GetValueOrDefault<TaskGroupFactory>("task group", nvc)
            });

            return mp;
        }

        public static AcousticMonitoringType CreateAcousticMonitoring(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "smart cover":
                    return container.GetInstance<SmartCoverAcousticMonitoringTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create acoustic monitoring with description '{nvc["description"]}'");
            }
        }

        public static SmartCoverAlertApplicationDescriptionType CreateSmartCoverAlertApplicationDescriptionType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var factory = container.GetInstance<SmartCoverAlertApplicationDescriptionTypeFactory>();

            var model = factory.Create(new {
                Description = nvc["description"]
            });

            return model;
        }

        public static SmartCoverAlert CreateSmartCoverAlert(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<SmartCoverAlertFactory>().Create(new {
                ApplicationDescription = objectCache.GetValueOrDefault<SmartCoverAlertApplicationDescriptionTypeFactory>("smart cover alert application description", nvc),
                SewerOpening = objectCache.GetValueOrDefault<SewerOpeningFactory>("sewer opening", nvc)
            });
        }

        public static ArcFlashStatus CreateArcFlashStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "completed":
                    return container.GetInstance<CompletedArcFlashStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingArcFlashStatusFactory>().Create();
                case "deferred":
                    return container.GetInstance<DeferredArcFlashStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create an arc flash status with description '{nvc["description"]}'");
            }
        }
        
        [Given("sewer overflow causes exist")]
        public static void GivenSewerOverflowCausesExist()
        {
            Action<string> createObject = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sewer overflow cause",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
        
            createObject("pipe failure");
            createObject("debris");
            createObject("grease");
            createObject("roots");
            createObject("power failure");
            createObject("pump station or mechanical failure");
            createObject("inflow and infiltration");
            createObject("vandalism");
            createObject("pipe capacity or design");
        }
        
        private static object CreateSewerOverflowCause(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("Please enter a description to create the sewer overflow cause");
            }
        
            switch (nvc["description"])
            {
                case "pipe failure":
                    return container.GetInstance<PipeFailureSewerOverflowCauseFactory>().Create();
                case "debris":
                    return container.GetInstance<DebrisSewerOverflowCauseFactory>().Create();
                case "grease":
                    return container.GetInstance<GreaseSewerOverflowCauseFactory>().Create();
                case "roots":
                    return container.GetInstance<RootsSewerOverflowCauseFactory>().Create();
                case "power failure":
                    return container.GetInstance<PowerFailureSewerOverflowCauseFactory>().Create();
                case "pump station or mechanical failure":
                    return container.GetInstance<MechanicalFailureSewerOverflowCauseFactory>().Create();
                case "inflow and infiltration":
                    return container.GetInstance<InflowAndInfiltrationSewerOverflowCauseFactory>().Create();
                case "vandalism":
                    return container.GetInstance<VandalismSewerOverflowCauseFactory>().Create();
                case "pipe capacity or design":
                    return container.GetInstance<PipeDesignSewerOverflowCauseFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create sewer overflow cause with description '{nvc["description"]}'.");
            }
        }

        [Given("sewer overflow discharge locations exist")]
        public static void GivenSewerOverflowDischargeLocationsExist()
        {
            Action<string> createObject = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sewer overflow discharge location",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
        
            createObject("runs on ground");
            createObject("ditch or detention basin");
            createObject("storm sewer");
            createObject("body of water");
            createObject("other");
        }
        
        public static SewerOverflowDischargeLocation CreateSewerOverflowDischargeLocation(NameValueCollection nvc,
            TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "runs on ground":
                    return container.GetInstance<RunsOnGroundSewerOverflowDischargeLocationFactory>().Create();
                case "ditch or detention basin":
                    return container.GetInstance<DitchOrDetentionBasinSewerOverflowDischargeLocationFactory>().Create();
                case "storm sewer":
                    return container.GetInstance<StormSewerSewerOverflowDischargeLocationFactory>().Create();
                case "body of water":
                    return container.GetInstance<BodyOfWaterSewerOverflowDischargeLocationFactory>().Create();
                case "other":
                    return container.GetInstance<OtherSewerOverflowDischargeLocationFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a sewer opening discharge location with description '{nvc["description"]}'");
            }
        }
        
        [Given("sewer overflow types exist")]
        public static void GivenSewerOverflowTypesExist()
        {
            Action<string> createObject = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("sewer overflow type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
        
            createObject("sso");
            createObject("cso approved");
            createObject("cso unapproved");
        }
        
        private static object CreateSewerOverflowType(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException("Please enter a description to create the sewer overflow type");
            }
        
            switch (nvc["description"])
            {
                case "sso":
                    return container.GetInstance<SSOSewerOverflowTypeFactory>().Create();
                case "cso approved":
                    return container.GetInstance<CSOApprovedLocationSewerOverflowTypeFactory>().Create();
                case "cso unapproved":
                    return container.GetInstance<CSOUnapprovedLocationSewerOverflowTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create sewer overflow type with description '{nvc["description"]}'.");
            }
        }

        [Given("licensed operator categories exist")]
        public static void GivenLicensedOperatorCategoriesExist()
        {
            Action<string> createObject = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("licensed operator category",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createObject("Internal Employee");
            createObject("No Licensed Operator Required");
            createObject("Contracted Licensed Operator");
        }

        private static object CreateLicensedOperatorCategory(NameValueCollection nvc, TestObjectCache _, IContainer container)
        {
            switch (nvc["description"])
            {
                case "Internal Employee":
                    return container.GetInstance<InternalEmployeeLicensedOperatorCategoryFactory>().Create();
                case "No Licensed Operator Required":
                    return container.GetInstance<NotRequiredLicensedOperatorCategoryFactory>().Create();
                case "Contracted Licensed Operator":
                    return container.GetInstance<ContractedLicensedOperatorCategoryFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create licensed operator category with description '{nvc["description"]}'.");
            }
        }
        
        #endregion

        [Given(@"I clear out the image upload root directory folder")]
        public static void GivenIClearOutTheAsbuiltImagesFolder()
        {
            Deleporter.Run(() => {
                var path = ConfigurationManager.AppSettings["ImageUploadRootDirectory"];
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }
            });
            //throw new Exception(ConfigurationManager.AppSettings["ImageUploadRootDirectory"]);
        }
    }

    /// <summary>
    /// Having this function here, lets the regression runner go and load up the other functions we
    /// actually want loaded up for the regressions, that we've defined in
    /// MMSINC.Core\Data\NHibernate\DatabaseConfiguration.cs
    /// If it's removed some regressions will fail due to the missing sqlite functions.
    /// </summary>
    [SQLiteFunction(Name = "wellywellywell", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class WellWellWellFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return args[0];
        }
    }

    public class ChangeTrackingUserAuthenticationService : IAuthenticationService<User>
    {
        private readonly IRepository<User> _userRepo;
        
        public bool CurrentUserIsAdmin { get; }
        public bool CurrentUserIsAuthenticated => true;
        public string CurrentUserIdentifier { get; }
        public int CurrentUserId { get; }

        public User CurrentUser => _userRepo
                                  .Where(x => x.UserName == Data.CHANGE_TRACKING_USER_NAME)
                                  .SingleOrDefault();

        public ChangeTrackingUserAuthenticationService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public UserLoginAttemptStatus ValidateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void SignIn(int userId, bool isTokenAuthenticated)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public int GetUserId(string email)
        {
            throw new NotImplementedException();
        }
    }
}
