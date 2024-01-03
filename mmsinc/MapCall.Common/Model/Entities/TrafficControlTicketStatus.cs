using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrafficControlTicketStatus : EntityLookup
    {
        public struct Indices
        {
            public const int OPEN = 1,
                             AWAITING_PAYMENT = 2,
                             PENDING_SUBMITTAL = 3,
                             SUBMITTED = 4,
                             CANCELED = 5,
                             CLEARED = 6,
                             PAID = 7;
        }
    }
}
