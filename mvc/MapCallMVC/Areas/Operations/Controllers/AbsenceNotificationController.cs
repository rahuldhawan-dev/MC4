using System;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Operations.Controllers
{
    public class AbsenceNotificationController : ControllerBaseWithPersistence<IAbsenceNotificationRepository, AbsenceNotification, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsManagement;

        public const string 
            NOT_FOUND = "A notification for absence notification #{0} can not be sent because it either does not exist or you do not have permission to access that absence notification.",
            NOTIFICATION_PURPOSE_SUPERVISOR = "Absence Notification Supervisor",
            NOTIFICATION_PURPOSE_ENTRY = "Absence Notification Entry",
            ABSENCE_NOTIFICATION_SUBJECT = "Direct report absence notification",
            ABSENCE_NOTIFICATION_SUPERVISOR_ERROR = "The employee's supervisor is not set or the employee's supervisor does not have an email address assigned.",
            SENT_SUCCESSFULLY = "Notification sent successfully!";
        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;

                case ControllerAction.Edit:
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }
        
        private void SendAbsenceNotificationToSupervisor(AbsenceNotification entity)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                Module = RoleModules.OperationsManagement,
                Purpose = NOTIFICATION_PURPOSE_SUPERVISOR,
                Data = entity,
                Subject = ABSENCE_NOTIFICATION_SUBJECT,
                Address = entity.Employee.ReportsTo.EmailAddress
            };
            notifier.Notify(args);
        }

        private void SendCreationsMostBodaciousNotification(AbsenceNotification entity)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = entity.Employee.OperatingCenter.Id,
                Module = RoleModules.OperationsManagement,
                Purpose = NOTIFICATION_PURPOSE_ENTRY,
                Subject = ABSENCE_NOTIFICATION_SUBJECT,
                Data = entity
            };
            notifier.Notify(args);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchAbsenceNotification search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchAbsenceNotification search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateAbsenceNotification(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateAbsenceNotification model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationsMostBodaciousNotification(Repository.Find(model.Id));
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }
         
        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditAbsenceNotification>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditAbsenceNotification model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Other

        [RequiresRole(ROLE)] // Shouldn't this have an HttpPost on it?
        public ActionResult SendNotification(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
            {
                DisplayErrorMessage(string.Format(NOT_FOUND, id));
                return RedirectToAction("Search");
            }
            if (entity.Employee.ReportsTo == null || String.IsNullOrWhiteSpace(entity.Employee.ReportsTo.EmailAddress))
            {
                DisplayErrorMessage(ABSENCE_NOTIFICATION_SUPERVISOR_ERROR);
                return RedirectToAction("Show", new { id = id });
            }

            SendAbsenceNotificationToSupervisor(entity);
            DisplaySuccessMessage(SENT_SUCCESSFULLY);

            return DoRedirectionToAction("Show", new { id = id });
        }

        #endregion

        #region Constructors

        public AbsenceNotificationController(ControllerBaseWithPersistenceArguments<IAbsenceNotificationRepository, AbsenceNotification, User> args) : base(args) { }       

        #endregion
    }
}