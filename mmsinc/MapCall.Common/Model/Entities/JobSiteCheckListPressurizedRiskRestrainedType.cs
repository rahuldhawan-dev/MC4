using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class JobSiteCheckListPressurizedRiskRestrainedType : EntityLookup
    {
        public struct Indices
        {
            public const int YES = 1, NO = 2;
        }
    }
}
