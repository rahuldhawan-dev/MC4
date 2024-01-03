using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class VehicleAuditMap : ClassMap<VehicleAudit>
    {
        public VehicleAuditMap()
        {
            Id(x => x.Id);

            Map(x => x.AuditedOn).Not.Nullable();
            Map(x => x.DecalNumber)
               .Length(VehicleAudit.StringLengths.DECAL_NUMBER)
               .Not.Nullable();
            Map(x => x.Mileage).Not.Nullable();
            Map(x => x.PlateNumber)
               .Length(VehicleAudit.StringLengths.PLATE_NUMBER)
               .Not.Nullable();

            References(x => x.Vehicle).Not.Nullable();
        }
    }
}
