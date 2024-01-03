using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.SAP.Controllers
{
    public class PlantMaintenanceActivityTypeController : ControllerBaseWithPersistence<IRepository<PlantMaintenanceActivityType>, PlantMaintenanceActivityType, User>
    {
        #region ByOrderTypeId

        [HttpGet]
        public ActionResult ByOrderTypeId(int id)
        {
            return new CascadingActionResult(
                Repository.Where(x => x.OrderType != null && x.OrderType.Id == id), "Description", "Id");
        }

        #endregion

        #region ByOrderTypeIds

        [HttpGet]
        public ActionResult ByOrderTypeIds(int[] ids)
        {
            return new CascadingActionResult(
                Repository.Where(x => x.OrderType != null && ids.Contains(x.OrderType.Id)), "Description", "Id");
        }

        #endregion

        public PlantMaintenanceActivityTypeController(ControllerBaseWithPersistenceArguments<IRepository<PlantMaintenanceActivityType>, PlantMaintenanceActivityType, User> args) : base(args) { }
    }
}
