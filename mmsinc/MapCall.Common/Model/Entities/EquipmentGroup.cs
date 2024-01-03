using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentGroup : ReadOnlyEntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            public const int DESCRIPTION = 25;
            public const int DEFINITION = 500;
        }

        public struct Indices
        {
            public const int ELECTRICAL = 5,
                             FLOW_METER = 7,
                             INSTRUMENT = 9,
                             MECHANICAL = 12,
                             SAFETY = 15,
                             TANK = 16,
                             TREATMENT = 17,
                             WELL = 19;
        }

        public static string ELECTRICAL = "Electrical",
                             FLOW_METER = "Flow Meter",
                             INSTRUMENT = "Instrument",
                             MECHANICAL = "Mechanical",
                             SAFETY = "Safety",
                             TANK = "Tank",
                             TREATMENT = "Treatment",
                             WELL = "Well",
                             ELECTRICAL_CODE = "E",
                             FLOW_METER_CODE = "FM",
                             INSTRUMENT_CODE = "I",
                             MECHANICAL_CODE = "M",
                             SAFETY_CODE = "S",
                             TANK_CODE = "T",
                             TREATMENT_CODE = "TR",
                             WELL_CODE = "W";
        #endregion

        #region Properties

        [Required]
        public virtual string Code { get; set; }
        
        [Required, StringLength(StringLengths.DESCRIPTION)]
        public override string Description { get; set; }

        [StringLength(StringLengths.DEFINITION)]
        public virtual string Definition { get; set; }

        #endregion
    }
}
