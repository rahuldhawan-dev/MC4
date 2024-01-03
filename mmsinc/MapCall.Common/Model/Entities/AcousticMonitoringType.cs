using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AcousticMonitoringType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int SMART_COVER = 8;
        }
    }
}
