using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class OpeningConditionMap : EntityLookupMap<OpeningCondition>
    {
        public OpeningConditionMap()
        {
            Map(x => x.IsActive).Not.Nullable().Default("true");
        }
    }
}
