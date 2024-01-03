using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AccountingType : EntityLookup
    {
        public struct Indices
        {
            public const int CAPITAL = 1,
                             O_AND_M = 2,
                             RETIREMENT = 3;
        }
    }
}
