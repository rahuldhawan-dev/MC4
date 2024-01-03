using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteLeadCopperTierClassification : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int TIER_1_SINGLE_FAMILY_WITH_LEAD_PIPE_OR_LEAD_SERVICE_LINES = 1,
                             TIER_2_BUILDING_AND_MULTI_FAMILY_RESIDENCES_WITH_COPPER_PIPES_AND_LEAD_SOLDER_INSTALLED_AFTER_1982 = 2,
                             TIER_2_BUILDING_AND_MULTI_FAMILY_RESIDENCES_WITH_LEAD_PIPES_OR_LEAD_SERVICE_LINES = 6,
                             TIER_3_SINGLE_FAMILY_RESIDENCES_WITH_COPPER_PIPES_AND_LEAD_SOLDER_INSTALLED_BEFORE_1983 = 3,
                             TIER_1_SINGLE_FAMILY_RESIDENCES_WITH_COPPER_PIPES_AND_LEAD_SOLDER_INSTALLED_AFTER_1982 = 5,
                             OTHER = 4,
                             TIER_1_NO_TEXT = 7,
                             TIER_2_NO_TEXT = 8,
                             TIER_3_NO_TEXT = 9;
        }

        #region Properties

        public virtual IList<State> States { get; set; } = new List<State>();

        #endregion
    }
}