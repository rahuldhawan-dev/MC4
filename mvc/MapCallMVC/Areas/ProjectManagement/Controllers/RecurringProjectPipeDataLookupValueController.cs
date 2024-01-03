using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class RecurringProjectPipeDataLookupValueController : ControllerBaseWithPersistence<IRepository<RecurringProjectPipeDataLookupValue>, RecurringProjectPipeDataLookupValue, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion
        
        #region Constructors

        public RecurringProjectPipeDataLookupValueController(ControllerBaseWithPersistenceArguments<IRepository<RecurringProjectPipeDataLookupValue>, RecurringProjectPipeDataLookupValue, User> args): base(args) {}

        #endregion

        #region Private Methods

        private void SetShowLookupData(int pipeDataLookupTypeId)
        {
            this.AddDropDownData<PipeDataLookupValue>(x => x.Where(y => y.IsEnabled && y.PipeDataLookupType.Id == pipeDataLookupTypeId), x => x.Id, x => x.Description);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRecurringProjectPipeDataLookupValue>(id, null, onModelFound:
                (entity) => {
                    // TODO: ActionHelper.DoEdit needs an OnViewModelCreated type argument.
                    var model = ViewModelFactory.Build<EditRecurringProjectPipeDataLookupValue, RecurringProjectPipeDataLookupValue>(entity);
                    SetShowLookupData(model.DisplayPipeDataLookupValue.PipeDataLookupType.Id);
                });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRecurringProjectPipeDataLookupValue model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "RecurringProject", new { area = "ProjectManagement", id = model.RecurringProject })
            });
        }

        #endregion
    }
}