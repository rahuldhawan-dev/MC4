using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TankInspectionQuestionGroup : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SECURITY_AND_VANDALISM = 1,
                             GENERAL_SITE_INFO = 2,
                             FOUNDATION = 3,
                             EXTERIOR_CONDITION = 4,
                             EXTERIOR_ROOF = 5,
                             OVERFLOW_PIPING = 6;
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 100;
        }
    }
}