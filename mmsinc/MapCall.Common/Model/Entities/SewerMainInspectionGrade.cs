using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerMainInspectionGrade : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int EXCELLENT = 1,
                             GOOD = 2,
                             FAIR = 3,
                             POOR = 4,
                             IMMEDIATE_ATTENTION = 5;
        }
    }
}
