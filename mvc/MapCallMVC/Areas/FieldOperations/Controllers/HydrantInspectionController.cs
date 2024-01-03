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
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class HydrantInspectionController : SapSyncronizedControllerBaseWithPersisence<IHydrantInspectionRepository, HydrantInspection, User>
    {

        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;
        public const string NOTIFICATION_PURPOSE_CHLORINE = "Hydrant Chlorine Reading Outside Expected Limit"; 
        public const int IN_RANGE_VALUE = 1;
        
        #endregion

        #region Constructor

        public HydrantInspectionController(ControllerBaseWithPersistenceArguments<IHydrantInspectionRepository, HydrantInspection, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private bool ChlorineLevelsOutOfRange(HydrantInspection model)
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
        
        private void SendChlorineNotification(HydrantInspection model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "HydrantInspection", "FieldOperations");
            model.Hydrant.RecordUrl = GetUrlForModel(model.Hydrant, "Show", "Hydrant", "FieldOperations");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE_CHLORINE,
                Data = model
            };

            notifier.Notify(args);
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<HydrantInspectionType>();
            this.AddDropDownData<HydrantTagStatus>();

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

        protected override void UpdateEntityForSap(HydrantInspection entity)
        {
            var sapEquipment = _container.GetInstance<ISAPInspectionRepository>().Save(new SAPInspection(entity));
            if (sapEquipment.SAPNotificationNumber != null)
                entity.SAPNotificationNumber = sapEquipment.SAPNotificationNumber;
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
            entity.BusinessUnit = sapEquipment.CostCenter;
        }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchHydrantInspection>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchHydrantInspection model)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchInspections(model)
                }));
                formatter.Map(() => {
                    // These are all hardcoded because the map icons must be sorted in this order
                    // for the lines to draw correctly.
                    model.EnablePaging = false;

                    // Bug 2432: Both the hydrant and blowoff inspections need to be displayed at the same time even 
                    // though they do not both show up in the regular table of results.
                    var blowOffSerach = new SearchBlowOffInspection {
                        EnablePaging = model.EnablePaging,
                        CreatedAt = model.CreatedAt,
                        DateInspected = model.DateInspected,
                        InspectedBy = model.InspectedBy,
                        // IsLargeValue
                        OperatingCenter = model.OperatingCenter,
                        Town = model.Town,
                        // ValveSuffix
                        WorkOrderRequestOne = model.WorkOrderRequestOne,
                        WorkOrderRequired = model.WorkOrderRequired
                    };

                    var hydCoords = Repository.SearchHydrantInspectionsForMap(model);
                    var blowOffCoords =
                        _container.GetInstance<IBlowOffInspectionRepository>()
                            .SearchBlowOffInspectionsForMap(blowOffSerach);

                    var amr = _container.GetInstance<AssetMapResult>();
                    amr.InitializeWithInspectionLineLayer(hydCoords, blowOffCoords);
                    return amr;
                });
                formatter.Excel(() => ActionHelper.DoExcel(model, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchInspections(model)
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: DisplaySapErrorIfApplicable);
        }

        [SkipRoleOperatingCenterCheck] // Role Authorizer does not know the id is for a hydrant and not a hydrant inspection
        [HttpGet, ActionBarVisible(false), RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int id)
        {
            var hydrant = _container.GetInstance<IHydrantRepository>().Find(id);
            if (hydrant == null)
            {
                return DoHttpNotFound("Hydrant not found.");
            }

            var model = new CreateHydrantInspection(_container);
            model.Hydrant = hydrant.Id;
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateHydrantInspection model)
        {
            // Is there even a need for the onSuccess call to pass in controller and model?
            // We already have references to them.
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    UpdateHydrantTagStatus(model);
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

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditHydrantInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditHydrantInspection model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                    UpdateHydrantTagStatus(model);
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

        private void UpdateHydrantTagStatus(HydrantInspectionViewModel model)
        {
            var repo = _container.GetInstance<IRepository<Hydrant>>();
            var hydrant = repo.Find(model.Hydrant.Value);
            //if we don't have a previous inspection or if this is the latest inspection and the tag status is changing
            if (hydrant.HydrantTagStatus?.Id != model.HydrantTagStatus && (hydrant.LastInspection == null || hydrant.LastInspection?.Id == model.Id))
            {
                hydrant.HydrantTagStatus = new HydrantTagStatus { Id = model.HydrantTagStatus.Value};
                repo.Save(hydrant);
            }
        }

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}