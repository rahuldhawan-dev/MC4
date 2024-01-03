using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantPaintingMap : ClassMap<HydrantPainting>
    {
        public HydrantPaintingMap()
        {
            Id(x => x.Id);

            References(x => x.Hydrant).Not.Nullable();
            References(x => x.CreatedBy).Not.Nullable();
            References(x => x.UpdatedBy).Not.Nullable();

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.PaintedAt).Not.Nullable();
        }
    }
}
