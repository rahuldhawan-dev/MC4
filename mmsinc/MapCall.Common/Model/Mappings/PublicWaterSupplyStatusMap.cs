using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyStatusMap : EntityLookupMap<PublicWaterSupplyStatus>
    {
        public PublicWaterSupplyStatusMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
