using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterSupplementalLocation : SapEntityLookup
    {
        public struct Indices
        {
            public const int INSIDE = 1, OUTSIDE = 2, SECURE_ACCESS = 3, BLANK = 4, LS = 5, RS = 6, XX = 7, UV = 8;
        }
    }
}
