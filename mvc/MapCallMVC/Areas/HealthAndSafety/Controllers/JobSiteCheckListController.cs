using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class JobSiteCheckListController : ControllerBaseWithPersistence<IJobSiteCheckListRepository, JobSiteCheckList, User>
    {
        #region Constants

        // NOTE: This needs to be a role that 271 users will have access to.
        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesWorkManagement;
        public const string NOTIFICATION_PURPOSE = "Job Site Check List";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(JobSiteCheckList model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "JobSiteCheckList", "HealthAndSafety");
            if (model.MapCallWorkOrder != null)
                model.MapCallWorkOrder.RecordUrl = GetUrlForModel(model.MapCallWorkOrder, "Show", "WorkOrder","FieldOperations");
            
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE_MODULE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = model
            };

            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE_MODULE, RoleActions.Read));
                    break;
                case ControllerAction.New:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE_MODULE, RoleActions.Add));
                    break;
                case ControllerAction.Edit:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE_MODULE, RoleActions.Edit));
                    break;
            }

            this.AddDropDownData<IRepository<JobSiteExcavationProtectionType>, JobSiteExcavationProtectionType>(
                "ProtectionTypes", r => r.GetAllSorted(), t => t.Id, t => t.Description);
            this.AddDropDownData<IRepository<JobSiteExcavationLocationType>, JobSiteExcavationLocationType>(
                "editorModel.LocationType", r => r.GetAllSorted(), t => t.Id, t => t.Description);
            this.AddDropDownData<IRepository<JobSiteExcavationSoilType>, JobSiteExcavationSoilType>(
                "editorModel.SoilType", r => r.GetAllSorted(), t => t.Id, t => t.Description);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchJobSiteCheckList model)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, model));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search(SearchJobSiteCheckList model)
        {
            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x =>
            {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
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
                x.Pdf(() =>
                {
                    // This needs similar lookup data, but it needs the actual entity
                    // and not the dropdown version.

                    ViewData["LocationTypes"] =
                        _container.GetInstance<IRepository<JobSiteExcavationLocationType>>().GetAll();
                    ViewData["SoilTypes"] =
                        _container.GetInstance<IRepository<JobSiteExcavationSoilType>>().GetAll();
                    ViewData["ProtectionTypes"] =
                        _container.GetInstance<IRepository<JobSiteExcavationProtectionType>>().GetAll();
                        
                    return ActionHelper.DoPdf(id);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int? workOrderId = null)
        {
            var model = ViewModelFactory.Build<CreateJobSiteCheckList>();
            model.MapCallWorkOrder = workOrderId;
            return ActionHelper.DoNew(model);
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreateJobSiteCheckList model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    return RedirectToAction("Edit", "WorkOrderFinalization",
                        new { area = "FieldOperations", id = entity.MapCallWorkOrder.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            if (!_container.GetInstance<IAuthenticationService>().CurrentUserIsAdmin)
            {
                var entity = Repository.Find(id);
                if (entity != null && entity.IsApprovedBySupervisor)
                {
                    DisplayNotification("Checklists can not be edited after it has been approved by a supervisor.");
                    return DoRedirectionToAction("Show", new { id });
                }
            }

            return ActionHelper.DoEdit<EditJobSiteCheckList>(id);
        }

        [HttpPost]
        [RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditJobSiteCheckList model)
        {
            var resendChecklistNotification = false;
            var entity = Repository.Find(model.Id);

            if (model.HasExcavation.HasValue && entity != null) 
            {
                resendChecklistNotification = entity.HasExcavation != model.HasExcavation;
            }
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs
            {
                OnSuccess = () =>
                {
                    if (resendChecklistNotification)
                    {
                        SendCreationsMostBodaciousNotification(entity);
                    }
                    return null; // defer to default 
                }
            });
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

        public JobSiteCheckListController(ControllerBaseWithPersistenceArguments<IJobSiteCheckListRepository, JobSiteCheckList, User> args) : base(args) {}
    }
}