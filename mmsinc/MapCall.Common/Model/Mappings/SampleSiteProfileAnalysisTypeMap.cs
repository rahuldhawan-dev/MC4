using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteProfileAnalysisTypeMap : EntityLookupMap<SampleSiteProfileAnalysisType>
    {
        public SampleSiteProfileAnalysisTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
