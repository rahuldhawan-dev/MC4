using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteLocationType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int PRIMARY = 1,
                             UPSTREAM = 2,
                             DOWNSTREAM = 3,
                             GROUNDWATER = 4;
        }

        public static readonly int[] ELIGIBLE_FOR_PARENT_SITE = new[] {
            Indices.UPSTREAM,
            Indices.DOWNSTREAM,
            Indices.GROUNDWATER
        };
    }
}
