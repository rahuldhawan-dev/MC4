using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentLikelyhoodOfFailureRatingMap : EntityLookupMap<EquipmentLikelyhoodOfFailureRating>
    {
        public EquipmentLikelyhoodOfFailureRatingMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
