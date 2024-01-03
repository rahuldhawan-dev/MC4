using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallIntranet.Controllers
{
    public class OperatingCenterController
        : ControllerBaseWithPersistence<IOperatingCenterRepository, OperatingCenter, User>
    {
        #region ByStateId

        [HttpGet]
        public ActionResult ByStateIds(int[] stateIds)
        {
            return
                new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(Repository.Where(oc => stateIds.Contains(oc.State.Id))) {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
        }

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return
                new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(Repository.GetByStateId(stateId)) {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
        }

        #endregion

        public OperatingCenterController(
            ControllerBaseWithPersistenceArguments<IOperatingCenterRepository, OperatingCenter, User> args) : base(args) { }
    }
}
