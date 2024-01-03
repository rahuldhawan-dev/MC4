using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GateStatusAnswerType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                YES = 1,
                NO = 2,
                NOT_AVAILABLE = 3;
        }

        #endregion
    }
}
