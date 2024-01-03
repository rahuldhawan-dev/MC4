using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;
using System.Web.Mvc;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SewerOpeningInspectionController : SapSyncronizedControllerBaseWithPersisence<ISewerOpeningInspectionRepository, SewerOpeningInspection, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public SewerOpeningInspectionController(
            ControllerBaseWithPersistenceArguments<ISewerOpeningInspectionRepository, SewerOpeningInspection, User> args)
            : base(args) { }

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(SewerOpeningInspection entity) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchSewerOpeningInspection>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: DisplaySapErrorIfApplicable);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSewerOpeningInspection search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                {
                    SearchOverrideCallback = () => Repository.SearchInspections(search)
                }));
                formatter.Excel(() =>
                {
                    search.EnablePaging = false;
                    var data = Repository.SearchInspections(search);
                    return this.Excel(data);
                });
            });
        }

        #endregion

        #region New/Create

        [SkipRoleOperatingCenterCheck]
        [HttpGet, ActionBarVisible(false), RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int id)
        {
            var sewerOpening = _container.GetInstance<ISewerOpeningRepository>().Find(id);
            if (sewerOpening == null)
                return HttpNotFound("SewerOpening not found");

            var model = new CreateSewerOpeningInspection(_container);
            model.SewerOpening = sewerOpening.Id;
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSewerOpeningInspection model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () =>
                {
                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully saved!");
                    }

                    return RedirectToAction(action, new { id = model.Id });
                }
            }
            );
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSewerOpeningInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSewerOpeningInspection model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs
            {
                OnSuccess = () =>
                {
                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully updated!");
                    }

                    return RedirectToAction(action, new { id = model.Id });
                }
            });
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}