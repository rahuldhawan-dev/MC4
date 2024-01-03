using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MMSINC.Testing
{
    public class FakeMvcApplication : MvcApplication
    {
        /// <summary>
        /// Maps the typical routes that would pretty much always exist by default.
        /// </summary>
        /// <param name="routes"></param>
        public static void MapTypicalDefaultRoute(RouteCollection routes)
        {
            // Always use the System.Web.Mvc MapRoute extension method here when possible. It does a lot
            // of magic for us behind the scenes when it creates the actual Route object.
            routes.MapRoute(
                name: "DefaultRouteForExtensions",
                url: "{controller}/{action}/{id}.{ext}",
                defaults: new
                    {controller = "Crud"} // Can not have id param optional, conflicts with IndexRouteForExtensions
            );
            routes.MapRoute(
                name: "IndexRouteForExtensions",
                url: "{controller}/{action}.{ext}",
                defaults: new {controller = "Crud", action = "Index"}
            );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {id = UrlParameter.Optional, controller = "Crud", action = "index"});
        }

        public override IContainer InitializeContainer()
        {
            return new Container();
        }

        public override void RegisterMetadata(ModelMetadataProvider metaDataProvider)
        {
            base.RegisterMetadata(metaDataProvider);
        }

        public override void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container) { }

        public override void RegisterRoutes(RouteCollection routes)
        {
            MapTypicalDefaultRoute(routes);
        }
    }
}
