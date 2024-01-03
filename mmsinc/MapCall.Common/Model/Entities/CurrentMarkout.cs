using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CurrentMarkout : IEntity
    {
        // this is actually `WorkOrder.Id`
        public virtual int Id { get; set; }
        public virtual Markout Markout { get; set; }
        public virtual DateTime? ReadyDate { get; set; }
        public virtual DateTime? ExpirationDate { get; set; }
    }
}
