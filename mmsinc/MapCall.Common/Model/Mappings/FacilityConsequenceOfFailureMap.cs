using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityConsequenceOfFailureMap : EntityLookupMap<FacilityConsequenceOfFailure>
    {
        public FacilityConsequenceOfFailureMap()
        {
            Table("FacilityConsequencesOfFailure");
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
