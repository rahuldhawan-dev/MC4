using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FunctionalLocation : EntityLookup
    {
        #region Properties

        public virtual Town Town { get; set; }
        public virtual AssetType AssetType { get; set; }
        public virtual bool IsActive { get; set; }

        #endregion
    }
}
