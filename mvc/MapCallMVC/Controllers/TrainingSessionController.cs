using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class TrainingSessionController : ControllerBaseWithPersistence<ITrainingSessionRepository, TrainingSession, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;
        public const string NOT_FOUND = "Training Session with the id: {0} could not be found.";

        #endregion

        #region Constructors

        public TrainingSessionController(ControllerBaseWithPersistenceArguments<ITrainingSessionRepository, TrainingSession, User> args) : base(args) {}

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTrainingSession>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTrainingSession model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "TrainingRecord", new { id = model.TrainingRecordId })
            });
        }

        #endregion
    }
}