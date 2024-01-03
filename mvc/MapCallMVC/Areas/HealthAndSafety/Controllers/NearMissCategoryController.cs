using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class NearMissCategoryController : ControllerBaseWithPersistence<NearMissCategory, User>
    {
        #region Constructor

        public NearMissCategoryController(ControllerBaseWithPersistenceArguments<IRepository<NearMissCategory>, NearMissCategory, User> args) : base(args) { }

        #endregion

        #region Cascades

        [HttpGet]
        public CascadingActionResult ByType(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.Type.Id == id), "Description", "Id");
        }

        #endregion
    }
}
