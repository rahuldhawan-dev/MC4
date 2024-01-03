using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.ClassExtensions;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class PlanReviewController : ControllerBaseWithPersistence<PlanReview, User>
    {
        #region Constants

        public const RoleModules ROLE = EmergencyResponsePlanController.ROLE;
        public const string PLAN_ID_NOT_FOUND_STATUS_DESCRIPTION = "The plan id could not be found.";

        #endregion

        #region Constructors

        public PlanReviewController(ControllerBaseWithPersistenceArguments<IRepository<PlanReview>, PlanReview, User> args) : base(args) { }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchPlanReview search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchPlanReview search)
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

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(NewPlanReview viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound(PLAN_ID_NOT_FOUND_STATUS_DESCRIPTION);
            }

            var createViewModel = ViewModelFactory.BuildWithOverrides<CreatePlanReview>(new {
                viewModel.Plan
            });

            return ActionHelper.DoNew(createViewModel);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePlanReview model)
        {
            return ActionHelper.DoCreate(model);
        }

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<PlanReviewViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(PlanReviewViewModel model)
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