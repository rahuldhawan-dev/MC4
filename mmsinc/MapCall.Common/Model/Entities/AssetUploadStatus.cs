using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetUploadStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int PENDING = 1, SUCCESS = 2, ERROR = 3;
        }
    }
}
