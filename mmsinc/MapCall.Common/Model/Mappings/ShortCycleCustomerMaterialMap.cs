using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ShortCycleCustomerMaterialMap : ClassMap<ShortCycleCustomerMaterial>
    {
        public ShortCycleCustomerMaterialMap()
        {
            Id(x => x.Id);

            References(x => x.Premise).Not.Nullable();
            References(x => x.CustomerSideMaterial).Nullable();
            References(x => x.ReadingDevicePositionalLocation).Nullable();
            References(x => x.ReadingDeviceDirectionalLocation).Nullable();

            Map(x => x.AssignmentStart).Nullable();
            Map(x => x.TechnicalInspectedOn).Nullable();
            Map(x => x.ServiceLineSize).Nullable();
            Map(x => x.ShortCycleWorkOrderNumber).Not.Nullable().Unique();
        }
    }
}
