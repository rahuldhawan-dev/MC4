using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantDuePaintingMap : ClassMap<HydrantDuePainting>
    {
        public HydrantDuePaintingMap()
        {
            ReadOnly();
            Table("HydrantsDuePaintingView");

            Id(x => x.Id);

            References(x => x.Hydrant, "Id")
               .Not.Nullable()
               .Not.Insert()
               .Not.Update();

            Map(x => x.RequiresPainting).Not.Nullable();
            Map(x => x.LastPaintedAt).Nullable();

            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
