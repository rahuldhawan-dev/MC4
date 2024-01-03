using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConditionType : EntityLookup
    {
        public struct Indices
        {
            public const int AS_FOUND = 1,
                             AS_LEFT = 2;
        }
    }
}
