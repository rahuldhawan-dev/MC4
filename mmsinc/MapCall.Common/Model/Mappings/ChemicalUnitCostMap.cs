using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalUnitCostMap : ClassMap<ChemicalUnitCost>
    {
        public ChemicalUnitCostMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Chemical).Not.Nullable();
            References(x => x.WarehouseNumber).Nullable();
            References(x => x.Vendor).Nullable();

            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.PricePerPoundWet).Nullable();
            Map(x => x.PoNumber).Column("PONumber").Nullable();
            Map(x => x.ChemicalLeadTimeDays).Nullable();
            Map(x => x.ChemicalOrderingProcess).Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
