using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConfinedSpaceFormEntrantType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ENTRANT = 1,
                             ATTENDANT = 2,
                             ENTRY_SUPERVISOR = 3;
        }
    }
}
