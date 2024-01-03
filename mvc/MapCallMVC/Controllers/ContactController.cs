using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class ContactController : ControllerBaseWithPersistence<IContactRepository, Contact, User>
    {
        #region Constants

        private const string CONTACT_NOT_FOUND = "Contact does not exist.";

        public const string CONTACT_CAN_NOT_BE_DELETED =
            "This contact can not be deleted because it is currently referenced by another record.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDropDownData<State>("Address.State", s => s.Id, s => s.Name);
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<State>(s => s.Id, s => s.Name);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Index(SearchContact model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet]
        public ActionResult Search(SearchContact model)
        {
            return ActionHelper.DoSearch(model);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new ContactViewModel(_container));
        }

        [HttpPost]
        [RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Add)]
        public ActionResult Create(ContactViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<ContactViewModel>(id);
        }

        [HttpPost]
        [RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Edit)]
        public ActionResult Update(ContactViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        [RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            // TODO: This should be a view model with validation rather
            // than doing validation and not found checking stuff in 
            // the action. -Ross 4/7/2020
            var contact = Repository.Find(id);
            if (contact == null)
            {
                return HttpNotFound(CONTACT_NOT_FOUND);
            }
            
            if (!Repository.CanDelete(contact))
            {
                ModelState.AddModelError("", CONTACT_CAN_NOT_BE_DELETED);
            }

            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ByPartialNameMatch

        [HttpGet]
        public ActionResult ByPartialNameMatch(string partialName)
        {
            var results = Repository.FindByPartialNameMatch(partialName);
            return new AutoCompleteResult(results, "Id", "ContactName");
        }

        #endregion

        public ContactController(ControllerBaseWithPersistenceArguments<IContactRepository, Contact, User> args) : base(args) {}
    }
}
