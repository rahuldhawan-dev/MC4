using System.Collections.Generic;
using System.Linq;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility;
using MapCall.SAP.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MapCall.SAP.Model.Entities;
using MMSINC.Results;
using SewerOpening = MapCall.Common.Model.Entities.SewerOpening;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    // cascades, no global role
    public class SewerOpeningController : SapSyncronizedControllerBaseWithPersisence<ISewerOpeningRepository, SewerOpening, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;
        public const string NOTIFICATION_PURPOSE = "SewerOpening";
        public const int MAX_AUTOCOMPLETE_RESULTS = 20;

        #endregion

        #region Private Methods

        /// <summary>
        /// This exists solely for the unit tests for this controller. These tests are doing validation testing
        /// rather than view model tests. They need to be rewritten.
        /// </summary>
        /// <param name="model"></param>
        internal void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }

        private void GenerateArcCollectorLink(SewerOpening opening)
        {
            var assetType = _container.GetInstance<IRepository<AssetType>>().Find(AssetType.Indices.SEWER_OPENING);
            ViewData["ArcCollectorLink"] = ArcCollectorLinkGenerator.ArcCollectorSewerOpeningHtmlString(opening, assetType);
        }

        protected override void UpdateEntityForSap(SewerOpening entity)
        {
            var equipment = new SAPEquipment(entity);
            var sapEquipment = _container.GetInstance<ISAPEquipmentRepository>().Save(equipment);
            if (!string.IsNullOrWhiteSpace(sapEquipment.SAPEquipmentNumber))
                entity.SAPEquipmentId = int.Parse(sapEquipment.SAPEquipmentNumber);
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }

        private void SendNotification(SewerOpening opening)
        {
            var templateModel = new SewerOpeningNotification();
            templateModel.SewerOpening = opening;
            templateModel.UserEmail = AuthenticationService.CurrentUser.Email;
            templateModel.UserName = AuthenticationService.CurrentUser.UserName;
            templateModel.RecordUrl = GetUrlForModel(opening, "Show", "SewerOpening", "FieldOperations");

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = opening.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = templateModel
            };
            notifier.Notify(args);
        }

        private MapResult GetMapResult(SearchSewerOpeningForMap search)
        {
            var result = _container.GetInstance<AssetMapResult>();

            if (!ModelState.IsValid)
            {
                return result;
            }

            if (Repository.GetCountForSearchSet(search) > SearchSewerOpeningForMap.MAX_MAP_RESULT_COUNT)
            {
                return null;
            }

            var searchResult = Repository.SearchForMap(search);

            result.Initialize(searchResult);
            
            return result;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, "OperatingCenter", x => x.IsActive);
                    break;
                case ControllerAction.Show:
                    this.AddDropDownData<PipeMaterial>("SewerPipeMaterial");
                    this.AddDropDownData<SewerTerminationType>();
                    this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
                    break;
            }
            this.AddDropDownData<SewerOpeningMaterial>();
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchSewerOpening search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id, onModelFound: entity => {
                    GenerateArcCollectorLink(entity);
                    DisplaySapErrorIfApplicable(entity);
                    this.AddDropDownData<ISewerOpeningRepository, SewerOpening>("ConnectedOpening", 
                        r => r.FindByTownId(entity.Town.Id), 
                        sm => sm.Id,
                        sm => sm.OpeningNumber);
                }));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    IsPartial = true, 
                    ViewName = "_ShowPopup"
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return _container.With((IEnumerable<IThingWithCoordinate>)new [] {model}).GetInstance<MapResultWithCoordinates>();
                });
            });
        }
        
        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchSewerOpening search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => {
                    return GetMapResult(search);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(_viewModelFactory.Build<CreateSewerOpening>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSewerOpening model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
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
            var opening = Repository.Find(id);
            if (opening != null)
            {
                var statusesThatRequireNotification = new int[] { AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, AssetStatus.Indices.RETIRED };
                if (opening.Status != null && statusesThatRequireNotification.Contains(opening.Status.Id))
                {
                    this.DisplayNotification("This asset is currently cancelled, removed, or retired. Any updates made to this record will not be saved to SAP.");
                }
            }

            return ActionHelper.DoEdit<EditSewerOpening>(id);
        }

        private void OnEditSewerOpeningUpdateSuccess(EditSewerOpening model)
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
        public ActionResult Update(EditSewerOpening model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    OnEditSewerOpeningUpdateSuccess(model);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Copy(int id)
        {
            var opening = Repository.Find(id);
            if (opening == null || (!opening.IsActive && !opening.CanBeCopied))
            {
                return DoHttpNotFound($"Could not find sewer opening with id {id}.");
            }

            var mh = ViewModelFactory.Build<CopySewerOpening, SewerOpening>(opening);
            return ActionHelper.DoCreate(mh, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (mh.SendNotificationOnSave)
                    {
                        SendNotification(Repository.Find(mh.Id));
                    }
                    return RedirectToAction("Edit", new { id = mh.Id });
                }
            });
    }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Replace(int id)
        {
            var opening = Repository.Find(id);
            if (opening == null || !opening.IsActive)
            {
                return DoHttpNotFound($"Could not find sewer opening with id {id}.");
            }

            // NOTE: If the edit saves fine, but the replace throws an exception, you end up with an irreplacable hydrant.

            // pending retire old hydrant
            var editOpening = ViewModelFactory.Build<EditSewerOpening, SewerOpening>(opening);
            editOpening.Status = _container.GetInstance<IAssetStatusRepository>().GetRequestRetirementStatus().Id;
            return ActionHelper.DoUpdate(editOpening, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    OnEditSewerOpeningUpdateSuccess(editOpening);
                    // NOTE: We're doing the replacement part inside the DoUpdate success call so that we know 
                    //       we're only creating a replacement when the update of the existing hydrant was successful.

                    // create the new hydrant record, also copies the inspections on the MapCall end. It was specifically stated that inspections
                    // are not copied to SAP http://bugzilla.mapcall.info/bugzilla/show_bug.cgi?id=3283#c9
                    var replaceOpening = GetReplaceSewerOpeningModel();
                    replaceOpening.Map(opening);

                    return ActionHelper.DoCreate(replaceOpening, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                        OnSuccess = () => {
                            if (replaceOpening.SendToSAP)
                            {
                                UpdateSAP(replaceOpening.Id, ROLE);
                            }

                            return RedirectToAction("Show", new { id = replaceOpening.Id });
                        }
                    });
                }
            });
        }

        // This is for testing purposes only.
        protected virtual ReplaceSewerOpening GetReplaceSewerOpeningModel()
        {
            return ViewModelFactory.Build<ReplaceSewerOpening>();
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region AddSewerOpeningConnections

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddSewerOpeningSewerOpeningConnection(AddSewerOpeningConnection model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnError = () => RedirectToAction("Show", new { model.Id })
            });
        }
        
        #endregion

        #region RemoveSewerOpeningConnections

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveSewerOpeningSewerOpeningConnection(RemoveSewerOpeningSewerOpeningConnection model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Public Methods

        #region RouteByTownId

        [HttpGet]
        public ActionResult RouteByTownId(int townId)
        {
            return new CascadingActionResult(Repository.RouteByTownId(townId).Select(x => new { value = x, text = x }),
                "value", "text");
        }

        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return new CascadingActionResult(Repository.FindByTownId(townId), "OpeningNumber", "Id") {
                SortItemsByTextField = false
            };
        }

        [HttpGet]
        public ActionResult ActiveByTownId(int townId)
        {
            return new CascadingActionResult(Repository.FindActiveByTownId(townId), "OpeningNumber", "Id")
            {
                SortItemsByTextField = false
            };
        }

        [HttpGet]
        public ActionResult ByStreetId(int streetId)
        {
            return new CascadingActionResult(Repository.ByStreetId(streetId), "OpeningNumber", "Id") {
                SortItemsByTextField = false
            };
        }

        [HttpGet]
        public ActionResult ByStreetIdForWorkOrders(int streetId)
        {
            return new CascadingActionResult<SewerOpening,SewerOpeningDisplayItem>(Repository.ByStreetIdForWorkOrders(streetId)) {
                SortItemsByTextField = false
            };
        }

        #endregion

        #region ByPartiaSewerOpeningMatch

        [HttpGet]
        public ActionResult ByPartialSewerOpeningMatchByTown(string partial, int townId)
        {
            var results = Repository.FindByPartialOpeningMatchByTown(partial, townId).Take(MAX_AUTOCOMPLETE_RESULTS).ToList();
            return new AutoCompleteResult(results, "Id", "OpeningNumber", "OpeningNumber");
        }

        #endregion

        #endregion

        #region Constructors

        public SewerOpeningController(ControllerBaseWithPersistenceArguments<ISewerOpeningRepository, SewerOpening, User> args) : base(args) { }

		#endregion
    }
}