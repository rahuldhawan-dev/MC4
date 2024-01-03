using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class CountyController : ControllerBaseWithPersistence<ICountyRepository, County, User>
    {
        #region ByStateId

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return new CascadingActionResult(Repository.GetByStateId(stateId), "Name", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        public CountyController(ControllerBaseWithPersistenceArguments<ICountyRepository, County, User> args) : base(args) {}
    }
}