using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DriversLicenseClass : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int A = 1, B = 2, C = 3, D = 4;
        }

        public static int[] CommercialDriversLicenseIndices = {Indices.A, Indices.B, Indices.C};
    }
}
