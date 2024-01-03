using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AsFoundCondition : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                UNABLE_TO_INSPECT = 1,
                SERIOUS_DETERIORATION = 2,
                SOME_DETERIORATION = 3,
                QUESTIONABLE = 4,
                ACCEPTABLE_GOOD = 5;
        }

        #endregion
    }
}