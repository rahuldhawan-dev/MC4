using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ConfinedSpaceFormEntrantTypeMap : EntityLookupMap<ConfinedSpaceFormEntrantType>
    {
        public ConfinedSpaceFormEntrantTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
