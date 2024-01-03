using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOverflowCause : EntityLookup
    {
        #region Constants
        
        public struct Indices
        {
            public const int PIPE_FAILURE = 1,
                             DEBRIS = 2,
                             GREASE = 3,
                             ROOTS = 4,
                             POWER_FAILURE = 5,
                             MECHANICAL_FAILURE = 6,
                             INFLOW_AND_INFILTRATION = 7,
                             VANDALISM = 8,
                             PIPE_DESIGN = 9;
        }
        
        #endregion
    }
}
