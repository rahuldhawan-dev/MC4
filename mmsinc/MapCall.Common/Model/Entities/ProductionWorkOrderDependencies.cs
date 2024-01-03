using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderDependencies
    {
        public virtual string Code { get; set; }
        public virtual string CodeGroup { get; set; }
    }
}
