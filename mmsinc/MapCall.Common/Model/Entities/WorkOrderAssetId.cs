using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkOrderAssetId : IEntity
    {
        /// <summary>
        /// This is actually <see cref="WorkOrder"/>.Id
        /// </summary>
        public virtual int Id { get; set; }

        public virtual string AssetId { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

        public override string ToString()
        {
            return AssetId;
        }
    }
}
