using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityConditionMap : EntityLookupMap<FacilityCondition>
    {
        public FacilityConditionMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
