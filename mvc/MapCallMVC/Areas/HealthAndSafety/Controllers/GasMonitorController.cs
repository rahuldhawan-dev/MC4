using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.ClassExtensions;
using MapCallMVC.ClassExtensions;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class GasMonitorController : ControllerBaseWithPersistence<IGasMonitorRepository, GasMonitor, User>
    {
        #region Constants

        // TODO: I feel like this role should be renamed to OperationsForms maybe?
        public const RoleModules ROLE = RoleModules.OperationsLockoutForms;

        #endregion

        #region Constructors

        public GasMonitorController(
            ControllerBaseWithPersistenceArguments<IGasMonitorRepository, GasMonitor, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.New)
            {
                this.AddOperatingCenterDropDownData(x => x.IsActive);
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchGasMonitor search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchGasMonitor search)
        {
            return this.RespondTo((formatter) => {
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

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateGasMonitor>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateGasMonitor model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGasMonitor>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditGasMonitor model)
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

        #region Add/Remove Calibration

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddGasMonitorCalibration(AddGasMonitorCalibration model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveGasMonitorCalibration(RemoveGasMonitorCalibration model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #endregion
    }
}
