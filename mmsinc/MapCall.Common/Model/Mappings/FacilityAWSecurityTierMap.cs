using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityAWSecurityTierMap : EntityLookupMap<FacilityAWSecurityTier>
    {
        public FacilityAWSecurityTierMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
