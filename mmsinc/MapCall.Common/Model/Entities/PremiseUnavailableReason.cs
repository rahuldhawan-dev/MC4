using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PremiseUnavailableReason : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int KILLED_PREMISE = 1, NEW_INSTALLATION = 2;
        }
    }
}
