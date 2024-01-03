using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallApi.Controllers
{
    public class HydrantController : ApiControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public HydrantController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult Create(CreateHydrant model)
        {
            if (model.Latitude != null && model.Longitude != null)
            {
                var repo = _container.GetInstance<IRepository<Coordinate>>();
                var coordinate = repo.FindByValues((decimal)model.Latitude, (decimal)model.Longitude, 0) ??
                                 repo.Save(
                                     new Coordinate {
                                         Latitude = (decimal)model.Latitude,
                                         Longitude = (decimal)model.Longitude
                                     });

                model.Coordinate = coordinate.Id;
            }
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => Json(new { model.Id }),
                OnError = GetError
            });
        }

        #endregion
    }
}
