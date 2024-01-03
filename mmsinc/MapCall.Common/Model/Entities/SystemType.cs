using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SystemType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int DRINKING_WATER = 1, WASTE_WATER = 2, OTHER = 3;
        }
    }
}