using System.Text;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallApi.Models.ShortCycleWorkOrders;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using Newtonsoft.Json;

namespace MapCallApi.Controllers
{
    public class ShortCycleWorkOrderSafetyBriefController : ControllerBaseWithPersistence<IRepository<ShortCycleWorkOrderSafetyBrief>, ShortCycleWorkOrderSafetyBrief, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #endregion

        #region Constructors

        public ShortCycleWorkOrderSafetyBriefController(ControllerBaseWithPersistenceArguments<IRepository<ShortCycleWorkOrderSafetyBrief>, ShortCycleWorkOrderSafetyBrief, User> args) : base(args) { }

        #endregion

        #region Exposed Methods
        
        [RequiresRole(ROLE)]
        public ActionResult Index(SearchShortCycleWorkOrderSafetyBrief search)
        {
            return Content(
                JsonConvert.SerializeObject(
                    Repository.Search(search),
                    new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                "application/json",
                Encoding.UTF8);
        }

        #endregion
    }
}
