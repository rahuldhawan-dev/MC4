using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// A Sample Site Point of Use Treatment Type represents a device a customer puts in their location
    /// to treat the water between the meter and any of their faucets / dispensers. We want to track
    /// these devices because they are known to skew the results of a lead copper test and AW would
    /// prefer to not sample at these sites at all when a device is installed.
    /// </summary>
    [Serializable]
    public class SampleSitePointOfUseTreatmentType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int 
                NONE = 1,  
                ENTIRE_BUILDING = 2,
                INDIVIDUAL_TAPS = 3,
                FAUCET_FILTER = 4,
                WATER_SOFTENER = 5,
                WHOLE_HOME_FILTER = 6,
                OTHER = 7;
        }
    }
}