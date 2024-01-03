using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class CrewController : ControllerBaseWithPersistence<CrewRepository, Crew, User>
    {
        public CrewController(ControllerBaseWithPersistenceArguments<CrewRepository, Crew, User> args) : base(args) { }

        [HttpGet]
        public ActionResult ByOperatingCenterOrAll(int? opc)
        {
            IQueryable<Crew> data = null;
            if (opc.HasValue)
            {
                data = Repository.GetAll().Where(x => x.OperatingCenter.Id == opc.Value && x.Active);
            }
            else
            {
                data = Repository.GetAllSorted(x => x.Id).Where(x => x.Active);
            }
            return new CascadingActionResult(data, "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCenter.Id == operatingCenterId));
        }

        #endregion
    }
}
