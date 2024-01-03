using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class RestorationMethodController : ControllerBaseWithPersistence<IRestorationMethodRepository, RestorationMethod, User> {

        #region Constructor

        public RestorationMethodController(ControllerBaseWithPersistenceArguments<IRestorationMethodRepository, RestorationMethod, User> args) : base(args) {}

        #endregion

        [HttpGet, NoCache]
        public ActionResult ByRestorationTypeID(int restorationTypeID)
        {
            return new CascadingActionResult(Repository.GetByRestorationTypeID(restorationTypeID), "Description", "Id");
        }
    }
}