using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MaterialMap : ClassMap<Material>
    {
        public MaterialMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("MaterialID");

            Map(x => x.OldPartNumber).Length(15);
            Map(x => x.Size).Length(15);
            Map(x => x.Description);
            Map(x => x.PartNumber).Not.Nullable().Length(15);
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.UnitOfMeasure).Length(30).Nullable();
            Map(x => x.DoNotOrder).Not.Nullable();

            HasMany(x => x.OperatingCenterStockedMaterials)
               .KeyColumn("MaterialID").LazyLoad().Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.OperatingCenters)
               .Table("OperatingCenterStockedMaterials")
               .ParentKeyColumn("MaterialId")
               .ChildKeyColumn("OperatingCenterId");
        }
    }
}
