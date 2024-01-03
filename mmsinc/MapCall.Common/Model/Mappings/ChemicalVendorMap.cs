using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalVendorMap : ClassMap<ChemicalVendor>
    {
        public ChemicalVendorMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.JdeVendorId).Column("JDEVendorId").Length(50).Nullable();
            Map(x => x.Vendor).Length(50).Not.Nullable();
            Map(x => x.OrderContact).Length(50).Nullable();
            Map(x => x.PhoneOffice).Length(50).Nullable();
            Map(x => x.PhoneCell).Length(25).Nullable();
            Map(x => x.Fax).Length(50).Nullable();
            Map(x => x.Email).Length(50).Nullable();
            Map(x => x.Address).Length(25).Nullable();
            Map(x => x.City).Length(25).Nullable();
            Map(x => x.State).Length(25).Nullable();
            Map(x => x.Zip).Length(25).Nullable();

            References(x => x.Coordinate).Cascade.All();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
