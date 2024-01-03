using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PremiseDistrict : EntityLookup, ISAPLookup
    {
        public virtual string SAPCode { get; set; }
    }
}
