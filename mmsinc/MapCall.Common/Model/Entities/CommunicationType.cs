using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CommunicationType : EntityLookup
    {
        public struct Indices
        {
            public const int LETTER = 1,
                             ELECTRONIC = 2,
                             UPLOAD = 3,
                             EMAIL = 4,
                             PDF = 5,
                             AGENCY_SUBMITTAL_FORM = 6,
                             OTHER = 7;
        }
    }
}
