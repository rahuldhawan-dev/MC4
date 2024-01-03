using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentFailureRiskRatingMap : EntityLookupMap<EquipmentFailureRiskRating>
    {
        public EquipmentFailureRiskRatingMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
