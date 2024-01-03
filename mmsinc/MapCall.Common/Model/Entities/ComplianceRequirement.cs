using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ComplianceRequirement : EntityLookup
    {
        public struct Indices
        {
            public const int COMPANY = 1, OSHA = 2, PSM = 3, REGULATORY = 4, TCPA = 5;
        }
    }
}
