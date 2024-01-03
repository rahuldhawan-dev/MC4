using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LEARNItemType : EntityLookup
    {
        public virtual string Abbreviation { get; set; }
    }
}
