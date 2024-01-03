using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityInspectionRatingType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SATISFACTORY = 1,
                             ACTION_REQUIRED = 2;
        }
    }
}
