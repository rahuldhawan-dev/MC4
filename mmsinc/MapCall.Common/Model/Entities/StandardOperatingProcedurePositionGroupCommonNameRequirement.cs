using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StandardOperatingProcedurePositionGroupCommonNameRequirement : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual StandardOperatingProcedure StandardOperatingProcedure { get; set; }
        public virtual PositionGroupCommonName PositionGroupCommonName { get; set; }
        public virtual int Frequency { get; set; }
        public virtual RecurringFrequencyUnit FrequencyUnit { get; set; }

        #endregion
    }
}
