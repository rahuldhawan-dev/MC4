using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteDisplayItem : DisplayItem<SampleSite>
    {
        #region Properties

        public override string Display => $"{CommonSiteName},{LocationNameDescription},{Town},{Facility?.FacilityName}";

        public Facility Facility { get; set; }

        public string TownText { get; set; }

        [SelectDynamic("ShortName")]
        public string Town { get; set; }

        public string CommonSiteName { get; set; }

        public string LocationNameDescription { get; set; }

        #endregion
    }
}
