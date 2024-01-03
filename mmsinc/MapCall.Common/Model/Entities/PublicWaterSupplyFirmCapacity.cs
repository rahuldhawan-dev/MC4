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

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyFirmCapacity
        : IEntityWithUpdateTimeTracking, IThingWithNotes, IThingWithDocuments
    {
        #region Constants

        public struct CapacityMultiplierRange
        {
            public const double MIN_VALUE = 0.0,
                                MAX_VALUE = 1.0;
        }

        public struct DisplayNames
        {
            public const string FIRM_CAPACITY_MULTIPLIER = "Firm Capacity Multiplier (<1.0)";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        public virtual float? CurrentSystemPeakDailyDemandMGD { get; set; }

        [View(FormatStyle.Date), DisplayName("Current System Peak Daily Demand Date")]
        public virtual DateTime? CurrentSystemPeakDailyDemandYearMonth { get; set; }

        public virtual float? TotalSystemSourceCapacityMGD { get; set; }

        [View(FormatStyle.DecimalMaxFourDecimalPlaces)]
        public virtual decimal? FirmSystemSourceCapacityMGD => TotalCapacityFacilitySumMGD * FirmCapacityMultiplier;

        [View(DisplayNames.FIRM_CAPACITY_MULTIPLIER, FormatStyle.DecimalMaxFourDecimalPlaces)]
        public virtual decimal FirmCapacityMultiplier { get; set; }

        [View(FormatStyle.DecimalMaxFourDecimalPlaces)]
        public virtual decimal? TotalCapacityFacilitySumMGD { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime UpdatedAt { get; set; }

        [DoesNotExport]
        public virtual string TableName => PublicWaterSupplyFirmCapacityMap.TABLE_NAME;

        public virtual IList<Note<PublicWaterSupplyFirmCapacity>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<Document<PublicWaterSupplyFirmCapacity>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion
    }
}
