using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalWarehouseNumberMap : ClassMap<ChemicalWarehouseNumber>
    {
        public ChemicalWarehouseNumberMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();

            Map(x => x.WarehouseNumber).Length(15).Not.Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
