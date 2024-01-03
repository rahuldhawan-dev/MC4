using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentPerformanceRatingMap : EntityLookupMap<EquipmentPerformanceRating>
    {
        public EquipmentPerformanceRatingMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
