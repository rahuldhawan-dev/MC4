using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PointOfInterestStatus : EntityLookup
    {
        public struct Indices
        {
            public const int FIELD_INVESTIGATION_RECOMMENDED = 3;
        }
    }
}
