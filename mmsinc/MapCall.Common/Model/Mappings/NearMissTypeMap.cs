using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class NearMissTypeMap : EntityLookupMap<NearMissType>
    {
        public NearMissTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
