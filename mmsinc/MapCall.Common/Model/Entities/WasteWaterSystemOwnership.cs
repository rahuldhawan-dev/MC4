using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WasteWaterSystemOwnership : EntityLookup
    {
        public struct Indices
        {
            public const int AW_CONTRACT = 1,
                             AW_OWNED = 2,
                             CSG = 3,
                             MSG = 4,
                             OTHER = 5;
        }

        public static int[] WaterlyLookupStatuses = {
            Indices.AW_CONTRACT,
            Indices.AW_OWNED
        };
    }
}