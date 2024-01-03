using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MMSINC;

namespace MapCallIntranet.Controllers
{
    public class PublicWaterSupplyController : ControllerBaseWithPersistence<IPublicWaterSupplyRepository, PublicWaterSupply, User>
    {
        #region GetSystemNameByOperatingCenterId

        [HttpGet]
        public ActionResult GetSystemNameByOperatingCenter(int operatingCenterId)
        {
            return new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItemForNearMiss>(Repository.GetByOperatingCenterId(operatingCenterId).AsQueryable());
        }

        #endregion

        #region Constructors

        public PublicWaterSupplyController(ControllerBaseWithPersistenceArguments<IPublicWaterSupplyRepository, PublicWaterSupply, User> args) : base(args) { }

        #endregion
    }
}
