using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupply
        : IEntityLookup,
            IThingWithCoordinate,
            IThingWithDocuments,
            IThingWithNotes,
            IEntityWithUpdateTimeTracking
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int OP_CODE = 255,
                             OPERATING_AREA = 50,
                             SYSTEM = 255,
                             PWSID = 50,
                             STATUS = 255,
                             LOCAL_CERTIFIED_STATE_ID = 10;

            #endregion
        }

        #endregion

        #region Private Members

        private PublicWaterSupplyDisplayItem _display;
        private PublicWaterSupplyDisplayItemForNearMiss _nearMissDisplay;
        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        // Needs to be a set so that the SearchYearlyWaterSampleComplianceReport repo method
        // doesn't fail in relation to fetching multiple bags.
        public virtual ISet<OperatingCenterPublicWaterSupply> OperatingCenterPublicWaterSupplies { get; set; } = new HashSet<OperatingCenterPublicWaterSupply>();

        [DisplayName("Purveyor Name")]
        public virtual string OperatingArea { get; set; }

        public virtual string System { get; set; }
        public virtual State State { get; set; }

        [DisplayName("PWSID")]
        public virtual string Identifier { get; set; }

        public virtual DateTime UpdatedAt { get; set; }
        public virtual PublicWaterSupplyStatus Status { get; set; }
        public virtual PublicWaterSupplyOwnership Ownership { get; set; }
        public virtual PublicWaterSupplyType Type { get; set; }
        public virtual int? LIMSProfileNumber { get; set; }

        public virtual string LocalCertifiedStateId { get; set; }

        [DisplayName("AW Owned?")]
        public virtual bool? AWOwned { get; set; }

        public virtual int JanuaryRequiredBacterialWaterSamples { get; set; }
        public virtual int FebruaryRequiredBacterialWaterSamples { get; set; }
        public virtual int MarchRequiredBacterialWaterSamples { get; set; }
        public virtual int AprilRequiredBacterialWaterSamples { get; set; }
        public virtual int MayRequiredBacterialWaterSamples { get; set; }
        public virtual int JuneRequiredBacterialWaterSamples { get; set; }
        public virtual int JulyRequiredBacterialWaterSamples { get; set; }
        public virtual int AugustRequiredBacterialWaterSamples { get; set; }
        public virtual int SeptemberRequiredBacterialWaterSamples { get; set; }
        public virtual int OctoberRequiredBacterialWaterSamples { get; set; }
        public virtual int NovemberRequiredBacterialWaterSamples { get; set; }
        public virtual int DecemberRequiredBacterialWaterSamples { get; set; }

        public virtual bool FreeChlorineReported { get; set; }
        public virtual bool TotalChlorineReported { get; set; }

        [DisplayName("Usage Last Year (kgal)")]
        public virtual int? UsageLastYear { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? AnticipatedActiveDate { get; set; }

        public virtual bool? HasConsentOrder { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? AnticipatedMergerDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ValidTo { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ValidFrom { get; set; }
        public virtual DateTime? DateOfOwnership { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? ConsentOrderStartDate { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? ConsentOrderEndDate { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? NewSystemInitialSafetyAssessmentCompleted { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? DateSafetyAssessmentActionItemsCompleted { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? NewSystemInitialWQEnvAssessmentCompleted { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? DateWQEnvAssessmentActionItemsCompleted { get; set; }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        public virtual PublicWaterSupply AnticipatedMergePublicWaterSupply { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual PublicWaterSupplyFirmCapacity CurrentPublicWaterSupplyFirmCapacity { get; set; }

        public virtual MapIcon Icon => Coordinate?.Icon;

        public virtual string Description
        {
            get
            {
                if (_display == null)
                {
                    _display = new PublicWaterSupplyDisplayItem {
                        Identifier = Identifier,
                        OperatingArea = OperatingArea,
                        System = System
                    };
                }
                
                return _display.Display;
            }
        }

        public virtual IList<PublicWaterSupply> PendingMergerPublicWaterSupplies { get; set; } = new List<PublicWaterSupply>();

        public virtual string NearMissDescription => (_nearMissDisplay ?? (_nearMissDisplay = new PublicWaterSupplyDisplayItemForNearMiss {
            Identifier = Identifier,
            System = System
        })).Display;

        public virtual PublicWaterSupplyCustomerData CustomerData => CustomerDataRecords?.FirstOrDefault();
        public virtual IList<PublicWaterSupplyCustomerData> CustomerDataRecords { get; set; }
        public virtual IList<EnvironmentalPermit> EnvironmentalPermits { get; set; } = new List<EnvironmentalPermit>();
        public virtual IList<PublicWaterSupplyFirmCapacity> FirmCapacities { get; set; } = new List<PublicWaterSupplyFirmCapacity>();
        public virtual IList<PublicWaterSupplyPressureZone> PressureZones { get; set; } = new List<PublicWaterSupplyPressureZone>();
        public virtual IList<WaterSampleComplianceForm> WaterSampleComplianceForms { get; set; } = new List<WaterSampleComplianceForm>();
        public virtual IList<SampleSite> SampleSites { get; set; } = new List<SampleSite>();
        public virtual IList<PublicWaterSupplyLicensedOperator> LicensedOperators { get; set; } = new List<PublicWaterSupplyLicensedOperator>();

        public virtual Employee WaterDistributionLicensedOperator => 
            LicensedOperators.Select(x => x.LicensedOperator)
                             .FirstOrDefault(x => x.OperatorLicenseType.Id == OperatorLicenseType.Indices.WATER_DISTRIBUTION_OPERATOR && 
                                                  x.LicensedOperatorOfRecord)
                            ?.Employee;

        public virtual bool HasWaterSampleComplianceFormForTheCurrentMonth =>
            WaterSampleComplianceFormForTheCurrentMonth != null;

        public virtual WaterSampleComplianceForm WaterSampleComplianceFormForTheCurrentMonth
        {
            get
            {
                var now = _dateTimeProvider.GetCurrentDate();
                var monthYear = new WaterSampleComplianceMonthYear(now);

                // Using SingleOrDefault because there should only ever be one matching result at most.
                return WaterSampleComplianceForms.SingleOrDefault(x =>
                    x.CertifiedMonth == monthYear.Month && 
                    x.CertifiedYear == monthYear.Year);
            }
        }

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }
        
        public virtual ISet<PlanningPlantPublicWaterSupply> PlanningPlantPublicWaterSupplies { get; set; } = new HashSet<PlanningPlantPublicWaterSupply>();

        #region Notes/Docs

        public virtual IList<Document<PublicWaterSupply>> Documents { get; set; }
        public virtual IList<Note<PublicWaterSupply>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual string TableName => PublicWaterSupplyMap.TABLE_NAME;

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        public virtual object PublicWaterSupplyToJson()
        {
            return new {
                Id,
                System,
                Identifier,
                OperatingCenters = OperatingCenterPublicWaterSupplies
                                  .Select(y => new {
                                       y.OperatingCenter.Id,
                                       y.OperatingCenter.OperatingCenterCode,
                                       y.OperatingCenter.OperatingCenterName
                                   }).ToList(),
                FreeChlorineReported,
                TotalChlorineReported,
                Description,
                PlanningPlants = PlanningPlantPublicWaterSupplies.Select(x => new {
                    x.PlanningPlant.Id,
                    x.PlanningPlant.Code,
                    x.PlanningPlant.Description
                }) 
            };
        }

        #endregion
    }
}
