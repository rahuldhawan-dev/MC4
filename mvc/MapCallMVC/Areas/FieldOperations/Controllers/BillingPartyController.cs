using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class BillingPartyController : ControllerBaseWithPersistence<BillingParty, User>
    {
        #region Constants

        public const RoleModules ROLE = TrafficControlTicketController.ROLE;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDynamicDropDownData<BillingPartyContactType, BillingPartyContactTypeDisplayItem>("ContactType");
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchBillingParty search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchBillingParty search)
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
            return ActionHelper.DoNew(new CreateBillingParty(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBillingParty model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBillingParty>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBillingParty model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region BillingPartyContact

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult CreateBillingPartyContact(CreateBillingPartyContact model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult DestroyBillingPartyContact(DestroyBillingPartyContact model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public BillingPartyController(ControllerBaseWithPersistenceArguments<IRepository<BillingParty>, BillingParty, User> args) : base(args) {}
    }
}