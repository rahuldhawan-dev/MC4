using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Utilities;
using MapCallApi.Models;
using MapCall.Common.Metadata;
using MMSINC.Data.NHibernate;

namespace MapCallApi.Controllers
{
    public class ValveController : ApiControllerBaseWithPersistence<IValveRepository, Valve, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public ValveController(ControllerBaseWithPersistenceArguments<IValveRepository, Valve, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult Create(CreateValve model)
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
