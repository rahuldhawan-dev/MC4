using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ValveInspectionController : SapSyncronizedControllerBaseWithPersisence<IValveInspectionRepository, ValveInspection, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public ValveInspectionController(
            ControllerBaseWithPersistenceArguments<IValveInspectionRepository, ValveInspection, User> args)
            : base(args) { }

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(ValveInspection entity)
        {
            var inspection = new SAPInspection(entity);
            var sapEquipment = _container.GetInstance<ISAPInspectionRepository>().Save(inspection);
            if (sapEquipment.SAPNotificationNumber != null)
                entity.SAPNotificationNumber = sapEquipment.SAPNotificationNumber;
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }
        
        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<ValveNormalPosition>("PositionFound");
            this.AddDropDownData<ValveNormalPosition>("PositionLeft");
            this.AddDropDownData<ValveWorkOrderRequest>("WorkOrderRequestOne");
            this.AddDropDownData<ValveWorkOrderRequest>("WorkOrderRequestTwo");
            this.AddDropDownData<ValveWorkOrderRequest>("WorkOrderRequestThree");
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData("OperatingCenter");
                this.AddDropDownData<ValveZone>("ValveZone");
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchValveInspection>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: DisplaySapErrorIfApplicable);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchValveInspection search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchInspections(search)
                }));
                formatter.Map(() =>
                {
                    // These are all hardcoded because the map icons must be sorted in this order
                    // for the lines to draw correctly.
                    search.EnablePaging = false;
                    // NOTE: Unlike HydrantInspections and BlowOffInspections, ValveInspections can do sql sorting by DateInspected.
                    search.SortAscending = true;
                    search.SortBy = "DateInspected";

                    var coords = Repository.SearchValveInspectionsForMap(search);
                    var result = _container.GetInstance<AssetMapResult>();
                    result.InitializeWithInspectionLineLayer(coords);
                    return result;
                });
                formatter.Excel(() => ActionHelper.DoExcel(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                {
                    SearchOverrideCallback = () => Repository.SearchInspections(search)
                }));
            });
        }

        #endregion

        #region New/Create

        [SkipRoleOperatingCenterCheck] // RoleAuthorizer does not know that this id is a Valve and not a ValveInspection.
        [HttpGet, ActionBarVisible(false), RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int id)
        {
            var valve = _container.GetInstance<IValveRepository>().Find(id);
            if (valve == null)
                return HttpNotFound("Valve not found");

            var model = new CreateValveInspection(_container);
            model.Valve = valve.Id;
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateValveInspection model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully saved!");
                    }
                    return RedirectToAction(action, new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditValveInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditValveInspection model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

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