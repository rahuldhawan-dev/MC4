using System.Web.Mvc;
using MapCall.Common.Authentication;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Contractors.Controllers
{
    public class ContractorUserController : ControllerBaseWithPersistence<ContractorUser, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.ContractorsGeneral;

        #endregion

        #region Constructor

        public ContractorUserController(ControllerBaseWithPersistenceArguments<IRepository<ContractorUser>, ContractorUser, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void DisplayPasswordRequirementNotification()
        {
            DisplayNotification(ContractorUserCredentialPolicy.REQUIREMENTS_MESSAGE);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchContractorUser>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchContractorUser search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search));
                f.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "Index",
                    OnNoResults = () => PartialView("_NoResults")
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            DisplayPasswordRequirementNotification();
            return ActionHelper.DoNew(new CreateContractorUser(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateContractorUser model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs()
            {
                OnError = () => {
                    DisplayPasswordRequirementNotification();
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            // TODO: This is needlessly using TempData to render static information to the view. 
            DisplayPasswordRequirementNotification();
            return ActionHelper.DoEdit<EditContractorUser>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditContractorUser model)
        {
            // Needs to be displayed again if validation fails.
            var args = new ActionHelperDoUpdateArgs();
            args.OnError = () => {
                DisplayPasswordRequirementNotification();
                return null;
            };
            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        // ContractorUsers weren't allowed to be deleted on the contractors portal, so it's not here either. 

    }
}
