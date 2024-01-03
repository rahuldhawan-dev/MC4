using System;
using System.Web.Routing;

namespace MapCall.Common.Utility
{
    public class MapCallUrlHelper : IUrlHelper
    {
        #region Constants

        public const string BASE_MVC_PATH = "/Modules/mvc";

        #endregion

        #region Exposed Methods

        public string Action(string action, string controller, object routeValues)
        {
            var dict = new RouteValueDictionary(routeValues);
            var area = dict.ContainsKey("area") ? dict["area"].ToString() : String.Empty;
            var id = dict.ContainsKey("id") ? dict["id"].ToString() : string.Empty;
            var frag = dict.ContainsKey("frag") ? dict["frag"].ToString() : string.Empty;

            var url = BASE_MVC_PATH + (String.IsNullOrWhiteSpace(area)
                ? String.Format("/{0}/{1}", controller, action)
                : String.Format("/{0}/{1}/{2}", area, controller, action));

            if (!String.IsNullOrWhiteSpace(id))
                url += $"/{id}";
            if (!string.IsNullOrWhiteSpace(frag))
                url += $".{frag}";

            return url;
        }

        #endregion
    }
}
