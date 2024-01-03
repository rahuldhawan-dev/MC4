using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteCollectionTypeMap : EntityLookupMap<SampleSiteCollectionType>
    {
        public SampleSiteCollectionTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}