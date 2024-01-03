using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Configuration;
using MapCall.Common.Utility;

namespace MapCall.Common.Helpers
{
    public static class MenuViewHelper
    {
        public static IHtmlString RenderMenu(RequestContext request)
        {
            // NOTE: Do not cache this menu instance. It's not been tested for thread/request safety.
            //       Also, a quick time test shows it's incredibly fast(0ms, 200 ticks) after the
            //       first call.

            using (var sw = new StringWriter())
            {
                MenuRenderHelper.Render<MvcMenuRenderHelper>(MenuConfiguration.GetMenu(), new StringOrTextWriter(sw));
                return new HtmlString(sw.ToString());
            }
        }
    }

    public class MyUrlHelper : UrlHelper, IUrlHelper
    {
        public MyUrlHelper(RequestContext requestContext) : base(requestContext) { }

        public MyUrlHelper(RequestContext requestContext, RouteCollection routeCollection) : base(requestContext,
            routeCollection) { }
    }
}
