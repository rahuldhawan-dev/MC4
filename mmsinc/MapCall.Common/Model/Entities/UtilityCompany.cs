using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UtilityCompany : EntityLookup
    {
        public virtual State State { get; set; }
    }
}
