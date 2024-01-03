using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MapCall.Common.Model.Mappings;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyPressureZone : IEntity, IThingWithNotes
    {
        #region Structs

        public struct StringLengths
        {
            public const int HYDRAULIC_MODEL_NAME = 50,
                             COMMON_NAME = 50;
        }

        public struct PressureRangeValues
        {
            public struct Min
            {
                public const int LOWER = 0,
                                 UPPER = 499;
            }

            public struct Max
            {
                public const int LOWER = 1,
                                 UPPER = 500;
            }
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        public virtual PublicWaterSupplyFirmCapacity PublicWaterSupplyFirmCapacity { get; set; }

        public virtual string HydraulicModelName { get; set; }

        [View("Hydraulic Gradient (HGL) Min")]
        public virtual int HydraulicGradientMin { get; set; }

        [View("Hydraulic Gradient (HGL) Max")]
        public virtual int HydraulicGradientMax { get; set; }

        public virtual int? PressureMin { get; set; }

        public virtual int? PressureMax { get; set; }

        public virtual string CommonName { get; set; }

        [DoesNotExport]
        public virtual string TableName => PublicWaterSupplyPressureZoneMap.TABLE_NAME;

        public virtual IList<Note<PublicWaterSupplyPressureZone>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Exposed Methods

        public override string ToString() => HydraulicModelName;

        #endregion
    }
}
