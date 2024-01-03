using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CommercialDriversLicenseProgramStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int IN_PROGRAM = 1, PURSUING = 2, NOT_IN_PROGRAM = 3;
        }

        public struct Descriptions
        {
            public const string IN_PROGRAM = "In Program",
                                PURSUING = "Pursing CDL",
                                NOT_IN_PROGRAM = "Not In Program";
        }
    }
}
