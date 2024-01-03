using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterLocation : EntityLookup, ISAPLookup
    {
        public struct Indices
        {
            public const int INSIDE = 1, OUTSIDE = 2, UNKNOWN = 3;
        }

        public virtual string SAPCode { get; set; }
    }
}
