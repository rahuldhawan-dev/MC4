using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ArcFlashLabelType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int
                STANDARDLABEL = 1,
                CUSTOMLABEL = 2;
        }
    }
}
