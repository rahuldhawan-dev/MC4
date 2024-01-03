using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteBracketSite : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual SampleSite SampleSite { get; set; }
        public virtual SampleSite BracketSampleSite { get; set; }
        public virtual SampleSiteBracketSiteLocationType BracketSiteLocationType { get; set; }

        #endregion
    }
}
