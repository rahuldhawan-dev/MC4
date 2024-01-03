using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GeneralLiabilityClaimType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int PREVENTABLE = 1, NONPREVENTABLE = 2;
        }
    }
}
