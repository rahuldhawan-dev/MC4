using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class
        FacilityAssetManagementMaintenanceStrategyTierMap : EntityLookupMap<
            FacilityAssetManagementMaintenanceStrategyTier>
    {
        public FacilityAssetManagementMaintenanceStrategyTierMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
