using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ChemicalStorageLocationMap : ClassMap<ChemicalStorageLocation>
    {
        public ChemicalStorageLocationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.StorageLocationNumber).Length(ChemicalStorageLocation.StringLengths.STORAGE_LOCATION_NUMBER).Not.Nullable();
            Map(x => x.StorageLocationDescription).Length(ChemicalStorageLocation.StringLengths.STORAGE_LOCATION_DESCRIPTION).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();

            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PlanningPlant).Nullable();
            References(x => x.ChemicalWarehouseNumber).Nullable();

            References(x => x.CreatedBy).Not.Nullable();
            References(x => x.UpdatedBy).Not.Nullable();
        }
    }
}
