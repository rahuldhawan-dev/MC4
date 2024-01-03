using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SmallMeterLocation : SapEntityLookup
    {
        public virtual IList<MeterSupplementalLocation> MeterSupplementalLocations { get; set; }

        public SmallMeterLocation()
        {
            MeterSupplementalLocations = new List<MeterSupplementalLocation>();
        }
    }
}
