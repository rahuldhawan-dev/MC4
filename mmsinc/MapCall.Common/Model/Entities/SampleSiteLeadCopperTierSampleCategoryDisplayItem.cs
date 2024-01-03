using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteLeadCopperTierSampleCategoryDisplayItem : DisplayItem<SampleSiteLeadCopperTierSampleCategory>
    {
        #region Properties

        public string Description { get; set; }

        public string DisplayValue { get; set; }

        public override string Display => $"{DisplayValue} - {Description}";

        #endregion
    }
}
