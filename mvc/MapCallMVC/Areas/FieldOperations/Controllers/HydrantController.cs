using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities;
using MapCall.SAP.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    // This is used for cascades, so no global role.
    public class HydrantController : SapSyncronizedControllerBaseWithPersisence<IHydrantRepository, Hydrant, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        public const string NOTIFICATION_PURPOSE = "Hydrant",
                            HYRDRANT_OUT_OF_SERVICE_NOTIFICATION_PURPOSE = "Hydrant Out Of Service",
                            SAP_NOTIFICATION_EMAIL_ADDRESS = "mapcall@amwater.com";

        #endregion

        #region Private Methods
        
        private void GenerateArcCollectorLink(Hydrant hydrant)
        {
            var hydrantAssetType = _container
                                  .GetInstance<IRepository<AssetType>>()
                                  .Find(AssetType.Indices.HYDRANT);
            ViewData["ArcCollectorLink"] = ArcCollectorLinkGenerator
               .ArcCollectorHydrantHtmlString(hydrant, hydrantAssetType);
        }

        private MapResult GetMapResult(SearchHydrantForMap search)
        {
            var result = _container.GetInstance<AssetMapResult>();

            if (!ModelState.IsValid)
            {
                return result;
            }

            // Bug 2558: If loading related assets from AssetMap, do not include the RequiresInspection
            //           search.
            if (search.IsRelatedAssetSearch)
            {
                search.RequiresInspection = null;
            }

            if (Repository.GetCountForSearchSet(search) > SearchHydrantForMap.MAX_MAP_RESULT_COUNT)
            {
                return null;
            }

            var hydResult = Repository.SearchForMap(search);

            if (search.EntityId.HasValue)
            {
                result.Initialize(hydResult);
            }
            else
            {
                var valveSearchRvd = new RouteValueDictionary {
                    [ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                        ResponseFormatterExtensions.MAP_ROUTE_EXTENSION
                };

                foreach (var ms in ModelState)
                {
                    valveSearchRvd[ms.Key] = ms.Value.Value.AttemptedValue;
                }
                valveSearchRvd["IsRelatedAssetSearch"] = true;
                result.RelatedAssetsUrl = Url.Action("Index", "Valve", valveSearchRvd);
                result.Initialize(hydResult);

                // bug 2817: If a specific Route is searched for, display the line layer. 
                if (search.Route.HasValue)
                {
                    var hydCoordsSorted = hydResult.OrderBy(x => x.Stop);
                    result.InitializeWithLineLayer(hydCoordsSorted);
                }
            }

            return result;
        }

        protected override void UpdateEntityForSap(Hydrant entity)
        {
            var equipment = new SAPEquipment(entity);
            var sapEquipment = _container.GetInstance<ISAPEquipmentRepository>().Save(equipment);

            if (!string.IsNullOrWhiteSpace(sapEquipment.SAPEquipmentNumber))
                entity.SAPEquipmentId = int.Parse(sapEquipment.SAPEquipmentNumber);
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }

        private void SendNotification(Hydrant hydrant)
        {
            var templateModel = new HydrantNotification {
                Hydrant = hydrant,
                UserName = AuthenticationService.CurrentUser.UserName,
                UserEmail = AuthenticationService.CurrentUser.Email,
                RecordUrl = GetUrlForModel(hydrant, "Show", "Hydrant", "FieldOperations")
            };
            templateModel.ActiveHydrantsOnPremise = Repository
               .GetTotalNumberOfActiveHydrantsForPremise(templateModel.Hydrant.PremiseNumber);

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = hydrant.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = templateModel
            };
            notifier.Notify(args);
        }

        private void SendOutOfServiceNotification(HydrantOutOfService model)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.Hydrant.OperatingCenter.Id,
                Module = ROLE,
                Purpose = HYRDRANT_OUT_OF_SERVICE_NOTIFICATION_PURPOSE,
                Data = model,
                Subject = $"{model.Hydrant.Town.State} American Water - Hydrant Status Notification"
            };

            // send emails out to any town contacts signed up
            foreach (var contactEmail in model
                                        .Hydrant.Town.TownContacts
                                        .Where(x => x.ContactType.Id ==
                                                    ContactType.Indices.HYDRANT_OUT_OF_SERVICE)
                                        .Select(x => x.Contact.Email))
            {
                if (string.IsNullOrWhiteSpace(contactEmail))
                {
                    continue;
                }

                args.Address = contactEmail;
                notifier.Notify(args);
            }
            args.Address = null;

            // send out emails to normal users signed up
            notifier.Notify(args);
        }

        private void SetHydrantStatusLookupDataForEditAction(int entityId)
        {
            // Bug 2432: Non-admin users are only allowed to change the hydrant status
            //           to a few select values.

            var entity = Repository.Find(entityId);
            if (entity == null)
            {
                return; // Can't do anything and Edit will return a 404 anyway.
            }

            var statuses = _container.GetInstance<IRepository<AssetStatus>>().GetAll();
            var isUserAdmin = _container
                             .GetInstance<IRoleService>()
                             .CanAccessRole(ROLE, RoleActions.UserAdministrator, entity.OperatingCenter);

            // they want the new status to be visible for all users on Show, but not an option for any user on New or Edit.
            statuses = statuses.Where(x => x.Id != AssetStatus.Indices.NSI_PENDING || x.Id == entity.Status.Id);

            if (!isUserAdmin)
            {
                // We also need to include the currently selected value, even if the selected value
                // is admin-only and the user is not an admin. This is needed due to validation. Also
                // it's still allowed since the user isn't technically changing the hydrant status if
                // they keep the value the same.
                statuses = statuses.Where(x => !x.IsUserAdminOnly || x.Id == entity.Status.Id);
            }

            this.AddDropDownData<AssetStatus>(
                key: "Status",
                dataGetter: _ => statuses,
                keyGetter: x => x.Id,
                valueGetter: x => x.Description);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action addGenericLookups = () => {
                this.AddDropDownData<HydrantDirection>("OpenDirection");
                this.AddDropDownData<HydrantMainSize>(x => x.GetAllSorted(y => y.SortOrder));
                this.AddDropDownData<HydrantManufacturer>();
                this.AddDropDownData<HydrantSize>(x => x.GetAllSorted(y => y.SortOrder));
                this.AddDropDownData<HydrantTagStatus>();
                this.AddDropDownData<HydrantThreadType>();
                this.AddDropDownData<LateralSize>(x => x.GetAllSorted(y => y.SortOrder));
                this.AddDropDownData<MainType>();
                this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
            };

            switch (action)
            {
                case ControllerAction.New:
                    addGenericLookups();
                    this.AddDropDownData<HydrantBilling>();
                    this.AddDropDownData<AssetStatus>();
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
                case ControllerAction.Edit:
                    if (_container
                       .GetInstance<IRoleService>()
                       .CanAccessRole(ROLE, RoleActions.UserAdministrator))
                    {
                        this.AddDropDownData<HydrantBilling>();
                    }
                    // Don't add HydrantStatus here as it needs to be role checked.
                    addGenericLookups();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddDropDownData<AssetStatus>("Status");
                    this.AddDropDownData<HydrantManufacturer>();
                    this.AddDropDownData<HydrantBilling>();
                    this.AddDropDownData<WorkDescription>(
                        "OpenWorkOrderWorkDescription",
                        x => x.GetAllSorted().Where(y => y.AssetType.Id == AssetType.Indices.HYDRANT),
                        x => x.Id,
                        x => x.Description);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchHydrant search)
        {
            // NOTE: There are report links that forward to this search action with values set.
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id, bool nopdf = false)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id, null, onModelFound: hyd => {
                    GenerateArcCollectorLink(hyd);
                    // Bug 2512: Display out of service alert in such a way that the users can see it when
                    //           changing the out of service/back in service status.
                    if (hyd.OutOfService)
                    {
                        DisplayNotification("This hydrant is currently out of service.");
                    }
                    DisplaySapErrorIfApplicable(hyd);
                }));
                formatter.Pdf(() => {
                    var model = Repository.Find(id);

                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    var viewPath = this.GetStateViewPath(model, "Pdf");

                    return nopdf
                        ? View(viewPath, model)
                        : new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), viewPath, model);
                });
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
                formatter.Map(() => {
                    // NOTE: This does not display other valves/blowoffs since the whole point of
                    // mapping this is to display a single coordinate.
                    var search = new SearchHydrantForMap { EntityId = id };
                    return GetMapResult(search);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchHydrant search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => {
                    return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                        // If it's not obvious, this is a specific overload of the Search method that
                        // takes an ISearchHydrant and does special things to the query.
                        SearchOverrideCallback = () => Repository.Search(search)
                    });
                });
                formatter.Map(() => GetMapResult(search));
                formatter.Excel(() => {
                    // NOTE: The excel export is unit tested! If you add fields, make sure to update the
                    //       test.
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return this.Excel(results.Select(x => new {
                        x.Id,
                        x.HydrantNumber,
                        x.HydrantSuffix,
                        x.LegacyId,
                        x.GISUID,
                        x.OperatingCenter,
                        x.WaterSystem,
                        x.Town,
                        x.TownSection,
                        x.FireDistrict,
                        x.StreetNumber,
                        x.Street,
                        x.CrossStreet,
                        x.Location,
                        Latitude = (x.Coordinate != null ? (object)x.Coordinate.Latitude : null),
                        Longitude = (x.Coordinate != null ? (object)x.Coordinate.Longitude : null),
                        x.SAPEquipmentId,
                        x.SAPErrorCode,
                        x.Route,
                        x.Stop,
                        x.HydrantManufacturer,
                        x.YearManufactured,
                        x.OpenDirection,
                        x.WorkOrderNumber,
                        x.HydrantDueInspection?.RequiresInspection,
                        x.HydrantDuePainting?.RequiresPainting,
                        x.HydrantDuePainting?.LastPaintedAt,
                        LastInspection = x.ActualLastInspection,
                        x.HasOpenWorkOrder,
                        x.DateInstalled,
                        HydrantStatus = x.Status,
                        x.HydrantBilling,
                        x.HydrantSize,
                        x.HydrantMainSize,
                        x.LateralSize,
                        x.FunctionalLocation,
                        LastUpdated = x.UpdatedAt,
                        x.OutOfService,
                        x.PremiseNumber,
                        DateAdded = x.CreatedAt,
                        x.Town.State,
                        x.HasWorkOrder,
                        HasCriticalNotes = x.Critical,
                        x.CriticalNotes,
                        x.Initiator,
                        x.Facility,
                        x.MapPage,
                        x.Gradient,
                        x.Elevation,
                        x.IsDeadEndMain,
                        x.HydrantModel,
                        x.MainType,
                        x.LateralValve,
                        x.BranchLengthFeet,
                        x.BranchLengthInches,
                        x.HydrantThreadType,
                        x.HydrantOutletConfiguration,
                        x.HydrantType,
                        x.DepthBuryFeet,
                        x.DepthBuryInches,
                        x.IsNonBPUKPI,
                        x.BillingDate,
                        x.InspectionFrequency,
                        x.HydrantTagStatus
                    }));
                });
            });
        }

        #endregion

        #region New/Create

        // Bug 2432: Only UserAdmin roled users are allowed to create new hydrants.
        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New(CreateHydrant model)
        {
            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        // Bug 2432: Only UserAdmin roled users are allowed to create new hydrants.
        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateHydrant model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                        UpdateSAP(model.Id, ROLE);
                    if (model.SendNotificationOnSave)
                        SendNotification(Repository.Find(model.Id));
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update/Replace/Copy

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            SetHydrantStatusLookupDataForEditAction(id);

            var hydrant = Repository.Find(id);
            if (hydrant != null)
            {
                var statusesThatRequireNotification = new[] {
                    AssetStatus.Indices.CANCELLED, 
                    AssetStatus.Indices.REMOVED,
                    AssetStatus.Indices.RETIRED
                };
                if (statusesThatRequireNotification.Contains(hydrant.Status.Id))
                {
                    DisplayNotification(
                        "This asset is currently cancelled, removed, or retired. Any updates made to " +
                        "this record will not be saved to SAP.");
                }
            }

            return ActionHelper.DoEdit<EditHydrant>(id);
        }

        private void OnEditHydrantUpdateSuccess(EditHydrant model)
        {
            if (model.SendToSAP)
            {
                UpdateSAP(model.Id, ROLE);
            }

            if (model.SendNotificationOnSave)
            {
                SendNotification(Repository.Find(model.Id));
            }
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditHydrant model)
        {
            // onSuccess func needs to shoot the email out  
            // The view model will need to know if the email should be sent out or not.
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    OnEditHydrantUpdateSuccess(model);
                    return RedirectToAction("Show", new { id = model.Id });
                },
                OnError = () => {
                    SetHydrantStatusLookupDataForEditAction(model.Id);
                    return null;
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Replace(int id)
        {
            var hydrant = Repository.Find(id);
            if (hydrant == null || !hydrant.IsActive)
            {
                return DoHttpNotFound($"Could not find hydrant with id {id}.");
            }

            // NOTE: If the edit saves fine, but the replace throws an exception, you end up with an
            //       irreplacable hydrant.

            // pending retire old hydrant
            var editHydrant = ViewModelFactory.BuildWithOverrides<EditHydrant, Hydrant>(hydrant,
                new {
                    Status = AssetStatus.Indices.REQUEST_RETIREMENT
                }); 
            
            return ActionHelper.DoUpdate(editHydrant, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    OnEditHydrantUpdateSuccess(editHydrant);
                    // NOTE: We're doing the replacement part inside the DoUpdate success call so that we
                    //       know we're only creating a replacement when the update of the existing hydrant
                    //       was successful.

                    // create the new hydrant record, also copies the inspections on the MapCall end. It was
                    // specifically stated that inspections are not copied to SAP
                    // http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c9
                    var replaceHydrant = GetReplaceHydrantModel();
                    replaceHydrant.Map(hydrant);

                    return ActionHelper.DoCreate(replaceHydrant, new ActionHelperDoCreateArgs {
                        OnSuccess = () => {
                            if (replaceHydrant.SendToSAP)
                            {
                                UpdateSAP(replaceHydrant.Id, ROLE);
                            }

                            if (replaceHydrant.SendNotificationOnSave)
                            {
                                SendNotification(Repository.Find(replaceHydrant.Id));
                            }

                            return RedirectToAction("Show", new { id = replaceHydrant.Id });
                        }
                    });
                },
                OnError = () => {
                    SetHydrantStatusLookupDataForEditAction(editHydrant.Id);
                    return null;
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Copy(int id)
        {
            var hydrant = Repository.Find(id);
            if (hydrant == null || !hydrant.CanBeCopied)
            {
                return DoHttpNotFound($"Could not find hydrant with id {id}.");
            }

            var viewModel = ViewModelFactory.Build<CopyHydrant, Hydrant>(hydrant);
            return ActionHelper.DoCreate(viewModel, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (viewModel.SendNotificationOnSave)
                    {
                        SendNotification(Repository.Find(viewModel.Id));
                    }

                    return RedirectToAction("Edit", new { id = viewModel.Id });
                }
            });
        }

        // This is for testing purposes only.
        protected virtual ReplaceHydrant GetReplaceHydrantModel()
        {
            return ViewModelFactory.Build<ReplaceHydrant>();
        }

        #endregion

        #region GetHydrantPrefix

        [HttpGet, AlwaysCache, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult GetHydrantPrefix(int? operatingCenterId, int? townId, int? townSectionId)
        {
            var result = new Dictionary<string, string>();
            var operatingCenter = _container
                                 .GetInstance<IOperatingCenterRepository>()
                                 .Find(operatingCenterId.GetValueOrDefault());
            var town = _container.GetInstance<ITownRepository>().Find(townId.GetValueOrDefault());
            if (town == null)
            {
                result["prefix"] = string.Empty;
            }
            else
            {
                var ts = _container
                        .GetInstance<ITownSectionRepository>()
                        .Find(townSectionId.GetValueOrDefault());
                result["prefix"] = Repository.GenerateHydrantPrefix(operatingCenter, town, ts, null);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// NOTE: This does not get called during server validation, which is dumb. 
        /// </summary>
        [HttpGet, NoCache, RequiresRole(ROLE, RoleActions.Read)]
        // Remote validation must be a GET for some reason.
        public ActionResult ValidateUnusedFoundHydrantSuffix(int? hydrantSuffix, int? operatingCenter, int? town, int? townSection)
        {
            if (!hydrantSuffix.HasValue || !town.HasValue || !operatingCenter.HasValue)
            {
                // These are all required fields.
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var model = _viewModelFactory.BuildWithOverrides<CreateHydrant>(new {
                HydrantSuffix = hydrantSuffix,
                OperatingCenter = operatingCenter,
                Town = town,
                TownSection = townSection,
                IsFoundHydrant = true
            });

            var result = model.ValidateHydrantSuffixForFoundHydrants().ToArray();

            if (!result.Any())
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(result.First().ErrorMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult SetHydrantBackInService(MarkBackInServiceHydrant model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var hydrant = Repository.Find(model.Id);
                    var notificationModel = new HydrantOutOfService {
                        Hydrant = hydrant,
                        BackInServiceDate = model.BackInServiceDate.Value,
                        OutOfServiceDate = hydrant.OutOfServiceRecords
                                                  .OrderBy(x => x.OutOfServiceDate)
                                                  .Last()
                                                  .OutOfServiceDate
                    };
                    SendOutOfServiceNotification(notificationModel);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult SetHydrantOutOfService(MarkOutOfServiceHydrant model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var notificationModel = new HydrantOutOfService {
                        Hydrant = Repository.Find(model.Id),
                        OutOfServiceDate = model.OutOfServiceDate.Value
                    };
                    SendOutOfServiceNotification(notificationModel);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Public Methods

        #region RouteByTownId

        [HttpGet]
        public ActionResult RouteByTownId(int townId)
        {
            return new CascadingActionResult(
                Repository.RouteByTownId(townId).Select(x => new { value = x, text = x }),
                "value",
                "text");
        }

        #endregion

        [HttpGet]
        public ActionResult RouteByOperatingCenterIdAndOrTownId(int operatingCenterId, int? townId)
        {
            var results =
                new CascadingActionResult(
                    Repository.RouteByOperatingCenterIdAndOrTownId(operatingCenterId, townId)
                              .Select(x => new { value = x, text = x }),
                    "value",
                    "text");
            return results;
        }

        [HttpGet]
        public ActionResult ActiveByTownId(int townId)
        {
            return
                new CascadingActionResult<Hydrant, HydrantDisplayItem>(
                    Repository.FindActiveByTownId(townId)) {
                    SortItemsByTextField = false
                };
        }

        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return
                new CascadingActionResult(Repository.FindByTownId(townId), "HydrantNumber", "Id") {
                    SortItemsByTextField = false
                };
        }

        [HttpGet]
        public ActionResult ByOperatingCenter(params int[] id)
        {
            return new CascadingActionResult<Hydrant, HydrantDisplayItem>(
                Repository.GetHydrantsByOperatingCenter(id));
        }

        [HttpGet]
        public ActionResult ByStreetId(int streetId)
        {
            return new CascadingActionResult(Repository.FindByStreetId(streetId), "HydrantNumber", "Id") {
                SortItemsByTextField = false
            };
        }

        [HttpGet]
        public ActionResult ByStreetIdForWorkOrders(int streetId)
        {
            return new CascadingActionResult<Hydrant, HydrantDisplayItem>(
                Repository.FindByStreetIdForWorkOrders(streetId)) {
                SortItemsByTextField = false
            };
        }

        #endregion

        #region Constructors

        public HydrantController(
            ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args)
            : base(args) { }

        #endregion
    }
}
