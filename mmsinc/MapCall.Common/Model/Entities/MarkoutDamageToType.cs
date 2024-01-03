using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MarkoutDamageToType : EntityLookup
    {
        #region Consts

        public struct ImportantDescriptions
        {
            public const string OTHERS = "Other",
                                OURS = "Ours";
        }

        public struct Indices
        {
            public const int OURS = 1, OTHERS = 2;
        }

        #endregion
    }
}
