using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AbbreviationType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int TOWN = 1, TOWN_SECTION = 2, FIRE_DISTRICT = 3;
        }
    }
}
