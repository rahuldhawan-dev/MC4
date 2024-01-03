using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteInactivationReasonMap : EntityLookupMap<SampleSiteInactivationReason>
    {
        public SampleSiteInactivationReasonMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}