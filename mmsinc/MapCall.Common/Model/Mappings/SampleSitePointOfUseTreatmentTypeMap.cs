using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSitePointOfUseTreatmentTypeMap : EntityLookupMap<SampleSitePointOfUseTreatmentType>
    {
        public SampleSitePointOfUseTreatmentTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
