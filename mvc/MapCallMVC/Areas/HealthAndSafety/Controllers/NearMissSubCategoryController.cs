using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class NearMissSubCategoryController : ControllerBaseWithPersistence<NearMissSubCategory, User>
    {
        #region Constructor

        public NearMissSubCategoryController(
            ControllerBaseWithPersistenceArguments<IRepository<NearMissSubCategory>, NearMissSubCategory, User> args) :
            base(args) { }

        #endregion

        #region Cascades

        [HttpGet]
        public CascadingActionResult ByCategory(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.Category.Id == id), "Description", "Id");
        }

        #endregion
    }
}
