using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteLeadCopperValidationMethodMap : EntityLookupMap<SampleSiteLeadCopperValidationMethod>
    {
        public SampleSiteLeadCopperValidationMethodMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
