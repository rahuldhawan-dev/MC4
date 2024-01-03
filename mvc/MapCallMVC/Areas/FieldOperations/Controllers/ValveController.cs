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
    public class ValveController : SapSyncronizedControllerBaseWithPersisence<IValveRepository, Valve, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;
        public const string NOTIFICATION_PURPOSE = "Valve";

        public const string CANNOT_COPY_ERROR =
                                "Valve cannot be copied, ensure it has a valve size, number of turns, " +
                                "and it has the correct status.";

        #endregion

        #region Private Methods
        
        private void GenerateArcCollectorLink(Valve valve)
        {
            var assetType = _container.GetInstance<IRepository<AssetType>>().Find(AssetType.Indices.VALVE);
            ViewData["ArcCollectorLink"] = ArcCollectorLinkGenerator
               .ArcCollectorValveHtmlString(valve, assetType);
        }

        private MapResult GetMapResult(SearchValve search)
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
                search.RequiresBlowOffInspection = null;
            }

            if (Repository.GetCountForSearchSet(search) > SearchValveForMap.MAX_MAP_RESULT_COUNT)
            {
                return null;
            }

            // Bug 2465: They want to see duplicate icons for valves that are also blowoffs, even though
            //           one will always cover the other.
            var valveResult = Repository.SearchForMap((SearchValveForMap)search);

            // if this is happening we don't want to concat with other assets.
            if (search.EntityId.HasValue)
            {
                result.Initialize(valveResult);
            }
            else
            {
                search.ValveControls = ValveControl.Indices.BLOW_OFF_WITH_FLUSHING;
                var blowOffResult = Repository.SearchBlowOffsForMap((SearchBlowOffForMap)search);
                var hydrantSearchRvd = new RouteValueDictionary {
                    [ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                        ResponseFormatterExtensions.MAP_ROUTE_EXTENSION
                };

                foreach (var ms in ModelState)
                {
                    hydrantSearchRvd[ms.Key] = ms.Value.Value.AttemptedValue;
                }
                hydrantSearchRvd["IsRelatedAssetSearch"] = true;
                result.RelatedAssetsUrl = Url.Action("Index", "Hydrant", hydrantSearchRvd);

                result.Initialize(blowOffResult, valveResult);

                // bug 2817: If a specific Route is searched for, display the line layer. 
                if (search.Route.HasValue)
                {
                    var coordsSorted = valveResult
                                      .OrderBy(x => x.Stop)
                                      .ToList();
                    result.InitializeWithLineLayer(coordsSorted);
                }
            }

            return result;
        }

        private void SendNotification(Valve model)
        {
            var templateModel = new ValveNotification();
            templateModel.Valve = model;
            templateModel.UserName = AuthenticationService.CurrentUser.UserName;
            templateModel.UserEmail = AuthenticationService.CurrentUser.Email;
            templateModel.RecordUrl = GetUrlForModel(model, "Show", "Valve", "FieldOperations");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = templateModel
            };

            notifier.Notify(args);
        }

        private void SetValveStatusLookupDataForEditAction(int entityId)
        {
            // Bug 2432: Non-admin users are only allowed to change the hydrant status
            //           to a few select values.

            var entity = Repository.Find(entityId);
            if (entity == null)
            {
                return; // Can't do anything and Edit will return a 404 anyway.
            }

            var statuses = _container.GetInstance<IAssetStatusRepository>().GetAll();
            var isUserAdmin = _container
                             .GetInstance<IRoleService>()
                             .CanAccessRole(ROLE, RoleActions.UserAdministrator, entity.OperatingCenter);

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

        protected override void UpdateEntityForSap(Valve entity)
        {
            var equipment = new SAPEquipment(entity);
            var sapEquipment = _container.GetInstance<ISAPEquipmentRepository>().Save(equipment);
            if (!string.IsNullOrWhiteSpace(sapEquipment.SAPEquipmentNumber))
                entity.SAPEquipmentId = int.Parse(sapEquipment.SAPEquipmentNumber);
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }

        private void ShowShowNotifications(Valve entity)
        {
            DisplaySapErrorIfApplicable(entity);
            if (!entity.CanBeCopied)
                DisplayNotification(CANNOT_COPY_ERROR);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchValve search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id, bool nopdf = false)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id, onModelFound: entity => {
                    GenerateArcCollectorLink(entity);
                    ShowShowNotifications(entity);
                }));
                x.Map(() => {
                    var search = new SearchValve { EntityId = id };
                    return GetMapResult(search);
                });
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
                x.Pdf(() => {
                    var model = Repository.Find(id);

                    if (model == null)
                        return HttpNotFound();

                    var viewPath = this.GetStateViewPath(model, "Pdf");
                    return nopdf
                        ? View(viewPath, model)
                        : new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), viewPath, model);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchValve search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Map(() => GetMapResult(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        // Bug 2432: Only UserAdmin roled users are allowed to create new hydrants.
        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New(CreateValve model)
        {
            return ActionHelper.DoNew(model);
        }

        // Bug 2432: Only UserAdmin roled users are allowed to create new hydrants.
        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateValve model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    if (model.RequiresNotification())
                    {
                        SendNotification(Repository.Find(model.Id));
                    }
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update/Replace/Copy

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditValve>(id, null, onModelFound: valve => {
                // TODO: Fix this so we aren't querying for the Valve again.
                SetValveStatusLookupDataForEditAction(id);

                var statusesThatRequireNotification = new[] {
                    AssetStatus.Indices.CANCELLED,
                    AssetStatus.Indices.REMOVED,
                    AssetStatus.Indices.RETIRED
                };
                if (statusesThatRequireNotification.Contains(valve.Status.Id))
                {
                    DisplayNotification(
                        "This asset is currently cancelled, removed, or retired. Any updates made to " +
                        "this record will not be saved to SAP.");
                }
            });
        }

        private void OnEditValveUpdateSuccess(EditValve model)
        {
            if (model.SendToSAP)
            {
                UpdateSAP(model.Id, ROLE);
            }
            if (model.SendNotificationsOnSave)
            {
                SendNotification(Repository.Find(model.Id));
            }
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditValve model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    OnEditValveUpdateSuccess(model);
                    return RedirectToAction("Show", new { id = model.Id });
                },
                OnError = () => {
                    SetValveStatusLookupDataForEditAction(model.Id);
                    return null;
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Replace(int id)
        {
            var valve = Repository.Find(id);
            if (valve == null || !valve.IsActive)
            {
                return DoHttpNotFound($"Could not find valve with id {id}.");
            }

            // NOTE: If the edit saves fine, but the replace throws an exception, you end up with an
            // irreplaceable hydrant.

            // pending retire old hydrant
            var editValve = ViewModelFactory.BuildWithOverrides<EditValve, Valve>(valve,
                new { Status = AssetStatus.Indices.REQUEST_RETIREMENT });

            return ActionHelper.DoUpdate(editValve, new ActionHelperDoUpdateArgs {
                OnError = () => {
                    SetValveStatusLookupDataForEditAction(editValve.Id);
                    return null;
                },
                OnSuccess = () => {
                    OnEditValveUpdateSuccess(editValve);
                    // NOTE: We're doing the replacement part inside the DoUpdate success call so that we
                    //       know we're only creating a replacement when the update of the existing hydrant
                    //       was successful.

                    // create the new valve record, inspections are *not* copied
                    // http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c6
                    var replaceValve = GetReplaceValveModel();
                    replaceValve.Map(valve);

                    return ActionHelper.DoCreate(replaceValve, new ActionHelperDoCreateArgs {
                        OnSuccess = () => {
                            if (replaceValve.SendToSAP)
                            {
                                UpdateSAP(replaceValve.Id, ROLE);
                            }
                            // NOTE: There is no notification sending here because creating new records
                            // does not send notifications for pending status valves.
                            return RedirectToAction("Show", new { id = replaceValve.Id });
                        }
                    });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Copy(int id)
        {
            // TODO: The CanBeCopied part should be a validation error, not a 404.
            var valve = Repository.Find(id);
            if (valve == null || !valve.CanBeCopied)
            {
                return DoHttpNotFound($"Could not find valve with id {id}.");
            }

            var valveCopy = ViewModelFactory.Build<CopyValve, Valve>(valve);
            return ActionHelper.DoCreate(valveCopy, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (valveCopy.SendNotificationsOnSave)
                    {
                        SendNotification(Repository.Find(valveCopy.Id));
                    }
                    return RedirectToAction("Edit", new { id = valveCopy.Id });
                }
            });
        }

        // This is for testing purposes only.
        protected virtual ReplaceValve GetReplaceValveModel()
        {
            return ViewModelFactory.Build<ReplaceValve>();
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action allActions = () => {
                this.AddDropDownData<MainType>();
                this.AddDropDownData<ValveType>();
                this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
                this.AddDropDownData<ValveNormalPosition>("NormalPosition");
                this.AddDropDownData<ValveOpenDirection>("OpenDirection");
                this.AddDropDownData<ValveControl>("ValveControls");
                this.AddDropDownData<ValveManufacturer>("ValveMake");
                this.AddDynamicDropDownData<ValveSize, ValveSizeDisplayItem>(
                    dataGetter: x => x.GetAllSorted(y => y.Size));
                this.AddDropDownData<ValveZone>();
            };

            switch (action)
            {
                case ControllerAction.New:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    allActions();
                    this.AddDropDownData<ValveBilling>();
                    this.AddDropDownData<AssetStatus>();
                    break;
                case ControllerAction.Edit:
                    if (_container.GetInstance<IRoleService>()
                                  .CanAccessRole(ROLE, RoleActions.UserAdministrator))
                    {
                        this.AddDropDownData<ValveBilling>();
                    }
                    // Do not add ValveStatus here. It needs to be done separately.
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    allActions();
                    break;
                case ControllerAction.Search:
                    allActions();
                    this.AddDropDownData<ValveBilling>();
                    this.AddDropDownData<AssetStatus>("Status");
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
            }
        }

        #region ByOperatingCenterAndValveNumber

        [HttpGet]
        public ActionResult ByOperatingCenterAndValveNumber(SearchOperatingCenterValveNumber model)
        {
            var result = new Dictionary<string, object> {
                ["success"] = false
            };

            if (!ModelState.IsValid)
            {
                result["message"] = "Invalid search parameters.";
            }
            else
            {
                var operatingCenter = _container
                                     .GetInstance<OperatingCenterRepository>()
                                     .Find(model.OperatingCenterIdentifier.GetValueOrDefault());
                var valves = Repository
                   .FindByOperatingCenterAndValveNumber(operatingCenter, model.ValveNumberSearch);
                // ReSharper disable PossibleMultipleEnumeration
                if (!valves.Any())
                {
                    result["message"] = "There are no valves that match the search parameters.";
                }
                else if (valves.Count() > 1)
                {
                    result["message"] =
                        "Unable to retrieve valve data because there are multiple valves matching the " +
                        "search parameters.";
                }
                else
                {
                    var valve = valves.Single();
                    result["success"] = true;
                    result["valveId"] = valve.Id;
                    result["valveNumber"] = valve.ValveNumber;
                    result["streetId"] = valve.Street != null ? valve.Street.Id : 0;
                    result["townId"] = valve.Town.Id;
                    result["operatingCenterId"] = valve.OperatingCenter.Id;
                    result["townSection"] = valve.TownSection != null
                        ? valve.TownSection.Description
                        : string.Empty;
                    result["streetNumber"] = valve.StreetNumber;
                    result["turns"] = valve.Turns;
                    result["valveSize"] = valve.ValveSize != null
                        ? valve.ValveSize.Size.ToString()
                        : string.Empty;
                    result["crossStreet"] = valve.CrossStreet != null
                        ? valve.CrossStreet.FullStName
                        : string.Empty;
                    result["location"] = valve.ValveLocation;

                    if (valve.DateInstalled.HasValue)
                    {
                        result["dateCompleted"] = string.Format(
                            CommonStringFormats.DATE,
                            valve.DateInstalled.Value);
                    }

                    if (valve.NormalPosition != null)
                    {
                        result["normalPosition"] = valve.NormalPosition.Id;
                    }

                    if (valve.OpenDirection != null)
                    {
                        result["openDirection"] = valve.OpenDirection.Id;
                    }

                    result["isDefaultImageForValve"] =
                        (!valve.ValveImages.Any(x => x.IsDefaultImageForValve)).ToString();
                }
                // ReSharper restore PossibleMultipleEnumeration
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ByStreetId

        [HttpGet]
        public ActionResult ByStreetId(int id)
        {
            return new CascadingActionResult(Repository.FindByStreetId(id), "ValveNumber", "Id") {
                SortItemsByTextField = false
            };
        }

        [HttpGet]
        public ActionResult ByStreetIdForWorkOrders(int id)
        {
            return new CascadingActionResult<Valve, ValveDisplayItem>(
                Repository.FindByStreetIdForWorkOrders(id)) {
                SortItemsByTextField = false
            };
        }

        #endregion

        [HttpGet]
        public ActionResult ByTownId(int id)
        {
            return new CascadingActionResult(Repository.FindByTownIdOther(id), "Description", "Id") {
                SortItemsByTextField = false
            };
        }

        [HttpGet]
        public ActionResult ByOperatingCenter(params int[] id)
        {
            return new CascadingActionResult<Valve, ValveDisplayItem>(
                Repository.GetValvesByOperatingCenter(id));
        }

        [HttpGet]
        public ActionResult ByTownIdAndOperatingCenterId(int townId, int ocId)
        {
            return new CascadingActionResult<Valve, ValveDisplayItem>(
                Repository.FindByTownIdAndOperatingCenterId(townId, ocId));
        }

        [HttpGet]
        public ActionResult RouteByOperatingCenterIdAndOrTownId(int operatingCenterId, int? townId)
        {
            var results =
                new CascadingActionResult(
                    Repository
                       .RouteByOperatingCenterIdAndOrTownId(operatingCenterId, townId)
                       .Select(x => new { value = x, text = x }),
                    "value",
                    "text");
            return results;
        }

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

        #region ValveSuffix

        [HttpGet, NoCache] // Remote validation must be a GET for some reason.
        public ActionResult ValidateUnusedFoundValveSuffix(
            int? valveSuffix,
            int? operatingCenter,
            int? town,
            int? townSection)
        {
            if (!valveSuffix.HasValue || !town.HasValue || !operatingCenter.HasValue)
            {
                // These are all required fields.
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var model = _viewModelFactory.BuildWithOverrides<CreateValve>(new {
                ValveSuffix = valveSuffix,
                OperatingCenter = operatingCenter,
                Town = town,
                TownSection = townSection,
                IsFoundValve = true
            });

            var result = model.ValidateValveSuffixForFoundValve().ToArray();

            if (!result.Any())
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(result.First().ErrorMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Constructors

        public ValveController(ControllerBaseWithPersistenceArguments<IValveRepository, Valve, User> args)
            : base(args) { }

        #endregion
    }
}
