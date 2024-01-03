using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HydrantSize : EntityLookup
    {
        public virtual decimal Size { get; set; }
        public virtual int SortOrder { get; set; }
    }

    [Serializable]
    public class HydrantMainSize : EntityLookup
    {
        public virtual decimal Size { get; set; }
        public virtual int SortOrder { get; set; }
    }

    [Serializable]
    public class LateralSize : EntityLookup
    {
        public virtual decimal Size { get; set; }
        public virtual int SortOrder { get; set; }
    }
}
