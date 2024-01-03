using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentTypeMap : ClassMap<EquipmentType>
    {
        public EquipmentTypeMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Abbreviation).Not.Nullable().Length(8);
            Map(x => x.Description).Not.Nullable().Length(50);
            Map(x => x.IsLockoutRequired).Not.Nullable();
            Map(x => x.IsEligibleForRedTagPermit).Not.Nullable();
            Map(x => x.EquipmentCategory).Nullable().Length(1);
            Map(x => x.ReferenceEquipmentNumber).Nullable().Length(60);

            References(x => x.ProductionAssetType, "ProductionAssetTypeId").Nullable();

            HasMany(x => x.EquipmentPurposes).KeyColumn("EquipmentTypeId");
            HasMany(x => x.CharacteristicFields).KeyColumn("EquipmentTypeId").Cascade.AllDeleteOrphan();

            HasMany(x => x.MeasurementPoints).KeyColumn("EquipmentTypeId").Inverse().Cascade.AllDeleteOrphan();

            References(x => x.EquipmentGroup).Not.Nullable();
        }
    }
}
