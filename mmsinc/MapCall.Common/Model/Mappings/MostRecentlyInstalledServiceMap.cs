using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2022;

namespace MapCall.Common.Model.Mappings
{
    public class MostRecentlyInstalledServiceMap : ClassMap<MostRecentlyInstalledService>
    {
        public MostRecentlyInstalledServiceMap()
        {
            ReadOnly();
            Table(MC4687_CreateMostRecentlyInstalledServicesView.VIEW_NAME);

            Id(x => x.Id).Column("PremiseId");

            References(x => x.Premise)
               .Not.Nullable()
               .Not.Insert()
               .Not.Update();
            References(x => x.Service)
               .Not.Nullable()
               .Not.Insert()
               .Not.Update();
            References(x => x.MeterSettingSize).Nullable();
            References(x => x.ServiceMaterial).Nullable();
            References(x => x.ServiceSize).Nullable();
            References(x => x.CustomerSideMaterial).Nullable();
            References(x => x.CustomerSideSize).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.ServiceMaterialSetBy).Nullable();
            References(x => x.CustomerMaterialSetBy).Nullable();

            Map(x => x.DateInstalled).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            
            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
