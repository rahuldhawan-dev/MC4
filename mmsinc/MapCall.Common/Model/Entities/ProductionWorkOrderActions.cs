using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderActions
    {
        public virtual string Code { get; set; }
        public virtual string CodeGroup { get; set; }
    }
}
