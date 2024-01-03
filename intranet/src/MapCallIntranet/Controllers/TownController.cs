using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallIntranet.Controllers
{
    [DisplayName("Municipalities")]
    public class TownController : ControllerBaseWithPersistence<ITownRepository, Town, User>
    {
        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(operatingCenterId), "ShortName", "Id");
        }

        [HttpGet]
        public ActionResult ByOperatingCenterIds(int[] operatingCenterIds)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterIds(operatingCenterIds), "ShortName", "Id");
        }

        #endregion

        public TownController(ControllerBaseWithPersistenceArguments<ITownRepository, Town, User> args) : base(args) { }
    }
}
