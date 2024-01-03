using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BondPurposeMap : ClassMap<BondPurpose>
    {
        public BondPurposeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned().Column("BondPurposeID").Not.Nullable();

            Map(x => x.Description).Not.Nullable();
        }
    }
}
