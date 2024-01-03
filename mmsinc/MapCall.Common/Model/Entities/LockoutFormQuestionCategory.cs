using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LockoutFormQuestionCategory : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int OUT_OF_SERVICE = 1, RETURN_TO_SERVICE = 2, MANAGEMENT = 3, LOCKOUT_CONDITIONS = 4;
        }

        public static readonly int[] NEW_CATEGORIES = {Indices.OUT_OF_SERVICE, Indices.LOCKOUT_CONDITIONS};

        #endregion

        #region Properties

        #endregion
    }
}
