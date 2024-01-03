using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOverflowType : EntityLookup
    {
        public struct Indices
        {
            public const int SSO = 1,
                             CSO_APPROVED = 2,
                             CSO_UNAPPROVED = 3;
        }
    }
}
