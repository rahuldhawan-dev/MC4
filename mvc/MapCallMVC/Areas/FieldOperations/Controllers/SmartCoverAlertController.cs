using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MapCall.Common.ClassExtensions;
using System.Collections.Generic;
using MapCall.Common.Configuration;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SmartCoverAlertController : ControllerBaseWithPersistence<SmartCoverAlert, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        public const string SMART_COVER_ALERT_ACKNOWLEDGED = "Smart Cover Alert Acknowledged";

        #endregion

        #region Private Methods

        private void SendAcknowledgedNotification(SmartCoverAlert entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "SmartCoverAlert", "FieldOperations");
            if (entity.SewerOpening != null)
            {
                entity.SewerOpening.RecordUrl = GetUrlForModel(entity.SewerOpening, "Show", "SewerOpening", "FieldOperations");
                _container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                    OperatingCenterId = entity.SewerOpening.OperatingCenter.Id,
                    Module = ROLE,
                    Purpose = SMART_COVER_ALERT_ACKNOWLEDGED,
                    Data = entity
                });
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<SmartCoverAlertAlarmType>("HighLevelAlarmType",
                        _ => _container.GetInstance<IRepository<SmartCoverAlertAlarmType>>().GetAll().AsQueryable(), t => t.Id,
                        t => t.Description);
                    this.AddDropDownData<SmartCoverAlertType>("AlertType",
                        _ => _container.GetInstance<IRepository<SmartCoverAlertType>>().GetAll().AsQueryable(), t => t.Id,
                        t => t.Description);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchSmartCoverAlert search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSmartCoverAlert search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => {
                    search.EnablePaging = false;
                    return ActionHelper.DoIndex(search);
                });
                formatter.Fragment(() => ActionHelper.DoIndex(search,
                    new ActionHelperDoIndexArgs {
                        IsPartial = true,
                        ViewName = "_SmartCoverAlerts",
                        OnNoResults = () => PartialView("_NoResults")
                    }));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        e.AlertId,
                        State = e.SewerOpening?.State,
                        OperatingCenter = e.SewerOpening?.OperatingCenter,
                        Town = e.SewerOpening?.Town,
                        TownSection = e.SewerOpening?.TownSection,
                        e.SewerOpeningNumber,
                        e.Latitude,
                        e.Longitude,
                        e.Elevation,
                        e.SensorToBottom,
                        e.ManholeDepth,
                        e.DateReceived,
                        e.Acknowledged,
                        e.AcknowledgedOn,
                        e.AcknowledgedBy,
                        e.PowerPackVoltage,
                        e.WaterLevelAboveBottom,
                        e.Temperature,
                        e.SignalStrength,
                        e.SignalQuality,
                        e.HighAlarmThreshold,
                        SewerOpeningId = e.SewerOpening?.Id,
                        e.ApplicationDescription,
                        Notes = e.SmartCoverAlertAlarms.ToString()
                    });
                    return this.Excel(results);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id));
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
                formatter.Map(() =>
                {
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }
                    return _container.With((IEnumerable<IThingWithCoordinate>)new[] { model }).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        #endregion

        #region Edit/Update

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSmartCoverAlert model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.Acknowledged)
                    {
                        SendAcknowledgedNotification(Repository.Find(model.Id));
                    }
                    if (string.IsNullOrEmpty(model.IndexSearch))
                    {
                        return RedirectToAction("Show", new { id = model.Id });
                    }
                    return RedirectToAction("Index", model.IndexSearch?.ToRouteValues());
                }
            });
        }
        
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit), ActionBarVisible(false)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSmartCoverAlert>(id);
        }

        #endregion

        #region Constructors

        public SmartCoverAlertController(ControllerBaseWithPersistenceArguments<IRepository<SmartCoverAlert>, SmartCoverAlert, User> args) : base(args) { }

        #endregion
    }
}
