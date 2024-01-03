using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class TrainingRequirementController : ControllerBaseWithPersistence<IRepository<TrainingRequirement>, TrainingRequirement, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingModules;
        public const string MISSING_TRAINING_MODULES = "The current record has incomplete active training modules.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDynamicDropDownData<Regulation, RegulationDisplayItem>();
                    this.AddDynamicDropDownData<Regulation, RegulationOSHADisplayItem>("OSHAStandard",
                        repo => repo.Where(r => r.Status.Description == "Active" && r.Agency.Description == "OSHA"));
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchTrainingRequirement search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = false
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, null, model => {
                if (!model.HasActiveTrainingModules)
                {
                    DisplayNotification(MISSING_TRAINING_MODULES);
                }
            });
        }
        
        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchTrainingRequirement>();
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new TrainingRequirementViewModel(_container));
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(TrainingRequirementViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<TrainingRequirement, TrainingRequirementViewModel>{
                NotFound = $"Training Requirement with id {id} was not found."
            }, entity => {
                ViewData["ActiveInitialTrainingModule"] = entity.InitialTrainingModules.Where(x => x.IsActive == true).Select(x => new SelectListItem() { Text = x.Display, Value = x.Id.ToString() });
                ViewData["ActiveRecurringTrainingModule"] = entity.RecurringTrainingModules.Where(x => x.IsActive == true).Select(x => new SelectListItem() { Text = x.Display, Value = x.Id.ToString() });
                ViewData["ActiveInitialAndRecurringTrainingModule"] = entity.InitialAndRecurringTrainingModules.Where(x => x.IsActive == true).Select(x => new SelectListItem() { Text = x.Display, Value = x.Id.ToString() });
            });
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(TrainingRequirementViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE_MODULE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            // TODO: Why does this redirect to Index rather than Search?
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccessRedirectAction = "Index"
            });
        }

        #endregion

        public TrainingRequirementController(ControllerBaseWithPersistenceArguments<IRepository<TrainingRequirement>, TrainingRequirement, User> args) : base(args) {}
    }
}
