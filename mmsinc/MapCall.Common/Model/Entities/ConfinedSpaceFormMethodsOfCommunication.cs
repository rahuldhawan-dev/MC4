using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConfinedSpaceFormMethodOfCommunication : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int VOICE = 1,
                             RADIO = 2,
                             OTHER = 3;
        }
    }
}
