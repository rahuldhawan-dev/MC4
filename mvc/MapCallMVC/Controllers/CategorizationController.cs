using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class GrievanceCategorizationController : ControllerBaseWithPersistence<GrievanceCategorization, User>
    {
        #region ByCategoryIdOrAll

        [HttpGet]
        public ActionResult ByCategoryIdOrAll(int? categoryId)
        {
            var results = (categoryId.HasValue)
                ? Repository.Where(x => x.GrievanceCategory.Id == categoryId)
                : Repository.GetAll();
            return new CascadingActionResult(results, "Description", "Id");
        }

        #endregion

        #region Constructors

        public GrievanceCategorizationController(ControllerBaseWithPersistenceArguments<IRepository<GrievanceCategorization>, GrievanceCategorization, User> args)
            : base(args) { }

        #endregion
    }
}