using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MerchantTotalFee : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual decimal Fee { get; set; }
        public virtual bool IsCurrent { get; set; }

        #endregion
    }
}
