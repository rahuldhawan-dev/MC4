using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.Linq;
using System.Web.Mvc;

namespace MapCallIntranet.Controllers
{
    public class WasteWaterSystemController : ControllerBaseWithPersistence<IRepository<WasteWaterSystem>, WasteWaterSystem, User>
    {
        #region GetSystemNameByOperatingCenterId

        [HttpGet]
        public ActionResult GetSystemNameByOperatingCenter(int operatingCenterId)
        {
            var data = Repository.Where(x => x.OperatingCenter.Id == operatingCenterId);
            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(data.OrderBy(x =>
                x.WasteWaterSystemName));
        }

        #endregion

        public WasteWaterSystemController(ControllerBaseWithPersistenceArguments<IRepository<WasteWaterSystem>, WasteWaterSystem, User> args) : base(args) { }
    }
}
