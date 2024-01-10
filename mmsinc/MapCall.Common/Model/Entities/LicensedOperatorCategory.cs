using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LicensedOperatorCategory : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int INTERNAL_EMPLOYEE = 1,
                             NO_LICENSED_OPERATOR_REQUIRED = 2,
                             CONTRACTED_LICENSED_OPERATOR = 3;
        }
    }
}
