using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GISDataInaccuracyType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int MATERIAL = 1, DATE = 2, DIAMETER = 3, MAIN_BREAK = 4;
        }
    }
}
