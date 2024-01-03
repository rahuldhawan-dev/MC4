using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MarkoutDamageUtilityDamageTypeMap : EntityLookupMap<MarkoutDamageUtilityDamageType>
    {
        public MarkoutDamageUtilityDamageTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
