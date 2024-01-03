using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;

namespace MapCall.Common.Controllers
{
    // Map... map! Excuse me.. map! Map! Map! 
    // http://www.youtube.com/watch?v=Tekhh7Iy-sM
    // Don't allow anonymous access. The view itself needs a MapCall user to find the default coordinates.
    public class MapController : ControllerBase<MapIcon>
    {
        #region Constants

        public const string MAP_CONFIGURATION = "Map Configuration!",
                            THREAT_ALERTS = "Threat Alerts";

        #endregion

        #region Exposed Methods

        public static IEnumerable<JsonMapIcon> SerializeIcons(IRepository<MapIcon> repository, UrlHelper url)
        {
            return repository.GetAll().ToList().Select(x => new JsonMapIcon {
                id = x.Id,
                url = url.Content(x.Url),
                width = x.Width,
                height = x.Height,
                offset = x.Offset.Description
            }).ToList();
        }

        #endregion

        #region Search/Index/Show

        /// <remarks>
        /// This isn't setup to use any sort of role authorization checks. When the
        /// map view requests the MapDataUrl, that controller will deal with authorization. 
        /// </remarks>
        [HttpGet, NoCache]
        public ActionResult Index(MapView model)
        {
            if (ModelState.IsValid)
            {
                var urlHelper = new UrlHelper(Request.RequestContext);
                var routeData = model.CopySearchToRouteValueDictionary();
                routeData.Add(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME,
                    ResponseFormatterExtensions.MAP_ROUTE_EXTENSION);
                routeData["area"] = model.AreaName;

                model.MapConfiguration = new JsonMapConfiguration {
                    dataUrl = urlHelper.Action(model.ActionName, model.ControllerName, routeData),
                    icons = SerializeIcons(Repository, urlHelper),
                    defaultLayers = GetDefaultLayers(model.DefaultLayers)
                };

                //ViewData[MAP_CONFIGURATION] = new {
                //    dataUrl = urlHelper.Action(model.ActionName, model.ControllerName, routeData),
                //    iconDir = urlHelper.Content("~/Content/Images/"),
                //    icons = SerializeIcons(Repository, urlHelper)
                //};
                return View(model);
            }
            else
            {
                return HttpNotFound("Invalid map");
            }
        }

        private string[] GetDefaultLayers(string[] modelDefaultLayers)
        {
            if (modelDefaultLayers == null)
            {
                return new[] { THREAT_ALERTS };
            }

            return !modelDefaultLayers.Contains(THREAT_ALERTS)
                ? new List<string>(modelDefaultLayers) { THREAT_ALERTS }.ToArray()
                : modelDefaultLayers;
        }

        #endregion

        public MapController(ControllerBaseArguments<IRepository<MapIcon>, MapIcon> args) : base(args) { }
    }
}
