using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class StandardOperatingProcedureQuestionController : ControllerBaseWithPersistence<StandardOperatingProcedureQuestion, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ManagementGeneral;

        #endregion

        #region Constructors

        public StandardOperatingProcedureQuestionController(ControllerBaseWithPersistenceArguments<IRepository<StandardOperatingProcedureQuestion>, StandardOperatingProcedureQuestion, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private ActionResult ActivateOrNot(int id, bool yallWannaActivateThatOrWhat)
        {
            var model = Repository.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.IsActive = yallWannaActivateThatOrWhat;
            Repository.Save(model);

            return RedirectToAction("Show", "StandardOperatingProcedure", new { id = model.StandardOperatingProcedure.Id });
        }

        #endregion

        #region Public Methods

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Activate(int id)
        {
            return ActivateOrNot(id, true);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Deactivate(int id)
        {
            return ActivateOrNot(id, false);
        }

        #endregion
    }
}