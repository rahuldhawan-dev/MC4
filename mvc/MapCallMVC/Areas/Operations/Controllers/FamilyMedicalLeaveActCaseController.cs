using System;
using System.ComponentModel;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Operations.Controllers
{
    [DisplayName("FMLA Case")]
    public class FamilyMedicalLeaveActCaseController : ControllerBaseWithPersistence<IFamilyMedicalLeaveActCaseRepository,FamilyMedicalLeaveActCase, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsManagement;

        public const string 
            NOTIFICATION_PURPOSE = "FMLA Case",
            NOTIFICATION_SUBJECT = "FMLA Case",
            NOT_FOUND = "A notification for FMLA Case #{0} can not be sent because it either does not exist or you do not have permission to access that FMLA Case.",
            SUPERVISOR_ERROR = "The employee's supervisor is not set or the employee's supervisor does not have an email address assigned.",
            SENT_SUCCESSFULLY = "Notification sent successfully!";

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddOperatingCenterDropDownData();
            this.AddDropDownData<CompanyAbsenceCertification>();
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Employee");
                    break;
            }
        }

        private void SendFMLACaseToSupervisor(FamilyMedicalLeaveActCase entity)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = entity,
                Subject = NOTIFICATION_SUBJECT,
                Address = entity.Employee.ReportsTo.EmailAddress
            };
            notifier.Notify(args);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchFamilyMedicalLeaveActCase search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFamilyMedicalLeaveActCase search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateFamilyMedicalLeaveActCase model)
        {
            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateFamilyMedicalLeaveActCase model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditFamilyMedicalLeaveActCase>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditFamilyMedicalLeaveActCase model)
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

        [HttpGet]
        public CascadingActionResult GetByEmployeeId(int id)
        {
            return new CascadingActionResult(Repository.GetByEmployeeId(id), "Description", "Id");
        }

        [RequiresRole(ROLE)]
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
                DisplayErrorMessage(SUPERVISOR_ERROR);
                return RedirectToAction("Show", new { id = id });
            }

            SendFMLACaseToSupervisor(entity);
            DisplaySuccessMessage(SENT_SUCCESSFULLY);

            return DoRedirectionToAction("Show", new { id = id });
        }

        #endregion

		#region Constructors

        public FamilyMedicalLeaveActCaseController(ControllerBaseWithPersistenceArguments<IFamilyMedicalLeaveActCaseRepository, FamilyMedicalLeaveActCase, User> args) : base(args) { }

		#endregion
    }
}