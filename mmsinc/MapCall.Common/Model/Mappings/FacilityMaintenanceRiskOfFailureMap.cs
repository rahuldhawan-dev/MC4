using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityMaintenanceRiskOfFailureMap : EntityLookupMap<FacilityMaintenanceRiskOfFailure>
    {
        public FacilityMaintenanceRiskOfFailureMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.RiskScore).Not.Nullable();
        }
    }
}
