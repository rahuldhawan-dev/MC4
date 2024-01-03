using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class JobObservationController : ControllerBaseWithPersistence<IRepository<JobObservation>, JobObservation, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;
        public const string NOTIFICATION_PURPOSE = "Job Observation";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(JobObservation model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "JobObservation", "HealthAndSafety");

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = model
            };

            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            Action allActions = () => {
                this.AddDropDownData<JobCategory>();
                this.AddDropDownData<OverallQualityRating>();
                this.AddDropDownData<OverallSafetyRating>();
            };
            switch (action)
            {
                case ControllerAction.New:
                    allActions();
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;
                case ControllerAction.Edit:
                    allActions();
                    break;
                case ControllerAction.Search:
                    allActions();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchJobObservation>();
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
                    var model = Repository.Find(id);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    return _container.With((IEnumerable<IThingWithCoordinate>)new [] {model}).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchJobObservation search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id = null, bool production = false)
        {
            // NOTE: id is not a required field. Links with an id come from
            //       a work order in 271.
            var model = new CreateJobObservation(_container);
            if (!production && id != null)
                model.WorkOrder = id;
            if (production && id != null)
                model.ProductionWorkOrder = id;
            model.CoordinateCreateUrl = Url.Action("Create", "Coordinate", new { area = "" });
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateJobObservation model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendCreationsMostBodaciousNotification(entity);
                    return null;
                },
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditJobObservation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditJobObservation model)
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
        public JobObservationController(ControllerBaseWithPersistenceArguments<IRepository<JobObservation>, JobObservation, User> args) : base(args) {}
    }
}
