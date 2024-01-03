using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FleetManagement.Models;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Areas.FleetManagement.Controllers
{
    public class VehicleController : ControllerBaseWithPersistence<IVehicleRepository, Vehicle, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FleetManagementGeneral;

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            var employees = new List<Employee>();
            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    employees = _container.GetInstance<IEmployeeRepository>().GetAll()
                                          .Where(x => x.Status != null && x.Status.Id == EmployeeStatus.Indices.ACTIVE).ToList();
                    ViewData["Manager"] = employees;
                    ViewData["FleetContactPerson"] = employees;
                    break;
                case ControllerAction.Edit:
                    // For performance reasons, we only wanna query the db for all the employees once.
                    employees = _container.GetInstance<IEmployeeRepository>().GetAllSorted().ToList();
                    ViewData["Manager"] = employees;
                    ViewData["FleetContactPerson"] = employees;
                    break;
            }
          
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchVehicle>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchVehicle model)
        {
            return this.RespondTo(x =>
            {
                x.Excel(() => ActionHelper.DoExcel(model));
                x.View(() => ActionHelper.DoIndex(model));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateVehicle(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateVehicle model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditVehicle>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditVehicle model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddVehicleAudit(CreateVehicleAudit model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #region ByOperatingCenterId

        [HttpGet] // This has no role requirement. Bug 1784.
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "Description", "Id");
        }

        #endregion

        #endregion

        #region Constructor

        public VehicleController(ControllerBaseWithPersistenceArguments<IVehicleRepository, Vehicle, User> args) : base(args) { }

        #endregion
    }
}
