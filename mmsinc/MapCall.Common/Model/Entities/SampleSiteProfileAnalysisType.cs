using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteProfileAnalysisType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int CHEMICAL = 1,
                             BACTERIAL = 2;
        }
    }
}
