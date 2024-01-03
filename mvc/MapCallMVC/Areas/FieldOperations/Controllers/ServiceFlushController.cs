using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    // NOTE: If you add a property to this model, you need to add the field to the
    // ServiceFlush/Edit view as well as the Service/_AddFlush view.
    public class ServiceFlushController : ControllerBaseWithPersistence<ServiceFlush, User>
    {
        #region Consts

        // This should always have the same role as the ServiceController as these
        // records are always child records of a Service.
        private const RoleModules ROLE = ServiceController.ROLE;

        #endregion

        #region Constructor

        public ServiceFlushController(ControllerBaseWithPersistenceArguments<IRepository<ServiceFlush>, ServiceFlush, User> args) : base(args) { }

        #endregion

        #region Public Methods

        // MC-244: Only user admins can edit these records.
        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<ServiceFlushViewModel>(id);
        }

        // MC-244: Only user admins can edit these records.
        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(ServiceFlushViewModelBase model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var id = Repository.Find(model.Id).Service.Id;
                    return RedirectToAction("Show", "Service", new { id = id });
                }
            });
        }

        #endregion
    }
}