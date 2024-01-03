using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetConditionReason : EntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int CODE = 4;
        }

        #endregion

        #region Properties

        public virtual string Code { get; set; }
        public virtual ConditionDescription ConditionDescription { get; set; }

        #endregion
    }
}
