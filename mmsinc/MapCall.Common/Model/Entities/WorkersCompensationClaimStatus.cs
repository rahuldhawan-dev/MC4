using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkersCompensationClaimStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int NO_CLAIM = 1,
                             OPEN = 2,
                             CLOSED_ACCEPTED = 3,
                             CLOSED_DENIED = 4;
        }

        /// <summary>
        /// MC-4111 - These are the statuses that are considered when using Claims Carrier Id
        /// </summary>
        public static readonly int[] CLAIMS_CARRIER_ID = new[] { Indices.OPEN, Indices.CLOSED_ACCEPTED };
    }
}
