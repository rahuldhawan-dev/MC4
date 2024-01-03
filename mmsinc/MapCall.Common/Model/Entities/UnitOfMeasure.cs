using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UnitOfMeasure : EntityLookup
    {
        public virtual string SAPCode { get; set; }
    }
}
