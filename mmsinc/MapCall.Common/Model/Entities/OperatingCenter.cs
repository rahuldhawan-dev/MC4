using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatingCenter : IOperatingCenter, IEntityLookup
    {
        #region Structs

        public struct MaxLengths
        {
            public const int ARC_MOBILE_MAP_ID = 32,
                             COMPANY_INFO = 65,
                             FAX_NUMBER = 12,
                             HYDRANT_INSPECTION_FREQUENCY = 10,
                             HYDRANT_INSPECTION_FREQUENCY_UNIT = 50,
                             INFOMASTER_MAP_ID = 32,
                             LARGE_VALVE_INSPECTION_FREQUENCY = 10,
                             LARGE_VALVE_INSPECTION_FREQUENCY_UNIT = 50,
                             MAP_ID = 32,
                             MAILING_ADDRESS_NAME = 30,
                             MAILING_ADDRESS_STREET = 30,
                             MAILING_ADDRESS_CITY_STATE_ZIP = 30,
                             OPERATING_CENTER_CODE = 4,
                             OPERATING_CENTER_NAME = 30,
                             PERMITS_CAPITAL_USER_NAME = 50,
                             PERMITS_OM_USER_NAME = 50,
                             PHONE_NUMBER = 12,
                             SERVICE_CONTACT_PHONE_NUMBER = 12,
                             SMALL_VALVE_INSPECTION_FREQUENCY = 10,
                             SMALL_VALVE_INSPECTION_FREQUENCY_UNIT = 50,
                             STATE = 2,
                             DATA_COLLECTION_MAP_URL = 50;
        }

        #endregion

        #region Private Members

        private OperatingCenterDisplayItem _display;

        #endregion

        #region Properties

        [Obsolete("Use Id property")]
        public virtual int OperatingCenterId
        {
            get => Id;
            protected set => Id = value;
        }

        [Obsolete("Use Id property")]
        public virtual int OperatingCenterID
        {
            get => Id;
            protected set => Id = value;
        }
        
        public virtual int Id { get; set; }

        public virtual string CompanyInfo { get; set; }
        public virtual string FaxNumber { get; set; }

        [View(Description =
            "Where Hydrant Zones are used, this number equates to the number of zones an Operating Center has.")]
        public virtual int HydrantInspectionFrequency { get; set; }

        public virtual RecurringFrequencyUnit HydrantInspectionFrequencyUnit { get; set; }
        [View(Description =
            "Where Hydrant Zones are used, this number equates to the number of zones an Operating Center has.")]
        public virtual int? HydrantPaintingFrequency { get; set; }

        public virtual RecurringFrequencyUnit HydrantPaintingFrequencyUnit { get; set; }
        public virtual int SewerOpeningInspectionFrequency { get; set; }
        public virtual RecurringFrequencyUnit SewerOpeningInspectionFrequencyUnit { get; set; }
        public virtual int? ZoneStartYear { get; set; }
        public virtual int? PaintingZoneStartYear { get; set; }
        public virtual int LargeValveInspectionFrequency { get; set; }
        public virtual RecurringFrequencyUnit LargeValveInspectionFrequencyUnit { get; set; }
        public virtual string MailingAddressName { get; set; }
        public virtual string MailingAddressStreet { get; set; }
        public virtual string MailingAddressCityStateZip { get; set; }

        [Display(Name = "OpCode")]
        public virtual string OperatingCenterCode { get; set; }

        public virtual string OperatingCenterName { get; set; }
        public virtual string PermitsOMUserName { get; set; }
        public virtual string PermitsCapitalUserName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string ServiceContactPhoneNumber { get; set; }
        public virtual int SmallValveInspectionFrequency { get; set; }

        public virtual RecurringFrequencyUnit SmallValveInspectionFrequencyUnit { get; set; }

        public virtual State State { get; set; }
        public virtual StateRegion StateRegion { get; set; }
        public virtual bool WorkOrdersEnabled { get; set; }
        public virtual bool HasWorkOrderInvoicing { get; set; }

        [DisplayName("RSA/Division #")]
        public virtual int? RSADivisionNumber { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual int? MaximumOverflowGallons { get; set; }
        public virtual bool IsContractedOperations { get; set; }

        /// <summary>
        /// Gets/sets whether this operating center is enabled for SAP for all things.
        /// </summary>
        public virtual bool SAPEnabled { get; set; }

        /// <summary>
        /// Gets/sets whether this operating center is enabled specifically
        /// for both T&D and production work orders. If you're checking this field
        /// you should also be checking SAPEnabled == true.
        /// </summary>
        public virtual bool SAPWorkOrdersEnabled { get; set; }
        public virtual bool UsesValveInspectionFrequency { get; set; }
        public virtual bool MarkoutsEditable { get; set; }
        
        [View("Data Collection Map")]
        public virtual string DataCollectionMapUrl { get; set; }

        /// <summary>
        /// This is *ONLY* for use with JobSiteCheckLists. This comes from bug 3533. 
        /// </summary>
        public virtual OperatingCenter OperatedByOperatingCenter { get; set; }

        public virtual WBSNumber DefaultServiceReplacementWBSNumber { get; set; }

        public virtual IList<AsBuiltImage> AsBuiltImages { get; set; }
        public virtual IList<BusinessUnit> BusinessUnits { get; set; }

        public virtual IList<Town> Towns
        {
            get { return OperatingCenterTowns.Select(x => x.Town).ToList(); }
        }

        public virtual IList<Contractor> Contractors { get; set; }
        public virtual IList<BappTeam> BappTeams { get; set; }
        public virtual IList<ContractorLaborCost> ContractorLaborCosts { get; set; }
        public virtual IList<Employee> Employees { get; set; }
        public virtual IList<OperatingCenterTown> OperatingCenterTowns { get; set; }
        public virtual IList<OperatingCenterPublicWaterSupply> OperatingCenterPublicWaterSupplies { get; set; }
        public virtual IList<OperatingCenterServiceMaterial> OperatingCenterServiceMaterials { get; set; }
        public virtual IList<OperatingCenterAssetType> OperatingCenterAssetTypes { get; set; }
        public virtual IList<WaterSystem> WaterSystems { get; set; }
        public virtual IList<Facility> Facilities { get; set; }
        public virtual IList<OperatingCenterStockedMaterial> StockedMaterials { get; set; }
        public virtual IList<StockLocation> StockLocations { get; set; }

        public virtual string InfoMasterMapId { get; set; }
        public virtual string InfoMasterMapLayerName { get; set; }
        public virtual string MapId { get; set; }
        public virtual string ArcMobileMapId { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual TimeZone TimeZone { get; set; }

        public virtual string PermitsUserName => !string.IsNullOrWhiteSpace(PermitsOMUserName)
            ? PermitsOMUserName
            : PermitsCapitalUserName;

        #region Logical Properties

        public virtual string Name => Description;

        public virtual string Description => (_display ?? (_display = new OperatingCenterDisplayItem {
            OperatingCenterCode = OperatingCenterCode,
            OperatingCenterName = OperatingCenterName
        })).Display;

        public virtual IList<PlanningPlant> PlanningPlants { get; set; }

        public virtual PlanningPlant DistributionPlanningPlant =>
            PlanningPlants.FirstOrDefault(x => x.Code.StartsWith("D"));

        public virtual PlanningPlant SewerPlanningPlant => PlanningPlants.FirstOrDefault(x => x.Code.StartsWith("S"));

        public virtual PlanningPlant ProductionPlanningPlant =>
            PlanningPlants.FirstOrDefault(x => x.Code.StartsWith("P"));

        /// <summary>
        /// Returns true if records associated with this operating center should be synchronized with SAP.  
        /// </summary>
        public virtual bool CanSyncWithSAP => SAPEnabled && !IsContractedOperations;

        #endregion

        #endregion

        #region Constructors

        public OperatingCenter()
        {
            AsBuiltImages = new List<AsBuiltImage>();
            BusinessUnits = new List<BusinessUnit>();
            Contractors = new List<Contractor>();
            ContractorLaborCosts = new List<ContractorLaborCost>();
            BappTeams = new List<BappTeam>();
            Facilities = new List<Facility>();
            PlanningPlants = new List<PlanningPlant>();
            OperatingCenterAssetTypes = new List<OperatingCenterAssetType>();
            WaterSystems = new List<WaterSystem>();
            OperatingCenterTowns = new List<OperatingCenterTown>();
            StockedMaterials = new List<OperatingCenterStockedMaterial>();
            StockLocations = new List<StockLocation>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }

    [Serializable]
    public class OperatingCenterDisplayItem : DisplayItem<OperatingCenter>
    {
        public string OperatingCenterCode { get; set; }
        public string OperatingCenterName { get; set; }

        public override string Display => $"{OperatingCenterCode} - {OperatingCenterName}";
    }
}
