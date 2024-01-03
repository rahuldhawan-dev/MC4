using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallApi.Controllers
{
    public class WorkOrderController : ApiControllerBaseWithPersistence<IRepository<WorkOrder>, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion
        
        #region Constructors

        public WorkOrderController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Private Methods

        /// <summary>
        /// This exists solely for the unit tests for this controller. These tests are doing validation testing
        /// rather than view model tests. They need to be rewritten.
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        public void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }

        #endregion

        #region Public Methods

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateInstallationWorkOrder model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => Json(new { model.Id }),
                OnError = GetError
            });
        }

        #endregion
    }
}
