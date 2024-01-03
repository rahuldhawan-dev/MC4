using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OrderTypeMap : ClassMap<OrderType>
    {
        public OrderTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.SAPCode).Not.Nullable();
            Map(x => x.IsSAPEnabled).Not.Nullable();
        }
    }
}
