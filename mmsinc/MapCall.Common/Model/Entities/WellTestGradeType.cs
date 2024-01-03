using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WellTestGradeType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ABOVE_GRADE = 1,
                             BELOW_GRADE = 2;
        }
    }
}
