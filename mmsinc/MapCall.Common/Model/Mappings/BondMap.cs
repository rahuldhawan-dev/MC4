using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BondMap : ClassMap<Bond>
    {
        public BondMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("BondID").Not.Nullable();

            References(x => x.State).Nullable();
            References(x => x.County).Nullable();
            References(x => x.Town).Nullable();
            References(x => x.BondType).Nullable();
            References(x => x.BondPurpose).Nullable();
            References(x => x.OperatingCenter).Nullable();

            Map(x => x.BondNumber).Nullable();
            Map(x => x.Principal).Nullable();
            Map(x => x.Obligee).Nullable();
            Map(x => x.RecurringBond).Nullable();
            Map(x => x.BondingAgency).Nullable();
            Map(x => x.BondValue).Nullable();
            Map(x => x.AnnualPremium).Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.PermitsBondId).Not.Nullable();
            Map(x => x.BondOpen).Nullable();

            HasMany(x => x.BondDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.BondNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
