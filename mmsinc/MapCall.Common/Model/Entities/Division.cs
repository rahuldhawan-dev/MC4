using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Division : EntityLookup
    {
        #region Properties

        public virtual State State { get; set; }

        #endregion
    }
}
