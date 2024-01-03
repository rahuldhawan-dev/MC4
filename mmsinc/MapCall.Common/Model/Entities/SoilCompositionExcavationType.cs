using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SoilCompositionExcavationType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SOLID_ROCK = 1,
                             CLAY = 2,
                             GRANULATED = 3;
        }
    }
}
