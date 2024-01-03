using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContactType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int
                CONTRACTOR_QUALITY_CONTROL = 1,
                CONTRACTOR_SAFETY_CONTROL = 2,
                POLICE_DEPARTMENT = 3,
                PERMITS = 5,
                BOROUGH_ADMINISTRATOR = 6,
                HYDRANT_OUT_OF_SERVICE = 7,
                MAIN_BREAK_NOTIFICATION = 8,
                TRAFFIC_CONTROL = 9;
        }
    }
}
