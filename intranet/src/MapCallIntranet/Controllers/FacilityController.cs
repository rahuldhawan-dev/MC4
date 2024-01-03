using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallIntranet.Helpers;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Controllers;
using Facility = MapCall.Common.Model.Entities.Facility;

namespace MapCallIntranet.Controllers
{
    public class FacilityController : ControllerBaseWithPersistence<IFacilityRepository, Facility, User>
    {
        #region GetFacilityBy

        [Authorize]
        [HttpGet]
        public ActionResult GetFacilityBy(int operatingCenterId)
        {
            LoginHelper.DoLogin();
            return new CascadingActionResult<Facility, FacilityDisplayItem>(
                Repository.GetByOperatingCenterId(operatingCenterId).OrderBy(x => x.OperatingCenter.OperatingCenterCode)
                          .ThenBy(x => x.Id), "Display");
        }

        #endregion

        #region Constructors

        public FacilityController(ControllerBaseWithPersistenceArguments<IFacilityRepository, Facility, User> args) : base(args) { }

        #endregion
    }
}
