using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class RecurringProjectMainController : ControllerBaseWithPersistence<RecurringProjectMain, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion
        
        #region Constructors

        public RecurringProjectMainController(ControllerBaseWithPersistenceArguments<IRepository<RecurringProjectMain>, RecurringProjectMain, User> args) : base(args) {}

        #endregion

        #region Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            var results = Repository.Where(x => x.RecurringProject.Id == id);
            // TODO: This will never be null.
            if (results == null)
                return HttpNotFound();
            return View(results);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Edit(int id )
        {
            var results = Repository.Where(x => x.RecurringProject.Id == id);
            // TODO: This will never be null.
            if (results == null)
                return HttpNotFound();
            return View(results);
        }

        #endregion
    }
}