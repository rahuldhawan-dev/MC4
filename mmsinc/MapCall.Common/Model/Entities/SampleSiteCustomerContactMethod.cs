using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteCustomerContactMethod : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int EMAIL = 1,
                             MAIL = 2,
                             PHONE = 3,
                             TEXT_MESSAGE = 4;
        }
    }
}
