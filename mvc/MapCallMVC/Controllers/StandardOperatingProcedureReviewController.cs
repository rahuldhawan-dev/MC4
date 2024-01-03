using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class StandardOperatingProcedureReviewController : ControllerBaseWithPersistence<IStandardOperatingProcedureReviewRepository, StandardOperatingProcedureReview, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ManagementGeneral;

        #endregion

        #region Constructor
         
        public StandardOperatingProcedureReviewController(ControllerBaseWithPersistenceArguments<IStandardOperatingProcedureReviewRepository, StandardOperatingProcedureReview, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            var model = new SOPWrapModel();

            try
            {
                model.ReviewableProcedures =
                    Repository.GetStandardOperatingProcedureReviewsDueForUser(AuthenticationService.CurrentUser);
            }
            catch (StandardOperatingProcedureReviewException ex)
            {
                DisplayErrorMessage(ex.Message);
            }

            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchStandardOperatingProcedureReview search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet,RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int standardOperatingProcedure)
        {
            var model = new CreateStandardOperatingProcedureReview(_container) {
                StandardOperatingProcedure = standardOperatingProcedure
            };

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateStandardOperatingProcedureReview model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)] // Bug 2662: Yes it's user admin and not edit.
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditStandardOperatingProcedureReview>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)] // Bug 2662: Yes it's user admin and not edit.
        public ActionResult Update(EditStandardOperatingProcedureReview model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

    }
}