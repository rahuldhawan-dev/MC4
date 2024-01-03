using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class ServiceRestorationContractorController : ControllerBaseWithPersistence<IServiceRestorationContractorRepository, ServiceRestorationContractor, User>
    {
        #region Constants

        public const RoleModules ROLE = ServiceRestorationController.ROLE;

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        protected ActionResult RedirectIfHasRestorations(int id, string action, Func<int, ActionResult> orElse)
        {
            var record = Repository.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }

            if (!record.HasRestorations)
            {
                return orElse(id);
            }

            DisplayErrorMessage($"Cannot {action} a contractor record with linked restorations.");
            return RedirectToReferrerOr("Search", "ServiceRestorationContractor");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchServiceRestorationContractor search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchServiceRestorationContractor search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateServiceRestorationContractor model)
        {
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateServiceRestorationContractor model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => RedirectToAction("Index")
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return RedirectIfHasRestorations(id, "edit", x => ActionHelper.DoEdit<EditServiceRestorationContractor>(x));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditServiceRestorationContractor model)
        {
            return RedirectIfHasRestorations(model.Id, "edit", _ => ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Index")
            }));
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            // NOTE: This redirects to the Index action because users can only delete these records
            // from that view. There is no Show view.
            return RedirectIfHasRestorations(id, "delete",
                x => ActionHelper.DoDestroy(x, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                    OnSuccess = () => RedirectToAction("Index")
                }));
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(id), "Description", "Id");
        }

        #endregion

        public ServiceRestorationContractorController(ControllerBaseWithPersistenceArguments<IServiceRestorationContractorRepository, ServiceRestorationContractor, User> args) : base(args) {}
    }
}