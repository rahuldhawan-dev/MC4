using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    public class CutoffSawQuestionnaireController : ControllerBaseWithPersistence<IRepository<CutoffSawQuestionnaire>, CutoffSawQuestionnaire, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDynamicDropDownData<Employee, EmployeeDisplayItem>("LeadPerson");
                    this.AddDynamicDropDownData<Employee, EmployeeDisplayItem>("SawOperator");
                    this.AddDropDownData<IRepository<PipeDiameter>, PipeDiameter>(
                        e => e.GetAllSorted(d => d.Diameter), e => e.Id, e => e.Diameter);
                    this.AddDropDownData<PipeMaterial>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Read)]
        public ActionResult Search(SearchCutoffSawQuestionnaire search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x =>
            {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Read)]
        public ActionResult Index(SearchCutoffSawQuestionnaire search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Read)]
        public ActionResult New()
        {
            var questions = _container.GetInstance<ICutoffSawQuestionRepository>().GetActiveQuestions().ToList();
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var model = new CreateCutoffSawQuestionnaire(_container) { OperatedOn = now };
            foreach (var q in questions)
            {
                model.CutoffSawQuestions.Add(ViewModelFactory.Build<CutoffSawQuestionViewModel, CutoffSawQuestion>(q));
            }
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Read)]
        public ActionResult Create(CreateCutoffSawQuestionnaire model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public CutoffSawQuestionnaireController(ControllerBaseWithPersistenceArguments<IRepository<CutoffSawQuestionnaire>, CutoffSawQuestionnaire, User> args) : base(args) {}
    }
}