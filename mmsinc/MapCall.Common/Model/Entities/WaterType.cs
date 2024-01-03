using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterType : EntityLookup
    {
        public struct Indices
        {
            public const int WATER = 1, WASTEWATER = 2, WATERWASTEWATER = 3;
        }
    }
}
