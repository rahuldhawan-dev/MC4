using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Controllers
{
    public class TrainingContactHoursProgramCoordinatorController : ControllerBaseWithPersistence<ITrainingContactHoursProgramCoordinatorRepository, TrainingContactHoursProgramCoordinator, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddOperatingCenterDropDownData();
            this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("ProgramCoordinator");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchTrainingContactHoursProgramCoordinator search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTrainingContactHoursProgramCoordinator search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTrainingContactHoursProgramCoordinator(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateTrainingContactHoursProgramCoordinator model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTrainingContactHoursProgramCoordinator>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTrainingContactHoursProgramCoordinator model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public TrainingContactHoursProgramCoordinatorController(ControllerBaseWithPersistenceArguments<ITrainingContactHoursProgramCoordinatorRepository, TrainingContactHoursProgramCoordinator, User> args) : base(args) {}
    }
}
