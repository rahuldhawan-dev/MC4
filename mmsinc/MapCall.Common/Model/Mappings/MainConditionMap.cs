using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainConditionMap : EntityLookupMap<MainCondition>
    {
        public MainConditionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainConditionID").Not.Nullable();
        }
    }
}
