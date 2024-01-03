using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MarkoutDamageController : ControllerBaseWithPersistence<IMarkoutDamageRepository, MarkoutDamage, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesWorkManagement;
        public const string NOTIFICATION_PURPOSE = "Markout Damage",
                            SIF_OR_SIFP_EVENT_NOTIFICATION_PURPOSE = "Markout Damage SIF Or SIFP Event";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(MarkoutDamage model, string notificationPurpose)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "MarkoutDamage", "FieldOperations");

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE_MODULE,
                Purpose = notificationPurpose,
                Data = model
            };

            notifier.Notify(args);
        }

        private int GetOrCreateCoordinate(Coordinate woCoordinate)
        {
            var repo = _container.GetInstance<IRepository<Coordinate>>();
            var coordinate = repo.FindByValues(woCoordinate.Latitude, woCoordinate.Longitude, woCoordinate.Icon?.Id ?? 0); // work orders don't have a real connection to coordinates yet.

            if (coordinate == null)
            {
                coordinate = repo
                   .Save(new Coordinate
                    {
                        Latitude = woCoordinate.Latitude,
                        Longitude = woCoordinate.Longitude,
                        Icon = woCoordinate.Icon
                    });
            }

            return coordinate.Id;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action addStates = () => {
                var states = _container.GetInstance<IStateRepository>().GetAllSorted(x => x.Abbreviation).ToArray();
                this.AddDropDownData("State", states, state => state.Id, state => state.ToString());
            };

            Action addMarkoutDamageToTypes = () => {
                var types =
                    _container.GetInstance<IRepository<MarkoutDamageToType>>().GetAllSorted();
                this.AddDropDownData("MarkoutDamageToType", types, x => x.Id, x => x.Description);
            };

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE_MODULE, RoleActions.Add, extraFilterP: x => x.IsActive);
                   // addStates();
                    addMarkoutDamageToTypes();
                    break;
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE_MODULE, RoleActions.Edit);
                   // addStates();
                    addMarkoutDamageToTypes();
                    break;
                case ControllerAction.Search:
                    addStates();
                    addMarkoutDamageToTypes();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchMarkoutDamage model)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Excel(() => ActionHelper.DoExcel(model));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, model));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search(SearchMarkoutDamage model)
        {
            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x =>
            {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                x.Map(() =>
                {
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }
                    return _container.With((IEnumerable<IThingWithCoordinate>)new [] {model}).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            var model = new CreateMarkoutDamage(_container);
            if (id.HasValue)
            {
                PopulateFromWorkOrder(id.Value, model);
            }
            return ActionHelper.DoNew(model);
        }

        private void PopulateFromWorkOrder(int id, CreateMarkoutDamage model)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(id);
            if (workOrder != null)
            {
                model.WorkOrder = id;
                model.OperatingCenter = workOrder.OperatingCenter.Id;
                model.State = workOrder.OperatingCenter.State.Id;
                model.County = workOrder.Town?.County?.Id;
                model.Town = workOrder.Town?.Id;
                if (workOrder.Coordinate != null)
                {
                    model.Coordinate = GetOrCreateCoordinate(workOrder.Coordinate);
                }
            }
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreateMarkoutDamage model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendCreationsMostBodaciousNotification(entity, NOTIFICATION_PURPOSE);
                    var sifpDamageTypes = new[] {
                        MarkoutDamageUtilityDamageType.Indices.ELECTRIC,
                        MarkoutDamageUtilityDamageType.Indices.GAS
                    };
                    if (entity.UtilityDamages.Select(x => x.Id).Intersect(sifpDamageTypes).Any())
                    {
                        SendCreationsMostBodaciousNotification(entity, SIF_OR_SIFP_EVENT_NOTIFICATION_PURPOSE);
                    }

                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMarkoutDamage>(id);
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditMarkoutDamage model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        [RequiresRole(ROLE_MODULE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public MarkoutDamageController(ControllerBaseWithPersistenceArguments<IMarkoutDamageRepository, MarkoutDamage, User> args) : base(args) {}
    }
}