using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallIntranet.Helpers;
using MapCallIntranet.Models.ViewModels;
using MapCallIntranet.Services;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Metadata;

namespace MapCallIntranet.Controllers
{
    public class CoordinateController : ControllerBaseWithPersistence<Coordinate, User>
    {
        private readonly IAssetCoordinateService _coordinateService;

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    NotFound = $"Coordinate with the id {id} not found."
                }, onModelFound:
                (model) => {
                    if (model.Icon == null)
                    {
                        // Not every coordinate has an icon, so we wanna give
                        // it a default one to use. Otherwise everything crashes.
                        model.Icon = _container.GetInstance<IIconSetRepository>().GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon;
                    }
                });
        }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New(CreateCoordinate coord)
        {
            LoginHelper.DoLogin();
            if (coord.Id > 0)
            {
                coord.Map(Repository.Find(coord.Id));
            }

            return ActionHelper.DoNew(coord);
        }

        [HttpPost, RequiresSecureForm(false)]
        public ActionResult Create(CreateCoordinate coord)
        {
            var existing = Repository.FindByValues(coord.Latitude, coord.Longitude, coord.IconId);

            if (existing != null)
            {
                return Json(new { coordinateId = existing.Id, lat = coord.Latitude, lng = coord.Longitude });
            }

            return ActionHelper.DoCreate(coord, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => Json(new {
                    coordinateId = coord.Id,
                    lat = coord.Latitude,
                    lng = coord.Longitude
                }),
                OnError = () => {
                    // TODO: This should return validation errors to the client rather than throwing an exception.
                    throw new ModelValidationException(ModelState);
                }
            });
        }

        #endregion

        #region Close

        [HttpGet]
        public ActionResult Close(CreateCoordinate coordinate)
        {
            return View(coordinate);
        }

        #endregion

        [HttpGet]
        public ActionResult GetCoordinateIdForAsset(int assetType, int assetId)
        {
            return Json(_coordinateService.GetCoordinateIdForAsset(assetType, assetId), JsonRequestBehavior.AllowGet);
        }

        public CoordinateController(
            ControllerBaseWithPersistenceArguments<IRepository<Coordinate>, Coordinate, User> args,
            IAssetCoordinateService coordService) : base(args)
        {
            _coordinateService = coordService;
        }
    }
}
