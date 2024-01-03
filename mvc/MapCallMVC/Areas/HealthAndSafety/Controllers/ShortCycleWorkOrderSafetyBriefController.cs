using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class ShortCycleWorkOrderSafetyBriefController : ControllerBaseWithPersistence<ShortCycleWorkOrderSafetyBrief, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesShortCycle;
        public const string NOTIFICATION_PURPOSE = "Short Cycle Safety Brief";
        
        #endregion

        #region Constructor
        public ShortCycleWorkOrderSafetyBriefController(ControllerBaseWithPersistenceArguments<IRepository<ShortCycleWorkOrderSafetyBrief>, ShortCycleWorkOrderSafetyBrief, User> args) : base(args) { }
        
        #endregion

        #region Private Methods

        private void SendEmailToFSRSupervisorMaybe(ShortCycleWorkOrderSafetyBrief entity)
        {
            if (!entity.HasCompletedDailyStretchingRoutine || !entity.HasPerformedInspectionOnVehicle || !entity.IsPPEInGoodCondition)
            {
                var supervisor = AuthenticationService.CurrentUser.Employee.ReportsTo;
                var notifier = _container.GetInstance<INotificationService>();
                var templateModel = new ShortCycleWorkOrderSafetyBriefNotificationViewModel {
                    Entity = entity,
                    RecordUrl = GetUrlForModel(entity, "Show", nameof(ShortCycleWorkOrderSafetyBrief), "HealthAndSafety")
                };
                var args = new NotifierArgs {

                    Address = supervisor.EmailAddress,
                    Module = RoleModules.OperationsHealthAndSafety,
                    Purpose = NOTIFICATION_PURPOSE, // comes back for these once questions answered for notification content
                    Data = templateModel
                };

                notifier.Notify(args);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchShortCycleWorkOrderSafetyBrief search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchShortCycleWorkOrderSafetyBrief search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        e.FSR,
                        e.DateCompleted,
                        e.IsPPEInGoodCondition,
                        e.HasCompletedDailyStretchingRoutine,
                        e.HasPerformedInspectionOnVehicle,
                        LocationTypes = e.LocationTypes.ToString(),
                        HazardTypes = e.HazardTypes.ToString(),
                        PPETypes = e.PPETypes.ToString(),
                        ToolTypes = e.ToolTypes.ToString()
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            var model = ViewModelFactory.Build<CreateShortCycleWorkOrderSafetyBrief>();
            model.FSR = AuthenticationService.CurrentUser.Employee?.Id;
            model.DateCompleted = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            model.FSRName = AuthenticationService.CurrentUser.Employee?.FullName;
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateShortCycleWorkOrderSafetyBrief model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendEmailToFSRSupervisorMaybe(entity);
                    return null;
                }
            });
        }

        #endregion
    }
}