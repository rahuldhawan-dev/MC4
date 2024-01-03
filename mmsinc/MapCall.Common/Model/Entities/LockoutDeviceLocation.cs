using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LockoutDeviceLocation : EntityLookup
    {
        public struct Indices
        {
            public const int OTHER = 5;
        }

        public virtual bool IsActive { get; set; }
    }
}
