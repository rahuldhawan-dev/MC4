using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyOwnership : EntityLookup
    {
        public struct Indices
        {
            public const int AW_OWNED = 1,
                             AW_CONTRACT = 2,
                             MSG = 3,
                             CSG = 4,
                             OTHER = 5;
        }
    }
}
