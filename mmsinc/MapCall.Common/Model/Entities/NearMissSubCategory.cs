using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NearMissSubCategory : ReadOnlyEntityLookup
    {
        public virtual NearMissCategory Category { get; set; }
    }
}
