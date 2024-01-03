using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class AssetMapController : ControllerBase<IIconSetRepository, IconSet>
    {
        #region Constructor

        public AssetMapController(ControllerBaseArguments<IIconSetRepository, IconSet> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        /// <remarks>
        /// This isn't setup to use any sort of role authorization checks. When the
        /// map view requests the MapDataUrl, that controller will deal with authorization. 
        /// </remarks>
        [HttpGet, NoCache, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Index(AssetMapView model)
        {
            if (ModelState.IsValid)
            {
                var urlHelper = new UrlHelper(Request.RequestContext);
                var routeData = model.CopySearchToRouteValueDictionary();
                routeData.Add(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME,
                    ResponseFormatterExtensions.MAP_ROUTE_EXTENSION);
                routeData["area"] = model.AreaName;

                model.MapConfiguration = new JsonMapConfiguration
                {
                    dataUrl = urlHelper.Action(model.ActionName, model.ControllerName, routeData),
                    icons = Repository.Find(IconSets.Assets).Icons.Select(x => new JsonMapIcon
                    {
                        id = x.Id,
                        url = urlHelper.Content(x.Url),
                        width = x.Width,
                        height = x.Height,
                        offset = x.Offset.Description
                    })
                };

                model.MapConfiguration.additionalData.Add("extentsUrl", urlHelper.Action("GetExtents", "AssetMap", new
                {
                    OperatingCenter = (model.OperatingCenter!=null) ? string.Join(",", model.OperatingCenter) : null
                }));
            }
            else
            {
                model.MapConfiguration = new JsonMapConfiguration
                {
                    validationErrors = GetModelStateErrors()
                };
            }

            return View(model);
        }

        [HttpGet, NoCache, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult GetExtents(AssetCoordinateSearch search)
        {
            var mr = _container.GetInstance<AssetMapResult>();

            if (ModelState.IsValid)
            {
                // NOTE: Because the same search model is being used for multiple queries, you MUST create a copy of the list
                //       result returned from the repository as it will keep returning the same list instance the SearchSet uses.
                // i.e. Don't kill the ToList() calls.
                var valRepo = _container.GetInstance<IValveRepository>();
                var valves = valRepo.GetValveAssetCoordinates(search).ToList();
                var blowOffs = valRepo.GetBlowOffAssetCoordinates(search).ToList();
                var hydrants = _container.GetInstance<IHydrantRepository>().GetAssetCoordinates(search).ToList();
                var mainCrossings = _container.GetInstance<IMainCrossingRepository>().GetAssetCoordinates(search).ToList();
                var sewerOpenings = _container.GetInstance<ISewerOpeningRepository>().GetAssetCoordinates(search).ToList();
                mr.Initialize(valves, blowOffs, hydrants, mainCrossings, sewerOpenings);
            }

            return mr;
        }

        private IEnumerable<string> GetModelStateErrors()
        {
            foreach (var ms in ModelState.Values.Where(x => x.Errors.Any()))
            {
                yield return ms.Errors.First().ErrorMessage;
            }
        }

        #endregion

    }
}