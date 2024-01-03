using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class HydrantModelController : ControllerBaseWithPersistence<IHydrantModelRepository, HydrantModel, User>
    {
        #region Constructor

        public HydrantModelController(ControllerBaseWithPersistenceArguments<IHydrantModelRepository, HydrantModel, User> args) : base(args) {}

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult ByManufacturerId(int id)
        {
            return new CascadingActionResult(Repository.GetByManufacturerId(id), "Description", "Id");
        }

        #endregion
    }
}