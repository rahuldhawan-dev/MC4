using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderRequiresSupervisorApproval
    {
        #region Properties

        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual bool RequiresSupervisorApproval { get; set; }

        #endregion

        #region Exposed Methods

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
