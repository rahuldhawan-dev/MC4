using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using System;

namespace MapCall.Common.Model.Mappings
{
    [Serializable]
    public class OperatorLicenseMap : ClassMap<OperatorLicense>
    {
        public OperatorLicenseMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Employee).Not.Nullable();
            References(x => x.State).Not.Nullable();
            References(x => x.OperatorLicenseType).Not.Nullable();

            Map(x => x.LicenseLevel).Length(OperatorLicense.StringLengths.LICENSE_LEVEL).Not.Nullable();
            Map(x => x.LicenseSubLevel).Length(OperatorLicense.StringLengths.LICENSE_SUB_LEVEL).Nullable();
            Map(x => x.LicenseNumber).Not.Nullable();
            Map(x => x.ValidationDate).Not.Nullable();
            Map(x => x.ExpirationDate).Not.Nullable();
            Map(x => x.LicensedOperatorOfRecord).Not.Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.PublicWaterSupplies).KeyColumn("OperatorLicenseId").Inverse().Cascade.AllDeleteOrphan();

            // not sure why my IDE and TC disagree about this
            // ReSharper disable once WrongIndentSize
            HasManyToMany(x => x.WasteWaterSystems)
               .Table("OperatorLicensesWasteWaterSystems")
               .ParentKeyColumn("OperatorLicenseId")
               .ChildKeyColumn("WasteWaterSystemId");
        }
    }
}
