using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalDeliveryMap : ClassMap<ChemicalDelivery>
    {
        public const string TABLE_NAME = "ChemicalDeliveries";

        public ChemicalDeliveryMap()
        {
            Table(TABLE_NAME);
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Storage).Not.Nullable();
            References(x => x.Facility)
               .Formula("(SELECT cs.FacilityId FROM ChemicalStorage cs WHERE cs.Id = StorageId)").LazyLoad();
            References(x => x.UnitCost).Nullable();
            References(x => x.Chemical).Not.Nullable();

            Map(x => x.DateOrdered).Nullable();
            Map(x => x.ScheduledDeliveryDate).Nullable();
            Map(x => x.ActualDeliveryDate).Nullable();
            Map(x => x.ConfirmationInformation).Length(255).Nullable();
            Map(x => x.ReceiptNumberJde).Column("ReceiptNumberJDE").Length(50).Nullable();
            Map(x => x.BatchNumberJde).Column("BatchNumberJDE").Length(50).Nullable();
            Map(x => x.EstimatedDeliveryQuantityGallons).Nullable();
            Map(x => x.ActualDeliveryQuantityGallons).Nullable();
            Map(x => x.EstimatedDeliveryQuantityPounds).Nullable();
            Map(x => x.ActualDeliveryQuantityPounds).Nullable();
            Map(x => x.DeliveryTicketNumber).Length(50).Nullable();
            Map(x => x.DeliveryInstructions).Nullable();
            Map(x => x.SplitFacilityDelivery).Not.Nullable();
            Map(x => x.SecurityInformation).Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
