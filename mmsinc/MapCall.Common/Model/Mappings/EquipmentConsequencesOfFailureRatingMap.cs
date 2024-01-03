using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentConsequencesOfFailureRatingMap : EntityLookupMap<EquipmentConsequencesOfFailureRating>
    {
        public EquipmentConsequencesOfFailureRatingMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
