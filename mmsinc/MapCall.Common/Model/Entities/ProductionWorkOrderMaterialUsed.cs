using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderMaterialUsed : IEntity
    {
        public virtual int Id { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual Material Material { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string NonStockDescription { get; set; }
        public virtual StockLocation StockLocation { get; set; }
    }

    [Serializable]
    public class EmployeeProductionSkillSet : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ProductionSkillSet ProductionSkillSet { get; set; }
    }
}
