using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class VehicleEZPassMap : ClassMap<VehicleEZPass>
    {
        public const string TABLE_NAME = "VehicleEZPasses";

        public VehicleEZPassMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.EZPassSerialNumber).Not.Nullable().Length(50);
            Map(x => x.BillingInfo).Not.Nullable().Length(255);
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
