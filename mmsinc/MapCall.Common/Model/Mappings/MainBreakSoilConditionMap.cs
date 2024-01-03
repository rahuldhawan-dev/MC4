using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainBreakSoilConditionMap : EntityLookupMap<MainBreakSoilCondition>
    {
        public MainBreakSoilConditionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainBreakSoilConditionID").Not.Nullable();
        }
    }
}
