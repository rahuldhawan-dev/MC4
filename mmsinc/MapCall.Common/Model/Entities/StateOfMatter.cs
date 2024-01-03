using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StateOfMatter : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int
                SOLID = 1,
                LIQUID = 2,
                GAS = 3;
        }
    }
}
