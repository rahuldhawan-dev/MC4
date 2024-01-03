using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityLikelihoodOfFailureMap : EntityLookupMap<FacilityLikelihoodOfFailure>
    {
        public FacilityLikelihoodOfFailureMap()
        {
            Table("FacilityLikelihoodsOfFailure");
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
