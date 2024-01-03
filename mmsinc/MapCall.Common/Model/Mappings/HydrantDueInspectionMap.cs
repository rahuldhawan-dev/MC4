using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantDueInspectionMap : ClassMap<HydrantDueInspection>
    {
        public HydrantDueInspectionMap()
        {
            ReadOnly();
            Table("HydrantsDueInspectionView");

            Id(x => x.Id);

            References(x => x.Hydrant, "Id")
               .Not.Nullable()
               .Not.Insert()
               .Not.Update();

            Map(x => x.RequiresInspection).Not.Nullable();
            Map(x => x.LastInspectionDate).Nullable();

            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
