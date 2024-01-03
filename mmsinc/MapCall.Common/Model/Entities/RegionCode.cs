using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RegionCode : EntityLookup, ISAPLookup
    {
        public virtual string SAPCode { get; set; }
        public virtual State State { get; set; }
    }
}
