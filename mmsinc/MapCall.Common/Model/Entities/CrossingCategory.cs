using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CrossingCategory : EntityLookup
    {
        public struct Indices
        {
            public const int RAILROAD = 2, HIGHWAY = 3;
        }
    }
}
