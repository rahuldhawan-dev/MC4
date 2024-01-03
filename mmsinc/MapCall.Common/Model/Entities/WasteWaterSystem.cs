using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WasteWaterSystem : IEntityLookup, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        #region Consts

        public struct DisplayNames
        {
            public const string NEW_SYSTEM_INITIAL_WATER_QUALITY_ENVIRONMENTAL_ASSESSMENT_COMPLETED = @"New System Initial WQ\Env. Assessment Completed",
                                WATER_QUALITY_ENVIRONMENTAL_ASSESSMENT_ACTION_ITEMS_COMPLETED = @"Date WQ\Env. Assessment Action Items Completed",
                                WASTEWATER_SYSTEM = "Wastewater System";
        }

        public struct StringLengths
        {
            #region Constants

            public const int WASTE_WATER_SYSTEM_NAME = 50,
                             PERMIT_NUMBER = 50,
                             TREATMENT_DESCRIPTION = 255;

            #endregion
        }

        #endregion

        #endregion

        #region Private Members

        private WasteWaterSystemDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }

        [View("WWSID")]
        public virtual string WasteWaterSystemId => Description;

        public virtual IList<Town> Towns { get; set; } = new List<Town>();
        [View(DisplayName = DisplayNames.WASTEWATER_SYSTEM)]
        public virtual string WasteWaterSystemName { get; set; }
        public virtual string PermitNumber { get; set; }
        public virtual WasteWaterSystemStatus Status { get; set; }
        public virtual WasteWaterSystemOwnership Ownership { get; set; }
        public virtual WasteWaterSystemType Type { get; set; }
        public virtual WasteWaterSystemSubType SubType { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateOfOwnership { get; set; }

        [View("Date of Responsibility (not yet under AW ownership)", FormatStyle.Date)]
        public virtual DateTime? DateOfResponsibility { get; set; }

        public virtual int? GravityLength { get; set; }
        public virtual int? ForceLength { get; set; }
        public virtual int? NumberOfLiftStations { get; set; }
        public virtual string TreatmentDescription { get; set; }
        public virtual int? NumberOfCustomers { get; set; }
        public virtual int? PeakFlowMGD { get; set; }
        public virtual bool? IsCombinedSewerSystem { get; set; }
        public virtual bool? HasConsentOrder { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ConsentOrderStartDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ConsentOrderEndDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? NewSystemInitialSafetyAssessmentCompleted { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateSafetyAssessmentActionItemsCompleted { get; set; }

        [View(FormatStyle.Date, 
            DisplayName = DisplayNames.NEW_SYSTEM_INITIAL_WATER_QUALITY_ENVIRONMENTAL_ASSESSMENT_COMPLETED)]
        public virtual DateTime? NewSystemInitialWQEnvAssessmentCompleted { get; set; }

        [View(FormatStyle.Date, 
            DisplayName = DisplayNames.WATER_QUALITY_ENVIRONMENTAL_ASSESSMENT_ACTION_ITEMS_COMPLETED)]
        public virtual DateTime? DateWQEnvAssessmentActionItemsCompleted { get; set; }

        public virtual State State => OperatingCenter?.State;

        public virtual int TotalLengthFeet => (GravityLength ?? 0) + (ForceLength ?? 0);

        [DisplayFormat(DataFormatString = "{0:F3}")]
        public virtual decimal TotalLengthMiles
        {
            get
            {
                const int FEET_IN_MILE = 5280;
                return (decimal)TotalLengthFeet / FEET_IN_MILE;
            }
        }

        [DisplayFormat(DataFormatString = "{0:F3}")]
        public virtual decimal Total100Miles => TotalLengthMiles / 100;

        public virtual string Description => (_display ?? (_display = new WasteWaterSystemDisplayItem {
            OperatingCenter = OperatingCenter,
            Id = Id,
            WasteWaterSystemName = WasteWaterSystemName
        })).Display;

        public virtual IList<Note<WasteWaterSystem>> Notes { get; set; } = new List<Note<WasteWaterSystem>>();

        public virtual IList<Document<WasteWaterSystem>> Documents { get; set; } = new List<Document<WasteWaterSystem>>();

        public virtual IList<EnvironmentalPermit> EnvironmentalPermits { get; set; } = new List<EnvironmentalPermit>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<OperatorLicense> OperatorLicenses { get; set; } = new List<OperatorLicense>();

        public virtual IList<WasteWaterSystemBasin> WasteWaterSystemBasins { get; set; } = new List<WasteWaterSystemBasin>();

        public virtual string TableName => nameof(WasteWaterSystem) + "s";

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }
        
        public virtual ISet<PlanningPlantWasteWaterSystem> PlanningPlantWasteWaterSystems { get; set; } = new HashSet<PlanningPlantWasteWaterSystem>();

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => Enumerable.Empty<ValidationResult>();

        public override string ToString() => Description;

        public virtual object ToJson()
        {
            return new {
                Id,
                OperatingCenter = OperatingCenter.Description,
                WWSID = WasteWaterSystemId,
                WasteWaterSystemName,
                PermitNumber,
                Type = Type?.Description,
                SubType = SubType?.Description,
                GravityLength,
                ForceLength,
                NumberOfLiftStations,
                State = State.Abbreviation,
                Description,
                PlanningPlants = PlanningPlantWasteWaterSystems?.Select(x => new {
                    x.PlanningPlant.Id,
                    x.PlanningPlant.Code,
                    x.PlanningPlant.Description
                })
            };
        }

        #endregion
    }
}
