using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SapCommunicationStatus : EntityLookup
    {
        public struct Indices
        {
            public const int PENDING = 1, RETRY = 2, SUCCESS = 3;
        }
    }
}
