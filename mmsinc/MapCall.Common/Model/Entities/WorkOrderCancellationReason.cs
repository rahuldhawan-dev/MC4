using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkOrderCancellationReason : EntityLookup
    {
        public virtual string Status { get; set; }
    }
}
