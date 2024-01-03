using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ArcFlashStatus : EntityLookup
    {
        public struct Indices
        {
            public const int
                COMPLETED = 1,
                PENDING = 2,
                DEFFERED = 3,
                N_A = 4,
                AFHA_RETIRED = 5,
                RETIRED_TB_RETIRED = 6,
                UNDERWAY = 7;
        }
    }

    [Serializable]
    public class PowerPhase : EntityLookup { }

    [Serializable]
    public class Voltage : EntityLookup
    {
        #region Properties

        public virtual IList<UtilityTransformerKVARating> UtilityTransformerKVARatings { get; set; }

        #endregion

        #region Constructor

        public Voltage()
        {
            UtilityTransformerKVARatings = new List<UtilityTransformerKVARating>();
        }

        #endregion
    }
}
