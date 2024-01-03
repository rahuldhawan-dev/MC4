using System;
using System.Collections.Generic;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Premise : IEntity, IThingWithCoordinate, IThingWithOperatingCenter, IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                PREMISE_NUMBER = 10,
                CONNECTION_OBJECT = 30,
                DEVICE_CATEGORY = 18,
                DEVICE_LOCATION = 30,
                EQUIPMENT = 18,
                INSTALLATION = 10,
                SERIAL_NUMBER = 18,
                SERVICE_ADDRESS_HOUSE_NUMBER = 10,
                SERVICE_ADDRESS_FRACTION = 10,
                SERVICE_ADDRESS_APARTMENT = 10,
                SERVICE_ADDRESS_STREET = 60,
                SERVICE_ZIP = 10,
                SERVICE_DISTRICT = 8,
                AREA_CODE = 4,
                REGION_CODE = 4,
                ROUTE_NUMBER = 4,
                STATUS_CODE = 1,
                METER_SIZE = 8,
                METER_LOCATION = 10,
                MAJOR_ACCOUNT_MANAGER = 100,
                ACCOUNT_MANAGER_CONTACT_NUMBER = 20,
                ACCOUNT_MANAGER_EMAIL = 255;
        }

        #endregion

        #region Fields

        [NonSerialized] private IRepository<MapIcon> _mapIconRepository;

        [NonSerialized] private IRepository<ConsolidatedCustomerSideMaterial> _consolidatedMaterial;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string PremiseNumber { get; set; }
        public virtual string ConnectionObject { get; set; }
        public virtual string DeviceCategory { get; set; }
        public virtual string DeviceLocation { get; set; }
        public virtual string Equipment { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? NextMeterReadingDate { get; set; }

        // this is assigned in SAP, not actual MeterSerialNumber which is a separate field
        [View(DisplayName = "Device Serial #")]
        public virtual string DeviceSerialNumber { get; set; }

        public virtual Coordinate Coordinate { get; set; }
        public virtual decimal? Longitude => Coordinate?.Longitude;
        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual string ServiceAddressHouseNumber { get; set; }

        public virtual string ServiceAddressFraction { get; set; }

        // This View is here because I wasn't trying to break the premise import by renaming the column on
        // this day
        [View("Apartment Addtl")]
        public virtual string ServiceAddressApartment { get; set; }

        public virtual string ServiceAddressStreet { get; set; }
        public virtual Town ServiceCity { get; set; }
        public virtual State State { get; set; }
        public virtual string ServiceZip { get; set; }
        public virtual PremiseDistrict ServiceDistrict { get; set; }
        public virtual PremiseAreaCode AreaCode { get; set; }
        public virtual RegionCode RegionCode { get; set; }
        public virtual MeterReadingRoute RouteNumber { get; set; }
        public virtual PremiseStatusCode StatusCode { get; set; }
        public virtual ServiceUtilityType ServiceUtilityType { get; set; }
        public virtual ServiceSize MeterSize { get; set; }
        public virtual MeterLocation MeterLocation { get; set; }
        public virtual string MeterLocationFreeText { get; set; }
        public virtual string Installation { get; set; }
        public virtual string MeterSerialNumber { get; set; }
        public virtual PremiseCriticalCareType CriticalCareType { get; set; }
        public virtual bool IsMajorAccount { get; set; }
        public virtual string MajorAccountManager { get; set; }
        public virtual string AccountManagerContactNumber { get; set; }
        public virtual string AccountManagerEmail { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual PlanningPlant PlanningPlant { get; set; }
        public virtual PremiseType PremiseType { get; set; }
        public virtual IEnumerable<Service> Services { get; set; }
        public virtual IEnumerable<MeterReadingRouteReadingDate> MeterReadingDates { get; set; }

        public virtual IEnumerable<ShortCycleCustomerMaterial> ShortCycleCustomerMaterials { get; set; }
            = new List<ShortCycleCustomerMaterial>();

        /// <summary>
        /// A Premise can have zero or more sample sites associated to it. A real world example of this would
        /// be when a Premise has a Chemical sample site and a Bac-T sample site. A premise can also have multiple
        /// chemical samples sites, based on the assigned LIMS Facility ID/ LIMS Site ID combination.
        /// </summary>
        [DoesNotExport]
        public virtual IList<SampleSite> SampleSites { get; set; } = new List<SampleSite>();

        /// <summary>
        /// SampleSites and SampleSite were copied over from Service as part of MC-4723. MC-4723 was a large
        /// feature request and we did not want to make it larger by dissecting why SampleSite (singular) is needed.
        /// It's used mostly in the Service class as a means of displaying the most recent SampleSite for
        /// the given Service's Premise.
        /// </summary>
        [DoesNotExport]
        public virtual SampleSite SampleSite => SampleSites.FirstOrDefault();

        [SetterProperty]
        public virtual IRepository<MapIcon> MapIconRepository
        {
            set => _mapIconRepository = value;
        }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        // This is used in Current Material/Size tab of a premise record, Services File Dump. 
        public virtual MostRecentlyInstalledService MostRecentService { get; set; }

        [DoesNotExport]
        // This is used in Current Material/Size tab of a premise record. 
        public virtual ShortCycleCustomerMaterial MostRecentCustomerMaterial =>
            ShortCycleCustomerMaterials
               .Where(x => x.CustomerSideMaterial != null)
               .OrderByDescending(x => x.TechnicalInspectedOn)
               .FirstOrDefault();

        [DoesNotExport]
        public virtual Street Street
        {
            get
            {
                return ServiceCity?.Streets
                                   .FirstOrDefault(x => x.FullStName.ToUpper() == ServiceAddressStreet?.ToUpper());
            }
        }

        public virtual EPACode MostRecentServiceCompanyMaterialEPACode =>
            MostRecentService?.ServiceMaterial?.CompanyEPACodeOverridenOrDefault(OperatingCenter.State);
            
        public virtual EPACode MostRecentServiceCustomerMaterialEPACode =>
            MostRecentService?.CustomerSideMaterial?.CustomerEPACodeOverridenOrDefault(OperatingCenter.State);

        public virtual EPACode MostRecentCustomerMaterialEPACode =>
            MostRecentCustomerMaterial?.CustomerSideMaterial?.CustomerEPACodeOverridenOrDefault(OperatingCenter.State);

        public virtual EPACode ConsolidatedCustomerSideMaterial
        {
            get
            {
                return _consolidatedMaterial.Where(x =>
                        x.CustomerSideEPACode.Id == (MostRecentServiceCustomerMaterialEPACode == null ? EPACode.Indices.LEAD_STATUS_UNKNOWN : MostRecentServiceCustomerMaterialEPACode.Id) &&
                        x.CustomerSideExternalEPACode.Id == (MostRecentCustomerMaterialEPACode == null ? EPACode.Indices.LEAD_STATUS_UNKNOWN : MostRecentCustomerMaterialEPACode.Id))?
                   .FirstOrDefault()?.ConsolidatedEPACode;
            }
        }

        #endregion

        #region Formula Fields

        public virtual string FullStreetAddress { get; set; }
        public virtual string FullStreetNumber { get; set; }

        public virtual IList<Document<Premise>> Documents { get; set; } = new List<Document<Premise>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual string TableName => nameof(Premise) + "s";

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IRepository<ConsolidatedCustomerSideMaterial> ConsolidatedCustomerConvertedMaterialsRepository
        {
            set => _consolidatedMaterial = value;
        }

        #endregion
        
        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return PremiseNumber;
        }

        #endregion
    }
}
