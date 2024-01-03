using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class RecurringProjectEndorsementController : ControllerBaseWithPersistence<IRepository<RecurringProjectEndorsement>, RecurringProjectEndorsement, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion

        #region Constructors

        public RecurringProjectEndorsementController(
            ControllerBaseWithPersistenceArguments
                <IRepository<RecurringProjectEndorsement>, RecurringProjectEndorsement, User> args) : base(args) {}

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRecurringProjectEndorsement>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRecurringProjectEndorsement model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => RedirectToAction("Show", "RecurringProject", new { area = "ProjectManagement", id = model.RecurringProject })
            });
        }


        #endregion
    }
}