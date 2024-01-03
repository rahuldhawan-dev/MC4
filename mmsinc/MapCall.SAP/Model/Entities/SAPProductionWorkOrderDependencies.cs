using System;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPProductionWorkOrderDependencies
    {
        public virtual string Code { get; set; }
        public virtual string CodeGroup { get; set; }
    }
}
