using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteProfileDisplayItem : DisplayItem<SampleSiteProfile>
    {
        #region Properties

        public int Number { get; set; }

        public string Name { get; set; }

        public virtual SampleSiteProfileAnalysisType SampleSiteProfileAnalysisType { get; set; }

        public override string Display => string.IsNullOrEmpty(Name)
            ? $"{Number} - {SampleSiteProfileAnalysisType.Description}"
            : $"{Number} - {Name} - {SampleSiteProfileAnalysisType.Description}";

        #endregion
    }
}
