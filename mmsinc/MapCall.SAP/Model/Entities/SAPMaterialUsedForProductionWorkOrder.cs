using System;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPMaterialUsedForProductionWorkOrder
    {
        #region Properties

        #region Logical Properties

        public virtual string ItemCategory => PartNumber != null ? "L" : "T";

        #endregion

        public virtual string PartNumber { get; set; }
        public virtual string Description { get; set; }
        public virtual string Quantity { get; set; }
        public virtual string StcokLocation { get; set; }
        public virtual string PlanningPlan { get; set; }

        #endregion
    }
}
