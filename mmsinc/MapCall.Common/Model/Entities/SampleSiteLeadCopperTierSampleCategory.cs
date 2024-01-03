using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteLeadCopperTierSampleCategory : ReadOnlyEntityLookup
    {
        #region Constants

        public const int DISPLAY_VALUE_LENGTH = 5;

        #endregion

        #region Private Members

        private SampleSiteLeadCopperTierSampleCategoryDisplayItem _display;

        #endregion

        #region Properties

        public virtual string DisplayValue { get; set; }

        public virtual IList<SampleSiteLeadCopperTierClassification> TierClassifications { get; set; } = new List<SampleSiteLeadCopperTierClassification>();

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return (_display ?? (_display = new SampleSiteLeadCopperTierSampleCategoryDisplayItem {
                Description = Description,
                DisplayValue = DisplayValue
            })).Display;
        }

        #endregion
    }
}
