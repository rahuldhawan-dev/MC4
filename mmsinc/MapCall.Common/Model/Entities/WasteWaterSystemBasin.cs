using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WasteWaterSystemBasin : IEntity, IThingWithNotes
    {
        #region Consts

        public struct StringLengths
        {
            public const int MAX_NAME_LENGTH = 100;
        }

        public struct DisplayNames
        {
            public const string WASTEWATER_SYSTEM_BASIN = "Wastewater System Basin",
                                WASTEWATER_SYSTEM_BASIN_ID = "WastewaterSystemBasinID";
        }

        #endregion

        #region Properties

        public virtual string TableName => nameof(WasteWaterSystemBasin) + "s";
        public virtual int Id { get; set; }
        [View(DisplayName = WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }
        public virtual string BasinName { get; set; }
        public virtual decimal? FirmCapacity { get; set; }
        public virtual decimal? FirmCapacityUnderStandbyPower { get; set; }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [View("Firm Capacity Date Updated", MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime? FirmCapacityDateUpdated { get; set; }

        public virtual IList<Note<WasteWaterSystemBasin>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        public WasteWaterSystemBasin()
        {
            Notes = new List<Note<WasteWaterSystemBasin>>();
        }

        public override string ToString() => BasinName;
    }
}
