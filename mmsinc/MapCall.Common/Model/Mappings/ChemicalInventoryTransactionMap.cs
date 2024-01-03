using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalInventoryTransactionMap : ClassMap<ChemicalInventoryTransaction>
    {
        public ChemicalInventoryTransactionMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Storage).Not.Nullable();
            References(x => x.Delivery).Nullable();

            Map(x => x.Date).Nullable();
            Map(x => x.InventoryRecordType).Nullable();
            Map(x => x.QuantityGallons).Nullable();
            Map(x => x.QuantityPounds).Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
