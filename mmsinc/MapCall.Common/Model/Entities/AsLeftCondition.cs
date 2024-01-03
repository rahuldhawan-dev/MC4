using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AsLeftCondition : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                UNABLE_TO_INSPECT = 1,
                NEEDS_EMERGENCY_REPAIR = 2,
                NEEDS_REPAIR = 3,
                NEEDS_RE_INSPECTION = 4,
                NEEDS_RE_INSPECTION_SOONER_THAN_NORMAL = 5,
                ACCEPTABLE_GOOD = 6;
        }

        #endregion

        #region Properties

        public static readonly int[] AUTO_CREATE_PRODUCTION_WORK_ORDER_STATUSES = 
            { Indices.NEEDS_EMERGENCY_REPAIR, Indices.NEEDS_REPAIR };
        
        #endregion
    }
}