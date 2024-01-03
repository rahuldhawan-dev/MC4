using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConditionDescription : EntityLookup
    {
        #region Properties

        public virtual ConditionType ConditionType { get; set; }

        #endregion
    }
}
