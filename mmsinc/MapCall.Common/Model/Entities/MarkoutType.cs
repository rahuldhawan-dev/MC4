using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MarkoutType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int NONE = 38,
                             C_TO_C = 5;
        }

        public new struct StringLengths
        {
            public const int DESCRIPTION = 120;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Order { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
