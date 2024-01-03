using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SAPWorkOrderPurpose : EntityLookup
    {
        #region Consts

        public new struct StringLengths
        {
            public const int CODE = 3, CODE_GROUP = 10, DESCRIPTION = 100;
        }

        #endregion

        #region Properties

        public virtual string Code { get; set; }
        public virtual string CodeGroup { get; set; }

        #endregion
    }
}
