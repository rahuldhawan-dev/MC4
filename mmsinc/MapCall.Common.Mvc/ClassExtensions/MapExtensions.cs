using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MapCall.Common.Utility;
using MMSINC.Authentication;
using StructureMap;

// ReSharper disable CheckNamespace
namespace MapCall.Common.ClassExtensions.MapExtensions
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods specifically for dealing with the map models and controller.
    /// </summary>
    public static class MapExtensions
    {
        #region Consts

        public const string MAPVIEW_VIEWDATA_KEY = "MapView ViewData Key!",
                            REQUEST_SERVER_NAME_KEY = "SERVER_NAME";

        public const decimal DEFAULT_LAT = 40.3224m, DEFAULT_LNG = -74.1481m;

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a MapView instance for this controller to be used with the
        /// view it renders.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="mapView"></param>
        public static void SetMapView(this ControllerBase controller, MapView mapView)
        {
            // Allowing for this to be overwritten.
            controller.ViewData[MAPVIEW_VIEWDATA_KEY] = mapView;
        }

        /// <summary>
        /// Returns a MapView instance that was registered with 
        /// a controller using SetMapView. This method will attempt
        /// to create one based off of this controller and the current
        /// request route. 
        /// </summary>
        public static MapView GetMapView(this ControllerBase controller)
        {
            var mapView = (MapView)controller.ViewData[MAPVIEW_VIEWDATA_KEY];
            if (mapView == null && controller is Controller)
            {
                var routeContext = new RouteContext(controller.ControllerContext);
                mapView = new MapView(((Controller)controller).ModelState) {
                    ControllerName = routeContext.RouteControllerName,
                    ActionName = routeContext.ActionName
                };
                controller.SetMapView(mapView);
            }

            return mapView;
        }

        /// <summary>
        /// Returns a MapView instance that was registered with 
        /// a controller using SetMapView. This method will attempt
        /// to create one based off of this controller and the current
        /// request route. 
        /// </summary>
        public static MapView GetMapView(this HtmlHelper helper)
        {
            return helper.ViewContext.Controller.GetMapView();
        }

        /// <summary>
        /// Generates the url needed to access the MapController from a search results page.
        /// </summary>
        public static string GenerateMapUrl(this HtmlHelper helper, MapView mapView = null,
            object additionalRouteData = null)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            mapView = mapView ?? GetMapView(helper);
            mapView.ThrowIfNull("Can not generate map url when no MapView object is available.");

            var routeValues = mapView.ToRouteValueDictionary();
            routeValues.Add("area", string.Empty);
            routeValues.Add("AreaName", helper.ViewContext.RouteData.DataTokens["area"]);
            //foreach (var thing in additionalRouteData)
            //{
            //    
            //}
            if (additionalRouteData != null)
            {
                var additionalRVD = HtmlHelperExtensions.ConvertToRouteValueDictionary(additionalRouteData);
                routeValues = routeValues.Merge(additionalRVD);
                foreach (var routeValue in routeValues.ToList())
                {
                    if (routeValue.Value is int[])
                    {
                        routeValues[routeValue.Key] = string.Join(",", (int[])routeValues[routeValue.Key]);
                    }

                    // Without this you end up with Key=System.String[] in your query string
                    if (routeValue.Value is string[] values)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            var key = $"{routeValue.Key}[{i}]";
                            routeValues.Add(key, values[i]);
                        }

                        routeValues.Remove(routeValue.Key);
                    }
                }
            }

            var controller = "Map";
            if (routeValues.ContainsKey("controller"))
            {
                controller = routeValues["controller"].ToString();
            }

            return urlHelper.Action("Index", controller, routeValues);
        }

        /// <summary>
        /// Creates a plain link to the Maps page for the current view's search results.
        /// </summary>
        public static IHtmlString MapLink(this HtmlHelper helper, string text, object htmlAttributes = null,
            MapView mapView = null, object additionalRouteData = null)
        {
            var url = GenerateMapUrl(helper, mapView, additionalRouteData);
            return helper.Link(url, text, htmlAttributes);
        }

        /// <summary>
        /// Creates a link button to the Maps page for the current view's search results.
        /// </summary>
        public static IHtmlString MapLinkButton(this HtmlHelper helper, string text, object htmlAttributes = null,
            MapView mapView = null, object additionalRouteData = null)
        {
            var url = GenerateMapUrl(helper, mapView, additionalRouteData);
            return helper.LinkButton(text, url, htmlAttributes);
        }

        private static GISLayerUpdate GetLatestGISLayerUpdate(IRepository<GISLayerUpdate> repo)
        {
            return repo.Where(x => x.IsActive).Single();
        }

        private static OperatingCenter GetUsersDefaultOperatingCenter(User currentUser)
        {
            return currentUser.DefaultOperatingCenter;
        }

        public static IHtmlString DisplayGISDataDate(this HtmlHelper helper, IRepository<GISLayerUpdate> repository)
        {
            return new HtmlString(String.Format(CommonStringFormats.DATE, GetLatestGISLayerUpdate(repository).Updated));
        }

        public static IHtmlString DisplayCurrentMapId(this HtmlHelper helper, IRepository<GISLayerUpdate> repository,
            User currentUser)
        {
            var operatingCenter = GetUsersDefaultOperatingCenter(currentUser);
            if (operatingCenter != null && !string.IsNullOrWhiteSpace(operatingCenter.MapId))
                return new HtmlString(operatingCenter.MapId);
            return new HtmlString(GetLatestGISLayerUpdate(repository).MapId);
        }

        public static IHtmlString DisplayCurrentMapCenter(this HtmlHelper helper, User currentUser)
        {
            var operatingCenter = GetUsersDefaultOperatingCenter(currentUser);
            if (operatingCenter?.Coordinate != null)
            {
                return
                    new HtmlString(
                        $"{{lat: {operatingCenter.Coordinate.Latitude}, lng: {operatingCenter.Coordinate.Longitude}}}");
            }

            return new HtmlString($"{{lat: {DEFAULT_LAT}, lng: {DEFAULT_LNG}}}");
        }

        #endregion
    }
}
