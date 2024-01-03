using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RestorationTypeCost : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual double Cost { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual RestorationType RestorationType { get; set; }
        public virtual int FinalCost { get; set; }

        #endregion
    }
}
