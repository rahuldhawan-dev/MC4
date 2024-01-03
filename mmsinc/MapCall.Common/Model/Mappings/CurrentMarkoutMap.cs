using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2023;

namespace MapCall.Common.Model.Mappings
{
    public class CurrentMarkoutMap : ClassMap<CurrentMarkout>
    {
        public CurrentMarkoutMap()
        {
            ReadOnly();
            Table(MC5406_CreateCurrentMarkoutView.VIEW_NAME);

            Id(x => x.Id).Column("WorkOrderID");

            References(x => x.Markout)
               .Not.Nullable()
               .Not.Insert()
               .Not.Update();

            Map(x => x.ReadyDate).Nullable();
            Map(x => x.ExpirationDate).Nullable();

            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
