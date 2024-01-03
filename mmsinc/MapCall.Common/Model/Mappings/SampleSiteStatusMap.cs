using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteStatusMap : EntityLookupMap<SampleSiteStatus>
    {
        public const string TABLE_NAME = "SampleSiteStatuses";

        public SampleSiteStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
