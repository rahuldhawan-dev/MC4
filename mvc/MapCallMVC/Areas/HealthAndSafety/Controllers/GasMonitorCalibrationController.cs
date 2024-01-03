using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.ClassExtensions;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class GasMonitorCalibrationController : ControllerBaseWithPersistence<GasMonitorCalibration, User>
    {
        #region Constants

        public const RoleModules ROLE = GasMonitorController.ROLE;

        #endregion

        #region Constructors

        public GasMonitorCalibrationController(ControllerBaseWithPersistenceArguments<IRepository<GasMonitorCalibration>, GasMonitorCalibration, User> args) : base(args) { }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchGasMonitorCalibration search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchGasMonitorCalibration search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGasMonitorCalibration>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditGasMonitorCalibration model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #endregion
    }
}
