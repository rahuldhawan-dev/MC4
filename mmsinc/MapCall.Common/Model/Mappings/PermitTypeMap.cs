using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PermitTypeMap : ClassMap<PermitType>
    {
        public PermitTypeMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("PermitTypeID");

            Map(x => x.Description).Not.Nullable();
        }
    }
}
