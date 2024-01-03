using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderCancellationReason : ReadOnlyEntityLookup
    {
        public virtual string SAPCode { get; set; }

        public struct Indices
        {
            public const int CREATED_IN_ERROR = 1,
                             CUSTOMER_REQUEST = 2,
                             COMPANY_ERROR = 3,
                             ORDER_PAST_EXPIRATION_DATE = 4,
                             NO_LONGER_VALID = 5,
                             SUPERVISOR_INSTRUCTED = 6,
                             WORK_ALREADY_COMPLETED = 7;
        }
    }
}
