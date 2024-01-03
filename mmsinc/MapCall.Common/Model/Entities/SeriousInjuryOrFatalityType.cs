using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SeriousInjuryOrFatalityType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SIF = 1, SIF_POTENTIAL = 2;
        }
    }
}
