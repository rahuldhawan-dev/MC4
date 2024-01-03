using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // TODO: Assembly test that each RoleModules enum has unique values.

    /// <summary>
    /// Representations of every Module by their ModuleID in the database.
    /// Your Modules should start with your ApplicationName
    /// </summary>
    public enum RoleModules
    {
        FieldServicesAssetPlanningApproval = UpdateStatusFieldsFor2041.ASSET_PLANNING_APPROVAL, // 67
        FieldServicesAssetPlanningEndorsement = UpdateStatusFieldsFor2041.ASSET_PLANNING_ENDORSEMENT, // 68
        BAPPTeamSharingGeneral = CreateBAPPTeamIdeasTableForBug1999.MODULE_ID, // 65
        BPUGeneral = 42,
        BusinessPerformanceGeneral = 43,
        FieldServicesCapitalPlanning = UpdateStatusFieldsFor2041.CAPITAL_PLANNING, // 69
        ContractorsAgreements = 52,
        ContractorsGeneral = 50,
        CustomerGeneral = 45,
        CustomerPremises = 56,
        EngineeringEAMAssetManagement = 90,
        EnvironmentalGeneral = 58,
        EventsEvents = 48,
        FieldServicesAssets = RoleChangesForBug2432.FIELD_SERVICES_ASSETS, // 73
        FieldServicesDataLookups = 1,
        FieldServicesEstimatingProjects = 63,

        // FieldServicesHydrantInspections = 5,
        // FieldServicesHydrants = 4,
        FieldServicesImages = 55,
        FieldServicesMeterChangeOuts = 33,
        FieldServicesMeters = 47,
        FieldServicesProjects = 60,
        FieldServicesReports = 2,
        FieldServicesServices = 3,

        // FieldServicesValveInspections = 7,
        // FieldServicesValves = 6,
        FieldServicesWorkManagement = 34,
        FleetManagementGeneral = 46,
        GeneralTowns = 64,
        H2OGeneral = 54,
        HumanResourcesAccountabilityAction = 84,
        HumanResourcesAdmin = 32,
        HumanResourcesAssets = 25,
        HumanResourcesContracts = 21,
        HumanResourcesCovid = 81,
        HumanResourcesEmployee = 16,
        HumanResourcesEmployeeLimited = AddTablesAndSuchForHepBForBug2196.EMPLOYEE_LIMITED, // 71
        HumanResourcesEnvironmental = 26,
        HumanResourcesExpenseLines = 28,
        ProductionFacilities = 29,
        HumanResourcesGrievances = 24,
        HumanResourcesLookups = 31,
        HumanResourcesPositionHistory = 18,
        HumanResourcesPositionPosting = 19,
        HumanResourcesPositions = 17,
        HumanResourcesProposals = 23,
        HumanResourcesSampleSites = 30,
        HumanResourcesSections = 22,
        HumanResourcesStaffingHours = 59,
        HumanResourcesSystemDelivery = 27,
        HumanResourcesUnion = 20,
        FieldServicesLocalApproval = UpdateStatusFieldsFor2041.LOCAL_APPROVAL, // 66
        ManagementGeneral = 44,
        OperationsDistributionOnly = 39,
        OperationsHealthAndSafety = 35,
        OperationsIncidents = 61,
        OperationsIncidentsDrugTesting = 89,
        OperationsJobSiteCheckLists = 62,
        OperationsLockoutForms = AddRoleForLockoutFormForBug2173.MODULE_ID, // 72
        OperationsManagement = 38,
        OperationsTrainingModules = 36,
        OperationsTrainingRecords = 70,
        ProductionProduction = 8,
        ServiceLineProtection = 74,

        //   WaterNonPotableSewer = 40,
        //   WaterNonPotableStormWater = 41,
        WaterQualityGeneral = 51,
        FieldServicesWorkOrderInvoice = 75,
        FieldServicesMaterials = 76,
        FieldServicesSAPNotifications = 77,
        ProductionWorkManagement = 78,
        FieldServicesShortCycle = 79,
        ProductionAssetReliability = 92,
        ProductionDataAdministration = 100,
        ProductionFacilityAreaManagement = 91,
        ProductionPlannedWork = 80,
        ProductionEquipment = 82,
        ProductionSystemDeliveryConfiguration = 85,
        ProductionSystemDeliveryEntry = 86,
        ProductionSystemDeliveryApprover = 87,
        EngineeringRiskRegister = 88,
        EnvironmentalPermitTypesExpiration = 93,
        EngineeringJ100AssessmentData = 95,
        EngineeringPWSIDCapacity = 96,
        EnvironmentalWaterSystems = 97,
        EnvironmentalWasteWaterSystems = 98,
        ProductionSystemDeliveryAdmin = 99,
        EngineeringArcFlash = 101,
        ProductionInterconnections = 102,
        ProductionNonRevenueWaterUnbilledUsage = 103,
        EnvironmentalChemicalData = 104
    }

    [Serializable]
    public class Module : IEntityLookup
    {
        #region Private Members

        private ModuleDisplayItem _display;

        #endregion

        #region Properties

        [Obsolete("Use Id instead.")]
        public virtual int ModuleID => Id;

        public virtual int Id { get; set; }

        public virtual string Description => (_display ?? (_display = new ModuleDisplayItem {
            Application = Application.Name,
            Name = Name
        })).Display;

        public virtual Application Application { get; set; }
        public virtual string Name { get; set; }

        public virtual RoleModules Value => (RoleModules)Id;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class ModuleDisplayItem : DisplayItem<Module>
    {
        [SelectDynamic("Name")]
        public string Application { get; set; }

        public string Name { get; set; }

        public override string Display => $"{Application} - {Name}";
    }
}
