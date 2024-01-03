using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NpdesRegulatorInspectionType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int STANDARD = 1,
                             RAIN_EVENT = 2;
        }

        #endregion
    }
}
