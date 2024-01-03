using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteCollectionType : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int RAW = 1,
                             IN_PLANT = 2,
                             ENTRY_POINT = 3,
                             INTERCONNECT = 4,
                             DISTRIBUTION = 5,
                             WASTEWATER = 6;
        }

        #endregion
    }
}