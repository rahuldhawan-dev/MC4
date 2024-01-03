using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentModelMap : ClassMap<EquipmentModel>
    {
        #region Constructors

        public EquipmentModelMap()
        {
            Id(x => x.Id, "EquipmentModelID");

            References(x => x.EquipmentManufacturer).Column("EquipmentManufacturerID").Not.Nullable();

            Map(x => x.Description).Not.Nullable();

            HasMany(x => x.Equipment).KeyColumn("ModelID");

            HasMany(x => x.EquipmentModelDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.EquipmentModelNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
