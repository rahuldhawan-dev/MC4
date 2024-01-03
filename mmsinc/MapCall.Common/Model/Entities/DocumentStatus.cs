using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DocumentStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1, ARCHIVED = 2;
        }
    }
}
