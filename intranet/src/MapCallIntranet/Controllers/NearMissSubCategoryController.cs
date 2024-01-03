using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallIntranet.Controllers
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
        public CascadingActionResult ByCategory(int categoryId)
        {
            return new CascadingActionResult(Repository.Where(x => x.Category.Id == categoryId), "Description", "Id");
        }

        #endregion
    }
}
