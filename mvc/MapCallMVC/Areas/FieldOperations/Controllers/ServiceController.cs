using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations._2016;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Controllers;
using MMSINC.Helpers;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ServiceController : SapSyncronizedControllerBaseWithPersisence<IServiceRepository, Service, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        public const string
            UPDATE_LARGE_SERVICE_OR_FIRE_NOTIFICATION_PURPOSE = "Service Large Or Fire",
            UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE = "Service Renewal Record Closed",
            UPDATE_NEW_SERVICE_INSTALLED = "New Service Installed",
            RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE =
                DoThingsWithSampleSitesForBug3013.Notifications.RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE,
            DEACTIVATED_SERVICE_WITH_SAMPLE_SITE =
                DoThingsWithSampleSitesForBug3013.Notifications.DEACTIVATED_SERVICE_WITH_SAMPLE_SITE,
            SAMPLE_SITE_WARNING =
                "This Service Record is Linked to a Sample Site. Contact WQ before making any changes.",
            PREMISE_CONTACT_MUST_BE_ADDED =
                "A premise contact must be added because this service's Customer Side SL Replacement " +
                "field was updated.",
            SERVICE_FLUSH_MUST_BE_ADDED =
                "A service flushing record must be added because this service's Previous Material was " +
                "updated to Lead.",
            SERVICE_WITH_SAMPLE_SITE = "Service With Sample Site";

        #endregion

        #region Private Methods

        private static string GetTerribleServiceType(ServiceCategory cat)
        {
            /* This is based off what the old Services.asmx's query did. It's terrible and
             * I'm not even sure why we need this data in the TapImages table. Here's the
             * terrible reminder:
             *
             *  CASE
	         *  WHEN(charindex('retire',sc.Description)>0) THEN 'RETIREMENT'
	         *  WHEN(charindex('sewer',sc.Description)>0) THEN 'SEWER'
	         *  WHEN(CharIndex('irrigation',sc.Description)>0) THEN 'IRRIGATION'
	         *  WHEN(charIndex('fire',sc.Description)>0) THEN 'FIRE SERVICE'
	         *  ELSE 'WATER'
             *  END AS CatOfService,
             */

            var desc = cat != null ? cat.Description : string.Empty;
            if (desc.Contains("retire", StringComparison.InvariantCultureIgnoreCase))
            {
                return "RETIREMENT";
            }
            else if (desc.Contains("sewer", StringComparison.InvariantCultureIgnoreCase))
            {
                return "SEWER";
            }
            else if (desc.Contains("irrigation", StringComparison.InvariantCultureIgnoreCase))
            {
                return "IRRIGATION";
            }
            else if (desc.Contains("fire", StringComparison.InvariantCultureIgnoreCase))
            {
                return "FIRE SERVICE";
            }

            return "WATER";
        }

        // Notifications
        // SendRenewalInstalledNotification
        // sendLargeServiceOrFireNotification
        // ON CREATE
        //if DateInstalled<> "" and(CatOfService = "13" or CatOfService = "23") then
        //  SendRenewalInstalledNotification(RecID)
        //end if

        private void SendServiceNotification(Service service, string updateNotificationPurpose)
        {
            var templateModel = new ServiceNotification {
                Service = service
            };

            templateModel.Service.RecordUrl = GetUrlForModel(templateModel.Service, "Show", "Service", "FieldOperations");

            if (templateModel.Service.Premise?.SampleSite != null)
            {
                templateModel.Service.Premise.SampleSite.RecordUrl = GetUrlForModel(templateModel.Service.Premise.SampleSite, "Show", "SampleSite", "WaterQuality");

                if (templateModel.Service.RenewalOf?.Premise?.SampleSite != null)
                {
                    templateModel.Service.RenewalOf.Premise.SampleSite.RecordUrl = GetUrlForModel(templateModel.Service.RenewalOf.Premise.SampleSite, "Show", "SampleSite", "WaterQuality");
                }
            }

            templateModel.Service.RecordUrlMap = GetMapUrlForModel(templateModel.Service, "Show", "Service", "FieldOperations");
            this.SendNotification(service.OperatingCenter.Id, ROLE, updateNotificationPurpose, templateModel);
        }

        protected override void UpdateEntityForSap(Service entity)
        {
            var repo = _container.GetInstance<ISAPNewServiceInstallationRepository>();
            var sapEntity = repo.SaveService(new SAPNewServiceInstallation(entity));
            entity.SAPErrorCode = sapEntity.SAPStatus;
        }
        
        /// <summary>
        /// Get the requested work order or return an actionResult for the specific issue with accessing it.
        /// </summary>
        private bool TryGetWorkOrder(
            int workOrderId,
            out ActionResult actionResult,
            out WorkOrder workOrder)
        {
            workOrder = _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
            if (workOrder == null)
            {
                actionResult = DoHttpNotFound($"Work Order #{workOrderId} not found.");
                return false;
            }

            var roleService = _container.GetInstance<IRoleService>();

            var canAccess = roleService.CanAccessRole(ROLE, RoleActions.Add, workOrder.OperatingCenter);
            if (!canAccess)
            {
                var forbiddenModel = new ForbiddenRoleAccessModel {
                    RequiredRoles = { new RequiresRoleAttribute(ROLE, RoleActions.Add) }
                };
                actionResult = View("~/Views/Shared/ForbiddenRoleAccess.cshtml", forbiddenModel);
                return false;
            }
            actionResult = null;
            return true;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.New:
                    this.AddDropDownData<BackflowDevice>();
                    this.AddDropDownData<MainType>();
                    this.AddDropDownData<PermitType>();
                    this.AddDropDownData<PremiseType>();
                    this.AddDropDownData<ServiceCategory>();
                    this.AddDropDownData<ServiceInstallationPurpose>();
                    // this.AddDropDownData<ServiceMaterial>();
                    // this.AddDropDownData<ServiceMaterial>("PreviousServiceMaterial");
                    this.AddDropDownData<ServicePriority>();
                    this.AddDropDownData<ServiceSize>(
                        x => x.GetAllSorted(y => y.SortOrder)
                              .Where(z => z.Service),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>(
                        "MainSize",
                        x => x.GetAllSorted(y => y.SortOrder)
                              .Where(z => z.Main),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>(
                        "MeterSettingSize",
                        x => x.GetAllSorted(y => y.SortOrder)
                              .Where(z => z.Meter),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>(
                        "PreviousServiceSize",
                        x => x.GetAllSorted(y => y.SortOrder),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceStatus>();
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddDropDownData<StreetMaterial>();
                    break;

                case ControllerAction.Edit:
                    this.AddDropDownData<BackflowDevice>();
                    this.AddDropDownData<MainType>();
                    this.AddDropDownData<PermitType>();
                    this.AddDropDownData<PremiseType>();
                    this.AddDropDownData<ServiceCategory>();
                    this.AddDropDownData<ServiceInstallationPurpose>();
                    // this.AddDropDownData<ServiceMaterial>();
                    // this.AddDropDownData<ServiceMaterial>("PreviousServiceMaterial");
                    this.AddDropDownData<ServicePriority>();
                    this.AddDropDownData<ServiceSize>(
                        x => x.GetAllSorted(y => y.SortOrder)
                              .Where(z => z.Service),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>(
                        "MainSize",
                        x => x.GetAllSorted(y => y.SortOrder)
                              .Where(z => z.Main),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>(
                        "MeterSettingSize",
                        x => x.GetAllSorted(y => y.SortOrder)
                              .Where(z => z.Meter),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceSize>(
                        "PreviousServiceSize",
                        x => x.GetAllSorted(y => y.SortOrder),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceStatus>();
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddDropDownData<StreetMaterial>();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    break;

                case ControllerAction.Search:
                    this.AddDropDownData<ServiceSize>(
                        "ServiceSize",
                        x => x.GetAllSorted(y => y.SortOrder),
                        x => x.Id,
                        x => x.ServiceSizeDescription);
                    this.AddDropDownData<ServiceCategory>();
                    this.AddDropDownData<ServiceInstallationPurpose>();
                    this.AddDropDownData<ServicePriority>();
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;

                case ControllerAction.Show:
                    this.AddDropDownData<ServicePremiseContactMethod>("ViewModel.ContactMethod");
                    this.AddDropDownData<ServicePremiseContactType>("ViewModel.ContactType");
                    this.AddDropDownData<ServiceFlushFlushType>("ViewModel.FlushType");
                    this.AddDropDownData<ServiceFlushSampleType>("ViewModel.SampleType");
                    this.AddDropDownData<ServiceFlushSampleStatus>("ViewModel.SampleStatus");
                    this.AddDropDownData<ServiceFlushSampleTakenByType>("ViewModel.TakenBy");
                    this.AddDropDownData<ServiceFlushPremiseContactMethod>("ViewModel.FlushContactMethod");
                    this.AddDropDownData<ServiceFlushReplacementType>("ViewModel.ReplacementType");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Search(SearchService search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Show(int id, bool nopdf = false)
        {
            return this.RespondTo(x => {
                x.View(() => {
                    Action<Service> onModelFound = (entity) => {
                        if (entity.StatusMessage != string.Empty)
                        {
                            DisplayNotification(entity.StatusMessage);
                        }
                        if (entity.SAPErrorCodeType == ServiceSAPErrorCodeType.InvalidDeviceLocation)
                        {
                            DisplayErrorMessage("SAP Error: " + entity.SAPErrorCode);
                        }
                        if ((entity.Premise != null && 
                             entity.Premise.SampleSites.Any()) || 
                            Repository.AnyWithInstallationNumberAndOperatingCenterAndSampleSites(entity.Installation, entity.OperatingCenter.Id))
                        {
                            DisplayErrorMessage(SAMPLE_SITE_WARNING);
                        }

                        this.AddDropDownData<IWorkOrderRepository, WorkOrder>(
                            "WorkOrder",
                            r => r.GetByTownIdForServices(entity.Town.Id), 
                            wo => wo.Id, 
                            wo => wo.Id);
                    };

                    return ActionHelper.DoShow(id, onModelFound: onModelFound);
                });
                x.Json(() => {
                    var model = Repository.Find(id);

                    if (model == null)
                        return HttpNotFound();
                    return new JsonResult {
                        Data = new {
                            DateInstalled = $"{model.DateInstalled:d}",
                            ServiceMaterial = model.ServiceMaterial?.Id,
                            ServiceSize = model.ServiceSize?.Id,
                            ServiceMaterialDescription = model.ServiceMaterial?.Description,
                            CustomerSideMaterialDescription = model.CustomerSideMaterial?.Description
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                });
                x.Map(() => {
                    var model = Repository.Find(id);
                    return model == null ? 
                        (ActionResult)HttpNotFound() : 
                        _container.With((IEnumerable<IThingWithCoordinate>)new[] { model })
                                  .GetInstance<MapResultWithCoordinates>();
                });
                x.Pdf(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    var viewPath = this.GetStateViewPath(model, "Pdf");
                    return nopdf ? 
                        View(viewPath, model) : 
                        new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), viewPath, model);
                });
            });
        }

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Index(SearchService search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id, bool? copy, bool? withServiceNumber, bool? forSewer)
        {
            // TODO: This action is doing the work of what should be four different actions.
            // I recommend splitting this up into "New", "NewRenewal", "CopyWithServiceNumber", and
            // "WhateverForSewerMeans". These actions should all have required, validated parameters.
            // Similar to how NewFromWorkOrder works.

            // NOTE: CreateService.SetDefaults does the view model mapping when the service is being
            // renewed.
            var model = new CreateService(_container) { RenewalOf = id };
            if (id.HasValue && copy == true)
            {
                var entity = Repository.Find(id.Value);
                model = ViewModelFactory.BuildWithOverrides<CreateService, Service>(entity, new {
                    Copy = true,
                    WithServiceNumber = withServiceNumber,
                    ForSewer = forSewer,
                    RenewalOf = id
                });
                //mc-794 never copy with these numbers
                model.SAPNotificationNumber = null;
                model.SAPWorkOrderNumber = null;
            }

            return ActionHelper.DoNew(model);
        }

        /// <summary>
        /// We can't use the parameter name id here because the authentication checks against the service
        /// with this id instead of the work order which can be in different operating centers
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [HttpGet, RequiresRole(ROLE, RoleActions.Add), Crumb(Action = "New")]
        public ActionResult NewFromWorkOrder(int workOrderId)
        {
            return TryGetWorkOrder(workOrderId, out var actionResult, out var workOrder)
                ? ActionHelper.DoNew(new CreateService(_container, workOrder))
                : actionResult;
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateService model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendRenewalInstalledNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id),
                            UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE);
                    }

                    if (model.SendRenewalAtServiceWithSampleSiteNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id),
                            RENEWAL_AT_SERVICE_WITH_SAMPLE_SITE);
                    }

                    if (model.SendServiceInstallationNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id), UPDATE_NEW_SERVICE_INSTALLED);
                    }

                    if (model.SendServiceWithSampleSitesNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id), SERVICE_WITH_SAMPLE_SITE);
                    }
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    if (model.CustomerSideSLReplacementWasUpdated)
                    {
                        DisplayNotification(PREMISE_CONTACT_MUST_BE_ADDED);
                    }

                    if (model.PreviousServiceMaterialWasUpdatedToLead)
                    {
                        DisplayNotification(SERVICE_FLUSH_MUST_BE_ADDED);
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        /// <summary>
        /// This method loads a view that will allow the user to link a work order to an existing service
        /// or provide a link to create a new service from the work order.
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult LinkOrNew(int workOrderId)
        {
            // check if we can access the work order and return either the work order or an error action
            // result
            if (!TryGetWorkOrder(workOrderId, out var actionResult, out var workOrder))
            {
                return actionResult;
            }

            var model = new LinkOrNewService();
            model.WorkOrder = workOrder;

            // Requested in bug 3524
            model.RelatedServices = Repository.FindManyByWorkOrder(workOrder).Select(s => new Service {
                Id = s.Id,
                ServiceNumber = s.ServiceNumber, 
                StreetNumber = s.StreetNumber,
                Street = s.Street,
                OperatingCenter = s.OperatingCenter,
                ServiceType = s.ServiceType
            });
            
            return View(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditService>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditService model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendLargeServiceOrFireNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id),
                            UPDATE_LARGE_SERVICE_OR_FIRE_NOTIFICATION_PURPOSE);
                    }
                    if (model.SendRenewalInstalledNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id),
                            UPDATE_RENEWAL_INSTALLED_NOTIFICATION_PURPOSE);
                    }
                    if (model.SendServiceInstallationNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id), UPDATE_NEW_SERVICE_INSTALLED);
                    }
                    if (model.SendDeactivatedServiceWithSampleSiteOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id),
                            DEACTIVATED_SERVICE_WITH_SAMPLE_SITE);
                    }
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    if (model.CustomerSideSLReplacementWasUpdated)
                    {
                        DisplayNotification(PREMISE_CONTACT_MUST_BE_ADDED);
                    }

                    if (model.PreviousServiceMaterialWasUpdatedToLead)
                    {
                        DisplayNotification(SERVICE_FLUSH_MUST_BE_ADDED);
                    }

                    if (model.SendServiceWithSampleSitesNotificationOnSave)
                    {
                        SendServiceNotification(Repository.Find(model.Id), SERVICE_WITH_SAMPLE_SITE);
                    }
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddWorkOrder(AddServiceWorkOrder model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveWorkOrder(RemoveServiceWorkOrder model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnError = () => RedirectToAction("Show", new { id = model.Id })
            });
        }

        #endregion

        #region PremiseContact

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddServicePremiseContact(CreateServicePremiseContactViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult RemoveServicePremiseContact(RemoveServicePremiseContactViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ServiceFlush

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddServiceFlush(CreateServiceFlushViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult RemoveServiceFlush(RemoveServiceFlushViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByPremiseNumberAndServiceNumber

        [HttpGet]
        public ActionResult ByPremiseNumberAndServiceNumber(SearchServicePremiseNumberServiceNumber model)
        {
            var result = new Dictionary<string, object>();
            result["success"] = false;

            if (!ModelState.IsValid)
            {
                result["message"] = "Invalid search parameters.";
            }
            else
            {
                var s = Repository.FindByPremiseNumber(model.PremiseNumberSearch);
                if (model.ServiceNumberSearch != null)
                {
                    s = Repository.FindByPremiseNumberAndServiceNumber(model.ServiceNumberSearch,
                        model.PremiseNumberSearch);
                }

                if (s == null)
                {
                    result["message"] = "There are no services that match the search parameters.";
                }
                else
                {
                    result["success"] = true;
                    result["townSection"] = (s.TownSection != null)
                        ? s.TownSection.Description
                        : string.Empty;
                    result["serviceId"] = s.Id;
                    result["serviceNumber"] = s.ServiceNumber;
                    result["premiseNumber"] = s.PremiseNumber;
                    result["lot"] = s.Lot;
                    result["apartmentNumber"] = s.ApartmentNumber;
                    result["block"] = s.Block;
                    result["operatingCenterId"] = s.OperatingCenter.Id; // op center is not nullable
                    result["lengthOfService"] = s.LengthOfService;
                    result["serviceType"] = GetTerribleServiceType(s.ServiceCategory);
                    result["streetNumber"] = s.StreetNumber;
                    result["isDefaultImageForService"] =
                        (!s.TapImages.Any(x => x.IsDefaultImageForService)).ToString();

                    if (s.Street != null)
                    {
                        result["streetId"] = s.Street.Id;
                    }

                    if (s.CrossStreet != null)
                    {
                        result["crossStreetId"] = s.CrossStreet.Id;
                    }

                    if (s.Town != null)
                    {
                        result["townId"] = s.Town.Id;
                    }

                    if (s.ServiceMaterial != null)
                    {
                        result["serviceMaterial"] = s.ServiceMaterial.Id;
                    }

                    if (s.ServiceSize != null)
                    {
                        result["serviceSize"] = s.ServiceSize.Id;
                    }

                    if (s.PreviousServiceMaterial != null)
                    {
                        result["previousServiceMaterial"] = s.PreviousServiceMaterial.Id;
                    }

                    if (s.PreviousServiceSize != null)
                    {
                        result["previousServiceSize"] = s.PreviousServiceSize.Id;
                    }

                    if (s.CustomerSideMaterial != null)
                    {
                        result["customerSideMaterial"] = s.CustomerSideMaterial.Id;
                    }

                    if (s.CustomerSideSize != null)
                    {
                        result["customerSideSize"] = s.CustomerSideSize.Id;
                    }

                    if (s.DateInstalled.HasValue)
                    {
                        result["dateInstalled"] =
                            string.Format(CommonStringFormats.DATE, s.DateInstalled.Value);
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpGet]
        public ActionResult AnyWithInstallationNumberAndOperatingCenter(
            string installation,
            int operatingCenterId)
        {
            var results = Repository.FindByInstallationNumberAndOperatingCenterAndSampleSites(
                installation,
                operatingCenterId);
            return Json(new { Data = results.Select(s => new { s.Id })}, JsonRequestBehavior.AllowGet);
        }

        #region ByStreetId

        [HttpGet]
        public ActionResult ByStreetId(int id)
        {
            return new CascadingActionResult(Repository.FindByStreetId(id), "Description", "Id");
        }

        #endregion

        public ServiceController(
            ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args)
            : base(args) { }
    }
}
