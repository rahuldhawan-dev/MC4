using System;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReleaseReason : EntityLookup
    {
        public struct Indices
        {
            public const int RECOVERED = 1, INSTRUCTED_BY_DOCTOR = 2;
        }
    }
}
