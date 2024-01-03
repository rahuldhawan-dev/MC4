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
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class BelowGroundHazardController : ControllerBaseWithPersistence<BelowGroundHazard, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;
        public const string NOTIFICATION_PURPOSE = "Below Ground Hazard Created";

        #endregion

        #region Constructors

        public BelowGroundHazardController(ControllerBaseWithPersistenceArguments<IRepository<BelowGroundHazard>, BelowGroundHazard, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private MapResult GetMapResult(SearchBelowGroundHazard search)
        {
            var result = _container.GetInstance<AssetMapResult>();

            if (ModelState.IsValid)
            {
                search.EnablePaging = false;
                var hazardResult = Repository.Search(search);
                var hazardCoords = hazardResult.Select(x => x.ToAssetCoordinate()).ToList();
                if (search.EntityId.HasValue)
                {
                    result.Initialize(hazardCoords);
                }
                else
                {
                    var hazardSearchRvd = new RouteValueDictionary {
                        [ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatterExtensions.MAP_ROUTE_EXTENSION
                    };

                    foreach (var ms in ModelState)
                    {
                        hazardSearchRvd[ms.Key] = ms.Value.Value.AttemptedValue;
                    }
                    result.RelatedAssetsUrl = Url.Action("Index", "BelowGroundHazard", hazardSearchRvd);
                    result.Initialize(hazardCoords);
                }
            }

            return result;
        }

        private void SendNotification(BelowGroundHazard belowGroundHazard)
        {
            var templateModel = new BelowGroundHazardNotification {
                BelowGroundHazard = belowGroundHazard,
                CreatedBy = AuthenticationService.CurrentUser.UserName,
                CreatedOn = DateTime.Now,
                RecordUrl = GetUrlForModel(belowGroundHazard, "Show", "BelowGroundHazard", "Facilities")
            };

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = belowGroundHazard.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = templateModel
            };
            notifier.Notify(args);
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    break;

                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchBelowGroundHazard search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchBelowGroundHazard search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => GetMapResult(search));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                x.Map(() => {
                    var search = new SearchBelowGroundHazard { EntityId = id };
                    return GetMapResult(search);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id = null, int? asset = null)
        {
            // NOTE: id is not a required field. Links with an id come from
            //       a work order in 271.
            var model = new CreateBelowGroundHazard(_container);
            if (asset == BelowGroundHazard.Indices.WORKORDER && id != null)
            {
                model.WorkOrder = id;
                model.CoordinateCreateUrl = Url.Action("Create", "Coordinate", new { area = "" });
            }
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBelowGroundHazard model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendNotification(Repository.Find(model.Id));
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBelowGroundHazard>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBelowGroundHazard model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #endregion
    }
}