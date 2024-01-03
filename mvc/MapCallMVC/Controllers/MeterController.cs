using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class MeterController : ControllerBaseWithPersistence<IMeterRepository, Meter, User>
    {
        #region ByProfileId

        [HttpGet]
        public ActionResult ByProfileId(int meterProfileId)
        {
            return new CascadingActionResult(Repository.GetByProfileId(meterProfileId), "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        public MeterController(ControllerBaseWithPersistenceArguments<IMeterRepository, Meter, User> args) : base(args) {}
    }
}
