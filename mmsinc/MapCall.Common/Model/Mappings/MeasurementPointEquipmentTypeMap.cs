using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeasurementPointEquipmentTypeMap : ClassMap<MeasurementPointEquipmentType>
    {
        public const string TABLE_NAME = "MeasurementPointsEquipmentTypes";

        public MeasurementPointEquipmentTypeMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.UnitOfMeasure).Not.Nullable();
            References(x => x.EquipmentType, "EquipmentTypeId").Not.Nullable();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.Min).Not.Nullable();
            Map(x => x.Max).Not.Nullable();
            Map(x => x.Category).Not.Nullable();
            Map(x => x.Position).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
        }
    }
}
