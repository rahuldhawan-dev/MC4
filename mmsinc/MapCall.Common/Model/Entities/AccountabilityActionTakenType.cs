using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AccountabilityActionTakenType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int VERBAL_COUNSELING = 1,
                             WRITTEN_WARNING = 2,
                             SUSPENSION = 3,
                             SUSPENSION_WITH_LAST_CHANCE_AGREEMENT = 4,
                             TERMINATION = 5,
                             OTHER = 6;
        }

        public struct TableName
        {
            public const string TABLE_NAME = "AccountabilityActionTakenTypes";
        }
    }
}