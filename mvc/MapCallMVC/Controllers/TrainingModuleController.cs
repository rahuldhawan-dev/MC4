using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class TrainingModuleController : ControllerBaseWithPersistence<IRepository<TrainingModule>, TrainingModule, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingModules;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Show:
                    this.AddDropDownData<TrainingRequirement>();
                    break;
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDropDownData<ProcessStage>();
                    this.AddDropDownData<TrainingModuleCategory>();
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<TrainingModuleRecurrantType>();
                    this.AddDropDownData<TrainingRequirement>();
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<IRepository<TrainingModuleCategory>, TrainingModuleCategory>("TrainingModuleCategory", t => t.GetAllSorted(), t => t.Id, t => t.Description);
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchTrainingModule>();
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchTrainingModule search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = false
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTrainingModule(_container));
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreateTrainingModule model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTrainingModule>(id);
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditTrainingModule model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE_MODULE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult GetByOperatingCenter(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.Where(m => m.OperatingCenter.Id == operatingCenterId), "Description", "Id");
        }

        public TrainingModuleController(ControllerBaseWithPersistenceArguments<IRepository<TrainingModule>, TrainingModule, User> args) : base(args) {}
    }
}
