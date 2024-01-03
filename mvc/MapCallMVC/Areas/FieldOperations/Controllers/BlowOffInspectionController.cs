using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class BlowOffInspectionController : ControllerBaseWithPersistence<IBlowOffInspectionRepository, BlowOffInspection, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;
        public const string SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ",
                            NOTIFICATION_PURPOSE_CHLORINE = "B O Chlorine Reading Outside Expected Limit";
        public const int IN_RANGE_VALUE = 1;

        #endregion

        #region Constructors

        public BlowOffInspectionController(ControllerBaseWithPersistenceArguments<IBlowOffInspectionRepository, BlowOffInspection, User> args) : base(args) {}

        #endregion

        #region Private Methods

        private bool ChlorineLevelsOutOfRange(BlowOffInspection model)
        {
            var totalChlorine = model.TotalChlorine ?? IN_RANGE_VALUE;
            var residualChlorine = model.ResidualChlorine ?? IN_RANGE_VALUE;

            if (totalChlorine > model.MAX_CHLORINE_LEVEL || totalChlorine < model.MIN_CHLORINE_LEVEL ||
                residualChlorine > model.MAX_CHLORINE_LEVEL || residualChlorine < model.MIN_CHLORINE_LEVEL)
            {
                return true;
            }
            return false;
        }

        private void SendChlorineNotification(BlowOffInspection model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "BlowOffInspection", "FieldOperations");
            model.Valve.RecordUrl = GetUrlForModel(model.Valve, "Show", "valve", "FieldOperations");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                 OperatingCenterId = model.Valve.OperatingCenter.Id,
                 Module = ROLE,
                 Purpose = NOTIFICATION_PURPOSE_CHLORINE, 
                 Data = model
            };

            notifier.Notify(args);
        }

        // TODO: Refactor, there are 4 of these
        // Difficult because of this - new SAPInspection(entity)
        private void UpdateSAP(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                throw new InvalidOperationException("The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            try
            {
                var inspection = new SAPInspection(entity);
                var sapEquipment = _container.GetInstance<ISAPInspectionRepository>().Save(inspection);
                if (sapEquipment.SAPNotificationNumber != null)
                    entity.SAPNotificationNumber = sapEquipment.SAPNotificationNumber;
                entity.SAPErrorCode = sapEquipment.SAPErrorCode;
                entity.BusinessUnit = sapEquipment.CostCenter;
            }
            catch (Exception ex)
            {
                entity.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
            }
            Repository.Save(entity);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<HydrantInspectionType>();
            this.AddDropDownData<HydrantProblem>();

            // Cache this so we're not sending the same query four times.
            var woRequests = _container.GetInstance<IRepository<WorkOrderRequest>>().GetAll().ToList();
            this.AddDropDownData("WorkOrderRequestOne", woRequests, x => x.Id, x => x.Description);
            this.AddDropDownData("WorkOrderRequestTwo", woRequests, x => x.Id, x => x.Description);
            this.AddDropDownData("WorkOrderRequestThree", woRequests, x => x.Id, x => x.Description);
            this.AddDropDownData("WorkOrderRequestFour", woRequests, x => x.Id, x => x.Description);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData("OperatingCenter");
            }
        }

        #endregion

        #region Search/Show/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchBlowOffInspection>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchBlowOffInspection model)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchInspections(model)
                }));
                formatter.Map(() => {
                    // These are all hardcoded because the map icons must be sorted in this order
                    // for the lines to draw correctly.
                    model.EnablePaging = false;
                    model.SortBy = null; // No sorting.

                    // Bug 2432: Both the hydrant and blowoff inspections need to be displayed at the same time even 
                    // though they do not both show up in the regular table of results.
                    var hydrantSearch = new SearchHydrantInspection {
                        CreatedAt = model.CreatedAt,
                        DateInspected = model.DateInspected,
                        EnablePaging = model.EnablePaging,
                        // FireDistrict
                        HydrantInspectionType = model.HydrantInspectionType,
                        // HydrantSuffix
                        InspectedBy = model.InspectedBy,
                        OperatingCenter = model.OperatingCenter,
                        // Route
                        // SAPEquipmentId
                        // SAPEquipmentOnly
                        Town = model.Town,
                        WorkOrderRequestOne = model.WorkOrderRequestOne,
                        WorkOrderRequired = model.WorkOrderRequired
                    };
                    
                    var blowOffCoords = Repository.SearchBlowOffInspectionsForMap(model);
                    var hydCoords = _container.GetInstance<IHydrantInspectionRepository>().SearchHydrantInspectionsForMap(hydrantSearch);
                    var amr = _container.GetInstance<AssetMapResult>();
                    amr.InitializeWithInspectionLineLayer(blowOffCoords, hydCoords);
                    return amr;
                });
                formatter.Excel(() => ActionHelper.DoExcel(model, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchInspections(model)
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [SkipRoleOperatingCenterCheck] // RoleAuthorizer does not know the id param is for a valve and not a BlowOffInspection.
        [HttpGet, ActionBarVisible(false), RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int id)
        {
            var valve = _container.GetInstance<IValveRepository>().Find(id);
            if (valve == null)
                return DoHttpNotFound("Valve not found.");

            var model = new CreateBlowOffInspection(_container) {Valve = valve.Id};
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBlowOffInspection model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id);
                    }
                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully saved!");
                    }
                    if (ChlorineLevelsOutOfRange(Repository.Find(model.Id)))
                    {
                        SendChlorineNotification(Repository.Find(model.Id));
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
            return ActionHelper.DoEdit<EditBlowOffInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBlowOffInspection model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id);
                    }
                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully updated!");
                    }
                    if (ChlorineLevelsOutOfRange(Repository.Find(model.Id)))
                    {
                        SendChlorineNotification(Repository.Find(model.Id));
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