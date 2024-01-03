using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOpeningMaterial : EntityLookup
    {
        public virtual string SAPCode { get; set; }
    }
}
