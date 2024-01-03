using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterReadingRoute : EntityLookup, ISAPLookup
    {
        public virtual string SAPCode { get; set; }
        public virtual IEnumerable<MeterReadingRouteReadingDate> MeterReadingDates { get; set; }
    }
}
