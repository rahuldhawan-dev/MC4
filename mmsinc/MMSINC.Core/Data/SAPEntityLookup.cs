using System;

namespace MMSINC.Data
{
    [Serializable]
    public class SapEntityLookup : EntityLookup
    {
        public virtual string SAPCode { get; set; }
    }
}
