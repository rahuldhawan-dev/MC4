using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CrashType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int REAR_END = 1,
                             SIDEWIPE = 2,
                             FRONTAL = 3,
                             SIDE = 4,
                             OTHER = 5;
        }
    }
}
