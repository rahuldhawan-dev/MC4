using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SAPWorkOrderStep : EntityLookup
    {
        public struct Indices
        {
            public const int CREATE = 1, UPDATE = 2, COMPLETE = 3, APPROVE_GOODS = 4, UPDATE_WITH_NMI = 5, NMI = 6;
        }
    }
}
