using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkOrderRequester : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int
                CUSTOMER = 1,
                EMPLOYEE = 2,
                LOCAL_GOVERNMENT = 3,
                CALL_CENTER = 4,
                FRCC = 5,
                ACOUSTIC_MONITORING = 6,
                NSI = 9;
        }
    }
}
