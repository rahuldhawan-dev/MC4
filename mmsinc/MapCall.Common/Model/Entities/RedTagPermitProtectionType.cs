using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RedTagPermitProtectionType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SPRINKLER_SYSTEM = 1,
                             FIRE_PUMP = 2,
                             SPECIAL_PROTECTION = 3,
                             OTHER = 4;
        }

        /// <summary>
        /// The set of Red Tag Permit Protection Types that require additional information, for example
        /// when the protection type is 'other', we want to prompt users for additional information.
        /// </summary>
        public static readonly IEnumerable<int> ADDITIONAL_INFORMATION_REQUIRED_TYPES = new[] {
            Indices.SPECIAL_PROTECTION, 
            Indices.OTHER 
        };
    }
}